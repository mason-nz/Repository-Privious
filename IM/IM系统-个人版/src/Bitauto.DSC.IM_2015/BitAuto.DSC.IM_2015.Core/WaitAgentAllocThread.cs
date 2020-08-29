using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using BitAuto.DSC.IM_2015.Core.Messages;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.Utils.Config;
using System.Collections.Concurrent;

namespace BitAuto.DSC.IM_2015.Core
{

    public class WaitAgentAllocThread
    {
        //private static IDictionary<string, List<WaitAgentAlloc>> ListWaitAgents = new Dictionary<string, List<WaitAgentAlloc>>();
        //private static Dictionary<string,string> businessLineDictionary = new Dictionary<string, string>();

        private static Dictionary<string, BusinessWaiteQueueInfo> DicListWaitAgents = new Dictionary<string, BusinessWaiteQueueInfo>();

        private static Thread MainMonitorThread;
        private static int nWYCount = 0;

        private CometStateManager globalManager;
        private static int maxqueue = Convert.ToInt16(ConfigurationUtil.GetAppSettingValue("MaxQueue"));
        private static readonly object _locker = new object();

        /*
        private static BusinessWaiteQueueInfo GetBwInfo(string strBusinessLine)
        {
            lock (_locker)
            {
                foreach (var businessWaiteQueueInfo in ListWaitAgents)
                {
                    if (string.Equals(businessWaiteQueueInfo.Value, strBusinessLine))
                    {
                        return businessWaiteQueueInfo;
                        break;
                    }
                }
            }
            return null;
        }
        */

        public WaitAgentAllocThread(CometStateManager stateManager)
        {
            globalManager = stateManager;
            MainMonitorThread = new Thread(DoQueueWork);
            MainMonitorThread.Start();
            foreach (SourceType type in BitAuto.DSC.IM_2015.BLL.Util.GetAllSourceType(false))
            {
                DicListWaitAgents.Add(type.SourceTypeValue, new BusinessWaiteQueueInfo()
                {
                    Name = type.SourceTypeName,
                    Value = type.SourceTypeValue,
                    QueueDicWaitAgents = new ConcurrentDictionary<string, int>()
                });
            }
        }

        //根据业务线获取所有等待队列中数据
        public List<CometClient> GetCometClientsByBusinessLine(string businessLine)
        {
            if (!DicListWaitAgents.ContainsKey(businessLine))
            {
                return null;
            }
            var bwInfo = DicListWaitAgents[businessLine];// GetBwInfo(BusinessLine);

            if (bwInfo != null)
            {
                List<CometClient> lst = new List<CometClient>();
                var arrayWaiting = bwInfo.QueueDicWaitAgents.ToArray();

                for (int i = 0; i < arrayWaiting.Length; i++)
                {
                    lst.Add(this.globalManager.GetCometClient(arrayWaiting[i].Key));
                }
                return lst;
            }
            return null;
        }

        /// <summary>
        /// 插入等待队列
        /// </summary>
        /// <param name="businessLine"></param>
        /// <param name="agent"></param>
        /// <returns>0:成功;1:参数错误，未找到业务线，2：等待队列已满</returns>
        public int EnQueueWaitAgent(string businessLine, string PrivateToken)
        {
            if (!DicListWaitAgents.ContainsKey(businessLine))
            {
                return 1;
            }
            var bwInfo = DicListWaitAgents[businessLine];
            if (bwInfo == null)
            {
                return 1;
            }

            if (bwInfo.QueueDicWaitAgents.Count >= maxqueue)
            {
                return 2;
            }
            int nIndex = -1;

            //lock (_locker)
            //{
            //    bwInfo.ListWaitAgents.Add(wxLoginID);
            //    //bwInfo.ListWaitAgents.Insert(0,wxLoginID);  //新ID插入到头部，遍历从尾部开始
            //    nIndex = bwInfo.ListWaitAgents.Count;
            //}


            bwInfo.QueueDicWaitAgents.TryAdd(PrivateToken, Interlocked.Increment(ref nWYCount));
            nIndex = bwInfo.QueueDicWaitAgents.Count;

            //BLL.Loger.Log4Net.Info(string.Format("方法：EnQueueWaitAgent：线程中{0},插入用户{1}.", Thread.CurrentThread.ManagedThreadId, PrivateToken));
            // 发送当前位置消息。
            string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MQueueSort);

            var clientWY = globalManager.GetCometClient(PrivateToken);
            if (clientWY != null)
            {
                ChatMessage t = new ChatMessage()
                {
                    CsID = clientWY.CSId, //对于网友端此时CSID无意义。
                    From = PrivateToken,
                    Message = string.Format("目前您前面有{0}个人正在排队，请耐心等待。", nIndex),
                    Time = DateTime.Now
                };
                this.globalManager.SendMessage(clientWY.PrivateToken, messagetype, t);
            }
            return 0;
        }


        private void DoQueueWork()
        {
            while (true)
            {
                //List<string> test = new List<string>(ListWaitAgents.Keys);
                //for (int i = test.Count-1; i >= 0; i--)
                //{
                //    var bwAgent = ListWaitAgents[test[i]];
                //    if (bwAgent != null && !bwAgent.QueueWaitAgents.IsEmpty && bwAgent.Check_SetBusy())
                //    {
                //        ThreadPool.QueueUserWorkItem(ProcessSingleBusinessLineInQueue, bwAgent);
                //    }
                //}
                foreach (BusinessWaiteQueueInfo bwAgent in DicListWaitAgents.Values)
                {
                    if (bwAgent != null && !bwAgent.QueueDicWaitAgents.IsEmpty && bwAgent.Check_SetBusy())
                    {
                        ThreadPool.QueueUserWorkItem(ProcessSingleBusinessLineInQueue, bwAgent);
                    }
                }
                Thread.Sleep(100);
            }

        }

        private void ProcessSingleBusinessLineInQueue(object objPara)
        {
            var bwAgentQueue = objPara as BusinessWaiteQueueInfo;
            if (bwAgentQueue == null)
            {
                return;
            }
            //  如果队列为空时sleep 500ms
            if (bwAgentQueue.QueueDicWaitAgents.IsEmpty)
            {
                Thread.Sleep(500);
                bwAgentQueue.SetIdle();
                return;
            }

            try
            {


                string strPrivateToken = string.Empty;
                CometClient clientWY = null;
                CometClient cometAgent = null;
                bool isNeedNoticePostionChanged = false;
                var listWyPrivateTokens = (from k in bwAgentQueue.QueueDicWaitAgents.ToArray()
                                           orderby k.Value
                                           select k.Key).ToList(); //bwAgentQueue.QueueDicWaitAgents.Keys.ToList();

                var outT = 0; //此值无意义
                for (int i = 0; i < listWyPrivateTokens.Count; i++)
                {
                    cometAgent = null;
                    cometAgent = null;

                    strPrivateToken = listWyPrivateTokens[i];
                    clientWY = this.globalManager.GetCometClient(strPrivateToken);
                    if (clientWY == null || clientWY.Status == AgentStatus.Leaveline)
                    {
                        //bwAgentQueue.ListWaitAgents.Remove(strWYLoginId);
                        if (bwAgentQueue.QueueDicWaitAgents.TryRemove(strPrivateToken, out outT))
                        {
                            isNeedNoticePostionChanged = true;
                            continue;
                        }
                    }

                    //读取可以分配的坐席
                    cometAgent = this.globalManager.GetClientByBusinessLine(clientWY.BusinessLines);
                    if (cometAgent != null)
                    {

                        if (bwAgentQueue.QueueDicWaitAgents.TryRemove(strPrivateToken, out outT))
                        {
                            cometAgent.AddTalkUser(clientWY.PrivateToken);
                            isNeedNoticePostionChanged = true;
                            //BLL.Loger.Log4Net.Info(
                            //    string.Format("方法：ProcessSingleBusinessLine：当前坐席：PrivateToken：{0},添加网友：{1}，线程{2}",
                            //        cometAgent.AgentID, clientWY.PrivateToken, Thread.CurrentThread.ManagedThreadId));
                        }

                    }

                }
                if (isNeedNoticePostionChanged)
                {
                    listWyPrivateTokens = (from k in bwAgentQueue.QueueDicWaitAgents.ToArray()
                                           orderby k.Value
                                           select k.Key).ToList();
                    //listWyPrivateTokens = bwAgentQueue.QueueDicWaitAgents.Keys;

                    //对于队列中的所有网友发送当前位置消息
                    string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType),
                        (int)Entities.MessageType.MQueueSort);

                    for (int i = 0; i < listWyPrivateTokens.Count; i++)
                    {
                        clientWY = this.globalManager.GetCometClient(listWyPrivateTokens[i]);
                        if (clientWY != null)
                        {
                            ChatMessage t = new ChatMessage()
                            {
                                CsID = clientWY.CSId, //对于网友端此时CSID无意义。
                                From = clientWY.PrivateToken,
                                Message = string.Format("目前您前面有{0}个人正在排队，请耐心等待，", i + 1),
                                Time = DateTime.Now
                            };
                            this.globalManager.SendMessage(clientWY.PrivateToken, messagetype, t);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("方法：ProcessSingleBusinessLineInQueue中出严重错：{0}", ex.Message));

            }
            finally
            {
                bwAgentQueue.SetIdle();
            }

            /*
            while (bwAgentQueue.QueueWaitAgents.TryPeek(out strPrivateToken))
            {
                clientWY = null;
                cometAgent = null;

                //线程测试勿动
                //isNeedNoticePostionChanged = true;
                //bwAgentQueue.QueueWaitAgents.TryDequeue(out strWYLoginId);
                //Debug.WriteLine(string.Format("已为网友分配{0},业务线{1},线程ID:{2},业务线剩余个数{3}", strWYLoginId, bwAgentQueue.Name, Thread.CurrentThread.ManagedThreadId, bwAgentQueue.QueueWaitAgents.Count));
                //Thread.Sleep(10);
                //if (Interlocked.Increment(ref inC) % 100 == 0) { break; }
                //continue;

                clientWY = this.globalManager.GetCometClient(strPrivateToken);
                if (clientWY == null || clientWY.Status == AgentStatus.Leaveline)
                {
                    isNeedNoticePostionChanged = true;
                    bwAgentQueue.QueueWaitAgents.TryDequeue(out strPrivateToken);
                    continue;
                }
                cometAgent = this.globalManager.GetClientByBusinessLine(clientWY.BusinessLines);
                if (cometAgent == null)
                {
                    break; // 如果没有取到业务线，说明用户聊天数量已满，直接跳出循环
                }
                else
                {
                    BLL.Loger.Log4Net.Info(string.Format("方法：ProcessSingleBusinessLineInQueue：当前坐席：PrivateToken：{0},添加网友：{1}，线程{2}", cometAgent.AgentID, clientWY.PrivateToken, Thread.CurrentThread.ManagedThreadId));
                    cometAgent.AddTalkUser(clientWY.PrivateToken);
                    isNeedNoticePostionChanged = true;
                    bwAgentQueue.QueueWaitAgents.TryDequeue(out strPrivateToken);
                }
            }
            */





        }

        /*
        private void ProcessSingleBusinessLine(object objPara)
        {
            var bwAgentQueue = objPara as BusinessWaiteQueueInfo;
            if (bwAgentQueue == null || bwAgentQueue.GetListWaiteAgentsCount() == 0)
            {
                if (bwAgentQueue != null) { bwAgentQueue.SetIdle(); }
                return;
            }
            
            //Debug.WriteLine(string.Format("开始执行，Name:{0},TreadID:{1}", bwAgentQueue.Name, Thread.CurrentThread.ManagedThreadId));

            ////线程测试用
            //if (Interlocked.Increment(ref bwAgentQueue.ExecutiveCount) >= 2)
            //{
            //    string str = string.Format("方法ProcessSingleBusinessLine：中出现多次执行同一个方法bug,Name:{0},Val:{1},TreadID:{2}",
            //        bwAgentQueue.Name, bwAgentQueue.ExecutiveCount, Thread.CurrentThread.ManagedThreadId);
            //    BLL.Loger.Log4Net.Info(str);
            //    Debug.WriteLine(str);
            //};

            //Thread.Sleep(80);
            //Debug.WriteLine(string.Format("结束执行，Name:{0},TreadID:{1}", bwAgentQueue.Name, Thread.CurrentThread.ManagedThreadId));
            //Interlocked.Decrement(ref bwAgentQueue.ExecutiveCount);
            //bwAgentQueue.SetIdle();
            //return;
            

            //bwAgentQueue.IsBusy = true;
            //bwAgentQueue.SetBusy();

            CometClient clientWY = null;
            bool isNeedNoticePostionChanged = false;


            var lst4RemovedIDs = new List<string>();
            string strWYLoginId;
            int iTotalCount = bwAgentQueue.GetListWaiteAgentsCount();

            //遍历所有等待网友
            for (int i = 0; i < iTotalCount; i++)
            {
                strWYLoginId = bwAgentQueue.ListWaitAgents[i];
                clientWY = this.globalManager.GetCometClient(strWYLoginId);

                if (clientWY == null || clientWY.Status == AgentStatus.Leaveline)
                {
                    //bwAgentQueue.ListWaitAgents.Remove(strWYLoginId);
                    lst4RemovedIDs.Add(strWYLoginId);
                    isNeedNoticePostionChanged = true;
                    continue;
                }
                //读取可以分配的坐席
                var cometAgent = this.globalManager.GetClientByBusinessLine(clientWY.BusinessLines);
                if (cometAgent != null)
                {
                    BLL.Loger.Log4Net.Info(string.Format("方法：ProcessSingleBusinessLine：当前坐席：PrivateToken：{0},添加网友：{1}，线程{2}", cometAgent.AgentID, clientWY.PrivateToken, Thread.CurrentThread.ManagedThreadId));
                    cometAgent.AddTalkUser(clientWY.PrivateToken);
                    lst4RemovedIDs.Add(strWYLoginId);
                    //bwAgentQueue.ListWaitAgents.Remove(strWYLoginId);
                    isNeedNoticePostionChanged = true;
                }
            }


            //lock (_locker)
            //{
            for (int j = 0; j < lst4RemovedIDs.Count; j++)
            {
                try
                {
                    lock (_locker)
                    {
                        Monitor.Enter(_locker);
                        bwAgentQueue.ListWaitAgents.Remove(lst4RemovedIDs[j]);
                        Monitor.Exit(_locker);
                    }
                    BLL.Loger.Log4Net.Info(string.Format("方法：ProcessSingleBusinessLine：线程中{0},移除用户{1},队列{2},剩余排队数{3}", Thread.CurrentThread.ManagedThreadId, lst4RemovedIDs[j], bwAgentQueue.Name, bwAgentQueue.GetListWaiteAgentsCount()));

                }
                catch
                {
                }
            }
            //}

            if (isNeedNoticePostionChanged)
            {
                //对于队列中的所有网友发送当前位置消息
                string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType),
                    (int)Entities.MessageType.MQueueSort);

                for (int i = 0; i < bwAgentQueue.GetListWaiteAgentsCount(); i++)
                {
                    clientWY = this.globalManager.GetCometClient(bwAgentQueue.ListWaitAgents[i]);
                    if (clientWY != null)
                    {
                        ChatMessage t = new ChatMessage()
                        {
                            CsID = clientWY.CSId, //对于网友端此时CSID无意义。
                            From = clientWY.PrivateToken,
                            Message = string.Format("由于目前用户咨询量较大，您目前排至第{0}位，", i + 1),
                            Time = DateTime.Now
                        };
                        this.globalManager.SendMessage(clientWY.PrivateToken, messagetype, t);
                    }
                }
            }
            bwAgentQueue.SetIdle();// .IsBusy = false;
        }
         */
    }


    public class BusinessWaiteQueueInfo
    {
        //public static readonly object InnerLocker = new object();
        public string Name;
        public string Value;
        //public int ExecutiveCount = 0;
        //最大排队量
        //public int MaxWaiteNum;



        //true:运行中，false：停止
        //0:false,1 true
        private int _isBusy = 0;


        /// <summary>
        /// 判断如果当前状态为不忙时，设置为忙，并且返回true;
        /// </summary>
        /// <returns></returns>
        public bool Check_SetBusy()
        {
            return Interlocked.CompareExchange(ref _isBusy, 1, 0) == 0;
        }


        /// <summary>
        /// 如果当前状态为忙时设置为不忙。
        /// </summary>
        public void SetIdle()
        {
            Interlocked.CompareExchange(ref _isBusy, 0, 1);

        }

        //等待列表
        //public List<string> ListWaitAgents = new List<string>();

        public ConcurrentDictionary<string, int> QueueDicWaitAgents = new ConcurrentDictionary<string, int>();
        //public ConcurrentQueue<string> QueueWaitAgents = new ConcurrentQueue<string>();

        public int GetListWaiteAgentsCount()
        {
            //lock (InnerLocker)
            //{
            //return this.ListWaitAgents.Count;
            //return this.QueueWaitAgents.Count;
            return this.QueueDicWaitAgents.Count;
            //}
        }
    }

    /*
    public class WaitAgentAllocThreadOld
    {
        private object state = new object();
        private IDictionary<string, List<WaitAgentAlloc>> idc = new Dictionary<string, List<WaitAgentAlloc>>();
        private CometStateManager stateManager;

        public IDictionary<string, List<WaitAgentAlloc>> IDC
        {
            get { return this.idc; }
        }

        public WaitAgentAllocThreadOld(CometStateManager stateManager)
        {
            //  get the state manager
            this.stateManager = stateManager;

            Thread t = new Thread(new ThreadStart(QueueCometWaitRequest_WaitCallback));
            t.IsBackground = false;
            t.Start();
        }
        internal void QueueWaitAgentAlloc(WaitAgentAlloc request)
        {
            int MaxQueue = 5000;
            string maxqueue = ConfigurationUtil.GetAppSettingValue("MaxQueue");
            if (!string.IsNullOrEmpty(maxqueue))
            {
                int.TryParse(maxqueue, out MaxQueue);
            }
            lock (this.state)
            {
                List<WaitAgentAlloc> waitagentalloclist = null;
                IDC.TryGetValue(request.SourceType, out waitagentalloclist);
                //排队人数达到上限
                if (waitagentalloclist != null)
                {
                    if (waitagentalloclist.Count >= MaxQueue)
                    {
                        //给网友发送排队人数达到上限的通知
                        //ChatMessage chatMessage = new ChatMessage();
                        //chatMessage.From = "System";
                        //chatMessage.Message = "MaxQueue";
                        ////string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MaxQueue);
                        //stateManager.SendMessage(request.UserID, messagetype, chatMessage);
                    }
                    else
                    {
                        waitagentalloclist.Add(request);
                    }
                }
            }
        }
        private void QueueCometWaitRequest_WaitCallback()
        {
            //  here we are...
            //  in a loop

            while (true)
            {
                //Debug.WriteLine(string.Format("QueueCometWaitRequest_WaitCallback Tick: {0} {1} ", Thread.CurrentThread.IsThreadPoolThread, Thread.CurrentThread.ManagedThreadId));
                foreach (string key in IDC.Keys)
                {
                    WaitAgentAlloc[] processRequest;
                    lock (this.state)
                    {
                        List<WaitAgentAlloc> waitagentalloclist = null;
                        IDC.TryGetValue(key, out waitagentalloclist);
                        processRequest = waitagentalloclist.ToArray();
                    }

                    //  we have no more wait requests left, so we want exis
                   

                    if (processRequest.Length == 0)
                    {
                        //  sleep for this time
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        Thread.Sleep(1000);
                        DealWaitAgentAlloc(processRequest);
                    }
                }
            }
        }
        /// <summary>
        /// 分配坐席给队列里的网友，把分配到坐席或已经离开的网友从队列中移除
        /// </summary>
        internal void DealWaitAgentAlloc(WaitAgentAlloc[] processRequest)
        {

            for (int i = 0; i < processRequest.Length; i++)
            {
                WaitAgentAlloc request = processRequest[i];

                if (request.WaitStatus == 1)
                {
                    lock (this.state)
                    {
                        List<WaitAgentAlloc> waitagentalloclist = null;
                        IDC.TryGetValue(request.SourceType, out waitagentalloclist);
                        waitagentalloclist.Remove(request);
                    }
                }
                else
                {
                    CometClient args = null;
                    try
                    {
                        args = stateManager.GetCometClient(request.LoginID);
                    }
                    catch (Exception ex)
                    {
                    }
                    try
                    {
                        //没找到说明网友关闭了
                        if (args == null)
                        {
                            request.WaitStatus = 1;
                            request.WaitEndTime = System.DateTime.Now;
                        }
                        else
                        {
                            //取一个坐席为网友服务
                            CometWaitRequest comwaitrequest = stateManager.AssignAgent();
                            if (comwaitrequest != null)
                            {
                                if ((i == 0 && request.WaitStatus == 0) || (i > 0 && processRequest[i - 1].WaitStatus == 1))
                                {
                                    //给坐席的在聊人数加1
                                    CometClient agentcometclient = stateManager.GetCometClient(comwaitrequest.ClientPrivateToken);
                                    agentcometclient.DialogCount = agentcometclient.DialogCount + 1;
                                    //给坐席的最后发送消息时间赋值
                                    agentcometclient.SendMessageTime = System.DateTime.Now;
                                    //给坐席的在聊网友加入当前网友
                                    //agentcometclient.addUser(request.UserID);
                                    //分配坐席后把网友坐席对应关系保存下来
                                    //Entities.AllocationAgent allocaagentModel = new Entities.AllocationAgent();
                                    //allocaagentModel.AgentID = comwaitrequest.ClientPrivateToken;
                                    //allocaagentModel.UserID = request.UserID;
                                    //allocaagentModel.StartTime = System.DateTime.Now;
                                    //allocaagentModel.UserReferURL = args.UserReferURL;
                                    //allocaagentModel.QueueStartTime = request.WaitBeginTime;
                                    //allocaagentModel.AgentEndTime = Convert.ToDateTime("9999-12-31");
                                    //allocaagentModel.UserEndTime = Convert.ToDateTime("9999-12-31");
                                    //allocaagentModel.LocalIP = args.LocalIP;
                                    //allocaagentModel.Location = args.Location;
                                    //allocaagentModel.LocationID = args.LocationID;
                                    //long allocid = BLL.AllocationAgent.Instance.Insert(allocaagentModel);

                                    //给网友等待时间赋值
                                    //DateTime _entertime = System.DateTime.Now;
                                    //DateTime.TryParse(args.EnterTime, out _entertime);
                                    //TimeSpan ts = System.DateTime.Now - _entertime;
                                    //args.WaitTime = (Int32)ts.TotalSeconds;
                                    //args.TalkTime = System.DateTime.Now.ToString();
                                    ////给网友的最后发送消息时间赋值
                                    //args.SendMessageTime = System.DateTime.Now;
                                    //
                                    //UserInitialMsg cmforuser = new UserInitialMsg();
                                    //cmforuser.AgentID = comwaitrequest.ClientPrivateToken;
                                    //cmforuser.UserID = args.PrivateToken;
                                    //cmforuser.UserReferURL = args.UserReferURL;
                                    //cmforuser.AllocID = allocid;
                                    //cmforuser.LocalIP = args.LocalIP;
                                    //cmforuser.Location = args.Location;
                                    //cmforuser.WaitTime = args.WaitTime;
                                    //cmforuser.EnterTime = args.EnterTime;
                                    //cmforuser.TalkTime = System.DateTime.Now.ToString();
                                    //string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MAllocAgent);
                                    ////以系统身份通知网友坐席为您服务
                                    //stateManager.SendMessage(args.PrivateToken, messagetype, cmforuser);
                                    ////以系统身份通知坐席为网友服务
                                    //stateManager.SendMessage(comwaitrequest.ClientPrivateToken, messagetype, cmforuser);

                                    request.WaitStatus = 1;
                                    request.WaitEndTime = System.DateTime.Now;
                                }
                                else
                                {
                                    //通知客户端现在有几位在等待
                                    int number = GetWaitNumber(processRequest, request.LoginID);
                                    // 以系统身份通知网友现在有多少人在排队
                                    //UserAgentBussyMsg cm = new UserAgentBussyMsg();
                                    //cm.UserID = request.UserID;
                                    //cm.WaitCount = number;
                                    ////cm.From = "System";
                                    ////cm.Message = "目前没有闲置坐席为您服务,您可以填写留言！";
                                    //string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MAllBussy);
                                    //stateManager.SendMessage(request.UserID, messagetype, cm);
                                }
                            }
                            else
                            {
                                if (args != null)
                                {
                                    //通知客户端现在有几位在等待
                                    int number = GetWaitNumber(processRequest, request.LoginID);
                                    // 以系统身份通知网友现在有多少人在排队
                                    //UserAgentBussyMsg cm = new UserAgentBussyMsg();
                                    //cm.UserID = request.UserID;
                                    //cm.WaitCount = number;
                                    ////cm.From = "System";
                                    ////cm.Message = "目前没有闲置坐席为您服务,您可以填写留言！";
                                    //string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MAllBussy);
                                    //stateManager.SendMessage(request.UserID, messagetype, cm);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

        /// <summary>
        /// 取userid前面的等待人数
        /// </summary>
        /// <param name="processRequest"></param>
        /// <param name="userid"></param>
        internal int GetWaitNumber(WaitAgentAlloc[] processRequest, string loginid)
        {
            int number = 1;
            for (int i = 0; i < processRequest.Length; i++)
            {
                WaitAgentAlloc request = processRequest[i];
                if (request.LoginID == loginid)
                {
                    break;
                }
                else
                {
                    if (request.WaitStatus == 0)
                    {
                        number++;
                    }
                }
            }
            return number;
        }
    }

    */
}
