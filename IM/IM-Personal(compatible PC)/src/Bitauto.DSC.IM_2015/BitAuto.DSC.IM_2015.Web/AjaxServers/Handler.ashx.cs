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
using BitAuto.DSC.IM_2015.MainInterface;
using BitAuto.DSC.IM_2015.Web.Channels;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using BitAuto.DSC.IM_2015.WebService.CC;
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
        /// 网友验证答案
        /// </summary>
        public string DaAn
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["DaAn"]); }
        }
        /// <summary>
        /// 网友验证标识
        /// </summary>
        public string WYGUID
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["WYGUID"]); }
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

        /// <summary>
        /// 业务类型ID，cc工单标签相关
        /// </summary>
        private string BusinessTypeID
        {
            get
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["businesstypeid"]))
                {
                    return System.Web.HttpContext.Current.Request.Form["businesstypeid"];
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        private string TagId
        {

            get
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["tgd"]))
                {
                    return System.Web.HttpContext.Current.Request.Form["tgd"];
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string TagName
        {
            get
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["tgn"]))
                {
                    return HttpContext.Current.Server.UrlDecode(System.Web.HttpContext.Current.Request.Form["tgn"]);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        private string CustName
        {
            get
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["custname"]))
                {
                    return HttpContext.Current.Server.UrlDecode(System.Web.HttpContext.Current.Request.Form["custname"]);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        private string CallNum
        {
            get { return HttpContext.Current.Request.Form["callnum"]; }
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
        /// 坐席id
        /// </summary>
        public string AgentID
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["AgentID"]); }
        }
        ///// <summary>
        ///// 消息接受者ip
        ///// </summary>
        //public string IISIP
        //{
        //    get { 
        //        if(
        //        return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["IISIP"]); }
        //}

        /// <summary>
        /// 最后访问页面url
        /// </summary>
        public string UserReferURLWap
        {
            get { return System.Web.HttpContext.Current.Request.ServerVariables["HTTP_REFERER"]; }
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

            if (UserType == (Int32)Entities.UserType.Agent)
            {
                try
                {
                    if (!string.IsNullOrEmpty(FromPrivateToken))
                    {
                        int nAgentId = 0;
                        if (FromPrivateToken.IndexOf("@") < 0)
                        {
                            nAgentId = Convert.ToInt32(FromPrivateToken);
                        }
                        else
                        {
                            nAgentId = Convert.ToInt32(FromPrivateToken.Substring(0, FromPrivateToken.LastIndexOf("@", StringComparison.CurrentCultureIgnoreCase)));
                        }
                        var channel = DefaultChannelHandler.StateManager.GetAutoWCFClient();

                        if (channel.IsAgentExists(nAgentId))
                        {
                            msg = "您已经登录，不能重复登录！";
                            msg = "{\"result\":\"" + msg + "\",\"loginid\":\"" + loginid + "\",\"visitid\":\"" + visitid +
                                  "\",\"Cookieid\":\"" + Cookieid + "\"}";
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {

                    BLL.Loger.Log4Net.Error("Init 出错            " + ex.Message);
                    msg = "初始化失败！" + ex.Message;
                    msg = "{\"result\":\"" + msg + "\",\"loginid\":\"" + loginid + "\",\"visitid\":\"" + visitid +
                          "\",\"Cookieid\":\"" + Cookieid + "\"}";
                    return;
                }

            }

            string strFromToken = string.Empty;
            try
            {
                //如果WCF内存中不存在，但在IIS中存在的对象，直接删除。
                //var cAgent = DefaultChannelHandler.StateManager.GetCometClient(FromPrivateToken);
                //if (cAgent != null)
                //{
                //    if (cAgent.Type == 1)
                //    {
                //        DefaultChannelHandler.StateManager.AgentQuit(cAgent);
                //    }
                //    else
                //    {
                //        DefaultChannelHandler.StateManager.NetFriendQuit(cAgent);
                //    }
                //}

                string errormsg = string.Empty;
                //长连接超时时间
                int connectionTimeoutSeconds = BLL.Util.GetConnectionTimeoutSeconds();
                int connectionIdleSeconds = 0;
                if (UserType == (Int32)Entities.UserType.Agent)
                {
                    //坐席没有长链接多久被移除对象
                    connectionIdleSeconds = BLL.Util.GetConnectionIdleSecondsAgent();

                    //DefaultChannelHandler.StateManager.InitializeClient(
                    //FromPrivateToken, FromPrivateToken, context.Session["truename"].ToString(), connectionTimeoutSeconds, connectionIdleSeconds, 0, "", UserType, "", "", "", out errormsg);
                    strFromToken = FromPrivateToken.IndexOf("@") > 0 ? FromPrivateToken : (FromPrivateToken + "@" + BLL.Util.IpToLong().ToString());
                    var cometT = DefaultChannelHandler.StateManager.InitializeClient_new(FromPrivateToken, strFromToken, BLL.Util.GetLoginRealName(), connectionTimeoutSeconds, connectionIdleSeconds, 0, UserType, "", "", "", "", "", "", IsReInit, out errormsg, false);
                    if (cometT == null)
                    {
                        if (string.IsNullOrWhiteSpace(errormsg))
                        {
                            msg = "坐席初始化失败，请联系管理员.";
                        }
                        else
                        {
                            msg = errormsg;
                        }
                    }
                    else
                    {
                        msg = "loginok";
                    }

                }
                else
                {
                    //如果是wap站
                    bool isWap = false;
                    isWap = BLL.Util.CheckMurl(UserReferURLWap);
                    //isWap = true;

                    //不是wap站
                    if (isWap == false)
                    {
                        //验证网友是否通过图片验证
                        System.Web.Caching.Cache objCache = HttpRuntime.Cache;
                        if (string.IsNullOrEmpty(WYGUID) || objCache[WYGUID + "_yanzheng"] == null)
                        {
                            msg = "{\"result\":\"eploginerror\"}";
                            return;
                        }
                        //
                    }


                    //网友多久没有长连接被移除
                    connectionIdleSeconds = BLL.Util.GetConnectionIdleSecondsUser();
                    //网友多久没发消息被移除
                    int sendmessageIdleSeconds = BLL.Util.GetSendMessageIdleSeconds();
                    CometClient cometClient = null;

                    //验证是否达到最大接待量默认是5000
                    int MaxNetFrind = BLL.Util.GetMaxNetFrinedNumber();
                    int HaveNetFrindCount = DefaultChannelHandler.StateManager.GetAllNetFrindHaveQuene();
                    //如果超过了最大接待网友数则
                    if (HaveNetFrindCount > MaxNetFrind)
                    {
                        msg = "{\"result\":\"moreperson\"}";
                        return;
                    }


                    //cometClient = DefaultChannelHandler.StateManager.InitializeClient(FromPrivateToken, FromPrivateToken, FromPrivateToken, connectionTimeoutSeconds, connectionIdleSeconds, sendmessageIdleSeconds, UserReferURL, UserType, EPTitle, EPKey, EPPostURL, out errormsg);
                    cometClient = DefaultChannelHandler.StateManager.InitializeClient_new(FromPrivateToken, FromPrivateToken, FromPrivateToken, connectionTimeoutSeconds, connectionIdleSeconds, sendmessageIdleSeconds, UserType, SourceType, LoginID, EPPostURL, EPTitle, ProvinceID, CityID, "", out errormsg, isWap);
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
            //}
            msg = "{\"result\":\"" + msg + "\",\"loginid\":\"" + loginid + "\",\"visitid\":\"" + visitid + "\",\"Cookieid\":\"" + Cookieid + "\",\"fm\":\"" + strFromToken + "\"}";
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
                string IISIP = string.Empty;
                if (SendToPrivateToken.IndexOf("@") > -1)
                {
                    IISIP = SendToPrivateToken.Split('@')[1];
                }

                ////  get who the message is from
                ////消息接收人状态是否正常
                ProxyAgentClient agentClient = null;
                ProxyNetFriend netFriend = null;
                int Type = 0;
                try
                {
                    DefaultChannelHandler.StateManager.GetCometClient(FromPrivateToken, ref netFriend, ref agentClient, ref Type);
                    //cometClientsendto = DefaultChannelHandler.StateManager.GetCometClient(SendToPrivateToken);
                    //Debug.WriteLine(string.Format("获取双方对象{0}", DateTime.Now.ToString("G")));

                }
                catch (Exception ex)
                {
                    msg = "ClientNotExists";
                }
                if (agentClient == null && netFriend == null)
                {
                    msg = "ClientNotExists";
                }
                else
                {
                    //cometClientfrom.SendMessageTime = System.DateTime.Now;
                    //给聊天记录入库线程消息实体赋值 add by qizq 2014-3-6
                    int _AllocID = 0;
                    int.TryParse(AllocID, out _AllocID);

                    string content = string.Empty;
                    int _messagetype = 0;
                    string messagetype = string.Empty;
                    int.TryParse(MessageType, out _messagetype);
                    //数据库类型
                    int _DbType = 0;
                    string msgContent = message;
                    msgContent = Regex.Replace(msgContent, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
                    //如果不是发送文件,就封装文本实体
                    if (_messagetype == Convert.ToInt32(Entities.MessageType.MSatisfaction))
                    {
                        ChatMessage chatMessage = new ChatMessage();
                        chatMessage.CsID = _AllocID;
                        chatMessage.From = FromPrivateToken;
                        chatMessage.Message = msgContent;
                        chatMessage.Time = System.DateTime.Now;

                        content = BLL.Util.DataContractObject2Json(chatMessage, typeof(ChatMessage));
                        //JsonConvert.SerializeObject(chatMessage);

                    }
                    //上传文件消息
                    else if (_messagetype == Convert.ToInt32(Entities.MessageType.MSendFile))
                    {
                        SendFileMessage sendfile = new SendFileMessage();
                        sendfile.CsID = _AllocID;
                        sendfile.From = FromPrivateToken;
                        sendfile.Message = msgContent;
                        sendfile.Time = System.DateTime.Now;
                        sendfile.FilePath = FilePath;
                        sendfile.FileName = FileName;
                        long size = 0;
                        long.TryParse(FileSize, out size);
                        sendfile.FileSize = size;
                        sendfile.FileType = FileType;

                        content = BLL.Util.DataContractObject2Json(sendfile, typeof(SendFileMessage)); //JsonConvert.SerializeObject(sendfile);

                        _DbType = 1;
                    }
                    else
                    {
                        ChatMessage chatMessage = new ChatMessage();
                        chatMessage.CsID = _AllocID;
                        chatMessage.From = FromPrivateToken;
                        chatMessage.Message = msgContent;
                        chatMessage.Time = System.DateTime.Now;
                        if (Type == 2 && netFriend != null)
                        {
                            chatMessage.VisitID = netFriend.VisitID.ToString();
                            chatMessage.WYName = netFriend.NetFName;
                            chatMessage.contractphone = netFriend.contractphone;
                        }
                        content = BLL.Util.DataContractObject2Json(chatMessage, typeof(ChatMessage));//JsonConvert.SerializeObject(chatMessage);
                        _messagetype = 2;
                    }

                    var cometMessage = new CometMessage()
                    {
                        Name = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), _messagetype),
                        Contents = content,
                        //CreateDateTime = DateTime.Now,
                        FromToken = FromPrivateToken,
                        ToToken = SendToPrivateToken,
                        ProcessTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                        IISIP = IISIP
                    };
                    //messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), _messagetype);
                    //DefaultChannelHandler.StateManager.SendMessage(FromPrivateToken, SendToPrivateToken, messagetype, content, 1);
                    DefaultChannelHandler.StateManager.SendMessage(cometMessage, 1);
                    DefaultChannelHandler.StateManager.SendMessageInDB(_AllocID, UserType, msgContent, _DbType);
                    msg = "{'result':'sendok','rectime':'" + DateTime.Now + "'}";
                }
            }
            msg = "{\"result\":\"" + msg + "\"}";
        }

        /// <summary>
        /// 坐席退出系统
        /// </summary>
        /// <param name="msg"></param>
        private void AgentQuit(out string msg)
        {
            msg = string.Empty;
            ProxyAgentClient agentClient = null;
            ProxyNetFriend netFrind = null;
            int Type = 0;
            try
            {
                DefaultChannelHandler.StateManager.GetCometClient(FromPrivateToken, ref netFrind, ref agentClient, ref Type);
            }
            catch (Exception)
            {
                //BLL.Loger.Log4Net.Info(string.Format("Handler AgentCloseChat方法中异常对象{0}", FromPrivateToken));
                msg = "没找到客服。";
                msg = "{\"result\":\"" + msg + "\"}";
                return;
            }
            if (agentClient != null)
            {
                DefaultChannelHandler.StateManager.AgentQuit(agentClient);
            }

            msg = "sendok";
            msg = "{\"result\":\"" + msg + "\"}";
        }

        /// <summary>
        /// 坐席关闭网友
        /// </summary>
        /// <param name="msg"></param>
        private void AgentCloseSinglechat(out string msg)
        {
            msg = string.Empty;
            ProxyAgentClient agentClient = null;
            ProxyNetFriend netFrind = null;
            int Type = 0;
            try
            {
                DefaultChannelHandler.StateManager.GetCometClient(FromPrivateToken, ref netFrind, ref agentClient, ref Type);
                if (agentClient != null)
                {
                    DefaultChannelHandler.StateManager.AgentRemoveNetFriend(agentClient, SendToPrivateToken);
                }
            }
            catch (Exception ex)
            {
                //msg = "没找到网友。";
                msg = "{\"result\":\"" + ex.Message + "\"}";
                return;
            }

            msg = "sendok";
            msg = "{\"result\":\"" + msg + "\"}";
        }

        private void checkstate(out string msg)
        {

            var strAgentState = string.Empty;
            //strAgentState = DefaultChannelHandler.StateManager.GetCometClient(FromPrivateToken) != null
            //    ? " ,\"astatus\":1 "
            //    : " ,\"astatus\":0 ";

            //if (string.IsNullOrEmpty(CsIds))
            //{ msg = "\"noids\""; }
            //else
            //{

            int agentId = Convert.ToInt32(FromPrivateToken.Substring(0, FromPrivateToken.IndexOf("@")));
            msg = DefaultChannelHandler.StateManager.CheckState(agentId, CsIds);
            /*
            if (string.IsNullOrWhiteSpace(msg))
            {
                if (!string.IsNullOrWhiteSpace(CsIds))
                {
                    var lstTokens = CsIds.Split(',');
                    StringBuilder sbids = new StringBuilder();
                    string strToken, strCsid;
                    int nIndex = -1;

                    for (int i = 0; i < lstTokens.Length; i++)
                    {
                        nIndex = lstTokens[i].LastIndexOf("_");
                        strCsid = lstTokens[i].Substring(0, nIndex);
                        strToken = lstTokens[i].Substring(nIndex + 1);
                        sbids.Append("{");
                        sbids.Append(string.Format("\"csid\":\"{0}\",\"state\":\"{1}\"", strCsid, 0));
                        sbids.Append("},");
                    }
                    if (sbids.Length > 0)
                    {
                        msg = "[" + sbids.Remove(sbids.Length - 1, 1) + "]";
                    }
                }
                else
                {
                    msg += "[],\"astatus\":0 ";
                }
                
            }*/


            //string strMsg = DefaultChannelHandler.StateManager.CheckState(CsIds.Split(','));
            msg = "{\"result\":\"sendok\",\"data\":" + msg + "}";
            //CheckState()
        }

        /// <summary>
        /// 网友退出
        /// </summary>
        /// <param name="msg"></param>
        private void NetFriendQuit(out string msg)
        {
            msg = string.Empty;
            CometClient cometClient = null;
            //CometClient cometClientsendto = null;
            try
            {
                ProxyNetFriend netFrind = null;
                netFrind = DefaultChannelHandler.StateManager.GetNetFrind(FromPrivateToken);
                if (netFrind != null)
                {
                    //cometClient.Status = AgentStatus.Leaveline;
                    DefaultChannelHandler.StateManager.NetFriendQuit(netFrind);
                }
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
            //CometClient cometClient = null;
            try
            {

                var nResult = DefaultChannelHandler.StateManager.SetAgentState(FromPrivateToken, AgentState);
                if (nResult == -1)
                {
                    msg = "没找到客服。";
                    msg = "{\"result\":\"" + msg + "\"}";
                }
                else if (nResult == -2)
                {
                    msg = "更改失败，请联系管理员。";
                    msg = "{\"result\":\"" + msg + "\"}";
                }

                #region 写日志

                //add by qizq 2015-1-7 加监控入库日志，状态修改日志
                Entities.UserActionLog model = new UserActionLog();
                model.CreateTime = System.DateTime.Now;
                int _CreateUserID = 0;
                if (int.TryParse(FromPrivateToken.Split(',')[0], out _CreateUserID))
                {
                    model.CreateUserID = _CreateUserID;
                }
                //操作人是坐席
                model.OperUserType = 1;
                //坐席修改状态
                model.LogInType = 10;
                //model.LogInfo = string.Format("坐席{0}，修改状态为{1}", cometClient.PrivateToken, AgentState == Entities.AgentStatus.Leaveline ? "离线" : "在线");
                model.LogInfo = string.Format("坐席{0}，修改状态为{1}", FromPrivateToken.Split(',')[0], AgentState == Entities.AgentStatus.Leaveline ? "离线" : (AgentState == Entities.AgentStatus.Online ? "在线" : "暂离"));
                //BLL.UserActionLog.Instance.Insert(model);
                BulkInserUserActionThread.EnQueueActionLogs(model);

                #endregion

            }
            catch (Exception ex)
            {
                msg = "{\"result\":\"" + ex.Message + "\"}";
                return;
            }
            //msg = "sendok";
            msg = "{\"result\":\"sendok\"}";
            //cometClient = null;
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

        /*
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
        */

        ///// <summary>
        ///// 重新分配坐席
        ///// </summary>
        ///// <param name="msg"></param>
        //private void Resetagent(out string msg)
        //{
        //    CometClient cometClient = null;
        //    try
        //    {
        //        cometClient = DefaultChannelHandler.StateManager.GetCometClient(FromPrivateToken);
        //        //cometClientsendto = DefaultChannelHandler.StateManager.GetCometClient(SendToPrivateToken);
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    if (cometClient != null)
        //    {
        //        //重新分配不初始化
        //        //DefaultChannelHandler.StateManager.agentallocInQueue(FromPrivateToken);
        //        //long u = 0;
        //        //long.TryParse(FromPrivateToken, out u);
        //        //测试用，联调改成正式的
        //        //DefaultChannelHandler.StateManager.MainAllocateAgent4Test(Convert.ToInt32(cometClient.CityGroupId), cometClient.MemberCode, u);
        //        //TODO　重新分配
        //        //DefaultChannelHandler.StateManager.MainAllocateAgent(cometClient.DistrictId, cometClient.MemberCode, u);
        //        msg = "sendok";
        //    }
        //    else
        //    {
        //        msg = "Initializeok";
        //    }
        //    msg = "{\"result\":\"" + msg + "\"}";
        //}
        /// <summary>
        /// 判断实体是否存在
        /// </summary>
        /// <param name="msg"></param>
        private void IsExists(out string msg)
        {
            msg = string.Empty;

            var channel = DefaultChannelHandler.StateManager.GetAutoWCFClient();
            if (channel.IsAgentExists(Convert.ToInt32(FromPrivateToken)))
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

            switch (action)
            {
                case "init":
                    init(context, out msg);
                    break;
                case "sendmessage":
                    SendMessage(out msg);
                    break;
                case "closechat":
                    AgentQuit(out msg);
                    break;
                case "userclosechat":
                    NetFriendQuit(out msg);
                    break;
                case "setagentstate":
                    SetAgentState(out msg);
                    break;
                //case "getagentsatetbyid":
                //    GetAgentSatetById(out msg);
                //    break;
                case "getallagentstate":
                    GetAllAgentState(out msg);
                    break;
                //case "resetagent":
                //    //Resetagent(out msg);
                //    break;
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
                    ReturnMessage returnMessage = null;
                    checkorder(out returnMessage);
                    msg = Newtonsoft.Json.JavaScriptConvert.SerializeObject(returnMessage);
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
                case "deletewcfagent":
                    DeleteWcfAgent(out msg);
                    break;
                case "udpt":
                    UpDateConversationTag(out msg);
                    break;
                case "recordlog":
                    RecordLog(out msg);
                    break;
                case "getyanzhengimage":
                    TokeYanZhengGif(out msg);
                    break;
                case "yanzhengdanan":
                    YanZhengDaAn(out msg);
                    break;
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
        /// <summary>
        /// 根据agentid，把wcf坐席移除
        /// </summary>
        /// <param name="agentid"></param>
        /// <param name="msg"></param>
        private void DeleteWcfAgent(out string msg)
        {
            msg = string.Empty;
            int _agentid = 0;
            if (int.TryParse(AgentID, out _agentid))
            {
                if (_agentid != 0)
                {
                    DefaultChannelHandler.StateManager.RemoveAgentFromWCF(_agentid);
                }
            }
            msg = "ok";
        }
        private void PressureTest()
        {
            //DefaultChannelHandler.StateManager.PressureTest();
            DefaultChannelHandler.StateManager.WcfTest();
        }

        private void GetServerTime(out string msg)
        {
            msg = "{\"result\":" + (DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1))).TotalMilliseconds.ToString() + "}";
        }

        private void RecordLog(out string msg)
        {

            BLL.Loger.Log4Net.Info(FromPrivateToken + "前台Client端收到分配消息，会话id：" + AllocID + ",网友是" + SendToPrivateToken);
            msg = "{\"result\":\"success\"}";
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
                    var comet = DefaultChannelHandler.StateManager.GetNetFrind(FromPrivateToken);
                    if (comet != null)
                    {
                        //comet.LastRequestTime = DateTime.Now;
                        //comet.SendMessageTime = DateTime.Now;
                        //comet.Status = AgentStatus.Online;
                        comet.AgentToken = "";
                        //取排队队列
                        List<ProxyNetFriend> listWait = DefaultChannelHandler.StateManager.GetWaitingCometClientsByBusinessLine(SourceType);
                        //判断是否在排队队列里
                        bool isWait = false;
                        if (listWait != null)
                        {
                            for (int i = 0; i < listWait.Count; i++)
                            {
                                ProxyNetFriend netFriend = listWait[i];
                                if (netFriend.Token == comet.Token)
                                {
                                    isWait = true;
                                    break;
                                }
                            }
                        }
                        //取正在聊天中的网友，
                        bool isLiaoTian = false;
                        if (!isWait)
                        {
                            ProxyNetFriend[] wyarray = null;
                            wyarray = DefaultChannelHandler.StateManager.GetWCFAllNetFriends();
                            if (wyarray != null)
                            {
                                for (int i = 0; i < wyarray.Length; i++)
                                {
                                    ProxyNetFriend netFriend = wyarray[i];
                                    if (netFriend.Token == comet.Token)
                                    {
                                        isLiaoTian = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (isWait == false && isLiaoTian == false)
                        {
                            int rint = DefaultChannelHandler.StateManager.EnQueueWaitAgent(SourceType, comet);
                            msg = rint.ToString();
                        }
                        else
                        {
                            msg = "2";
                        }
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
                DefaultChannelHandler.StateManager.TransferAgent(FromPrivateToken, Convert.ToInt32(TargetAgent), WyId);
            }
            catch (Exception ex)
            {
                msg = "{\"result\":\"" + ex.Message + "\"}";
            }

        }

        public class ReturnMessage
        {
            public string Result { get; set; }
            public string Data { get; set; }
            public string OrderID { get; set; }
            public string CustID { get; set; }
            public string OrderURL { get; set; }
        }
        private void checkorder(out ReturnMessage modelReturn)
        {
            modelReturn = new ReturnMessage();
            try
            {
                if (string.IsNullOrEmpty(CsId))
                {
                    modelReturn.Result = "error";
                    modelReturn.Data = "没有找到会话，参数错误.";
                    //msg = "{\"result\":\"error\",\"data\":\"没有找到会话，参数错误.\"}";
                    return;
                }
                var dt = BLL.Conversations.Instance.CheckConversationOrderCustInfo(CsId);

                if (dt == null || dt.Rows.Count == 0)
                {
                    modelReturn.Result = "error";
                    modelReturn.Data = "没有找到会话，请检查参数.";
                    //msg = "{\"result\":\"error\",\"data\":\"没有找到会话，请检查参数.\"}";
                    BLL.Loger.Log4Net.Info(modelReturn.Data + " CSID：" + CsId);
                    return;
                }
                var dr = dt.Rows[0];
                //如果不存在工单ID则调用cc添加个人工单接口，生成链接
                modelReturn.Result = "sendok";
                if (string.IsNullOrEmpty(dr["OrderID"].ToString()))
                {
                    string urlReturn = string.Empty;
                    string CustID = dr["CustID"].ToString();

                    //调用CC接口返回工单添加地址
                    //BLL.Loger.Log4Net.Info("调用CC个人版添加工单接口参数是");
                    int bussinesstype = -1;
                    int tagid = -1;
                    int.TryParse(BusinessTypeID, out bussinesstype);
                    int.TryParse(TagId, out tagid);
                    BLL.Loger.Log4Net.Info(string.Format("调用CC个人版添加工单接口，参数callNum:{0},csID:{1},custName:{2},cbsex:{3},cbprovinceID:{4},cbCityID:{5},cbCounty:{6},businessTypeID:{7},tagID:{8}", CallNum, CsId, CustName, -1, -1, -1, -1, bussinesstype, tagid));
                    urlReturn = CCWebServiceHepler.Instance.CCDataInterface_GetAddWOrderComeIn_IMGR_URL(CallNum, CsId, CustName, -1, -1, -1, -1, bussinesstype, tagid, 1);
                    BLL.Loger.Log4Net.Info(string.Format("调用CC个人版添加工单接口,返回值：{0}", urlReturn));
                    modelReturn.OrderURL = urlReturn;
                }
                else
                {
                    modelReturn.OrderID = dr["OrderID"].ToString();
                    modelReturn.CustID = dr["CustID"].ToString();
                    //msg = "{\"result\":\"sendok\",\"OrderID\":\"" + dr["OrderID"].ToString() + "\",\"CustID\":\"" + dr["CustID"].ToString() + "\"}";
                }
            }
            catch (Exception ex)
            {
                modelReturn.Result = "error";
                modelReturn.Data = ex.Message;
                //msg = "{\"result\":\"error\",\"data\":" + ex.Message + "\"}";
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

        public void UpDateConversationTag(out string msg)
        {
            msg = "ok";
            int tId = 0;
            int cid = 0;
            if (string.IsNullOrEmpty(CsId) || string.IsNullOrWhiteSpace(TagId) || !int.TryParse(CsId, out cid) || !int.TryParse(TagId, out tId))
            {
                msg = "参数错误";
                return;
            }
            if (cid == 0)
            {
                msg = "参数错误";
                return;
            }
            try
            {
                BLL.Conversations.Instance.UpdateConversationTag(CsId, TagId, TagName);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

        }
        /// <summary>
        /// 取验证图片
        /// </summary>
        private void TokeYanZhengGif(out string msg)
        {
            msg = string.Empty;
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            Dictionary<string, string> dicyanzheng = null;
            if (objCache["yanzheng"] == null)
            {
                Dictionary<string, string> dic = BLL.BaseData.Instance.ReadYanZhengXml();
                dicyanzheng = dic;
                objCache.Insert("yanzheng", dic, null, DateTime.Now.AddHours(8), TimeSpan.Zero);
            }
            else
            {
                dicyanzheng = (Dictionary<string, string>)objCache["yanzheng"];
            }
            //随机数
            Random a = new Random();
            int radomnumber = a.Next(100);
            string urlstr = dicyanzheng[radomnumber.ToString()];

            if (objCache[WYGUID + "_daan"] == null)
            {
                objCache.Insert(WYGUID + "_daan", urlstr.Split('|')[1], null, DateTime.Now.AddSeconds(30), TimeSpan.Zero);
            }
            else
            {
                objCache[WYGUID + "_daan"] = urlstr.Split('|')[1];
            }
            msg = HttpContext.Current.Request.Url.ToString().Substring(0, HttpContext.Current.Request.Url.ToString().LastIndexOf('/')) + "/ProduceImage/" + urlstr.Split('|')[0];
            //HttpContext.Current.Response.Write();
        }
        /// <summary>
        /// 验证答案
        /// </summary>
        private void YanZhengDaAn(out string msg)
        {
            msg = string.Empty;
            //回答错误取新图
            if (!string.IsNullOrEmpty(DaAn))
            {
                System.Web.Caching.Cache objCache = HttpRuntime.Cache;
                if (objCache[WYGUID + "_daan"] != null && objCache[WYGUID + "_daan"].ToString() == DaAn)
                {
                    objCache.Insert(WYGUID + "_yanzheng", 1, null, DateTime.Now.AddSeconds(30), TimeSpan.Zero);
                    msg = "yes";
                }
                else
                {
                    TokeYanZhengGif(out msg);
                }
            }
            else
            {
                TokeYanZhengGif(out msg);
            }
        }



    }
}