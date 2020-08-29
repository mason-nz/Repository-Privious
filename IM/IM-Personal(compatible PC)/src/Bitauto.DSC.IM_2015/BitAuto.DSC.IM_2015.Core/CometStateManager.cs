using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.ServiceModel;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Runtime.Serialization.Json;
using BitAuto.DSC.IM_2015.MainInterface;
using BitAuto.DSC.IM_2015.Messages;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Core.Messages;
using System.Configuration;
using BitAuto.DSC.IM_2015.WebService.CC;
using log4net.Repository.Hierarchy;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using BitAuto.Utils;

namespace BitAuto.DSC.IM_2015.Core
{
    /// <summary>
    /// CometStateManger Class
    /// 
    /// An instance of the this class is used to control the manager the state of the COMET Application.
    /// This class manages an instance of a ICometStateProvider instance
    /// </summary>
    /// 
    public class CometStateManager : IDisposable, IIsMsgCallBackServices
    {
        private ICometStateProvider stateProvider;
        private InstanceContext wcfContext;// = new InstanceContext(this);
        public int isWcfCallBackRegisted = 0;
        private System.Threading.Timer tmPressTest;

        private CometWaitThread workerThread;
        private MessageInDBThread messageInDBThread;
        private CometMessageDeal cometmessagedeal;
        private object state = new object();
        private long RandomNumID = 0;

        public long GetRandomNumID
        {
            get { return Interlocked.Increment(ref RandomNumID); }
        }

        public string IISIP = string.Empty;

        public CometWaitRequest[] GetAllRequest()
        {
            return this.workerThread.WaitRequestsArray;
        }

        public CometStateManager(ICometStateProvider stateProvider)
        {
            try
            {
                BLL.Loger.Log4Net.Info(string.Format("开始初始化CometStateManager对象"));

                wcfContext = new InstanceContext(this);
                //IsBatchDebug = ConfigurationManager.AppSettings["IsDebug"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["IsDebug"]);
                if (stateProvider == null)
                {
                    throw new ArgumentNullException("stateProvider");
                }
                this.stateProvider = stateProvider;
                this.workerThread = new CometWaitThread(this);

                //启动聊天记录入库线程 add by qizq 2014-3-6
                this.messageInDBThread = new MessageInDBThread(this);
                //消息缓存区
                this.cometmessagedeal = new CometMessageDeal(this);

                IISIP = BLL.Util.IpToLong().ToString();
                RandomNumID = Convert.ToInt64(DateTime.Now.ToString("yyMMddHHmmss"));

            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info(string.Format("初始化CometStateManager对象出错              {0}", ex.Message));
            }

            /*
            try
            {
                //BLL.Loger.Log4Net.Info(string.Format("第一次注册IIS回调"));
                //RegisterIIS();
                //Interlocked.Exchange(ref isWcfCallBackRegisted, 1);
            }
            catch (Exception)
            {
                try
                {
                    Thread.Sleep(1000);
                    BLL.Loger.Log4Net.Info(string.Format("第二次注册IIS回调"));
                    RegisterIIS();
                    Interlocked.Exchange(ref isWcfCallBackRegisted, 1);
                }
                catch (Exception)
                {
                    try
                    {
                        Thread.Sleep(1500);
                        BLL.Loger.Log4Net.Info(string.Format("第三次注册IIS回调"));
                        RegisterIIS();
                        Interlocked.Exchange(ref isWcfCallBackRegisted, 1);
                    }
                    catch (Exception)
                    {
                        BLL.Loger.Log4Net.Info(string.Format("第三次注册IIS失败,后续等待轮询检测并注册。。。。"));
                    }

                }

            }
            */

        }


        /// <summary>
        /// 根据业务线获取等待队列
        /// </summary>
        /// <param name="strBusinessLine"></param>
        /// <returns></returns>
        public List<ProxyNetFriend> GetWaitingCometClientsByBusinessLine(string strBusinessLines)
        {
            //return mainAllocateThread.GetCometClientsByBusinessLine(strBusinessLine);
            try
            {
                var channel = GetAutoWCFClient();
                return channel.GetCometClientsByBusinessLines(strBusinessLines);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("方法：{0},出错：{1}", "CometStateManager.GetWaitingCometClientsByBusinessLine", ex.Message));
                return null;
            }
            //return null;
        }

        /// <summary>
        /// 取所有网友，包括排队中的
        /// </summary>
        /// <returns></returns>
        public int GetAllNetFrindHaveQuene()
        {
            int maxcount = 0;
            //取排队中的
            foreach (SourceType type in BitAuto.DSC.IM_2015.BLL.Util.GetAllSourceType(false))
            {
                List<ProxyNetFriend> listFrind = GetWaitingCometClientsByBusinessLine(type.SourceTypeValue);
                if (listFrind != null)
                {
                    maxcount += listFrind.Count;
                }
            }
            //取在聊的网友
            ProxyNetFriend[] NetFrindArray = GetWCFAllNetFriends();
            if (NetFrindArray != null)
            {
                maxcount += NetFrindArray.Length;
            }
            return maxcount;
        }


        /// <summary>
        /// 取wcf所有坐席对象
        /// </summary>
        /// <returns></returns>
        public ProxyAgentClient[] GetWCFAllAgents()
        {
            try
            {
                var channel = GetAutoWCFClient();
                return channel.GetAllAgents();
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("方法：{0},出错：{1}", "CometStateManager.GetWCFAllAgents", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// 取wcf所有网友对象
        /// </summary>
        /// <returns></returns>
        public ProxyNetFriend[] GetWCFAllNetFriends()
        {
            try
            {
                var channel = GetAutoWCFClient();
                return channel.GetAllNetFriends();
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("方法：{0},出错：{1}", "CometStateManager.GetWCFAllNetFriends", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// 根据agentid，移除wcf坐席
        /// </summary>
        /// <param name="agentid"></param>
        public void RemoveAgentFromWCF(int agentid)
        {
            try
            {
                var channel = GetAutoWCFClient();
                channel.ForceAgentLeave(agentid, (int)Entities.CloseType.SystemClose);


            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("方法：{0},出错：{1}", "CometStateManager.RemoveAgentFromWCF", ex.Message));

            }
        }

        public int EnQueueWaitAgent(string businessLine, ProxyNetFriend netFriendClient)
        {
            try
            {
                var channel = GetAutoWCFClient();
                channel.AddNetFriendWaitList(new ProxyNetFriend()
                {
                    BusinessLines = businessLine,
                    AgentToken = "",
                    contractphone = netFriendClient.contractphone,
                    ConverSTime = new DateTime(1900, 1, 1),
                    CreateTime = new DateTime(1900, 1, 1),
                    CSID = -1,
                    IISIP = netFriendClient.IISIP,
                    IsTurnOut = false,
                    NetFName = netFriendClient.NetFName,
                    Token = netFriendClient.Token,
                    VisitID = netFriendClient.VisitID,
                    LastMessageTime = new DateTime(1900, 1, 1)

                });
                //return mainAllocateThread.EnQueueWaitAgent(businessLine, PrivateToken);
                return 0;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("方法：{0},出错：{1}", "CometStateManager.EnQueueWaitAgent", ex.Message));
                return -1;
            }

        }


        //usertype 1坐席，0：网友
        public CometClient InitializeClient_new(string publicToken, string privateToken, string displayName, int connectionTimeoutSeconds, int connectionIdleSeconds, int sendmessageIdleSeconds, int usertype, string strSourceType, string strLoginID, string Posturl, string Title, string ProvinceID, string CityID, string IsReInit, out string msg, bool isWap)
        {
            //DateTime dtT = DateTime.Now;
            //BLL.Loger.Log4Net.Info(string.Format("初始化线程ID:{0},初始化用户:{1},初始化时间：{2}", Thread.CurrentThread.ManagedThreadId, privateToken, dtT.ToString("O")));

            msg = string.Empty;


            if (connectionIdleSeconds <= 0)
                throw new ArgumentOutOfRangeException("connectionIdleSeconds must be greater than 0");

            if (connectionTimeoutSeconds <= 0)
                throw new ArgumentOutOfRangeException("connectionTimeoutSeconds must be greater than 0");

            CometClient cometClient = new CometClient();
            //  ok, set it up
            cometClient.ConnectionIdleSeconds = connectionIdleSeconds;
            cometClient.ConnectionTimeoutSeconds = connectionTimeoutSeconds;
            cometClient.DisplayName = displayName;
            //cometClient.LastActivity = DateTime.Now;
            cometClient.PrivateToken = privateToken;
            //cometClient.PublicToken = publicToken;
            //cometClient.UserReferURL = UserReferURL;
            cometClient.Type = usertype;

            if (usertype == 1)
            {
                //如果是坐席的的话,坐席默认为离线，初始化坐席增删用户事件
                cometClient.AgentToken = privateToken;
                cometClient.AgentID = privateToken.Substring(0, privateToken.LastIndexOf("@", StringComparison.CurrentCultureIgnoreCase));
                cometClient.Status = AgentStatus.Leaveline;
                cometClient.IPIISAgent = privateToken.Substring(privateToken.LastIndexOf("@", StringComparison.CurrentCultureIgnoreCase) + 1); //BLL.Util.GetLocalIp();


                cometClient.MaxDialogCount = Convert.ToInt32(ConfigurationManager.AppSettings["MaxDialogueN"]);

                #region 获取客户最大会话量，排队量信息


                // 调用CC接口获取客服最大等待量，BGID 等信息
                EmployeeAgent agent = CCWebServiceHepler.Instance.GetEmployeeAgent(Convert.ToInt32(cometClient.AgentID));
                if (agent == null)
                {
                    BLL.Loger.Log4Net.Info("初始化坐席失败，调用CC接口获取客服信息失败.");
                    return null;
                }
                cometClient.InBGID = agent.BGID.ToString();
                cometClient.InBGIDName = agent.BGName;
                cometClient.BusinessLines = agent.BusinessLineIDs;
                cometClient.AgentName = agent.UserName;
                cometClient.ManagedBGID = agent.ManageBGIDs;

                cometClient.AgentNum = agent.AgentNum;


                //cometClient.BusinessLines = "2";
                //cometClient.AgentNum = cometClient.AgentID;

                //cometClient.AgentNum = BLL.EmployeeAgent.Instance.GetAgentNum(publicToken);
                //调用WCF，添加坐席
                var channel = GetAutoWCFClient();
                var addResult = channel.AddAgent(new ProxyAgentClient()
                   {
                       AgentID = Convert.ToInt32(cometClient.AgentID),
                       AgentToken = cometClient.PrivateToken,
                       AgentName = displayName,
                       AgentNum = cometClient.AgentNum,
                       BusinessLines = cometClient.BusinessLines,
                       IISIP = cometClient.IPIISAgent,
                       MaxDialogNum = cometClient.MaxDialogCount,
                       Status = (int)AgentStatus.Leaveline,
                       InBGID = agent.BGID.ToString(),
                       InBGIDName = agent.BGName,
                       Type = 1
                   });

                if (addResult == -1)
                {
                    msg = "添加失败，坐席已经存在。";
                    return null;
                }
                else if (addResult == -2)
                {
                    msg = "坐席添加失败";
                    return null;
                }
                if (IsReInit != "1")
                {
                    //cometClient.RecordAgentStatus((int)AgentStatus.Leaveline); //记录状态
                    int agentID = 0;
                    int.TryParse(cometClient.AgentID, out agentID);
                    int recIDStatus = BLL.AgentStatusDetail.Instance.AddAgentStatusData(agentID, (int)AgentStatus.Leaveline, 0);
                    channel = GetAutoWCFClient();
                    channel.SetAgentStatus(agentID, (int)AgentStatus.Leaveline, recIDStatus);
                }

                #endregion

                //InitCometClientEvent(cometClient);
            }
            else
            {
                //如果是网友，默认为在线
                cometClient.Status = AgentStatus.Online;
                cometClient.SendMessageIdleSeconds = sendmessageIdleSeconds;
                cometClient.IPIISWY = BLL.Util.IpToLong().ToString();


                cometClient.Userloginfo = BLL.UserVisitLog.Instance.GetUserInfo(Title, Posturl, strSourceType, strLoginID, CityID, ProvinceID, isWap);
                cometClient.BusinessLines = cometClient.Userloginfo.SourceType;
                cometClient.DisplayName = cometClient.Userloginfo.UserName;
                //给网友加区域
                int _cityid = 0;
                int _provinceid = 0;
                if (int.TryParse(CityID, out _cityid))
                {
                    cometClient.Userloginfo.CityID = _cityid;
                }
                if (int.TryParse(ProvinceID, out _provinceid))
                {
                    cometClient.Userloginfo.ProvinceID = _provinceid;
                }
                //判断是否已存在网友，如果存在清除，用于网友刷新页面
                //CometClient cometmodelhave = IsNetFrindExists(cometClient.PrivateToken);
                if (IsNetFrindExists(cometClient.PrivateToken))
                {
                    msg = "exists";
                    return null;
                }
                else
                {
                    cometClient.PrivateToken = GetRandomNumID.ToString() + "@" + cometClient.IPIISWY;
                    if (isWap)
                    {
                        cometClient.PrivateToken = "M_" + GetRandomNumID.ToString() + "@" + cometClient.IPIISWY;
                    }

                    var st = new ServeTime(9, 0);
                    var et = new ServeTime(18, 0);
                    BLL.BaseData.Instance.ReadTime(out st, out et, strSourceType);
                    DateTime logintime = System.DateTime.Now;
                    //判断是否是工作时间
                    if (logintime.Hour < st.Hour || logintime.Hour > et.Hour || (logintime.Hour == st.Hour && logintime.Minute < st.Min) || (logintime.Hour == et.Hour && logintime.Minute > et.Min))
                    {
                        msg = "servicetimeout";
                        return cometClient;
                    }

                    var channel = GetAutoWCFClient();
                    var addResult = channel.AddNetFrind(new ProxyNetFriend()
                    {
                        BusinessLines = cometClient.BusinessLines,
                        AgentToken = "",
                        contractphone = cometClient.Userloginfo.Phone,
                        ConverSTime = new DateTime(1900, 1, 1),
                        CreateTime = new DateTime(1900, 1, 1),
                        CSID = -1,
                        IISIP = cometClient.IPIISWY,
                        IsTurnOut = false,
                        NetFName = cometClient.DisplayName,
                        Token = cometClient.PrivateToken,
                        VisitID = cometClient.Userloginfo.VisitID,
                        Type = 2,
                        IsAgentReply = false
                    });

                    if (addResult == -1)
                    {
                        msg = "添加失败，网友已经存在。";
                        return null;
                    }
                    else if (addResult == -2)
                    {
                        msg = "网友添加失败";
                        return null;
                    }


                }


            }


            //对象不放到本地对象集合，因为本地不保存对象
            //this.stateProvider.InitializeClient(cometClient);

            return cometClient;
        }


        //private static int i = 0;
        public IAsyncResult BeginSubscribe(HttpContext context, AsyncCallback callback, object extraData)
        {
            var result = new CometAsyncResult(context, callback, state);
            long lastMessageId = 0;
            string privateToken = string.Empty;
            string lastServiceIp = string.Empty;
            try
            {

                if (!long.TryParse(context.Request["lastMessageId"] ?? "-1", out lastMessageId))
                    throw CometException.CometHandlerParametersAreInvalidException();

                privateToken = context.Request["privateToken"];
                lastServiceIp = context.Request["lServiceIp"];
                //Debug.WriteLine(privateToken);


                //string str = string.Format("BeginSubscribe: 请求进入{0},时间:{1},剩余个数{2},线程ID:{3}", privateToken, DateTime.Now.ToString("O"), Interlocked.Increment(ref i),Thread.CurrentThread.ManagedThreadId);
                //Debug.WriteLine(str);
                //BLL.Loger.Log4Net.Info(str);

                if (string.IsNullOrEmpty(privateToken))
                    throw CometException.CometHandlerParametersAreInvalidException();


                ProxyAgentClient agentClient = null;
                ProxyNetFriend netFriend = null;
                int Type = 0;
                GetCometClient(privateToken, ref netFriend, ref agentClient, ref Type);


                if (agentClient == null && netFriend == null)
                {

                    ////查找wcf服务里中是否存在该对象，不存在则提示未找到服务对象
                    //if (IsExistsWcf(privateToken))
                    //{
                    //    string[] ad = privateToken.Split('@');
                    //    if (ad.Length > 1)
                    //    {
                    //        //被转移过来的，对象对应的服务器与请求到达的服务器不一致，发送超时消息
                    //        if (ad[1] != BLL.Util.IpToLong().ToString())
                    //        {
                    //            BLL.Loger.Log4Net.Info("BeginSubscribe收到来自," + privateToken + ",的长连接，时间是" + DateTime.Now.ToString());
                    //            Thread.Sleep(1000);
                    //            WriteTimeOutToResponse(context, result);
                    //            return result;
                    //        }
                    //    }
                    //}

                    // throw new Exception("未找到服务端对象");
                    this.WriteErrorToResponse(context, result, "未找到服务端对象");
                    return result;
                }

                //收集要删除的messageid
                if (lastMessageId > -1)
                {
                    this.cometmessagedeal.EnqueueDelete(privateToken + "," + lastMessageId);
                }
                //判断上次服务与这次服务的机器是否发生变化
                if (!string.IsNullOrEmpty(lastServiceIp) && lastServiceIp != BLL.Util.IpToLong().ToString())
                {
                    //更新实体服务机器
                    UpdateServiceIp(privateToken, lastServiceIp);
                }

                //

                //  ok, fire the event
                //this.FireClientSubscribed(cometClient);

                this.workerThread.RemoveWaitRequestByToken(privateToken);
                var request = new CometWaitRequest(privateToken, lastMessageId, context, callback, extraData);
                this.workerThread.QueueCometWaitRequest(request);

                //string str2 = string.Format("BeginSubscribe: 请求进入{0},时间:{1},线程ID:{2} 结束", privateToken, DateTime.Now.ToString("O"), Thread.CurrentThread.ManagedThreadId);
                //Debug.WriteLine(str2);
                //BLL.Loger.Log4Net.Info(str2);

                return request.Result;

            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info(string.Format("BeginSubscribe:ID:{0},ErrorMsg:{1}, StackTrace:{1}", privateToken, ex.Message, ex.StackTrace));
                this.WriteErrorToResponse(context, result, ex.Message);
            }
            return result;

        }

        /// <summary>
        /// Called from an Asynchronous HttpHandler Method method to complete the Subscribe call
        /// </summary>
        /// <param name="result">The IAsyncResult instance that was initialized in the BeginSubscribe call</param>
        public void EndSubscribe(IAsyncResult result)
        {
            CometAsyncResult cometAsyncResult = result as CometAsyncResult;

            if (cometAsyncResult != null)
            {
                try
                {
                    //  get the messages 
                    CometMessage[] messages = cometAsyncResult.CometMessages;
                    //  serialize the messages
                    //  back to the client
                    if (messages != null && messages.Length > 0)
                    {
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(messages.GetType());
                        serializer.WriteObject(((CometAsyncResult)result).Context.Response.OutputStream, messages);
                    }
                    //string str = string.Format("EndSubscribe=> :{0}  离开,DT:{1}，剩余个数{2},线程ID:{3}",
                    //    cometAsyncResult.Context.Request["PriveteToken"], DateTime.Now.ToString("O"), Interlocked.Decrement(ref i), Thread.CurrentThread.ManagedThreadId);
                    //Debug.WriteLine(str);
                    //BLL.Loger.Log4Net.Info(str);
                }
                catch (Exception ex)
                {
                    //  write the error out??
                    //this.WriteErrorToResponse(((CometAsyncResult)result).Context, ex.Message);
                    //context, callback, extraData, ex.Message
                    this.WriteErrorToResponse(((CometAsyncResult)result).Context, (CometAsyncResult)result, ex.Message);

                }
            }
        }

        public void SendMessage(CometMessage cometMessage, int sentType = 0)
        {
            try
            {
                //var comet = GetCometClient(cometMessage.FromToken);
                //comet.SendMessageTime = DateTime.Now;

                ProxyAgentClient agentClient = null;
                ProxyNetFriend netFriend = null;
                int Type = 0;
                GetCometClient(cometMessage.ToToken, ref netFriend, ref agentClient, ref Type);


                if (agentClient != null || netFriend != null)
                {
                    this.cometmessagedeal.Enqueue(cometMessage);
                }

                //if (netFriendClient != null)
                //{
                //    //本地网友
                //    if (comet.Type == 1 && sentType == 1 && !netFriendClient.IsAgentReply)
                //    {
                //        //更新客服回复时间
                //        BLL.Conversations.Instance.UpdateConversationReplyTime(DateTime.Now, netFriendClient.CSId);
                //        //comet.IsAgentReply = true;
                //        //comet.AgentSTime = DateTime.Now;
                //        netFriendClient.IsAgentReply = true;
                //        netFriendClient.AgentSTime = DateTime.Now;
                //        //更新wcf网友实体，坐席首次回复时间
                //        UpdateNetFriendAgentReplayTime(netFriendClient.PrivateToken);
                //        //
                //    }
                //    this.stateProvider.SendMessage(cometMessage);
                //}
                //else
                //{
                //    //非本地IIS发送消息到消息池
                //    this.cometmessagedeal.Enqueue(cometMessage);
                //}

            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("方法：{0},出错：{1}", "CometStateManager.SendMessage", ex.Message));
            }

        }

        /*
        /// <summary>
        /// Send a message to a specific client
        /// </summary>
        /// <param name="toToken">The public token of the client</param>
        /// <param name="name">The name of the message</param>
        /// <param name="contents">The contents of the message</param>
        /// <param name="sentType">0:系统消息，1：用户发送消息</param>
        public void SendMessage(string fromToken, string toToken, string name, string contents, int sentType = 0)
        {
            var comet = GetCometClient(fromToken);
            comet.LastMessageTime = DateTime.Now;
            if (comet.Type == 1)
            {
                var netFriendClient = GetCometClient(toToken);
                if (sentType == 1 && !netFriendClient.IsAgentReply)
                {
                    //更新客服回复时间
                    BLL.Conversations.Instance.UpdateConversationReplyTime(DateTime.Now, netFriendClient.CSId);
                    comet.IsAgentReply = true;
                    comet.AgentSTime = DateTime.Now;
                }

                if (netFriendClient.IsNative)
                {
                    this.stateProvider.SendMessage(toToken, name, contents, netFriendClient.IPIISWY);
                }
                else
                {
                    //非本地网友，发送到消息池
                    var message = new CometMessage();
                    message.Contents = contents;
                    message.Name = name;
                    message.ProcessTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    //Debug.WriteLine(string.Format("InProcCometStateProvider开始发送消息到对象{0}", DateTime.Now.ToString("G")));
                    message.IISIP = netFriendClient.IPIISWY;
                    this.cometmessagedeal.Enqueue(message);
                }
            }



            ////判断消息接受者与发送者是否在同一台机器
            //string iisip = BLL.Util.GetLocalIp();


            //if (iisip == IISIP)
            //{
            //    //如果消息接收者与本机为同一台机器则，从本机找消息接受者对象
            //    this.stateProvider.SendMessage(toToken, name, contents, IISIP);
            //}
            //else
            //{
            //    //消息发送到消息缓存区，等待线程把消息推到wcf中转
            //    var message = new CometMessage();
            //    message.Contents = contents;
            //    message.Name = name;
            //    message.ProcessTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            //    //Debug.WriteLine(string.Format("InProcCometStateProvider开始发送消息到对象{0}", DateTime.Now.ToString("G")));
            //    message.IISIP = IISIP;
            //    this.cometmessagedeal.Enqueue(message);
            //    //
            //}


        }
        */

        public void AddMessage(string clientPrivateToken, CometMessage message)
        {
            this.stateProvider.AddMessage(clientPrivateToken, message);
        }


        public void RemoveDicMessage()
        {
            this.stateProvider.RemoveDicMessage();
        }


        /// <summary>
        /// 给聊天记录入库线程消息实体赋值，add by qizq 2014-3-6
        /// </summary>
        /// <param name="clientPublicToken"></param>
        /// <param name="allocid"></param>
        /// <param name="from"></param>
        /// <param name="message"></param>
        /// <param name="messagetype"></param>
        /// <param name="sendTime"></param>
        public void SendMessageInDB(int CSID, int sender, string Content, int type)
        {
            MessageInDB db = new MessageInDB();
            db.IsInDB = false;
            db.CSID = CSID;
            db.Content = Content;
            db.Sender = sender;
            db.Type = type;
            db.CreateTime = System.DateTime.Now;
            db.Status = 0;
            this.messageInDBThread.QueueMessageInDB(db);
        }

        /// <summary>
        /// 判断坐席是否存在
        /// </summary>
        /// <param name="token"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool IsExists(string token, out string msg)
        {
            msg = string.Empty;
            try
            {
                var channel = GetAutoWCFClient();
                if (channel.IsAgentExists(Convert.ToInt32(token)))
                {
                    return true;
                }
                else
                {
                    msg = "NoExists";

                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return false;
        }

        /// <summary>
        /// Gets the ICometStateProvider instance this manager consumes
        /// </summary>
        internal ICometStateProvider StateProvider
        {
            get { return this.stateProvider; }
        }


        #region 分布式相关

        /// <summary>
        /// 获取自动版WCFClient,内部使用反射机制，连接调用后直接关闭，并自带有异常处理机制。
        /// </summary>
        /// <returns></returns>
        public IIMServices GetAutoWCFClient()
        {
            //var channelF = new DuplexChannelFactory<IIMServices>(this.wcfContext, "NetTcpBinding_IIMServices");
            var serviceProxy = new ServiceClientProxy(this.wcfContext, "NetTcpBinding_IIMServices");
            return serviceProxy.Channel;
        }

        /// <summary>
        /// 获取手动WCFClient，你需要自己关闭连接并处理异常信息
        /// </summary>
        /// <returns></returns>
        public IIMServices GetManualWCFClient()
        {
            var channelFactory = ChannelFactories.GetFactory(this.wcfContext, "NetTcpBinding_IIMServices");
            return channelFactory.CreateChannel();
        }

        public void RegisterIIS()
        {
            var channel = GetManualWCFClient();
            channel.RegisterIIS(this.IISIP);
        }

        //public IIMServices GetMulWCFClient(InstanceContext contex)
        //{
        //    var channelF = new DuplexChannelFactory<IIMServices>(contex, "NetTcpBinding_IIMServices");
        //    IIMServices iChannel = channelF.CreateChannel();
        //    return iChannel;
        //}

        /// <summary>
        /// 网友断网,空闲超时,自动退出时同步消息【从坐席在聊网友中移除自己】
        /// </summary>
        /// <param name="netFriend"></param>
        /// <param name="CloseType">会话关闭方式</param>
        public void NoticeNetFriendRemoved(ProxyNetFriend netFriend, int CloseType)
        {
            try
            {
                this.cometmessagedeal.ForceSyncMessage();
                var channel = GetAutoWCFClient();

                //网友未分配时，直接调用接口移除
                if (string.IsNullOrWhiteSpace(netFriend.AgentToken))
                {
                    //坐席网友不在同一个服务器上,调用接口删除网友
                    channel.RemoveNetFriendFromWaitList(netFriend.BusinessLines, netFriend.Token);

                    //更新网友队列放弃时间
                    if (netFriend.VisitID != 0)
                    {
                        BLL.UserVisitLog.Instance.UpdateQueueFailTime(netFriend.VisitID);
                    }

                    return;
                }

                //CometClient agentclient = GetCometClient(netFriend.AgentToken);

                //if (agentclient == null)
                //{
                //坐席网友不在同一个服务器上,调用接口删除网友
                channel.RemoveNetFriend(netFriend.Token, false, true, CloseType);
                //return;
                //}

                //坐席，网友在同一个服务器上,直接给坐席发离线消息,
                //if (agentclient.Status == AgentStatus.Online || agentclient.Status == AgentStatus.LeavingForAWhile)
                //{
                //    SendLeaveLineMsg(netFriend.PrivateToken, agentclient.AgentToken, string.Format("网友{0}已离线.", netFriend.PrivateToken), agentclient.IPIISAgent, netFriend.CSId);
                //}
                //从坐席在聊队列中移除
                //this.stateProvider.RemoveSingleUser(netFriend.PrivateToken, agentclient.AgentToken);

                //调用接口删除网友
                //channel.RemoveNetFriend(netFriend.PrivateToken, false, false, CloseType);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("方法：{0},出错：{1}", "NoticeNetFriendRemoved", ex.Message));
            }


        }

        /// <summary>
        /// 坐席离线,空闲超时,自动退出时同步消息[移除所有网友，WS同步移除网友]
        /// </summary>
        /// <param name="agentClient"></param>
        public void NoticeAgentRemoved(ProxyAgentClient agentClient, int closeType)
        {
            if (agentClient == null)
            { return; }
            try
            {
                //var TalkList = agentClient.TalkUserList;
                //CometClient cometWY = null;


                var channel = GetAutoWCFClient();

                this.cometmessagedeal.ForceSyncMessage();

                //移除坐席当前本地在聊网友，非本地网友移除逻辑由WCF完成
                //RemoveLocalAgentTalkUsers(agentClient);

                //坐席的非本地网友会被WCF删除
                channel.RemoveAgent(Convert.ToInt32(agentClient.AgentID), closeType);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("方法：{0},出错：{1}", "NoticeAgentRemoved", ex.Message));
            }

        }

        /// <summary>
        /// 移除坐席当前对话列表中所有网友，坐席修改状态为离线时使用
        /// </summary>
        /// <param name="agentClient"></param>
        private void RemoveLocalAgentTalkUsers(CometClient agentClient)
        {
            if (agentClient == null)
            { return; }
            try
            {
                var talkList = agentClient.TalkUserList;
                BLL.Loger.Log4Net.Error(string.Format("方法：{0}中移除在聊网友。", "RemoveLocalAgentTalkUsers"));

                //for (int i = 0; i < talkList.Length; i++)
                //{
                //    CometClient cometWy = GetCometClient(talkList[i]);
                //    agentClient.RemoveTalkUser(talkList[i]);

                //    if (cometWy != null)
                //    {
                //        //坐席，网友在同一个服务器上
                //        SendLeaveLineMsg(agentClient.PrivateToken, cometWy.PrivateToken, string.Format("坐席{0}已离线.", agentClient.AgentID), cometWy.IPIISWY, cometWy.CSId);
                //        KillWaitRequest(cometWy);               //结束消息                
                //        KillIdleCometClient(cometWy);
                //    }

                //}
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("方法：{0},出错：{1}", "RemoveLocalAgentTalkUsers", ex.Message));
            }
        }

        public void SendLeaveLineMsg(string fromToken, string toToken, string messge, string ip, int csid)
        {
            try
            {
                ChatMessage chatMessageWy = new ChatMessage();
                chatMessageWy.From = fromToken;
                chatMessageWy.CsID = csid;
                chatMessageWy.Message = messge;
                chatMessageWy.Time = DateTime.Now;

                CometMessage cometMessage = new CometMessage()
                {
                    Name = "MLline",
                    Contents = BLL.Util.DataContractObject2Json(chatMessageWy, typeof(ChatMessage)), //JsonConvert.SerializeObject(chatMessageWy),
                    FromToken = fromToken,
                    ToToken = toToken,
                    IISIP = ip
                };
                SendMessage(cometMessage);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("方法：{0},出错：{1}", "CometStateManager.SendLeaveLineMsg", ex.Message));
            }
        }

        /// <summary>
        /// 坐席关闭网友
        /// </summary>
        /// <param name="agentClient"></param>
        /// <param name="strNetFriendToken"></param>
        public void AgentRemoveNetFriend(ProxyAgentClient agentClient, string strNetFriendToken)
        {
            BLL.Loger.Log4Net.Info(string.Format("方法：{0};", "CometStateManager.AgentRemoveNetFriend"));
            try
            {
                this.cometmessagedeal.ForceSyncMessage();
                //将网友从坐席对话列表中删除。
                //this.StateProvider.RemoveSingleUser(strNetFriendToken, agentClient.AgentToken);

                var channel = GetAutoWCFClient();

                var netFriendClient = channel.GetNetFrind(strNetFriendToken);
                if (netFriendClient != null)
                {
                    //非本地网友，给网友发离线消息
                    var channel1 = GetAutoWCFClient();
                    channel1.RemoveNetFriend(strNetFriendToken, true, false, (int)Entities.CloseType.AgentClose);
                    //return;
                }

                //坐席，网友在同一个服务器上,直接给网友发送离线消息
                //SendLeaveLineMsg(agentClient.PrivateToken, strNetFriendToken, string.Format("网友{0}被移除.", strNetFriendToken), netFriendClient.IPIISWY, netFriendClient.CSId);

                KillWaitRequest(strNetFriendToken);
                KillIdleCometClient(netFriendClient.Token, 2);

                //channel.RemoveNetFriend(strNetFriendToken, false, false, (int)Entities.CloseType.AgentClose);

            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("方法：{0},出错：{1}", "CometStateManager.SendLeaveLineMsg", ex.Message));
            }
        }

        /// <summary>
        /// 坐席主动退出
        /// </summary>
        /// <param name="agentClient"></param>
        public void AgentQuit(ProxyAgentClient agentClient)
        {
            try
            {
                BLL.Loger.Log4Net.Info("坐席退出：AgentQuit");
                //agentClient.Status = AgentStatus.Leaveline;
                //同步坐席离线消息
                NoticeAgentRemoved(agentClient, (int)Entities.CloseType.AgentClose);
                KillWaitRequest(agentClient.AgentToken);
                KillIdleCometClient(agentClient.AgentToken, 1);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("方法：{0},出错：{1}", "CometStateManager.AgentQuit", ex.Message));
            }

        }

        public void ForceAgentLeave(string agentToken)
        {
            try
            {
                //var agentClient = GetCometClient(agentToken);
                //if (agentClient == null) { return; }

                BLL.Loger.Log4Net.Info("ForceAgentLeave        坐席退出");
                //agentClient.Status = AgentStatus.Leaveline;

                //移除坐席当前本地在聊网友，非本地网友移除逻辑由WCF完成
                //RemoveLocalAgentTalkUsers((CometClient)agentClient);

                KillWaitRequest(agentToken);
                //KillIdleCometClient(agentToken, 1);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("方法：{0},出错：{1}", "CometStateManager.AgentQuit", ex.Message));
            }
        }

        /// <summary>
        /// 网友主动退出
        /// </summary>
        /// <param name="netFriendClient"></param>
        public void NetFriendQuit(ProxyNetFriend netFriendClient)
        {
            BLL.Loger.Log4Net.Info("网友退出：NetFriendQuit");
            try
            {
                NoticeNetFriendRemoved(netFriendClient, (int)Entities.CloseType.NetFrindClose);

                KillWaitRequest(netFriendClient.Token);
                KillIdleCometClient(netFriendClient.Token, 2);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("方法：{0},出错：{1}", "CometStateManager.NetFriendQuit", ex.Message));
            }

        }
        #endregion

        /// <summary>
        /// 移除CometClient对象
        /// </summary>
        /// <param name="cometClient"></param>
        public void KillIdleCometClient(string token)
        {
            ProxyAgentClient agentClient = null;
            ProxyNetFriend netFrind = null;
            int Type = 0;
            try
            {
                GetCometClient(token, ref netFrind, ref agentClient, ref Type);
                //add by qizq 2015-1-7 加监控入库日志
                Entities.UserActionLog model = new UserActionLog();
                model.CreateTime = System.DateTime.Now;
                if (Type == 1 && agentClient != null)
                {
                    model.CreateUserID = agentClient.AgentID;

                    model.OperUserType = 1;

                    //坐席退出
                    model.LogInType = 7;
                    //修改坐席状态结束时间,记得维护agentClient的AgentStatusRecID
                    BLL.AgentStatusDetail.Instance.UpdateAgentLastStatus(agentClient.AgentStatusRecID);
                }
                else
                {
                    //客户退出
                    model.LogInType = 5;
                    model.OperUserType = 2;
                    //model.TrueName = tComnet.MemberName;
                }
                model.LogInfo = string.Format("InProcCometStateProvider.KillIdleCometClient 移除comet对象：{0},对象类型：{1}", token,
                    Type == 1 ? "客服" : "网友");
                BulkInserUserActionThread.EnQueueActionLogs(model);

            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("CometStateManager.KillIdleCometClient中异常：{0}", ex.Message));
            }

        }

        /// <summary>
        /// 移除CometClient对象
        /// </summary>
        /// <param name="cometClient"></param>
        public void KillIdleCometClient(string token, int Type)
        {
            //ProxyAgentClient agentClient = null;
            //ProxyNetFriend netFrind = null;
            //int Type = 0;
            try
            {
                //GetCometClient(token, ref netFrind, ref agentClient, ref Type);
                //add by qizq 2015-1-7 加监控入库日志
                Entities.UserActionLog model = new UserActionLog();
                model.CreateTime = System.DateTime.Now;
                if (Type == 1)
                {
                    model.CreateUserID = int.Parse(token.Split('@')[0]);

                    model.OperUserType = 1;

                    //坐席退出
                    model.LogInType = 7;
                    //修改坐席状态结束时间,记得维护agentClient的AgentStatusRecID
                    //BLL.AgentStatusDetail.Instance.UpdateAgentLastStatus(agentClient.AgentStatusRecID);
                }
                else
                {
                    //客户退出
                    model.LogInType = 5;
                    model.OperUserType = 2;
                    //model.TrueName = tComnet.MemberName;
                }
                model.LogInfo = string.Format("InProcCometStateProvider.KillIdleCometClient 移除comet对象：{0},对象类型：{1}", token,
                    Type == 1 ? "客服" : "网友");
                BulkInserUserActionThread.EnQueueActionLogs(model);

            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("CometStateManager.KillIdleCometClient中异常：{0}", ex.Message));
            }

        }


        /// <summary>
        /// 移除线程中的request请求对象
        /// </summary>
        /// <param name="clientPrivateToken"></param>
        public void KillWaitRequest(string clientPrivateToken)
        {
            workerThread.RemoveWaitRequestByToken(clientPrivateToken);
        }
        public void KillWaitRequest(CometClient cometClient)
        {
            workerThread.RemoveWaitRequestByToken(cometClient.PrivateToken);
        }


        //public CometClient GetCometClient(string clientPrivateToken)
        //{
        //    return this.stateProvider.GetCometClient(clientPrivateToken);
        //}

        public void GetCometClient(string clientPrivateToken, ref ProxyNetFriend netFrind, ref ProxyAgentClient agentClient, ref int Type)
        {
            try
            {
                netFrind = null;
                agentClient = null;
                Type = 0;
                if (clientPrivateToken.IndexOf('@') > 0)
                {
                    int _agentID = 0;
                    if (int.TryParse(clientPrivateToken.Split('@')[0], out _agentID))
                    {
                        var channel = GetAutoWCFClient();
                        agentClient = channel.GetAgentByToken(_agentID);
                    }
                    if (agentClient == null)
                    {
                        var channel = GetAutoWCFClient();
                        netFrind = channel.GetNetFrind(clientPrivateToken);
                        if (netFrind != null)
                        {
                            Type = 2;
                        }
                    }
                    else
                    {
                        Type = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("StateManager中方法GetCometClient 出错：{0}", ex.Message));
            }
        }


        /// <summary>
        /// 判断网友实体是否存在
        /// </summary>
        /// <param name="clientPrivateToken"></param>
        /// <returns></returns>
        public bool IsNetFrindExists(string clientPrivateToken)
        {
            try
            {
                var channel = GetAutoWCFClient();
                return channel.IsNetFrindExists(clientPrivateToken);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("StateManager中方法 IsNetFrindExists 出错：{0}", ex.Message));
                return false;
            }
        }

        /// <summary>
        /// 取网友实体
        /// </summary>
        /// <param name="clientPrivateToken"></param>
        /// <returns></returns>
        public ProxyNetFriend GetNetFrind(string clientPrivateToken)
        {
            try
            {
                var channel = GetAutoWCFClient();
                return channel.GetNetFrind(clientPrivateToken);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("StateManager中方法 GetNetFrind 出错：{0}", ex.Message));
                return null;
            }
        }


        private void WriteErrorToResponse(HttpContext context, CometAsyncResult result, string message)
        {
            try
            {
                var errorMessage = new CometMessage()
                {
                    Name = "aspNetComet.error",
                    MessageId = 0,
                    Contents = message
                };
                result.CometMessages = new CometMessage[] { errorMessage };
                result.SetCompleted();
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("方法 WriteErrorToResponse Exception:" + context.Request["privateToken"] + ex.Message);
            }

        }
        /// <summary>
        /// 给前端返回超时消息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        /// <param name="messageID"></param>
        private void WriteTimeOutToResponse(HttpContext context, CometAsyncResult result)
        {
            try
            {
                var timeOutMessage = new CometMessage()
                {
                    MessageId = 0,
                    Name = "aspNetComet.timeout",
                    Contents = null
                };
                result.CometMessages = new CometMessage[] { timeOutMessage };
                result.SetCompleted();
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("方法 WriteTimeOutToResponse Exception:" + context.Request["privateToken"] + ex.Message);
            }

        }



        #region 自定义方法
        /// <summary>
        /// 设置坐席状态
        /// </summary>
        /// <param name="clientPrivateToken"></param>
        /// <param name="state"></param>
        /// <returns>0:成功；-1:坐席不存在,-2:错误</returns>
        public int SetAgentState(string clientPrivateToken, AgentStatus state)
        {
            try
            {
                var channel = GetAutoWCFClient();
                int agentID = 0;
                int.TryParse(clientPrivateToken.Split('@')[0], out agentID);
                ProxyAgentClient agentClient = channel.GetAgentByToken(agentID);
                if (agentClient != null)
                {
                    int agentStatusRecID = 0;
                    agentStatusRecID = BLL.AgentStatusDetail.Instance.AddAgentStatusData(agentID, (int)state, agentClient.AgentStatusRecID);
                    channel = GetAutoWCFClient();
                    return channel.SetAgentStatus(agentClient.AgentID, (int)state, agentStatusRecID);
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info(string.Format("StateManager中方法 SetAgentState 出错：{0}", ex.Message));
            }
            return -2;
        }


        ///// <summary>
        ///// 重新分配离线客服下所有的网友
        ///// </summary>
        ///// <param name="privateToken">离席客服ID</param>
        //public void ReAllocateOffLineAgentUsers(CometClient cometClient)
        //{
        //    if (cometClient != null)
        //    {
        //        //移除在聊网友
        //        foreach (string wu in cometClient.TalkUserList.ToArray())
        //        {
        //            cometClient.RemoveTalkUser(wu);
        //        }
        //    }
        //}

        /*
        /// <summary>
        /// 移除指定坐席下的网友，在坐席关闭在聊网友时使用
        /// </summary>
        /// <param name="wyToken"></param>
        /// <param name="zxToken"></param>
       
        public void RemoveSingleUser(string wyToken, string zxToken)
        {
            stateProvider.RemoveSingleUser(wyToken, zxToken);
            var netFrinedClient = GetCometClient(wyToken);

            //TODO 强制同步消息
            try
            {
                //本地网友时直接发送离线消息
                if (netFrinedClient.IsNative)
                {
                    ChatMessage chatMessageWy = new ChatMessage();
                    chatMessageWy.From = zxToken;
                    chatMessageWy.Message = string.Format("坐席{0}已离线.", zxToken);
                    chatMessageWy.Time = DateTime.Now;

                    SendMessage(wyToken, "MLline", JsonConvert.SerializeObject(chatMessageWy), netFrinedClient.IPIISWY, 0);
                }

                var channel = GetWCFClient();
                channel.RemoveNetFriend(wyToken, true, false);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info(string.Format("StateManager中方法 RemoveSingleUser 出错：{0}", ex.Message));
            }
            KillWaitRequest(wyToken);
            KillIdleCometClient(GetCometClient(wyToken));
            
        }
        */

        #endregion


        //public List<CometClient> GetAllCometClients()
        //{
        //    return this.StateProvider.GetAllCometClients();
        //}


        public string CheckState(int agentId, string csids)
        {
            //if (string.IsNullOrWhiteSpace(csids)) return "[]";
            try
            {
                var channel = GetAutoWCFClient();
                return channel.CheckStates(agentId, csids);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info(string.Format("StateManager中方法 CheckState 出错：{0}", ex.Message));
                return "[],\"astatus\":0 ";
            }

        }

        /// <summary>
        /// 根据网友IP，与业务线标识，是否已登录
        /// </summary>
        /// <param name="netfriendip"></param>
        /// <param name="bussiness"></param>
        /// <returns></returns>
        public string IpIsLogined(string netfriendip, string bussiness)
        {
            //if (string.IsNullOrWhiteSpace(csids)) return "[]";
            string flag = "0";
            ProxyNetFriend[] wyarray = null;
            wyarray = GetWCFAllNetFriends();
            if (wyarray != null && wyarray.Length > 0)
            {
                for (int i = 0; i < wyarray.Length; i++)
                {
                    if (wyarray[i].IISIP == netfriendip)
                    {
                        flag = "1";
                        return flag;
                    }
                }
            }
            List<ProxyNetFriend> QueueNetFriend = GetWaitingCometClientsByBusinessLine(bussiness);
            if (QueueNetFriend != null && QueueNetFriend.Count > 0)
            {
                for (int i = 0; i < QueueNetFriend.Count; i++)
                {
                    if (QueueNetFriend[i].IISIP == netfriendip)
                    {
                        flag = "1";
                        return flag;
                    }
                }
            }
            return flag;

        }



        public void TransferAgent(string strSourceAgent, int strTargetAgent, string strWYID)
        {
            try
            {
                this.cometmessagedeal.ForceSyncMessage();
                var channel = GetAutoWCFClient();
                int agentid = 0;
                int.TryParse(strSourceAgent.Split('@')[0], out agentid);
                var fromClient = channel.GetAgentByToken(agentid);
                //从当前坐席中移除网友
                //this.StateProvider.RemoveSingleUser(strWYID, strSourceAgent);

                if (fromClient != null)
                {
                    channel = GetAutoWCFClient();
                    channel.TransferNetFriend(Convert.ToInt32(fromClient.AgentID), strTargetAgent, strWYID);
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info(string.Format("StateManager中方法 TransferAgent 出错：{0}", ex.Message));
            }

            #region 记日志
            //UserActionLog SourceLog = new UserActionLog()
            //{
            //    CreateTime = DateTime.Now,
            //    CreateUserID = -2,
            //    IP = string.Empty,
            //    OperUserType = 1,
            //    LogInType = 10, //转出
            //    LogInfo = string.Format(" 从坐席 {0} 转出网友 {1} 给新坐席{2}. ", strSourceAgent, strWYID, strTargetAgent)
            //};
            //UserActionLog TagertLog = new UserActionLog()
            //{
            //    CreateTime = DateTime.Now,
            //    CreateUserID = -2,
            //    IP = string.Empty,
            //    OperUserType = 1,
            //    LogInType = 9, //转入
            //    LogInfo = string.Format(" 新坐席 {0} 从坐席{2}转出网友 {1} 给. ", strTargetAgent, strSourceAgent, strWYID)
            //};

            //BulkInserUserActionThread.EnQueueActionLogs(SourceLog);
            //BulkInserUserActionThread.EnQueueActionLogs(TagertLog);

            #endregion

        }

        public void Dispose()
        {
            stateProvider.Dispose();

            workerThread.Dispose();

        }


        public void WcfTest()
        {

            Parallel.For(0, 500, (i) =>
            {
                var channel = GetAutoWCFClient();
                try
                {

                    channel.WcfTest(i);
                    //(channel as ICommunicationObject).Close();
                }
                catch (CommunicationException)
                {
                    (channel as ICommunicationObject).Abort();
                }
                catch (TimeoutException)
                {
                    (channel as ICommunicationObject).Abort();
                }


            });


            Parallel.For(1500, 2000, (i) =>
            {
                var channel = GetManualWCFClient();
                try
                {

                    channel.WcfTest(i);
                    (channel as ICommunicationObject).Close();
                    //(channel as ICommunicationObject).Close();
                }
                catch (CommunicationException)
                {
                    (channel as ICommunicationObject).Abort();
                }
                catch (TimeoutException)
                {
                    (channel as ICommunicationObject).Abort();
                }


            });
        }

        private void SendMsgToClientForPressureTest()
        {
            //tmPresstest = new timer(obj =>
            //{
            //    var allagents = getallcometclients();
            //    if (allagents.count > 0)
            //    {
            //        allagents = allagents.where(s => s.type == 1).tolist();
            //    }
            //    int i = 0;
            //    var messagetype = "chatmessage";
            //    foreach (cometclient cometclient in allagents)
            //    {
            //        var talklist = cometclient.talkuserlist.tolist();
            //        foreach (string s in talklist)
            //        {
            //            string str = string.format("来自坐席：{0}自动发送的消息,参数:{1}:时间{2}", cometclient.agentnum, ++i, datetime.now.tostring("o"));
            //            var chatmessage = new chatmessage()
            //            {
            //                message = str,
            //                csid = -100,
            //                from = cometclient.agentid.tostring(),
            //                time = datetime.now
            //            };
            //            //this.stateprovider.sendmessage(s, messagetype, str);
            //            this.stateprovider.sendmessage(s, messagetype, chatmessage);
            //        }
            //    }

            //    tmpresstest.change(6000, timeout.infinite);
            //}, null, 2000, timeout.infinite);



        }

        //public DataTable GetAllAgentsStatus4Monotor()
        //{
        //    var a = GetAllCometClients();
        //    var Agents = a.Where(s => s.Type == 1).ToList().OrderBy(s => s.AgentID);


        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("id");
        //    dt.Columns.Add("name");
        //    dt.Columns.Add("talkNum");

        //    var datas = (from ags in Agents
        //                 select new
        //                 {
        //                     id = ags.AgentID,
        //                     name = ags.AgentName,
        //                     talkNum = a.Where(wy => wy.AgentID == ags.AgentID).Count()
        //                 }).ToList();

        //    foreach (var data in datas)
        //    {
        //        var dr = dt.NewRow();
        //        dr[0] = data.id;
        //        dr[1] = data.name;
        //        dr[2] = data.talkNum;
        //        dt.Rows.Add(dr);
        //    }

        //    return dt;
        //}

        public void ReceiveMessage(CometMessage[] messages)
        {
            //cometmessagedeal.MessageDeal(messages);
        }





        //报表相关
        //根据条件取客服实时监控数据
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inbgid">分组id，如果-1则是所有分组的意思</param>
        /// <param name="agentid">客服id</param>
        /// <param name="agentnum">客服工号</param>
        /// <param name="status">状态，2离线,1非离线</param>
        /// <returns>InBGIDName分组名称，InBGID分组id，AgentID坐席id，AgentName坐席名称，AgentNum坐席工号，StatusName状态名称，Status状态标识，MaxDialogNum最大聊天量，CurrentDialogNum当前对话量，CurrentReceptionDNum当前接待量，NoReceptionDNum尚未响应量，SaturationRate饱和度,NoReceptionRate尚未响应率</returns>
        public DataTable GetServiceMonitoring(string inbgid, string agentid, string agentnum, string status)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("InBGIDName", typeof(string));
            dt.Columns.Add("InBGID", typeof(string));
            dt.Columns.Add("AgentID", typeof(int));
            dt.Columns.Add("AgentName", typeof(string));
            dt.Columns.Add("AgentNum", typeof(string));
            dt.Columns.Add("StatusName", typeof(string));
            dt.Columns.Add("Status", typeof(int));
            dt.Columns.Add("MaxDialogNum", typeof(int));
            dt.Columns.Add("CurrentDialogNum", typeof(int));
            dt.Columns.Add("CurrentReceptionDNum", typeof(int));
            dt.Columns.Add("NoReceptionDNum", typeof(int));
            dt.Columns.Add("SaturationRate", typeof(Decimal));
            dt.Columns.Add("NoReceptionRate", typeof(Decimal));
            ProxyAgentClient[] agentarray = null;
            agentarray = GetWCFAllAgents();
            if (agentarray != null)
            {
                for (int i = 0; i < agentarray.Length; i++)
                {
                    ProxyAgentClient agentclient = agentarray[i];
                    DataRow r = dt.NewRow();
                    r["InBGIDName"] = agentclient.InBGIDName;
                    r["InBGID"] = agentclient.InBGID;
                    r["AgentID"] = agentclient.AgentID;
                    r["AgentName"] = agentclient.AgentName;
                    r["AgentNum"] = agentclient.AgentNum;
                    r["StatusName"] = agentclient.Status == 1 ? "在线" : agentclient.Status == 2 ? "离线" : "暂离";
                    r["Status"] = agentclient.Status;
                    r["MaxDialogNum"] = agentclient.MaxDialogNum;
                    int talkCount = 0;
                    int ReceptionCount = 0;
                    GetReceptionDNum(agentarray[i], out talkCount, out ReceptionCount);
                    r["CurrentDialogNum"] = talkCount;
                    r["CurrentReceptionDNum"] = ReceptionCount;
                    r["NoReceptionDNum"] = talkCount - ReceptionCount;
                    r["NoReceptionRate"] = talkCount != 0 ? decimal.Round((decimal)(talkCount - ReceptionCount) / talkCount, 2) : 0;
                    r["SaturationRate"] = agentclient.MaxDialogNum != 0 ? decimal.Round((decimal)talkCount / agentclient.MaxDialogNum, 2) : 0;
                    dt.Rows.Add(r);
                }
            }
            string sqlwhere = " 1=1 ";
            if (inbgid != "-1")
            {
                sqlwhere += " and InBGID='" + StringHelper.SqlFilter(inbgid) + "'";
            }
            else
            {
                DataTable dtbg = BLL.BaseData.Instance.GetUserGroupDataRigth();
                if (dtbg != null && dtbg.Rows.Count > 0)
                {
                    sqlwhere += " and (";
                    for (int i = 0; i < dtbg.Rows.Count; i++)
                    {
                        if (i != (dtbg.Rows.Count - 1))
                        {
                            sqlwhere += " InBGID='" + StringHelper.SqlFilter(dtbg.Rows[i]["BGID"].ToString()) + "' or ";
                        }
                        else
                        {
                            sqlwhere += " InBGID='" + StringHelper.SqlFilter(dtbg.Rows[i]["BGID"].ToString()) + "')";
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(agentid))
            {
                sqlwhere += " and AgentID='" + StringHelper.SqlFilter(agentid) + "'";
            }
            if (!string.IsNullOrEmpty(agentnum))
            {
                sqlwhere += " and agentnum='" + StringHelper.SqlFilter(agentnum) + "'";
            }
            if (!string.IsNullOrEmpty(status))
            {
                if (status == "2")
                {
                    sqlwhere += " and Status='" + StringHelper.SqlFilter(status) + "'";
                }
                else
                {
                    sqlwhere += " and (Status='1' or Status='3')";
                }
            }

            DataTable newdt = new DataTable();
            newdt = dt.Clone(); // 克隆dt 的结构，包括所有 dt 架构和约束,并无数据；
            DataRow[] rows = dt.Select(sqlwhere, " status,SaturationRate"); // 从dt 中查询符合条件的记录；
            foreach (DataRow row in rows)  // 将查询的结果添加到dt中；
            {
                newdt.Rows.Add(row.ItemArray);
            }
            return newdt;
        }
        /// <summary>
        /// 根据坐席对象，取坐席接待网友人数,在聊人数
        /// </summary>
        /// <param name="agentclient"></param>
        /// <returns></returns>
        public void GetReceptionDNum(ProxyAgentClient agentclient, out int talkCount, out int ReceptionCount)
        {
            talkCount = 0;
            ReceptionCount = 0;
            //取所有在聊网友
            ProxyNetFriend[] ArrayNetFriend = null;
            ArrayNetFriend = GetWCFAllNetFriends();
            if (ArrayNetFriend != null && ArrayNetFriend.Length > 0)
            {
                for (int i = 0; i < ArrayNetFriend.Length; i++)
                {
                    ProxyNetFriend netfriend = ArrayNetFriend[i];
                    if (netfriend != null && netfriend.AgentToken == agentclient.AgentToken)
                    {
                        talkCount++;
                        if (netfriend.ConverSTime > new DateTime(1900, 1, 1))
                        {
                            ReceptionCount++;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 取坐席实时在线情况
        /// </summary>
        /// <returns>InBGIDName：分组名称，InBGID：分组id，AgentIDCount：分组人数，AgentOnlineCount:在线客服人数，AgentBussyCount:离线客服人数，AgentBussyCount：暂离客服数，AgentReceptionCount：接待客服人数</returns>
        public DataTable GetAgentRealTime_OnlineHaveBGID()
        {
            //取所有业务分组,以及分组人数
            DataTable dtBGID = BLL.BaseData.Instance.GetOnlineBgIDHaveUserCount();
            DataTable dt = new DataTable();
            dt.Columns.Add("InBGIDName", typeof(string));
            dt.Columns.Add("InBGID", typeof(string));
            dt.Columns.Add("AgentIDCount", typeof(int));
            dt.Columns.Add("AgentLoginCount", typeof(int));
            dt.Columns.Add("AgentOnlineCount", typeof(int));
            dt.Columns.Add("AgentBussyCount", typeof(int));
            dt.Columns.Add("AgentLeaveCount", typeof(int));
            dt.Columns.Add("AgentReceptionCount", typeof(int));
            dt.Columns.Add("AgentIdleount", typeof(int));
            //取所有坐席
            ProxyAgentClient[] agentarray = null;
            agentarray = GetWCFAllAgents();
            if (dtBGID != null && dtBGID.Rows.Count > 0)
            {
                int allagentCount = 0;
                int allAgentOnlineCount = 0;
                int allAgentBussyCount = 0;
                int allAgentLeaveCount = 0;
                int allAgentReceptionCount = 0;
                int allAgentLoginCount = 0;
                int allAgentIdleount = 0;
                for (int agentcount = 0; agentcount < dtBGID.Rows.Count; agentcount++)
                {
                    string InBGIDName = dtBGID.Rows[agentcount]["Name"].ToString();
                    string InBGID = dtBGID.Rows[agentcount]["BGID"].ToString();
                    int AgentIDCount = Convert.ToInt32(dtBGID.Rows[agentcount]["usercount"].ToString());
                    allagentCount += AgentIDCount;
                    int AgentOnlineCount = 0;
                    int AgentBussyCount = 0;
                    int AgentLeaveCount = 0;
                    int AgentReceptionCount = 0;
                    int AgentLoginCount = 0;
                    if (agentarray != null)
                    {
                        allAgentLoginCount = agentarray.Length;
                        for (int i = 0; i < agentarray.Length; i++)
                        {
                            if (agentarray[i].InBGID == InBGID)
                            {
                                AgentLoginCount++;
                                if (agentarray[i].Status == 1)
                                {
                                    AgentOnlineCount++;
                                    allAgentOnlineCount++;
                                }
                                if (agentarray[i].Status == 2)
                                {
                                    AgentLeaveCount++;
                                    allAgentLeaveCount++;

                                }
                                if (agentarray[i].Status == 3)
                                {
                                    AgentBussyCount++;
                                    allAgentBussyCount++;
                                }
                                int talkCount = 0;
                                int ReceptionCount = 0;
                                GetReceptionDNum(agentarray[i], out talkCount, out ReceptionCount);
                                if (ReceptionCount > 0)
                                {
                                    AgentReceptionCount++;
                                    allAgentReceptionCount++;
                                }
                            }
                        }
                    }
                    DataRow r = dt.NewRow();
                    r["InBGIDName"] = InBGIDName;
                    r["InBGID"] = InBGID;
                    r["AgentIDCount"] = AgentIDCount;
                    r["AgentLoginCount"] = AgentLoginCount;
                    r["AgentOnlineCount"] = AgentOnlineCount;
                    r["AgentBussyCount"] = AgentBussyCount;
                    r["AgentLeaveCount"] = AgentLeaveCount;
                    r["AgentReceptionCount"] = AgentReceptionCount;
                    r["AgentIdleount"] = AgentOnlineCount + AgentBussyCount - AgentReceptionCount;
                    //allAgentOnlineCount + allAgentBussyCount - allAgentReceptionCount;
                    allAgentIdleount += AgentOnlineCount + AgentBussyCount - AgentReceptionCount;
                    dt.Rows.Add(r);
                }
                if (allagentCount > 0)
                {
                    DataRow rSum = dt.NewRow();
                    rSum["InBGIDName"] = "合计(共" + dt.Rows.Count + "项)";
                    rSum["InBGID"] = "";
                    rSum["AgentIDCount"] = allagentCount;
                    rSum["AgentLoginCount"] = allAgentLoginCount;
                    rSum["AgentOnlineCount"] = allAgentOnlineCount;
                    rSum["AgentBussyCount"] = allAgentBussyCount;
                    rSum["AgentLeaveCount"] = allAgentLeaveCount;
                    rSum["AgentReceptionCount"] = allAgentReceptionCount;
                    rSum["AgentIdleount"] = allAgentIdleount;
                    dt.Rows.Add(rSum);
                }
            }
            return dt;
        }

        /// <summary>
        /// 取坐席实时在线情况
        /// </summary>
        /// <returns>AgentOnlineCount:在线客服人数，AgentBussyCount:离线客服人数，AgentBussyCount：暂离客服数，AgentReceptionCount：接待客服人数，AgentIdleount：空闲客服人数</returns>
        public DataTable GetAgentRealTime_Online()
        {
            //取所有业务分组,以及分组人数
            DataTable dt = new DataTable();
            dt.Columns.Add("AgentOnlineCount", typeof(int));
            dt.Columns.Add("AgentBussyCount", typeof(int));
            dt.Columns.Add("AgentLeaveCount", typeof(int));
            dt.Columns.Add("AgentReceptionCount", typeof(int));
            dt.Columns.Add("AgentIdleount", typeof(int));
            //取所有坐席
            ProxyAgentClient[] agentarray = null;
            agentarray = GetWCFAllAgents();
            int allAgentOnlineCount = 0;
            int allAgentBussyCount = 0;
            int allAgentLeaveCount = 0;
            int allAgentReceptionCount = 0;
            if (agentarray != null)
            {
                for (int i = 0; i < agentarray.Length; i++)
                {
                    if (agentarray[i].Status == 1)
                    {
                        allAgentOnlineCount++;
                    }
                    if (agentarray[i].Status == 2)
                    {
                        allAgentLeaveCount++;
                    }
                    if (agentarray[i].Status == 3)
                    {
                        allAgentBussyCount++;
                    }

                    int talkCount = 0;
                    int ReceptionCount = 0;
                    GetReceptionDNum(agentarray[i], out talkCount, out ReceptionCount);
                    if (ReceptionCount > 0)
                    {
                        allAgentReceptionCount++;
                    }
                }
            }
            DataRow rSum = dt.NewRow();
            rSum["AgentOnlineCount"] = allAgentOnlineCount;
            rSum["AgentBussyCount"] = allAgentBussyCount;
            rSum["AgentLeaveCount"] = allAgentLeaveCount;
            rSum["AgentReceptionCount"] = allAgentReceptionCount;
            if (agentarray != null)
            {
                rSum["AgentIdleount"] = allAgentOnlineCount + allAgentBussyCount - allAgentReceptionCount;
            }
            else
            {
                rSum["AgentIdleount"] = 0;
            }
            dt.Rows.Add(rSum);
            return dt;
        }

        /// <summary>
        /// 根据业务线，取业务线排队中访客量,-1取所有业务线排队量
        /// </summary>
        /// <param name="strBusinessLines"></param>
        /// <returns></returns>
        public int GetQueueCountByBusinessLine(string strBusinessLines)
        {
            int queuecount = 0;
            if (strBusinessLines != "-1")
            {
                List<ProxyNetFriend> listProxyNetFriend = GetWaitingCometClientsByBusinessLine(strBusinessLines);
                if (listProxyNetFriend != null)
                {

                    queuecount = listProxyNetFriend.Count;
                }
            }
            else
            {
                List<SourceType> list = new List<SourceType>();
                list = BLL.Util.GetAllSourceType(false);
                if (list != null && list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        List<ProxyNetFriend> listProxyNetFriend = GetWaitingCometClientsByBusinessLine(list[i].SourceTypeValue);
                        if (listProxyNetFriend != null)
                        {
                            queuecount += listProxyNetFriend.Count;
                        }
                    }
                }
            }
            return queuecount;
        }
        /// <summary>
        /// 根据业务线，取业务线对话中访客量，-1取所有业务线对话量
        /// </summary>
        /// <param name="strBusinessLines"></param>
        /// <returns></returns>
        public int GetConvertCountByBusinessLine(string strBusinessLines)
        {
            int queuecount = 0;
            ProxyNetFriend[] arryNetFriend = GetWCFAllNetFriends();
            if (strBusinessLines == "-1")
            {
                //取所有在聊网友数量
                if (arryNetFriend != null)
                {
                    queuecount = arryNetFriend.Length;
                }
            }
            else
            {
                if (arryNetFriend != null && arryNetFriend.Length > 0)
                {
                    for (int i = 0; i < arryNetFriend.Length; i++)
                    {
                        if (arryNetFriend[i].BusinessLines == strBusinessLines)
                        {
                            queuecount++;
                        }
                    }
                }
            }
            return queuecount;
        }
        /// <summary>
        /// 根据实体服务机器IP
        /// </summary>
        /// <param name="token"></param>
        /// <param name="serviceIp"></param>
        public void UpdateServiceIp(string token, string serviceIp)
        {
            try
            {
                var channel = GetAutoWCFClient();
                //默认是网友，因为坐席pc.
                int Type = 0;
                channel.UpdateClientPostion(token, serviceIp, Type);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info(string.Format("StateManager中方法 UpdateNetFriendAgentReplayTime 出错：{0}", ex.Message));
            }
        }


        /// <summary>
        /// 修改wcf网友属性，坐席首次回复时间
        /// </summary>
        public void UpdateNetFriendAgentReplayTime(string NetFrindKey)
        {
            try
            {
                var channel = GetAutoWCFClient();
                channel.UpdateNetFriendAgentReplayTime(NetFrindKey);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info(string.Format("StateManager中方法 UpdateNetFriendAgentReplayTime 出错：{0}", ex.Message));
            }
        }

        /// <summary>
        /// 根据标识，判断该对象在wcf里是否存在
        /// </summary>
        /// <param name="privatetoken"></param>
        /// <returns></returns>
        public bool IsExistsWcf(string privatetoken)
        {
            bool flag = false;
            //
            try
            {
                //所有坐席
                ProxyAgentClient[] agentarray = null;
                agentarray = GetWCFAllAgents();
                if (agentarray != null && agentarray.Length > 0)
                {
                    for (int i = 0; i < agentarray.Length; i++)
                    {
                        ProxyAgentClient agentclient = agentarray[i];
                        if (agentclient != null && agentclient.AgentToken == privatetoken)
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                //所有网友
                if (flag == false)
                {
                    ProxyNetFriend[] ArrayNetFriend = null;
                    ArrayNetFriend = GetWCFAllNetFriends();
                    if (ArrayNetFriend != null && ArrayNetFriend.Length > 0)
                    {
                        for (int i = 0; i < ArrayNetFriend.Length; i++)
                        {
                            ProxyNetFriend netfriend = ArrayNetFriend[i];
                            if (netfriend != null && netfriend.Token == privatetoken)
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                }
                //所有排队网友
                if (flag == false)
                {
                    List<SourceType> list = new List<SourceType>();
                    list = BLL.Util.GetAllSourceType(false);
                    if (list != null && list.Count > 0)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            List<ProxyNetFriend> listProxyNetFriend = GetWaitingCometClientsByBusinessLine(list[i].SourceTypeValue);
                            if (listProxyNetFriend != null && listProxyNetFriend.Count > 0)
                            {
                                for (int n = 0; n < listProxyNetFriend.Count; n++)
                                {
                                    ProxyNetFriend netfriendQuene = listProxyNetFriend[n];
                                    if (netfriendQuene != null && netfriendQuene.Token == privatetoken)
                                    {
                                        flag = true;
                                        break;
                                    }

                                }
                                if (flag)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info(string.Format("StateManager中方法 IsExistsWcf 出错：{0}", ex.Message));
            }
            return flag;
        }

    }
}
