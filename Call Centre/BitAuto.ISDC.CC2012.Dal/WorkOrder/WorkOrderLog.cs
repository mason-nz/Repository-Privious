using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using System.Data.SqlClient;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class WorkOrderLog : DataBase
    {
        #region Instance
        public static readonly WorkOrderLog Instance = new WorkOrderLog();
        #endregion

        #region const
        private const string P_WorkOrderLog_SELECT = "p_WorkOrderLog_Select";
        private const string P_WorkOrderLog_INSERT = "p_WorkOrderLog_Insert";
        private const string P_WorkOrderLog_UPDATE = "p_WorkOrderLog_Update";
        private const string P_WorkOrderLog_DELETE = "p_WorkOrderLog_Delete";
        #endregion

        #region Contructor
        protected WorkOrderLog()
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
        public DataTable GetWorkOrderLog(QueryWorkOrderLog query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            where = GetWhereStr(query);

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WorkOrderLog_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        private string GetWhereStr(Entities.QueryWorkOrderLog query)
        {
            string where = "";

            if (query.ReceiverRecID != Constant.INT_INVALID_VALUE)
            {
                where += " And l.ReceiverRecID=" + query.ReceiverRecID;
            }

            if (query.OrderID != Constant.STRING_EMPTY_VALUE)
            {
                where += " And l.OrderID='" + StringHelper.SqlFilter(query.OrderID) + "'";
            }


            return where;
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.WorkOrderLog GetWorkOrderLog(int RecID)
        {
            QueryWorkOrderLog query = new QueryWorkOrderLog();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetWorkOrderLog(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleWorkOrderLog(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        public Entities.WorkOrderLog LoadSingleWorkOrderLog(DataRow row)
        {
            Entities.WorkOrderLog model = new Entities.WorkOrderLog();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }

            model.OrderID = row["OrderID"].ToString();
            model.LogDesc = row["LogDesc"].ToString();

            if (row["ReceiverRecID"].ToString() != "")
            {
                model.ReceiverRecID = int.Parse(row["ReceiverRecID"].ToString());
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
        public int Insert(Entities.WorkOrderLog model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@OrderID", SqlDbType.NVarChar,20),			 
					new SqlParameter("@ReceiverRecID", SqlDbType.Int,4),				 
                    new SqlParameter("@LogDesc", SqlDbType.NVarChar,1000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.ReceiverRecID;
            parameters[3].Value = model.LogDesc;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WorkOrderLog_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.WorkOrderLog model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@OrderID", SqlDbType.NVarChar,20),			 
					new SqlParameter("@ReceiverRecID", SqlDbType.Int,4),				 
                    new SqlParameter("@LogDesc", SqlDbType.NVarChar,1000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.ReceiverRecID;
            parameters[3].Value = model.LogDesc;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_WorkOrderLog_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.WorkOrderLog model)
        {
            SqlParameter[] parameters = {			 
                     new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@OrderID", SqlDbType.NVarChar,20),			 
					new SqlParameter("@ReceiverRecID", SqlDbType.Int,4),				 
                    new SqlParameter("@LogDesc", SqlDbType.NVarChar,1000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.ReceiverRecID;
            parameters[3].Value = model.LogDesc;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WorkOrderLog_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.WorkOrderLog model)
        {
            SqlParameter[] parameters = {			 
					  new SqlParameter("@RecID", SqlDbType.Int,4),
                       new SqlParameter("@OrderID", SqlDbType.NVarChar,20),			 
					new SqlParameter("@ReceiverRecID", SqlDbType.Int,4),				 
                    new SqlParameter("@LogDesc", SqlDbType.NVarChar,1000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.ReceiverRecID;
            parameters[3].Value = model.LogDesc;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_WorkOrderLog_UPDATE, parameters);
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

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WorkOrderLog_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_WorkOrderLog_DELETE, parameters);
        }
        #endregion



        #region SelectByOrderID
        /// <summary>
        /// 根据工单号查询用到的标签
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public DataTable GetWorkOrderLogByOrderID(string orderid)
        {
            string sql = "";
            sql = "SELECT wot.RecID AS TagID,wot.TagName AS TagName FROM WorkOrderLog wot " +
                  "LEFT JOIN WorkOrderInfo worder ON worder.OrderID = wotm.OrderID " +
                  "WHERE worder.OrderID='" + StringHelper.SqlFilter(orderid) + "'";

            DataSet ds;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return ds.Tables[0];
        }

        #endregion


    }
}
