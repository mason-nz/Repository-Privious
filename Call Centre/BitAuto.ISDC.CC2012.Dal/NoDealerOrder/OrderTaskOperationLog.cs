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
    /// 数据访问类OrderTaskOperationLog。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-21 10:33:33 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class OrderTaskOperationLog : DataBase
    {
        #region Instance
        public static readonly OrderTaskOperationLog Instance = new OrderTaskOperationLog();
        #endregion

        #region const
        private const string P_ORDERTASKOPERATIONLOG_SELECTHaveCall = "p_OrderTaskOperationLog_SelectHaveCall";
        private const string P_ORDERTASKOPERATIONLOG_SELECT = "p_OrderTaskOperationLog_Select";
        private const string P_ORDERTASKOPERATIONLOG_INSERT = "p_OrderTaskOperationLog_Insert";
        private const string P_ORDERTASKOPERATIONLOG_UPDATE = "p_OrderTaskOperationLog_Update";
        private const string P_ORDERTASKOPERATIONLOG_DELETE = "p_OrderTaskOperationLog_Delete";
        #endregion

        #region Contructor
        protected OrderTaskOperationLog()
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
        public DataTable GetOrderTaskOperationLog(QueryOrderTaskOperationLog query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.TaskID != Constant.INT_INVALID_VALUE)
            {
                where += " AND TaskID=" + query.TaskID;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERTASKOPERATIONLOG_SELECT, parameters);
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
        public DataTable GetOrderTaskOperationLogHaveCall(QueryOrderTaskOperationLog query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.TaskID != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.TaskID=" + query.TaskID;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERTASKOPERATIONLOG_SELECTHaveCall, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.OrderTaskOperationLog GetOrderTaskOperationLog(long RecID)
        {
            QueryOrderTaskOperationLog query = new QueryOrderTaskOperationLog();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetOrderTaskOperationLog(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleOrderTaskOperationLog(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.OrderTaskOperationLog LoadSingleOrderTaskOperationLog(DataRow row)
        {
            Entities.OrderTaskOperationLog model = new Entities.OrderTaskOperationLog();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = long.Parse(row["RecID"].ToString());
            }
            if (row["TaskID"].ToString() != "")
            {
                model.TaskID = long.Parse(row["TaskID"].ToString());
            }
            if (row["OperationStatus"].ToString() != "")
            {
                model.OperationStatus = int.Parse(row["OperationStatus"].ToString());
            }
            if (row["TaskStatus"].ToString() != "")
            {
                model.TaskStatus = int.Parse(row["TaskStatus"].ToString());
            }
            model.Remark = row["Remark"].ToString();
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
        public int Insert(Entities.OrderTaskOperationLog model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,8),
					new SqlParameter("@TaskID", SqlDbType.Int,8),
					new SqlParameter("@OperationStatus", SqlDbType.Int,4),
					new SqlParameter("@TaskStatus", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.NVarChar,1000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                                        new SqlParameter("@CallRecordID", SqlDbType.BigInt,8)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.TaskID;
            parameters[2].Value = model.OperationStatus;
            parameters[3].Value = model.TaskStatus;
            parameters[4].Value = model.Remark;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.CallRecordID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERTASKOPERATIONLOG_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.OrderTaskOperationLog model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,8),
					new SqlParameter("@TaskID", SqlDbType.Int,8),
					new SqlParameter("@OperationStatus", SqlDbType.Int,4),
					new SqlParameter("@TaskStatus", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.NVarChar,1000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                                         new SqlParameter("@CallRecordID", SqlDbType.BigInt,8)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.TaskID;
            parameters[2].Value = model.OperationStatus;
            parameters[3].Value = model.TaskStatus;
            parameters[4].Value = model.Remark;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.CallRecordID;
            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_ORDERTASKOPERATIONLOG_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.OrderTaskOperationLog model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@TaskID", SqlDbType.BigInt,8),
					new SqlParameter("@OperationStatus", SqlDbType.Int,4),
					new SqlParameter("@TaskStatus", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.NVarChar,1000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                    new SqlParameter("@CallRecordID", SqlDbType.BigInt,8)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.TaskID;
            parameters[2].Value = model.OperationStatus;
            parameters[3].Value = model.TaskStatus;
            parameters[4].Value = model.Remark;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.CallRecordID;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERTASKOPERATIONLOG_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.OrderTaskOperationLog model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@TaskID", SqlDbType.BigInt,8),
					new SqlParameter("@OperationStatus", SqlDbType.Int,4),
					new SqlParameter("@TaskStatus", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.NVarChar,1000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                    new SqlParameter("@CallRecordID", SqlDbType.BigInt,8)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.TaskID;
            parameters[2].Value = model.OperationStatus;
            parameters[3].Value = model.TaskStatus;
            parameters[4].Value = model.Remark;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.CallRecordID;
            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_ORDERTASKOPERATIONLOG_UPDATE, parameters);
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

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERTASKOPERATIONLOG_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERTASKOPERATIONLOG_DELETE, parameters);
        }
        #endregion

    }
}

