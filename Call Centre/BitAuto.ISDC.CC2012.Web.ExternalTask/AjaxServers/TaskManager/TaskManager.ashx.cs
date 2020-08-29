using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.YanFa.SysRightManager.Common;
using System.Web.SessionState;
using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.Web.ExternalTask.AjaxServers.TaskManager
{
    /// <summary>
    /// TaskManager 的摘要说明
    /// </summary>
    public class TaskManager : IHttpHandler, IRequiresSessionState
    {
        #region 属性
        private string RequestAction
        {
            get { return HttpContext.Current.Request["Action"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Action"].ToString()); }
        }
        //CustHistoryLog表属性
        private string RequestCHLRecID
        {
            get { return HttpContext.Current.Request["CHLRecID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CHLRecID"].ToString()); }
        }
        private string RequestCHLTaskID    //任务ID
        {
            get { return HttpContext.Current.Request["CHLTaskID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CHLTaskID"].ToString()); }
        }
        private string RequestSolveUserEID  //受理人ID（HR）
        {
            get { return HttpContext.Current.Request["SolveUserEID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SolveUserEID"].ToString()); }
        }
        private string RequestComment   //处理意见
        {
            get { return HttpContext.Current.Request["Comment"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Comment"].ToString()); }
        }
        private string RequestCHLAction   //CustHistoryLog 动作
        {
            get { return HttpContext.Current.Request["CHLAction"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CHLAction"].ToString()); }
        }
        private string RequestCHLStatus   //处理状态
        {
            get { return HttpContext.Current.Request["CHLStatus"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CHLStatus"].ToString()); }
        }
        private string RequestToNextSolveUserEID   //转到下一个受理人（HR）
        {
            get { return HttpContext.Current.Request["ToNextSolveUserEID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ToNextSolveUserEID"].ToString()); }
        }
        private string RequestPid   //父ID
        {
            get { return HttpContext.Current.Request["Pid"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Pid"].ToString()); }
        }
        //CustHistoryInfo表属性
        private string RequestCHIRecID
        {
            get { return HttpContext.Current.Request["CHIRecID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CHIRecID"].ToString()); }
        }
        private string RequestCHITaskID
        {
            get { return HttpContext.Current.Request["CHITaskID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CHITaskID"].ToString()); }
        }
        private string RequestIsComplaint   //是否确定投诉
        {
            get { return HttpContext.Current.Request["IsComplaint"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["IsComplaint"].ToString()); }
        }
        private string RequestIsSendEmail //是否确认发送邮件
        {
            get { return HttpContext.Current.Request["IsSendEmail"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["IsSendEmail"].ToString()); }
        }
        private string RequestProcessStatus //任务状态
        {
            get { return HttpContext.Current.Request["ProcessStatus"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ProcessStatus"].ToString()); }
        }
        //TaskCurrentSolveUser表属性
        private string RequestTCSRecID
        {
            get { return HttpContext.Current.Request["TCSRecID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TCSRecID"].ToString()); }
        }
        private string RequestTCSTaskID
        {
            get { return HttpContext.Current.Request["TCSTaskID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TCSTaskID"].ToString()); }
        }
        private string RequestCurrentSolveUserEID
        {
            get { return HttpContext.Current.Request["CurrentSolveUserEID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CurrentSolveUserEID"].ToString()); }
        }
        private string RequestTCSStatus
        {
            get { return HttpContext.Current.Request["TCSStatus"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TCSStatus"].ToString()); }
        }
        //SendEmailLog表属性 
        private string RequestCustID
        {
            get { return HttpContext.Current.Request["CustID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CustID"].ToString()); }
        }

        private string RequestUrlParameter
        {
            get { return HttpContext.Current.Request["UrlParameter"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["UrlParameter"].ToString()); }
        }
        #endregion
        private string RequestLoginEID
        {
            get { return HttpContext.Current.Request["LoginEID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["LoginEID"].ToString()); }
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;
            switch (RequestAction.ToLower())
            {
                case "custhistorylogexternalsubmit": CustHistoryLogExternalSubmit(out msg);//外部提交
                    break;
            }
            context.Response.Write(msg);
        }

        //根据任务ID获取客户历史信息
        private Entities.CustHistoryInfo getModelCustHistoryInfoByTaskID(string taskID)
        {
            Entities.CustHistoryInfo Model_CustHistoryInfo = new CustHistoryInfo();
            if (taskID != "")
            {
                QueryCustHistoryInfo query_info = new QueryCustHistoryInfo();
                query_info.TaskID = taskID;
                int count;
                DataTable dt_info = BLL.CustHistoryInfo.Instance.GetCustHistoryInfo(query_info, "", 1, 10000, Entities.CustHistoryInfo.SelectFieldStr, out count);
                if (dt_info.Rows.Count == 1)
                {
                    Model_CustHistoryInfo = BLL.CustHistoryInfo.Instance.GetCustHistoryInfo(long.Parse(dt_info.Rows[0]["RecID"].ToString()));
                }
            }
            return Model_CustHistoryInfo;
        }

        //提交 外部使用（插入客户历史信息处理日志）
        private void CustHistoryLogExternalSubmit(out string msg)
        {
            msg = string.Empty;

            if (RequestCHITaskID == "")
            {
                msg = "{'result':'no','msg':'没有任务ID，操作失败！'}";
                return;
            }

            Entities.CustHistoryInfo model_CustHistoryInfo = getModelCustHistoryInfoByTaskID(RequestCHITaskID);
            if (model_CustHistoryInfo == null)
            {
                msg = "{'result':'no','msg':'没有找到该条任务记录，操作失败！'}";
                return;
            }

            int action = (int)Entities.Action.ActionSumbit;//记录动作 默认：提交

            //1 判断是否存在“转到受理人ID(ToNextSolveUserEID)”存在：向当前受理人表插入一条记录
            int nextSolveEID;
            if (RequestToNextSolveUserEID != "")
            {
                if (int.TryParse(RequestToNextSolveUserEID, out nextSolveEID))
                {
                    //判断登陆者权限 如果权限有高级操作权限，则不需要修改自己的记录 自己添加提交

                    //坐席权限 
                    //修改：当前受理人表（修改自己的记录） 状态改为0 无效
                    QueryTaskCurrentSolveUser query_TCSUpdate = new QueryTaskCurrentSolveUser();
                    query_TCSUpdate.TaskID = RequestCHITaskID;
                    query_TCSUpdate.CurrentSolveUserEID = int.Parse(RequestLoginEID);
                    query_TCSUpdate.Status = 1;
                    int count;
                    DataTable dt_TCSUpdate = BLL.TaskCurrentSolveUser.Instance.GetTaskCurrentSolveUser(query_TCSUpdate, "", 1, 10000, out count);
                    if (dt_TCSUpdate.Rows.Count == 0)
                    {
                        msg = "{'result':'no','msg':'您不是当前受理人，操作失败！'}";
                        return;
                    }
                    for (int i = 0; i < dt_TCSUpdate.Rows.Count; i++)
                    {
                        UpdateTaskCurrentSolveUser(dt_TCSUpdate.Rows[i]["RecID"].ToString(), 0);
                    }

                    // 插入：当前受理人表  （插入转到受理人的记录）
                    TaskCurrentSolveUser model_TCS = new TaskCurrentSolveUser();
                    model_TCS.TaskID = RequestCHITaskID;
                    model_TCS.CurrentSolveUserEID = nextSolveEID;
                    //model_TCS.CurrentSolveUserID = getUserEID(nextSolveEID);
                    model_TCS.Status = 1;
                    model_TCS.CreateTime = DateTime.Now;
                    int _loginerEID;
                    if (int.TryParse(RequestLoginEID, out _loginerEID))
                    {
                        model_TCS.CreateUserAdName = BLL.Util.GetEmployeeDomainAccountByEid(_loginerEID);
                    }
                    InsertTaskCurrentSolveUser(model_TCS);
                    action = (int)Entities.Action.ActionTurnOut;//动作：转出

                    //发送邮件 
                    sendEmailByType(2, RequestCHITaskID, nextSolveEID, model_CustHistoryInfo.CustID, _loginerEID);
                    //sendEmail(RequestCHITaskID, nextSolveEID, model_CustHistoryInfo.CustID);
                }
                else
                {
                    msg = "{'result':'no','msg':'转到下一个受理人ID出错，操作失败！'}";
                    return;
                }
            }

            //2 插入处理信息到CustHistoryLog表
            CustHistoryLog model = new CustHistoryLog();
            model.TaskID = RequestCHITaskID;
            //model.SolveUserID = BLL.Util.GetLoginUserID();
            model.SolveUserEID = int.Parse(RequestLoginEID);
            model.SolveTime = DateTime.Now;
            model.Comment = RequestComment;
            model.Action = action;
            int status;
            if (int.TryParse(RequestCHLStatus, out status))
            {
                model.Status = status;
            }

            int toNextSolveUserEID;
            if (int.TryParse(RequestToNextSolveUserEID, out toNextSolveUserEID))
            {
                model.ToNextSolveUserEID = toNextSolveUserEID;
                //model.ToNextSolveUserID = getUserEID(toNextSolveUserEID);
            }
            int _loginEID;
            if (int.TryParse(RequestLoginEID, out _loginEID))
            {
                long pid = getPIDByNextSolveEID(RequestCHITaskID, _loginEID);
                if (pid != 0)
                {
                    model.Pid = pid;
                }
            }

            InsertCustHistoryLog(model);

            //查找登陆人的客户历史关联邮件模板的记录
            QueryCustHistoryTemplateMapping query_templateMappingOld = new QueryCustHistoryTemplateMapping();
            query_templateMappingOld.TaskID = model.TaskID;
            query_templateMappingOld.SolveUserEID = model.SolveUserEID;
            int templateCount;
            DataTable dt_templateMappingOld = BLL.CustHistoryTemplateMapping.Instance.GetCustHistoryTemplateMapping(query_templateMappingOld, "", 1, 10000, out templateCount);
            if (dt_templateMappingOld.Rows.Count > 0)
            {
                int _templateID;
                if (int.TryParse(dt_templateMappingOld.Rows[0]["TemplateID"].ToString(), out _templateID))
                {
                    //插入（下一个受理人）到客户历史关联邮件模板
                    CustHistoryTemplateMapping model_templateMappingNew = new CustHistoryTemplateMapping();
                    model_templateMappingNew.TaskID = query_templateMappingOld.TaskID;
                    model_templateMappingNew.TemplateID = _templateID;
                    model_templateMappingNew.SolveUserEID = toNextSolveUserEID;
                    model_templateMappingNew.CreateTime = DateTime.Now;
                    model_templateMappingNew.CreateUserID = _loginEID;
                    BLL.CustHistoryTemplateMapping.Instance.Insert(model_templateMappingNew);
                }
            }

            msg = "{'result':'yes','msg':'操作成功'}";
        }
        //插入 ：当前受理人表 TaskCurrentSolveUser
        private void InsertTaskCurrentSolveUser(TaskCurrentSolveUser model)
        {
            if (model != null)
            {
                BLL.TaskCurrentSolveUser.Instance.Insert(model);
                BLL.Util.InsertUserLog("【外部提交】当前受理人表【插入】任务ID：【" + model.TaskID + "】当前受理人：【" + BLL.Util.GetEmployeeNameByEID(int.Parse(model.CurrentSolveUserEID.ToString())) + "】状态：【" + (model.Status == 0 ? "无效" : "有效") + "】的记录", int.Parse(RequestLoginEID));
            }
        }

        //插入 CustHistoryLog表
        private void InsertCustHistoryLog(CustHistoryLog model)
        {
            if (model != null)
            {
                BLL.CustHistoryLog.Instance.Insert(model);
                //日志
                string action = string.Empty;
                switch (model.Action)
                {
                    case (int)Entities.Action.ActionSumbit: action = "提交";
                        break;
                    case (int)Entities.Action.ActionTurnOut: action = "转出";
                        break;
                    case (int)Entities.Action.ActionApplyTurn: action = "申请转出";
                        break;
                    case (int)Entities.Action.ActionTurnOver: action = "结束";
                        break;
                    case (int)Entities.Action.ActionAgreeApplyTurn: action = "同意转出";
                        break;
                }
                string status = string.Empty;
                switch (model.Status)
                {
                    case (int)Entities.ProcessStatus.ProcessSolve: status = "已解决";
                        break;
                    case (int)Entities.ProcessStatus.ProcessUnresolved: status = "未解决";
                        break;
                    case (int)Entities.ProcessStatus.ProcessNotSolve: status = "不解决";
                        break;
                }
                BLL.Util.InsertUserLog("【外部提交】客户历史信息处理日志表【插入】任务ID：【" + model.TaskID + "】受理人：【" + BLL.Util.GetEmployeeNameByEID(int.Parse(model.SolveUserEID.ToString())) + "】处理意见：【" + model.Comment + "】动作：【" + action + "】处理状态：【" + status + "】pid：【" + model.Pid + "】的记录",int.Parse(RequestLoginEID));
            }
        }

        //修改 ：当前受理人表 TaskCurrentSolveUser.
        private void UpdateTaskCurrentSolveUser(string recID, int status)
        {
            int RecID;
            if (int.TryParse(recID, out RecID))
            {
                TaskCurrentSolveUser model = BLL.TaskCurrentSolveUser.Instance.GetTaskCurrentSolveUser(RecID);
                if (model != null)
                {
                    model.Status = status;
                    BLL.TaskCurrentSolveUser.Instance.Update(model);
                    if (model.Status != status)
                    {
                        BLL.Util.InsertUserLog("【外部提交】当前受理人表【修改】主键：【" + RecID + "】的记录，将状态【" + (model.Status == 0 ? "无效" : "有效") + "】修改为【" + (status == 0 ? "无效" : "有效") + "】");
                    }
                }
            }
        }
        //根据转到受理人EID 获取到主键 返回PID
        private long getPIDByNextSolveEID(string taskID, int toNextSolveEID)
        {
            long pid = 0;
            int count;
            QueryCustHistoryLog query_Log = new QueryCustHistoryLog();
            query_Log.TaskID = taskID;
            query_Log.ToNextSolveUserEID = toNextSolveEID;
            DataTable dt_Log = BLL.CustHistoryLog.Instance.GetCustHistoryLog(query_Log, " SolveTime Desc", 1, 10000, out count);
            if (dt_Log.Rows.Count > 0)
            {
                pid = long.Parse(dt_Log.Rows[0]["RecID"].ToString());
            }

            return pid;
        }

        //发送邮件 eid:hr系统人员主键 type:2-提交   
        private void sendEmailByType(int type, string taskID, int eid, string custID, int loginID)
        {
            // 根据该条任务ID去邮件模板表找到对应的模板ID发送
            if (type == 2)
            {
                QueryCustHistoryTemplateMapping query_Template = new QueryCustHistoryTemplateMapping();
                query_Template.TaskID = taskID;
                query_Template.SolveUserEID = loginID;
                int count;
                DataTable dt_Template = BLL.CustHistoryTemplateMapping.Instance.GetCustHistoryTemplateMapping(query_Template, "", 1, 10000, out count);
                if (dt_Template.Rows.Count > 0)
                {
                    sendEmail(taskID, dt_Template.Rows[0]["TemplateID"].ToString(), eid, custID);
                }
                else
                {
                    sendEmail(taskID, "", eid, custID);
                }
            }

        }

        //发送邮件 外部提交-根据固定的模板发送邮件
        private void sendEmail(string taskID, string templateID, int eid, string custID)
        {
            //url
            string urlHead = ConfigurationUtil.GetAppSettingValue("TaskProcessUrl");

            //找到邮件接收人的邮箱 
            string userEmail = BLL.Util.GetEmployeeEmailByEid(eid);
            //1 插入 发送邮件日志表 （记录）
            SendEmailLog model_Email = new SendEmailLog();
            string templateName = BLL.Util.GetEmployeeNameByEid(eid);//收件人姓名  
            string urlInternal = urlHead + "/TaskManager/TaskProcess.aspx?TaskID=" + taskID;
            string urlExternal = urlHead + "/ExternalTask/ExternalTaskProcess.aspx?TaskID=" + taskID;
            string templateTitle = string.Empty;    //模板标题
            string templateLog = string.Empty;  //模板日志
            string templateUrl = string.Empty;  //模板链接格式
            int _templateID;   //模板ID

            //根据TaskID和受理人ID获取所要发送的模板ID
            if (int.TryParse(templateID, out _templateID))
            {
                QueryCustHistoryTemplateMapping query_Template = new QueryCustHistoryTemplateMapping();
                query_Template.TaskID = taskID;
                query_Template.SolveUserEID = eid;
                int count;
                DataTable dt_Template = BLL.CustHistoryTemplateMapping.Instance.GetCustHistoryTemplateMapping(query_Template, "", 1, 10000, out count);
                if (dt_Template != null)
                {
                    //根据模板ID获取模板内容
                    TemplateInfo model_TemplateInfo = new TemplateInfo();
                    model_TemplateInfo = BLL.TemplateInfo.Instance.GetTemplateInfo(_templateID);
                    if (model_TemplateInfo == null)
                    {
                        return;
                    }
                    //templateInfo += model_TemplateInfo.Content;
                    templateUrl += model_TemplateInfo.Content + "<br/>";
                    templateTitle = model_TemplateInfo.Title;
                }
                templateLog += "【同意转出】";
            }
            else
            {
                _templateID = 0;
                templateUrl += "呼叫中心系统有需要您协助处理的问题，";
                templateLog += "【提交】";
            }

            templateLog += "发出邮件至【" + userEmail + "】；接收人【" + templateName + "】";
            templateUrl += "详情请点击：<br/><a href='" + urlExternal + "'>" + urlHead + "/ExternalTask/ExternalTaskProcess.aspx</a><br/>";
            templateUrl += "呼叫中心系统内部人员请访问：<br/><a href='" + urlInternal + "'>" + urlHead + "/TaskManager/TaskProcess.aspx</a>";

            model_Email.TemplateID = 0;
            model_Email.CustID = custID;
            model_Email.MailTo = userEmail;
            model_Email.SendTime = DateTime.Now;
            model_Email.SendContent = templateUrl;
            model_Email.CreateUserID = int.Parse(RequestLoginEID);

            BLL.SendEmailLog.Instance.Insert(model_Email);

            //2根据 客户历史记录信息表中 模板ID来发送模板邮件
            BLL.EmailHelper.Instance.SendMail(templateName, templateUrl, "呼叫中心问题处理", new string[] { userEmail });

            BLL.Util.InsertUserLog("【外部提交】发送邮件日志表【插入】客户名称：【" + BLL.CustBasicInfo.Instance.GetCustBasicInfo(model_Email.CustID).CustName + "】接收人：【" + templateName + "】邮箱：【" + model_Email.MailTo + "】发送内容：【" + model_Email.SendContent + "】的记录【邮件发送成功】", int.Parse(RequestLoginEID));

        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}