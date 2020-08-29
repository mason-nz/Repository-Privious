using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using BitAuto.DSC.IM_2015.Core.Messages;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.MainInterface;
using BitAuto.DSC.IM_2015.Core;
using BitAuto.DSC.IM_2015.BLL;
using System.ServiceModel;
using log4net.Repository.Hierarchy;

namespace BitAuto.DSC.IM_2015.MainWindowsService
{
    public partial class MainIMService : ServiceBase
    {
        /// <summary>
        /// 总消息接收池，用于接受所有业务线消息，然后分发到指定业务线消息池中。
        /// </summary>        
        public UserWorkQueue<CometMessage> MainMessageReceiver = null;

        //垃圾清理定时器
        public int RubbishChecker = 60 * 10; //10分钟无效对象移除
        public long nMessageNum = 0;

        public WaitNetFriendsAllocMonitor MainAllocThreadMonitor;
        /// <summary>
        /// 所有坐席
        /// </summary>
        public ConcurrentDictionary<int, ProxyAgentClient> DicAllAgents = new ConcurrentDictionary<int, ProxyAgentClient>();
        /// <summary>
        /// 所有网友清单，包括排队中的，聊天中的
        /// </summary>
        public ConcurrentDictionary<string, ProxyNetFriend> DicAllQAndTNetFrinds = new ConcurrentDictionary<string, ProxyNetFriend>();
        /// <summary>
        /// 所有聊网友
        /// </summary>
        public ConcurrentDictionary<string, ProxyNetFriend> DicAllNetFriends = new ConcurrentDictionary<string, ProxyNetFriend>();

        /// <summary>
        /// 存储每个IIS对应的消息池。键：IP，
        /// </summary>
        public ConcurrentDictionary<string, ConcurrentDictionary<long, CometMessage>> DicIPMessagePool = new ConcurrentDictionary<string, ConcurrentDictionary<long, CometMessage>>();


        public static Dictionary<string, IIsMsgCallBackServices> DicIISServices = new Dictionary<string, IIsMsgCallBackServices>();

        //删除空闲时间超时的网友线程
        private static Thread DeleteAgentAndNetFrindThread;
        //删除过期消息
        private static Thread DeleteHistroyMessageThread;

        public MainIMService()
        {
            InitializeComponent();

            MainAllocThreadMonitor = new WaitNetFriendsAllocMonitor(this);

            MainMessageReceiver = new UserWorkQueue<CometMessage>() { ISSequential = false };

            IMServices.globalManager = this;

            MainMessageReceiver.DoUserWork += new EventHandler<EnqueueEventArgs<CometMessage>>(MainMessageReceiver_DoUserWork);
            //SyncDownMsgTimer = new Timer(obj => SyncDownMsg(), null, 2000, Timeout.Infinite);
            //删除空闲时间超时的网友线程
            DeleteAgentAndNetFrindThread = new Thread(DoDeleteAgentAndNetFrindWork);
            DeleteAgentAndNetFrindThread.Start();
            //删除过期消息
            DeleteHistroyMessageThread = new Thread(DoDeleteHistroyWork);
            DeleteHistroyMessageThread.Start();

        }
        //移除历史消息
        private void DeleteHistroyMessage(object message)
        {
            CometMessage messageDelete = (CometMessage)message;
            if (DicIPMessagePool.ContainsKey(messageDelete.ToToken))
            {
                CometMessage delete = null;
                DicIPMessagePool[messageDelete.ToToken].TryRemove(messageDelete.MessageId, out delete);
                BLL.Loger.Log4Net.Info("服务删除历史消息，消息ID：" + messageDelete.MessageId);
            }
        }
        /// <summary>
        /// 每隔30000毫秒，遍历消息是否需要删除
        /// </summary>
        private void DoDeleteHistroyWork()
        {
            while (true)
            {
                try
                {
                    var messagePoolList = DicIPMessagePool.ToArray();
                    foreach (KeyValuePair<string, ConcurrentDictionary<long, CometMessage>> keyValue in messagePoolList)
                    {
                        var messageList = keyValue.Value.ToArray();
                        foreach (KeyValuePair<long, CometMessage> message in messageList)
                        {

                            //BLL.Loger.Log4Net.Info("遍历历史消息，消息ID：" + message.Value.MessageId + "创建时间" + message.Value.CreateDateTime + ",据当前" + DateTime.Now.Subtract(message.Value.CreateDateTime).TotalSeconds + "秒");
                            //删除两分钟前的消息
                            if (message.Value != null && DateTime.Now.Subtract(message.Value.CreateDateTime).TotalSeconds > 120)
                            {
                                ThreadPool.QueueUserWorkItem(DeleteHistroyMessage, message.Value);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Error(string.Format("方法：MainIMService.DoDeleteHistroyWork中出严重错：{0}", ex.Message));

                }
                Thread.Sleep(30000);
            }
        }



        /// <summary>
        /// 删除网友，给坐席发消息，结束会话
        /// </summary>
        /// <param name="objnetFrind"></param>
        private void DeleteNetFrind(object objnetFrind)
        {

            try
            {

                ProxyNetFriend netFrind = (ProxyNetFriend)objnetFrind;

                if (DicAllNetFriends.ContainsKey(netFrind.Token))
                {
                    ProxyNetFriend removeNetFrind = null;
                    DicAllNetFriends.TryRemove(netFrind.Token, out removeNetFrind);

                    if (DicAllQAndTNetFrinds.ContainsKey(netFrind.Token))
                    {
                        DicAllQAndTNetFrinds.TryRemove(netFrind.Token, out removeNetFrind);
                    }
                    BLL.Loger.Log4Net.Info("网友空闲超时，移除网友" + netFrind.Token);
                }
                //删除对象，同时删除它的消息
                if (DicIPMessagePool.ContainsKey(netFrind.Token))
                {
                    ConcurrentDictionary<long, CometMessage> deleteMessage = null;
                    DicIPMessagePool.TryRemove(netFrind.Token, out deleteMessage);
                    deleteMessage = null;
                    BLL.Loger.Log4Net.Info("网友空闲超时，移除网友" + netFrind.Token + "的消息");
                }
                //判断是否有在聊坐席，且有会话
                if (string.IsNullOrEmpty(netFrind.AgentToken) || netFrind.CSID <= 0)
                {
                    return;
                }

                Entities.Conversations modelConversation = BLL.Conversations.Instance.GetConversations(netFrind.CSID);
                //会话未结束
                if (modelConversation.EndTime != null && modelConversation.EndTime > Convert.ToDateTime("1900-01-01 00:00:00.000"))
                {
                    return;
                }

                modelConversation.EndTime = DateTime.Now;
                modelConversation.CloseType = (int)Entities.CloseType.SystemClose;
                BLL.Conversations.Instance.CallBackUpdate(modelConversation);

                //判断坐席是否在线
                int agentKey = 0;
                int.TryParse(netFrind.AgentToken.Split('@')[0], out agentKey);
                ProxyAgentClient agentClient = DicAllAgents[agentKey];
                if (agentClient == null || agentClient.Status == (int)AgentStatus.Leaveline)
                {
                    return;
                }
                //移除坐席中的在聊网友
                if (agentClient.TalkUserList.ContainsKey(netFrind.Token))
                {
                    ProxyNetFriend pef = null;
                    agentClient.TalkUserList.TryRemove(netFrind.Token, out pef);
                    pef = null;
                }

                ChatMessage chatMessageWy = new ChatMessage();
                chatMessageWy.From = netFrind.Token;
                chatMessageWy.CsID = netFrind.CSID;
                chatMessageWy.Message = "";
                chatMessageWy.Time = DateTime.Now;

                CometMessage cometMessage = new CometMessage()
                {
                    Name = "MLline",
                    Contents = BLL.Util.DataContractObject2Json(chatMessageWy, typeof(ChatMessage)), //JsonConvert.SerializeObject(chatMessageWy),
                    FromToken = netFrind.Token,
                    ToToken = netFrind.AgentToken
                };
                SendMessage(netFrind.AgentToken, cometMessage);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("方法：MainIMService.DeleteNetFrind中出严重错：{0}", ex.Message));
            }

        }
        /// <summary>
        /// 删除坐席，给网友发消息，结束会话
        /// </summary>
        /// <param name="objnetFrind"></param>
        private void DeleteAgent(object objAgent)
        {
            //同接口里的
            //RemoveAgent()
            try
            {
                ProxyAgentClient agent = (ProxyAgentClient)objAgent;
                if (DicAllAgents.ContainsKey(agent.AgentID))
                {
                    ProxyAgentClient deleteAgent = null;
                    DicAllAgents.TryRemove(agent.AgentID, out deleteAgent);
                    if (deleteAgent == null) return;

                    //删除其对应的消息
                    //删除对象，同时删除它的消息
                    if (DicIPMessagePool.ContainsKey(deleteAgent.AgentToken))
                    {
                        ConcurrentDictionary<long, CometMessage> deleteMessage = null;
                        DicIPMessagePool.TryRemove(deleteAgent.AgentToken, out deleteMessage);
                        deleteMessage = null;
                        BLL.Loger.Log4Net.Info("坐席断网超时，移除坐席" + deleteAgent.AgentToken + "的消息");
                    }
                    //


                    BLL.AgentStatusDetail.Instance.UpdateAgentLastStatus(deleteAgent.AgentStatusRecID);
                    BLL.Loger.Log4Net.Info(string.Format("由于坐席失去长连接超时移除坐席{0},最后一次长连接请求是{1}", deleteAgent.AgentID, deleteAgent.LastRequestTime));
                    //移除网友在聊网友。

                    BLL.Loger.Log4Net.Info(string.Format("由于坐席失去长连接超时移除坐席{0}所有在聊网友", deleteAgent.AgentToken));
                    //agent.LastActiveTime = DateTime.Now;
                    string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MLline);

                    //通知所有网友，坐席已离线
                    foreach (ProxyNetFriend netFriend in deleteAgent.TalkUserList.Values.ToArray())
                    {
                        //
                        //ChatMessage chatMessageWy = new ChatMessage();
                        //chatMessageWy.From = deleteAgent.AgentToken;
                        //chatMessageWy.Message = string.Format("坐席{0}已离线.", deleteAgent.AgentID);
                        //chatMessageWy.Time = DateTime.Now;
                        //chatMessageWy.CsID = netFriend.CSID;

                        //CometMessage cometMessage = new CometMessage()
                        //{
                        //    Name = "MLline",
                        //    Contents = BLL.Util.DataContractObject2Json(chatMessageWy, typeof(ChatMessage)),//JsonConvert.SerializeObject(chatMessageWy),
                        //    FromToken = deleteAgent.AgentToken,
                        //    ToToken = netFriend.Token,
                        //    IISIP = netFriend.IISIP
                        //};
                        //BLL.Loger.Log4Net.Info(string.Format("SendLeaveLineMsg  from:{0},to:{1},csid:{2}", deleteAgent.AgentToken, netFriend.Token, netFriend.CSID));
                        //SendMessage(netFriend.IISIP, cometMessage);

                        netFriend.CloseType = (int)Entities.CloseType.SystemClose;
                        //更新对话
                        //数据库更新会话表
                        Entities.Conversations csEntity = new Entities.Conversations()
                        {
                            CSID = netFriend.CSID,
                            VisitID = netFriend.VisitID,
                            AgentStartTime = netFriend.ConverSTime,
                            LastClientTime = netFriend.LastMessageTime,
                            EndTime = DateTime.Now,
                            IsTurenOut = netFriend.IsTurnOut,
                            CloseType = netFriend.CloseType
                        };

                        try
                        {
                            BLL.Loger.Log4Net.Info(string.Format("UpdateConversation更新网友{0}会话状态", netFriend.Token));
                            BLL.Conversations.Instance.CallBackUpdate(csEntity);
                        }
                        catch (Exception ex)
                        {
                            BLL.Loger.Log4Net.Error(string.Format("更新会话记录错误：CSID：{0},错误信息:{1}", csEntity.CSID, ex.Message));
                        }

                        //把网友从会话清单中移除
                        if (DicAllNetFriends.ContainsKey(netFriend.Token))
                        {
                            ProxyNetFriend netFrienddelete;
                            DicAllNetFriends.TryRemove(netFriend.Token, out netFrienddelete);
                            netFrienddelete = null;
                            if (DicAllQAndTNetFrinds.ContainsKey(netFriend.Token))
                            {
                                DicAllQAndTNetFrinds.TryRemove(netFriend.Token, out netFrienddelete);
                                netFrienddelete = null;
                            }
                            //删除对象，同时删除它的消息
                            if (DicIPMessagePool.ContainsKey(netFriend.Token))
                            {
                                ConcurrentDictionary<long, CometMessage> deleteMessage = null;
                                DicIPMessagePool.TryRemove(netFriend.Token, out deleteMessage);
                                deleteMessage = null;
                            }
                        }
                    }
                    deleteAgent.TalkUserList.Clear();
                    deleteAgent = null;
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("方法：MainIMService.DeleteAgent中出严重错：{0}", ex.Message));
            }


        }
        /// <summary>
        /// 每隔30000毫秒，遍历网友是否空闲超时
        /// </summary>
        private void DoDeleteAgentAndNetFrindWork()
        {
            while (true)
            {
                try
                {
                    if (DicAllQAndTNetFrinds.Count == 0 && DicAllAgents.Count == 0)
                    {
                        nMessageNum = 0;
                        DicIPMessagePool.Clear();
                        continue;
                    }

                    var netFrindsList = DicAllNetFriends.ToArray();
                    foreach (KeyValuePair<string, ProxyNetFriend> keyValue in netFrindsList)
                    {
                        ProxyNetFriend netFrind = keyValue.Value;
                        if (netFrind != null && DateTime.Now.Subtract(netFrind.LastActiveTime).TotalSeconds > BLL.Util.GetSendMessageIdleSeconds())
                        {
                            ThreadPool.QueueUserWorkItem(DeleteNetFrind, netFrind);
                        }
                    }


                    var agentList = DicAllAgents.ToArray();
                    foreach (KeyValuePair<int, ProxyAgentClient> keyValue in agentList)
                    {
                        ProxyAgentClient agentClient = keyValue.Value;
                        if (agentClient != null && DateTime.Now.Subtract(agentClient.LastRequestTime).TotalSeconds > BLL.Util.GetConnectionIdleSecondsAgent())
                        {
                            ThreadPool.QueueUserWorkItem(DeleteAgent, agentClient);
                        }
                    }


                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Error(string.Format("方法：MainIMService.DoDeleteAgentAndNetFrindWork中出严重错：{0}", ex.Message));

                }
                Thread.Sleep(30000);
            }
        }


        /// <summary>
        /// [已删除],从指定IP消息队列中取所有消息
        /// </summary>
        private void SyncDownMsg()
        {
            try
            {
                //从指定IP消息队列中取所有消息
                CometMessage messageT;
                //var resultList = new List<CometMessage>();

                foreach (KeyValuePair<string, ConcurrentDictionary<long, CometMessage>> keyValuePair in DicIPMessagePool)
                {
                    //resultList.Clear();
                    if (DicIISServices.ContainsKey(keyValuePair.Key) && !keyValuePair.Value.IsEmpty)
                    {

                        var resultList = keyValuePair.Value.ToArray();
                        //while (keyValuePair.Value.TryDequeue(out messageT))
                        //{
                        //    resultList.Add(messageT);
                        //}
                        try
                        {
                            DicIISServices[keyValuePair.Key].ReceiveMessage(resultList.Select(s => s.Value).ToArray());
                            foreach (KeyValuePair<long, CometMessage> valuePair in resultList)
                            {
                                CometMessage nT = null;
                                DicIPMessagePool[keyValuePair.Key].TryRemove(valuePair.Key, out  nT);
                            }
                            //foreach (CometMessage message in resultList)
                            //{
                            //    (DicIPMessagePool[keyValuePair.Key].Values as  ConcurrentDictionary<long, CometMessage>).TryRemove(message.)
                            //}
                        }
                        catch (Exception ex)
                        {
                            //记录回调失败的IP，然后重新注册
                            DicIISServices.Remove(keyValuePair.Key);
                            BLL.Loger.Log4Net.Error(string.Format("MainWindowsService.CheckRubbishClient中出错,删除key{0},错误信息{1}：", keyValuePair.Key, ex.Message));
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("MainWindowsService.CheckRubbishClient中出错：" + ex.Message));
            }
            //SyncDownMsgTimer.Change(500, Timeout.Infinite);
        }

        /// <summary>
        /// 接收到消息之后，将消息发送到对应的IIS消息池中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMessageReceiver_DoUserWork(object sender, EnqueueEventArgs<CometMessage> e)
        {
            if (e.Item == null) return;
            try
            {
                //如果当前消息列表中无对应IIS，则添加
                if (!DicIPMessagePool.ContainsKey(e.Item.ToToken))
                {
                    DicIPMessagePool.TryAdd(e.Item.ToToken, new ConcurrentDictionary<long, CometMessage>());
                }
                ;
                e.Item.MessageId = Interlocked.Increment(ref this.nMessageNum);
                e.Item.CreateDateTime = DateTime.Now;
                DicIPMessagePool[e.Item.ToToken].TryAdd(e.Item.MessageId, e.Item);// .Enqueue(e.Item);

                if (e.Item.Name == "ChatMessage")
                {
                    if (DicAllNetFriends.ContainsKey(e.Item.FromToken))
                    {
                        DicAllNetFriends[e.Item.FromToken].LastMessageTime = DateTime.Now;

                    }
                    if (DicAllQAndTNetFrinds.ContainsKey(e.Item.FromToken))
                    {
                        DicAllQAndTNetFrinds[e.Item.FromToken].LastMessageTime = DateTime.Now;

                    }
                }

                if (!string.IsNullOrWhiteSpace(e.Item.FromToken))
                {
                    if (DicAllNetFriends.ContainsKey(e.Item.FromToken))
                    {
                        DicAllNetFriends[e.Item.FromToken].LastActiveTime = DateTime.Now;
                        if (DicAllQAndTNetFrinds.ContainsKey(e.Item.FromToken))
                        {
                            DicAllQAndTNetFrinds[e.Item.FromToken].LastActiveTime = DateTime.Now;

                        }
                    }
                    else
                    {
                        int nAgent = 0;
                        if (int.TryParse(e.Item.FromToken, out nAgent))
                        {
                            if (DicAllAgents.ContainsKey(nAgent))
                            {
                                DicAllAgents[nAgent].LastActiveTime = DateTime.Now;
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("MainMessageReceiver_DoUserWork                 {0}", ex.Message));
            }

        }

        public void SendMessage(string IP, CometMessage message)
        {
            if (!DicIPMessagePool.ContainsKey(message.ToToken))
            {
                DicIPMessagePool.TryAdd(message.ToToken, new ConcurrentDictionary<long, CometMessage>());
            }

            message.MessageId = Interlocked.Increment(ref this.nMessageNum);
            message.CreateDateTime = DateTime.Now;
            DicIPMessagePool[message.ToToken].TryAdd(message.MessageId, message);

            //更新最后会话时间
            if (!string.IsNullOrWhiteSpace(message.FromToken))
            {
                if (DicAllNetFriends.ContainsKey(message.FromToken))
                {
                    DicAllNetFriends[message.FromToken].LastActiveTime = DateTime.Now;
                    DicAllNetFriends[message.FromToken].LastMessageTime = DateTime.Now;
                    if (DicAllQAndTNetFrinds.ContainsKey(message.FromToken))
                    {
                        DicAllNetFriends[message.FromToken].LastActiveTime = DateTime.Now;
                        DicAllQAndTNetFrinds[message.FromToken].LastMessageTime = DateTime.Now;

                    }
                }
                else
                {
                    int nAgent = 0;
                    if (int.TryParse(message.FromToken, out nAgent))
                    {
                        if (DicAllAgents.ContainsKey(nAgent))
                        {
                            DicAllAgents[nAgent].LastActiveTime = DateTime.Now;
                        }
                    }
                }

            }
        }

        /// <summary>
        /// 根据业务线获取会话量最小的坐席
        /// </summary>
        /// <param name="strBusinessLine"></param>
        /// <returns></returns>
        public ProxyAgentClient GetProperAgentByBusinessLine(string strBusinessLine)
        {
            if (string.IsNullOrEmpty(strBusinessLine))
                return null;

            var lstAgents = this.DicAllAgents.Values.ToArray().Where(w => w.Status == 1).OrderBy(w => w.TalkUserList.Count).ToList();

            List<ProxyAgentClient> cls = new List<ProxyAgentClient>();

            foreach (var comet in lstAgents)
            {
                if (!string.IsNullOrEmpty(comet.BusinessLines))
                {
                    List<string> businesses = comet.BusinessLines.Split(',').ToList();
                    if (comet.TalkUserList.Count < comet.MaxDialogNum && businesses.Contains(strBusinessLine))
                    {
                        cls.Add(comet);
                    }
                }

            }
            if (cls.Count == 0)
            {
                return null;
            }
            int nTalkCount = cls[0].TalkUserList.Count;
            cls = cls.Where(w => w.TalkUserList.Count == nTalkCount).ToList();

            var rd = new Random();
            int nTmp = rd.Next(0, cls.Count);
            return cls[nTmp];
            //return cls.FirstOrDefault();

        }


        #region 公共方法

        //public void UploadMessage(CometMessage[] messages)
        //{
        //    if (messages == null) return;
        //    for (int i = 0; i < messages.Length; i++)
        //    {
        //        MainMessageReceiver.EnqueueToProcess(messages[i]);
        //    }
        //}


        public string CheckState(int nAgentId, string[] lstTokens)
        {
            if (lstTokens.Length == 0) return "[]";
            StringBuilder sbids = new StringBuilder();
            string strToken, strCsid;
            int nIndex = -1;
            List<string> listPureToken = new List<string>();
            for (int i = 0; i < lstTokens.Length; i++)
            {
                nIndex = lstTokens[i].LastIndexOf("_");
                strCsid = lstTokens[i].Substring(0, nIndex);
                strToken = lstTokens[i].Substring(nIndex + 1);
                listPureToken.Add(strToken);
                sbids.Append("{");
                if (DicAllNetFriends.ContainsKey(strToken))
                {
                    sbids.Append(string.Format("\"csid\":\"{0}\",\"state\":\"{1}\"", strCsid, 1));
                }
                else
                {
                    sbids.Append(string.Format("\"csid\":\"{0}\",\"state\":\"{1}\"", strCsid, 0));
                }
                sbids.Append("},");
            }

            string strResult = string.Empty;
            if (sbids.Length > 0)
            {
                strResult = "[" + sbids.Remove(sbids.Length - 1, 1) + "]";
            }

            //自检坐席的在聊网友
            /*
                        try
                        {
                
                            if (DicAllAgents.ContainsKey(nAgentId) && DicAllAgents[nAgentId].TalkUserList.Count != listPureToken.Count)
                            {
                                var agentClient = DicAllAgents[nAgentId];
                                //移除TackUser中未被移除的token
                                var talkUsers = DicAllAgents[nAgentId].TalkUserList.ToArray();
                                bool isExists = false;
                                ProxyNetFriend net = null;
                                for (int i = 0; i < talkUsers.Length; i++)
                                {
                                    isExists = false;
                                    for (int j = 0; j < listPureToken.Count; j++)
                                    {
                                        if (talkUsers[i].Key == listPureToken[j])
                                        {
                                            isExists = true;
                                            break;
                                        }
                                    }
                                    if (!isExists)
                                    {
                                        DicAllAgents[nAgentId].TalkUserList.TryRemove(talkUsers[i].Key, out net);
                                        net = null;
                                    }
                                }
                            }
                
                        }
                        catch (Exception ex)
                        {
                            BLL.Loger.Log4Net.Info(string.Format("自检坐席的在聊网友时出错:             {0}", ex.Message));
                        }*/
            return strResult;
        }

        #endregion


        protected override void OnStart(string[] args)
        {
            StartService();
        }

        private static ServiceHost host = null;
        public void StartService()
        {
            try
            {
                BLL.Loger.Log4Net.Info("StartService....");
                host = new ServiceHost(typeof(IMServices));
                if (host.State != CommunicationState.Opening)
                    host.Open();
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info(string.Format("StartService error {0}", ex.Message));
            }
        }

        protected override void OnStop()
        {
            if (host != null)
            {
                host.Close();
            }
            BLL.Loger.Log4Net.Info("Service End");
        }
    }
}
