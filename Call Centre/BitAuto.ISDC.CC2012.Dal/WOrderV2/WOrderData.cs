using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.Utils.Data;
using System.Data;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class WOrderData : DataBase
    {
        public static WOrderData Instance = new WOrderData();

        /// 导入冗余数据，补全数据
        /// <summary>
        /// 导入冗余数据，补全数据
        /// </summary>
        /// <param name="recid"></param>
        public void ImportDataFromCC(string orderid, int receiverid, long callid, int loginuserid)
        {
            string sql = @"INSERT WOrderData(OrderID,ReceiverID,DataType,DataID,StartTime,EndTime,TallTime,AudioURL,Status,CreateTime,CreateUserID)
                                    SELECT a.OrderID,a.ReceiverID,a.DataType,a.DataID,
                                    b.EstablishedTime,ISNULL(b.CustomerReleaseTime,b.AgentReleaseTime) AS EndTime,b.TallTime,b.AudioURL,
                                    0,GETDATE()," + loginuserid + @"
                                    FROM (
                                    SELECT 
                                    '" + orderid + "' AS OrderID," + receiverid + " AS ReceiverID," + (int)BusinessTypeEnum.CC + " AS DataType,'" + callid + @"' AS DataID
                                    ) AS a LEFT JOIN CallRecord_ORIG b ON a.DataID=CAST(b.CallID AS NVARCHAR)";
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }
        /// 导入冗余数据，补全数据
        /// <summary>
        /// 导入冗余数据，补全数据
        /// </summary>
        /// <param name="recid"></param>
        public void ImportDataFromIM(string orderid, int receiverid, long csid, int loginuserid)
        {
            string sql = @"INSERT WOrderData(OrderID,ReceiverID,DataType,DataID,StartTime,EndTime,TallTime,AudioURL,Status,CreateTime,CreateUserID)
                                    SELECT a.OrderID,a.ReceiverID,a.DataType,a.DataID,
                                    b.CreateTime,b.EndTime,DATEDIFF(SECOND,b.CreateTime,b.EndTime) AS TallTime,NULL AS AudioURL,
                                   0,GETDATE()," + loginuserid + @"
                                    FROM (
                                    SELECT 
                                    '" + orderid + "' AS OrderID," + receiverid + " AS ReceiverID," + (int)BusinessTypeEnum.IM + " AS DataType,'" + csid + @"' AS DataID
                                    ) AS a LEFT JOIN v_Conversations b ON a.DataID=CAST(b.CSID AS NVARCHAR)";
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
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
            string sql = @"SELECT   
            RecID, OrderID, ReceiverID, DataType, DataID, StartTime, EndTime, TallTime, AudioURL, Status, CreateUserID, CreateTime
            FROM WOrderData 
            WHERE  OrderID='" + OrderID + "'";
            if (isWorkOrder)
            {
                sql += " AND ReceiverID=-1 ";//工单时为-1
            }
            else
            {
                sql += " AND ReceiverID<>-1 ";//不等于-1时查询处理记录
            }
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            List<WOrderDataInfo> list = new List<WOrderDataInfo>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new WOrderDataInfo(dr));
                }
            }
            return list;
        }
        /// 根据CSID获取对话的数量
        /// <summary>
        /// 根据CSID获取对话的数量
        /// </summary>
        /// <param name="csid"></param>
        /// <returns></returns>
        public int GetDailogCount(long csid)
        {
            string strSql = "select SumDailog FROM dbo.v_Conversations WHERE CSID=" + csid; 
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text,strSql);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }
    }
}
