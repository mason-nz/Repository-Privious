using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Dal;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class WOrderToAndCC
    {
        public static WOrderToAndCC Instance = new WOrderToAndCC();

        /// �Ƿ��ǽ�����
        /// <summary>
        /// �Ƿ��ǽ�����
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool IsToPersonForNumber(string orderid, int lastrecid, WOrderProcessRightJsonData right)
        {
            if (right.RightType_Out == WOrderProcessRightTypeEnum.R04_����Ȩ��)
            {
                right.RightData = BLL.Util.CheckRightForCRM(right.LoginUserID, right.RightData_Out).ToString();
            }
            return Dal.WOrderToAndCC.Instance.IsToPersonForNumber(orderid, lastrecid, right.RightType_Out, right.LoginUserID, right.RightData_Out);
        }
        /// ��ӳ����˻������
        /// <summary>
        /// ��ӳ����˻������
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="recid"></param>
        /// <param name="type"></param>
        /// <param name="userid"></param>
        /// <param name="num"></param>
        /// <param name="name"></param>
        /// <param name="loginuserid"></param>
        public void SaveWOrderToAndCC(string orderid, int recid, WOrderPersonTypeEnum type, int userid, string num, string name, int loginuserid)
        {
            WOrderToAndCCInfo info = new WOrderToAndCCInfo();
            info.OrderID = orderid;
            info.ReceiverID = recid;
            info.PersonType = (int)type;
            info.UserID = userid;
            info.UserNum = num;
            info.UserName = name;
            //����
            info.Status = 0;
            info.CreateTime = DateTime.Now;
            info.CreateUserID = loginuserid;

            CommonBll.Instance.InsertComAdoInfo(info);
        }

        /// ���ݹ���ID��ѯ������
        /// <summary>
        /// ���ݹ���ID��ѯ������
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public List<WOrderToAndCCInfo> GetReceiverPeopleByOrderID(string orderID)
        {
            return Dal.WOrderToAndCC.Instance.GetReceiverPeopleByOrderID(orderID);
        }
    }
}
