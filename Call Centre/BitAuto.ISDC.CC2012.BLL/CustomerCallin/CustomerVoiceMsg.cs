using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class CustomerVoiceMsg
    {
        public static CustomerVoiceMsg Instance = new CustomerVoiceMsg();

        /// 根据联络ID获取数据
        /// <summary>
        /// 根据联络ID获取数据
        /// </summary>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        public CustomerVoiceMsgInfo GetCustomerVoiceMsgInfo(string SessionID)
        {
            return Dal.CustomerVoiceMsg.Instance.GetCustomerVoiceMsgInfo(SessionID);
        }

        /// 分页查询专属客服未接来电
        /// <summary>
        /// 分页查询专属客服未接来电
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetExclusiveMissedCallingsData(ExclusiveMissedCalls query, string order, int currentPage, int pageSize, out int totalCount)
        {
            int userid = BLL.Util.GetLoginUserID();
            return Dal.CustomerVoiceMsg.Instance.GetExclusiveMissedCallingsData(query, order, currentPage, pageSize, out  totalCount, userid);
        }
        /// 分页查询数据
        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCustomerVoiceMsgData(QueryCustomerVoiceMsg query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.CustomerVoiceMsg.Instance.GetCustomerVoiceMsgData(query, order, currentPage, pageSize, out  totalCount);
        }
        /// 添加工单之后，更新未接来电表
        /// <summary>
        /// 添加工单之后，更新未接来电表
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="orderid"></param>
        /// <param name="custid"></param>
        public void UpdateCustomerVoiceMsgInfoAfterNewOrder(int recid, string orderid, string custid)
        {
            CustomerVoiceMsgInfo info = BitAuto.ISDC.CC2012.BLL.CommonBll.Instance.GetComAdoInfo<CustomerVoiceMsgInfo>(recid);
            if (info != null)
            {
                info.CustID = custid;
                info.OrderID = orderid;
                BitAuto.ISDC.CC2012.BLL.CommonBll.Instance.UpdateComAdoInfo(info);
            }
        }
    }
}
