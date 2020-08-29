/********************************************************
*创建人：lixiong
*创建时间：2017/6/5 11:00:15
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.Media
{
    public class MediaBasePCAPP : DataBase
    {
        #region Instance

        public static readonly MediaBasePCAPP Instance = new MediaBasePCAPP();

        #endregion Instance

        public Entities.Media.MediaBasePCAPP GetEntity(int mediaId)
        {
            const string sql = @"SELECT TOP 1 [RecID]
                                  ,[Name]
                                  ,[HeadIconURL]
                                  ,[ProvinceID]
                                  ,[CityID]
                                  ,[DailyLive]
                                  ,[Remark]
                                  ,[Status]
                                  ,[CreateTime]
                            FROM [dbo].[Media_BasePCAPP] WITH(NOLOCK)
                            WHERE Status = 0 AND RecID = @RecID";
            var paras = new List<SqlParameter>() { new SqlParameter("@RecID", mediaId) };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.Media.MediaBasePCAPP>(data.Tables[0]);
        }

        /// <summary>
        /// app获取详情
        /// </summary>
        /// <param name="baseMediaId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Entities.Media.MediaBasePCAPP GetInfo(int baseMediaId, int userId)
        {
            #region sql

            var sql = @"
                    CREATE TABLE #AreaTemp(AreaID VARCHAR(100),AreaName VARCHAR(100))
                    INSERT INTO #AreaTemp
                            ( AreaID, AreaName )
                    SELECT AreaID,AreaName FROM DBO.AreaInfo WITH(NOLOCK)

                    --DROP TABLE #AreaTemp

                    SELECT  * ,AI1.AreaName AS ProvinceName,AI2.AreaName AS CityName
                           , CommonlyClassStr = (SELECT  STUFF(( SELECT  '|' + ( ISNULL(
															( CAST(DIF.DictId AS VARCHAR(15)) + ','
                                                             + DIF.DictName ) +','
															 + CAST(MC.SortNumber AS VARCHAR(15)), '') )
							                    FROM    dbo.MediaCategory AS MC WITH ( NOLOCK )
									                    LEFT JOIN dbo.DictInfo AS DIF WITH ( NOLOCK ) ON MC.CategoryID = DIF.DictId
							                    WHERE   MC.MediaType = {0}
									                    AND MC.WxID = MBPP.RecID
							                    ORDER BY MC.SortNumber DESC
						                      FOR
							                    XML PATH('')
						                      ), 1, 1, '')
                                          )
		                    ,AreaMapping=( SELECT  STUFF(( SELECT  '|' + RTRIM(ISNULL(( AI1.AreaID + ',' + AI1.AreaName ),
                                                               '') + '@=' + ISNULL(( AI2.AreaID
                                                                                  + ','
                                                                                  + ISNULL(AI2.AreaName,
                                                                                  '') ), ''))
                                    FROM    dbo.Media_Area_Mapping_Basic AS MAPB WITH ( NOLOCK )
                                            LEFT JOIN #AreaTemp AS AI1 WITH ( NOLOCK ) ON AI1.AreaID = MAPB.ProvinceID
                                            LEFT JOIN #AreaTemp AS AI2 WITH ( NOLOCK ) ON AI2.AreaID = MAPB.CityID
                                    WHERE   MAPB.MediaType = {0}
                                            AND MAPB.BaseMediaID = MBPP.RecID
                                  FOR
                                    XML PATH('')
                                  ), 1, 1, '')
                                          )
		                    ,OrderRemarkStr=(
                                SELECT  STUFF(( SELECT  '|'
                                                        + RTRIM(( CAST(PRKB.RemarkID AS VARCHAR(15))
                                                                  + ',' + ISNULL(DI.DictName, '')
                                                                  + ',' + ISNULL(PRKB.OtherContent,
                                                                                 '') ))
                                                FROM    dbo.Media_Remark_Basic AS PRKB WITH ( NOLOCK )
                                                        LEFT JOIN dbo.DictInfo AS DI WITH ( NOLOCK ) ON PRKB.RemarkID = DI.DictId
                                                WHERE   RelationID =MBPP.RecID
									                    AND PRKB.EnumType = {1}
                                              FOR
                                                XML PATH('')
                                              ), 1, 1, '')

		                    )
                            ,AdTemplateId = (
			                    --判断当前媒体下是否有已审核通过的模板或自己添加的未通过的模板
                                    SELECT  TemplateId = ISNULL(( CASE WHEN PassTemplateId > 0 THEN PassTemplateId
																ELSE NotPassTemplateId
														   END ),0)
									FROM    ( SELECT    MAX(PassTemplateId) AS PassTemplateId ,
														MAX(NotPassTemplateId) AS NotPassTemplateId
											  FROM      ( SELECT    *
														  FROM      ( SELECT TOP 1
																				ADDT.RecID AS PassTemplateId ,
																				0 AS NotPassTemplateId
																	  FROM      dbo.App_AdTemplate AS ADDT WITH ( NOLOCK )
																	  WHERE     ADDT.BaseMediaID = MBPP.RecID
																				AND ADDT.AuditStatus = {2} --已通过
																				AND ADDT.Status = 0
																	  ORDER BY  ADDT.RecID ASC
																	) AS Template1
														) AS D
											) AS C
		                    )
                    FROM    dbo.Media_BasePCAPP AS MBPP WITH ( NOLOCK )
		                         LEFT JOIN #AreaTemp AS AI1 WITH ( NOLOCK ) ON AI1.AreaID = MBPP.ProvinceID
                                 LEFT JOIN #AreaTemp AS AI2 WITH ( NOLOCK ) ON AI2.AreaID = MBPP.CityID
                    WHERE   MBPP.RecID = @RecID
                            AND MBPP.Status = 0
                    ";

            #endregion sql

            sql = string.Format(sql, (int)MediaType.APP, 45003, (int)AppTemplateEnum.已通过, (int)AppTemplateEnum.待审核,
                (int)AppTemplateEnum.已驳回, userId);
            var paras = new List<SqlParameter>() { new SqlParameter("@RecID", baseMediaId) };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.Media.MediaBasePCAPP>(data.Tables[0]);
        }

        public Entities.Media.MediaBasePCAPP GetEntity(string name, int filterMediaId = 0)
        {
            string sql = @"SELECT TOP 1 [RecID]
                                  ,[Name]
                                  ,[HeadIconURL]
                                  ,[ProvinceID]
                                  ,[CityID]
                                  ,[DailyLive]
                                  ,[Remark]
                                  ,[Status]
                                  ,[CreateTime]
                            FROM [dbo].[Media_BasePCAPP] WITH(NOLOCK)
                            WHERE Status = 0 AND Name = @Name";
            var paras = new List<SqlParameter>() { new SqlParameter("@Name", name) };
            if (filterMediaId > 0)
            {
                sql += " AND RecID != @RecID";
                paras.Add(new SqlParameter("@RecID", filterMediaId));
            }
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.Media.MediaBasePCAPP>(data.Tables[0]);
        }

        /// <summary>
		/// 增加一条数据
		/// </summary>
		public int Insert(Entities.Media.MediaBasePCAPP entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into Media_BasePCAPP(");
            strSql.Append("Name,HeadIconURL,ProvinceID,CityID,DailyLive,Remark,Status,CreateTime,CreateUserID,LastUpdateTime,LastUpdateUserID");
            strSql.Append(") values (");
            strSql.Append("@Name,@HeadIconURL,@ProvinceID,@CityID,@DailyLive,@Remark,@Status,GETDATE(),@CreateUserID,getdate(),@LastUpdateUserID");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@Name",entity.Name),
                        new SqlParameter("@HeadIconURL",entity.HeadIconURL),
                        new SqlParameter("@ProvinceID",entity.ProvinceID),
                        new SqlParameter("@CityID",entity.CityID),
                        new SqlParameter("@DailyLive",entity.DailyLive),
                        new SqlParameter("@Remark",entity.Remark),
                        new SqlParameter("@Status",entity.Status),
                         new SqlParameter("@CreateUserID",entity.CreateUserID),
                          new SqlParameter("@LastUpdateUserID",entity.LastUpdateUserID),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public int Update(Entities.Media.MediaBasePCAPP entity)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"UPDATE [dbo].[Media_BasePCAPP]");
            strSql.Append(@"SET     [Name] = @Name
                                    ,[HeadIconURL] = @HeadIconURL
                                  ,[ProvinceID] = @ProvinceID
                                  ,[CityID] = @CityID
                                  ,[DailyLive] = @DailyLive
                                  ,[Remark] = @Remark
                                    ,[LastUpdateUserID] = @LastUpdateUserID
                                    ,[LastUpdateTime] = getdate()
                            WHERE RecID = @RecID");
            var parameters = new SqlParameter[]{
                     new SqlParameter("@RecID",entity.RecID),
                      new SqlParameter("@Name",entity.Name),
                        new SqlParameter("@HeadIconURL",entity.HeadIconURL),
                        new SqlParameter("@ProvinceID",entity.ProvinceID),
                        new SqlParameter("@CityID",entity.CityID),
                        new SqlParameter("@DailyLive",entity.DailyLive),
                        new SqlParameter("@Remark",entity.Remark),
                            new SqlParameter("@LastUpdateUserID",entity.LastUpdateUserID),
                        };

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
        }
    }
}