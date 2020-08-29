using System;
using System.Collections.Generic;
using System.Web;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using System.Web.SessionState;
using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TaskManager
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

        //add by qizq 2013-1-4是否通话中
        private string IsCalling
        {
            get { return HttpContext.Current.Request["IsCalling"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["IsCalling"].ToString()); }
        }
        //本地录音表主键
        private string CallRecordID
        {
            get { return HttpContext.Current.Request["CallRecordID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CallRecordID"].ToString()); }
        }
        //处理记录表主键
        private string HistoryLogID
        {
            get { return HttpContext.Current.Request["HistoryLogID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["HistoryLogID"].ToString()); }
        }
        //


        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;
            switch (RequestAction.ToLower())
            {
                case "agreeturnout": AgreeTurnOut(out msg); //同意转出
                    break;
                case "taskend": TaskEnd(out msg);           //结束任务
                    break;
                case "custhistorylogsubmit": CustHistoryLogSubmit(out msg); //提交
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

        //提交（插入客户历史信息处理日志）
        private void CustHistoryLogSubmit(out string msg)
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

            //0 判断如果是有高级操作按钮的人操作，则在提交时将其他当前受理人的表中记录状态改为无效0
            bool right_AgreeTurnOut = BLL.Util.CheckRight(BLL.Util.GetLoginUserID(), "SYS024BUT1101");
            bool right_TaskTurnOver = BLL.Util.CheckRight(BLL.Util.GetLoginUserID(), "SYS024BUT1102");
            if (right_AgreeTurnOut && right_TaskTurnOver)
            {
                QueryTaskCurrentSolveUser query_taskCurrentSolveUser = new QueryTaskCurrentSolveUser();
                query_taskCurrentSolveUser.TaskID = model_CustHistoryInfo.TaskID;
                query_taskCurrentSolveUser.Status = 1;
                int count;
                DataTable dt_taskCurrentSolveUser = BLL.TaskCurrentSolveUser.Instance.GetTaskCurrentSolveUser(query_taskCurrentSolveUser, "", 1, 10000, out count);
                for (int i = 0; i < dt_taskCurrentSolveUser.Rows.Count; i++)
                {
                    UpdateTaskCurrentSolveUser(dt_taskCurrentSolveUser.Rows[i]["RecID"].ToString(), 0);
                }
            }

            //1 判断是否存在“转到受理人ID(ToNextSolveUserEID)”存在：向当前受理人表插入一条记录
            int nextSolveEID;
            if (RequestToNextSolveUserEID != "")
            {
                if (int.TryParse(RequestToNextSolveUserEID, out nextSolveEID))
                {
                    //判断登陆者权限 如果权限有高级操作权限，则不需要修改自己的记录 自己添加提交

                    //坐席权限
                    if (!right_AgreeTurnOut && !right_TaskTurnOver)
                    {
                        //修改：当前受理人表（修改自己的记录） 状态改为0 无效
                        QueryTaskCurrentSolveUser query_TCSUpdate = new QueryTaskCurrentSolveUser();
                        query_TCSUpdate.TaskID = model_CustHistoryInfo.TaskID;
                        query_TCSUpdate.CurrentSolveUserEID = BLL.Util.GetLoginUserID();
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
                    }

                    if (right_AgreeTurnOut && right_TaskTurnOver)
                    {
                        //因为是高级权限操作，先判断是否已经转出过，如果没有同意转出而直接点击提交，则向Log表插入一条同意转出的动作记录

                        //1 判断是否是第一次转出，如果是，则插入一条同意转出的动作记录  
                        QueryCustHistoryLog query_Log = new QueryCustHistoryLog();
                        query_Log.TaskID = model_CustHistoryInfo.TaskID;
                        int totalCount;
                        query_Log.Action = (int)Entities.Action.ActionAgreeApplyTurn;
                        DataTable dt_Log = BLL.CustHistoryLog.Instance.GetCustHistoryLog(query_Log, "", 1, 10000, out totalCount);
                        if (dt_Log.Rows.Count == 0)
                        {
                            //2 插入 客户历史信息处理日志 （动作记录）
                            InsertCustHistoryLogByAction(model_CustHistoryInfo.TaskID, RequestComment, (int)Entities.Action.ActionAgreeApplyTurn);//动作：同意转出
                        }

                        //3 修改客户历史记录信息表的记录:是否确定投诉 可修改,是否确认发送邮件 必须修改为true,任务状态 必须修改为“处理中”
                        model_CustHistoryInfo.IsSendEmail = true;
                        if (RequestIsComplaint != "")
                        {
                            model_CustHistoryInfo.IsComplaint = bool.Parse(RequestIsComplaint);
                        }
                        model_CustHistoryInfo.ProcessStatus = (int)Entities.EnumTaskStatus.TaskStatusNow;

                        UpdateCustHistoryInfo(model_CustHistoryInfo);
                    }

                    // 插入：当前受理人表  （插入转到受理人的记录）
                    TaskCurrentSolveUser model_TCS = new TaskCurrentSolveUser();
                    model_TCS.TaskID = model_CustHistoryInfo.TaskID;
                    model_TCS.CurrentSolveUserEID = nextSolveEID;
                    model_TCS.CurrentSolveUserID = getUserEID(nextSolveEID);
                    model_TCS.Status = 1;
                    model_TCS.CreateTime = DateTime.Now;
                    model_TCS.CreateUserAdName = BLL.Util.GetDomainAccountByLimitEID(BLL.Util.GetLoginUserID());
                    InsertTaskCurrentSolveUser(model_TCS);
                    action = (int)Entities.Action.ActionTurnOut;//动作：转出

                    //发送邮件
                    sendEmailByType(2, model_CustHistoryInfo.TaskID, nextSolveEID, model_CustHistoryInfo.CustID, BLL.Util.GetLoginUserID());
                }
                else
                {
                    msg = "{'result':'no','msg':'转到下一个受理人ID出错，操作失败！'}";
                    return;
                }
            }

            //2 插入处理信息到CustHistoryLog表
            CustHistoryLog model = new CustHistoryLog();
            //modify by qizq 2013-1-4首先判断是否是通话中
            if (IsCalling == "1")
            {
                if (HistoryLogID == "")
                {
                    //通话中提交把本地录音主键付给实体
                    long CallRecordReCID = 0;
                    if (CallRecordID != "")
                    {
                        if (long.TryParse(CallRecordID, out CallRecordReCID))
                        {
                            model.CallRecordID = CallRecordReCID;
                        }
                    }
                }
            }
            //
            model.TaskID = model_CustHistoryInfo.TaskID;
            model.SolveUserID = BLL.Util.GetLoginUserID();
            model.SolveUserEID = BLL.Util.GetLoginUserID();
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
                model.ToNextSolveUserID = getUserEID(toNextSolveUserEID);
            }
            long pid = getPIDByNextSolveEID(model_CustHistoryInfo.TaskID, BLL.Util.GetLoginUserID());
            if (pid != 0)
            {
                model.Pid = pid;
            }
            //modify by qizq 2013-1-4不是在通话中，处理记录已存在更新处理记录
            if (IsCalling != "1" && HistoryLogID != "")
            {
                //通话中提交把本地录音主键付给实体
                long CallRecordReCID = 0;
                if (CallRecordID != "")
                {
                    if (long.TryParse(CallRecordID, out CallRecordReCID))
                    {
                        model.CallRecordID = CallRecordReCID;
                    }
                }
                long HistoryLogIDLog = 0;
                if (long.TryParse(HistoryLogID, out HistoryLogIDLog))
                {
                    model.RecID = HistoryLogIDLog;
                }
                //CustHistoryLog 作废 2016-3-1 强斐
                //UpdateCustHistoryLog(model);
            }
            else
            {
                //CustHistoryLog 作废 2016-3-1 强斐
                //InsertCustHistoryLog(model);
            }

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
                    model_templateMappingNew.CreateUserID = BLL.Util.GetLoginUserID();
                    BLL.CustHistoryTemplateMapping.Instance.Insert(model_templateMappingNew);
                }
            }

            msg = "{'result':'yes','msg':'操作成功'}";
        }

        //同意转出
        private void AgreeTurnOut(out string msg)
        {
            msg = string.Empty;

            if (RequestTCSTaskID == "")
            {
                msg = "{'result':'no','msg':'没有任务ID，操作失败！'}";
                return;
            }

            Entities.CustHistoryInfo model_CustHistoryInfo = getModelCustHistoryInfoByTaskID(RequestTCSTaskID);
            if (model_CustHistoryInfo == null)
            {
                msg = "{'result':'no','msg':'没有找到该条任务记录，操作失败！'}";
                return;
            }

            //1 修改客户历史记录信息表：是否确定投诉 可修改,是否确认发送邮件 必须修改为true,任务状态 必须修改为“处理中”
            model_CustHistoryInfo.IsSendEmail = true;
            if (RequestIsComplaint != "")
            {
                model_CustHistoryInfo.IsComplaint = bool.Parse(RequestIsComplaint);
            }
            model_CustHistoryInfo.ProcessStatus = (int)Entities.EnumTaskStatus.TaskStatusNow;

            UpdateCustHistoryInfo(model_CustHistoryInfo);


            //2 修改当前受理人表中记录：状态改为1有效
            QueryTaskCurrentSolveUser query = new QueryTaskCurrentSolveUser();
            query.TaskID = model_CustHistoryInfo.TaskID;
            int count;
            DataTable dt = BLL.TaskCurrentSolveUser.Instance.GetTaskCurrentSolveUser(query, "", 1, 10000, out count);
            if (dt.Rows.Count == 0)
            {
                msg = "{'result':'no','msg':'没有找到转发受理人，操作失败！'}";
                return;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                UpdateTaskCurrentSolveUser(dt.Rows[i]["RecID"].ToString(), 1);
            }

            //3 发送邮件
            sendEmailByType(1, RequestTCSTaskID, 0, model_CustHistoryInfo.CustID, 0);

            //4 插入 客户历史信息处理日志 （动作记录）
            InsertCustHistoryLogByAction(model_CustHistoryInfo.TaskID, RequestComment, (int)Entities.Action.ActionAgreeApplyTurn);//动作：同意转出

            msg = "{'result':'yes','msg':'邮件发送成功，操作成功！'}";
        }

        //结束任务
        private void TaskEnd(out string msg)
        {
            msg = string.Empty;

            if (RequestCHITaskID == "")
            {
                msg = "'result':'no','msg','没有任务ID，操作失败！'}";
                return;
            }

            //1 当前受理人表中记录：为当前任务ID的受理人状态全置为0无效
            UpdateTaskCurrentSolveUserByTaskID();

            //2 客户历史记录信息表：是否确定投诉 可修改，任务状态为150003已处理 必须修改
            QueryCustHistoryInfo query = new QueryCustHistoryInfo();
            query.TaskID = RequestCHITaskID;
            int count;
            DataTable dt = BLL.CustHistoryInfo.Instance.GetCustHistoryInfo(query, "", 1, 10000, Entities.CustHistoryInfo.SelectFieldStr, out count);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CustHistoryInfo model = BLL.CustHistoryInfo.Instance.GetCustHistoryInfo(long.Parse(dt.Rows[i]["RecID"].ToString()));
                if (RequestIsComplaint != "")
                {
                    model.IsComplaint = bool.Parse(RequestIsComplaint);
                }
                model.ProcessStatus = (int)Entities.EnumTaskStatus.TaskStatusOver;
                UpdateCustHistoryInfo(model);
            }

            //3 插入 客户历史信息处理日志（动作记录） 
            InsertCustHistoryLogByAction(RequestCHITaskID, RequestComment, (int)Entities.Action.ActionTurnOver);  //动作：结束

            msg = "{'result':'yes','msg':'操作成功！'}";
        }

        //插入 客户历史信息处理日志（动作记录）
        public void InsertCustHistoryLogByAction(string taskID, string comment, int action)
        {
            CustHistoryLog model_Log = new CustHistoryLog();
            model_Log.TaskID = taskID;
            model_Log.Action = action;
            model_Log.Comment = comment;
            model_Log.SolveTime = DateTime.Now;
            model_Log.SolveUserID = BLL.Util.GetLoginUserID();
            model_Log.SolveUserEID = BLL.Util.GetLoginUserID();
            //CustHistoryLog 作废 2016-3-1 强斐
            //InsertCustHistoryLog(model_Log);

            string _action = string.Empty;
            switch (model_Log.Action)
            {
                case (int)Entities.Action.ActionSumbit: _action = "提交";
                    break;
                case (int)Entities.Action.ActionTurnOut: _action = "转出";
                    break;
                case (int)Entities.Action.ActionApplyTurn: _action = "申请转出";
                    break;
                case (int)Entities.Action.ActionTurnOver: _action = "结束";
                    break;
                case (int)Entities.Action.ActionAgreeApplyTurn: _action = "同意转出";
                    break;
            }
            BLL.Util.InsertUserLog("【CustHistoryLog】客户历史信息处理日志表【插入】【动作记录】任务ID：【" + model_Log.TaskID + "】受理人：【" + model_Log.SolveUserEID.GetValueOrDefault(-2) + "】动作：【" + _action + "】的记录");
        }

        //根据Hr系统EID找到权限系统EID
        private int getUserEID(int eid)
        {
            int User_ID = 0;
            //根据EID找到域账号
            string domainAccount = "暂不实现";
            //根据域账号找到权限系统该人员的主键
            User_ID = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetUserIDByNameDomainAccount(domainAccount);
            return User_ID;
        }

        //发送邮件 eid:hr系统人员主键 type: 1-同意转出；2-提交   
        private void sendEmailByType(int type, string taskID, int eid, string custID, int loginID)
        {
            //1 如果type=1 同意转出，则根据该条任务ID去邮件模板表找到对应的模板ID发送
            if (type == 1)
            {
                QueryCustHistoryTemplateMapping query_Template = new QueryCustHistoryTemplateMapping();
                query_Template.TaskID = taskID;
                int count;
                DataTable dt_Template = BLL.CustHistoryTemplateMapping.Instance.GetCustHistoryTemplateMapping(query_Template, "", 1, 10000, out count);
                for (int i = 0; i < dt_Template.Rows.Count; i++)
                {
                    sendEmail(taskID, dt_Template.Rows[i]["TemplateID"].ToString(), int.Parse(dt_Template.Rows[i]["SolveUserEID"].ToString()), custID);
                }
            }
            else if (type == 2)
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

        //发送邮件
        //1-发送邮件根据任务ID去CustHistoryTemplateMapping表找到对应的模板ID，再找到其模板内容；2-根据固定的模板发送邮件
        private void sendEmail(string taskID, string templateID, int eid, string custID)
        {
            //url
            string urlHead = ConfigurationUtil.GetAppSettingValue("TaskProcessUrl");

            //找到邮件接收人的邮箱 
            string userEmail = "暂不实现";
            //1 插入 发送邮件日志表 （记录）
            SendEmailLog model_Email = new SendEmailLog();
            string templateName = "暂不实现";//收件人姓名 
            string templateInfo = string.Empty; //模板内容 
            string urlInternal = urlHead + "/TaskManager/TaskProcess.aspx?TaskID=" + taskID;
            string urlExternal = urlHead + "/ExternalTask/ExternalTaskProcess.aspx?TaskID=" + taskID;
            int _templateID;   //模板ID
            string templateTitle = string.Empty;    //模板标题
            string templateLog = string.Empty;  //模板日志
            string templateUrl = string.Empty;  //模板链接格式

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

            model_Email.TemplateID = _templateID;
            model_Email.CustID = custID;
            model_Email.MailTo = userEmail;
            model_Email.SendTime = DateTime.Now;
            model_Email.SendContent = templateUrl;
            model_Email.CreateUserID = BLL.Util.GetLoginUserID();

            BLL.SendEmailLog.Instance.Insert(model_Email);

            //2根据 客户历史记录信息表中 模板ID来发送模板邮件
            BLL.EmailHelper.Instance.SendMail(templateName, templateUrl, "呼叫中心问题处理", new string[] { userEmail });

            BLL.Util.InsertUserLog("【SendEmailLog】发送邮件日志表【插入】模板ID：【" + model_Email.TemplateID + "】客户ID：【" + BLL.CustBasicInfo.Instance.GetCustBasicInfo(model_Email.CustID).CustName + "】接收人：【" + templateName + "】邮箱：【" + model_Email.MailTo + "】发送内容：【" + model_Email.SendContent + "】的记录【发送邮件成功】");
        }

        //根据转到受理人EID 获取到主键 返回PID
        private long getPIDByNextSolveEID(string taskID, int toNextSolveUserID)
        {
            long pid = 0;
            int count;
            QueryCustHistoryLog query_Log = new QueryCustHistoryLog();
            query_Log.TaskID = taskID;
            query_Log.ToNextSolveUserID = toNextSolveUserID;
            DataTable dt_Log = BLL.CustHistoryLog.Instance.GetCustHistoryLog(query_Log, " SolveTime Desc", 1, 10000, out count);
            if (dt_Log.Rows.Count > 0)
            {
                pid = long.Parse(dt_Log.Rows[0]["RecID"].ToString());
            }

            return pid;
        }

        //CustHistoryLog 作废 2016-3-1 强斐
        ////插入 CustHistoryLog表
        //private void InsertCustHistoryLog(CustHistoryLog model)
        //{
        //    if (model != null)
        //    {
        //        BLL.CustHistoryLog.Instance.Insert(model);//日志
        //        string action = string.Empty;
        //        switch (model.Action)
        //        {
        //            case (int)Entities.Action.ActionSumbit: action = "提交";
        //                break;
        //            case (int)Entities.Action.ActionTurnOut: action = "转出";
        //                break;
        //            case (int)Entities.Action.ActionApplyTurn: action = "申请转出";
        //                break;
        //            case (int)Entities.Action.ActionTurnOver: action = "结束";
        //                break;
        //            case (int)Entities.Action.ActionAgreeApplyTurn: action = "同意转出";
        //                break;
        //            case (int)Entities.Action.ActionCallOut: action = "呼出";
        //                break;
        //        }
        //        string status = string.Empty;
        //        switch (model.Status)
        //        {
        //            case (int)Entities.ProcessStatus.ProcessSolve: status = "已解决";
        //                break;
        //            case (int)Entities.ProcessStatus.ProcessUnresolved: status = "未解决";
        //                break;
        //            case (int)Entities.ProcessStatus.ProcessNotSolve: status = "不解决";
        //                break;
        //        }
        //        BLL.Util.InsertUserLog("【CustHistoryLog】客户历史信息处理日志表【插入】任务ID：【" + model.TaskID + "】受理人：【" + BLL.Util.GetEmployeeNameByEID(int.Parse(model.SolveUserEID.ToString())) + "】处理意见：【" + model.Comment + "】动作：【" + action + "】处理状态：【" + status + "】pid：【" + model.Pid + "】的记录");
        //    }
        //}

        //private void UpdateCustHistoryLog(CustHistoryLog model)
        //{
        //    if (model != null)
        //    {
        //        BLL.CustHistoryLog.Instance.Update(model);//日志
        //        string action = string.Empty;
        //        switch (model.Action)
        //        {
        //            case (int)Entities.Action.ActionSumbit: action = "提交";
        //                break;
        //            case (int)Entities.Action.ActionTurnOut: action = "转出";
        //                break;
        //            case (int)Entities.Action.ActionApplyTurn: action = "申请转出";
        //                break;
        //            case (int)Entities.Action.ActionTurnOver: action = "结束";
        //                break;
        //            case (int)Entities.Action.ActionAgreeApplyTurn: action = "同意转出";
        //                break;
        //            case (int)Entities.Action.ActionCallOut: action = "呼出";
        //                break;
        //        }
        //        string status = string.Empty;
        //        switch (model.Status)
        //        {
        //            case (int)Entities.ProcessStatus.ProcessSolve: status = "已解决";
        //                break;
        //            case (int)Entities.ProcessStatus.ProcessUnresolved: status = "未解决";
        //                break;
        //            case (int)Entities.ProcessStatus.ProcessNotSolve: status = "不解决";
        //                break;
        //        }
        //        BLL.Util.InsertUserLog("【CustHistoryLog】客户历史信息处理日志表【修改】任务ID：【" + model.TaskID + "】受理人：【" + BLL.Util.GetEmployeeNameByEID(int.Parse(model.SolveUserEID.ToString())) + "】处理意见：【" + model.Comment + "】动作：【" + action + "】处理状态：【" + status + "】pid：【" + model.Pid + "】的记录");
        //    }
        //}


        //修改：客户历史记录信息表CustHistoryInfo

        private void UpdateCustHistoryInfo(CustHistoryInfo model)
        {
            if (model != null)
            {
                Entities.CustHistoryInfo pre_Model = BLL.CustHistoryInfo.Instance.GetCustHistoryInfo(model.RecID);
                string preIsComplaint = pre_Model.IsComplaint.ToString();
                string preProcessStatus = pre_Model.ProcessStatus.ToString();
                string preIsSendEmail = pre_Model.IsSendEmail.ToString();

                BLL.CustHistoryInfo.Instance.Update(model);

                string updateStr = string.Empty;
                if (preIsComplaint != model.IsComplaint.ToString())
                {
                    updateStr += "【是否投诉】从【" + (preIsComplaint.ToLower() == "true" ? "是" : "否") + "】修改为【" + (model.IsComplaint.ToString().ToLower() == "true" ? "是" : "否") + "】";
                }
                if (preProcessStatus != model.ProcessStatus.ToString())
                {
                    updateStr += "【任务状态】从【" + getProcessStatusByID(int.Parse(preProcessStatus)) + "】修改为【" + getProcessStatusByID(int.Parse(model.ProcessStatus.ToString())) + "】";
                }
                if (preIsSendEmail != model.IsSendEmail.ToString())
                {
                    updateStr += "【是否确认发送邮件】从【" + (preIsSendEmail.ToLower() == "true" ? "是" : "否") + "】修改为【" + (model.IsSendEmail.ToString().ToLower() == "true" ? "是" : "否") + "】";
                }
                BLL.Util.InsertUserLog("【CustHistoryInfo】客户历史记录信息表【修改】主键：【" + model.RecID + "】的记录");
            }
        }

        //通过客户历史记录信息表任务ID得到任务状态名称
        private string getProcessStatusByID(int statusID)
        {
            string processStatus = string.Empty;
            switch (statusID)
            {
                case (int)Entities.EnumTaskStatus.TaskStatusNow: processStatus = "处理中";
                    break;
                case (int)Entities.EnumTaskStatus.TaskStatusOver: processStatus = "已处理";
                    break;
                case (int)Entities.EnumTaskStatus.TaskStatusWait: processStatus = "待处理";
                    break;
            }
            return processStatus;
        }

        //插入 ：当前受理人表 TaskCurrentSolveUser
        private void InsertTaskCurrentSolveUser(TaskCurrentSolveUser model)
        {
            if (model != null)
            {
                BLL.TaskCurrentSolveUser.Instance.Insert(model);
                BLL.Util.InsertUserLog("【TaskCurrentSolveUser】当前受理人表【插入】任务：【" + model.TaskID + "】当前受理人：【" + model.CurrentSolveUserEID.GetValueOrDefault(-2) + "】状态：【" + (model.Status == 0 ? "无效" : "有效") + "】的记录");
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
                    if (model.Status != status)
                    {
                        model.Status = status;
                        BLL.TaskCurrentSolveUser.Instance.Update(model);
                        BLL.Util.InsertUserLog("【TaskCurrentSolveUser】当前受理人表【修改】主键：【" + RecID + "】的记录，将状态【" + (model.Status == 0 ? "无效" : "有效") + "】修改为【" + (status == 0 ? "无效" : "有效") + "】");
                    }
                }
            }
        }

        //修改：根据任务ID修改当前受理人表 TaskCurrentSolveUser
        private void UpdateTaskCurrentSolveUserByTaskID()
        {
            if (RequestTCSTaskID != "")
            {
                BLL.TaskCurrentSolveUser.Instance.UpdateByTaskID(RequestTCSTaskID);
                BLL.Util.InsertUserLog("【TaskCurrentSolveUser】当前受理人表【修改】任务ID：【" + RequestTCSTaskID + "】的记录，将状态从【有效】修改为【无效】");
            }
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