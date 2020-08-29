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
    /// 数据访问类CustHistoryLog。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-07-27 10:30:14 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CustHistoryLog : DataBase
    {
        #region Instance
        public static readonly CustHistoryLog Instance = new CustHistoryLog();
        #endregion

        #region const
        private const string P_CustHistoryLog_SelectHaveCall = "p_CustHistoryLog_SelectHaveCall";
        private const string P_CUSTHISTORYLOG_SELECT = "p_CustHistoryLog_Select";
        private const string P_CUSTHISTORYLOG_INSERT = "p_CustHistoryLog_Insert";
        private const string P_CUSTHISTORYLOG_UPDATE = "p_CustHistoryLog_Update";
        private const string P_CUSTHISTORYLOG_DELETE = "p_CustHistoryLog_Delete";
        #endregion

        #region Contructor
        protected CustHistoryLog()
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
        public DataTable GetCustHistoryLogHaveCallRecord(QueryCustHistoryLog query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.TaskID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND a.TaskID='" + StringHelper.SqlFilter(query.TaskID) + "'";
            }
            if (query.Action != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.Action =" + query.Action;
            }
            if (query.ToNextSolveUserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.ToNextSolveUserID=" + query.ToNextSolveUserID;
            }
            if (query.ToNextSolveUserEID != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.ToNextSolveUserEID=" + query.ToNextSolveUserEID;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CustHistoryLog_SelectHaveCall, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetCustHistoryLog(QueryCustHistoryLog query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.TaskID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND TaskID='" + StringHelper.SqlFilter(query.TaskID) + "'";
            }
            if (query.Action != Constant.INT_INVALID_VALUE)
            {
                where += " AND Action =" + query.Action;
            }
            if (query.ToNextSolveUserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND ToNextSolveUserID=" + query.ToNextSolveUserID;
            }
            if (query.ToNextSolveUserEID != Constant.INT_INVALID_VALUE)
            {
                where += " AND ToNextSolveUserEID=" + query.ToNextSolveUserEID;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTHISTORYLOG_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.CustHistoryLog GetCustHistoryLog(long RecID)
        {
            QueryCustHistoryLog query = new QueryCustHistoryLog();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetCustHistoryLog(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleCustHistoryLog(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.CustHistoryLog LoadSingleCustHistoryLog(DataRow row)
        {
            Entities.CustHistoryLog model = new Entities.CustHistoryLog();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = long.Parse(row["RecID"].ToString());
            }
            model.TaskID = row["TaskID"].ToString();
            if (row["SolveUserEID"].ToString() != "")
            {
                model.SolveUserEID = int.Parse(row["SolveUserEID"].ToString());
            }
            if (row["SolveUserID"].ToString() != "")
            {
                model.SolveUserID = int.Parse(row["SolveUserID"].ToString());
            }
            if (row["SolveTime"].ToString() != "")
            {
                model.SolveTime = DateTime.Parse(row["SolveTime"].ToString());
            }
            model.Comment = row["Comment"].ToString();
            if (row["Action"].ToString() != "")
            {
                model.Action = int.Parse(row["Action"].ToString());
            }
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["Pid"].ToString() != "")
            {
                model.Pid = long.Parse(row["Pid"].ToString());
            }
            if (row["CallRecordID"].ToString() != "")
            {
                model.CallRecordID = long.Parse(row["CallRecordID"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public long Insert(Entities.CustHistoryLog model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@TaskID", SqlDbType.VarChar,20),
					new SqlParameter("@SolveUserEID", SqlDbType.Int,4),
					new SqlParameter("@SolveUserID", SqlDbType.Int,4),
					new SqlParameter("@SolveTime", SqlDbType.DateTime),
					new SqlParameter("@Comment", SqlDbType.NVarChar,2000),
					new SqlParameter("@Action", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@Pid", SqlDbType.BigInt,8),
					new SqlParameter("@ToNextSolveUserEID", SqlDbType.Int),
					new SqlParameter("@ToNextSolveUserID", SqlDbType.Int),
                                        new SqlParameter("@CallRecordID", SqlDbType.BigInt,8)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.TaskID;
            parameters[2].Value = model.SolveUserEID;
            parameters[3].Value = model.SolveUserID;
            parameters[4].Value = model.SolveTime;
            parameters[5].Value = model.Comment;
            parameters[6].Value = model.Action;
            parameters[7].Value = model.Status;
            parameters[8].Value = model.Pid;
            parameters[9].Value = model.ToNextSolveUserEID;
            parameters[10].Value = model.ToNextSolveUserID;
            parameters[11].Value = model.CallRecordID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTHISTORYLOG_INSERT, parameters);
            return (long)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.CustHistoryLog model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@TaskID", SqlDbType.VarChar,20),
					new SqlParameter("@SolveUserEID", SqlDbType.Int,4),
					new SqlParameter("@SolveUserID", SqlDbType.Int,4),
					new SqlParameter("@SolveTime", SqlDbType.DateTime),
					new SqlParameter("@Comment", SqlDbType.NVarChar,2000),
					new SqlParameter("@Action", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@Pid", SqlDbType.BigInt,8),
                    new SqlParameter("@ToNextSolveUserEID", SqlDbType.Int),
					new SqlParameter("@ToNextSolveUserID", SqlDbType.Int),
                                        new SqlParameter("@CallRecordID", SqlDbType.BigInt,8)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.TaskID;
            parameters[2].Value = model.SolveUserEID;
            parameters[3].Value = model.SolveUserID;
            parameters[4].Value = model.SolveTime;
            parameters[5].Value = model.Comment;
            parameters[6].Value = model.Action;
            parameters[7].Value = model.Status;
            parameters[8].Value = model.Pid;
            parameters[9].Value = model.ToNextSolveUserEID;
            parameters[10].Value = model.ToNextSolveUserID;
            parameters[11].Value = model.CallRecordID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTHISTORYLOG_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTHISTORYLOG_DELETE, parameters);
        }
        #endregion

    }
}

