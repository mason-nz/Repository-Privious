using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Dal
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 数据访问类SurveyInfo。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-10-24 10:32:17 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class SurveyInfo : DataBase
    {
        #region Instance
        public static readonly SurveyInfo Instance = new SurveyInfo();
        #endregion

        #region const
        private const string P_SURVEYINFO_SELECT = "p_SurveyInfo_Select";
        private const string P_SURVEYINFO_INSERT = "p_SurveyInfo_Insert";
        private const string P_SURVEYINFO_UPDATE = "p_SurveyInfo_Update";
        private const string P_SURVEYINFO_DELETE = "p_SurveyInfo_Delete";
        #endregion

        #region Contructor
        protected SurveyInfo()
        { }
        #endregion

        #region Select
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetSurveyInfo(QuerySurveyInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            //分组权限判断
            //if ((query.OwnGroup != Constant.STRING_INVALID_VALUE && query.OwnGroup != string.Empty) || (query.OneSelf != Constant.STRING_INVALID_VALUE && query.OneSelf != string.Empty))
            //{
            //    if (query.LoginID != Constant.INT_INVALID_VALUE)
            //    {
            //        where += " AND (";

            //        if (query.OwnGroup != Constant.STRING_INVALID_VALUE && query.OwnGroup != string.Empty)
            //        {
            //            //筛选登陆人管理的所属业务组权限是 本组 的信息
            //            where += " SurveyCategory.BGID IN ( " + query.OwnGroup + ") ";
            //        }

            //        if (query.OwnGroup != Constant.STRING_INVALID_VALUE && query.OwnGroup != string.Empty && query.OneSelf != Constant.STRING_INVALID_VALUE && query.OneSelf != string.Empty)
            //        {
            //            where += " OR ";
            //        }

            //        if (query.OneSelf != Constant.STRING_INVALID_VALUE && query.OneSelf != string.Empty)
            //        {
            //            //筛选登陆人管理的所属业务组权限是 本人 的信息 
            //            where += " (SurveyCategory.BGID IN (" + query.OneSelf + ") AND SurveyInfo.CreateUserID=" + query.LoginID + ")";
            //        }

            //        where += ")";
            //    }
            //}

            #region 数据权限判断
            if (query.LoginID != Constant.INT_INVALID_VALUE)
            {
                where = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("SurveyCategory", "SurveyInfo", "BGID", "CreateUserID", query.LoginID);
            }
            #endregion

            //状态：0-未完成；1-未使用；2-已使用（需要判断）
            if (query.Statuss != Constant.STRING_INVALID_VALUE)
            {
                if (query.Statuss.Contains("0") || query.Statuss.Contains("1") || query.Statuss.Contains("2"))
                {

                    string[] array_status = Dal.Util.SqlFilterByInCondition(query.Statuss).Split(',');

                    where += " AND (";

                    string whereStr = string.Empty;

                    for (int i = 0; i < array_status.Length; i++)
                    {
                        if (array_status[i] == "0")
                        {
                            whereStr += "OR ";

                            whereStr += " (SurveyInfo.Status = 0 ";

                            //并且在SurveyProjectInfo表中不存在，否则就是已使用的
                            whereStr += " AND SurveyInfo.SIID NOT IN (Select top 1 SIID From SurveyProjectInfo where SurveyProjectInfo.SIID = SurveyInfo.SIID) )";
                        }
                        if (array_status[i] == "1")
                        {
                            whereStr += "OR ";

                            whereStr += " (SurveyInfo.Status = 1";

                            //并且在SurveyProjectInfo表中不存在，否则就是已使用的
                            whereStr += @" AND SurveyInfo.SIID NOT IN (Select top 1 SIID From SurveyProjectInfo where SurveyProjectInfo.SIID = SurveyInfo.SIID 
                  UNION
                  SELECT TOP 1
                            SIID
                  FROM      ProjectSurveyMapping
                  WHERE     ProjectSurveyMapping.SIID = SurveyInfo.SIID) )";
                        }
                        if (array_status[i] == "2")
                        {
                            whereStr += "OR ";

                            whereStr += @" ( SurveyInfo.SIID IN (Select top 1 SIID From SurveyProjectInfo where SurveyProjectInfo.SIID = SurveyInfo.SIID
                  UNION
                  SELECT TOP 1
                            SIID
                  FROM      ProjectSurveyMapping
                  WHERE     ProjectSurveyMapping.SIID = SurveyInfo.SIID)";
                            //并且在SurveyInfo中的状态必须为1-未使用，这样才有可能在SurveyProjectInfo表中存在
                            whereStr += " AND SurveyInfo.Status = 1 )";

                        }

                        if (i == array_status.Length - 1)
                        {
                            where += whereStr.TrimStart('O', 'R');
                        }
                    }

                    where += ")";
                }

            }

            if (query.SIID != Constant.INT_INVALID_VALUE)
            {
                where += " AND SurveyInfo.SIID=" + query.SIID;
            }

            if (query.Name != Constant.STRING_INVALID_VALUE)
            {
                where += " AND SurveyInfo.Name like '%" + StringHelper.SqlFilter(query.Name) + "%'";
            }

            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " AND SurveyInfo.BGID=" + query.BGID;
            }

            if (query.SCID != Constant.INT_INVALID_VALUE)
            {
                where += " AND SurveyInfo.SCID=" + query.SCID;
            }

            if (query.IsAvailable != Constant.INT_INVALID_VALUE)
            {
                where += " AND SurveyInfo.IsAvailable =" + query.IsAvailable;
            }

            if (query.IsAvailables != Constant.STRING_INVALID_VALUE)
            {
                where += " AND SurveyInfo.IsAvailable IN (" + Dal.Util.SqlFilterByInCondition(query.IsAvailables) + ")";
            }

            if (query.CreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND SurveyInfo.CreateUserID =" + query.CreateUserID;
            }

            if (query.BeginTime != Constant.STRING_INVALID_VALUE)
            {
                where += " AND SurveyInfo.CreateTime >='" + StringHelper.SqlFilter(query.BeginTime) + " 0:0:0'";
            }

            if (query.EndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " AND SurveyInfo.CreateTime <='" + StringHelper.SqlFilter(query.EndTime) + " 23:59:59'";
            }

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYINFO_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.SurveyInfo GetSurveyInfo(int SIID)
        {
            QuerySurveyInfo query = new QuerySurveyInfo();
            query.SIID = SIID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetSurveyInfo(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleSurveyInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.SurveyInfo LoadSingleSurveyInfo(DataRow row)
        {
            Entities.SurveyInfo model = new Entities.SurveyInfo();

            if (row["SIID"].ToString() != "")
            {
                model.SIID = int.Parse(row["SIID"].ToString());
            }
            model.Name = row["Name"].ToString();
            if (row["BGID"].ToString() != "")
            {
                model.BGID = int.Parse(row["BGID"].ToString());
            }
            if (row["SCID"].ToString() != "")
            {
                model.SCID = int.Parse(row["SCID"].ToString());
            }
            model.Description = row["Description"].ToString();
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["IsAvailable"].ToString() != "")
            {
                model.IsAvailable = int.Parse(row["IsAvailable"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.SurveyInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@Description", SqlDbType.NVarChar,500),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@IsAvailable", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.BGID;
            parameters[3].Value = model.SCID;
            parameters[4].Value = model.Description;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.IsAvailable;
            parameters[7].Value = model.CreateTime;
            parameters[8].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYINFO_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.SurveyInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@Description", SqlDbType.NVarChar,500),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@IsAvailable", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.BGID;
            parameters[3].Value = model.SCID;
            parameters[4].Value = model.Description;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.IsAvailable;
            parameters[7].Value = model.CreateTime;
            parameters[8].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_SURVEYINFO_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.SurveyInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@Description", SqlDbType.NVarChar,500),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@IsAvailable", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.SIID;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.BGID;
            parameters[3].Value = model.SCID;
            parameters[4].Value = model.Description;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.IsAvailable;
            parameters[7].Value = model.CreateTime;
            parameters[8].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYINFO_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.SurveyInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@Description", SqlDbType.NVarChar,500),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@IsAvailable", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.SIID;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.BGID;
            parameters[3].Value = model.SCID;
            parameters[4].Value = model.Description;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.IsAvailable;
            parameters[7].Value = model.CreateTime;
            parameters[8].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_SURVEYINFO_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int SIID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SIID", SqlDbType.Int,4)};
            parameters[0].Value = SIID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYINFO_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int SIID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SIID", SqlDbType.Int,4)};
            parameters[0].Value = SIID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_SURVEYINFO_DELETE, parameters);
        }
        #endregion


        #region 获取所有创建人
        public DataTable getCreateUser()
        {

            //            string strSql = @"
            //                        SELECT DISTINCT a.CreateUserID FROM dbo.SurveyInfo a
            //                         JOIN dbo.EmployeeAgent b ON a.CreateUserID = b.UserID
            //                         WHERE b.RegionID  =(SELECT RegionID FROM dbo.EmployeeAgent WHERE UserID=" + userId.ToString() + ") ";
            //            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, "SELECT DISTINCT CreateUserID FROM dbo.SurveyInfo");
            return ds.Tables[0];
        }
        #endregion
    }
}

