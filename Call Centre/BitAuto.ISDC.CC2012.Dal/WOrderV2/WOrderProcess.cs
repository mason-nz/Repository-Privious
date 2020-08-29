using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.Utils.Data;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class WOrderProcess : DataBase
    {
        public static WOrderProcess Instance = new WOrderProcess();

        /// ���ݹ���ID��ȡ�����¼
        /// <summary>
        /// ���ݹ���ID��ȡ�����¼
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public List<WOrderProcessInfo> GetWOrderProcessByOrderID(string orderID)
        {
            string sql = @"SELECT 
            RecID, OrderID, ProcessType, WorkOrderStatus, IsReturnVisit, ProcessContent, Status, 
            CreateUserID, CreateUserNum, CreateUserName, CreateUserDeptName, CreateTime
            FROM WOrderProcess 
            WHERE OrderID='" + SqlFilter(orderID) + @"'    
            AND WorkOrderStatus NOT IN(" + (int)WorkOrderStatus.Pending + "," + (int)WorkOrderStatus.Completed + @")      
            ORDER BY CreateTime DESC";

            List<WOrderProcessInfo> list = new List<WOrderProcessInfo>();
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new WOrderProcessInfo(dr));
            }
            return list;

        }
        /// �Ƿ��Ѿ��ط�
        /// <summary>
        /// �Ƿ��Ѿ��ط�
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public bool IsHasReturnForProcess(string orderid)
        {
            string sql = @"SELECT TOP 1 1 FROM dbo.WOrderProcess WHERE OrderID='" + SqlFilter(orderid) + "' AND ProcessType=" + (int)WOrderOperTypeEnum.L05_�ط�;
            int a = CommonFunction.ObjectToInteger(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql));
            return a != 0;
        }
    }
}
