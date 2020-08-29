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

        /// �Ƿ��ǽ�����
        /// <summary>
        /// �Ƿ��ǽ�����
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool IsToPersonForNumber(string orderid, int lastrecid, WOrderProcessRightTypeEnum righttype, int loginuserid, string rightdata)
        {
            string where = "";
            switch (righttype)
            {
                case WOrderProcessRightTypeEnum.R01_��ԱID:
                    //R01_��ԱID = 1����¼�˵�userid
                    where = " AND UserID='" + loginuserid + "'";
                    break;
                case WOrderProcessRightTypeEnum.R02_����ID:
                    //R02_����ID = 2����¼�˵Ĳ���id
                    where = " AND UserID IN (SELECT UserID FROM dbo.v_userinfo WHERE DepartID IN (SELECT id FROM Crm2009.[dbo].[f_GetChildDepartid]('" + SqlFilter(rightdata) + "')))";
                    break;
                case WOrderProcessRightTypeEnum.R03_Ա�����:
                    //R03_Ա����� = 3����¼�˵�Ա�����
                    where = " AND UserNum='" + SqlFilter(rightdata) + "'";
                    break;
                case WOrderProcessRightTypeEnum.R04_����Ȩ��:
                    //R04_����Ȩ�� = 4���Ƿ��д���Ȩ��
                    bool result = bool.Parse(rightdata);
                    if (result)
                    {
                        //�й���Ȩ��
                        return true;
                    }
                    else
                    {
                        //�ж��Ƿ�ǰ������
                        where = " AND UserID='" + loginuserid + "'";
                    }
                    break;
                default:
                    return false;
            }
            string sql = @"SELECT TOP 1 1 FROM WOrderToAndCC 
                WHERE OrderID='" + SqlFilter(orderid) + @"' 
                AND ReceiverID='" + lastrecid + @"' 
                AND PersonType=" + (int)WOrderPersonTypeEnum.P01_������
                + where;
            int a = CommonFunction.ObjectToInteger(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql));
            return a != 0;
        }

        /// ���ݹ���ID��ѯ������
        /// <summary>
        /// ���ݹ���ID��ѯ������
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public List<WOrderToAndCCInfo> GetReceiverPeopleByOrderID(string orderID)
        {
            string sql = @"SELECT RecID, OrderID, ReceiverID, PersonType, UserNum, UserID, UserName, Status, CreateUserID, CreateTime
                FROM WOrderToAndCC WHERE OrderID='" + SqlFilter(orderID) + @"'
            AND PersonType=" + (int)WOrderPersonTypeEnum.P01_������ + @"  AND Status=0";

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
