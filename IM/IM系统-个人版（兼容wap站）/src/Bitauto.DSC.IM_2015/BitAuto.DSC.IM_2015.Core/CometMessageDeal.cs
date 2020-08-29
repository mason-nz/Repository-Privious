using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.MainInterface;
using BitAuto.DSC.IM_2015.Core.Messages;
using BitAuto.DSC.IM_2015.Messages;
using Newtonsoft.Json;
using System.ServiceModel;

namespace BitAuto.DSC.IM_2015.Core
{
    public class CometMessageDeal
    {
        public static ConcurrentQueue<CometMessage> IISCometMessages = new ConcurrentQueue<CometMessage>();
        private CometStateManager globalManager;
        private List<CometMessage> iisCometMessagesForSync = new List<CometMessage>();  //记录上次没有同步成功的消息

        private static ManualResetEventSlim _manulEventSlim = new ManualResetEventSlim(true);

        //private static readonly object _locker = new object();
        private static long ni = 0;
        private static Timer _syncMessageTimer;
        private IIMServices MainChannel = null;

        private IIMServices MainChannelDelete = null;
        //private static Timer _clearRubbishObjTimer;
        public InstanceContext context = null;

        public static ConcurrentQueue<string> DeleteMessageIDStr = new ConcurrentQueue<string>();
        private List<string> DeleteMessageIDStrList = new List<string>();  //记录上次没有同步成功的消息
        private static ManualResetEventSlim _manulEventSlimForDelete = new ManualResetEventSlim(true);

        //private static readonly object _locker = new object();
        private static long niDelete = 0;
        private static Timer _syncMessageTimerDelete;



        public CometMessageDeal(CometStateManager stateManager)
        {
            globalManager = stateManager;
            //MainChannel = globalManager.GetWCFClient();
            //_syncMessageTimer = new Timer(obj => SyncMessage(false), null, 500, Timeout.Infinite);
            //_clearRubbishObjTimer = new Timer(obj => CheckRubbishWcfClient(), null, 2000, Timeout.Infinite);
            _syncMessageTimer = new Timer(obj =>
            {
                CircleSyncMsg();
                _syncMessageTimer.Dispose();
            }, null, 2000, Timeout.Infinite);

            _syncMessageTimerDelete = new Timer(obj =>
            {
                CircleDeleteMsg();
                _syncMessageTimerDelete.Dispose();
            }, null, 2000, Timeout.Infinite);

        }
        /// <summary>
        /// 五秒执行一次
        /// </summary>
        private void CircleDeleteMsg()
        {
            while (true)
            {
                if (Interlocked.CompareExchange(ref niDelete, 1, 0) == 0)
                {
                    SyncDeleteMessage();
                }
                Thread.Sleep(5000);
            }
        }

        public void EnqueueDelete(string item)
        {
            if (string.IsNullOrEmpty(item)) return;
            if (!DeleteMessageIDStr.Contains(item))
            {
                DeleteMessageIDStr.Enqueue(item);
            }
        }

        public void Enqueue(CometMessage item)
        {
            if (item == null) return;
            IISCometMessages.Enqueue(item);
        }


        public void ForceSyncMessage()
        {
            if (Interlocked.CompareExchange(ref ni, 1, 0) == 0)
            {
                SyncMessage();
            }
            else
            {
                //等待同步完成后再往下操作
                _manulEventSlim.Wait(1000);
            }
        }

        private void CircleSyncMsg()
        {
            int i = 0;
            while (true)
            {
                i++;
                if (i % 100 == 0)
                {
                    i = 0;
                    ThreadPool.QueueUserWorkItem(obj => CheckRubbishWcfClient(), null);
                }
                if (Interlocked.CompareExchange(ref ni, 1, 0) == 0)
                {
                    SyncMessage();
                }
                Thread.Sleep(500);
            }
        }

        private void SyncDeleteMessage()
        {
            Interlocked.Exchange(ref niDelete, 1);
            //设置信号为无，阻塞强制刷新线程
            _manulEventSlimForDelete.Reset();
            try
            {
                if (DeleteMessageIDStrList.Count == 0)
                {
                    string result = null;
                    //每次最多同步100条数据
                    while (DeleteMessageIDStr.TryDequeue(out result) && DeleteMessageIDStrList.Count <= 100)
                    {
                        if (result != null)
                        {
                            DeleteMessageIDStrList.Add(result);
                        }
                    }
                }
                try
                {
                    if (MainChannelDelete == null)
                    {
                        MainChannelDelete = globalManager.GetManualWCFClient();
                    }
                    for (int i = 0; i < DeleteMessageIDStrList.Count; i++)
                    {
                        BLL.Loger.Log4Net.Info("要删除的消息:" + DeleteMessageIDStrList[i]);
                    }
                    MainChannelDelete.DeleteHistroyMessage(DeleteMessageIDStrList.ToArray());

                    //同步成功之后才删除以前的消息，防止消息丢失
                    DeleteMessageIDStrList.Clear();
                }
                catch (Exception ex)
                {
                    if (ex is TimeoutException || ex is CommunicationException)
                    {
                        ICommunicationObject t = MainChannelDelete as ICommunicationObject;
                        if (t != null)
                        {
                            t.Abort();
                        }
                    }
                    MainChannelDelete = null;
                    BLL.Loger.Log4Net.Error(string.Format("CometMessageDeal.SyncDeleteMessage               {0}", ex.Message));

                }
                finally
                {
                    //设置信号为有信号，解除阻塞
                    _manulEventSlimForDelete.Set();
                }

            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info(string.Format("CometMessageDeal.SyncDeleteMessage出错：                {0}", ex.Message));
            }
            _manulEventSlimForDelete.Set();
            Interlocked.Exchange(ref niDelete, 0);
        }

        private void SyncMessage()
        {
            //Stopwatch stw = new Stopwatch();
            //stw.Start();
            //设置运行中标志，防止对此运行，类似锁机制
            Interlocked.Exchange(ref ni, 1);

            //设置信号为无，阻塞强制刷新线程
            _manulEventSlim.Reset();

            CometMessage[] message = null;

            try
            {
                if (iisCometMessagesForSync.Count == 0)
                {
                    CometMessage result = null;
                    //每次最多同步5000条数据
                    while (IISCometMessages.TryDequeue(out result) && iisCometMessagesForSync.Count <= 5000)
                    {
                        if (result != null)
                        {
                            iisCometMessagesForSync.Add(result);
                        }
                    }
                    //BLL.Loger.Log4Net.Info(string.Format("SyncMessage       同步数量{0}", iisCometMessagesForSync.Count));
                }
                //else
                //{
                //    BLL.Loger.Log4Net.Info(string.Format("上次同步失败再次同步SyncMessage       同步数量{0}", iisCometMessagesForSync.Count));
                //}

                try
                {
                    if (MainChannel == null)
                    {
                        MainChannel = globalManager.GetManualWCFClient();
                    }
                    message = MainChannel.SwitchMessage(globalManager.IISIP, iisCometMessagesForSync.ToArray());

                    //同步成功之后才删除以前的消息，防止消息丢失
                    iisCometMessagesForSync.Clear();
                    //关闭连接
                    //(MainChannel as ICommunicationObject).Close();

                    //清空消息池消息
                    globalManager.RemoveDicMessage();

                }
                catch (Exception ex)
                {
                    if (ex is TimeoutException || ex is CommunicationException)
                    {
                        ICommunicationObject t = MainChannel as ICommunicationObject;
                        if (t != null)
                        {
                            t.Abort();
                        }
                    }
                    MainChannel = null;
                    BLL.Loger.Log4Net.Error(string.Format("CometMessageDeal.SyncMessage               {0}", ex.Message));

                }
                finally
                {
                    //设置信号为有信号，解除阻塞
                    _manulEventSlim.Set();
                }

                if (globalManager.isWcfCallBackRegisted == 0)
                {
                    //globalManager.RegisterIIS();
                    //globalManager.isWcfCallBackRegisted = 1;
                    //BLL.Loger.Log4Net.Info(string.Format("SyncMessage中注册IIS成功."));
                }

                MessageDeal(message);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info(string.Format("SyncMessage出错：                {0}", ex.Message));
            }
            _manulEventSlim.Set();
            Interlocked.Exchange(ref ni, 0);

            //BLL.Loger.Log4Net.Info(string.Format("本次同步耗时:{0}", stw.Elapsed.TotalMilliseconds));

            //_syncMessageTimer.Change(500, Timeout.Infinite);

        }

        public void MessageDeal(CometMessage[] messageArray)
        {
            if (messageArray != null)
            {
                for (int i = 0; i < messageArray.Length; i++)
                {
                    ThreadPool.QueueUserWorkItem(msg =>
                    {
                        CometMessage message = msg as CometMessage;

                        var privateToken = message.ToToken;
                        //CometClient cometclient = globalManager.GetCometClient(privateToken);

                        BLL.Loger.Log4Net.Info("同步回来的消息，发给" + privateToken + "消息类型：" + message.Name + ",消息ID" + message.MessageId + ",消息内容：" + message.Contents);


                        switch (message.Name)
                        {
                            case "MAllocAgent":
                                DealMAllocAgentMessage(message);

                                break;
                            case "MLline":
                                DealLeaveLineMessage(message);
                                break;
                            case "MTransfer":
                                DealTransferMessage(message);
                                break;
                            case "ForceAgentLeave":
                                globalManager.ForceAgentLeave(message.ToToken);
                                break;
                            default:
                                ProxyAgentClient agentClient = null;
                                ProxyNetFriend netFriend = null;
                                int Type = 0;
                                globalManager.GetCometClient(privateToken, ref netFriend, ref agentClient, ref Type);

                                //if (netFriend != null)
                                //{
                                //    BLL.Loger.Log4Net.Info("网友：" + privateToken + ",csid:" + netFriend.CSID + ",IsAgentReply：" + netFriend.IsAgentReply.ToString());
                                //}

                                //如果消息接收者是网友，并且坐席初次回复，更新回复时间
                                if (netFriend != null && netFriend.CSID > 0 && !netFriend.IsAgentReply)
                                {
                                    //接收人是网友，并且发送人是坐席，并且坐席初次回复，更新回复时间
                                    if (message.FromToken == netFriend.AgentToken)
                                    {

                                        BLL.Conversations.Instance.UpdateConversationReplyTime(DateTime.Now, netFriend.CSID);
                                        //CometClient.IsAgentReply = true;
                                        //CometClient.AgentSTime = DateTime.Now;

                                        //更新wcf网友实体，坐席首次回复时间
                                        globalManager.UpdateNetFriendAgentReplayTime(privateToken);
                                    }
                                    //
                                }
                                if (netFriend != null || agentClient != null)
                                {
                                    globalManager.AddMessage(privateToken, message);
                                }

                                break;
                        }

                    }, messageArray[i]);
                }

            }
        }

        private void DealMAllocAgentMessage(CometMessage message)
        {
            //BLL.Loger.Log4Net.Info("方法：CometMessageDeal.DealMAllocAgentMessage");
            //CometClient cometclient = globalManager.GetCometClient(message.ToToken);

            ProxyAgentClient agentClient = null;
            ProxyNetFriend netFriend = null;
            int Type = 0;
            globalManager.GetCometClient(message.ToToken, ref netFriend, ref agentClient, ref Type);

            if (agentClient == null && netFriend == null)
            {
                return;
            }
            //if (cometclient == null)
            //{
            //    return;
            //}
            var allocAgentMessage = BLL.Util.DataContractJson2Object<AllocAgentMessage>(message.Contents); //JsonConvert.DeserializeObject(message.Contents, typeof(AllocAgentMessage)) as AllocAgentMessage;
            if (allocAgentMessage == null) { return; }
            if (Type == 1)
            {
                //坐席
                BLL.Loger.Log4Net.Info("从wcf服务同步回来分配消息，消息接收人：" + message.ToToken + ",会话id：" + allocAgentMessage.CsID + ",网友：" + allocAgentMessage.UserId);
                //cometclient.AddTalkUser(allocAgentMessage.UserId);
            }
            //if (cometclient.Type == 1)
            //{
            //    //坐席
            //    BLL.Loger.Log4Net.Info("从wcf服务同步回来分配消息，消息接收人：" + message.ToToken + ",会话id：" + allocAgentMessage.CsID + ",网友：" + allocAgentMessage.UserId);
            //    cometclient.AddTalkUser(allocAgentMessage.UserId);
            //}
            //else if (cometclient.Type == 2)
            //{
            //    cometclient.IsTurnIn = false;
            //    cometclient.IsTurnOut = false;
            //    cometclient.CSId = allocAgentMessage.CsID;
            //    cometclient.AgentID = allocAgentMessage.AgentID.ToString();
            //    cometclient.AgentNum = allocAgentMessage.AgentNum;
            //    cometclient.AgentToken = allocAgentMessage.AgentToken;
            //    cometclient.SendMessageTime = System.DateTime.Now;
            //    cometclient.IsAgentReply = false;
            //}

            globalManager.AddMessage(message.ToToken, message);
        }

        private void DealLeaveLineMessage(CometMessage message)
        {
            try
            {
                //BLL.Loger.Log4Net.Info("方法：CometMessageDeal.DealLeaveLineMessage");

                ProxyAgentClient agentClient = null;
                ProxyNetFriend netFriend = null;
                int Type = 0;
                globalManager.GetCometClient(message.ToToken, ref netFriend, ref agentClient, ref Type);

                if (agentClient == null && netFriend == null)
                {
                    return;
                }
                if (Type == 1)
                {
                    //坐席收到离线消息，直接移除网友
                    if (agentClient.Status == (int)AgentStatus.Online || agentClient.Status == (int)AgentStatus.LeavingForAWhile)
                    {
                        //cometclient.RemoveTalkUser(message.FromToken);
                        globalManager.AddMessage(agentClient.AgentToken, message);
                    }
                }
                //else if (Type == 2)
                //{
                //    //网友收到离线消息，发送离线消息
                //    globalManager.AddMessage(netFriend.Token, message);
                //    globalManager.KillWaitRequest(netFriend.Token);
                //    globalManager.KillIdleCometClient(netFriend.Token, 2);

                //    //删除网友从wcf中

                //}
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("方法：{0},出错：{1}", "CometMessageDeal.DealLeaveLineMessage", ex.Message));
            }
        }
        private void DealTransferMessage(CometMessage message)
        {
            ProxyAgentClient agentClient = null;
            ProxyNetFriend netFriend = null;
            int Type = 0;
            globalManager.GetCometClient(message.ToToken, ref netFriend, ref agentClient, ref Type);

            if (agentClient == null && netFriend == null)
            {
                return;
            }
            var allocAgentMessage = BLL.Util.DataContractJson2Object<AllocAgentMessage>(message.Contents);// JsonConvert.DeserializeObject(message.Contents, typeof(AllocAgentMessage)) as AllocAgentMessage;
            if (allocAgentMessage == null) { return; }


            //if (cometclient.Type == 1)
            //{
            //    //坐席
            //    cometclient.AddTalkUser(allocAgentMessage.UserId);
            //}
            //else if (cometclient.Type == 2)
            //{
            //    cometclient.IsTurnIn = true;
            //    cometclient.IsTurnOut = false;
            //    cometclient.CSId = allocAgentMessage.CsID;
            //    cometclient.AgentID = allocAgentMessage.AgentID.ToString();
            //    cometclient.AgentNum = allocAgentMessage.AgentNum;
            //    cometclient.AgentToken = allocAgentMessage.AgentToken;
            //    cometclient.SendMessageTime = System.DateTime.Now;
            //    cometclient.IsAgentReply = false;
            //}
            globalManager.AddMessage(message.ToToken, message);
        }

        private void CheckRubbishWcfClient()
        {

            //try
            //{
            //    var AllClients = globalManager.GetAllCometClients();
            //    int[] agents4Delete = null;
            //    string[] netFnd4Delete = null;
            //    bool CallbackExist = false;

            //    IIMServices channel = null;
            //    try
            //    {
            //        channel = globalManager.GetManualWCFClient();

            //        channel.SyncObj(AllClients.Where(s => s.Type == 1).Select(s => Convert.ToInt32(s.AgentID)).ToArray(), AllClients.Where(s => s.Type == 2).Select(s => s.PrivateToken).ToArray(),
            //        globalManager.IISIP, out agents4Delete, out netFnd4Delete, out CallbackExist);

            //        (channel as ICommunicationObject).Close();

            //    }
            //    catch (Exception ex)
            //    {
            //        if (ex is TimeoutException || ex is CommunicationException)
            //        {
            //            ICommunicationObject t = channel as ICommunicationObject;
            //            if (t != null)
            //            {
            //                t.Abort();
            //            }
            //        }

            //        BLL.Loger.Log4Net.Error(string.Format("CometMessageDeal.CheckRubbishWcfClient               {0}", ex.Message));
            //        return;
            //    }

            //    if (!CallbackExist)
            //    {
            //        //如果在WCF中客户端回调消失，则重新建立回调
            //        //Interlocked.Exchange(ref globalManager.isWcfCallBackRegisted, 0);
            //    }

            //    if (agents4Delete != null)
            //    {
            //        foreach (int i in agents4Delete)
            //        {
            //            foreach (CometClient agent in globalManager.GetAllCometClients())
            //            {
            //                if (agent.AgentID == i.ToString() && (DateTime.Now - agent.LastRequestTime).TotalSeconds > 20)
            //                {
            //                    //globalManager.SendLeaveLineMsg(agent.PrivateToken, agent.PrivateToken, "对象不存在", agent.IPIISAgent, 0);

            //                }
            //            }
            //        }
            //    }
            //    //if (netFnd4Delete != null)
            //    //{
            //    //    foreach (var netFnd in netFnd4Delete)
            //    //    {
            //    //        var comet = globalManager.GetCometClient(netFnd);
            //    //        if (comet != null && (DateTime.Now - comet.LastRequestTime).TotalSeconds > 20)
            //    //        {
            //    //            globalManager.SendLeaveLineMsg(comet.PrivateToken, comet.PrivateToken, "对象不存在", comet.IPIISWY, comet.CSId);
            //    //            globalManager.KillWaitRequest(comet.PrivateToken);
            //    //            globalManager.KillIdleCometClient(comet);
            //    //        }
            //    //    }
            //    //}

            //}
            //catch (Exception ex)
            //{
            //    BLL.Loger.Log4Net.Error("CometMessageDeal.CheckRubbishWCFClient异常           " + ex.Message);
            //}
            ////_clearRubbishObjTimer.Change(2 * 2 * 1000, Timeout.Infinite);
        }




    }
}
