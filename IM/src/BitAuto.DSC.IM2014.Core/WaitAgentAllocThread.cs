using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BitAuto.DSC.IM2014.Core.Messages;
using BitAuto.Utils.Config;

namespace BitAuto.DSC.IM2014.Core
{
    /// <summary>
    /// 处理坐席分配线程
    /// </summary>
    public class WaitAgentAllocThread
    {
        private object state = new object();
        private List<WaitAgentAlloc> waitagentalloclist = new List<WaitAgentAlloc>();
        private CometStateManager stateManager;

        public List<WaitAgentAlloc> WaitAgentAllocList
        {
            get { return this.waitagentalloclist; }
        }

        public WaitAgentAllocThread(CometStateManager stateManager)
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
                //排队人数达到上限
                if (waitagentalloclist.Count >= MaxQueue)
                {
                    //给网友发送排队人数达到上限的通知
                    ChatMessage chatMessage = new ChatMessage();
                    chatMessage.From = "System";
                    chatMessage.Message = "MaxQueue";
                    string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MaxQueue);
                    stateManager.SendMessage(request.UserID, messagetype, chatMessage);
                }
                else
                {
                    waitagentalloclist.Add(request);
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

                WaitAgentAlloc[] processRequest;

                lock (this.state)
                {
                    processRequest = waitagentalloclist.ToArray();
                }

                //  we have no more wait requests left, so we want exis
                /*if (processRequest.Length == 0)
                    break;*/

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
                        waitagentalloclist.Remove(request);
                    }
                }
                else
                {
                    CometClient args = null;
                    try
                    {
                        args = stateManager.GetCometClient(request.UserID);
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
                                    agentcometclient.addUser(request.UserID);
                                    //分配坐席后把网友坐席对应关系保存下来
                                    Entities.AllocationAgent allocaagentModel = new Entities.AllocationAgent();
                                    allocaagentModel.AgentID = comwaitrequest.ClientPrivateToken;
                                    allocaagentModel.UserID = request.UserID;
                                    allocaagentModel.StartTime = System.DateTime.Now;
                                    allocaagentModel.UserReferURL = args.UserReferURL;
                                    allocaagentModel.QueueStartTime = request.WaitBeginTime;
                                    allocaagentModel.AgentEndTime = Convert.ToDateTime("9999-12-31");
                                    allocaagentModel.UserEndTime = Convert.ToDateTime("9999-12-31");
                                    allocaagentModel.LocalIP = args.LocalIP;
                                    allocaagentModel.Location = args.Location;
                                    allocaagentModel.LocationID = args.LocationID;
                                    long allocid = BLL.AllocationAgent.Instance.Insert(allocaagentModel);

                                    //给网友等待时间赋值
                                    DateTime _entertime = System.DateTime.Now;
                                    DateTime.TryParse(args.EnterTime, out _entertime);
                                    TimeSpan ts = System.DateTime.Now - _entertime;
                                    args.WaitTime = (Int32)ts.TotalSeconds;
                                    args.TalkTime = System.DateTime.Now.ToString();
                                    //给网友的最后发送消息时间赋值
                                    args.SendMessageTime = System.DateTime.Now;
                                    //
                                    UserInitialMsg cmforuser = new UserInitialMsg();
                                    cmforuser.AgentID = comwaitrequest.ClientPrivateToken;
                                    cmforuser.UserID = args.PrivateToken;
                                    cmforuser.UserReferURL = args.UserReferURL;
                                    cmforuser.AllocID = allocid;
                                    cmforuser.LocalIP = args.LocalIP;
                                    cmforuser.Location = args.Location;
                                    cmforuser.WaitTime = args.WaitTime;
                                    cmforuser.EnterTime = args.EnterTime;
                                    cmforuser.TalkTime = System.DateTime.Now.ToString();
                                    string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MAllocAgent);
                                    //以系统身份通知网友坐席为您服务
                                    stateManager.SendMessage(args.PrivateToken, messagetype, cmforuser);
                                    //以系统身份通知坐席为网友服务
                                    stateManager.SendMessage(comwaitrequest.ClientPrivateToken, messagetype, cmforuser);

                                    request.WaitStatus = 1;
                                    request.WaitEndTime = System.DateTime.Now;
                                    //坐席发给网友的欢迎语
                                    //ChatMessage cm = new ChatMessage();
                                    //cm.From = comwaitrequest.ClientPrivateToken;
                                    //cm.Message = "客服代表为您服务，请问有什么可以帮您？";
                                    //string messagetypehello = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MTalk);
                                    //stateManager.SendMessage(args.PrivateToken, messagetypehello, cm);
                                }
                                else
                                {
                                    //通知客户端现在有几位在等待
                                    int number = GetWaitNumber(processRequest, request.UserID);
                                    // 以系统身份通知网友现在有多少人在排队
                                    UserAgentBussyMsg cm = new UserAgentBussyMsg();
                                    cm.UserID = request.UserID;
                                    cm.WaitCount = number;
                                    //cm.From = "System";
                                    //cm.Message = "目前没有闲置坐席为您服务,您可以填写留言！";
                                    string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MAllBussy);
                                    stateManager.SendMessage(request.UserID, messagetype, cm);
                                }
                            }
                            else
                            {
                                if (args != null)
                                {
                                    //通知客户端现在有几位在等待
                                    int number = GetWaitNumber(processRequest, request.UserID);
                                    // 以系统身份通知网友现在有多少人在排队
                                    UserAgentBussyMsg cm = new UserAgentBussyMsg();
                                    cm.UserID = request.UserID;
                                    cm.WaitCount = number;
                                    //cm.From = "System";
                                    //cm.Message = "目前没有闲置坐席为您服务,您可以填写留言！";
                                    string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MAllBussy);
                                    stateManager.SendMessage(request.UserID, messagetype, cm);
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
        internal int GetWaitNumber(WaitAgentAlloc[] processRequest, string userid)
        {
            int number = 1;
            for (int i = 0; i < processRequest.Length; i++)
            {
                WaitAgentAlloc request = processRequest[i];
                if (request.UserID == userid)
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
}
