using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using System.Data.SqlClient;
using BitAuto.Utils.Data;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class AgentStateDetail : DataBase
    {

        #region Instance
        public static readonly AgentStateDetail Instance = new AgentStateDetail();
        #endregion

        #region const
        private const string P_GetDetail_SELECT = "P_GetCCStateDetail";

        #endregion

        #region Contructor
        protected AgentStateDetail()
        { }
        #endregion

        /// <summary>
        /// 按照查询条件查询  分页
        /// </summary>
        /// <param name="query">查询值对象，用来存放查询条件</param>        
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="totalCount">总行数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="loginUser">当前登录用户</param>
        /// <returns>销售网络集合</returns>
        public DataTable GetAgentsDetails(string strWhere, int isToday, int currentPage, int pageSize, int loginUser, out int totalCount)
        {
            totalCount = 0;
            SqlParameter[] parameters = {
                new SqlParameter("@whereS",strWhere),
			    new SqlParameter("@pageIndex", currentPage),
			    new SqlParameter("@pageSize", pageSize),			
			    new SqlParameter("@total", totalCount),
                new SqlParameter("@IsToday",isToday)   //增加是否今天的标识，确定是否联合历史数据
             };
            parameters[3].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_GetDetail_SELECT, parameters);

            totalCount = int.Parse(parameters[3].Value.ToString());

            return ds.Tables[0];
        }
    }
}
