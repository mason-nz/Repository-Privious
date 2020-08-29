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
    /// 数据访问类OrderCRMStopCustTaskOperationLog。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-07-01 12:21:55 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class OrderCRMStopCustTaskOperationLog : DataBase
    {
        public static readonly OrderCRMStopCustTaskOperationLog Instance = new OrderCRMStopCustTaskOperationLog();
        private const string P_ORDERCRMSTOPCUSTTASKOPERATIONLOG_SELECT = "p_OrderCRMStopCustTaskOperationLog_Select";

        protected OrderCRMStopCustTaskOperationLog() { }

        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetOrderCRMStopCustTaskOperationLog(string taskid, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (!string.IsNullOrEmpty(taskid))
            {
                where += " and ocrl.TaskID='" + taskid + "'";
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERCRMSTOPCUSTTASKOPERATIONLOG_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        public DataTable GetListByTaskID(string taskID)
        {
            string sqlStr = "SELECT * FROM OrderCRMStopCustTaskOperationLog WHERE TaskID ='" + StringHelper.SqlFilter(taskID) + "' ORDER BY CreateTime Asc ";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            DataTable dt = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }
    }
}

