using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class ListenAgentLog
    {
        public static ListenAgentLog Instance = new ListenAgentLog();

        /// 插入一条监控日志
        /// <summary>
        /// 插入一条监控日志
        /// </summary>
        /// <param name="ListenUserID"></param>
        /// <param name="AgentUserID"></param>
        /// <param name="CurrentStatus"></param>
        /// <param name="CurrentOper"></param>
        /// <param name="Vendor"></param>
        /// <param name="Remark"></param>
        /// <returns></returns>
        public int InsertListenAgentLog(
            int ListenUserID, string ListenUserName, string ListenNum, string ListenExtensionNum, OperForListen ListenOper,
            int AgentUserID, string AgentUserName, string AgentNum, string AgentExtensionNum, AgentStateForListen AgentStatus,
            Vender Vendor, string Remark)
        {
            ListenAgentLogInfo info = new ListenAgentLogInfo();
            info.ListenUserID = ListenUserID;
            info.ListenUserName = ListenUserName;
            info.ListenNum = ListenNum;
            info.ListenExtensionNum = ListenExtensionNum;
            info.ListenOper = (int)ListenOper;

            info.AgentUserID = AgentUserID;
            info.AgentUserName = AgentUserName;
            info.AgentNum = AgentNum;
            info.AgentExtensionNum = AgentExtensionNum;
            info.AgentStatus = (int)AgentStatus;

            info.Vendor = (int)Vendor;
            info.Remark = Remark;

            info.CreateTime = DateTime.Now;
            info.CreateUserID = ListenUserID;

            CommonBll.Instance.InsertComAdoInfo(info);
            return info.ValueOrDefault_RecID;
        }
    }
}
