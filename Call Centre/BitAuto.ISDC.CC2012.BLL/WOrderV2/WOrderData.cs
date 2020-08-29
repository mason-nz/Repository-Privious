using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Dal;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class WOrderData
    {
        public static WOrderData Instance = new WOrderData();

        /// 插入关联数据表
        /// <summary>
        /// 插入关联数据表
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="receiverid"></param>
        /// <param name="callid"></param>
        /// <param name="loginuserid"></param>
        public void InsertWOrderDataForCC(string orderid, int receiverid, long callid, int loginuserid)
        {
            //从数据库导入冗余数据
            Dal.WOrderData.Instance.ImportDataFromCC(orderid, receiverid, callid, loginuserid);
        }
        /// 插入关联数据表
        /// <summary>
        /// 插入关联数据表
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="receiverid"></param>
        /// <param name="csid"></param>
        /// <param name="loginuserid"></param>
        public void InserWOrderDataForIM(string orderid, int receiverid, long csid, int loginuserid)
        {
            //从数据库导入冗余数据
            Dal.WOrderData.Instance.ImportDataFromIM(orderid, receiverid, csid, loginuserid);
        }
        /// 根据工单ID查询话务数据
        /// <summary>
        /// 根据工单ID查询话务数据
        /// </summary>
        /// <param name="OrderID">工单ID</param>
        /// <param name="isWorkOrder">是否工单，查询工单话务时为true，查询处理记录时为false</param>
        /// <returns></returns>
        public List<WOrderDataInfo> GetCallReportByOrderID(string OrderID, bool isWorkOrder)
        {
            return Dal.WOrderData.Instance.GetCallReportByOrderID(OrderID, isWorkOrder);
        }
        /// 根据CSID获取对话的数量
        /// <summary>
        /// 根据CSID获取对话的数量
        /// </summary>
        /// <param name="csid"></param>
        /// <returns></returns>
        public int GetDailogCount(long csid)
        {
            return Dal.WOrderData.Instance.GetDailogCount(csid);
        }
    }
}
