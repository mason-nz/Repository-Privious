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
    /// 数据访问类CallEventActionLog。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-01-28 05:24:27 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CallEventActionLog : DataBase
    {
        #region Instance
        public static readonly CallEventActionLog Instance = new CallEventActionLog();
        #endregion

        #region const
        private const string P_CALLEVENTACTIONLOG_SELECT = "p_CallEventActionLog_Select";
        private const string P_CALLEVENTACTIONLOG_INSERT = "p_CallEventActionLog_Insert";
        private const string P_CALLEVENTACTIONLOG_UPDATE = "p_CallEventActionLog_Update";
        private const string P_CALLEVENTACTIONLOG_DELETE = "p_CallEventActionLog_Delete";
        #endregion

        #region Contructor
        protected CallEventActionLog()
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
        public DataTable GetCallEventActionLog(string where, string order, int currentPage, int pageSize, out int totalCount)
        {
            //string where = string.Empty;

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CALLEVENTACTIONLOG_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.CallEventActionLog GetCallEventActionLog(int RecID)
        {
            //QueryCallEventActionLog query = new QueryCallEventActionLog();
            //query.RecID = RecID;
            string where = " And RecID="+RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetCallEventActionLog(where, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleCallEventActionLog(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.CallEventActionLog LoadSingleCallEventActionLog(DataRow row)
        {
            Entities.CallEventActionLog model = new Entities.CallEventActionLog();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            model.EventName = row["EventName"].ToString();
            model.SessionID = row["SessionID"].ToString();
            model.Loginfo = row["Loginfo"].ToString();
            if (row["UserID"].ToString() != "")
            {
                model.UserID = int.Parse(row["UserID"].ToString());
            }
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
        public void Insert(Entities.CallEventActionLog model)
        {
            string sqlStr = "INSERT CallEventActionLog VALUES (@EventName,@SessionID,@Loginfo,@UserID,@CreateTime)";
            SqlParameter[] parameters = {
					new SqlParameter("@EventName", SqlDbType.NVarChar,200),
					new SqlParameter("@SessionID", SqlDbType.NVarChar,200),
					new SqlParameter("@Loginfo", SqlDbType.VarChar,3000),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Value = model.EventName;
            parameters[1].Value = model.SessionID;
            parameters[2].Value = model.Loginfo;
            parameters[3].Value = model.UserID;
            parameters[4].Value = model.CreateTime;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameters);
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.CallEventActionLog model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@EventName", SqlDbType.NVarChar,200),
					new SqlParameter("@SessionID", SqlDbType.NVarChar,200),
					new SqlParameter("@Loginfo", SqlDbType.VarChar,3000),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.EventName;
            parameters[2].Value = model.SessionID;
            parameters[3].Value = model.Loginfo;
            parameters[4].Value = model.UserID;
            parameters[5].Value = model.CreateTime;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CALLEVENTACTIONLOG_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.CallEventActionLog model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@EventName", SqlDbType.NVarChar,200),
					new SqlParameter("@SessionID", SqlDbType.NVarChar,200),
					new SqlParameter("@Loginfo", SqlDbType.VarChar,3000),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.EventName;
            parameters[2].Value = model.SessionID;
            parameters[3].Value = model.Loginfo;
            parameters[4].Value = model.UserID;
            parameters[5].Value = model.CreateTime;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CALLEVENTACTIONLOG_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.CallEventActionLog model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@EventName", SqlDbType.NVarChar,200),
					new SqlParameter("@SessionID", SqlDbType.NVarChar,200),
					new SqlParameter("@Loginfo", SqlDbType.VarChar,3000),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.EventName;
            parameters[2].Value = model.SessionID;
            parameters[3].Value = model.Loginfo;
            parameters[4].Value = model.UserID;
            parameters[5].Value = model.CreateTime;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CALLEVENTACTIONLOG_UPDATE, parameters);
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

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CALLEVENTACTIONLOG_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CALLEVENTACTIONLOG_DELETE, parameters);
        }
        #endregion

    }
}

