using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.Utils.Data;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class WOrderToAndCC : DataBase
    {
        public static WOrderToAndCC Instance = new WOrderToAndCC();

        /// 是否是接收人
        /// <summary>
        /// 是否是接收人
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool IsToPersonForNumber(string orderid, int lastrecid, WOrderProcessRightTypeEnum righttype, int loginuserid, string rightdata)
        {
            string where = "";
            switch (righttype)
            {
                case WOrderProcessRightTypeEnum.R01_人员ID:
                    //R01_人员ID = 1：登录人的userid
                    where = " AND UserID='" + loginuserid + "'";
                    break;
                case WOrderProcessRightTypeEnum.R02_部门ID:
                    //R02_部门ID = 2：登录人的部门id
                    where = " AND UserID IN (SELECT UserID FROM dbo.v_userinfo WHERE DepartID IN (SELECT id FROM Crm2009.[dbo].[f_GetChildDepartid]('" + SqlFilter(rightdata) + "')))";
                    break;
                case WOrderProcessRightTypeEnum.R03_员工编号:
                    //R03_员工编号 = 3：登录人的员工编号
                    where = " AND UserNum='" + SqlFilter(rightdata) + "'";
                    break;
                case WOrderProcessRightTypeEnum.R04_功能权限:
                    //R04_功能权限 = 4：是否有处理权限
                    bool result = bool.Parse(rightdata);
                    if (result)
                    {
                        //有功能权限
                        return true;
                    }
                    else
                    {
                        //判断是否当前处理人
                        where = " AND UserID='" + loginuserid + "'";
                    }
                    break;
                default:
                    return false;
            }
            string sql = @"SELECT TOP 1 1 FROM WOrderToAndCC 
                WHERE OrderID='" + SqlFilter(orderid) + @"' 
                AND ReceiverID='" + lastrecid + @"' 
                AND PersonType=" + (int)WOrderPersonTypeEnum.P01_接收人
                + where;
            int a = CommonFunction.ObjectToInteger(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql));
            return a != 0;
        }

        /// 根据工单ID查询接收人
        /// <summary>
        /// 根据工单ID查询接收人
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public List<WOrderToAndCCInfo> GetReceiverPeopleByOrderID(string orderID)
        {
            string sql = @"SELECT RecID, OrderID, ReceiverID, PersonType, UserNum, UserID, UserName, Status, CreateUserID, CreateTime
                FROM WOrderToAndCC WHERE OrderID='" + SqlFilter(orderID) + @"'
            AND PersonType=" + (int)WOrderPersonTypeEnum.P01_接收人 + @"  AND Status=0";

            List<WOrderToAndCCInfo> list = new List<WOrderToAndCCInfo>();
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new WOrderToAndCCInfo(dr));
            }
            return list;
        }
    }
}
