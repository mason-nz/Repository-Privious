using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.DSC.IM2014.Server.Web.Channels;
using BitAuto.DSC.IM2014.Core;
using BitAuto.DSC.IM2014.Core.Messages;
using System.Data;

namespace BitAuto.DSC.IM2014.Server.Web.AjaxServers
{
    /// <summary>
    /// Summary description for Handler
    /// </summary>
    public class Handler : IHttpHandler
    {

        public string username
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["username"]); }
        }
        public string message
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["message"]); }
        }
        public string action
        {
            get { return HttpContext.Current.Request.Form["action"]; }
        }
        public string SendToPublicToken
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["SendToPublicToken"]); }
        }
        //坐席与网友建立聊天标识
        public string AllocID
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["AllocID"]); }
        }
        /// <summary>
        /// 坐席状态
        /// </summary>
        public int AgentState
        {
            get { return string.IsNullOrEmpty(HttpContext.Current.Request["AgentState"]) == true ? -1 : Convert.ToInt16(HttpContext.Current.Request["AgentState"]); }
        }
        /// <summary>
        /// 用户类型，1坐席，2网友
        /// </summary>
        public int UserType
        {
            get { return string.IsNullOrEmpty(HttpContext.Current.Request["usertype"]) == true ? -1 : Convert.ToInt16(HttpContext.Current.Request["usertype"]); }
        }
        /// <summary>
        /// 涞源
        /// </summary>
        public string UserReferURL
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["UserReferURL"]); }
        }
        /// <summary>
        /// 地理位置
        /// </summary>
        public string UserCityName
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["UserCityName"]); }
        }
        /// <summary>
        /// ip
        /// </summary>
        public string UserIP
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["UserIP"]); }
        }
        /// <summary>
        /// 地理位置Id
        /// </summary>
        public string LocationID
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["cityID"]); }
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string msg = "";
            if (action == "init")
            {
                CometClient cometmodel = null;
                try
                {
                    cometmodel = DefaultChannelHandler.StateManager.GetCometClient(username);
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
                        int connectionTimeoutSeconds = BLL.Util.GetConnectionTimeoutSeconds();
                        int connectionIdleSeconds = 0;
                        if (UserType == (Int32)Entities.UserType.Agent)
                        {
                            connectionIdleSeconds = BLL.Util.GetConnectionIdleSecondsAgent();
                            DefaultChannelHandler.StateManager.InitializeClient(
                            username, username, username, connectionTimeoutSeconds, connectionIdleSeconds,UserType);

                            //更新坐席状态为离线
                            DefaultChannelHandler.StateManager.SetAgentState(username, (Int32)Entities.AgentStatus.Leaveline);
                        }
                        else
                        {
                            connectionIdleSeconds = BLL.Util.GetConnectionIdleSecondsUser();
                            int sendmessageIdleSeconds = BLL.Util.GetSendMessageIdleSeconds();
                            DefaultChannelHandler.StateManager.InitializeClient(
                                username, username, username, connectionTimeoutSeconds, connectionIdleSeconds,sendmessageIdleSeconds, UserReferURL, UserType, UserCityName, LocationID, UserIP);
                        }
                        msg = "loginok";
                    }
                    catch (CometException ce)
                    {
                        if (ce.MessageId == CometException.CometClientAlreadyExists)
                        {
                            msg = "User is already logged into the chat application.";
                        }
                    }
                }
            }
            else if (action == "sendmessage")
            {
                if (string.IsNullOrEmpty(username))
                {
                    msg = "消息发送人不能为空！";
                }
                else if (string.IsNullOrEmpty(SendToPublicToken))
                {
                    msg = "消息接收人不能为空！";
                }
                else if (string.IsNullOrEmpty(message))
                {
                    msg = "发送的消息不能为空！";
                }
                else if (string.IsNullOrEmpty(AllocID))
                {
                    msg = "分配坐席标识不能为空！";
                }
                else
                {
                    ChatMessage chatMessage = new ChatMessage();
                    //
                    //  get who the message is from
                    //消息接收人状态是否正常
                    bool isExist = true;
                    CometClient cometClientfrom = null;
                    CometClient cometClientsendto = null;
                    try
                    {
                        cometClientfrom = DefaultChannelHandler.StateManager.GetCometClient(username);
                        cometClientsendto = DefaultChannelHandler.StateManager.GetCometClient(SendToPublicToken);
                    }
                    catch (Exception ex)
                    {
                        isExist = false;
                    }
                    if (cometClientfrom != null)
                    {
                        if (isExist == true && cometClientsendto != null && cometClientsendto.Status == (Int32)Entities.AgentStatus.Online)
                        {
                            cometClientfrom.SendMessageTime = System.DateTime.Now;
                            //给聊天记录入库线程消息实体赋值 add by qizq 2014-3-6
                            long _AllocID = 0;
                            long.TryParse(AllocID, out _AllocID);
                            //  get the display name
                            chatMessage.From = cometClientfrom.DisplayName;
                            chatMessage.Message = message;
                            chatMessage.AllocID = _AllocID;
                            chatMessage.EnterTime = cometClientfrom.EnterTime;
                            chatMessage.LocalIP = cometClientfrom.LocalIP;
                            chatMessage.Location = cometClientfrom.Location;
                            chatMessage.TalkTime = cometClientfrom.TalkTime;
                            chatMessage.WaitTime = cometClientfrom.WaitTime;
                            chatMessage.UserReferURL = cometClientfrom.UserReferURL;
                            string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MTalk);
                            DefaultChannelHandler.StateManager.SendMessage(SendToPublicToken, messagetype, chatMessage);

                            DefaultChannelHandler.StateManager.SendMessageInDB(SendToPublicToken, _AllocID, cometClientfrom.PrivateToken, message, (Int32)Entities.MessageType.MTalk, System.DateTime.Now);
                            // Add your operation implementation here
                            //return;

                            msg = "{'result':'sendok','rectime':'" + DateTime.Now + "'}";
                            
                        }
                        else
                        {
                            //msg = "消息: " + message + " 发送失败，对方已离线！";
                            msg = "SendToLeave";
                        }
                    }
                    else
                    {
                        //msg = "对话被中断或已结束！请退出后重新连接！";
                        msg = "ClientNotExists";
                    }
                }
            }
            else if (action == "closechat")
            {
                CometClient cometClient = null;
                try
                {
                    cometClient = DefaultChannelHandler.StateManager.GetCometClient(username);
                }
                catch (Exception)
                {
                }
                if (cometClient != null)
                {
                    //取坐席所有在聊网友然后给网友发送坐席离线通知
                    string[] userlist = cometClient.TalkUserList;
                    ChatMessage chatMessage = new ChatMessage();
                    chatMessage.From = cometClient.DisplayName;
                    chatMessage.Message = "离线";
                    string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MLline);
                    for (int i = 0; i < userlist.Length; i++)
                    {
                        DefaultChannelHandler.StateManager.SendMessage(userlist[i], messagetype, chatMessage);
                    }

                    //更新坐席状态为离线
                    DefaultChannelHandler.StateManager.SetAgentState(cometClient.PrivateToken, (Int32)Entities.AgentStatus.Leaveline);
                    DefaultChannelHandler.StateManager.KillIdleCometClient(cometClient.PrivateToken);
                    DefaultChannelHandler.StateManager.KillWaitRequest(cometClient.PrivateToken);
                }

                msg = "sendok";
            }
            else if (action == "userclosechat")
            {
                CometClient cometClient = null;
                CometClient cometClientsendto = null;
                try
                {
                    cometClient = DefaultChannelHandler.StateManager.GetCometClient(username);
                    if (!string.IsNullOrEmpty(SendToPublicToken))
                    {
                        cometClientsendto = DefaultChannelHandler.StateManager.GetCometClient(SendToPublicToken);
                    }
                }
                catch (Exception)
                {
                }
                if (cometClient != null && cometClientsendto != null && cometClientsendto.Status == (Int32)Entities.AgentStatus.Online)
                {
                    ChatMessage chatMessage = new ChatMessage();
                    chatMessage.From = cometClient.DisplayName;
                    chatMessage.Message = "离线";
                    string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MLline);
                    //网友离线坐席聊天人数减1
                    cometClientsendto.DialogCount = cometClientsendto.DialogCount - 1;
                    //给坐席的在聊网友加入当前网友
                    cometClientsendto.RemoveUser(cometClient.PrivateToken);
                    DefaultChannelHandler.StateManager.SendMessage(SendToPublicToken, messagetype, chatMessage);
                    DefaultChannelHandler.StateManager.KillIdleCometClient(cometClient.PrivateToken);
                    DefaultChannelHandler.StateManager.KillWaitRequest(cometClient.PrivateToken);
                }
                else if (cometClient != null)
                {
                    DefaultChannelHandler.StateManager.KillIdleCometClient(cometClient.PrivateToken);
                    DefaultChannelHandler.StateManager.KillWaitRequest(cometClient.PrivateToken);
                }
                msg = "sendok";
            }
            else if (action == "SetAgentState")
            {
                CometClient cometClient = null;
                try
                {
                    cometClient = DefaultChannelHandler.StateManager.GetCometClient(username);
                }
                catch (Exception)
                {
                }
                if (cometClient != null)
                {
                    DefaultChannelHandler.StateManager.SetAgentState(cometClient.PrivateToken, AgentState);
                    if (AgentState == (int)Entities.AgentStatus.Leaveline)
                    {

                        string[] userlist = cometClient.TalkUserList;
                        ChatMessage chatMessage = new ChatMessage();
                        chatMessage.From = cometClient.DisplayName;
                        chatMessage.Message = "离线";
                        string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MLline);
                        for (int i = 0; i < userlist.Length; i++)
                        {
                            DefaultChannelHandler.StateManager.SendMessage(userlist[i], messagetype, chatMessage);
                        }
                        cometClient.DialogCount = 0;
                        cometClient.RemoveAllUser();
                        //更新在聊网友的会话结束时间
                        BLL.AllocationAgent.Instance.UpdateEndTime(cometClient.PrivateToken);
                    }
                }
                msg = "sendok";
            }
            else if (action == "GetAgentState")
            {
                try
                {
                    DataTable dt = null;
                    dt = BitAuto.DSC.IM2014.BLL.Util.GetEnumDataTable(typeof(Entities.AgentStatus));
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
            }
            //重新分配坐席
            else if (action == "ResetAgent")
            {
                CometClient cometClient = null;
                try
                {
                    cometClient = DefaultChannelHandler.StateManager.GetCometClient(username);
                    //cometClientsendto = DefaultChannelHandler.StateManager.GetCometClient(SendToPublicToken);
                }
                catch (Exception)
                {
                }
                if (cometClient != null)
                {
                    //重新分配不初始化
                    //ResetAgent(cometClient);
                    //if (cometClientsendto != null)
                    //{
                    //    //网友离线坐席聊天人数减1
                    //    cometClientsendto.DialogCount = cometClientsendto.DialogCount - 1;
                    //    //给坐席的在聊网友加入当前网友
                    //    cometClientsendto.RemoveUser(cometClient.PrivateToken);
                    //}
                    DefaultChannelHandler.StateManager.agentallocInQueue(username);
                    msg = "sendok";
                }
                else
                {
                    msg = "Initializeok";
                }

            }
            else if (action == "IsExists")
            {
                CometClient cometClient = null;
                try
                {
                    cometClient = DefaultChannelHandler.StateManager.GetCometClient(username);
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
            }
            else if (action == "GetChatMessageLog")
            {
                msg = BLL.ChatMessageLog.Instance.GetChatMessageLog(Convert.ToInt32(AllocID));
                if (!string.IsNullOrEmpty(msg))
                {
                    context.Response.Write("{\"result\":" + msg + "}");
                    return;
                }
            }
            else if (action == "GetDetailInfo")
            {
                msg = BLL.AllocationAgent.Instance.GetAllocationAgent(Convert.ToInt32(AllocID));
            }
            context.Response.Write("{\"result\":\"" + msg + "\"}");
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