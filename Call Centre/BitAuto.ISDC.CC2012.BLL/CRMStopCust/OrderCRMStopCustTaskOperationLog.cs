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
    /// 业务逻辑类OrderCRMStopCustTaskOperationLog 的摘要说明。
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
    public class OrderCRMStopCustTaskOperationLog
    {
        public static readonly OrderCRMStopCustTaskOperationLog Instance = new OrderCRMStopCustTaskOperationLog();

        protected OrderCRMStopCustTaskOperationLog()
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
        public DataTable GetOrderCRMStopCustTaskOperationLog(string taskid, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.OrderCRMStopCustTaskOperationLog.Instance.GetOrderCRMStopCustTaskOperationLog(taskid, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Insert(OrderCRMStopCustTaskOperationLogInfo model)
        {
            CommonBll.Instance.InsertComAdoInfo(model);
        }
        public DataTable GetListByTaskID(string taskID)
        {
            return Dal.OrderCRMStopCustTaskOperationLog.Instance.GetListByTaskID(taskID);
        }
    }
}

