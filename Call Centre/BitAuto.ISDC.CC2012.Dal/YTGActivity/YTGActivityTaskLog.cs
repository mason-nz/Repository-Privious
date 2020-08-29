using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BitAuto.Utils.Data;
using BitAuto.Utils;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class YTGActivityTaskLog : DataBase
    {
        private YTGActivityTaskLog() { }
        private static YTGActivityTaskLog _instance = null;
        public static YTGActivityTaskLog Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new YTGActivityTaskLog();
                }
                return _instance;
            }
        }

        /// <summary>
        /// 按照查询条件查询操作日志数据
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetYTGActivityTaskOperationLog(YTGActivityTaskLogInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            DataSet ds;
            where += " and OperationStatus!=-2 and TaskStatus!=-2";
            if (query.TaskID != Constant.STRING_INVALID_VALUE)
            {
                where += " and TaskID='" + StringHelper.SqlFilter(query.TaskID) + "'";
            }

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_YTGActivityTaskOperationLog_Select", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

    }
}
