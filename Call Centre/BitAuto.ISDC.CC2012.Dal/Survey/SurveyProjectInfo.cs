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
    /// 数据访问类SurveyProjectInfo。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-10-24 10:32:18 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class SurveyProjectInfo : DataBase
    {
        #region Instance
        public static readonly SurveyProjectInfo Instance = new SurveyProjectInfo();
        #endregion

        #region const
        private const string P_SURVEYPROJECTINFO_SELECT = "p_SurveyProjectInfo_Select";
        private const string P_SURVEYPROJECTINFO_INSERT = "p_SurveyProjectInfo_Insert";
        private const string P_SURVEYPROJECTINFO_UPDATE = "p_SurveyProjectInfo_Update";
        private const string P_SURVEYPROJECTINFO_DELETE = "p_SurveyProjectInfo_Delete";
        #endregion

        #region Contructor
        protected SurveyProjectInfo()
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
        public DataTable GetSurveyProjectInfo(QuerySurveyProjectInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;


            if (query.SPIID != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                where += " AND spi.SPIID=" + query.SPIID;
            }
            if (query.Name != Entities.Constants.Constant.STRING_INVALID_VALUE)
            {
                where += " AND spi.Name LIKE '%" + Utils.StringHelper.SqlFilter(query.Name) + "%'";
            }
            if (query.BusinessGroup != Entities.Constants.Constant.STRING_INVALID_VALUE)
            {
                where += " AND spi.BusinessGroup LIKE '%" + Utils.StringHelper.SqlFilter(query.BusinessGroup) + "%'";
            }
            if (query.BGID != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                where += " AND spi.BGID=" + query.BGID;
            }
            if (query.SCID != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                where += " AND spi.SCID=" + query.SCID;
            }
            if (query.StatusStr != Entities.Constants.Constant.STRING_INVALID_VALUE)
            {
                string[] statusArry = query.StatusStr.Split(',');
                where += " And (";
                int i = 0;
                foreach (string status in statusArry)
                {
                    if (i > 0)
                    {
                        where += " OR ";
                    }
                    switch (status)
                    {
                        case "0"://未开始
                            where += "( spi.SurveyStartTime>getdate() )";
                            break;
                        case "1"://进行中
                            where += "( spi.SurveyStartTime<=getdate() And spi.SurveyEndTime>=getdate() )";
                            break;
                        case "2"://已结束
                            where += "( spi.SurveyEndTime<getdate() And spi.status=0 )";
                            break;
                    }
                    i++;
                }
                where += ")";
            }
            if (query.BeginCreateTime != Entities.Constants.Constant.DATE_INVALID_VALUE)
            {
                where += " AND spi.CreateTime>='" + query.BeginCreateTime + "'";
            }
            if (query.EndCreateTime != Entities.Constants.Constant.DATE_INVALID_VALUE)
            {
                where += " AND spi.CreateTime<='" + query.EndCreateTime + "'";
            }
            if (query.CreateUserID != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                where += " AND spi.CreateUserID=" + query.CreateUserID;
            }
            if (query.LoginUserID != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                where += Dal.UserGroupDataRigth.Instance.GetSqlRightstr("spi", "BGID", "CreateUserID", (int)query.LoginUserID);
                //where += " And (EXISTS(SELECT * FROM UserDataRigth WHERE UserID=" + query.LoginUserID + " And RightType=2)";
                //where += " OR ((spi.BGID IN (SELECT BGID FROM UserGroupDataRigth WHERE UserID=" + query.LoginUserID + " And RightType=2))";
                //where += " OR (spi.BGID IN (SELECT BGID FROM UserGroupDataRigth WHERE UserID=" + query.LoginUserID + " And RightType=1) And spi.CreateUserID=" + query.LoginUserID + ")))";
            }
            where += " AND spi.Status >=0 ";

            //lxw 10.29
            if (query.SCIDStr != Entities.Constants.Constant.STRING_INVALID_VALUE)
            {
                where += " And spi.SCID in (" + Dal.Util.SqlFilterByInCondition(query.SCIDStr) + ")";
            }
            if (query.CreaterType == 1 && query.UserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND spi.SPIID IN (Select SPIID From SurveyPerson sp Where sp.SPIID=spi.SPIID AND sp.ExamPersonID=" + query.UserID + ")";
            }
            if (query.SurveyStartTime != Constant.DATE_INVALID_VALUE)
            {
                where += " AND spi.SurveyStartTime>='" + query.SurveyStartTime + "'";
            }
            if (query.SurveyEndTime != Constant.DATE_INVALID_VALUE)
            {
                where += " AND spi.SurveyEndTime<='" + query.SurveyEndTime + "'";
            }
            if (query.Status != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                where += " AND spi.Status=" + query.Status;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYPROJECTINFO_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// 获取项目所有创建人员
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllCreateUserID(int userId)
        {
            List<int> list = new List<int>();
            string sqlStr = "SELECT DISTINCT CreateUserID FROM SurveyProjectInfo";

            //            string sqlStr = @"
            //                SELECT DISTINCT a.CreateUserID FROM dbo.SurveyProjectInfo a
            //                 JOIN dbo.EmployeeAgent b ON a.CreateUserID = b.UserID
            //                 WHERE b.RegionID  =(SELECT RegionID FROM dbo.EmployeeAgent WHERE UserID=" + userId.ToString() + ")";

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(int.Parse(dr["CreateUserID"].ToString()));
                }
            }

            return list;
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.SurveyProjectInfo GetSurveyProjectInfo(int SPIID)
        {
            QuerySurveyProjectInfo query = new QuerySurveyProjectInfo();
            query.SPIID = SPIID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetSurveyProjectInfo(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleSurveyProjectInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.SurveyProjectInfo LoadSingleSurveyProjectInfo(DataRow row)
        {
            Entities.SurveyProjectInfo model = new Entities.SurveyProjectInfo();

            if (row["SPIID"].ToString() != "")
            {
                model.SPIID = int.Parse(row["SPIID"].ToString());
            }
            if (row["BGID"].ToString() != "")
            {
                model.BGID = int.Parse(row["BGID"].ToString());
            }
            if (row["SCID"].ToString() != "")
            {
                model.SCID = int.Parse(row["SCID"].ToString());
            }
            model.Name = row["Name"].ToString();
            model.Description = row["Description"].ToString();
            model.BusinessGroup = row["BusinessGroup"].ToString();
            if (row["SurveyStartTime"].ToString() != "")
            {
                model.SurveyStartTime = DateTime.Parse(row["SurveyStartTime"].ToString());
            }
            if (row["SurveyEndTime"].ToString() != "")
            {
                model.SurveyEndTime = DateTime.Parse(row["SurveyEndTime"].ToString());
            }
            if (row["SIID"].ToString() != "")
            {
                model.SIID = int.Parse(row["SIID"].ToString());
            }
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            if (row["ModifyTime"].ToString() != "")
            {
                model.ModifyTime = DateTime.Parse(row["ModifyTime"].ToString());
            }
            if (row["ModifyUserID"].ToString() != "")
            {
                model.ModifyUserID = int.Parse(row["ModifyUserID"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.SurveyProjectInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SPIID", SqlDbType.Int,4),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@Description", SqlDbType.NVarChar,500),
					new SqlParameter("@BusinessGroup", SqlDbType.NVarChar,50),
					new SqlParameter("@SurveyStartTime", SqlDbType.DateTime),
					new SqlParameter("@SurveyEndTime", SqlDbType.DateTime),
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.BGID;
            parameters[2].Value = model.SCID;
            parameters[3].Value = model.Name;
            parameters[4].Value = model.Description;
            parameters[5].Value = model.BusinessGroup;
            parameters[6].Value = model.SurveyStartTime;
            parameters[7].Value = model.SurveyEndTime;
            parameters[8].Value = model.SIID;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.CreateTime;
            parameters[11].Value = model.CreateUserID;
            parameters[12].Value = model.ModifyTime;
            parameters[13].Value = model.ModifyUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYPROJECTINFO_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.SurveyProjectInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SPIID", SqlDbType.Int,4),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@Description", SqlDbType.NVarChar,500),
					new SqlParameter("@BusinessGroup", SqlDbType.NVarChar,50),
					new SqlParameter("@SurveyStartTime", SqlDbType.DateTime),
					new SqlParameter("@SurveyEndTime", SqlDbType.DateTime),
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.BGID;
            parameters[2].Value = model.SCID;
            parameters[3].Value = model.Name;
            parameters[4].Value = model.Description;
            parameters[5].Value = model.BusinessGroup;
            parameters[6].Value = model.SurveyStartTime;
            parameters[7].Value = model.SurveyEndTime;
            parameters[8].Value = model.SIID;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.CreateTime;
            parameters[11].Value = model.CreateUserID;
            parameters[12].Value = model.ModifyTime;
            parameters[13].Value = model.ModifyUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYPROJECTINFO_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.SurveyProjectInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SPIID", SqlDbType.Int,4),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@Description", SqlDbType.NVarChar,500),
					new SqlParameter("@BusinessGroup", SqlDbType.NVarChar,50),
					new SqlParameter("@SurveyStartTime", SqlDbType.DateTime),
					new SqlParameter("@SurveyEndTime", SqlDbType.DateTime),
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.SPIID;
            parameters[1].Value = model.BGID;
            parameters[2].Value = model.SCID;
            parameters[3].Value = model.Name;
            parameters[4].Value = model.Description;
            parameters[5].Value = model.BusinessGroup;
            parameters[6].Value = model.SurveyStartTime;
            parameters[7].Value = model.SurveyEndTime;
            parameters[8].Value = model.SIID;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.CreateTime;
            parameters[11].Value = model.CreateUserID;
            parameters[12].Value = model.ModifyTime;
            parameters[13].Value = model.ModifyUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYPROJECTINFO_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.SurveyProjectInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SPIID", SqlDbType.Int,4),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@Description", SqlDbType.NVarChar,500),
					new SqlParameter("@BusinessGroup", SqlDbType.NVarChar,50),
					new SqlParameter("@SurveyStartTime", SqlDbType.DateTime),
					new SqlParameter("@SurveyEndTime", SqlDbType.DateTime),
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.SPIID;
            parameters[1].Value = model.BGID;
            parameters[2].Value = model.SCID;
            parameters[3].Value = model.Name;
            parameters[4].Value = model.Description;
            parameters[5].Value = model.BusinessGroup;
            parameters[6].Value = model.SurveyStartTime;
            parameters[7].Value = model.SurveyEndTime;
            parameters[8].Value = model.SIID;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.CreateTime;
            parameters[11].Value = model.CreateUserID;
            parameters[12].Value = model.ModifyTime;
            parameters[13].Value = model.ModifyUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYPROJECTINFO_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int SPIID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SPIID", SqlDbType.Int,4)};
            parameters[0].Value = SPIID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYPROJECTINFO_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int SPIID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SPIID", SqlDbType.Int,4)};
            parameters[0].Value = SPIID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYPROJECTINFO_DELETE, parameters);
        }
        #endregion

    }
}

