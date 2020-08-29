using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using BitAuto.ISDC.CC2012.BLL;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using System.Xml;

namespace BitAuto.ISDC.CC2012.Web.CallRecordService
{
    /// <summary>
    /// Summary description for AgentTimeState
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class AgentTimeState : System.Web.Services.WebService
    {
        [WebMethod(Description = "获取站点服务器时间")]
        public DateTime GetCurrentTime(string verifyCode)
        {
            string msg = "";
            if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(verifyCode, 0, ref msg, "获取站点服务器时间，授权失败。"))
            {
                BLL.Loger.Log4Net.Info(msg);
                return new DateTime();
            }
            return DateTime.Now;
        }

        [WebMethod(Description = "插入客服临时状态记录")]
        public bool InsertAgentState2DB(string verifyCode, int state, int agentAuxState, int callType, int agTime, DateTime startTime, string agentId, string trueName, string groupName, string extensionNum)
        {
            string msg = "";
            if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(verifyCode, 0, ref msg, "插入客服临时状态记录，授权失败。"))
            {
                BLL.Loger.Log4Net.Info(msg);
                return false;
            }

            return BLL.AgentTimeState.Instance.InsertAgentState2DB(state, agentAuxState, callType, agTime, startTime, agentId, trueName, groupName, extensionNum);
        }

        [WebMethod(Description = "更新客服临时状态记录")]
        public bool UpdateAgentState2DB(string verifyCode, int state, int agentAuxState, int callType, int agTime, DateTime startTime, string agentId)
        {
            string msg = "";
            if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(verifyCode, 0, ref msg, "更新客服临时状态记录，授权失败。"))
            {
                BLL.Loger.Log4Net.Info(msg);
                return false;
            }
            return BLL.AgentTimeState.Instance.UpdateAgentState2DB(state, agentAuxState, callType, agTime, startTime, agentId);
        }

        [WebMethod(Description = "删除临时记录")]
        public bool DeleteAgentState2DB(string verifyCode, string agentId)
        {
            string msg = "";
            if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(verifyCode, 0, ref msg, "删除临时记录，授权失败。"))
            {
                BLL.Loger.Log4Net.Info(msg);
                return false;
            }
            return BLL.AgentTimeState.Instance.DeleteAgentState2DB(agentId);
        }

        [WebMethod(Description = "插入客服状态明细记录")]
        public int InsertAgentStateDetail2DB(string verifyCode, int state, int agentAuxState, int callType, DateTime startTime, DateTime endTime, string trueName, string agentId, string extensionNum, int bgid)
        {
            string msg = "";
            if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(verifyCode, 0, ref msg, "插入客服状态明细记录，授权失败。"))
            {
                BLL.Loger.Log4Net.Info(msg);
                return -1;
            }
            return BLL.AgentTimeState.Instance.InsertAgentStateDetail2DB(state, agentAuxState, callType, startTime, endTime, trueName, agentId, extensionNum, bgid);
        }

        [WebMethod(Description = "更新状态明细记录")]
        public bool UpdateStateDetail2DB(string verifyCode, int oid, DateTime endTime)
        {
            string msg = "";
            if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(verifyCode, 0, ref msg, "更新状态明细记录，授权失败。"))
            {
                BLL.Loger.Log4Net.Info(msg);
                return false;
            }
            return BLL.AgentTimeState.Instance.UpdateStateDetail2DB(oid, endTime);
        }

        [WebMethod(Description = "客服登录时更新退出时间")]
        public string UpdateLoginOffTime(string verifyCode, string agentID, DateTime endTime)
        {
            string msg = "";
            if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(verifyCode, 0, ref msg, "客服登录时更新退出时间，授权失败。"))
            {
                BLL.Loger.Log4Net.Info(msg);
                return "";
            }
            return BLL.AgentTimeState.Instance.UpdateLoginOffTime(agentID, endTime);
        }

        [WebMethod(Description = "获取用户组名字字符串")]
        public string GetUserGroupNamesStr(string verifyCode, int userId)
        {
            string msg = "";
            if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(verifyCode, 0, ref msg, "获取用户组名字字符串，授权失败。"))
            {
                BLL.Loger.Log4Net.Info(msg);
                return "";
            }
            return BLL.AgentTimeState.Instance.GetUserGroupNamesStr(userId);
        }

        [WebMethod(Description = "根据用户ID获取所属分组及外呼出局号")]
        public string GetBGNameAndOutNum(string verifyCode, int userId)
        {
            string msg = "";
            if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(verifyCode, 0, ref msg, "根据用户ID获取所属分组及外呼出局号，授权失败。"))
            {
                BLL.Loger.Log4Net.Info(msg);
                return "";
            }
            return BLL.AgentTimeState.Instance.GetBGNameAndOutNum(userId);
        }

        [WebMethod(Description = "获取在线客服列表（置闲、置忙）")]
        public bool GetAgentNumsOnLine(string verifyCode, out int[] iarray, out string errormsg)
        {
            string msg = "";
            if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(verifyCode, 0, ref msg, "获取在线客服列表（置闲、置忙），授权失败。"))
            {
                BLL.Loger.Log4Net.Info(msg);
                iarray = new int[] { };
                errormsg = msg;
                return false;
            }
            return BLL.AgentTimeState.Instance.GetAgentNumsOnLine(out iarray, out errormsg);
        }

        [WebMethod(Description = "获取在线客服状态列表（置闲、置忙）")]
        public bool GetAgentStateOnLine(string verifyCode, out System.Data.DataTable dt, Vender vender, out string errormsg)
        {
            string msg = "";
            if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(verifyCode, 0, ref msg, "获取在线客服状态列表（置闲、置忙），授权失败。"))
            {
                dt = new DataTable();
                errormsg = msg;
                BLL.Loger.Log4Net.Info(msg);
                return false;
            }
            return BLL.AgentTimeState.Instance.GetAgentStateOnLine(out dt, vender, out errormsg);
        }

        [WebMethod(Description = "获取在线客服状态列表（工作中）")]
        public bool GetAgentStateWorking(string verifyCode, out System.Data.DataTable dt, Vender vender, out string errormsg)
        {
            string msg = "";
            if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(verifyCode, 0, ref msg, "获取在线客服状态列表（工作中），授权失败。"))
            {
                dt = new DataTable();
                errormsg = msg;
                BLL.Loger.Log4Net.Info(msg);
                return false;
            }
            return BLL.AgentTimeState.Instance.GetAgentStateWorking(out dt, vender, out errormsg);
        }

        [WebMethod(Description = "根据用户UserID判断是否有监听权限")]
        public bool IsCanListenAgent(string verifyCode, int userID, string sysID)
        {
            try
            {
                string msg = "";
                if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(verifyCode, 0, ref msg, "根据用户UserID判断是否有监听权限，授权失败。"))
                {
                    BLL.Loger.Log4Net.Info(msg);
                    return false;
                }

                bool isOK = false;
                isOK = BLL.Util.CheckRight(userID, "SYS024MODKHD01");
                return isOK;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("[AgentTimeState.asmx]IsCanListenAgent失败！", ex);
                return false;
            }

        }

        [WebMethod(Description = "根据域名获取用户ID")]
        public int GetUserIDByNameDomainAccount(string verifyCode, string domainAccount)
        {
            string msg = "";
            if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(verifyCode, 0, ref msg, "根据域名获取用户ID，授权失败。"))
            {
                BLL.Loger.Log4Net.Info(msg);
                return -1;
            }

            return BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetUserIDByNameDomainAccount(domainAccount);
        }

        [WebMethod(Description = "根据UserName获取用户ID")]
        public int GetUserIDByUserName(string verifyCode, string username)
        {
            string msg = "";
            if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(verifyCode, 0, ref msg, "根据UserName获取用户ID，授权失败。"))
            {
                BLL.Loger.Log4Net.Info(msg);
                return -1;
            }
            return BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetUserIDByName(username);
        }

        [WebMethod(Description = "根据UserId获取用户的技能组数据（SGID, 厂商技能组id，优先级）")]
        public DataTable GetUserSkillGroupIdAndPriorty(string verifyCode, int userId)
        {
            string msg = "";
            if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(verifyCode, 0, ref msg, "根据UserId获取用户的技能组数据（SGID, 厂商技能组id，优先级），授权失败。"))
            {
                BLL.Loger.Log4Net.Info(msg);
                return new DataTable();
            }

            BLL.Loger.Log4Net.Info("[AgentTimeState.asmx]GetUserSkillGroupIdAndPriorty ...获取用户技能组数据结束...UserId：" + userId);

            DataTable skillDt = null;

            try
            {
                skillDt = BLL.SkillGroupDataRight.Instance.GetUserSkillGroupIdAndPriorty(userId);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("[AgentTimeState.asmx]GetUserSkillGroupIdAndPriorty ...获取用户技能组数据出错! ", ex);
            }

            BLL.Loger.Log4Net.Info("[AgentTimeState.asmx]GetUserSkillGroupIdAndPriorty ...获取用户技能组数据结束!");
            return skillDt;
        }

        [WebMethod(Description = "根据SGID获取技能组字母ID")]
        public string GetManufactureSkillGroupIDBySGID(string verifyCode, int sgID)
        {
            string msg = "";
            if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(verifyCode, 0, ref msg, "根据SGID获取技能组字母ID，授权失败。"))
            {
                BLL.Loger.Log4Net.Info(msg);
                return "";
            }

            BLL.Loger.Log4Net.Info("[AgentTimeState.asmx]GetManufactureSkillGroupIDBySGID ...获取厂商的SGID码开始...SGID：" + sgID);
            string msgid = "";
            try
            {
                msgid = BLL.SkillGroupDataRight.Instance.GetManufactureSkillGroupID(sgID);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("[AgentTimeState.asmx]GetManufactureSkillGroupIDBySGID ...获取厂商的SGID码出错!", ex);
            }

            BLL.Loger.Log4Net.Info("[AgentTimeState.asmx]GetManufactureSkillGroupIDBySGID ...获取厂商的SGID码结束!");
            return msgid;
        }

        [WebMethod(Description = "根据字母ID获取技能组名称")]
        public string GetSkillNameByMID(string verifyCode, string mID)
        {
            string msg = "";
            if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(verifyCode, 0, ref msg, "根据字母ID获取技能组名称，授权失败。"))
            {
                BLL.Loger.Log4Net.Info(msg);
                return "";
            }

            BLL.Loger.Log4Net.Info("[AgentTimeState.asmx]GetSkillNameByMID ...根据字母ID获取技能组名称...MID：" + mID);
            string msgid = "";
            try
            {
                msgid = BLL.SkillGroupDataRight.Instance.GetSkillNameByMID(mID);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("[AgentTimeState.asmx]GetSkillNameByMID ...根据字母ID获取技能组名称异常!", ex);
            }

            BLL.Loger.Log4Net.Info("[AgentTimeState.asmx]GetSkillNameByMID ...根据字母ID获取技能组名称结束!");
            return msgid;
        }

        [WebMethod(Description = "根据数字ID获取技能组名称")]
        public string GetSkillNameBySGID(string verifyCode, int sgID)
        {
            string msg = "";
            if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(verifyCode, 0, ref msg, "根据数字ID获取技能组名称，授权失败。"))
            {
                BLL.Loger.Log4Net.Info(msg);
                return "";
            }

            BLL.Loger.Log4Net.Info("[AgentTimeState.asmx]GetSkillNameBySGID ...根据数字ID获取技能组名称...SGID：" + sgID);
            string msgid = "";
            try
            {
                msgid = BLL.SkillGroupDataRight.Instance.GetSkillNameBySGID(sgID);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("[AgentTimeState.asmx]GetSkillNameBySGID ...根据数字ID获取技能组名称异常!", ex);
            }

            BLL.Loger.Log4Net.Info("[AgentTimeState.asmx]GetSkillNameBySGID ...根据数字ID获取技能组名称结束!");
            return msgid;
        }

        [WebMethod(Description = "根据热线获取技能组")]
        public DataTable GetHotlineSkillGroupByTelMainNum(string verifyCode, string tel)
        {
            try
            {
                string msg = "";
                if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(verifyCode, 0, ref msg, "根据热线获取技能组，授权失败。"))
                {
                    BLL.Loger.Log4Net.Info(msg);
                    return new DataTable();
                }

                BLL.Loger.Log4Net.Info("[AgentTimeState.asmx]GetHotlineSkillGroupByTelMainNum ...根据热线获取技能组...tel：" + tel);
                return BLL.SkillGroupDataRight.Instance.GetHotlineSkillGroupByTelMainNum(tel);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("[AgentTimeState.asmx]GetHotlineSkillGroupByTelMainNum ...根据热线获取技能组", ex);
                return new DataTable();
            }
        }

        [WebMethod(Description = "获取在线客服状态列表（置闲、置忙、自定义条件）")]
        public DataTable GetAgentStateOnLineByWhere(string verifyCode, Vender vender, string where)
        {
            try
            {
                string msg = "";
                if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(verifyCode, 0, ref msg, "获取在线客服状态列表（置闲、置忙、自定义条件），授权失败。"))
                {
                    BLL.Loger.Log4Net.Info(msg);
                    return new DataTable();
                }

                BLL.Loger.Log4Net.Info("[AgentTimeState.asmx]GetAgentStateOnLineByWhere ...获取在线客服状态列表（置闲、置忙、自定义条件）...vender：" + vender + ",where：" + where);
                return BLL.AgentTimeState.Instance.GetAgentStateOnLineByWhere(vender, where);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("[AgentTimeState.asmx]GetAgentStateOnLineByWhere ...获取在线客服状态列表（置闲、置忙、自定义条件）", ex);
                return new DataTable();
            }
        }

        [WebMethod(Description = "获取人员的管辖分组")]
        public DataTable GetManageBusinessGroups(string verifyCode, int userId)
        {
            try
            {
                string msg = "";
                if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(verifyCode, 0, ref msg, "获取人员的管辖分组，授权失败。"))
                {
                    BLL.Loger.Log4Net.Info(msg);
                    return null;
                }
                return BitAuto.ISDC.CC2012.BLL.UserGroupDataRigth.Instance.GetUserGroupDataRigthByUserID(userId, "and bg.Status=0");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("[AgentTimeState.asmx]GetManageBusinessGroups 获取人员的管辖分组 异常", ex);
                return null;
            }
        }

        [WebMethod(Description = "获取全部人员和全部分组")]
        public DataSet GetAllEmployeeAgentAndBusinessGroup(string verifyCode)
        {
            try
            {
                string msg = "";
                if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(verifyCode, 0, ref msg, "获取全部人员和全部分组，授权失败。"))
                {
                    BLL.Loger.Log4Net.Info(msg);
                    return null;
                }
                return BLL.EmployeeAgent.Instance.GetAllEmployeeAgentAndBusinessGroup();
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("[AgentTimeState.asmx]GetAllEmployeeAgentAndBusinessGroup 获取全部人员和全部分组 异常", ex);
                return null;
            }
        }

        [WebMethod(Description = "插入监控日志")]
        public int InsertListenAgentLog(string verifyCode,
            int listenUserID, string listenUserName, string listenNum, string listenExtensionNum, int listenOper,
            int agentUserID, string agentUserName, string agentNum, string agentExtensionNum, int agentStatus,
            Vender vendor, string remark)
        {
            try
            {
                string msg = "";
                if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(verifyCode, 0, ref msg, "插入监控日志，授权失败。"))
                {
                    BLL.Loger.Log4Net.Info(msg);
                    return -1;
                }
                return BLL.ListenAgentLog.Instance.InsertListenAgentLog(
                    listenUserID, listenUserName, listenNum, listenExtensionNum, (OperForListen)listenOper,
                    agentUserID, agentUserName, agentNum, agentExtensionNum, (AgentStateForListen)agentStatus,
                    vendor, remark);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("[AgentTimeState.asmx]InsertListenAgentLog 插入监控日志 异常", ex);
                return -1;
            }
        }

        [WebMethod(Description = "生成服务器端日志")]
        public void SendLogToServer(string verifyCode, string path, string logmsg)
        {
            try
            {
                string msg = "";
                if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(verifyCode, 0, ref msg, "生成服务器端日志，授权失败。"))
                {
                    BLL.Loger.Log4Net.Info(msg);
                    return;
                }
                BLL.Util.LogForWeb("[SendLogToServer]", logmsg, "客户端");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("[AgentTimeState.asmx] SendLogToServer 生成服务器端日志 异常", ex);
                return;
            }
        }
    }
}
