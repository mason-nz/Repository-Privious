using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using System.Data;
using BitAuto.DSC.IM_2015.Core;
using BitAuto.DSC.IM_2015.Core.Messages;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Messages;
using BitAuto.DSC.IM_2015.Web.Channels;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers
{
    /// <summary>
    /// Summary description for Handler
    /// </summary>
    public class Handler : IHttpHandler, IRequiresSessionState
    {
        //用来测试服务器反应时间
        //private static Dictionary<string, DateTime> ditTest = new Dictionary<string, DateTime>();

        /// <summary>
        /// 消息发送人标识
        /// </summary>
        public string FromPrivateToken
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["FromPrivateToken"]); }
        }
        public string TargetAgent
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["targetagent"]); }
        }
        /// <summary>
        /// 网友ID
        /// </summary>
        public string WyId
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["wyid"]); }
        }
        /// <summary>
        /// 消息文本
        /// </summary>
        public string message
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["message"]); }
        }
        public string action
        {
            get
            {
                var ac = string.Empty;
                try
                {
                    ac = HttpContext.Current.Request.Form["action"];
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Info("未找到action参数:" + ex.Message.ToString());
                }
                return ac;
            }
        }

        private string CsIds
        {
            get { return HttpContext.Current.Request.Form["csids"]; }
        }

        private string IsReInit
        {
            get { return HttpContext.Current.Request.Form["isReInit"]; }
        }
        private string CsId
        {
            get { return HttpContext.Current.Request.Form["csid"]; }
        }

        //消息接收人标识
        public string SendToPrivateToken
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["SendToPrivateToken"]); }
        }
        //坐席与网友建立聊天标识
        public string AllocID
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["AllocID"]); }
        }
        /// <summary>
        /// 坐席状态
        /// </summary>
        public AgentStatus AgentState
        {
            get { return string.IsNullOrEmpty(HttpContext.Current.Request.Form["AgentState"]) == true ? Entities.AgentStatus.Leaveline : (AgentStatus)Enum.Parse(typeof(AgentStatus), HttpContext.Current.Request.Form["AgentState"].ToString()); }
        }
        /// <summary>
        /// 用户类型，1坐席，2网友
        /// </summary>
        public int UserType
        {
            get { return string.IsNullOrEmpty(HttpContext.Current.Request.Form["usertype"]) == true ? 0 : Convert.ToInt16(HttpContext.Current.Request.Form["usertype"]); }
        }
        /// <summary>
        /// 涞源
        /// </summary>
        public string UserReferURL
        {
            get { return HttpContext.Current.Request.Form["UserReferURL"]; }
        }
        public string SourceType
        {
            get { return HttpContext.Current.Request.Form["sourcetype"]; }
        }
        public string LoginID
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["loginid"]); }
        }
        /// <summary>
        /// EP 传递的url
        /// </summary>
        public string EPPostURL
        {
            get { return HttpContext.Current.Request.Form["EPPostURL"]; }
        }
        /// <summary>
        /// 易湃最后访问页面的title
        /// </summary>
        public string EPTitle
        {
            get { return HttpContext.Current.Request.Form["EPTitle"]; }
        }
        /// <summary>
        /// 消息类型
        /// </summary>
        public string MessageType
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["MessageType"]); }
        }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["FileName"]); }
        }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["FilePath"]); }
        }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["FileSize"]); }
        }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["FileType"]); }
        }

        /// <summary>
        /// 网友省份
        /// </summary>
        public string ProvinceID
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["ProvinceID"]); }
        }

        /// <summary>
        /// 网友城市
        /// </summary>
        public string CityID
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["CityID"]); }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="msg"></param>
        private void init(HttpContext context, out string msg)
        {
            msg = string.Empty;
            string loginid = string.Empty;
            string visitid = string.Empty;
            string Cookieid = string.Empty;
            CometClient cometmodel = null;
            try
            {
                if (!string.IsNullOrEmpty(FromPrivateToken))
                {
                    cometmodel = DefaultChannelHandler.StateManager.GetCometClient(FromPrivateToken);
                }
                else
                {
                    if (!string.IsNullOrEmpty(LoginID))
                    {
                        if (SourceType == BLL.Util.GetSourceType("惠买车"))
                        {
                            loginid = "hmc_" + loginid;

                        }
                        cometmodel = DefaultChannelHandler.StateManager.GetCometClient(loginid);
                    }
                }
            }
            catch (Exception)
            {
            }
            if (cometmodel != null)
            {
                msg = "您已经登录，不能重复登录！";
            }
            else
            {
                try
                {
                    string errormsg = string.Empty;
                    //长连接超时时间
                    int connectionTimeoutSeconds = BLL.Util.GetConnectionTimeoutSeconds();
                    int connectionIdleSeconds = 0;
                    if (UserType == (Int32)Entities.UserType.Agent)
                    {
                        //坐席没有长链接多久被移除对象
                        connectionIdleSeconds = BLL.Util.GetConnectionIdleSecondsAgent();

                        if (context.Session["truename"] == null)
                        {
                            context.Session["truename"] = "";
                        }

                        //DefaultChannelHandler.StateManager.InitializeClient(
                        //FromPrivateToken, FromPrivateToken, context.Session["truename"].ToString(), connectionTimeoutSeconds, connectionIdleSeconds, 0, "", UserType, "", "", "", out errormsg);

                        var cometT = DefaultChannelHandler.StateManager.InitializeClient_new(FromPrivateToken, FromPrivateToken, context.Session["truename"].ToString(), connectionTimeoutSeconds, connectionIdleSeconds, 0, UserType, "", "", "", "", "", "", IsReInit, out errormsg);
                        if (cometT == null)
                        {
                            msg = "坐席初始化失败，请联系管理员.";
                        }
                        else
                        {
                            msg = "loginok";
                        }

                    }
                    else
                    {

                        //网友多久没有长连接被移除
                        connectionIdleSeconds = BLL.Util.GetConnectionIdleSecondsUser();
                        //网友多久没发消息被移除
                        int sendmessageIdleSeconds = BLL.Util.GetSendMessageIdleSeconds();
                        CometClient cometClient = null;
                        //cometClient = DefaultChannelHandler.StateManager.InitializeClient(FromPrivateToken, FromPrivateToken, FromPrivateToken, connectionTimeoutSeconds, connectionIdleSeconds, sendmessageIdleSeconds, UserReferURL, UserType, EPTitle, EPKey, EPPostURL, out errormsg);
                        cometClient = DefaultChannelHandler.StateManager.InitializeClient_new(FromPrivateToken, FromPrivateToken, FromPrivateToken, connectionTimeoutSeconds, connectionIdleSeconds, sendmessageIdleSeconds, UserType, SourceType, LoginID, EPPostURL, EPTitle, ProvinceID, CityID, "", out errormsg);
                        if (cometClient == null && string.IsNullOrEmpty(errormsg))
                        {
                            msg = "eploginerror";
                        }
                        else if (cometClient == null && !string.IsNullOrEmpty(errormsg))
                        {
                            msg = "exists";
                        }
                        else if (cometClient != null && !string.IsNullOrEmpty(errormsg))
                        {
                            visitid = cometClient.Userloginfo.VisitID.ToString();
                            msg = errormsg;
                        }
                        else if (cometClient != null && string.IsNullOrEmpty(errormsg))
                        {
                            loginid = cometClient.PrivateToken;
                            visitid = cometClient.Userloginfo.VisitID.ToString();
                            if (cometClient.Userloginfo != null)
                            {
                                Cookieid = cometClient.Userloginfo.LoginID;
                            }
                            msg = "loginok";
                        }
                    }
                }
                catch (CometException ce)
                {
                    if (ce.MessageId == CometException.CometClientAlreadyExists)
                    {
                        msg = "User is already logged into the chat application.";
                    }
                }
            }
            msg = "{\"result\":\"" + msg + "\",\"loginid\":\"" + loginid + "\",\"visitid\":\"" + visitid + "\",\"Cookieid\":\"" + Cookieid + "\"}";
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="context"></param>
        private void SendMessage(out string msg)
        {
            //Debug.WriteLine(string.Format("开始接受到消息{0}", DateTime.Now.ToString("G")));
            msg = string.Empty;
            if (string.IsNullOrEmpty(FromPrivateToken))
            {
                msg = "消息发送人不能为空！";
            }
            else if (string.IsNullOrEmpty(SendToPrivateToken))
            {
                msg = "消息接收人不能为空！";
            }
            else if (string.IsNullOrEmpty(AllocID))
            {
                msg = "分配客服标识不能为空！";
            }
            else
            {
                //
                ////
                ////  get who the message is from
                ////消息接收人状态是否正常
                bool isExist = true;
                CometClient cometClientfrom = null;
                CometClient cometClientsendto = null;
                try
                {
                    cometClientfrom = DefaultChannelHandler.StateManager.GetCometClient(FromPrivateToken);
                    cometClientsendto = DefaultChannelHandler.StateManager.GetCometClient(SendToPrivateToken);
                    //Debug.WriteLine(string.Format("获取双方对象{0}", DateTime.Now.ToString("G")));

                }
                catch (Exception ex)
                {
                    isExist = false;
                }
                if (cometClientfrom != null)
                {
                    if (isExist == true && cometClientsendto != null && (cometClientsendto.Status == Entities.AgentStatus.Online || cometClientsendto.Status == Entities.AgentStatus.LeavingForAWhile))
                    {
                        cometClientfrom.SendMessageTime = System.DateTime.Now;
                        //给聊天记录入库线程消息实体赋值 add by qizq 2014-3-6
                        int _AllocID = 0;
                        int.TryParse(AllocID, out _AllocID);
                        object content = null;
                        int _messagetype = 0;
                        string messagetype = string.Empty;
                        int.TryParse(MessageType, out _messagetype);
                        //数据库类型
                        int _DbType = 0;
                        //如果不是发送文件,就封装文本实体


                        if (_messagetype == Convert.ToInt32(Entities.MessageType.MSatisfaction))
                        {
                            ChatMessage chatMessage = new ChatMessage();
                            chatMessage.CsID = _AllocID;
                            chatMessage.From = cometClientfrom.PrivateToken;
                            chatMessage.Message = message;
                            chatMessage.Time = System.DateTime.Now;
                            content = chatMessage;
                        }
                        //上传文件消息
                        else if (_messagetype == Convert.ToInt32(Entities.MessageType.MSendFile))
                        {
                            SendFileMessage sendfile = new SendFileMessage();
                            sendfile.CsID = _AllocID;
                            sendfile.From = cometClientfrom.PrivateToken;
                            sendfile.Message = message;
                            sendfile.Time = System.DateTime.Now;
                            sendfile.FilePath = FilePath;
                            sendfile.FileName = FileName;
                            long size = 0;
                            long.TryParse(FileSize, out size);
                            sendfile.FileSize = size;
                            sendfile.FileType = FileType;
                            content = sendfile;
                            _DbType = 1;
                        }
                        else
                        {
                            ChatMessage chatMessage = new ChatMessage();
                            chatMessage.CsID = _AllocID;
                            chatMessage.From = cometClientfrom.PrivateToken;
                            chatMessage.Message = message;
                            chatMessage.Time = System.DateTime.Now;
                            if (cometClientfrom.Userloginfo != null)
                            {
                                chatMessage.VisitID = cometClientfrom.Userloginfo.VisitID.ToString();
                                chatMessage.WYName = cometClientfrom.Userloginfo.UserName;
                                chatMessage.contractphone = cometClientfrom.Userloginfo.Phone;
                            }
                            content = chatMessage;
                            _messagetype = 2;
                        }

                        //Debug.WriteLine(string.Format("开始发送消息到对象{0}", DateTime.Now.ToString("G")));

                        messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), _messagetype);
                        DefaultChannelHandler.StateManager.SendMessage(SendToPrivateToken, messagetype, content, 1);



                        DefaultChannelHandler.StateManager.SendMessageInDB(_AllocID, UserType, message, _DbType);
                        msg = "{'result':'sendok','rectime':'" + DateTime.Now + "'}";

                    }
                    else
                    {
                        //msg = "消息: " + message + " 发送失败，对方已离线！";
                        msg = "SendToLeave";
                    }
                    //}

                }
                else
                {
                    //msg = "对话被中断或已结束！请退出后重新连接！";
                    msg = "ClientNotExists";
                }
            }
            msg = "{\"result\":\"" + msg + "\"}";
        }
        /// <summary>
        /// 坐席退出系统
        /// </summary>
        /// <param name="msg"></param>
        private void AgentCloseChat(out string msg)
        {
            msg = string.Empty;
            CometClient cometClient = null;
            try
            {
                cometClient = DefaultChannelHandler.StateManager.GetCometClient(FromPrivateToken);
            }
            catch (Exception)
            {
                //BLL.Loger.Log4Net.Info(string.Format("Handler AgentCloseChat方法中异常对象{0}", FromPrivateToken));
                msg = "没找到客服。";
                msg = "{\"result\":\"" + msg + "\"}";
                return;
            }
            if (cometClient != null)
            {
                //DefaultChannelHandler.StateManager.SetAgentState(cometClient.PrivateToken, Entities.AgentStatus.Leaveline);
                cometClient.Status = AgentStatus.Leaveline;
                DefaultChannelHandler.StateManager.ReAllocateOffLineAgentUsers(cometClient);
                DefaultChannelHandler.StateManager.KillIdleCometClient(cometClient);
                DefaultChannelHandler.StateManager.KillWaitRequest(cometClient.PrivateToken);
            }

            msg = "sendok";
            msg = "{\"result\":\"" + msg + "\"}";
        }

        private void AgentCloseSinglechat(out string msg)
        {
            msg = string.Empty;
            CometClient cometClient = null;
            try
            {
                cometClient = DefaultChannelHandler.StateManager.GetCometClient(SendToPrivateToken);
            }
            catch (Exception)
            {
                msg = "没找到网友。";
                msg = "{\"result\":\"" + msg + "\"}";
                return;
            }
            if (cometClient != null)
            {
                DefaultChannelHandler.StateManager.RemoveSingleUser(SendToPrivateToken, FromPrivateToken);
            }
            msg = "sendok";
            msg = "{\"result\":\"" + msg + "\"}";
        }

        private void checkstate(out string msg)
        {

            var strAgentState = string.Empty;
            strAgentState = DefaultChannelHandler.StateManager.GetCometClient(FromPrivateToken) != null
                ? " ,\"astatus\":1 "
                : " ,\"astatus\":0 ";

            if (string.IsNullOrEmpty(CsIds))
            { msg = "\"noids\""; }
            else
            {
                msg = DefaultChannelHandler.StateManager.CheckState(CsIds.Split(','));
            }

            //string strMsg = DefaultChannelHandler.StateManager.CheckState(CsIds.Split(','));
            msg = "{\"result\":\"sendok\",\"data\":" + msg + strAgentState + "}";
            //CheckState()
        }

        /// <summary>
        /// 客户退出
        /// </summary>
        /// <param name="msg"></param>
        private void UserCloseChat(out string msg)
        {
            msg = string.Empty;
            CometClient cometClient = null;
            //CometClient cometClientsendto = null;
            try
            {
                cometClient = DefaultChannelHandler.StateManager.GetCometClient(FromPrivateToken);
                if (cometClient != null)
                {
                    cometClient.Status = AgentStatus.Leaveline;
                    DefaultChannelHandler.StateManager.KillIdleCometClient(cometClient);
                }
                DefaultChannelHandler.StateManager.KillWaitRequest(FromPrivateToken);
                //if (!string.IsNullOrEmpty(SendToPrivateToken))
                //{
                //    cometClientsendto = DefaultChannelHandler.StateManager.GetCometClient(SendToPrivateToken);
                //}
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info(string.Format("Handler UserCloseChat方法中异常对象{0},异常原因：{1}", FromPrivateToken, ex.Message));
            }
            msg = "sendok";
            msg = "{\"result\":\"" + msg + "\"}";
        }

        /// <summary>
        /// 设置坐席状态
        /// </summary>
        private void SetAgentState(out string msg)
        {
            CometClient cometClient = null;
            try
            {
                cometClient = DefaultChannelHandler.StateManager.GetCometClient(FromPrivateToken);
                if (cometClient == null)
                {
                    msg = "没找到客服。";
                    msg = "{\"result\":\"" + msg + "\"}";
                    return;
                }
                DefaultChannelHandler.StateManager.SetAgentState(cometClient.PrivateToken, AgentState);
                //add by qizq 2015-1-7 加监控入库日志，状态修改日志
                Entities.UserActionLog model = new UserActionLog();
                model.CreateTime = System.DateTime.Now;
                int _CreateUserID = 0;
                if (int.TryParse(cometClient.PrivateToken, out _CreateUserID))
                {
                    model.CreateUserID = _CreateUserID;
                }
                //操作人是坐席
                model.OperUserType = 1;
                //坐席修改状态
                model.LogInType = 10;
                model.LogInfo = string.Format("坐席{0}，修改状态为{1}", cometClient.PrivateToken, AgentState == Entities.AgentStatus.Leaveline ? "离线" : (AgentState == Entities.AgentStatus.Online ? "在线" : "暂离"));
                //BLL.UserActionLog.Instance.Insert(model);
                BulkInserUserActionThread.EnQueueActionLogs(model);
                //
                if (AgentState == Entities.AgentStatus.Leaveline)
                {
                    DefaultChannelHandler.StateManager.ReAllocateOffLineAgentUsers(cometClient);
                }
            }
            catch (Exception ex)
            {
                msg = "{\"result\":\"" + ex.Message + "\"}";
                return;
            }
            //msg = "sendok";
            msg = "{\"result\":\"sendok\"}";
        }

        /// <summary>
        /// 从枚举类型中获取所有的坐席状态
        /// </summary>
        /// <param name="msg"></param>
        private void GetAllAgentState(out string msg)
        {
            try
            {
                DataTable dt = null;
                dt = BitAuto.DSC.IM_2015.BLL.Util.GetEnumDataTable(typeof(Entities.AgentStatus));
                string tmsg = "";
                if (dt != null && dt.Rows.Count > 0)
                {
                    tmsg = "[";
                    foreach (DataRow row in dt.Rows)
                    {
                        tmsg += "{'name':'" + row["name"] + "','value':'" + row["value"] + "'},";
                    }
                    if (tmsg.EndsWith(","))
                        tmsg = tmsg.Substring(0, tmsg.Length - 1);

                    tmsg += "]";
                }
                msg = tmsg;
            }
            catch (Exception)
            {
                msg = "";
            }
            msg = "{\"result\":\"" + msg + "\"}";
        }

        /// <summary>
        /// 获取指定坐席的状态(内存)中
        /// </summary>
        /// <param name="msg"></param>
        private void GetAgentSatetById(out string msg)
        {
            msg = string.Empty;
            try
            {
                msg = ((int)DefaultChannelHandler.StateManager.GetCometClient(FromPrivateToken).Status).ToString();
            }
            catch
            {
                msg = "1";
            }
            msg = "{\"result\":\"" + msg + "\"}";
        }

        /// <summary>
        /// 重新分配坐席
        /// </summary>
        /// <param name="msg"></param>
        private void Resetagent(out string msg)
        {
            CometClient cometClient = null;
            try
            {
                cometClient = DefaultChannelHandler.StateManager.GetCometClient(FromPrivateToken);
                //cometClientsendto = DefaultChannelHandler.StateManager.GetCometClient(SendToPrivateToken);
            }
            catch (Exception)
            {
            }
            if (cometClient != null)
            {
                //重新分配不初始化
                //DefaultChannelHandler.StateManager.agentallocInQueue(FromPrivateToken);
                //long u = 0;
                //long.TryParse(FromPrivateToken, out u);
                //测试用，联调改成正式的
                //DefaultChannelHandler.StateManager.MainAllocateAgent4Test(Convert.ToInt32(cometClient.CityGroupId), cometClient.MemberCode, u);
                //TODO　重新分配
                //DefaultChannelHandler.StateManager.MainAllocateAgent(cometClient.DistrictId, cometClient.MemberCode, u);
                msg = "sendok";
            }
            else
            {
                msg = "Initializeok";
            }
            msg = "{\"result\":\"" + msg + "\"}";
        }
        /// <summary>
        /// 判断实体是否存在
        /// </summary>
        /// <param name="msg"></param>
        private void IsExists(out string msg)
        {
            msg = string.Empty;
            CometClient cometClient = null;
            try
            {
                cometClient = DefaultChannelHandler.StateManager.GetCometClient(FromPrivateToken);
            }
            catch (Exception)
            {
            }
            if (cometClient != null)
            {
                msg = "IsExists";
            }
            else
            {
                msg = "NoExists";
            }
            msg = "{\"result\":\"" + msg + "\"}";
        }
        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "text/plain";
            string msg = "";
            //BLL.Loger.Log4Net.Info(action + " Start...          " + DateTime.Now.ToString("O"));

            /* 
             var p = Process.GetCurrentProcess();
            BLL.Loger.Log4Net.Info(string.Format("PeakWorkingSet64:{0},Responding:{1},UserProcessorTime:{2},TotalProcessorTime:{3},Threadscount:{4}", p.PeakWorkingSet64, p.Responding, p.UserProcessorTime,
                p.TotalProcessorTime, p.Threads.Count));
            var tT = FromPrivateToken + action;
            if (ditTest.ContainsKey(tT))
            {
                ditTest[tT] = DateTime.Now;
            }
            else
            {
                ditTest.Add(tT, DateTime.Now);
            }
            */
            switch (action)
            {
                case "init":
                    init(context, out msg);
                    break;
                case "sendmessage":
                    SendMessage(out msg);
                    break;
                case "closechat":
                    AgentCloseChat(out msg);
                    break;
                case "userclosechat":
                    UserCloseChat(out msg);
                    break;
                case "setagentstate":
                    SetAgentState(out msg);
                    break;
                case "getagentsatetbyid":
                    GetAgentSatetById(out msg);
                    break;
                case "getallagentstate":
                    GetAllAgentState(out msg);
                    break;
                case "resetagent":
                    Resetagent(out msg);
                    break;
                case "closesinglechat":
                    AgentCloseSinglechat(out msg);
                    break;
                case "IsExists":
                    IsExists(out msg);
                    break;
                case "getfristhistroy":
                    GetFristHistroy(out msg);
                    break;
                case "checkstate":
                    checkstate(out msg);
                    break;
                case "checkorder":
                    checkorder(out msg);
                    break;
                case "transferagent":
                    TransferAgentHandLers(out msg);
                    break;
                case "cominquene":
                    CominQuene(out msg);
                    break;
                case "getservertime":
                    GetServerTime(out msg);
                    break;
                case "pressuretest":
                    PressureTest();
                    break; ;
                default:
                    break;
            }

            //else if (action == "GetChatMessageLog")
            //{
            //    //msg = BLL.ChatMessageLog.Instance.GetChatMessageLog(Convert.ToInt32(AllocID));
            //    //if (!string.IsNullOrEmpty(msg))
            //    //{
            //    //    context.Response.Write("{\"result\":" + msg + "}");
            //    //    return;
            //    //}
            //}
            //else if (action == "GetDetailInfo")
            //{
            //    //msg = BLL.AllocationAgent.Instance.GetAllocationAgent(Convert.ToInt32(AllocID));
            //}
            //BLL.Loger.Log4Net.Info(action + " End.");
            context.Response.Write(msg);
            //BLL.Loger.Log4Net.Info(string.Format("{0}: {1}", tT, DateTime.Now - ditTest[tT]));

        }

        private void PressureTest()
        {
            //DefaultChannelHandler.StateManager.PressureTest();
        }

        private void GetServerTime(out string msg)
        {
            msg = "{\"result\":" + (DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1))).TotalMilliseconds.ToString() + "}";
        }

        /// <summary>
        /// 进入队列
        /// </summary>
        /// <param name="msg"></param>
        private void CominQuene(out string msg)
        {
            msg = string.Empty;
            if (!string.IsNullOrEmpty(SourceType) && !string.IsNullOrEmpty(FromPrivateToken))
            {
                try
                {
                    var comet = DefaultChannelHandler.StateManager.GetCometClient(FromPrivateToken);
                    if (comet != null)
                    {
                        comet.LastRequestTime = DateTime.Now;
                        comet.LastMessageTime = DateTime.Now;
                        comet.Status = AgentStatus.Online;
                        int rint = DefaultChannelHandler.StateManager.EnQueueWaitAgent(SourceType, FromPrivateToken);
                        msg = rint.ToString();
                    }
                    else
                    {
                        msg = "未找到对象";
                    }

                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }
            }
            else
            {
                msg = "1";
            }
            msg = "{\"result\":\"" + msg + "\"}";

            //调用绪龙方法
        }
        private void TransferAgentHandLers(out string msg)
        {
            msg = "{\"result\":\"0\"}";
            try
            {
                DefaultChannelHandler.StateManager.TransferAgent(FromPrivateToken, TargetAgent, WyId);
            }
            catch (Exception ex)
            {
                msg = "{\"result\":\"" + ex.Message + "\"}";
            }

        }

        private void checkorder(out string msg)
        {
            msg = "0";
            try
            {
                if (string.IsNullOrEmpty(CsId))
                {
                    msg = "{\"result\":\"error\",\"data\":\"没有找到会话，参数错误.\"}";
                    return;
                }
                var dt = BLL.Conversations.Instance.CheckConversationOrderCustInfo(CsId);

                if (dt == null || dt.Rows.Count == 0)
                {
                    msg = "{\"result\":\"error\",\"data\":\"没有找到会话，请检查参数.\"}";
                    BLL.Loger.Log4Net.Info(msg + " CSID：" + CsId);
                    return;
                }
                var dr = dt.Rows[0];
                msg = "{\"result\":\"sendok\",\"OrderID\":\"" + dr["OrderID"].ToString() + "\",\"CustID\":\"" + dr["CustID"].ToString() + "\"}";
            }
            catch (Exception ex)
            {
                msg = "{\"result\":\"error\",\"data\":" + ex.Message + "\"}";
            }
        }
        //取前六条历史记录
        private void GetFristHistroy(out string msg)
        {
            msg = string.Empty;
            Entities.QueryConversations query = new QueryConversations();
            //int _loginID = 0;
            //int.TryParse(FromPrivateToken, out _loginID);
            int _userID = 0;
            int.TryParse(SendToPrivateToken, out _userID);
            query.UserID = _userID;
            query.LoginID = FromPrivateToken;
            int RecordCount = 0;
            DataTable dt = BLL.Conversations.Instance.GetConversationHistoryData(query, "", 1, 6, out RecordCount);
            if (dt != null && dt.Rows.Count > 0)
            {
                msg = "[";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DateTime sendtime = System.DateTime.Now;
                    DateTime.TryParse(dt.Rows[i]["CreateTime"].ToString(), out sendtime);
                    string timestr = sendtime.Hour.ToString() + "：" + sendtime.Minute.ToString();
                    msg += "{\"Sender\":\"" + dt.Rows[i]["Sender"].ToString() + "\",\"CreateTime\":\"" + timestr + "\",\"Content\":\"" + dt.Rows[i]["Content"].ToString().Trim().Replace("\"", "'") + "\",\"AgentNum\":\"" + dt.Rows[i]["AgentNum"].ToString() + "\"},";
                }
                msg = msg.Substring(0, msg.Length - 1);
                msg += "]";

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