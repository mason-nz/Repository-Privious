using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Dal.Media;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Publish;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.Publish
{
    public class PublishInfoQuery : DataBase
    {
        #region Instance

        public static readonly PublishInfoQuery Instance = new PublishInfoQuery();

        #endregion Instance

        /// <summary>
        /// 刊例查询列表 公共方法，Auth：李雄
        /// 因为5种媒体查询条件相似，只是返回数据格式稍微不同，而都是调用公共的分页方法，只需要传入不同的sql+where即可
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<T> QueryList<T>(QueryPageBase<T> query)
        {
            const string storedProcedure = "p_Page";
            var outParam = new SqlParameter("@TotalRecorder", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var sqlParams = new SqlParameter[]{
                outParam,
                new SqlParameter("@SQL",query.StrSql),
                new SqlParameter("@PageRows",query.PageSize),
                new SqlParameter("@CurPage",query.PageIndex),
                new SqlParameter("@Order",query.OrderBy)
            };

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, storedProcedure, sqlParams);
            query.Total = (int)(sqlParams[0].Value);
            return query.DataList = DataTableToList<T>(data.Tables[0]);
        }

        /// <summary>
        /// Auth：李雄
        /// 公共查询分页存储过程（2012以上版本可用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<T> QueryListBySql2014Offset_Fetch<T>(QueryPageBase<T> query)
        {
            const string storedProcedure = "p_Page_OFFSET_FETCH";
            var outParam = new SqlParameter("@TotalRecorder", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var sqlParams = new SqlParameter[]{
                outParam,
                new SqlParameter("@SQL",query.StrSql),
                new SqlParameter("@PageRows",query.PageSize),
                new SqlParameter("@CurPage",query.PageIndex),
                new SqlParameter("@Order",query.OrderBy)
            };

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, storedProcedure, sqlParams);
            query.Total = (int)(sqlParams[0].Value);
            return query.DataList = DataTableToList<T>(data.Tables[0]);
        }

        [Obsolete("经测试，p_Page存储过程不支持返回多个table")]
        public Tuple<List<T>, List<PublishItemInfo>> QueryListByTuple<T>(PublishQuery<T> query)
        {
            const string storedProcedure = "p_Page";
            var outParam = new SqlParameter("@TotalRecorder", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var sqlParams = new SqlParameter[]{
                outParam,
                new SqlParameter("@SQL",query.StrSql),
                new SqlParameter("@PageRows",query.PageSize),
                new SqlParameter("@CurPage",query.PageIndex),
                new SqlParameter("@Order",query.OrderBy)
            };

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, storedProcedure, sqlParams);
            query.Total = (int)(sqlParams[0].Value);
            query.DataList = DataTableToList<T>(data.Tables[0]);
            var publishItem = DataTableToList<PublishItemInfo>(data.Tables[1]);
            return new Tuple<List<T>, List<PublishItemInfo>>(query.DataList, publishItem);
        }

        public List<PublishDetailInfo> QueryPublishItemInfo(PublishQuery<PublishDetailInfo> query)
        {
            var sql = @"SELECT PD.RecID ,
                               PD.PubID ,
                               PD.MediaType ,
                               PD.MediaID ,
                               PD.ADPosition1 ,
                               PD.ADPosition2 ,
                               PD.ADPosition3 ,
	                          dbo.fn_GetADPositionDicName(PD.ADPosition1) AS ADPosition1Name,
                              dbo.fn_GetADPositionDicName(PD.ADPosition2) AS ADPosition2Name,
                              dbo.fn_GetADPositionDicName(PD.ADPosition3) AS ADPosition3Name,
                               PD.Price ,
                               PD.IsCarousel ,
                               PD.BeginPlayDays ,
                               PD.PublishStatus ,
                               PD.CreateTime ,
                               PD.CreateUserID FROM DBO.Publish_DetailInfo AS PD WITH(NOLOCK) WHERE 1=1 ";
            var paras = new List<SqlParameter>();

            sql += " AND PD.MediaType = " + query.MediaType;
            if (query.MediaId != null && query.MediaId.Count > 0)
            {
                sql += " AND PD.MediaID IN ({0})";
                sql = string.Format(sql, string.Join(",", query.MediaId));
            }
            if (!string.IsNullOrWhiteSpace(query.MediaIds))
            {
                sql += " AND PD.MediaID IN ({0})";
                sql = string.Format(sql, query.MediaIds);
            }
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToList<Entities.Publish.PublishDetailInfo>(data.Tables[0]);
        }

        public PublishBasicInfo GetPublishBasicInfo(PublishQuery<PublishBasicInfo> query)
        {
            var sql = @"
                SELECT A.PubID ,
                       A.MediaType ,
                       A.MediaID ,
                       A.PubCode ,
                       A.BeginTime ,
                       A.EndTime ,
                       A.PurchaseDiscount ,
                       A.SaleDiscount ,
                       A.Status ,
                       A.PublishStatus ,
                       A.CreateTime ,
                       A.CreateUserID ,
                       A.LastUpdateTime ,
                       A.LastUpdateUserID FROM dbo.Publish_BasicInfo AS A WHERE 1=1 ";
            var paras = new List<SqlParameter>();

            sql += " AND A.MediaType = " + query.MediaType;
            if (query.Media_Id > 0)
            {
                sql += " AND A.MediaID = {0}";
                sql = string.Format(sql, query.Media_Id);
            }
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.Publish.PublishBasicInfo>(data.Tables[0]);
        }

        public List<SearchTitleResponse> GetSearchAutoComplete(PublishQuery<SearchTitleResponse> query)
        {
            const string storedProcedure = "p_SearchAutoComplete";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@TopSize",query.PageSize),
                new SqlParameter("@KeyWord",query.KeyWord),
                new SqlParameter("@BusinessType",query.BusinessType),
            };

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, storedProcedure, sqlParams);
            query.Total = (int)(sqlParams[0].Value);
            return query.DataList = DataTableToList<SearchTitleResponse>(data.Tables[0]);
        }

        /// <summary>
        /// --只适合运营和超级管理员
        /// --通过或者驳回，看到的是自己的审核操作记录
        /// --驳回、通过广告数据统计
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public PublishStatisticsCount GetAdStatisticsCountByYunYing(PublishSearchAutoQuery<PublishStatisticsCount> query)
        {
            var sql = @"
                        ;
                        WITH    CTE_Statistics
                                  AS ( SELECT   COUNT(*) AS AuditPassCount ,
                                                0 AS RejectNotPassCount
                                       FROM     dbo.Publish_BasicInfo AS PB WITH ( NOLOCK )
                                                INNER JOIN dbo.Media_Weixin AS MW WITH ( NOLOCK ) ON PB.MediaID = MW.MediaID
                                                                                      AND MW.Status = 0
                                                INNER JOIN dbo.Weixin_OAuth AS WO WITH ( NOLOCK ) ON WO.RecID = MW.WxID
                                                                                      AND WO.Status = 0
                                       WHERE    PB.MediaType = {0}
                                                AND PB.IsDel = 0
                                                AND EXISTS ( SELECT 1
                                                             FROM   dbo.PublishAuditInfo AS PAI WITH ( NOLOCK )
                                                             WHERE  PAI.PublishID = PB.PubID
                                                                    AND PAI.MediaType = PB.MediaType
                                                                    AND PAI.PubStatus IN ({1}) --审核结果：通过
                                                                    AND PAI.CreateUserID = {2} -- 审核人
			                        )
                                       UNION
                                       SELECT   0 AS AppendAuditCount ,
                                                COUNT(*) AS RejectNotPassCount
                                       FROM     dbo.Publish_BasicInfo AS PB WITH ( NOLOCK )
                                                INNER JOIN dbo.Media_Weixin AS MW WITH ( NOLOCK ) ON PB.MediaID = MW.MediaID
                                                                                      AND MW.Status = 0
                                                INNER JOIN dbo.Weixin_OAuth AS WO WITH ( NOLOCK ) ON WO.RecID = MW.WxID
                                                                                      AND WO.Status = 0
                                       WHERE    PB.MediaType = {0}
                                                AND PB.IsDel = 0
                                                AND EXISTS ( SELECT 1
                                                             FROM   dbo.PublishAuditInfo AS PAI WITH ( NOLOCK )
                                                             WHERE  PAI.PublishID = PB.PubID
                                                                    AND PAI.MediaType = PB.MediaType
                                                                    AND PAI.PubStatus = {3} --审核结果：驳回
                                                                    AND PAI.CreateUserID = {2} -- 审核人
			                        )
                                     )
                            SELECT  MAX(AuditPassCount) AS AuditPassCount ,
                                    MAX(RejectNotPassCount) AS RejectNotPassCount
                            FROM    CTE_Statistics ";
            var paras = new List<SqlParameter>();

            var whereSql = new StringBuilder();

            sql = string.Format(sql, query.BusinessType, ((int)PublishBasicStatusEnum.上架 + "," + (int)PublishBasicStatusEnum.已通过)
                , query.CreateUserId, (int)PublishBasicStatusEnum.已驳回);

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<PublishStatisticsCount>(data.Tables[0]);
        }

        public PublishStatisticsCount GetPublishStatisticsCount(PublishSearchAutoQuery<PublishStatisticsCount> query)
        {
            var sql = @"
                    ;WITH    CTE_Statistics
                              AS ( SELECT   COUNT(*) AS AppendAuditCount ,
                                            0 AS RejectNotPassCount
                                   FROM     dbo.Publish_BasicInfo AS PB WITH ( NOLOCK )
                                            INNER JOIN dbo.Media_Weixin AS MW WITH(NOLOCK) ON PB.MediaID = MW.MediaID  AND MW.Status = 0
			                                INNER JOIN dbo.Weixin_OAuth AS WO WITH(NOLOCK)  ON WO.RecID = MW.WxID AND WO.Status = 0
                                   WHERE    PB.MediaType = {2}  AND PB.IsDel = 0
                                            {0}
                                            AND PB.Wx_Status ={1} --待审核
                                   UNION
                                   SELECT   0 AS AppendAuditCount ,
                                            COUNT(*) AS RejectNotPassCount
                                   FROM     dbo.Publish_BasicInfo AS PB WITH ( NOLOCK )
                                            INNER JOIN dbo.Media_Weixin AS MW WITH(NOLOCK) ON PB.MediaID = MW.MediaID AND MW.Status = 0
			                                INNER JOIN dbo.Weixin_OAuth AS WO WITH(NOLOCK)  ON WO.RecID = MW.WxID AND WO.Status = 0
                                   WHERE    PB.MediaType = {2}  AND PB.IsDel = 0
                                            {0}
                                            AND PB.Wx_Status ={3} --驳回
                                 )
                        SELECT  MAX(AppendAuditCount) AS AppendAuditCount ,
                                MAX(RejectNotPassCount) AS RejectNotPassCount
                        FROM    CTE_Statistics ";
            var paras = new List<SqlParameter>();

            var whereSql = new StringBuilder();

            if (query.CreateUserId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                whereSql.AppendFormat(@" AND PB.CreateUserID = {0}", query.CreateUserId);
            }

            sql = string.Format(sql, whereSql, (int)PublishBasicStatusEnum.待审核, query.BusinessType,
                (int)PublishBasicStatusEnum.已驳回);

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<PublishStatisticsCount>(data.Tables[0]);
        }

        public List<RespPublishAuditInfoDto> GetPublishAuditInfoList(PublishAuditInfoQuery<RespPublishAuditInfoDto> query)
        {
            var sql = @"
                    SELECT TOP ({0})  PA.* ,
                            DC1.DictName AS PubStatusName,
                            DC2.DictName AS OptTypeName,
                            UDI.TrueName
                    FROM    dbo.PublishAuditInfo AS PA WITH ( NOLOCK )
                            LEFT JOIN dbo.DictInfo AS DC1 WITH ( NOLOCK ) ON PA.OptType = DC1.DictId
                            LEFT JOIN dbo.DictInfo AS DC2 WITH ( NOLOCK ) ON PA.OptType = DC2.DictId
                            LEFT JOIN dbo.UserDetailInfo AS UDI WITH ( NOLOCK ) ON UDI.UserID = PA.CreateUserID
                    WHERE   1 = 1
                            ";

            sql = string.Format(sql, query.PageSize);
            var paras = new List<SqlParameter>();
            if (query.BusinessType != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " and PA.MediaType = @MediaType ";
                paras.Add(new SqlParameter("@MediaType", query.BusinessType));
            }
            if (query.MediaId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " and PA.MediaId = @MediaId ";
                paras.Add(new SqlParameter("@MediaId", query.MediaId));
            }
            if (query.PubId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " and PA.PublishID = @PublishID ";
                paras.Add(new SqlParameter("@PublishID", query.PubId));
            }
            if (query.TemplateId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " and PA.TemplateID = @TemplateID";
                paras.Add(new SqlParameter("@TemplateID", query.TemplateId));
            }
            if (query.CreateUserId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " and PA.CreateUserId = @CreateUserId ";
                paras.Add(new SqlParameter("@CreateUserId", query.CreateUserId));
            }

            sql += " ORDER BY PA.RecID DESC";

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToList<RespPublishAuditInfoDto>(data.Tables[0]);
        }
    }
}