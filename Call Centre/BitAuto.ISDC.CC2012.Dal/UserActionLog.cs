using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Dal
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 数据访问类UserActionLog。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-07-04 10:22:38 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class UserActionLog : DataBase
    {
        #region Instance
        public static readonly UserActionLog Instance = new UserActionLog();
        #endregion

        #region const
        private const string P_USERACTIONLOG_SELECT = "p_UserActionLog_Select";
        private const string P_USERACTIONLOG_INSERT = "p_UserActionLog_Insert";
        private const string P_USERACTIONLOG_UPDATE = "p_UserActionLog_Update";
        private const string P_USERACTIONLOG_DELETE = "p_UserActionLog_Delete";
        #endregion

        #region Contructor
        protected UserActionLog()
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
        public DataTable GetUserActionLog(QueryUserActionLog query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " AND RecID=" + query.RecID.ToString();
            }
            if (query.UserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND UserID=" + query.UserID.ToString();
            }
            if (query.IP != Constant.STRING_INVALID_VALUE)
            {
                where += " AND IP=" + StringHelper.SqlFilter(query.IP);
            }
            if (query.StartTime != Constant.STRING_INVALID_VALUE)
            {
                where += " AND CreateTime>='" + StringHelper.SqlFilter(query.StartTime) + " 0:00:00'";
            }
            if (query.EndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " AND CreateTime<='" + StringHelper.SqlFilter(query.EndTime) + " 23:59:59'";
            }
            if (query.Loginfo != Constant.STRING_EMPTY_VALUE && !string.IsNullOrEmpty(query.Loginfo))
            {
                where += " AND Loginfo like '%" + StringHelper.SqlFilter(query.Loginfo) + "%'";
            }
            if (query.UserEID != Constant.INT_INVALID_VALUE)
            {
                where += " AND UserEID = " + query.UserEID + "";
            }
            if (query.TrueName != Constant.STRING_EMPTY_VALUE)
            {
                where += " AND TrueName like '%" + StringHelper.SqlFilter(query.TrueName) + "%'";
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERACTIONLOG_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.UserActionLog GetUserActionLog(int RecID)
        {
            QueryUserActionLog query = new QueryUserActionLog();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetUserActionLog(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleUserActionLog(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.UserActionLog LoadSingleUserActionLog(DataRow row)
        {
            Entities.UserActionLog model = new Entities.UserActionLog();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            if (row["UserID"].ToString() != "")
            {
                model.UserID = int.Parse(row["UserID"].ToString());
            }
            if (row["UserEID"].ToString() != "")
            {
                model.UserEID = int.Parse(row["UserEID"].ToString());
            }
            model.TrueName = row["TrueName"].ToString();
            model.Loginfo = row["Loginfo"].ToString();
            model.IP = row["IP"].ToString();
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.UserActionLog model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@Loginfo", SqlDbType.VarChar,3000),
					new SqlParameter("@IP", SqlDbType.VarChar,20),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
                     new SqlParameter("@UserEID", SqlDbType.Int,4),
					new SqlParameter("@TrueName", SqlDbType.VarChar,200)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.Loginfo;
            parameters[3].Value = model.IP;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.UserEID;
            parameters[6].Value = model.TrueName;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERACTIONLOG_INSERT, parameters);
            return (int)parameters[0].Value;
        }

        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.UserActionLog model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@Loginfo", SqlDbType.VarChar,3000),
					new SqlParameter("@IP", SqlDbType.VarChar,20),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
                     new SqlParameter("@UserEID", SqlDbType.Int,4),
					new SqlParameter("@TrueName", SqlDbType.VarChar,200)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.Loginfo;
            parameters[3].Value = model.IP;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.UserEID;
            parameters[6].Value = model.TrueName;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_USERACTIONLOG_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.UserActionLog model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@Loginfo", SqlDbType.VarChar,3000),
					new SqlParameter("@IP", SqlDbType.VarChar,20),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
                     new SqlParameter("@UserEID", SqlDbType.Int,4),
					new SqlParameter("@TrueName", SqlDbType.VarChar,200)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.Loginfo;
            parameters[3].Value = model.IP;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.UserEID;
            parameters[6].Value = model.TrueName;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERACTIONLOG_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERACTIONLOG_DELETE, parameters);
        }
        #endregion
    }
}
