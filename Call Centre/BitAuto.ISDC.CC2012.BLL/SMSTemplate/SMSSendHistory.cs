using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 业务逻辑类SMSSendHistory 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-12-23 06:16:59 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class SMSSendHistory
    {
        public static readonly SMSSendHistory Instance = new SMSSendHistory();

        protected SMSSendHistory()
        { }

        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetSMSSendHistory(QuerySMSSendHistory query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.SMSSendHistory.Instance.GetSMSSendHistory(query, order, currentPage, pageSize, out totalCount);
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
        public DataTable GetSMSHistroyStatistics(QuerySMSSendHistory query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.SMSSendHistory.Instance.GetSMSHistroyStatistics(query, order, currentPage, pageSize, out totalCount);
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
        public DataTable GetSMSSendHistoryForExport(QuerySMSSendHistory query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.SMSSendHistory.Instance.GetSMSSendHistoryForExport(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.SMSSendHistory.Instance.GetSMSSendHistory(new QuerySMSSendHistory(), string.Empty, 1, 1000000, out totalCount);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Insert(Entities.SMSSendHistory model)
        {
            Dal.SMSSendHistory.Instance.Insert(model);
        }

        public int AddCustIdInfo(string SentSMSHistoryTel, string AddedCustID)
        {
            return Dal.SMSSendHistory.Instance.AddCustIdInfo(SentSMSHistoryTel, AddedCustID);
        }

        /// 绑定短信记录和任务id
        /// <summary>
        /// 绑定短信记录和任务id
        /// </summary>
        /// <param name="recids"></param>
        /// <param name="taskid"></param>
        public void BindSMSSendHistoryForTask(string recids, string taskid)
        {
            Dal.SMSSendHistory.Instance.BindSMSSendHistoryForTask(recids, taskid);
        }
    }
}

