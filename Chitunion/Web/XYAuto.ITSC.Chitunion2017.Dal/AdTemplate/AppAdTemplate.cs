/********************************************************
*创建人：lixiong
*创建时间：2017/6/6 13:53:08
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.AdTemplate;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.AdTemplate
{
    public class AppAdTemplate : DataBase
    {
        #region Instance

        public static readonly AppAdTemplate Instance = new AppAdTemplate();

        #endregion Instance

        /// <summary>
		/// 增加一条数据
		/// </summary>
		public int Insert(Entities.AdTemplate.AppAdTemplate entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into App_AdTemplate(");
            strSql.Append("BaseMediaID,BaseAdID,AdTemplateName,OriginalFile,AdForm,CarouselCount,SellingPlatform,SellingMode,AdLegendURL,AdDisplay,AdDescription,Remarks,AdDisplayLength,AuditStatus,CreateUserID,CreateTime,Status");
            strSql.Append(") values (");
            strSql.Append("@BaseMediaID,@BaseAdID,@AdTemplateName,@OriginalFile,@AdForm,@CarouselCount,@SellingPlatform,@SellingMode,@AdLegendURL,@AdDisplay,@AdDescription,@Remarks,@AdDisplayLength,@AuditStatus,@CreateUserID,GETDATE(),@Status");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@BaseMediaID",entity.BaseMediaID),
                        new SqlParameter("@BaseAdID",entity.BaseAdID),
                        new SqlParameter("@AdTemplateName",entity.AdTemplateName),
                        new SqlParameter("@OriginalFile",entity.OriginalFile),
                        new SqlParameter("@AdForm",entity.AdForm),
                        new SqlParameter("@CarouselCount",entity.CarouselCount),
                        new SqlParameter("@SellingPlatform",entity.SellingPlatform),
                        new SqlParameter("@SellingMode",entity.SellingMode),
                        new SqlParameter("@AdLegendURL",entity.AdLegendURL),
                        new SqlParameter("@AdDisplay",entity.AdDisplay),
                        new SqlParameter("@AdDescription",entity.AdDescription),
                        new SqlParameter("@Remarks",entity.Remarks),
                        new SqlParameter("@AdDisplayLength",entity.AdDisplayLength),
                        new SqlParameter("@AuditStatus",entity.AuditStatus),
                        new SqlParameter("@CreateUserID",entity.CreateUserID),
                        //new SqlParameter("@CreateTime",entity.CreateTime),
                        new SqlParameter("@Status",entity.Status),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public int Update(Entities.AdTemplate.AppAdTemplate entity)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"UPDATE [dbo].[App_AdTemplate]");
            strSql.Append(@"SET [BaseMediaID] = @BaseMediaID
                              ,[BaseAdID] = @BaseAdID
                              ,[AdTemplateName] = @AdTemplateName
                              ,[OriginalFile] = @OriginalFile
                              ,[AdForm] = @AdForm
                              ,[CarouselCount] = @CarouselCount
                              ,[SellingPlatform] = @SellingPlatform
                              ,[SellingMode] = @SellingMode
                              ,[AdLegendURL] = @AdLegendURL
                              ,[AdDisplay] = @AdDisplay
                              ,[AdDescription] = @AdDescription
                              ,[Remarks] = @Remarks
                              ,[AdDisplayLength] = @AdDisplayLength
                              ,[AuditStatus] = @AuditStatus
                            WHERE RecID = @RecID");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@RecID",entity.RecID),
                        new SqlParameter("@BaseMediaID",entity.BaseMediaID),
                        new SqlParameter("@BaseAdID",entity.BaseAdID),
                        new SqlParameter("@AdTemplateName",entity.AdTemplateName),
                        new SqlParameter("@OriginalFile",entity.OriginalFile),
                        new SqlParameter("@AdForm",entity.AdForm),
                        new SqlParameter("@CarouselCount",entity.CarouselCount),
                        new SqlParameter("@SellingPlatform",entity.SellingPlatform),
                        new SqlParameter("@SellingMode",entity.SellingMode),
                        new SqlParameter("@AdLegendURL",entity.AdLegendURL),
                        new SqlParameter("@AdDisplay",entity.AdDisplay),
                        new SqlParameter("@AdDescription",entity.AdDescription),
                        new SqlParameter("@Remarks",entity.Remarks),
                        new SqlParameter("@AdDisplayLength",entity.AdDisplayLength),
                        new SqlParameter("@AuditStatus",entity.AuditStatus),
                        };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
        }

        public Entities.AdTemplate.AppAdTemplate GetEntity(int templateId, string templateName = "")
        {
            string sql = @"SELECT TOP 1 [RecID]
                              ,[BaseMediaID]
                              ,[BaseAdID]
                              ,[AdTemplateName]
                              ,[OriginalFile]
                              ,[AdForm]
                              ,[CarouselCount]
                              ,[SellingPlatform]
                              ,[SellingMode]
                              ,[AdLegendURL]
                              ,[AdDisplay]
                              ,[AdDescription]
                              ,[Remarks]
                              ,[AdDisplayLength]
                              ,[AuditStatus]
                              ,[CreateUserID]
                              ,[CreateTime]
                              ,[Status]
                          FROM [dbo].[App_AdTemplate] WITH(NOLOCK)
                        WHERE Status = 0 ";
            var paras = new List<SqlParameter>() { };
            if (templateId > 0)
            {
                sql += " AND RecID = @RecID ";
                paras.Add(new SqlParameter("@RecID", templateId));
            }
            if (!string.IsNullOrWhiteSpace(templateName))
            {
                sql += " AND AdTemplateName = @AdTemplateName ";
                paras.Add(new SqlParameter("@AdTemplateName", templateName));
            }

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.AdTemplate.AppAdTemplate>(data.Tables[0]);
        }

        public Entities.AdTemplate.AppAdTemplate GetInfoV1(int templateId, int mediaId = 0, int baseAdTempateId = 0,
            int userId = 0)
        {
            string sql = @"
                            SELECT  AADT.* ,
                                    AdStyleStr = ( SELECT    STUFF(( SELECT  '|'
                                                                            + (CAST(ATS.RecID AS VARCHAR(15))
                                                                            +','+ ISNULL(ATS.AdStyle, '')
                                                                            +','+ CAST(AADT.BaseMediaID AS VARCHAR(15))
                                                                            +','+ CAST(CAST(ATS.IsPublic AS INT)AS VARCHAR(15))
                                                                            +','+ CAST(CAST(ATS.CreateUserID AS INT)AS VARCHAR(15)))
                                                                    FROM    dbo.App_AdTemplateStyle AS ATS WITH ( NOLOCK )
                                                                    WHERE   ATS.AdTemplateID IN (AADT.RecID,AADT.BaseAdID)
                                                                  FOR
                                                                    XML PATH('')
                                                                  ), 1, 1, '')
                                                 )
		                            ,AdSaleAreaGroupStr=(
			                            SELECT  STUFF(( SELECT  '|' + ( ( CAST(SAI.GroupID AS VARCHAR(15)) + ','
                                                                    + ISNULL(SAI.GroupName, '') + ','+ CAST(SAI.GroupType AS VARCHAR(15)) + ','
                                                                    + CAST(CAST(SAI.IsPublic AS INT) AS VARCHAR(15)) ) + '$='
                                                                    + ISNULL( ( SELECT  ( ( ISNULL(CAST(AI2.AreaID AS VARCHAR(15)),'')
                                                                                    + ',' + ISNULL(AI2.AreaName,'') + ',' + ISNULL( CAST(CAST(SAR.IsPublic AS INT) AS VARCHAR(15)),''))
												                                                    + '@='
                                                                                    + ( ISNULL(CAST(AI1.AreaID AS VARCHAR(15)),'')
                                                                                    + ',' + ISNULL(AI1.AreaName,'') +',' + ISNULL( CAST(SAR.CreateUserID AS VARCHAR(15)),'') ) )
                                                                FROM    dbo.AreaInfo AS AI1 WITH ( NOLOCK )
                                                                        LEFT JOIN dbo.AreaInfo AS AI2 WITH ( NOLOCK ) ON SAR.ProvinceID = AI2.AreaID
                                                                WHERE   SAR.CityID = AI1.AreaID
                                                              FOR
                                                                XML PATH('')
                                                                ),'' ))
                                            FROM    dbo.SaleAreaInfo AS SAI WITH ( NOLOCK )
                                                    LEFT JOIN dbo.SaleAreaRelation AS SAR WITH ( NOLOCK ) ON SAI.GroupID = SAR.GroupID
                                                    WHERE SAI.TemplateID IN (AADT.RecID,AADT.BaseAdID)
                                          FOR
                                            XML PATH('')
                                          ), 1, 1, '')
		                            )
                                    ,MBP.Name AS BaseMediaName
									,MBP.HeadIconURL AS BaseMediaLogoUrl
                                    ,DI.DictName AS AdFormName
                                    ,VUI.SysName AS TrueName
                            FROM    dbo.App_AdTemplate AS AADT WITH ( NOLOCK )
							        LEFT JOIN DBO.Media_BasePCAPP AS MBP WITH(NOLOCK) ON AADT.BaseMediaID = MBP.RecID AND MBP.Status = 0
							        LEFT JOIN DBO.DictInfo AS DI WITH(NOLOCK) ON AADT.AdForm = DI.DictId
                                    LEFT JOIN dbo.v_UserInfo AS VUI WITH ( NOLOCK ) ON VUI.UserID = AADT.CreateUserID
                            WHERE   AADT.Status = 0
                            ";

            //sql = string.Format(sql, userId);
            var paras = new List<SqlParameter>() { };
            if (templateId > 0)
            {
                sql += " AND AADT.RecID = @RecID ";
                paras.Add(new SqlParameter("@RecID", templateId));
            }
            if (mediaId > 0)
            {
                sql += " AND AADT.BaseMediaID = @BaseMediaID ";
                paras.Add(new SqlParameter("@BaseMediaID", mediaId));
            }
            if (baseAdTempateId > 0)
            {
                sql += " AND AADT.BaseAdID = @BaseAdID ";
                paras.Add(new SqlParameter("@BaseAdID", baseAdTempateId));
            }

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.AdTemplate.AppAdTemplate>(data.Tables[0]);
        }

        public List<Entities.AdTemplate.AppAdTemplate> GetAuditInfoListV1(AdTemplateQuery<Entities.AdTemplate.AppAdTemplate> query)
        {
            #region sql

            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");
            sbSql.AppendFormat(@"
                            SELECT  AADT.* ,
                                    AdStyleStr = ( SELECT    STUFF(( SELECT  '|'
                                                                            + (CAST(ATS.RecID AS VARCHAR(15))
                                                                            +','+ ISNULL(ATS.AdStyle, '')
                                                                            +','+ CAST(AADT.BaseMediaID AS VARCHAR(15))
                                                                            +','+ CAST(CAST(ATS.IsPublic AS INT)AS VARCHAR(15))
                                                                            +','+ CAST(CAST(ATS.CreateUserID AS INT)AS VARCHAR(15)))
                                                                    FROM    dbo.App_AdTemplateStyle AS ATS WITH ( NOLOCK )
                                                                    WHERE   ATS.AdTemplateID IN (AADT.RecID,AADT.BaseAdID)
                                                                  FOR
                                                                    XML PATH('')
                                                                  ), 1, 1, '')
                                                 )
		                            ,AdSaleAreaGroupStr=(
			                            SELECT  STUFF(( SELECT  '|' + ( ( CAST(SAI.GroupID AS VARCHAR(15)) + ','
                                                                    + ISNULL(SAI.GroupName, '') + ','+ CAST(SAI.GroupType AS VARCHAR(15)) + ','
                                                                    + CAST(CAST(SAI.IsPublic AS INT) AS VARCHAR(15)) ) + '$='
                                                                    + ISNULL( ( SELECT  ( ( ISNULL(CAST(AI2.AreaID AS VARCHAR(15)),'')
                                                                                    + ',' + ISNULL(AI2.AreaName,'') + ',' + ISNULL( CAST(CAST(SAR.IsPublic AS INT) AS VARCHAR(15)),''))
												                                                    + '@='
                                                                                    + ( ISNULL(CAST(AI1.AreaID AS VARCHAR(15)),'')
                                                                                    + ',' + ISNULL(AI1.AreaName,'') +',' + ISNULL( CAST(SAR.CreateUserID AS VARCHAR(15)),'') ) )
                                                                FROM    dbo.AreaInfo AS AI1 WITH ( NOLOCK )
                                                                        LEFT JOIN dbo.AreaInfo AS AI2 WITH ( NOLOCK ) ON SAR.ProvinceID = AI2.AreaID
                                                                WHERE   SAR.CityID = AI1.AreaID
                                                              FOR
                                                                XML PATH('')
                                                                ),'' ))
                                            FROM    dbo.SaleAreaInfo AS SAI WITH ( NOLOCK )
                                                    LEFT JOIN dbo.SaleAreaRelation AS SAR WITH ( NOLOCK ) ON SAI.GroupID = SAR.GroupID
                                                    WHERE SAI.TemplateID IN (AADT.RecID,AADT.BaseAdID)
                                          FOR
                                            XML PATH('')
                                          ), 1, 1, '')
		                            )
                                    ,VUI.SysName AS TrueName
                                    ,DI.DictName AS AdFormName
                            FROM    dbo.App_AdTemplate AS AADT WITH ( NOLOCK )
                                    LEFT JOIN DBO.DictInfo AS DI WITH(NOLOCK) ON AADT.AdForm = DI.DictId
                                    LEFT JOIN dbo.v_UserInfo AS VUI WITH ( NOLOCK ) ON VUI.UserID = AADT.CreateUserID
                            WHERE   AADT.Status = 0
                            ");

            #endregion sql

            var paras = new List<SqlParameter>() { };
            if (query.TemplateId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.Append(" AND AADT.RecID = " + query.TemplateId);
                //paras.Add(new SqlParameter("@RecID", query.TemplateId));
            }
            if (query.BaseMediaId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.Append(" AND AADT.BaseMediaID = " + query.BaseMediaId);
                //paras.Add(new SqlParameter("@BaseMediaID", query.BaseMediaId));
            }
            if (query.BaseAdId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.Append(" AND AADT.BaseAdID =" + query.BaseAdId);
                //paras.Add(new SqlParameter("@BaseAdID", query.BaseAdId));
            }
            if (query.AuditStatus != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.Append(" AND AADT.AuditStatus = " + query.AuditStatus);
                //paras.Add(new SqlParameter("@AuditStatus", query.AuditStatus));
            }
            if (query.FilterTemplateId > 0)
            {
                sbSql.Append(" AND AADT.RecID != " + query.FilterTemplateId);
            }
            if (!string.IsNullOrWhiteSpace(query.AdTempIdList))
            {
                var sqlCon = string.Format("AND AADT.RecID IN ({0})", query.AdTempIdList.Trim(','));
                sbSql.Append(sqlCon);
            }
            sbSql.AppendLine(@") T");

            var sqlPageQuery = new PublishQuery<Entities.AdTemplate.AppAdTemplate>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " RecID DESC ",
                PageSize = query.PageSize,
                PageIndex = query.PageIndex
            };
            query.DataList = Dal.Publish.PublishInfoQuery.Instance.QueryList(sqlPageQuery);
            query.Total = sqlPageQuery.Total;

            return query.DataList;
        }

        public List<Entities.AdTemplate.AppAdTemplate> GetList(AdTemplateQuery<Entities.AdTemplate.AppAdTemplate> query)
        {
            string sql = @"SELECT TOP ({0}) AADT.[RecID]
                              ,AADT.[BaseMediaID]
                              ,AADT.[BaseAdID]
                              ,AADT.[AdTemplateName]
                              ,AADT.[OriginalFile]
                              ,AADT.[AdForm]
                              ,AADT.[CarouselCount]
                              ,AADT.[SellingPlatform]
                              ,AADT.[SellingMode]
                              ,AADT.[AdLegendURL]
                              ,AADT.[AdDisplay]
                              ,AADT.[AdDescription]
                              ,AADT.[Remarks]
                              ,AADT.[AdDisplayLength]
                              ,AADT.[AuditStatus]
                              ,AADT.[CreateUserID]
                              ,AADT.[CreateTime]
                              ,AADT.[Status]
                                {1}
                          FROM [dbo].[App_AdTemplate] AS AADT WITH(NOLOCK)
                                {2}
                        WHERE AADT.Status = 0 ";

            sql = string.Format(sql, query.PageSize, query.IsGetTrueName ? ",VUI.SysName AS TrueName" : string.Empty,
                   query.IsGetTrueName ? " LEFT JOIN dbo.v_UserInfo AS VUI WITH ( NOLOCK ) ON VUI.UserID = AADT.CreateUserID "
                   : string.Empty);
            var paras = new List<SqlParameter>();

            if (query.CreateUserId > Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND AADT.CreateUserID = @CreateUserID";
                paras.Add(new SqlParameter("@CreateUserID", query.CreateUserId));
            }
            if (query.BaseAdId > Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND AADT.BaseAdID = @BaseAdID";
                paras.Add(new SqlParameter("@BaseAdID", query.BaseAdId));
            }
            if (query.BaseMediaId > Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND AADT.BaseMediaID = @BaseMediaID";
                paras.Add(new SqlParameter("@BaseMediaID", query.BaseMediaId));
            }
            if (query.TemplateId > Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND AADT.RecID = @RecID";
                paras.Add(new SqlParameter("@RecID", query.TemplateId));
            }
            if (!string.IsNullOrWhiteSpace(query.AuditStatusStr))
            {
                sql += string.Format(" AND AADT.AuditStatus IN ({0})", query.AuditStatusStr);
            }
            if (query.AuditStatus > 0)
            {
                sql += " AND AADT.AuditStatus = @AuditStatus";
                paras.Add(new SqlParameter("@AuditStatus", query.AuditStatus));
            }
            if (!string.IsNullOrWhiteSpace(query.TemplateName))
            {
                sql += " AND AADT.AdTemplateName = @AdTemplateName";
                paras.Add(new SqlParameter("@AdTemplateName", SqlFilter(query.TemplateName)));
            }
            if (query.FilterTemplateId > 0)
            {
                sql += " AND AADT.RecID != @RecID";
                paras.Add(new SqlParameter("@RecID", query.FilterTemplateId));
            }

            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                sql = string.Format(" ORDER BY {0}", query.OrderBy);
            }
            else
            {
                sql += " ORDER BY RecID DESC ";
            }

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToList<Entities.AdTemplate.AppAdTemplate>(data.Tables[0]);
        }

        public Entities.AdTemplate.AppAdTemplate VerifyOfTemplateNameByRole(string roleId, string templateName,
            int mediaId, string auditStatus, int filterTemplateId = -2)
        {
            var sql = @"
                       SELECT   top 1 *
                        FROM    dbo.App_AdTemplate AS ADT WITH ( NOLOCK )
                        WHERE   ADT.BaseMediaID = {0}
                                AND ADT.AdTemplateName = '{1}'
                                AND ADT.AuditStatus IN ({2})
                                AND EXISTS ( SELECT UserID
                                             FROM   dbo.UserRole
                                             WHERE  RoleID = '{3}'
                                                    AND Status = 0
                                                    AND ADT.CreateUserID = UserID )
                        ";
            var paras = new List<SqlParameter>();
            sql = string.Format(sql, mediaId, templateName, auditStatus, roleId);

            if (filterTemplateId > 0)
            {
                sql += " AND ADT.RecID != " + filterTemplateId;
            }

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.AdTemplate.AppAdTemplate>(data.Tables[0]);
        }

        public Entities.AdTemplate.AppAdTemplate VerifyOfBaseAdTemplateIdByRole(string roleId, int baseAdTemplateId,
            int mediaId, string auditStatus, int filterTemplateId = -2)
        {
            var sql = @"
                       SELECT   top 1 *
                        FROM    dbo.App_AdTemplate AS ADT WITH ( NOLOCK )
                        WHERE   ADT.BaseMediaID = {0}
                                AND ADT.BaseAdID = {1}
                                AND ADT.Status = 0
                                AND ADT.AuditStatus IN ({2})
                                AND EXISTS ( SELECT UserID
                                             FROM   dbo.UserRole
                                             WHERE  RoleID = '{3}'
                                                    AND Status = 0
                                                    AND ADT.CreateUserID = UserID )
                        ";
            var paras = new List<SqlParameter>();
            sql = string.Format(sql, mediaId, baseAdTemplateId, auditStatus, roleId);

            if (filterTemplateId > 0)
            {
                sql += " AND ADT.RecID != " + filterTemplateId;
            }

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.AdTemplate.AppAdTemplate>(data.Tables[0]);
        }

        public PublishStatisticsCount GetAdStatisticsCountByYunYing(PublishSearchAutoQuery<PublishStatisticsCount> query)
        {
            var sql = @"
                        ;
                        WITH    CTE_Statistics
                                  AS ( SELECT   COUNT(*) AS AppendAuditCount ,
                                                0 AS RejectNotPassCount ,
                                                0 AS AuditPassCount
                                       FROM     dbo.App_AdTemplate AS ADT WITH ( NOLOCK )
                                       WHERE    ADT.AuditStatus = {0} --待审核
						                        AND ADT.Status = 0
                                       UNION
                                       SELECT   0 AS AppendAuditCount ,
                                                0 AS RejectNotPassCount ,
                                                COUNT(*) AS AuditPassCount
                                       FROM     dbo.App_AdTemplate AS ADT WITH ( NOLOCK )
                                       WHERE    ADT.AuditStatus = {1} --已通过
						                        AND ADT.Status = 0
                                       UNION
                                       SELECT   0 AS AppendAuditCount ,
                                                COUNT(*) AS RejectNotPassCount ,
                                                0 AS AuditPassCount
                                       FROM     dbo.App_AdTemplate AS ADT WITH ( NOLOCK )
                                       WHERE    ADT.AuditStatus = {2} --已驳回
						                        AND ADT.Status = 0
                                     )
                            SELECT  MAX(AppendAuditCount) AS AppendAuditCount ,
                                    MAX(RejectNotPassCount) AS RejectNotPassCount ,
                                    MAX(AuditPassCount) AS AuditPassCount
                            FROM    CTE_Statistics
                        ";
            var paras = new List<SqlParameter>();

            var whereSql = new StringBuilder();

            sql = string.Format(sql, (int)AppTemplateEnum.待审核, (int)AppTemplateEnum.已通过,
                (int)AppTemplateEnum.已驳回);

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<PublishStatisticsCount>(data.Tables[0]);
        }

        /// <summary>
        /// 模板修正最后的逻辑处理（修改指向公共模板的引用）
        /// </summary>
        /// <param name="adTemplateId"></param>
        public void TemplateModified(int adTemplateId)
        {
            var sql = @"
                    --将模板城市组信息和样式信息追加到公共模板
                    DECLARE @TemplateId INT = {0} , --参数
                        @BaseTemplateId INT = 0,
	                    @CarouselCount INT = 0,
	                    @SellingPlatform INT = 0,
	                    @SellingMode INT = 0

                    SELECT  @BaseTemplateId = ADT.BaseAdID,
		                    @CarouselCount = ADT.CarouselCount,
		                    @SellingMode = ADT.SellingMode,
		                    @SellingPlatform = ADT.SellingPlatform
                    FROM    dbo.App_AdTemplate AS ADT WITH ( NOLOCK )
                    WHERE   ADT.RecID = @TemplateId

                    IF ( @BaseTemplateId > 0 )
                        BEGIN
                            CREATE TABLE #GroupSaleAreaTemp
                                (
                                  GroupName VARCHAR(100) ,
                                  TemplateID INT ,
                                  IsPublic BIT ,
                                  GroupType INT ,
                                  CreateUserID INT ,
                                  CreateTime DATETIME
                                )
		                    INSERT INTO #GroupSaleAreaTemp
		                            ( GroupName ,
		                              TemplateID ,
		                              IsPublic ,
		                              GroupType ,
		                              CreateUserID ,
		                              CreateTime
		                            )
		                    SELECT SAI.GroupName,SAI.TemplateID,SAI.IsPublic,SAI.GroupType,SAI.CreateUserID,SAI.CreateTime
		                    FROM DBO.SaleAreaInfo AS SAI WITH(NOLOCK) WHERE SAI.TemplateID = @TemplateId AND SAI.GroupType = 0 --普通城市组

		                    --1.将模板的样式追加到公共模板(应该只需要修改TemplateID,IsPublic,因为默认先在当前模板存了这批数据)不需要重新添加
		                    UPDATE DBO.App_AdTemplateStyle SET AdTemplateID = @BaseTemplateId,IsPublic = 1
		                    WHERE AdTemplateID = @TemplateId

		                    --2.将城市组追加到公共模板(同上原理)
		                    UPDATE DBO.SaleAreaInfo SET TemplateID = @BaseTemplateId,IsPublic = 1
		                    WHERE TemplateID = @TemplateId

		                    --3.将城市列表追加到公共模板
		                    UPDATE DBO.SaleAreaRelation SET IsPublic = 1
		                    WHERE GroupID IN(
			                    SELECT GroupID FROM #GroupSaleAreaTemp WHERE TemplateID = @TemplateId
		                    )
                            --4.修改公共模型信息
		                            UPDATE DBO.App_AdTemplate SET
			                            CarouselCount = @CarouselCount,
			                            SellingPlatform = @SellingPlatform,
			                            SellingMode = @SellingMode
		                            WHERE RecID = @BaseTemplateId

		                    --5.删除此模板(因为所以的信息都修改指向了公共模板)
		                    UPDATE DBO.App_AdTemplate SET Status = -1 WHERE RecID = @TemplateId
                        END";
            var parameters = new SqlParameter[] { };
            sql = string.Format(sql, adTemplateId);
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }

        public AuditTemplateDTORes AuditTemplate(int templateID, int optType, string rejectReason, int createUserID, ref string notice)
        {
            int rowcount = 0;
            SqlParameter[] parameters = null;
            AuditTemplateDTORes res = new AuditTemplateDTORes();

            string sql = @"SELECT  dbo.App_AdTemplate.* ,
                                            v_UserInfo.SysName AS CreateUserName ,
                                            v_UserInfo.SysName ,
                                            IsAuthAE = CAST((CASE WHEN dbo.UserRole.RoleID = 'SYS001RL00005' THEN 1 ELSE 0 END) as BIT),
		                                    base.AdTemplateName AS BaseAdName
                                    FROM dbo.App_AdTemplate
                                    LEFT JOIN dbo.UserRole on dbo.App_AdTemplate.CreateUserID = dbo.UserRole.UserID
                                    INNER JOIN v_UserInfo ON App_AdTemplate.CreateUserID = v_UserInfo.UserID
                                    LEFT JOIN dbo.App_AdTemplate base ON dbo.App_AdTemplate.BaseAdID = base.RecID
                                    WHERE App_AdTemplate.RecID = " + templateID;
            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            var one = DataTableToEntity<Entities.AdTemplate.AppAdTemplate>(ds.Tables[0]);
            if (optType.Equals((int)AppTemplateEnum.已通过) && one.BaseAdID > 0)
            {
                if (this.CheckTemplateStyleIsRepeat(CheckRepeatTypeEnum.模板样式重复, one.BaseAdID, one.RecID))
                {
                    notice = "广告样式与已通过的模板中的重复，请重新修正或驳回";
                    return null;
                }
                if (this.CheckTemplateStyleIsRepeat(CheckRepeatTypeEnum.模板区域组重复, one.BaseAdID, one.RecID))
                {
                    notice = "售卖区域中的城市组与已通过的模板中的重复，请重新修正或驳回";
                    return null;
                }
                if (this.CheckTemplateStyleIsRepeat(CheckRepeatTypeEnum.模板区域组关联重复, one.BaseAdID, one.RecID))
                {
                    notice = "售卖区域中的城市与已通过模板中的重复，请重新修正或驳回";
                    return null;
                }
            }

            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (optType.Equals((int)AppTemplateEnum.已通过))
                        {
                            if (one.BaseAdID <= 0)
                            {
                                #region 新增公共

                                //检查同媒体下不能有审核通过重名的
                                sql = @"SELECT COUNT(1) FROM dbo.App_AdTemplate
                                              WHERE AuditStatus = 48002 AND Status = 0 AND BaseMediaID = @BaseMediaID AND AdTemplateName = @AdTemplateName";
                                parameters = new SqlParameter[]
                                {
                                    new SqlParameter("@BaseMediaID", one.BaseMediaID),
                                    new SqlParameter("@AdTemplateName", one.AdTemplateName)
                                };
                                int count = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sql, parameters));
                                if (count > 0)
                                {
                                    trans.Commit();
                                    notice = "广告模板已存在，请勿重复添加!";
                                    return null;
                                }
                                else
                                {
                                    //App_AdTemplate
                                    sql = "UPDATE dbo.App_AdTemplate SET AuditStatus = " + optType + " WHERE RecID = " + templateID;
                                    rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql);
                                    //PublishAuditInfo
                                    sql = @"INSERT INTO PublishAuditInfo(MediaType,TemplateID,OptType,PubStatus,RejectMsg,CreateTime,CreateUserID)
                                VALUES(@MediaType,@TemplateID,@OptType,@PubStatus,@RejectMsg,@CreateTime,@CreateUserID)";
                                    parameters = new SqlParameter[]
                                    {
                                    new SqlParameter("@MediaType",(int)MediaTypeEnum.APP),
                                    new SqlParameter("@TemplateID",templateID),
                                    new SqlParameter("@OptType",optType),
                                    new SqlParameter("@PubStatus",one.IsAuthAE ? (int)AppPublishStatus.待审核:(int)AppPublishStatus.已上架),
                                    new SqlParameter("@RejectMsg", string.Empty),
                                    new SqlParameter("@CreateTime",DateTime.Now),
                                    new SqlParameter("@CreateUserID",createUserID)
                                    };
                                    rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, parameters);
                                    //WeChatOperateMsg
                                    parameters = new SqlParameter[]
                                     {
                                    new SqlParameter("@ID", templateID),
                                    new SqlParameter("@MsgType", (int)OperateMsgTypeEnum.模板审核),
                                     new SqlParameter("@OptType", (int)AppTemplateEnum.已通过),
                                    new SqlParameter("@CreateUserID", createUserID),
                                    };
                                    SqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "p_OperateMsgAPPInsert", parameters);

                                    //Publish_BasicInfo
                                    //sql = "UPDATE dbo.Publish_BasicInfo SET Wx_Status = @Wx_Status WHERE MediaType = 14002 AND IsDel = 0 AND TemplateID = @TemplateID";
                                    //parameters = new SqlParameter[]
                                    //{
                                    //new SqlParameter("@Wx_Status", one.IsAuthAE ? (int)AppPublishStatus.待审核:(int)AppPublishStatus.已上架),
                                    //new SqlParameter("@TemplateID",templateID),
                                    //};
                                    //rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, parameters);

                                    //App_AdTemplateStyle
                                    sql = "UPDATE dbo.App_AdTemplateStyle SET IsPublic = 1 WHERE AdTemplateID = " + templateID;
                                    rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql);
                                    //SaleAreaInfo
                                    sql = "UPDATE dbo.SaleAreaInfo SET IsPublic = 1 WHERE TemplateID = " + templateID;
                                    rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql);
                                    //SaleAreaRelation
                                    sql = "UPDATE dbo.SaleAreaRelation SET IsPublic = 1 WHERE EXISTS (Select 1 from dbo.SaleAreaInfo  Where TemplateID = " + templateID + " and SaleAreaInfo.GroupID = SaleAreaRelation.GroupID)";
                                    rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql);
                                }

                                #endregion 新增公共
                            }
                            else
                            {
                                #region 修改公共

                                //App_AdTemplate
                                sql = @"UPDATE  dbo.App_AdTemplate
                                                SET AdTemplateName = @AdTemplateName,
                                                        OriginalFile = @OriginalFile,
                                                        AdForm = @AdForm,
                                                        CarouselCount = @CarouselCount,
                                                        SellingPlatform = @SellingPlatform,
                                                        SellingMode = @SellingMode,
                                                        AdLegendURL = @AdLegendURL,
                                                        AdDisplay = @AdDisplay,
                                                        AdDescription = @AdDescription,
                                                        Remarks = @Remarks,
                                                        AdDisplayLength = @AdDisplayLength
                                                WHERE RecID = @RecID ";
                                parameters = new SqlParameter[]
                                {
                                    new SqlParameter("@AdTemplateName",one.AdTemplateName),
                                    new SqlParameter("@OriginalFile",one.OriginalFile),
                                    new SqlParameter("@AdForm",one.AdForm),
                                    new SqlParameter("@CarouselCount",one.CarouselCount),
                                    new SqlParameter("@SellingPlatform",one.SellingPlatform),
                                    new SqlParameter("@SellingMode",one.SellingMode),
                                    new SqlParameter("@AdLegendURL",one.AdLegendURL),
                                    new SqlParameter("@AdDisplay",one.AdDisplay),
                                    new SqlParameter("@AdDescription",one.AdDescription),
                                    new SqlParameter("@Remarks",one.Remarks),
                                    new SqlParameter("@AdDisplayLength",one.AdDisplayLength),
                                    new SqlParameter("@RecID",one.BaseAdID),
                                };
                                rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, parameters);
                                sql = "UPDATE  dbo.App_AdTemplate SET Status = -1 Where RecID = " + templateID;
                                rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql);
                                //PublishAuditInfo
                                sql = @"INSERT INTO PublishAuditInfo(MediaType,TemplateID,OptType,PubStatus,RejectMsg,CreateTime,CreateUserID)
                                VALUES(@MediaType,@TemplateID,@OptType,@PubStatus,@RejectMsg,@CreateTime,@CreateUserID)";
                                parameters = new SqlParameter[]
                                {
                                    new SqlParameter("@MediaType",(int)MediaTypeEnum.APP),
                                    new SqlParameter("@TemplateID",templateID),
                                    new SqlParameter("@OptType",optType),
                                    new SqlParameter("@PubStatus",one.IsAuthAE ? (int)AppPublishStatus.待审核:(int)AppPublishStatus.已上架),
                                    new SqlParameter("@RejectMsg", string.Empty),
                                    new SqlParameter("@CreateTime",DateTime.Now),
                                    new SqlParameter("@CreateUserID",createUserID)
                                };
                                rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, parameters);
                                //WeChatOperateMsg
                                parameters = new SqlParameter[]
                                 {
                                     new SqlParameter("@ID", templateID),
                                     new SqlParameter("@MsgType", (int)OperateMsgTypeEnum.模板审核),
                                     new SqlParameter("@OptType", (int)AppTemplateEnum.已通过),
                                     new SqlParameter("@CreateUserID", createUserID),
                                };
                                SqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "p_OperateMsgAPPInsert", parameters);

                                //Publish_BasicInfo
                                //sql = "UPDATE dbo.Publish_BasicInfo SET TemplateID = @BaseTemplateID, Wx_Status = @Wx_Status WHERE MediaType = 14002 AND IsDel = 0 AND TemplateID = @TemplateID";
                                //parameters = new SqlParameter[]
                                //{
                                //new SqlParameter("@BaseTemplateID",one.BaseAdID),
                                //new SqlParameter("@Wx_Status", one.IsAuthAE ? (int)AppPublishStatus.待审核:(int)AppPublishStatus.已上架),
                                //new SqlParameter("@TemplateID",templateID),
                                //};
                                //rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, parameters);

                                //App_AdTemplateStyle
                                sql = "UPDATE dbo.App_AdTemplateStyle SET AdTemplateID = @BaseTemplateID,IsPublic = 1 WHERE AdTemplateID = @TemplateID";
                                parameters = new SqlParameter[]
                                {
                                    new SqlParameter("@BaseTemplateID",one.BaseAdID),
                                    new SqlParameter("@TemplateID",templateID),
                                };
                                rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, parameters);
                                //SaleAreaInfo
                                sql = "UPDATE dbo.SaleAreaInfo SET TemplateID = @BaseTemplateID,IsPublic = 1 WHERE TemplateID = @TemplateID AND GroupType = 1";
                                parameters = new SqlParameter[]
                                {
                                    new SqlParameter("@BaseTemplateID",one.BaseAdID),
                                    new SqlParameter("@TemplateID",templateID),
                                };
                                rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, parameters);
                                //SaleAreaRelation
                                sql = "UPDATE dbo.SaleAreaRelation SET IsPublic = 1 WHERE EXISTS (Select 1 from dbo.SaleAreaInfo  Where dbo.SaleAreaInfo.TemplateID = " + one.BaseAdID + " and SaleAreaInfo.GroupID = SaleAreaRelation.GroupID)";
                                if (one.IsAuthAE)
                                {
                                    sql += " and CreateUserID in (select UserID from UserRole where RoleID = 'SYS001RL00005')";
                                }
                                else
                                {
                                    sql += " and CreateUserID = " + one.CreateUserID;
                                }
                                rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql);
                                //AppPriceInfo
                                sql = "UPDATE dbo.AppPriceInfo SET TemplateID = @BaseTemplateID WHERE TemplateID =@TemplateID";
                                parameters = new SqlParameter[]
                                {
                                    new SqlParameter("@BaseTemplateID",one.BaseAdID),
                                    new SqlParameter("@TemplateID",templateID),
                                };
                                rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, parameters);

                                #endregion 修改公共
                            }
                        }
                        else
                        {
                            #region 驳回

                            //App_AdTemplate
                            sql = "UPDATE dbo.App_AdTemplate SET AuditStatus = " + optType + " WHERE RecID = " + templateID;
                            rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql);
                            //PublishAuditInfo
                            sql = @"INSERT INTO PublishAuditInfo(MediaType,TemplateID,OptType,PubStatus,RejectMsg,CreateTime,CreateUserID)
                                VALUES(@MediaType,@TemplateID,@OptType,@PubStatus,@RejectMsg,@CreateTime,@CreateUserID)";
                            parameters = new SqlParameter[]
                            {
                                new SqlParameter("@MediaType",(int)MediaTypeEnum.APP),
                                new SqlParameter("@TemplateID",templateID),
                                new SqlParameter("@OptType",optType),
                                new SqlParameter("@PubStatus",optType),
                                new SqlParameter("@RejectMsg", SqlFilter(rejectReason)),
                                new SqlParameter("@CreateTime",DateTime.Now),
                                new SqlParameter("@CreateUserID",createUserID)
                            };
                            rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, parameters);
                            //WeChatOperateMsg
                            parameters = new SqlParameter[]
                            {
                                new SqlParameter("@ID", templateID),
                                new SqlParameter("@MsgType", (int)OperateMsgTypeEnum.模板审核),
                                new SqlParameter("@OptType", (int)AppTemplateEnum.已驳回),
                                new SqlParameter("@CreateUserID", createUserID),
                            };
                            SqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "p_OperateMsgAPPInsert", parameters);

                            #endregion 驳回
                        }
                        trans.Commit();
                        var next = this.GetNextWaittingAudit(templateID);
                        if (next != null)
                        {
                            res.NextAdTempId = next.RecID;
                            res.NextAdBaseTempId = next.BaseAdID;
                        }
                        return res;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return null;
                    }
                }
            }
        }

        public bool CheckTemplateStyleIsRepeat(CheckRepeatTypeEnum type, int baseTemplateID, int newTemplateID)
        {
            string sql = string.Empty;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@BaseTemplateID", baseTemplateID),
                new SqlParameter("@NewTemplateID", newTemplateID)
            };
            switch (type)
            {
                case CheckRepeatTypeEnum.模板样式重复:
                    sql = @"SELECT  COUNT(1) FROM    dbo.App_AdTemplateStyle ats1
                                  WHERE   ats1.AdTemplateID = @NewTemplateID
                                  AND EXISTS ( SELECT 1 FROM    dbo.App_AdTemplateStyle ats2
                                                         WHERE  ats2.AdTemplateID = @BaseTemplateID
                                                         AND      ats1.AdStyle = ats2.AdStyle )";
                    break;

                case CheckRepeatTypeEnum.模板区域组重复://只比普通的GroupType =1
                    sql = @"SELECT  COUNT(1) FROM    dbo.SaleAreaInfo sai1
                                  WHERE   sai1.TemplateID = @NewTemplateID
                                  AND       sai1.GroupType = 1
                                  AND EXISTS ( SELECT 1 FROM   dbo.SaleAreaInfo sai2
                                                         WHERE  sai2.TemplateID = @BaseTemplateID
                                                         AND      sai2.GroupType = 1
                                                         AND      sai1.GroupName = sai2.GroupName)";
                    break;

                case CheckRepeatTypeEnum.模板区域组关联重复://只比普通的GroupType =1
                    sql = @"SELECT  COUNT(1) FROM    dbo.SaleAreaRelation sar1
                                  INNER JOIN dbo.SaleAreaInfo sai1 ON sar1.GroupID = sai1.GroupID
                                  WHERE   sai1.TemplateID = @NewTemplateID
                                  AND       sai1.GroupType = 1
                                  AND EXISTS ( SELECT * FROM   dbo.SaleAreaRelation sar2
                                                         INNER JOIN dbo.SaleAreaInfo sai2 ON sar2.GroupID = sai2.GroupID
                                                         WHERE  sai2.TemplateID = @BaseTemplateID
                                                         AND      sai2.GroupType = 1
                                                         AND      sar1.CityID = sar2.CityID )";
                    break;
            }
            var rowcount = Convert.ToInt32(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters));
            return rowcount > 0;
        }

        public bool CheckCanAddModifyTemplate(int baseTemplateID, string rightSql)
        {
            string sql = @"SELECT COUNT(1) FROM dbo.App_AdTemplate
                                    WHERE BaseAdID = @BaseAdID
                                    AND AuditStatus IN (48001,48003) AND Status = 0" + rightSql;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@BaseAdID", baseTemplateID),
            };
            return Convert.ToInt32(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters)) == 0;
        }

        public Entities.AdTemplate.AppAdTemplate GetNextWaittingAudit(int templateID)
        {
            string sql = @"SELECT TOP 1 * FROM dbo.App_AdTemplate
                                    WHERE Status = 0
                                    AND RecID <> @TemplateID
                                    AND AuditStatus = 48001
                                    ORDER BY CreateTime DESC
                                ";
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@TemplateID",templateID)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return DataTableToEntity<Entities.AdTemplate.AppAdTemplate>(ds.Tables[0]);
        }
    }
}