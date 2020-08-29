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

        /// ����������ݱ�
        /// <summary>
        /// ����������ݱ�
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="receiverid"></param>
        /// <param name="callid"></param>
        /// <param name="loginuserid"></param>
        public void InsertWOrderDataForCC(string orderid, int receiverid, long callid, int loginuserid)
        {
            //�����ݿ⵼����������
            Dal.WOrderData.Instance.ImportDataFromCC(orderid, receiverid, callid, loginuserid);
        }
        /// ����������ݱ�
        /// <summary>
        /// ����������ݱ�
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="receiverid"></param>
        /// <param name="csid"></param>
        /// <param name="loginuserid"></param>
        public void InserWOrderDataForIM(string orderid, int receiverid, long csid, int loginuserid)
        {
            //�����ݿ⵼����������
            Dal.WOrderData.Instance.ImportDataFromIM(orderid, receiverid, csid, loginuserid);
        }
        /// ���ݹ���ID��ѯ��������
        /// <summary>
        /// ���ݹ���ID��ѯ��������
        /// </summary>
        /// <param name="OrderID">����ID</param>
        /// <param name="isWorkOrder">�Ƿ񹤵�����ѯ��������ʱΪtrue����ѯ�����¼ʱΪfalse</param>
        /// <returns></returns>
        public List<WOrderDataInfo> GetCallReportByOrderID(string OrderID, bool isWorkOrder)
        {
            return Dal.WOrderData.Instance.GetCallReportByOrderID(OrderID, isWorkOrder);
        }
        /// ����CSID��ȡ�Ի�������
        /// <summary>
        /// ����CSID��ȡ�Ի�������
        /// </summary>
        /// <param name="csid"></param>
        /// <returns></returns>
        public int GetDailogCount(long csid)
        {
            return Dal.WOrderData.Instance.GetDailogCount(csid);
        }
    }
}
