using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Runtime.Serialization.Json;
using BitAuto.DSC.IM2014.Core.Messages;
using BitAuto.DSC.IM_DMS2014.Entities;
using BitAuto.DSC.IM_DMS2014.Core.Messages;
using System.Configuration;
using log4net.Repository.Hierarchy;

namespace BitAuto.DSC.IM_DMS2014.Core
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
        private CometWaitThread[] workerThreads;
        private object state = new object();
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

        private void InitCometClientEvent(CometClient client)
        {
            #region OnTalkUserAdded

            client.OnTalkUserAdded += new EventHandler<UserArgs>((sender, e) =>
            {
                //获取网友信息
                //插入会话表
                CometClient cAgent = sender as CometClient;
                if (cAgent == null || cAgent.Status == AgentStatus.Leaveline) { return; }

                CometClient userClient = GetCometClient(e.CurrentUserId);
                userClient.ConverSTime = DateTime.Now;
                userClient.AgentID = cAgent.PrivateToken;
                //userClient.AllocId = 

                var et = new Conversations()
                {
                    AgentStartTime = DateTime.Now,
                    BGID = BLL.BaseData.Instance.GetAgentBGIDByUserID(Convert.ToInt32(e.AgentId)),
                    CreateTime = DateTime.Now,
                    EndTime = new DateTime(1900, 1, 1),
                    LastClientTime = new DateTime(1900, 1, 1),
                    OrderID = DBNull.Value.ToString(),
                    Status = 0,
                    UserID = Convert.ToInt32(e.AgentId),
                    UserName = cAgent.DisplayName,
                    VisitID = userClient.VisitID
                };

                int nCSId = BLL.Conversations.Instance.Insert(et);
                if (nCSId < 0)
                {
                    throw new Exception(string.Format("插入会话记录失败,AgentID:{0},UserName:{1}", e.AgentId, userClient.DisplayName));
                }

                AllocAgentMessage cmforuser = new AllocAgentMessage()
                {
                    UserId = userClient.PrivateToken,
                    Address = userClient.Address,
                    AgentID = Convert.ToInt32(cAgent.PrivateToken),
                    CityGroupName = userClient.DistrictName,
                    CsID = nCSId,
                    ContractPhone = userClient.ContractPhone,
                    ContractJob = userClient.ContractJob,
                    ContractName = userClient.ContractName,
                    ConverSTime = DateTime.Now,
                    Distribution = userClient.Distribution,
                    LastConBeginTime = userClient.LastConBeginTime,
                    LastMessageTime = userClient.LastMessageTime,
                    MemberCode = userClient.MemberCode,
                    MemberName = userClient.MemberName,
                    Message = string.Empty,
                    Time = DateTime.Now,
                    LoginID = userClient.LoginID,
                    UserReferTitle = userClient.UserReferTitle,
                    AgentNum = cAgent.AgentNum


                };

                userClient.AllocId = nCSId;
                string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MAllocAgent);
                //以系统身份通知网友坐席为您服务
                cAgent.SendMessageTime = DateTime.Now;
                SendMessage(e.CurrentUserId, messagetype, cmforuser);
                SendMessage(e.AgentId, messagetype, cmforuser);
                //BLL.Loger.Log4Net.Info("分配用户成功");

            });


            #endregion



            client.OnTalkUserRemoved += new EventHandler<UserArgs>((sender, e) =>
            {


                //ChatMessage chatMessage = new ChatMessage();
                //chatMessage.From = cNow.PrivateToken;
                //chatMessage.Message = string.Format("坐席{0}已离线.", cNow.PrivateToken);
                //string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MLline);

                ////通知网友坐席已经离线
                //SendMessage(e.CurrentUserId,messagetype,chatMessage);

                //如果坐席在线时，通知坐席，网友已经离线
                CometClient cAgent = sender as CometClient;
                if (cAgent != null && cAgent.Status == AgentStatus.Online)
                {
                    ChatMessage chatMessage = new ChatMessage();
                    chatMessage.CsID = GetCometClient(e.CurrentUserId).AllocId;
                    chatMessage.From = e.CurrentUserId;
                    chatMessage.Message = string.Format("网友{0}已离线.", e.CurrentUserId);
                    chatMessage.Time = DateTime.Now;
                    string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MLline);

                    //通知网友坐席已经离线
                    SendMessage(cAgent.PrivateToken, messagetype, chatMessage);

                    //从等待队列中取出一个用户添加到对话队列中
                    long nUser = client.GetFirstWaitUser();
                    if (nUser != -2)
                    {
                        client.AddTalkUser(nUser);
                        client.RemoveWaitUser(nUser);
                    }
                }

                //如果网友在线，通知网友坐席已离线。
                CometClient cNowWY = GetCometClient(e.CurrentUserId);
                if (cNowWY != null && cNowWY.Status == AgentStatus.Online)
                {
                    ChatMessage chatMessageWy = new ChatMessage();
                    chatMessageWy.From = cAgent.PrivateToken;
                    chatMessageWy.Message = string.Format("坐席{0}已离线.", cAgent.PrivateToken);
                    chatMessageWy.Time = DateTime.Now;
                    string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MLline);

                    //通知网友坐席已经离线
                    SendMessage(e.CurrentUserId, messagetype, chatMessageWy);
                }
                #region 更新会话记录表


                //更新会话表
                Entities.Conversations csEntity = new Conversations()
                {
                    CSID = cNowWY.AllocId,
                    VisitID = cNowWY.VisitID,
                    AgentStartTime = cNowWY.ConverSTime,
                    LastClientTime = cNowWY.LastMessageTime,
                    EndTime = DateTime.Now
                };



                //更新客户信息表 

                Entities.CustomerInfo cif = new CustomerInfo()
                {
                    MemberCode = cNowWY.MemberCode,
                    LastUserID = Convert.ToInt32(cNowWY.AgentID),
                    LastMessageTime = cNowWY.LastMessageTime,
                    LastBeginTime = cNowWY.ConverSTime,
                    Distribution = cNowWY.Distribution
                };

                //BLL.Conversations.Instance.CallBackUpdate(csEntity);
                //BLL.CustomerInfo.Instance.CallBackUpdate(cif);

                //若错误，更新两次数据库
                var nCount = 0;
                while (nCount <= 1)
                {
                    if (UpdateDBonUserRemoved(cif, csEntity))
                    {
                        break;
                    }
                    else
                    {
                        nCount++;
                        Thread.Sleep(100);
                    }
                }


                #endregion


            });

            client.OnWaitUserAdded += new EventHandler<UserArgs>((sender, e) =>
            {
                //给新加user发消息：“当前第几位”
                CometClient WYC = GetCometClient(e.CurrentUserId);
                CometClient AgentC = sender as CometClient;
                WYC.AgentID = AgentC.PrivateToken;
                WYC.AgentNum = AgentC.AgentNum;

                if (AgentC == null || WYC == null) return;

                string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MQueueSort);
                ChatMessage t = new ChatMessage()
                {
                    CsID = WYC.AllocId,
                    From = AgentC.PrivateToken,
                    Message = string.Format("由于目前用户咨询量较大，您目前排至第{0}位，", client.WaitUserList.Length),
                    Time = DateTime.Now
                };
                //SendMessage(waitList[i].ToString(), messagetype, t);
                SendMessage(e.CurrentUserId, messagetype, t);
            });

            #region OnWaitUserRemoved

            client.OnWaitUserRemoved += new EventHandler<UserArgs>((sender, e) =>
            {
                CometClient AgentC = sender as CometClient;
                CometClient WYC = GetCometClient(e.CurrentUserId);
                if (AgentC == null || WYC == null) return;

                var waitList = client.WaitUserList;
                string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MQueueSort);

                //给其余所有等待队列发消息，当前“当前第几位”
                for (int i = 0; i < waitList.Length; i++)
                {
                    ChatMessage t = new ChatMessage()
                    {
                        CsID = WYC.AllocId,
                        From = AgentC.PrivateToken,
                        Message = string.Format("你当前的排队位置是{0} ", i + 1),
                        Time = DateTime.Now
                    };
                    SendMessage(waitList[i].ToString(), messagetype, t);
                }

            });
            #endregion
        }

        private bool UpdateDBonUserRemoved(Entities.CustomerInfo cif, Entities.Conversations csEntity)
        {
            bool isCorrent = false;
            try
            {
                isCorrent = true;
                BLL.Conversations.Instance.CallBackUpdate(csEntity);
                BLL.CustomerInfo.Instance.CallBackUpdate(cif);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info(string.Format("UpdateDBonUserRemoved方法中报错:{0}", ex.Message));
                isCorrent = false;
            }
            return isCorrent;
        }

        //void client_WaitUserAddedEventHandler(object sender, UserArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        /*
        /// <summary>
        /// Creates a CometClient instance that is persisted into the ICometStateProvider instance.  This needs to be
        /// called by the server prior to the client connecting from a client application.
        /// </summary>
        /// <remarks>
        /// This method will typically be used after a login script is executed, either from a standard ASP.NET form
        /// or an Ajax Method etc...
        /// 
        /// The server would validate the user information, if successfull initialize a client in the COMET engine ready
        /// for the client to connect.
        /// </remarks>
        /// <param name="publicToken">The public token of the client, this token is used to identify the client to other clients</param>
        /// <param name="privateToken">The private token of the client, this token is used to identify the client to itself</param>
        /// <param name="displayName">The display name of the client, can be used to hold a friendly display name of the client</param>
        /// <param name="connectionTimeoutSeconds">The number of seconds the client will be connected to the server for, until it needs to reestablish a connection becuase no messages have been sent</param>
        /// <param name="connectionIdleSeconds">The number of seconds the server will wait for the client to reconnect before it treats it as an idle connection and removes it from the server</param>
        /// <returns>An initialized CometClient object that represents the initialized client</returns>
        public CometClient InitializeClient(string publicToken, string privateToken, string displayName, int connectionTimeoutSeconds, int connectionIdleSeconds, int usertype)
        {
            //  validate the parameters
            if (string.IsNullOrEmpty(publicToken))
                throw new ArgumentNullException("publicToken");

            if (string.IsNullOrEmpty(privateToken))
                throw new ArgumentNullException("privateToken");

            if (string.IsNullOrEmpty(displayName))
                throw new ArgumentNullException("displayName");

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
            cometClient.Type = usertype;
            if (usertype == 1)
            {
                //如果是坐席的的话，初始化坐席增删用户事件
                InitCometClientEvent(cometClient);
            }
            cometClient.Status = Entities.AgentStatus.Online;

            //获取在聊网友数上限值
            int maxcount = 0;
            //maxcount = BLL.AgentInfo.Instance.GetAgentMaxDialogCount(privateToken);
            cometClient.MaxDialogCount = maxcount;
            //  send this to the state provider
            this.stateProvider.InitializeClient(cometClient);

            //  ok, fire the event
            this.FireClientInitialized(cometClient);

            //触发坐席状态保存事件
            Entities.AgentStatusDetail agentState = new Entities.AgentStatusDetail();
            agentState.AgentStatus = Entities.AgentStatus.Online;
            agentState.AgentID = privateToken;
            agentState.CreateTime = DateTime.Now;
            this.FireAgentStateOnChanged(agentState);

            return cometClient;
        }
        */
        /// <summary>
        /// Creates a CometClient instance that is persisted into the ICometStateProvider instance.  This needs to be
        /// called by the server prior to the client connecting from a client application.
        /// </summary>
        /// <remarks>
        /// This method will typically be used after a login script is executed, either from a standard ASP.NET form
        /// or an Ajax Method etc...
        /// 
        /// The server would validate the user information, if successfull initialize a client in the COMET engine ready
        /// for the client to connect.
        /// </remarks>
        /// <param name="publicToken">The public token of the client, this token is used to identify the client to other clients</param>
        /// <param name="privateToken">The private token of the client, this token is used to identify the client to itself</param>
        /// <param name="displayName">The display name of the client, can be used to hold a friendly display name of the client</param>
        /// <param name="connectionTimeoutSeconds">The number of seconds the client will be connected to the server for, until it needs to reestablish a connection becuase no messages have been sent</param>
        /// <param name="connectionIdleSeconds">The number of seconds the server will wait for the client to reconnect before it treats it as an idle connection and removes it from the server</param>
        /// <param name="UserReferURL">网友涞源</param>
        /// <returns>An initialized CometClient object that represents the initialized client</returns>
        public CometClient InitializeClient(string publicToken, string privateToken, string displayName, int connectionTimeoutSeconds, int connectionIdleSeconds, int sendmessageIdleSeconds, string UserReferURL, int usertype, string Eptitle, string Epkey, string EPPostURL, out string msg)
        {
            //DateTime dtT = DateTime.Now;
            //BLL.Loger.Log4Net.Info(string.Format("初始化线程ID:{0},初始化用户:{1},初始化时间：{2}", Thread.CurrentThread.ManagedThreadId, privateToken, dtT.ToString("O")));

            msg = string.Empty;
            //  validate the parameters
            //if (string.IsNullOrEmpty(publicToken))
            //    throw new ArgumentNullException("publicToken");

            //if (string.IsNullOrEmpty(privateToken))
            //    throw new ArgumentNullException("privateToken");

            //if (string.IsNullOrEmpty(displayName))
            //    throw new ArgumentNullException("displayName");

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
                cometClient.Status = AgentStatus.Leaveline;

                #region 获取客户最大会话量，排队量信息
                QueryEmployeeAgent query = new QueryEmployeeAgent();
                query.UserID = Convert.ToInt32(privateToken);


                int count = 0, nMaxDialogCount = 0, nMaxQueueN = 0;
                DataTable dt = BitAuto.DSC.IM_DMS2014.BLL.EmployeeAgent.Instance.GetEmployeeAgent(query, string.Empty, 1, 1, out count);
                if (count > 0 && dt.Rows.Count > 0)
                {
                    int.TryParse(dt.Rows[0]["MaxDialogueN"].ToString(), out nMaxDialogCount);
                    int.TryParse(dt.Rows[0]["MaxQueueN"].ToString(), out nMaxQueueN);
                }
                cometClient.MaxDialogCount = nMaxDialogCount == 0 ? Convert.ToInt32(ConfigurationManager.AppSettings["MaxDialogueN"]) : nMaxDialogCount;
                cometClient.MaxQueueN = nMaxQueueN == 0 ? Convert.ToInt32(ConfigurationManager.AppSettings["MaxQueue"]) : nMaxQueueN;
                cometClient.AgentNum = BLL.EmployeeAgent.Instance.GetAgentNum(publicToken);
                dt = null;
                #endregion
                InitCometClientEvent(cometClient);
            }
            else
            {
                //如果是网友，默认为在线
                cometClient.Status = AgentStatus.Online;
                cometClient.SendMessageIdleSeconds = sendmessageIdleSeconds;

                //测试先写死，联调在用ep验证

                //调用强斐EP初始化方法,验证
                if (!string.IsNullOrEmpty(UserReferURL) && !string.IsNullOrEmpty(EPPostURL))
                {

                    CometClient EpcometClient = EPDMSMember.GetDMSMemberByUrl(Eptitle, new System.Uri(UserReferURL), new System.Uri(EPPostURL), Epkey);
                    if (EpcometClient != null)
                    {
                        cometClient.VisitID = EpcometClient.VisitID;
                        cometClient.LoginID = EpcometClient.LoginID;
                        cometClient.MemberCode = EpcometClient.MemberCode;
                        cometClient.MemberName = EpcometClient.MemberName;
                        cometClient.UserReferTitle = EpcometClient.UserReferTitle;
                        cometClient.UserReferUrl = EpcometClient.UserReferUrl;
                        cometClient.VisitRefer = EpcometClient.VisitRefer;
                        cometClient.ContractName = EpcometClient.ContractName;
                        cometClient.ContractJob = EpcometClient.ContractJob;
                        cometClient.ContractPhone = EpcometClient.ContractPhone;
                        cometClient.ContractEmail = EpcometClient.ContractEmail;
                        cometClient.PublicToken = EpcometClient.LoginID.ToString();
                        cometClient.PrivateToken = EpcometClient.LoginID.ToString();
                        cometClient.DisplayName = EpcometClient.LoginID.ToString();
                        cometClient.Address = EpcometClient.ProvinceName + " " + EpcometClient.CityName + " " + EpcometClient.CountyName; //EpcometClient.Address;
                        cometClient.CityGroupId = EpcometClient.CityGroupId;
                        cometClient.CityGroupName = EpcometClient.CityGroupName;
                        cometClient.LastMessageTime = EpcometClient.LastMessageTime;
                        cometClient.LastConBeginTime = EpcometClient.LastConBeginTime;
                        cometClient.Distribution = EpcometClient.Distribution;
                        cometClient.DistrictId = EpcometClient.DistrictId;
                        cometClient.DistrictName = EpcometClient.DistrictName;
                    }
                    else
                    {
                        return null;
                    }
                    //cometClient.PublicToken = "123456";
                    //cometClient.PrivateToken = cometClient.PublicToken;
                    //cometClient.DisplayName = cometClient.PublicToken;
                    //cometClient.MemberName = "MemberName" + cometClient.PublicToken.ToString();
                    //cometClient.CityGroupId = "12";
                    //cometClient.LoginID = Convert.ToInt64(cometClient.PublicToken);
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
                else
                {
                    return null;
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

        public CometClient InitializeClient_new(string publicToken, string privateToken, string displayName, int connectionTimeoutSeconds, int connectionIdleSeconds, int sendmessageIdleSeconds, string UserReferURL, int usertype, string Eptitle, string Epkey, string EPPostURL, out string msg)
        {
            CometClient client = null;
            msg = string.Empty;
            /*
            var tMsg = string.Empty;
            var evehandler = new AutoResetEvent(false);
            ThreadPool.QueueUserWorkItem(o =>
            {
                client = InitializeClientOrigin(publicToken, privateToken, displayName, connectionTimeoutSeconds,
                     connectionIdleSeconds, sendmessageIdleSeconds, UserReferURL, usertype, Eptitle, Epkey, EPPostURL,
                     out tMsg);

                evehandler.Set();
            });
            evehandler.WaitOne();
            msg = tMsg;
            */
            return client;
        }



        public IAsyncResult BeginSubscribe(HttpContext context, AsyncCallback callback, object extraData)
        {
            var result = new CometAsyncResult(context, callback, state);
            long lastMessageId = 0;
            string privateToken = string.Empty;
            try
            {
                //currentThread++;
                //if (currentThread % 10 == 0)
                //{
                //    throw new Exception("测试");
                //}


                if (!long.TryParse(context.Request["lastMessageId"] ?? "-1", out lastMessageId))
                    throw CometException.CometHandlerParametersAreInvalidException();

                privateToken = context.Request["privateToken"];


                if (string.IsNullOrEmpty(privateToken))
                    throw CometException.CometHandlerParametersAreInvalidException();


                CometClient cometClient = this.GetCometClient(privateToken);
                if (cometClient == null)
                {
                    throw new Exception("未找到服务端对象");
                }

                //  ok, fire the event
                this.FireClientSubscribed(cometClient);

                this.workerThreads[0].RemoveWaitRequestByToken(privateToken);
                var request = new CometWaitRequest(privateToken, lastMessageId, context, callback, extraData);
                this.workerThreads[0].QueueCometWaitRequest(request);
                return request.Result;

                /*
                //  kill the previous one if one exists
                //  from the thread pool
                for (int i = 0; i < this.workerThreadCount; i++)
                {
                    this.workerThreads[i].RemoveWaitRequestByToken(privateToken);
                }

                //  ok, this is our result, so lets queue it
                CometWaitRequest request = new CometWaitRequest(privateToken, lastMessageId, context, callback, extraData);

                //  we have our request so lets queue it on a thread
                this.workerThreads[this.currentThread].QueueCometWaitRequest(request);

                Interlocked.Increment(ref this.currentThread);
                Interlocked.CompareExchange(ref currentThread, 0, this.workerThreadCount);

               
                return request.Result;
                 */

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
                    //BLL.Loger.Log4Net.Info(string.Format("EndSubscribe                 => :{0}      ,DT:{1}", cometAsyncResult.Context.Request["PriveteToken"], DateTime.Now.ToString("O")));
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
        public void SendMessage(string clientPublicToken, string name, object contents)
        {
            var comet = GetCometClient(clientPublicToken);
            comet.LastMessageTime = DateTime.Now;
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
        /// Register the required javascript for the page
        /// </summary>
        /// <param name="page">The page we want to write the scripts to</param>
        public static void RegisterAspNetCometScripts(Page page)
        {
            page.ClientScript.RegisterClientScriptResource(typeof(CometStateManager), "BitAuto.DSC.IM_DMS2014.Core.Scripts.AspNetComet.js");
        }

        /// <summary>
        /// Kill an IdleCometClient
        /// </summary>
        /// <param name="clientPrivateToken"></param>
        public void KillIdleCometClient(string clientPrivateToken)
        {
            //  get the comet client
            CometClient cometClient = this.stateProvider.GetCometClient(clientPrivateToken);

            if (cometClient != null)
            {
                try
                {
                    cometClient.Status = AgentStatus.Leaveline;
                    ;
                    //如果是网友的话，将此网友从其对应的坐席中删除
                    if (cometClient.Type == (int)Entities.UserType.User && !string.IsNullOrEmpty(cometClient.AgentID))
                    {
                        CometClient agentclient = GetCometClient(cometClient.AgentID);
                        if (agentclient != null && agentclient.Status == Entities.AgentStatus.Online)
                        {
                            agentclient.RemoveTalkUser(Convert.ToInt64(cometClient.PrivateToken));
                            agentclient.RemoveWaitUser(Convert.ToInt64(cometClient.PrivateToken));
                        }
                    }
                    else if (cometClient.Type == (int)Entities.UserType.Agent)
                    {
                        ReAllocateOffLineAgentUsers(cometClient);
                    }
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Info(string.Format("KillIdleCometClient中异常：{0}", ex.Message));
                }
                finally
                {
                    //从provider列表中移除该实体
                    this.stateProvider.KillIdleCometClient(clientPrivateToken);
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
            foreach (CometWaitThread item in this.workerThreads)
            {
                item.RemoveWaitRequestByToken(clientPrivateToken);
            }
        }

        public CometClient GetCometClient(string clientPrivateToken)
        {
            return this.stateProvider.GetCometClient(clientPrivateToken);
        }

        internal void DebugWriteThreadInfo(string message)
        {
            int workerAvailable = 0;
            int completionPortAvailable = 0;
            ThreadPool.GetAvailableThreads(out workerAvailable, out completionPortAvailable);

            Debug.WriteLine(string.Format("{0}: {1} {2} out of {3}/{4}", message, Thread.CurrentThread.IsThreadPoolThread, Thread.CurrentThread.ManagedThreadId, workerAvailable, completionPortAvailable));
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
                        if (cometClient.DialogCount >= cometClient.MaxDialogCount)
                        {
                        }
                        else
                        {
                            requestlist.Add(requests[j]);
                        }
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
            agentState.AgentID = clientPrivateToken;
            agentState.AgentStatus = state;
            agentState.CreateTime = DateTime.Now;
            this.FireAgentStateOnChanged(agentState);
        }
        /// <summary>
        /// 待删除 当网友空闲时间已到根据网友标识，在与其聊天的坐席的在聊网友里将其移除
        /// </summary>
        /// <param name="clientPrivateToken"></param>
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

        /// <summary>
        /// 重新分配离线客服下所有的网友
        /// </summary>
        /// <param name="privateToken">离席客服ID</param>
        public void ReAllocateOffLineAgentUsers(CometClient cometClient)
        {
            if (cometClient != null)
            {
                List<long> userList = new List<long>();
                //userList.AddRange(cometClient.RemoveAllTalkUser());
                //cometClient.RemoveAllTalkUser();

                //移除在聊网友
                foreach (long wu in cometClient.TalkUserList)
                {
                    cometClient.RemoveTalkUser(wu);
                }

                //重新分配等待网友
                userList.AddRange(cometClient.RemoveAllWaitUser());
                CometClient ct;

                if (userList.Count > 0)
                {
                    foreach (long u in userList)
                    {
                        ct = GetCometClient(u.ToString());
                        if (ct != null)
                        {
                            //ChatMessage chatMessage = new ChatMessage();
                            //chatMessage.From = cometClient.PrivateToken;
                            //chatMessage.Message = string.Format("坐席{0}已离线.", cometClient.PrivateToken);
                            //chatMessage.Time = DateTime.Now;
                            //string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MLline);
                            //////通知网友坐席已经离线
                            //SendMessage(u.ToString(), messagetype, chatMessage);

                            //重新分配网友
                            MainAllocateAgent(ct.DistrictId, ct.MemberCode, u);
                        }

                    }

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
            //KillIdleCometClient(wyToken);
            //KillWaitRequest(wyToken);

        }

        #endregion

        #region 客服分配规则

        private List<string> GetAgentsByCityGroup(string cityGroup, string memberCode, bool isConvert)
        {
            List<string> lstAll = BitAuto.DSC.IM_DMS2014.BLL.CityGroupAgent.Instance.GetAgentsByCityGroup(cityGroup, memberCode, isConvert);
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
        private int AllocateAgent(List<string> lstAgents, long wyUser)
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


        private void SendBusyMessage(long sendTo)
        {

            //以系统身份通知网友坐席全忙
            ChatMessage cm = new ChatMessage();
            cm.From = "System";
            cm.Message = "目前没有闲置坐席为您服务,您可以填写留言！";
            string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MAllBussy);
            SendMessage(sendTo.ToString(), messagetype, cm);
        }

        public void MainAllocateAgent4Test(string cityGroup, string memberCode, long wyUser)
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

        public void MainAllocateAgent(string cityGroup, string memberCode, long wyUser)
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

        public List<CometClient> GetAllCometClients()
        {
            return this.StateProvider.GetAllCometClients();
        }




        #endregion

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
                    if (client.AllocId == s)
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



        public void Dispose()
        {
            stateProvider.Dispose();
            for (int i = 0; i < workerThreadCount; i++)
            {
                workerThreads[i].Dispose();
            }
        }
    }
}
