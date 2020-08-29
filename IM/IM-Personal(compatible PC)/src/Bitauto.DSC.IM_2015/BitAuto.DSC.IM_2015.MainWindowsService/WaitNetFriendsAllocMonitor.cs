using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
//using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using BitAuto.DSC.IM_2015.Core;
using BitAuto.DSC.IM_2015.Core.Messages;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.BLL;
using BitAuto.DSC.IM_2015.Dal;
using BitAuto.DSC.IM_2015.MainInterface;
using BitAuto.DSC.IM_2015.Messages;
using Newtonsoft.Json;


namespace BitAuto.DSC.IM_2015.MainWindowsService
{

    public class WaitNetFriendsAllocMonitor
    {
        /// <summary>
        /// 记录所有业务线的等待队列，每条业务线都有自己的等待队列
        /// </summary>
        public static Dictionary<string, BusinessWaiteQueueInfo> DicListWaitAgents = new Dictionary<string, BusinessWaiteQueueInfo>();

        private static Thread MainMonitorThread;
        //private static long nWYCount = 0;

        private MainIMService globalManager;
        private static int maxqueue = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["MaxQueue"]);

        //private static readonly object _locker = new object();

        public WaitNetFriendsAllocMonitor(MainIMService stateManager)
        {
            globalManager = stateManager;

            //根据配置文件初始化各个业务线的排队队列
            foreach (SourceType type in BitAuto.DSC.IM_2015.BLL.Util.GetAllSourceType(false))
            {
                DicListWaitAgents.Add(type.SourceTypeValue, new BusinessWaiteQueueInfo()
                {
                    Name = type.SourceTypeName,
                    Value = type.SourceTypeValue,
                    DicWaitNetFriends = new ConcurrentDictionary<long, ProxyNetFriend>()
                });
            }

            MainMonitorThread = new Thread(DoQueueWork);
            MainMonitorThread.Start();

        }

        //根据业务线获取所有等待队列中数据
        public List<ProxyNetFriend> GetCometClientsByBusinessLine(string businessLine)
        {
            if (!DicListWaitAgents.ContainsKey(businessLine))
            {
                return null;
            }
            var bwInfo = DicListWaitAgents[businessLine];// GetBwInfo(BusinessLine);

            if (bwInfo != null)
            {
                return bwInfo.DicWaitNetFriends.Values.ToList();
            }
            return null;
        }

        /// <summary>
        /// 插入等待队列
        /// </summary>
        /// <param name="businessLine"></param>
        /// <param name="agent"></param>
        /// <returns>0:成功;-1:参数错误，未找到业务线，-2：等待队列已满</returns>
        public int EnQueueWaitList(string businessLine, ProxyNetFriend netFriend)
        {
            try
            {
                if (!DicListWaitAgents.ContainsKey(businessLine))
                {
                    return -1;
                }
                var businessLineWaitQueueInfo = DicListWaitAgents[businessLine];
                if (businessLineWaitQueueInfo == null)
                {
                    return -1;
                }

                if (businessLineWaitQueueInfo.DicWaitNetFriends.Count >= maxqueue)
                {
                    return -2;
                }
                int nIndex = -1;


                netFriend.WaitNum = Interlocked.Increment(ref businessLineWaitQueueInfo.WaitNum);
                netFriend.LastActiveTime = DateTime.Now;
                businessLineWaitQueueInfo.DicWaitNetFriends.TryAdd(netFriend.WaitNum, netFriend);
                nIndex = businessLineWaitQueueInfo.DicWaitNetFriends.Count;

                //BLL.Loger.Log4Net.Info(string.Format("方法：EnQueueWaitAgent：线程中{0},插入用户{1}.", Thread.CurrentThread.ManagedThreadId, PrivateToken));
                // 发送当前位置消息。
                string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MQueueSort);

                ChatMessage t = new ChatMessage()
                {
                    CsID = netFriend.CSID, //对于网友端此时CSID无意义。
                    From = netFriend.Token,
                    Message = string.Format("目前您前面有{0}个人正在排队，请耐心等待。", nIndex),
                    Time = DateTime.Now
                };


                CometMessage message = new CometMessage()
                {
                    //Name = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), 2),
                    Name = messagetype,
                    ProcessTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                    Contents = BLL.Util.DataContractObject2Json(t, typeof(ChatMessage)),//JsonConvert.SerializeObject(t),
                    ToToken = netFriend.Token
                };

                this.globalManager.SendMessage(netFriend.IISIP, message);

                return nIndex;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("EnQueueWaitList                 {0}", ex.Message));
                return -1;
            }
        }

        public int RemoveWaitList(string businessLine, string netFriendToken)
        {
            if (!DicListWaitAgents.ContainsKey(businessLine))
            {
                return -1;
            }
            var businessLineWaitQueueInfo = DicListWaitAgents[businessLine];
            if (businessLineWaitQueueInfo == null)
            {
                return -1;
            }
            foreach (ProxyNetFriend nf in businessLineWaitQueueInfo.DicWaitNetFriends.Values.ToArray())
            {
                if (nf.Token == netFriendToken)
                {
                    RemoveFromWaitList(nf);
                    break;
                }
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="netFriend"></param>
        /// <returns>-1:业务线不存在，</returns>
        public int RemoveFromWaitList(ProxyNetFriend netFriend)
        {
            if (!DicListWaitAgents.ContainsKey(netFriend.BusinessLines))
            {
                return -1;
            }
            var businessLineWaitQueueInfo = DicListWaitAgents[netFriend.BusinessLines];

            if (businessLineWaitQueueInfo.DicWaitNetFriends.TryRemove(netFriend.WaitNum, out netFriend))
            {
                ProxyNetFriend pef = null;
                globalManager.DicAllQAndTNetFrinds.TryRemove(netFriend.Token, out pef);
                //其他等待队列中网友收到位置变更消息
                NoticeWaitOrderChanged(businessLineWaitQueueInfo);
            }

            return 0;
        }

        /// <summary>
        /// 每隔100毫秒，让所有业务线分别处理自己等待队列中的数据；使用线程池使各业务线并发执行互不干扰，互不阻塞。
        /// </summary>
        private void DoQueueWork()
        {
            while (true)
            {
                try
                {
                    foreach (BusinessWaiteQueueInfo bwAgent in DicListWaitAgents.Values)
                    {
                        if (bwAgent != null && !bwAgent.DicWaitNetFriends.IsEmpty && bwAgent.Check_SetBusy())
                        {
                            ThreadPool.QueueUserWorkItem(ProcessSingleBusinessLineInQueue, bwAgent);
                        }
                    }
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Error(string.Format("方法：WaitNetFriendsAllocMonitor.DoQueueWork中出严重错：{0}", ex.Message));

                }

                Thread.Sleep(100);
            }

        }


        /// <summary>
        /// 业务线遍历并处理自己等待队列的网友
        /// </summary>
        /// <param name="objPara"></param>
        private void ProcessSingleBusinessLineInQueue(object objPara)
        {
            var businessWaiteNetFriendQueue = objPara as BusinessWaiteQueueInfo;
            if (businessWaiteNetFriendQueue == null)
            {
                return;
            }
            //  如果队列为空时sleep 500ms
            if (businessWaiteNetFriendQueue.DicWaitNetFriends.IsEmpty)
            {
                Thread.Sleep(100);
                businessWaiteNetFriendQueue.SetIdle();
                return;
            }

            try
            {
                string strPrivateToken = string.Empty;
                ProxyNetFriend clientWY = null;
                ProxyAgentClient cometAgent = null;

                //是否要通知其他等待队列中的网友其位置已经变化
                bool isNeedNoticePostionChanged = false;

                //按照队列号排序，此处一定要toArray,否则并发时会报错。
                var listWyPrivateTokens = businessWaiteNetFriendQueue.DicWaitNetFriends.Keys.ToArray().OrderBy(v => v).ToArray();

                long WaitKey = 0;

                for (int i = 0; i < listWyPrivateTokens.Length; i++)
                {
                    cometAgent = null;
                    clientWY = null;
                    WaitKey = listWyPrivateTokens[i];

                    clientWY = businessWaiteNetFriendQueue.DicWaitNetFriends[WaitKey];

                    if (clientWY == null)
                    {
                        if (businessWaiteNetFriendQueue.DicWaitNetFriends.TryRemove(WaitKey, out clientWY))
                        {
                            ProxyNetFriend proxy = null;
                            this.globalManager.DicAllQAndTNetFrinds.TryRemove(clientWY.Token, out proxy);
                            isNeedNoticePostionChanged = true;
                            continue;
                        }
                    }

                    //读取可以分配的坐席
                    cometAgent = this.globalManager.GetProperAgentByBusinessLine(clientWY.BusinessLines);
                    if (cometAgent != null)
                    {
                        if (businessWaiteNetFriendQueue.DicWaitNetFriends.TryRemove(WaitKey, out clientWY))
                        {
                            this.globalManager.DicAllNetFriends.TryAdd(clientWY.Token, clientWY);
                            clientWY.LastActiveTime = DateTime.Now;
                            cometAgent.TalkUserList.TryAdd(clientWY.Token, clientWY);

                            isNeedNoticePostionChanged = true;
                            //BLL.Loger.Log4Net.Info(string.Format("方法：ProcessSingleBusinessLine：当前坐席：PrivateToken：{0},添加网友：{1}", cometAgent.AgentID, clientWY.Token));
                            //添加会话
                            CreateConversation(cometAgent, clientWY, 0);
                            BLL.Loger.Log4Net.Info(string.Format("方法：ProcessSingleBusinessLine：当前坐席：PrivateToken：{0},添加网友：{1}", cometAgent.AgentID, clientWY.Token));
                        }

                    }

                }

                if (isNeedNoticePostionChanged)
                {
                    NoticeWaitOrderChanged(businessWaiteNetFriendQueue);
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("方法：ProcessSingleBusinessLineInQueue中出严重错：{0}", ex.Message));

            }
            finally
            {
                businessWaiteNetFriendQueue.SetIdle();
            }
        }

        /// <summary>
        /// 通知业务线等待队列中各网友最新顺序
        /// </summary>
        /// <param name="businessWaiteNetFriendQueue"></param>
        private void NoticeWaitOrderChanged(BusinessWaiteQueueInfo businessWaiteNetFriendQueue)
        {
            try
            {
                ProxyNetFriend clientWY = null;
                long WaitKey = 0;
                var listWyPrivateTokens = businessWaiteNetFriendQueue.DicWaitNetFriends.Keys.ToArray().OrderBy(v => v).ToArray();

                //对于队列中的所有网友发送当前位置消息
                string strMQueueSort = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MQueueSort);

                for (int i = 0; i < listWyPrivateTokens.Length; i++)
                {
                    clientWY = null;
                    WaitKey = listWyPrivateTokens[i];
                    clientWY = businessWaiteNetFriendQueue.DicWaitNetFriends[WaitKey];

                    if (clientWY != null)
                    {
                        clientWY.LastActiveTime = DateTime.Now;
                        ChatMessage t = new ChatMessage()
                        {
                            CsID = 0, //对于网友端此时CSID无意义。
                            From = clientWY.Token,
                            Message = string.Format("目前您前面有{0}个人正在排队，请耐心等待，", i + 1),
                            Time = DateTime.Now
                        };

                        CometMessage message = new CometMessage()
                        {
                            Name = strMQueueSort,
                            ProcessTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                            Contents = BLL.Util.DataContractObject2Json(t, typeof(ChatMessage)),//JsonConvert.SerializeObject(t),
                            ToToken = clientWY.Token
                        };
                        this.globalManager.SendMessage(clientWY.IISIP, message);
                    }
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("NoticeWaitOrderChanged                 {0}", ex.Message));
            }
        }

        /// <summary>
        /// 网友与坐席创建会话并且发送分配消息/转接消息
        /// </summary>
        /// <param name="Agent"></param>
        /// <param name="netFriend"></param>
        /// <param name="IsTurnOut"></param>
        public void CreateConversation(ProxyAgentClient Agent, ProxyNetFriend netFriend, int IsTurnOut = 0)
        {
            try
            {
                //netFriend.ConverSTime = DateTime.Now;
                netFriend.AgentToken = Agent.AgentToken;
                netFriend.AgentID = Agent.AgentID;
                netFriend.LastMessageTime = DateTime.Now;


                var et = new Entities.Conversations()
                {
                    CreateTime = DateTime.Now,
                    EndTime = new DateTime(1900, 1, 1),
                    LastClientTime = new DateTime(1900, 1, 1),
                    OrderID = DBNull.Value.ToString(),
                    Status = 0,
                    UserID = Convert.ToInt32(Agent.AgentID),
                    UserName = Agent.AgentName,
                    VisitID = netFriend.VisitID,
                    IsTurenIn = (IsTurnOut == 1)     //当网友是被转接过来时，则会话默认为转入
                };

                int nCSId = BLL.Conversations.Instance.Insert(et);
                netFriend.CSID = nCSId;

                if (nCSId < 0)
                {
                    var msg = string.Format("插入会话记录失败,AgentID:{0},UserName:{1}", Agent.AgentID, Agent.AgentName);
                    BLL.Loger.Log4Net.Info(msg);
                    //throw new Exception(msg);
                }


                AllocAgentMessage cmforuser = new AllocAgentMessage()
                {
                    UserId = netFriend.Token,
                    VisitID = netFriend.VisitID.ToString(),
                    contractphone = netFriend.contractphone,
                    AgentID = Agent.AgentID,
                    CsID = nCSId,
                    WYName = netFriend.NetFName,
                    Converstime = DateTime.Now,
                    AgentNum = Agent.AgentNum.ToString(),
                    AgentToken = Agent.AgentID + "@" + Agent.IISIP
                };

                string messagetype = string.Empty;

                if (IsTurnOut == 0)
                {
                    //如果网友是被转接过来的，则发送转接消息
                    messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MAllocAgent);
                    netFriend.IsTurnIn = false;
                    netFriend.IsTurnOut = false;
                    netFriend.CSID = cmforuser.CsID;
                    netFriend.AgentID = cmforuser.AgentID;
                    netFriend.AgentNum = cmforuser.AgentNum;
                    netFriend.AgentToken = cmforuser.AgentToken;
                    netFriend.SendMessageTime = System.DateTime.Now;
                    netFriend.IsAgentReply = false;
                }
                else
                {
                    //如果网友不是被转接过来的，则发送初始化消息
                    messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MTransfer);
                    netFriend.IsTurnIn = true;
                    netFriend.IsTurnOut = false;
                    netFriend.CSID = cmforuser.CsID;
                    netFriend.AgentID = cmforuser.AgentID;
                    netFriend.AgentNum = cmforuser.AgentNum;
                    netFriend.AgentToken = cmforuser.AgentToken;
                    netFriend.SendMessageTime = System.DateTime.Now;
                    netFriend.IsAgentReply = false;
                }

                if (globalManager.DicAllQAndTNetFrinds != null && netFriend != null)
                {
                    if (globalManager.DicAllQAndTNetFrinds.ContainsKey(netFriend.Token))
                    {
                        //ProxyNetFriend newNetfriend = globalManager.DicAllQAndTNetFrinds[netFriend.Token];
                        //newNetfriend = netFriend;
                        globalManager.DicAllQAndTNetFrinds[netFriend.Token].AgentToken = netFriend.AgentToken;
                        globalManager.DicAllQAndTNetFrinds[netFriend.Token].AgentID = netFriend.AgentID;
                        globalManager.DicAllQAndTNetFrinds[netFriend.Token].LastMessageTime = netFriend.LastMessageTime;
                        globalManager.DicAllQAndTNetFrinds[netFriend.Token].LastActiveTime = netFriend.LastActiveTime;
                        globalManager.DicAllQAndTNetFrinds[netFriend.Token].CSID = netFriend.CSID;
                        globalManager.DicAllQAndTNetFrinds[netFriend.Token].IsTurnIn = netFriend.IsTurnIn;
                        globalManager.DicAllQAndTNetFrinds[netFriend.Token].IsTurnOut = netFriend.IsTurnOut;
                        globalManager.DicAllQAndTNetFrinds[netFriend.Token].AgentNum = netFriend.AgentNum;
                        globalManager.DicAllQAndTNetFrinds[netFriend.Token].SendMessageTime = netFriend.SendMessageTime;
                        globalManager.DicAllQAndTNetFrinds[netFriend.Token].IsAgentReply = netFriend.IsAgentReply;
                    }
                }

                var messageT = BLL.Util.DataContractObject2Json(cmforuser, typeof(AllocAgentMessage));//JsonConvert.SerializeObject(cmforuser);



                CometMessage AgentMessage = new CometMessage()
                {
                    Name = messagetype,
                    ProcessTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                    Contents = messageT,
                    ToToken = Agent.AgentToken.ToString()
                };
                CometMessage NetFriendMessage = new CometMessage()
                {
                    Name = messagetype,
                    ProcessTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                    Contents = messageT,
                    ToToken = netFriend.Token
                };

                //给网友坐席分别发消息
                this.globalManager.SendMessage(Agent.IISIP, AgentMessage);
                this.globalManager.SendMessage(netFriend.IISIP, NetFriendMessage);

            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("CreateConversation                 {0}", ex.Message));
            }
        }

    }

}
