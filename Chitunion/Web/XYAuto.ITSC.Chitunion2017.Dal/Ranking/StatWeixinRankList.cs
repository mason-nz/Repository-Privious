/********************************************************
*创建人：lixiong
*创建时间：2017/7/10 16:16:04
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Ranking;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.Ranking
{
    public class StatWeixinRankList : DataBase
    {
        #region Instance

        public static readonly StatWeixinRankList Instance = new StatWeixinRankList();

        #endregion Instance

        public Tuple<List<RespStatWeixinRankListDto>, RespStatWeixinRankItemDto> GetRankList(RankingQuery<RespStatWeixinRankListDto> query)
        {
            var sql = new StringBuilder();
            var parameters = new List<SqlParameter> { new SqlParameter("@CreateUserID", query.CreateUserId) };

            sql.AppendFormat(@"
                            CREATE TABLE #RankingList(
	                            [RecID] INT
                                  ,[WxNum] VARCHAR(100)
                                  ,[MaLiIndex] DECIMAL(18,2)
                                  ,[CreateTime] DATETIME
                                  ,[LastModifyTime] DATETIME
                                  ,[AvgTopArticleReadNum] INT
                                  ,[AvgTopArticleLikeNum] INT
                                  ,[MaxReadNum] INT
                                  ,[PublishArticleNum] INT
                                  ,[PublishCount] INT
                            )
                            INSERT INTO #RankingList
                                    ( RecID ,
                                      WxNum ,
                                      MaLiIndex ,
                                      CreateTime ,
                                      LastModifyTime ,
                                      AvgTopArticleReadNum ,
                                      AvgTopArticleLikeNum ,
                                      MaxReadNum ,
                                      PublishArticleNum ,
                                      PublishCount
                                    )
                            SELECT [RecID]
                                  ,[WxNum]
                                  ,[MaLiIndex]
                                  ,[CreateTime]
                                  ,[LastModifyTime]
                                  ,[AvgTopArticleReadNum]
                                  ,[AvgTopArticleLikeNum]
                                  ,[MaxReadNum]
                                  ,[PublishArticleNum]
                                  ,[PublishCount]
                              FROM [Chitunion2017].[dbo].[v_RankList]

                            --更新时间
                            SELECT TOP 1 [LastModifyTime] =(
                            ISNULL(#RankingList.[LastModifyTime],#RankingList.CreateTime)
                            )
                            FROM #RankingList ORDER BY LastModifyTime DESC
                            ");

            sql.AppendFormat(@"
                            SELECT  TOP {0} V_Ranking.* ,
                                    MediaId =  ISNULL(( SELECT  MW.MediaID
                                                FROM    dbo.Media_Weixin AS MW WITH ( NOLOCK )
                                                WHERE   MW.WxID = WO.RecID
                                                        AND MW.CreateUserID = @CreateUserID
                                              ) ,0),
                                    WO.RecID AS BaseMediaId ,
                                    WO.NickName AS Name ,
                                    WO.HeadImg AS HeadIconURL
                            FROM    #RankingList AS V_Ranking
                                    INNER JOIN dbo.Weixin_OAuth AS WO WITH ( NOLOCK ) ON WO.WxNumber = V_Ranking.WxNum AND WO.WxNumber <> ''
                                    INNER JOIN dbo.Media_Weixin AS MW ON MW.WxID = WO.RecID
                            WHERE   WO.Status = 0 AND WO.QrCodeUrl <> '' AND WO.HeadImg <> ''
                                    AND EXISTS(
			                            SELECT 1 FROM dbo.Publish_BasicInfo AS PB WITH ( NOLOCK ) WHERE PB.MediaID = MW.MediaID AND PB.MediaType = {1} AND PB.IsDel = 0
		                            )
                       ", query.PageSize, (int)MediaType.WeiXin);
            if (query.CategoryId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql.AppendFormat(@" AND EXISTS(
	                                SELECT 1 FROM DBO.MediaCategory AS MC WITH(NOLOCK)
	                                WHERE MC.CategoryID = @CategoryID
                                           AND MC.WxID = WO.RecID
                            )");
                parameters.Add(new SqlParameter("@CategoryID", query.CategoryId));
            }

            if (!string.IsNullOrWhiteSpace(query.KeyWord))
            {
                sql.AppendFormat(@"AND (WO.NickName LIKE '%{0}%' OR WO.WxNumber LIKE '%{0}%')", XYAuto.Utils.StringHelper.SqlFilter(query.KeyWord));
            }

            sql.AppendFormat(
                @" ORDER BY MaLiIndex DESC,AvgTopArticleReadNum DESC,AvgTopArticleLikeNum DESC,MaxReadNum DESC");

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql.ToString(), parameters.ToArray());
            return new Tuple<List<RespStatWeixinRankListDto>, RespStatWeixinRankItemDto>
                (DataTableToList<RespStatWeixinRankListDto>(data.Tables[1]),
                DataTableToEntity<RespStatWeixinRankItemDto>(data.Tables[0]));
        }

        public List<CommonlyClassDto> GetRankingCategoryList(int userId)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(@"
                        SELECT  DD.CategoryID ,
                                DI.DictName AS CategoryName
                        FROM    ( SELECT
                                            DISTINCT MC.CategoryID ,
                                            CiteCount = (  SELECT COUNT(1)
                                                        FROM  dbo.Media_Weixin AS MW WITH ( NOLOCK )
			                                            INNER JOIN DBO.Weixin_OAuth AS WO WITH(NOLOCK) ON WO.RecID = MW.WxID AND WO.Status = 0
                                                        INNER JOIN dbo.[v_RankList] AS V_Ranking ON WO.WxNumber = V_Ranking.WxNum AND WO.WxNumber <> ''
						                                            WHERE   1 = 1
								                                            AND MW.CreateUserID ={1}
								                                            AND MW.WxID = MC.WxID
								                                            --AND MW.Status = 0
								                                            AND EXISTS(
						                                            SELECT 1 FROM dbo.Publish_BasicInfo AS PB WITH ( NOLOCK ) WHERE PB.MediaID = MW.MediaID AND PB.MediaType = {0} AND PB.IsDel = 0
					                                            )
					                                            AND WO.QrCodeUrl <> ''
					                                            AND WO.HeadImg <> ''
                                                        )
                                    FROM      dbo.MediaCategory AS MC WITH ( NOLOCK )
			                        WHERE  MC.MediaType = {0} AND MC.WxID > 0
                                ) AS DD
                                LEFT JOIN dbo.DictInfo AS DI WITH ( NOLOCK ) ON DI.DictId = DD.CategoryID
		                        WHERE DD.CiteCount > 0
                        ORDER BY CiteCount DESC
                        ", (int)MediaType.WeiXin, userId);

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql.ToString());
            return DataTableToList<CommonlyClassDto>(data.Tables[0]);
        }

        public List<CommonlyClassDto> GetCategoryList(int mediaType)
        {
            var sql = new StringBuilder();

            switch (mediaType)
            {
                case (int)MediaType.WeiXin:
                    sql.AppendFormat(@"

                        SELECT  DD.CategoryID ,
                                DI.DictName AS CategoryName
                        FROM    ( 
                                    SELECT a.CategoryID,SUM(a.CiteCount) AS CiteCount FROM (
                                        SELECT
                                                DISTINCT MC.CategoryID ,
                                                CiteCount = (  SELECT COUNT(1)
                                                            FROM  dbo.Media_Weixin AS MW WITH ( NOLOCK )
			                                                INNER JOIN DBO.Weixin_OAuth AS WO WITH(NOLOCK) ON WO.RecID = MW.WxID AND WO.Status = 0
                                                            LEFT JOIN dbo.UserRole AS UR WITH ( NOLOCK ) ON UR.UserID = mw.CreateUserID
						                                                WHERE   1 = 1
								                                                AND MW.WxID = MC.WxID
								                                                --AND MW.Status = 0
								                                                AND EXISTS(
						                                                            SELECT 1 FROM dbo.Publish_BasicInfo AS PB WITH ( NOLOCK )
											                                        WHERE PB.MediaID = MW.MediaID AND PB.MediaType = {0} AND PB.IsDel = 0 AND PB.Wx_Status = {1}
					                                                            )
                                                                                AND NOT (
														                            MW.IsAreaMedia = 1
														                            AND UR.RoleID = '{2}'
													                            )
                                                            )
                                        FROM      dbo.MediaCategory AS MC WITH ( NOLOCK )
			                            WHERE  MC.MediaType = {0} AND MC.WxID > 0
                                    ) AS a
									GROUP BY a.CategoryID
                                ) AS DD
                                LEFT JOIN dbo.DictInfo AS DI WITH ( NOLOCK ) ON DI.DictId = DD.CategoryID
		                        WHERE DD.CiteCount > 0
                        ORDER BY CiteCount DESC
                        ", mediaType, (int)PublishBasicStatusEnum.上架, "SYS001RL00005");
                    break;
            }

            if (string.IsNullOrWhiteSpace(sql.ToString()))
            {
                return null;
            }
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql.ToString());
            return DataTableToList<CommonlyClassDto>(data.Tables[0]);
        }
    }
}