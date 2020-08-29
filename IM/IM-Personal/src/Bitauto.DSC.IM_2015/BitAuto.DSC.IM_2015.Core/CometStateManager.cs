using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Runtime.Serialization.Json;
using BitAuto.DSC.IM_2015.Messages;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Core.Messages;
using System.Configuration;
using BitAuto.DSC.IM_2015.WebService.CC;
using log4net.Repository.Hierarchy;

namespace BitAuto.DSC.IM_2015.Core
{
    /// <summary>
    /// CometStateManger Class
    /// 
    /// An instance of the this class is used to control the manager the state of the COMET Application.
    /// This class manages an instance of a ICometStateProvider instance
    /// </summary>
    /// 
    public class CometStateManager : IDisposable
    {
        private ICometStateProvider stateProvider;
        private int workerThreadCount;
        private int maximumTimeSlot;
        private int currentThread = 0;
        private System.Threading.Timer tmPressTest;
        private CometWaitThread[] workerThreads;
        private object state = new object();
        private WaitAgentAllocThread mainAllocateThread;

        /// <summary>
        /// 是否是批量测试
        /// </summary>
        //public bool IsBatchDebug = false;

        //add by qizq 聊天记录入库线程类 add by qizq 2014-3-6
        private MessageInDBThread messageInDBThread;

        //add by qizq 坐席分配线程类 add by qizq 2014-3-6
        //private WaitAgentAllocThread waitAgentAllocThread;

        /// <summary>
        /// Event that is called when a Client is Initialized
        /// </summary>
        public event CometClientEventHandler ClientInitialized;
        /// <summary>
        /// Event that is called when a Client is killed
        /// </summary>
        public event CometClientEventHandler IdleClientKilled;
        /// <summary>
        /// Event that is called when a Client subscribes to this channel
        /// </summary>
        public event CometClientEventHandler ClientSubscribed;
        /// <summary>
        /// 当坐席状态发生变化时，触发该事件
        /// </summary>
        public event AgentStateEventHandler AgentStateOnChanged;

        /// <summary>
        /// Construct an instane of the CometStateManager class and pass in an
        /// instance of an ICometStateProvider to manage the persistence of the state
        /// </summary>
        /// <param name="stateProvider">An instance of an ICometStateProvider class that manages the persistence of the state</param>
        /// <param name="workerThreadCount">How many worked threads should this CometStateManager initialize</param>
        /// <param name="maximumTimeSlot">The maximum time in milliseconds that should be idle between each COMET client is polled within a worker thread</param>
        public CometStateManager(ICometStateProvider stateProvider, int workerThreadCount, int maximumTimeSlot)
        {
            //IsBatchDebug = ConfigurationManager.AppSettings["IsDebug"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["IsDebug"]);
            if (stateProvider == null)
                throw new ArgumentNullException("stateProvider");

            if (workerThreadCount <= 0)
                throw new ArgumentOutOfRangeException("workerThreadCount");

            //  ok, setup the member of this class
            this.stateProvider = stateProvider;
            this.workerThreadCount = workerThreadCount;
            this.maximumTimeSlot = maximumTimeSlot;

            this.workerThreads = new CometWaitThread[workerThreadCount];

            //设置线程池最大线程数为处理器核心数*10;
            //int nProcessCoreCount = System.Environment.ProcessorCount * 10;
            //ThreadPool.SetMaxThreads(nProcessCoreCount, nProcessCoreCount);
            //
            //  ok, lets fireup the threads
            for (int i = 0; i < workerThreadCount; i++)
            {
                this.workerThreads[i] =
                    new CometWaitThread(this);
            }
            //启动聊天记录入库线程 add by qizq 2014-3-6
            this.messageInDBThread = new MessageInDBThread(this);
            //启动等待队列处理线程
            this.mainAllocateThread = new WaitAgentAllocThread(this);
            //if (IsBatchDebug)
            //{
            //    SendMsgToClientForPressureTest();
            //}
            //启动坐席分配线程 add by qizq 2014-3-6
            //this.waitAgentAllocThread = new WaitAgentAllocThread(this);

        }

        /// <summary>
        /// Construct an instane of the CometStateManager class and pass in an
        /// instance of an ICometStateProvider to manage the persistence of the state
        /// 
        /// This calls the main constructor and specifies the default values workerThreadCount = 5 and maximumTimeSlot = 100.  These values
        /// can be tuned for your application by using the main constructor
        /// </summary>
        /// <param name="stateProvider">An instance of an ICometStateProvider class that manages the persistence of the state</param>
        public CometStateManager(ICometStateProvider stateProvider)
            : this(stateProvider, 1, 100)
        {
        }

        /// <summary>
        /// 根据业务线获取等待队列
        /// </summary>
        /// <param name="strBusinessLine"></param>
        /// <returns></returns>
        public List<CometClient> GetWaitingCometClientsByBusinessLine(string strBusinessLine)
        {
            return mainAllocateThread.GetCometClientsByBusinessLine(strBusinessLine);
        }

        public int EnQueueWaitAgent(string businessLine, string PrivateToken)
        {
            return mainAllocateThread.EnQueueWaitAgent(businessLine, PrivateToken);
        }

        private static void UpdateConversation(object obj)
        {
            var cE = obj as Conversations;
            if (cE == null)
            {
                return;
            }
            try
            {
                BLL.Conversations.Instance.CallBackUpdate(cE);
            }
            catch (Exception ex)
            {

                BLL.Loger.Log4Net.Error(string.Format("更新会话记录错误：CSID：{0},错误信息:{1}", cE.CSID, ex.Message));
            }

        }

        /// <summary>
        /// 初始化坐席增删网友事件
        /// </summary>
        /// <param name="client"></param>
        private void InitCometClientEvent(CometClient client)
        {
            #region OnTalkUserAdded

            client.OnTalkUserAdded += new EventHandler<UserArgs>((sender, e) =>
            {
                try
                {
                    //获取网友信息
                    //插入会话表
                    CometClient cAgent = sender as CometClient;
                    if (cAgent == null || cAgent.Status == AgentStatus.Leaveline) { return; }

                    CometClient wyClient = GetCometClient(e.CurrentUserId);
                    wyClient.ConverSTime = DateTime.Now;
                    wyClient.AgentSTime = DateTime.Now;
                    wyClient.AgentID = cAgent.PrivateToken;


                    var et = new Conversations()
                    {
                        //AgentStartTime = DateTime.Now,
                        //BGID = BLL.BaseData.Instance.GetAgentBGIDByUserID(Convert.ToInt32(e.AgentId)),
                        CreateTime = DateTime.Now,
                        EndTime = new DateTime(1900, 1, 1),
                        LastClientTime = new DateTime(1900, 1, 1),
                        OrderID = DBNull.Value.ToString(),
                        Status = 0,
                        UserID = Convert.ToInt32(e.AgentId),
                        UserName = cAgent.DisplayName,
                        VisitID = wyClient.Userloginfo.VisitID,
                        IsTurenIn = wyClient.IsTurnOut     //当网友是被转接过来时，则会话默认为转入
                    };

                    int nCSId = BLL.Conversations.Instance.Insert(et);
                    if (nCSId < 0)
                    {
                        var msg = string.Format("插入会话记录失败,AgentID:{0},UserName:{1}", e.AgentId, wyClient.DisplayName);
                        BLL.Loger.Log4Net.Info(msg);
                        //throw new Exception(msg);
                    }

                    AllocAgentMessage cmforuser = new AllocAgentMessage()
                    {
                        UserId = wyClient.PrivateToken,
                        VisitID = wyClient.Userloginfo.VisitID.ToString(),
                        contractphone = wyClient.Userloginfo.Phone,
                        //Address = userClient.Address,
                        AgentID = Convert.ToInt32(cAgent.PrivateToken),
                        //CityGroupName = userClient.DistrictName,
                        CsID = nCSId,
                        //ContractPhone = wyClient.Userloginfo.Phone,
                        //ContractJob = userClient.ContractJob,
                        WYName = wyClient.Userloginfo.UserName,
                        //ConverSTime = DateTime.Now,
                        //Distribution = userClient.Distribution,
                        //LastConBeginTime = userClient.LastConBeginTime,
                        //LastMessageTime = wyClient.LastMessageTime,
                        //MemberCode = userClient.MemberCode,
                        //MemberName = userClient.MemberName,
                        //Message = string.Empty,
                        //Time = DateTime.Now,
                        Converstime = wyClient.ConverSTime,
                        //AgentSTime = wyClient.AgentSTime,
                        //LoginID = wyClient.Userloginfo.LoginID,
                        //UserReferTitle = wyClient.Userloginfo.UserReferTitle,
                        AgentNum = cAgent.AgentNum


                    };

                    wyClient.CSId = nCSId;
                    wyClient.SendMessageTime = System.DateTime.Now;

                    string messagetype = string.Empty;

                    if (wyClient.IsTurnOut)
                    {
                        //如果网友是被转接过来的，则发送转接消息
                        messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MTransfer);
                    }
                    else
                    {
                        //如果网友不是被转接过来的，则发送初始化消息
                        messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MAllocAgent);
                    }

                    //以系统身份通知网友坐席为您服务
                    cAgent.SendMessageTime = DateTime.Now;
                    SendMessage(e.CurrentUserId, messagetype, cmforuser);
                    SendMessage(e.AgentId, messagetype, cmforuser);
                    //BLL.Loger.Log4Net.Info("分配用户成功");
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Info(string.Format("OnTalkUserAdded 方法中报错:{0} ", ex.Message));
                }
            });


            #endregion


            #region OnTalkUserRemoved

            client.OnTalkUserRemoved += new EventHandler<UserArgs>((sender, e) =>
            {
                try
                {
                    //如果坐席在线时，通知坐席，网友已经离线
                    CometClient cAgent = sender as CometClient;
                    CometClient cNowWY = GetCometClient(e.CurrentUserId);
                    if (cNowWY == null) { return; }

                    if (cAgent != null && cAgent.Status == AgentStatus.Online)
                    {
                        ChatMessage chatMessage = new ChatMessage();
                        chatMessage.CsID = cNowWY.CSId;
                        chatMessage.From = e.CurrentUserId;
                        chatMessage.Message = string.Format("网友{0}已离线.", e.CurrentUserId);
                        chatMessage.Time = DateTime.Now;
                        string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MLline);

                        //通知坐席已经网友已离线
                        SendMessage(cAgent.PrivateToken, messagetype, chatMessage);
                    }

                    //如果网友在线，并且非转移时，通知网友坐席已离线。

                    if (cNowWY.Status == AgentStatus.Online && cNowWY.IsTransfering == 0)
                    {
                        ChatMessage chatMessageWy = new ChatMessage();
                        chatMessageWy.From = cAgent.PrivateToken;
                        chatMessageWy.Message = string.Format("坐席{0}已离线.", cAgent.PrivateToken);
                        chatMessageWy.Time = DateTime.Now;
                        string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MLline);

                        //通知网友坐席已经离线
                        SendMessage(e.CurrentUserId, messagetype, chatMessageWy);
                        //cNowWY.Status = AgentStatus.Leaveline;
                    }



                    //数据库更新会话表
                    Entities.Conversations csEntity = new Conversations()
                    {
                        CSID = cNowWY.CSId,
                        VisitID = cNowWY.Userloginfo.VisitID,
                        AgentStartTime = cNowWY.ConverSTime,
                        LastClientTime = cNowWY.LastMessageTime,
                        EndTime = DateTime.Now,
                        IsTurenOut = cNowWY.IsTurnOut
                    };
                    cNowWY.CSId = 0;
                    //BLL.Conversations.Instance.CallBackUpdate(csEntity);
                    //更新数据库改为异步
                    ThreadPool.QueueUserWorkItem(UpdateConversation, csEntity);

                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Info(string.Format("OnTalkUserRemoved 方法中报错:{0} ", ex.Message));
                }

            });

            #endregion

        }

        //usertype 1坐席，0：网友
        public CometClient InitializeClient_new(string publicToken, string privateToken, string displayName, int connectionTimeoutSeconds, int connectionIdleSeconds, int sendmessageIdleSeconds, int usertype, string strSourceType, string strLoginID, string Posturl, string Title, string ProvinceID, string CityID, string IsReInit, out string msg)
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
            cometClient.LastActivity = DateTime.Now;
            cometClient.PrivateToken = privateToken;
            cometClient.PublicToken = publicToken;
            //cometClient.UserReferURL = UserReferURL;
            cometClient.Type = usertype;

            if (usertype == 1)
            {
                //如果是坐席的的话,坐席默认为离线，初始化坐席增删用户事件
                cometClient.AgentID = privateToken;
                cometClient.Status = AgentStatus.Leaveline;
                if (IsReInit != "1")
                {
                    cometClient.RecordAgentStatus((int)AgentStatus.Leaveline); //记录状态
                }
                cometClient.MaxDialogCount = Convert.ToInt32(ConfigurationManager.AppSettings["MaxDialogueN"]);

                #region 获取客户最大会话量，排队量信息
                //QueryEmployeeAgent query = new QueryEmployeeAgent();
                //query.UserID = Convert.ToInt32(privateToken);
                //int count = 0, nMaxDialogCount = 0, nMaxQueueN = 0;

                //if (IsBatchDebug)
                //{
                //    cometClient.AgentNum = privateToken;
                //    cometClient.BusinessLines = "2";
                //    cometClient.AgentID = privateToken;
                //}
                //else
                //{
                // 调用CC接口获取客服最大等待量，BGID 等信息
                EmployeeAgent agent = CCWebServiceHepler.Instance.GetEmployeeAgent(Convert.ToInt32(privateToken));
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
                //}

                //cometClient.Status= AgentStatus.Leaveline;
                /*
                 //不在从数据库中读取最大会话量
                DataTable dt = BitAuto.DSC.IM_2015.BLL.EmployeeAgent.Instance.GetEmployeeAgent(query, string.Empty, 1, 1, out count);
                if (count > 0 && dt.Rows.Count > 0)
                {
                    int.TryParse(dt.Rows[0]["MaxDialogueN"].ToString(), out nMaxDialogCount);
                }
                 dt = null;
                */


                //cometClient.AgentNum = BLL.EmployeeAgent.Instance.GetAgentNum(publicToken);

                #endregion
                InitCometClientEvent(cometClient);
            }
            else
            {
                //如果是网友，默认为在线
                cometClient.Status = AgentStatus.Online;
                cometClient.SendMessageIdleSeconds = sendmessageIdleSeconds;


                //TODO 志强实现获取访问记录，并返回LoginID作为PrivateToken

                cometClient.Userloginfo = BLL.UserVisitLog.Instance.GetUserInfo(Title, Posturl, strSourceType, strLoginID, CityID, ProvinceID);
                cometClient.BusinessLines = cometClient.Userloginfo.SourceType;
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

                cometClient.PrivateToken = Guid.NewGuid().ToString();

                //判断是否已存在网友，如果存在清除，用于网友刷新页面
                CometClient cometmodelhave = GetCometClient(cometClient.PrivateToken);
                if (cometmodelhave != null)
                {
                    msg = "exists";
                    return null;
                }
                else
                {
                    var st = new ServeTime(9, 0);
                    var et = new ServeTime(18, 0);
                    BLL.BaseData.Instance.ReadTime(out st, out et);
                    DateTime logintime = System.DateTime.Now;
                    //判断是否是工作时间
                    if (logintime.Hour < st.Hour || logintime.Hour > et.Hour || (logintime.Hour == st.Hour && logintime.Minute < st.Min) || (logintime.Hour == et.Hour && logintime.Minute > et.Min))
                    {
                        msg = "servicetimeout";
                        return cometClient;
                    }
                }


            }



            this.stateProvider.InitializeClient(cometClient);
            //  ok, fire the event
            //DateTime dtP = DateTime.Now;
            //BLL.Loger.Log4Net.Info(string.Format("初始化线程ID:{0},初始化用户:{1},分配用户前时间：{2}", Thread.CurrentThread.ManagedThreadId, privateToken, (DateTime.Now - dtT)));

            this.FireClientInitialized(cometClient);

            //BLL.Loger.Log4Net.Info(string.Format("初始化线程ID:{0},初始化用户:{1},分配用户时间:{2},初始化总时间：{3}", Thread.CurrentThread.ManagedThreadId, privateToken, DateTime.Now - dtP, DateTime.Now - dtT));
            return cometClient;
        }


        private static int i = 0;
        public IAsyncResult BeginSubscribe(HttpContext context, AsyncCallback callback, object extraData)
        {
            var result = new CometAsyncResult(context, callback, state);
            long lastMessageId = 0;
            string privateToken = string.Empty;
            try
            {

                if (!long.TryParse(context.Request["lastMessageId"] ?? "-1", out lastMessageId))
                    throw CometException.CometHandlerParametersAreInvalidException();

                privateToken = context.Request["privateToken"];
                //Debug.WriteLine(privateToken);


                //string str = string.Format("BeginSubscribe: 请求进入{0},时间:{1},剩余个数{2},线程ID:{3}", privateToken, DateTime.Now.ToString("O"), Interlocked.Increment(ref i),Thread.CurrentThread.ManagedThreadId);
                //Debug.WriteLine(str);
                //BLL.Loger.Log4Net.Info(str);

                if (string.IsNullOrEmpty(privateToken))
                    throw CometException.CometHandlerParametersAreInvalidException();


                CometClient cometClient = this.GetCometClient(privateToken);


                if (cometClient == null)
                {
                    // throw new Exception("未找到服务端对象");
                    this.WriteErrorToResponse(context, result, "未找到服务端对象");
                    return result;
                }


                //  ok, fire the event
                this.FireClientSubscribed(cometClient);

                this.workerThreads[0].RemoveWaitRequestByToken(privateToken);
                var request = new CometWaitRequest(privateToken, lastMessageId, context, callback, extraData);
                this.workerThreads[0].QueueCometWaitRequest(request);

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
            //this.DebugWriteThreadInfo("EndSubscribe");

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
                        List<Type> knownTypes = new List<Type>();
                        foreach (CometMessage message in messages)
                        {
                            if (message.Contents != null)
                            {
                                Type knownType = message.Contents.GetType();

                                if (!knownTypes.Contains(knownType))
                                {
                                    knownTypes.Add(knownType);
                                }
                                //添加消息处理时间add by qizq 2014-3-6
                                //message.ProcessTime = System.DateTime.Now;
                            }
                        }

                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(messages.GetType(), knownTypes);
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

        /// <summary>
        /// Send a message to a specific client
        /// </summary>
        /// <param name="clientPublicToken">The public token of the client</param>
        /// <param name="name">The name of the message</param>
        /// <param name="contents">The contents of the message</param>
        /// <param name="sentType">0:系统消息，1：用户发送消息</param>
        public void SendMessage(string clientPublicToken, string name, object contents, int sentType = 0)
        {
            var comet = GetCometClient(clientPublicToken);
            comet.LastMessageTime = DateTime.Now;
            if (sentType == 1 && comet.Type == 2 && !comet.IsAgentReply)
            {
                //更新客服回复时间
                BLL.Conversations.Instance.UpdateConversationReplyTime(DateTime.Now, comet.CSId);
                comet.IsAgentReply = true;
                comet.AgentSTime = DateTime.Now;
            }

            this.stateProvider.SendMessage(clientPublicToken, name, contents);
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
        /// Gets the ICometStateProvider instance this manager consumes
        /// </summary>
        internal ICometStateProvider StateProvider
        {
            get { return this.stateProvider; }
        }

        /// <summary>
        /// Kill an IdleCometClient
        /// </summary>
        /// <param name="clientPrivateToken"></param>
        public void KillIdleCometClient(CometClient cometClient)
        {
            //  get the comet client

            //CometClient cometClient = this.stateProvider.GetCometClient(clientPrivateToken);


            if (cometClient != null && Interlocked.CompareExchange(ref cometClient.IsDeleted, 1, 0) == 0)
            {
                //BLL.Loger.Log4Net.Info(string.Format("StateManager中方法 KillIdleCometClient移除对象：{0}", cometClient.PrivateToken));
                try
                {
                    cometClient.Status = AgentStatus.Leaveline;

                    //如果是网友的话，将此网友从其对应的坐席中删除
                    if (cometClient.Type == (int)Entities.UserType.User && !string.IsNullOrEmpty(cometClient.AgentID))
                    {
                        CometClient agentclient = GetCometClient(cometClient.AgentID);
                        if (agentclient != null)//&& agentclient.Status == Entities.AgentStatus.Online
                        {
                            agentclient.RemoveTalkUser(cometClient.PrivateToken);
                            //agentclient.RemoveWaitUser(cometClient.PrivateToken);
                        }
                    }
                    else if (cometClient.Type == (int)Entities.UserType.Agent)
                    {

                        BLL.Loger.Log4Net.Info(string.Format("5S线程监控 Agent 断网超时移除对象：{0},name:{1},LastRequestTime:{2},当前时间：{3},超时时间：{4},超时时间设置：{5}",
                            cometClient.PrivateToken, cometClient.AgentNum ?? cometClient.DisplayName, cometClient.LastRequestTime.ToString("o"), DateTime.Now.ToString("o"), DateTime.Now.Subtract(cometClient.LastRequestTime).TotalSeconds, cometClient.ConnectionIdleSeconds));

                        ThreadPool.QueueUserWorkItem((obj) =>
                        {
                            int nRecID = Convert.ToInt32(obj);
                            if (nRecID <= 0) { return; }
                            BLL.AgentStatusDetail.Instance.UpdateAgentLastStatus(nRecID);
                        }, cometClient.AgentStatusRecID);

                        ReAllocateOffLineAgentUsers(cometClient);  //坐席移除所有再聊网友
                    }

                    //更新坐席状态变化明细


                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Info(string.Format("KillIdleCometClient中异常：{0}", ex.Message));
                }
                finally
                {
                    //从provider列表中移除该实体
                    this.stateProvider.KillIdleCometClient(cometClient.PrivateToken);
                }

                //  and fire
                //this.FireIdleClientKilled(cometClient);
            }
        }

        /// <summary>
        /// 清楚线程中的request请求对象
        /// </summary>
        /// <param name="clientPrivateToken"></param>
        public void KillWaitRequest(string clientPrivateToken)
        {
            foreach (CometWaitThread item in this.workerThreads.ToArray())
            {
                item.RemoveWaitRequestByToken(clientPrivateToken);
            }
        }

        public CometClient GetCometClient(string clientPrivateToken)
        {
            return this.stateProvider.GetCometClient(clientPrivateToken);
        }

        internal void FireAgentStateOnChanged(Entities.AgentStatusDetail agentState)
        {
            if (this.AgentStateOnChanged != null)
                this.AgentStateOnChanged(this, new AgentStateEventArgs(agentState));
        }

        internal void FireClientInitialized(CometClient cometClient)
        {
            if (this.ClientInitialized != null)
                this.ClientInitialized(this, new CometClientEventArgs(cometClient));
        }

        internal void FireIdleClientKilled(CometClient cometClient)
        {
            if (this.IdleClientKilled != null)
                this.IdleClientKilled(this, new CometClientEventArgs(cometClient));

        }

        internal void FireClientSubscribed(CometClient cometClient)
        {
            if (this.ClientSubscribed != null)
                this.ClientSubscribed(this, new CometClientEventArgs(cometClient));
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

        //给网友分配坐席
        //TODO 无效的方法
        /*
         public CometWaitRequest AssignAgent()
         {
             //遍历所有线程类
             //所有请求集合
             //CometWaitRequest comwaitrequest = null;
             List<CometWaitRequest> requestlist = new List<CometWaitRequest>();
             CometWaitThread[] CometThreadS;

             CometThreadS = workerThreads.ToArray();

             for (int i = 0; i < CometThreadS.Length; i++)
             {
                 CometWaitThread CometThread = CometThreadS[i];

                 CometWaitRequest[] requests;

                 requests = CometThread.WaitRequestsArray;

                 //取每个线程类所对应的客户端请求，随机取一个活的请求，取请求所对应是坐席的，并且是在线的
                 for (int j = 0; j < requests.Length; j++)
                 {
                     CometClient cometClient = this.GetCometClient(requests[j].ClientPrivateToken);
                     //取请求所对应是坐席的，并且是在线的
                     if (cometClient.Type == (Int32)Entities.UserType.Agent && cometClient.Status == Entities.AgentStatus.Online)
                     {
                         //如果坐席的分配上线未达到，就放在分配列表中
                         //if (BLL.AllocationAgent.Instance.SelectCurrentAgentUserCount(cometClient.PrivateToken) >= cometClient.MaxDialogCount)
                         //{
                         //}
                         //if (cometClient.DialogCount >= cometClient.MaxDialogCount)
                         //{
                         //}
                         //else
                         //{
                         //    requestlist.Add(requests[j]);
                         //}
                     }
                 }
             }
             if (requestlist.Count == 0)
             {
                 return null;
             }
             else
             {
                 Random randObj = new Random();
                 int randnum = randObj.Next(0, requestlist.Count - 1);
                 return requestlist[randnum];
             }
         }
         */

        #region 自定义方法
        /// <summary>
        /// 设置坐席状态
        /// </summary>
        /// <param name="clientPrivateToken"></param>
        /// <param name="state"></param>
        public void SetAgentState(string clientPrivateToken, AgentStatus state)
        {
            stateProvider.SetAgentState(clientPrivateToken, state);

            //触发坐席状态变化事件
            var agentState = new Entities.AgentStatusDetail();
            agentState.UserID = clientPrivateToken;
            agentState.Status = (int)state;
            agentState.StartTime = DateTime.Now;
            this.FireAgentStateOnChanged(agentState);
        }
        /// <summary>
        /// 待删除 当网友空闲时间已到，根据网友标识，在与其聊天的坐席的在聊网友里将其移除
        /// </summary>
        /// <param name="clientPrivateToken"></param>
        /*
         public void RemoveDialogComeetByUserId(string clientPrivateToken)
         {
             //stateProvider.RemoveDialogComeetByUserID(clientPrivateToken);
             var comet = GetCometClient(clientPrivateToken);
             comet.Status = AgentStatus.Leaveline;

             //重新分配离线客服下所有的网友
             if (comet.Type == (int)UserType.Agent)
             {
                 ReAllocateOffLineAgentUsers(comet);
             }

         }
         */

        /// <summary>
        /// 重新分配离线客服下所有的网友
        /// </summary>
        /// <param name="privateToken">离席客服ID</param>
        public void ReAllocateOffLineAgentUsers(CometClient cometClient)
        {
            if (cometClient != null)
            {
                //移除在聊网友
                foreach (string wu in cometClient.TalkUserList.ToArray())
                {
                    cometClient.RemoveTalkUser(wu);
                }
            }
        }

        /// <summary>
        /// 移除指定坐席下的网友，在坐席关闭再聊网友时使用
        /// </summary>
        /// <param name="wyToken"></param>
        /// <param name="zxToken"></param>
        public void RemoveSingleUser(string wyToken, string zxToken)
        {
            stateProvider.RemoveSingleUser(wyToken, zxToken);
            KillIdleCometClient(GetCometClient(wyToken));
            KillWaitRequest(wyToken);
        }

        #endregion

        #region 客服分配规则

        /*
        private List<string> GetAgentsByCityGroup(string cityGroup, string memberCode, bool isConvert)
        {
            List<string> lstAll = BitAuto.DSC.IM_2015.BLL.CityGroupAgent.Instance.GetAgentsByCityGroup(cityGroup, memberCode, isConvert);
            CometClient fT = null;

            if (lstAll.Count == 0)
            {
                return new List<string>();
            }

            return lstAll.Where(s =>
              {
                  fT = GetCometClient(s);
                  return fT != null && fT.Status == AgentStatus.Online;
              }).ToList();

        }

        /// <summary>
        /// 分配客服给网友
        /// </summary>
        /// <param name="lstAgents">客服列表</param>
        /// <param name="wyUser">网友</param>
        /// <returns>0:分配新对话；1:排队；2：客服忙</returns>
        private int AllocateAgent(List<string> lstAgents, string wyUser)
        {
            //若找到客服会话量未满，将网友插入到会话队列，返回0
            foreach (var strAgent in lstAgents)
            {
                CometClient Agent = GetCometClient(strAgent);

                if (Agent != null)
                {
                    //坐席离线时跳过
                    if (Agent.Status == AgentStatus.Leaveline)
                    {
                        continue;
                    }
                    if (Agent.TalkUserList.Length < Agent.MaxDialogCount)
                    {
                        Agent.AddTalkUser(wyUser);
                        return 0;
                    }
                }
            }
            //排队
            CometClient minWaitAgent;
            string nWaitAgent = string.Empty;

            int nMin = 1000;

            foreach (var strAgent in lstAgents)
            {
                minWaitAgent = GetCometClient(strAgent);
                if (minWaitAgent != null)
                {
                    //坐席离线时跳过
                    if (minWaitAgent.Status == AgentStatus.Leaveline)
                    {
                        continue;
                    }
                    //排队数目满的排除
                    if (minWaitAgent.WaitUserList.Length >= minWaitAgent.MaxQueueN)
                    { continue; }
                    //取最小排队量的坐席
                    if (nMin > minWaitAgent.WaitUserList.Length)
                    {
                        nMin = minWaitAgent.WaitUserList.Length;
                        nWaitAgent = strAgent;
                    }
                }
            }

            if (nWaitAgent != string.Empty)
            {
                minWaitAgent = GetCometClient(nWaitAgent);
                if (minWaitAgent != null)
                {
                    if (minWaitAgent.WaitUserList.Length < minWaitAgent.MaxQueueN)
                    {
                        minWaitAgent.AddWaitUser(wyUser);
                        return 1;
                    }
                }
            }
            //会话全满，排队数也全满
            return 2;
        }


        private void SendBusyMessage(string sendTo)
        {

            //以系统身份通知网友坐席全忙
            ChatMessage cm = new ChatMessage();
            cm.From = "System";
            cm.Message = "目前没有闲置坐席为您服务,您可以填写留言！";
            string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MAllBussy);
            SendMessage(sendTo.ToString(), messagetype, cm);
        }

        public void MainAllocateAgent4Test(string cityGroup, string memberCode, string wyUser)
        {
            List<string> lstA = new List<string>();
            foreach (var client in stateProvider.GetAllCometClients())
            {
                if (client.Type == (int)UserType.Agent && client.Status == AgentStatus.Online)
                {
                    lstA.Add(client.PrivateToken);
                }
            }
            AllocateAgent(lstA, wyUser);
        }

        public void MainAllocateAgent(string cityGroup, string memberCode, string wyUser)
        {
            //1．	根据城市群取客服
            List<string> lstAgent = GetAgentsByCityGroup(cityGroup, memberCode, false);
            int nResult = -1;

            //	找到客服时，调用分配客服函数AllotAgent
            if (lstAgent.Count > 0)
            {
                nResult = AllocateAgent(lstAgent, wyUser);

                //若返回结果为2，取其他城市群的客服
                if (nResult == 2)
                {
                    lstAgent = GetAgentsByCityGroup(cityGroup, memberCode, true);
                    if (lstAgent.Count > 0)
                    {
                        nResult = AllocateAgent(lstAgent, wyUser);
                        if (nResult == 2)
                        {
                            SendBusyMessage(wyUser);
                        }
                    }
                    else
                    {
                        SendBusyMessage(wyUser);
                    }
                }
            }
            else
            {
                //若未找到客服，取所有客服
                List<CometClient> lstAllClient = this.StateProvider.GetAllCometClients();
                if (lstAllClient.Count > 0)
                {
                    //取坐席
                    nResult = AllocateAgent(lstAllClient.FindAll(obj => obj.Type == (int)UserType.Agent).ConvertAll(new Converter<CometClient, string>(o => o.PrivateToken)), wyUser);
                    if (nResult == 2)
                    {
                        SendBusyMessage(wyUser);
                    }
                }
                else
                {
                    SendBusyMessage(wyUser);
                }
            }
        }

        */
        #endregion


        public List<CometClient> GetAllCometClients()
        {
            return this.StateProvider.GetAllCometClients();
        }


        /// <summary>
        /// 根据业务线获取会话量最小的坐席
        /// </summary>
        /// <param name="strBusinessLine"></param>
        /// <returns></returns>
        public CometClient GetClientByBusinessLine(string strBusinessLine)
        {
            if (string.IsNullOrEmpty(strBusinessLine))
                return null;
            var lstComet = this.GetAllCometClients().Where(w => w.Type == 1 && w.Status == AgentStatus.Online).OrderBy(w => w.TalkUserList.Length).ToList();
            List<CometClient> cls = new List<CometClient>();

            foreach (var comet in lstComet)
            {
                if (comet.TalkUserList.Length < comet.MaxDialogCount && comet.BusinessLines.IndexOf(strBusinessLine, System.StringComparison.Ordinal) > -1)
                {
                    cls.Add(comet);
                }
            }
            if (cls.Count == 0)
            {
                return null;
            }
            int nTalkCount = cls[0].TalkUserList.Length;
            cls = cls.Where(w => w.TalkUserList.Length == nTalkCount).ToList();
            Random rd = new Random();
            int nTmp = rd.Next(0, cls.Count);
            return cls[nTmp];

        }


        public string CheckState(string[] lst)
        {
            if (lst.Length == 0) return "[]";

            List<int> lstNIDS = new List<int>();
            int nT = 0;
            Array.ForEach(lst, s =>
            {
                if (int.TryParse(s, out nT))
                {
                    lstNIDS.Add(nT);
                }
            });

            StringBuilder sbids = new StringBuilder();

            //只检测网友的
            var AllComent = GetAllCometClients().Where(c => c.Type == (Int32)Entities.UserType.User).ToList();
            //if (AllComent.Count == 0) return "[]"; 

            var state = 0;
            foreach (int s in lstNIDS)
            {
                state = 0;
                foreach (CometClient client in AllComent)
                {
                    if (client.CSId == s)
                    {
                        if (client.Status == AgentStatus.Online)
                        {
                            state = 1;
                        }
                        else
                        {
                            state = 0;
                        }
                        break;
                    }
                }
                sbids.Append("{");
                sbids.Append(string.Format("\"csid\":\"{0}\",\"state\":\"{1}\"", s, state));
                sbids.Append("},");
            }
            if (sbids.Length > 0)
            {
                return "[" + sbids.Remove(sbids.Length - 1, 1) + "]";
            }
            return "";
        }


        public void TransferAgent(string strSourceAgent, string strTargetAgent, string strWYID)
        {
            var sourceAgent = GetCometClient(strSourceAgent);
            var targetAgent = GetCometClient(strTargetAgent);
            var wyComet = GetCometClient(strWYID);
            wyComet.IsTurnOut = true;
            wyComet.IsTransfering = 1;
            sourceAgent.RemoveTalkUser(strWYID);
            targetAgent.AddTalkUser(strWYID);
            wyComet.IsTransfering = 0;

            UserActionLog SourceLog = new UserActionLog()
            {
                CreateTime = DateTime.Now,
                CreateUserID = -2,
                IP = string.Empty,
                OperUserType = 1,
                LogInType = 10, //转出
                LogInfo = string.Format(" 从坐席 {0} 转出网友 {1} 给新坐席{2}. ", strSourceAgent, strWYID, strTargetAgent)
            };
            UserActionLog TagertLog = new UserActionLog()
            {
                CreateTime = DateTime.Now,
                CreateUserID = -2,
                IP = string.Empty,
                OperUserType = 1,
                LogInType = 9, //转入
                LogInfo = string.Format(" 新坐席 {0} 从坐席{2}转出网友 {1} 给. ", strTargetAgent, strSourceAgent, strWYID)
            };

            BulkInserUserActionThread.EnQueueActionLogs(SourceLog);
            BulkInserUserActionThread.EnQueueActionLogs(TagertLog);


        }

        public void Dispose()
        {
            stateProvider.Dispose();
            for (int i = 0; i < workerThreadCount; i++)
            {
                workerThreads[i].Dispose();
            }
        }

        /*
        private void PressureTest()
        {
            Parallel.For(0, 600, (i) =>
            {
                CometClient c = new CometClient()
                {
                    AgentID = i.ToString(),
                    AgentName = "AgentName" + i.ToString(),
                    AgentNum = "AgentNum" + i.ToString(),
                    AgentSTime = DateTime.Now,
                    BusinessLines = "1",
                    ConnectionIdleSeconds = 200,
                    ConnectionTimeoutSeconds = 200,
                    PrivateToken = i.ToString()
                };
                this.stateProvider.InitializeClient(c);
            });

            //Debug.WriteLine(string.Format("当前Comet总数目:{0}", this.GetAllCometClients().Count));
        }
        */

        private void SendMsgToClientForPressureTest()
        {
            tmPressTest = new Timer(obj =>
            {
                var allAgents = GetAllCometClients();
                if (allAgents.Count > 0)
                {
                    allAgents = allAgents.Where(s => s.Type == 1).ToList();
                }
                int i = 0;
                var messageType = "ChatMessage";
                foreach (CometClient cometClient in allAgents)
                {
                    var TalkList = cometClient.TalkUserList.ToList();
                    foreach (string s in TalkList)
                    {
                        string str = string.Format("来自坐席：{0}自动发送的消息,参数:{1}:时间{2}", cometClient.AgentNum, ++i, DateTime.Now.ToString("O"));
                        var chatmessage = new ChatMessage()
                        {
                            Message = str,
                            CsID = -100,
                            From = cometClient.AgentID.ToString(),
                            Time = DateTime.Now
                        };
                        //this.stateProvider.SendMessage(s, messageType, str);
                        this.stateProvider.SendMessage(s, messageType, chatmessage);
                    }
                }

                tmPressTest.Change(6000, Timeout.Infinite);
            }, null, 2000, Timeout.Infinite);



        }

        public DataTable GetAllAgentsStatus4Monotor()
        {
            var a = GetAllCometClients();
            var Agents = a.Where(s => s.Type == 1).ToList().OrderBy(s => s.AgentID);


            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("name");
            dt.Columns.Add("talkNum");

            var datas = (from ags in Agents
                         select new
                         {
                             id = ags.AgentID,
                             name = ags.AgentName,
                             talkNum = a.Where(wy => wy.AgentID == ags.AgentID).Count()
                         }).ToList();

            foreach (var data in datas)
            {
                var dr = dt.NewRow();
                dr[0] = data.id;
                dr[1] = data.name;
                dr[2] = data.talkNum;
                dt.Rows.Add(dr);
            }

            return dt;
        }
    }

}
