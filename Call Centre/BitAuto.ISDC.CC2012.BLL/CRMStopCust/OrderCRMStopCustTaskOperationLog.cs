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
    /// ҵ���߼���OrderCRMStopCustTaskOperationLog ��ժҪ˵����
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
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetOrderCRMStopCustTaskOperationLog(string taskid, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.OrderCRMStopCustTaskOperationLog.Instance.GetOrderCRMStopCustTaskOperationLog(taskid, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// ����һ������
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

