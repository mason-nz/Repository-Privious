using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.AdTemplate;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query.AdTemplate;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.AdTemplate
{
    //SaleAreaInfo
    public class SaleAreaInfo : DataBase
    {
        #region Instance

        public static readonly SaleAreaInfo Instance = new SaleAreaInfo();

        #endregion Instance

        public List<Entities.AdTemplate.SaleAreaInfo> GetList(AdTemplateQuery<Entities.AdTemplate.SaleAreaInfo> query)
        {
            string sql = @"SELECT TOP ({0}) SAI.GroupID ,
                                       SAI.GroupName ,
                                       SAI.TemplateID ,
                                       SAI.IsPublic ,
                                       SAI.GroupType ,
                                       SAI.CreateUserID ,
                                       SAI.CreateTime FROM DBO.SaleAreaInfo AS SAI WITH(NOLOCK)
                                    WHERE 1 =1
                        ";

            sql = string.Format(sql, query.PageSize);
            var paras = new List<SqlParameter>();

            if (query.TemplateId > Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND SAI.TemplateID = @TemplateID";
                paras.Add(new SqlParameter("@TemplateID", query.TemplateId));
            }
            if (query.GroupType > Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND SAI.GroupType = @GroupType";
                paras.Add(new SqlParameter("@GroupType", query.GroupType));
            }
            if (query.IsPublic > Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND SAI.IsPublic = @IsPublic";
                paras.Add(new SqlParameter("@IsPublic", query.IsPublic));
            }
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToList<Entities.AdTemplate.SaleAreaInfo>(data.Tables[0]);
        }

        /// <summary>
        /// 只查了城市组与对应的城市列表（没有包括全国，其他城市）
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public string GetListBy(int templateId)
        {
            var sql = @"
                        --城市组：是否是公共模板的数据纬度，精确到SaleAreaRelation

                        SELECT  STUFF(( SELECT  '|' + ( ( CAST(SAI.GroupID AS VARCHAR(15)) + ','
                                                                    + ISNULL(SAI.GroupName, '') + ','+ CAST(SAI.GroupType AS VARCHAR(15)) + ','
                                                                    + CAST(CAST(SAI.IsPublic AS INT) AS VARCHAR(15)) ) + '$='
                                                                    + ISNULL( ( SELECT  ( ( ISNULL(CAST(AI2.AreaID AS VARCHAR(15)),'')
                                                                                    + ',' + ISNULL(AI2.AreaName,'') + ',' + ISNULL( CAST(CAST(SAR.IsPublic AS INT) AS VARCHAR(15)),''))
												                                                    + '@='
                                                                                    + ( ISNULL(CAST(AI1.AreaID AS VARCHAR(15)),'')
                                                                                        + ',' + ISNULL(AI1.AreaName,'') ) )
                                            FROM    dbo.AreaInfo AS AI1 WITH ( NOLOCK )
                                                    LEFT JOIN dbo.AreaInfo AS AI2 WITH ( NOLOCK ) ON SAR.ProvinceID = AI2.AreaID
                                            WHERE   SAR.CityID = AI1.AreaID
                                            FOR
                                            XML PATH('')
                                            ),'' ))
                        FROM    dbo.SaleAreaInfo AS SAI WITH ( NOLOCK )
                                LEFT JOIN dbo.SaleAreaRelation AS SAR WITH ( NOLOCK ) ON SAI.GroupID = SAR.GroupID
                                WHERE SAI.TemplateID = {0}
		                        AND SAI.GroupType = 1
                        FOR
                        XML PATH('')
                        ), 1, 1, '')
                        ";
            sql = string.Format(sql, templateId);
            var paras = new List<SqlParameter>();
            var data = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());

            return data.ToString();
        }

        public void Insert_BulkCopyToDB(DataTable dt, int adTemplateId)
        {
            DeleteSaleAreaInfo(adTemplateId);
            SqlBulkCopyByDataTable(CONNECTIONSTRINGS, "SaleAreaInfo", dt);
        }

        public void Insert(List<Entities.AdTemplate.SaleAreaInfo> list)
        {
            if (list.Count == 0) return;
            var strSql = new StringBuilder();
            strSql.AppendFormat("DELETE FROM DBO.SaleAreaInfo WHERE TemplateID = {0} AND IsPublic = 0", list[0].TemplateID);
            strSql.Append("insert into SaleAreaInfo(");
            strSql.Append("GroupName,TemplateID,IsPublic,GroupType,CreateUserID,CreateTime");
            strSql.Append(") values ");
            list.ForEach(item =>
            {
                strSql.AppendFormat(" ( '{0}',{1},{2},{3},{4},getdate()", item.GroupName, item.TemplateID,
                    item.IsPublic == true ? 1 : 0, item.GroupType, item.CreateUserID);
                strSql.Append(" ),");
            });
            var sql = strSql.ToString().TrimEnd(',');
            var paras = new List<SqlParameter>();
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
        }

        public int Delete(int templateId)
        {
            var strSql = new StringBuilder();

            strSql.AppendFormat("DELETE FROM DBO.SaleAreaInfo WHERE TemplateID = {0} AND IsPublic = 0", templateId);
            var paras = new List<SqlParameter>();
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), paras.ToArray());
        }

        public int DeleteForGroupId(int groupId)
        {
            var strSql = new StringBuilder();

            strSql.AppendFormat("DELETE FROM DBO.SaleAreaInfo WHERE GroupID = {0} AND IsPublic = 0", groupId);
            strSql.AppendFormat("DELETE FROM DBO.SaleAreaRelation WHERE GroupID = {0} AND IsPublic = 0", groupId);
            var paras = new List<SqlParameter>();
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), paras.ToArray());
        }

        public List<Entities.Publish.AppPriceInfo> DeleteAdPrice(int templateId, string whereCondition)
        {
            var strSql = new StringBuilder();

            strSql.AppendFormat(@"UPDATE DBO.AppPriceInfo SET Status = -1
                                    OUTPUT Inserted.RecID ,
                                           Inserted.PubID,
                                           Inserted.TemplateID ,
                                           Inserted.MediaID
                                    WHERE TemplateID = {0}", templateId);
            if (!string.IsNullOrWhiteSpace(whereCondition))
            {
                strSql.AppendFormat(" AND ( 1= 1 {0})", whereCondition);
            }
            var paras = new List<SqlParameter>();
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), paras.ToArray());
            return DataTableToList<Entities.Publish.AppPriceInfo>(data.Tables[0]);
        }

        public int DeletePublishBasicInfo(List<int> pubIds)
        {
            if (pubIds.Count == 0)
                return 1;
            var strSql = new StringBuilder();

            strSql.AppendFormat(@"UPDATE  DBO.Publish_BasicInfo SET IsDel = -1
                                    WHERE PubID IN ({0}) ", string.Join(",", pubIds));
            var paras = new List<SqlParameter>();
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), paras.ToArray());
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.AdTemplate.SaleAreaInfo entity)
        {
            var strSql = new StringBuilder();

            //strSql.AppendFormat("DELETE FROM DBO.SaleAreaInfo WHERE TemplateID = {0} AND IsPublic = 0", entity.TemplateID);
            strSql.AppendLine();
            strSql.Append("insert into SaleAreaInfo(");
            strSql.Append("GroupName,TemplateID,IsPublic,GroupType,CreateUserID,CreateTime");
            strSql.Append(") values (");
            strSql.Append("@GroupName,@TemplateID,@IsPublic,@GroupType,@CreateUserID,getdate()");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@GroupName",entity.GroupName),
                        new SqlParameter("@TemplateID",entity.TemplateID),
                        new SqlParameter("@IsPublic", entity.IsPublic == true?1:0),
                        new SqlParameter("@GroupType",entity.GroupType),
                        new SqlParameter("@CreateUserID",entity.CreateUserID)
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public void Insert_SaleAreaRelation_BulkCopyToDB(DataTable dt, int groupId)
        {
            //DeleteSaleAreaRelation(groupId);
            SqlBulkCopyByDataTable(CONNECTIONSTRINGS, "SaleAreaRelation", dt);
        }

        public int DeleteSaleAreaInfo(int adTemplateId)
        {
            var sql = @"DELETE FROM DBO.SaleAreaInfo WHERE TemplateID = @TemplateID AND IsPublic = 0

                        DELETE FROM DBO.SaleAreaRelation
                        WHERE GroupID IN
                        (
                        SELECT GroupID FROM DBO.SaleAreaInfo WITH(NOLOCK)
                            WHERE TemplateID = @TemplateID1 AND IsPublic = 0
                        )
                        AND IsPublic = 0";
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("@TemplateID",adTemplateId),
                new SqlParameter("@TemplateID1",adTemplateId)
            };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
        }

        /// <summary>
        /// 删除套用公共模板下面，在公共模板城市组里面加了城市，删除当前用户的
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="userId"></param>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public int DeleteSaleAreaRelation(int groupId, int userId, int templateId)
        {
            var sql = @"DELETE FROM DBO.SaleAreaRelation WHERE GroupID = @GroupID AND IsPublic = 0 AND CreateUserID =@CreateUserID";
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("@GroupID",groupId),
                //new SqlParameter("@TemplateID",templateId),
                new SqlParameter("@CreateUserID",userId)
            };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
        }

        public int DeleteSaleAreaRelation(List<int> groupIds)
        {
            if (groupIds.Count == 0) return 1;
            var sql = string.Format(@"DELETE FROM DBO.SaleAreaRelation WHERE GroupID IN ({0}) AND IsPublic = 0 ", string.Join(",", groupIds));
            var paras = new List<SqlParameter>()
            {
            };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
        }

        public int GetAllCountryCitysCount()
        {
            var sql = @"
                    SELECT Count(*) FROM DBO.AreaInfo AS AI WITH(NOLOCK)
                    WHERE AI.PID > 0";
            var paras = new List<SqlParameter>();
            var data = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return Convert.ToInt32(data);
        }

        public List<AreaInfoEntity> GetAreaInfoList(int pid = -2, int areaId = -2)
        {
            var strSql = new StringBuilder();

            strSql.AppendFormat(@"  SELECT AreaID ,
                                           PID ,
                                           AreaName
                                    FROM DBO.AreaInfo WITH(NOLOCK) ");
            var paras = new List<SqlParameter>();
            if (pid > Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                strSql.Append(" AND PID = @PID");
                paras.Add(new SqlParameter("@PID", pid));
            }
            if (areaId > Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                strSql.Append(" AND AreaID = @AreaID");
                paras.Add(new SqlParameter("@AreaID", areaId));
            }

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), paras.ToArray());
            return DataTableToList<AreaInfoEntity>(data.Tables[0]);
        }

        public void InsertOtherCitys(int templateId, int createUserId, bool isPublic)
        {
            var sql = @"

                    IF(NOT EXISTS(SELECT 1 FROM DBO.SaleAreaInfo WHERE TemplateID = {0} AND GroupType = {1}))
                    BEGIN
	                    INSERT DBO.SaleAreaInfo
	                            ( GroupName ,
	                              TemplateID ,
	                              IsPublic ,
	                              GroupType ,
	                              CreateUserID ,
	                              CreateTime
	                            )
	                    VALUES  ( '其他城市' , -- GroupName - varchar(50)
	                              {0} , -- TemplateID - int
	                              {3} , -- IsPublic - bit
	                              {1} , -- GroupType - int
	                              {2} , -- CreateUserID - int
	                              GETDATE()  -- CreateTime - datetime
	                            )
                    END ";
            sql = string.Format(sql, templateId, (int)SaleAreaGroupTypeEnum.Other, createUserId, isPublic ? 1 : 0);
            var paras = new List<SqlParameter>()
            {
            };
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
        }

        public void InsertAllCountry(int templateId, int createUserId, int isPublic)
        {
            var sql = @"

                    IF(NOT EXISTS(SELECT 1 FROM DBO.SaleAreaInfo WHERE TemplateID = {0} AND GroupType = {1}))
                    BEGIN
	                    INSERT DBO.SaleAreaInfo
	                            ( GroupName ,
	                              TemplateID ,
	                              IsPublic ,
	                              GroupType ,
	                              CreateUserID ,
	                              CreateTime
	                            )
	                    VALUES  ( '全国' , -- GroupName - varchar(50)
	                              {0} , -- TemplateID - int
	                              {3} , -- IsPublic - bit
	                              {1} , -- GroupType - int
	                              {2} , -- CreateUserID - int
	                              GETDATE()  -- CreateTime - datetime
	                            )
                    END ";
            sql = string.Format(sql, templateId, (int)SaleAreaGroupTypeEnum.AllCountry, createUserId, isPublic);
            var paras = new List<SqlParameter>()
            {
            };
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
        }
    }
}