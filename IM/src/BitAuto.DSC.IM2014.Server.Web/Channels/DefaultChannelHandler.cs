using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;

using BitAuto.DSC.IM2014.Core;
using System.Diagnostics;
using BitAuto.DSC.IM2014.Core.Messages;
namespace BitAuto.DSC.IM2014.Server.Web.Channels
{
    /// <summary>
    /// This is our handler for the comet subscription mechanism
    /// </summary>
    public class DefaultChannelHandler : IHttpAsyncHandler
    {
        /// <summary>
        /// This is our state manager that manages the state of the client
        /// </summary>
        private static CometStateManager stateManager;

        static DefaultChannelHandler()
        {
            //
            //  Initialize 
            stateManager = new CometStateManager(
                new InProcCometStateProvider());


            stateManager.ClientInitialized += new CometClientEventHandler(stateManager_ClientInitialized);
            stateManager.ClientSubscribed += new CometClientEventHandler(stateManager_ClientSubscribed);
            stateManager.IdleClientKilled += new CometClientEventHandler(stateManager_IdleClientKilled);
            stateManager.AgentStateOnChanged += new AgentStateEventHandler(stateManager_AgentStateOnChanged);
        }

        static void stateManager_AgentStateOnChanged(object sender, AgentStateEventArgs args)
        {
            Debug.WriteLine("Client AgentStateOnChanged: " + args.AgentState.AgentID + "," + args.AgentState.AgentStatus);


            if (BLL.AgentInfo.Instance.IsExistsByAgentID(args.AgentState.AgentID))
            {
                //更新坐席信息表AgentInfo
                Entities.AgentInfo agentInfo = BLL.AgentInfo.Instance.GetAgentInfo(args.AgentState.AgentID);
                agentInfo.AgentStatus = args.AgentState.AgentStatus;
                BLL.AgentInfo.Instance.Update(agentInfo);

                //插入坐席状态明细到AgentStatusDetail
                BLL.AgentStatusDetail.Instance.Insert(args.AgentState);
            }


        }

        static void stateManager_IdleClientKilled(object sender, CometClientEventArgs args)
        {
            //
            //  ok, write a message saying we have timed out
            //modify by qizq 2014-3-11 去掉广播通知
            //Debug.WriteLine("Client Killed: " + args.CometClient.DisplayName);
            ////  send a chat message
            //ChatMessage cm = new ChatMessage();

            //cm.From = "System";
            //if (args.CometClient.Type == (int)Entities.UserType.Agent)
            //{
            //    cm.Message = "坐席" + args.CometClient.DisplayName + " 退出系统.";
            //}
            //else
            //{
            //    cm.Message = "网友" + args.CometClient.DisplayName + " 退出系统.";
            //}

            //string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MTalk);
            ////退出广播消息
            //stateManager.SendMessage(messagetype, cm);
        }

        static void stateManager_ClientSubscribed(object sender, CometClientEventArgs args)
        {
            //
            //  ok, write a message saying we have timed out
            Debug.WriteLine("Client Subscribed: " + args.CometClient.DisplayName);
        }

        static void stateManager_ClientInitialized(object sender, CometClientEventArgs args)
        {
            //
            //  ok, write a message saying we have timed out
            //Debug.WriteLine("Client Initialized: " + args.CometClient.DisplayName);
            ////  send a chat message
            //ChatMessage cm = new ChatMessage();

            //cm.From = "System";
            //cm.Message = args.CometClient.DisplayName + " has joined the chat room.";

            //stateManager.SendMessage("ChatMessage", cm);

            //网友登录,为网友分配坐席
            if (args.CometClient.Type == (Int32)Entities.UserType.User)
            {
                //把网友登录记录下来,判断网友是否登录过
                bool isExist = false;
                isExist = BLL.UserInfo.Instance.IsExistsByUserID(args.CometClient.PrivateToken);
                if (!isExist)
                {
                    Entities.UserInfo userinfomodel = new Entities.UserInfo();
                    userinfomodel.UserID = args.CometClient.PrivateToken;
                    userinfomodel.Status = 0;
                    userinfomodel.CreateTime = System.DateTime.Now;
                    BLL.UserInfo.Instance.Insert(userinfomodel);
                }
                //把要分配坐席的网友放在队列里,进入排队队列时间
                args.CometClient.EnterTime = System.DateTime.Now.ToString();
                DefaultChannelHandler.StateManager.agentallocInQueue(args.CometClient.PrivateToken);
                ////取一个坐席为网友服务
                //CometWaitRequest comwaitrequest = DefaultChannelHandler.StateManager.AssignAgent();
                //if (comwaitrequest != null)
                //{
                //    //分配坐席后把网友坐席对应关系保存下来
                //    Entities.AllocationAgent allocaagentModel = new Entities.AllocationAgent();
                //    allocaagentModel.AgentID = Convert.ToInt32(comwaitrequest.ClientPrivateToken);
                //    allocaagentModel.UserID = args.CometClient.PrivateToken;
                //    allocaagentModel.StartTime = System.DateTime.Now;
                //    allocaagentModel.UserReferURL = args.CometClient.UserReferURL;
                //    allocaagentModel.EndTime = Convert.ToDateTime("9999-12-31");
                //    long allocid=BLL.AllocationAgent.Instance.Insert(allocaagentModel);
                //    //
                //    UserInitialMsg cmforuser = new UserInitialMsg();
                //    cmforuser.AgentID = Convert.ToInt32(comwaitrequest.ClientPrivateToken);
                //    cmforuser.UserID = args.CometClient.PrivateToken;
                //    cmforuser.UserReferURL = args.CometClient.UserReferURL;
                //    cmforuser.AllocID = allocid;
                //    string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MAllocAgent);
                //    //以系统身份通知网友坐席为您服务
                //    stateManager.SendMessage(args.CometClient.PrivateToken, messagetype, cmforuser);
                //    //以系统身份通知坐席为网友服务
                //    stateManager.SendMessage(comwaitrequest.ClientPrivateToken, messagetype, cmforuser);

                //    //坐席发给网友的欢迎语
                //    ChatMessage cm = new ChatMessage();
                //    cm.From = comwaitrequest.ClientPrivateToken;
                //    cm.Message = "您好，有什么可以帮助您！";
                //    string messagetypehello = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MTalk);
                //    stateManager.SendMessage(args.CometClient.PrivateToken, messagetypehello, cm);

                //}
                //else
                //{
                //    //以系统身份通知网友坐席全忙
                //    ChatMessage cm = new ChatMessage();
                //    cm.From = "System";
                //    cm.Message = "目前没有闲置坐席为您服务,您可以填写留言！";
                //    string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MAllBussy);
                //    stateManager.SendMessage(args.CometClient.PrivateToken, messagetype, cm);
                //}
            }
            else if (args.CometClient.Type == (Int32)Entities.UserType.Agent)
            {
                //如果是坐席，以系统身份通知坐席登录成功。
                ChatMessage cm = new ChatMessage();
                cm.From = "System";
                cm.Message = "坐席" + args.CometClient.DisplayName + "成功登录到系统.";
                string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MTalk);
                stateManager.SendMessage(args.CometClient.PrivateToken, messagetype, cm);
            }
        }

        #region IHttpAsyncHandler Members

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            return stateManager.BeginSubscribe(context, cb, extraData);
        }

        public void EndProcessRequest(IAsyncResult result)
        {
            stateManager.EndSubscribe(result);
        }

        #endregion

        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            throw new NotImplementedException();
        }

        public static CometStateManager StateManager
        {
            get { return stateManager; }
        }


        public static bool isExistClient(string clientPrivateToken)
        {
            CometClient client = null;
            try
            {
                client = stateManager.GetCometClient(clientPrivateToken);

            }
            catch (Exception ex)
            {
            }
            if (client != null)
                return true;

            return false;
        }
        #endregion
    }
}