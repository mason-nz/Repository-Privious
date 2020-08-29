using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace CC2015_HollyFormsApp
{
    public class AgentTimeStateHelper
    {
        public static readonly AgentTimeStateHelper Instance = new AgentTimeStateHelper();
        CCWebAgentTimeState.AgentTimeStateSoapClient agentService = null;
        private string CallRecordAuthorizeCode = System.Configuration.ConfigurationManager.AppSettings["CallRecordAuthorizeCode"];//调用话务记录接口授权码

        protected AgentTimeStateHelper()
        {
            agentService = new CCWebAgentTimeState.AgentTimeStateSoapClient();
        }

        /// 根据域名获取用户ID
        /// <summary>
        /// 根据域名获取用户ID
        /// </summary>
        /// <param name="domainAccount">域名</param>
        /// <returns>获取用户ID(集中权限系统内)</returns>
        public int GetUserIDByNameDomainAccount(string domainAccount)
        {
            return agentService.GetUserIDByNameDomainAccount(CallRecordAuthorizeCode, domainAccount);
        }
        /// 根据用户ID获取所属分组及外呼出局号
        /// <summary>
        /// 根据用户ID获取所属分组及外呼出局号
        /// </summary>
        /// <param name="userId">用户ID(集中权限系统内)</param>
        /// <returns>返回如字符串（1,分组名称,0,8001）
        /// 其中，第1个为分组ID
        /// 第2个为分组名称
        /// 第3个为外呼出局号码
        /// 第4个为客服AgentID</returns>
        public string GetBGNameAndOutNum(int userId)
        {
            return agentService.GetBGNameAndOutNum(CallRecordAuthorizeCode, userId);
        }

        /// 获取登录用户的技能组
        /// <summary>
        /// 获取登录用户的技能组
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetSkillGroupByUserID()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (LoginUser.UserID.HasValue)
            {
                DataTable dt = agentService.GetUserSkillGroupIdAndPriorty(CallRecordAuthorizeCode, LoginUser.UserID.Value);
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dic[dr["SGID"].ToString()] = dr["SkillPriority"].ToString();
                    }
                }
            }
            return dic;
        }
        /// 获取字母ID根据数字ID
        /// <summary>
        /// 获取字母ID根据数字ID
        /// </summary>
        /// <param name="SGID"></param>
        /// <returns></returns>
        public string GetManufacturerSGIDBySGID(int SGID)
        {
            return agentService.GetManufactureSkillGroupIDBySGID(CallRecordAuthorizeCode, SGID);
        }
        /// 根据字母ID获取名称
        /// <summary>
        /// 根据字母ID获取名称
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public string GetSkillNameByMID(string MID)
        {
            return agentService.GetSkillNameByMID(CallRecordAuthorizeCode, MID);
        }
        /// 根据数字ID获取名称
        /// <summary>
        /// 根据数字ID获取名称
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public string GetSkillNameBySGID(int SGID)
        {
            return agentService.GetSkillNameBySGID(CallRecordAuthorizeCode, SGID);
        }

        /// 根据热线获取技能组
        /// <summary>
        /// 根据热线获取技能组
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        public DataTable GetHotlineSkillGroupByTelMainNum(string tel)
        {
            return agentService.GetHotlineSkillGroupByTelMainNum(CallRecordAuthorizeCode, tel);
        }
        /// 获取在线客服状态列表（置闲、置忙、自定义条件）
        /// <summary>
        /// 获取在线客服状态列表（置闲、置忙、自定义条件）
        /// </summary>
        /// <param name="vender"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetAgentStateOnLineByWhere(string where)
        {
            return agentService.GetAgentStateOnLineByWhere(CallRecordAuthorizeCode, CCWebAgentTimeState.Vender.Holly, where);
        }

        /// 获取在线客服状态列表（置闲、置忙）
        /// <summary>
        /// 获取在线客服状态列表（置闲、置忙）
        /// </summary>
        /// <param name="iarray"></param>
        /// <param name="errormsg"></param>
        /// <returns></returns>
        public bool GetAgentStateOnLine(out DataTable dt, out string errormsg)
        {
            return agentService.GetAgentStateOnLine(out dt, out errormsg, CallRecordAuthorizeCode, CCWebAgentTimeState.Vender.Holly);
        }
        /// 获取在线客服状态列表（工作中）
        /// <summary>
        /// 获取在线客服状态列表（工作中）
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="errormsg"></param>
        /// <returns></returns>
        public bool GetAgentStateWorking(out DataTable dt, out string errormsg)
        {
            return agentService.GetAgentStateWorking(out dt, out errormsg, CallRecordAuthorizeCode, CCWebAgentTimeState.Vender.Holly);
        }
        /// 根据用户UserID判断是否有监控权限
        /// <summary>
        /// 根据用户UserID判断是否有监控权限
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="sysID">CC系统编号</param>
        /// <returns></returns>
        public bool IsCanListenAgent(int userID, string sysID)
        {
            return agentService.IsCanListenAgent(CallRecordAuthorizeCode, userID, sysID);
        }

        /// 获取CC服务器当前时间
        /// <summary>
        /// 获取CC服务器当前时间
        /// </summary>
        /// <returns>返回DateTime类型</returns>
        public DateTime GetCurrentTime()
        {
            return agentService.GetCurrentTime(CallRecordAuthorizeCode);
        }

        /// 获取管辖分组
        /// <summary>
        /// 获取管辖分组
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetManageBusinessGroups(int userid)
        {
            return agentService.GetManageBusinessGroups(CallRecordAuthorizeCode, userid);
        }
        /// 获取全部的客服信息和分组信息
        /// <summary>
        /// 获取全部的客服信息和分组信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllEmployeeAgentAndBusinessGroup()
        {
            return agentService.GetAllEmployeeAgentAndBusinessGroup(CallRecordAuthorizeCode);
        }
        /// 插入监控日志
        /// <summary>
        /// 插入监控日志
        /// </summary>
        /// <param name="AgentUserID"></param>
        /// <param name="ExtensionNum"></param>
        /// <param name="CurrentStatus"></param>
        /// <param name="CurrentOper"></param>
        /// <param name="Remark"></param>
        /// <returns></returns>
        public void InsertListenAgentLog(OperForListen CurrentOper, int AgentUserID, string AgentUserName, string AgentNum, string AgentExtensionNum, AgentStateForListen AgentStatus)
        {
            string remark = "监控人[{0} ({1})]<{2}>了在[{3}]状态的客服[{4} ({5})]";
            remark = string.Format(remark, LoginUser.TrueName, LoginUser.ExtensionNum, CurrentOper.ToString(), AgentStatus.ToString(), AgentUserName, AgentExtensionNum);
            agentService.InsertListenAgentLogAsync(CallRecordAuthorizeCode,
                LoginUser.UserID.Value, LoginUser.TrueName, LoginUser.AgentNum, LoginUser.ExtensionNum, (int)CurrentOper,
                AgentUserID, AgentUserName, AgentNum, AgentExtensionNum, (int)AgentStatus,
                CCWebAgentTimeState.Vender.Holly, remark);
        }
        /// 发送日志到服务器端
        /// <summary>
        /// 发送日志到服务器端
        /// </summary>
        /// <param name="path"></param>
        /// <param name="logmsg"></param>
        public void SendLogToServerAsync(string path, string logmsg)
        {
            agentService.SendLogToServerAsync(CallRecordAuthorizeCode, path, logmsg);
        }

        #region CAgent
        /// 插入临时记录
        /// <summary>
        /// 插入临时记录
        /// </summary>
        /// <param name="state"></param>
        /// <param name="agentAuxState"></param>
        /// <param name="callType"></param>
        /// <param name="agTime"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        public bool InsertAgentState2DB(AgentState state, BusyStatus agentAuxState, Calltype callType, int agTime, DateTime startTime)
        {
            int busystate = (int)agentAuxState;
            if (state != AgentState.AS4_置忙)
            {
                busystate = -1;
            }
            return agentService.InsertAgentState2DB(CallRecordAuthorizeCode, (int)state, busystate, (int)callType, agTime, startTime, LoginUser.UserID.ToString(), LoginUser.TrueName, LoginUser.BGID, LoginUser.ExtensionNum);
        }
        /// 插入临时记录
        /// <summary>
        /// 插入临时记录
        /// </summary>
        /// <param name="state"></param>
        /// <param name="agentAuxState"></param>
        /// <param name="callType"></param>
        /// <param name="agTime"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        public void InsertAgentState2DBAsync(AgentState state, BusyStatus agentAuxState, Calltype callType, int agTime, DateTime startTime)
        {
            int busystate = (int)agentAuxState;
            if (state != AgentState.AS4_置忙)
            {
                busystate = -1;
            }
            agentService.InsertAgentState2DBAsync(CallRecordAuthorizeCode, (int)state, busystate, (int)callType, agTime, startTime, LoginUser.UserID.ToString(), LoginUser.TrueName, LoginUser.BGID, LoginUser.ExtensionNum);
        }
        /// 更新临时记录
        /// <summary>
        /// 更新临时记录
        /// </summary>
        /// <param name="state"></param>
        /// <param name="agentAuxState"></param>
        /// <param name="callType"></param>
        /// <param name="agTime"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        public void UpdateAgentState2DBAsync(AgentState state, BusyStatus agentAuxState, Calltype callType, int agTime, DateTime startTime)
        {
            int busystate = (int)agentAuxState;
            if (state != AgentState.AS4_置忙)
            {
                busystate = -1;
            }
            agentService.UpdateAgentState2DBAsync(CallRecordAuthorizeCode, (int)state, busystate, (int)callType, agTime, startTime, LoginUser.UserID.ToString());
        }
        /// 删除临时记录
        /// <summary>
        /// 删除临时记录
        /// </summary>
        /// <returns></returns>
        public bool DeleteAgentStateToDB()
        {
            return agentService.DeleteAgentState2DB(CallRecordAuthorizeCode, LoginUser.UserID.ToString());
        }
        #endregion

        #region AgentStateDetail
        /// 插入明细记录
        /// <summary>
        /// 插入明细记录
        /// </summary>
        /// <param name="state"></param>
        /// <param name="agentAuxState"></param>
        /// <param name="callType"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public int InsertAgentStateDetail2DB(AgentState state, BusyStatus agentAuxState, Calltype callType, DateTime startTime, DateTime endTime)
        {
            int busystate = (int)agentAuxState;
            if (state != AgentState.AS4_置忙)
            {
                busystate = -1;
            }
            return agentService.InsertAgentStateDetail2DB(CallRecordAuthorizeCode, (int)state, busystate, (int)callType, startTime, endTime, LoginUser.TrueName, LoginUser.UserID.ToString(), LoginUser.ExtensionNum, Convert.ToInt32(LoginUser.BGID));
        }
        /// 更新状态明细记录
        /// <summary>
        /// 更新状态明细记录
        /// </summary>
        /// <param name="oid"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public void UpdateStateDetail2DBAsync(int oid, DateTime endTime)
        {
            agentService.UpdateStateDetail2DBAsync(CallRecordAuthorizeCode, oid, endTime);
        }
        /// 更新状态明细记录
        /// <summary>
        /// 更新状态明细记录
        /// </summary>
        /// <param name="oid"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public void UpdateStateDetail2DB(int oid, DateTime endTime)
        {
            bool a = agentService.UpdateStateDetail2DB(CallRecordAuthorizeCode, oid, endTime);
            if (a == false)
            {
                //重新插入在线时长记录
                LoginUser.LoginOnOid = InsertAgentStateDetail2DB(AgentState.AS2_签入, BusyStatus.BS0_自动, Calltype.C0_未知, endTime, endTime);
            }
        }
        /// 客服登录时更新退出时间
        /// <summary>
        /// 客服登录时更新退出时间
        /// </summary>
        /// <param name="agentID">客服ID</param>
        /// <param name="endTime">退出时间</param>
        /// <returns></returns>
        public string UpdateLoginOffTime(string agentID, DateTime endTime)
        {
            return agentService.UpdateLoginOffTime(CallRecordAuthorizeCode, agentID, endTime);
        }
        #endregion
    }
}
