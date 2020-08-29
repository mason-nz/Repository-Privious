using System;
using System.Collections.Concurrent;
using System.Data;
using System.Configuration;
using System.Linq;

using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Core.Messages;
using BitAuto.DSC.IM_2015.MainInterface;

namespace BitAuto.DSC.IM_2015.Core
{
    /// <summary>
    /// Class CometWaitThread
    /// 
    /// This class contains an implementation of the thread pool that controls the
    /// CometWaitRequest objects and returns specified messages, errors or timeout messages
    /// back to the client in a controlled and scalable fasion
    /// </summary>
    public class CometWaitThread : IDisposable
    {

        private Thread mainTheThread;
        private System.Threading.Timer timerDiagnose;
        private static readonly object _locker = new object();

        private ConcurrentDictionary<string, CometWaitRequest> waitRequests = new ConcurrentDictionary<string, CometWaitRequest>();

        private CometStateManager stateManager;

        public CometWaitRequest[] WaitRequestsArray
        {
            get
            {
                return waitRequests.Values.ToArray();
            }
        }

        public void RemoveWaitRequest(CometWaitRequest cwr, bool needFlush = true)
        {
            if (cwr == null) return;
            RemoveWaitRequestByToken(cwr.ClientPrivateToken, needFlush);
        }

        /// <summary>
        /// 结束长连接
        /// </summary>
        /// <param name="privateToken"></param>
        /// <param name="needFlush">需要强刷[再次获取消息]</param>
        public void RemoveWaitRequestByToken(string privateToken, bool needFlush = true)
        {
            if (string.IsNullOrEmpty(privateToken)) return;

            CometWaitRequest cwr;
            if (waitRequests.ContainsKey(privateToken))
            {
                if (waitRequests.TryRemove(privateToken, out cwr) && cwr != null && cwr.Result != null)
                {
                    if (needFlush)
                    {
                        var reaminMsg = this.CheckForServerPushMessages(cwr);
                        if (reaminMsg != null && reaminMsg.Length > 0)
                        {
                            if (cwr.Result.CometMessages == null)
                            {
                                cwr.Result.CometMessages = reaminMsg;
                            }
                            else
                            {
                                List<CometMessage> lst = new List<CometMessage>(cwr.Result.CometMessages);
                                lst.AddRange(reaminMsg);
                                cwr.Result.CometMessages = lst.ToArray();
                            }
                            BLL.Loger.Log4Net.Info(privateToken + "的消息，通过强刷，已经返回到Client端");
                        }

                        //cwr.Result.CometMessages = new CometMessage[] { };
                        //new CometMessage()
                        //{
                        //    MessageId = 0,
                        //    Name = "Be Killed",
                        //    Contents = "Be Killed"
                        //}};
                    }

                    cwr.Result.SetCompleted();
                    cwr.HadRetriveMsg = 0;
                }
                else
                {
                    if (cwr != null && cwr.Result != null)
                    {
                        cwr.Result.SetCompleted();
                        cwr.HadRetriveMsg = 0;
                    }
                }
            }

        }


        public void Close()
        {
            Dispose();
        }

        public CometWaitThread(CometStateManager stateManager)
        {
            //  get the state manager
            this.stateManager = stateManager;
            StartThread();
        }

        private void StartThread()
        {
            mainTheThread = new Thread(new ThreadStart(QueueCometWaitRequest_WaitCallback));
            mainTheThread.IsBackground = false;
            mainTheThread.Start();

            timerDiagnose = new Timer(new TimerCallback(o =>
            {
                //检测CometClient对象是否断线超时
                this.RemoveIdleCometClient();
                timerDiagnose.Dispose();
            }), null, 1000, Timeout.Infinite);
        }

        internal void QueueCometWaitRequest(CometWaitRequest request)
        {
            if (!waitRequests.ContainsKey(request.ClientPrivateToken))
            {
                waitRequests.TryAdd(request.ClientPrivateToken, request);
            }

            ////标记comet最后Request时间
            //var comtT = this.stateManager.GetCometClient(request.ClientPrivateToken);
            //if (comtT != null)
            //{
            //    comtT.LastRequestTime = DateTime.Now;
            //}
        }


        //结束请求
        private void QueueCometWaitRequest_Finished(CometWaitRequest target)
        {
            try
            {
                if (target != null)
                {
                    this.RemoveWaitRequest(target, false);//请求结束时移除Request对象                    
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("方法：QueueCometWaitRequest_Finished:错误：" + ex.Message + ex.StackTrace);
            }
        }

        private void ProcessEachRequest(object o)
        {
            var cometTRequest = o as CometWaitRequest;
            if (cometTRequest == null)
            {
                return;
            }

            try
            {

                //CometClient cometClient = this.stateManager.StateProvider.GetCometClient(cometTRequest.ClientPrivateToken);


                ////如果comet对象已删除，则请求也删除
                //if (cometClient == null)
                //{
                //    //this.RemoveWaitRequest(cometTRequest);
                //    //给网友发消息，提示被移除
                //    var errorMessage = new CometMessage()
                //    {
                //        Name = "aspNetComet.error",
                //        MessageId = 0,
                //        Contents = ""
                //    };
                //    cometTRequest.Result.CometMessages = new CometMessage[] { errorMessage };
                //    BLL.Loger.Log4Net.Info(string.Format("方法CometWaitThread.ProcessEachRequest，comet对象为空返回:{0}", cometTRequest.ClientPrivateToken));
                //    this.QueueCometWaitRequest_Finished(cometTRequest);
                //    return;
                //}

                //判断长连接是否超时
                if (DateTime.Now.Subtract(cometTRequest.DateTimeAdded).TotalSeconds >= BLL.Util.GetConnectionTimeoutSeconds())
                {
                    cometTRequest.Result.CometMessages = new CometMessage[]
                    {
                        new CometMessage()
                        {
                            MessageId = 0,
                            Name = "aspNetComet.timeout",
                            Contents = null,
                            ServiceIp=BLL.Util.IpToLong().ToString()
                        }
                    };
                    //BLL.Loger.Log4Net.Info(string.Format("方法CometWaitThread.ProcessEachRequest，长连接超时返回:{0},     LastMessageId：{1},HadRetriveMsg：{2}", cometTRequest.ClientPrivateToken, cometTRequest.LastMessageId, cometTRequest.HadRetriveMsg));
                    this.QueueCometWaitRequest_Finished(cometTRequest);
                    return;
                }

                ////如果是网友，并且sendMessagetime不为空，并且最后发送消息到现在超过空闲时间，把网友移除
                //if ((cometClient.Type == (Int32)Entities.UserType.User) && (DateTime.Now.Subtract(cometClient.SendMessageTime).TotalSeconds >= cometClient.SendMessageIdleSeconds)
                //     && (DateTime.Now.Subtract(cometClient.LastReceiveMsgTime).TotalSeconds >= cometClient.SendMessageIdleSeconds)
                //    && cometClient.CSId > 0)
                //{
                //    RemoveIdleCometWaitRequestForUserNoDailog(cometTRequest, cometClient);
                //    return;
                //}

                //有消息时发送消息                                
                CometMessage[] messages = this.CheckForServerPushMessages(cometTRequest);


                if (messages != null && messages.Length > 0)
                {
                    cometTRequest.Result.CometMessages = messages;
                    this.QueueCometWaitRequest_Finished(cometTRequest);
                    //BLL.Loger.Log4Net.Info(cometTRequest.ClientPrivateToken + "的消息已经返回到Client端");
                }
                //else
                //{
                //    BLL.Loger.Log4Net.Info(string.Format("方法CometWaitThread.ProcessEachRequest，长连接无有效消息返回:{0}", cometTRequest.ClientPrivateToken));
                //}


            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("遍历请求错误Message:" + ex.Message);
                BLL.Loger.Log4Net.Info("错误StackTrace:" + ex.StackTrace);

                cometTRequest.Result.CometMessages = new CometMessage[]
                    {
                        new CometMessage()
                        {
                            MessageId = 0,
                            Name = "aspNetComet.error",
                            Contents = ex.Message
                        }
                    };

                this.QueueCometWaitRequest_Finished(cometTRequest);


            }
            finally
            {
                cometTRequest.SetIdle();
            }
        }

        //用于轮询监控请求是否超时，空闲超时
        private void QueueCometWaitRequest_WaitCallback()
        {
            CometWaitRequest[] processRequest = null;
            //默认是100ms
            int nSleepTime = 100;
            long iTest = 0;
            int nLength = 0;
            try
            {
                while (true)
                {
                    Thread.Sleep(nSleepTime);

                    processRequest = this.WaitRequestsArray;
                    nLength = processRequest.Length;


                    if (nLength == 0)
                    {
                        nSleepTime = 500;
                        continue;
                    }
                    if (nLength < 10)
                    {
                        nSleepTime = 100;
                    }
                    else if (nLength < 100)
                    {
                        nSleepTime = 30;
                    }
                    else
                    {
                        nSleepTime = 10;
                    }

                    iTest++;

                    //if (iTest%500 == 0)
                    //{
                    //    throw new Exception("QueueCometWaitRequest_WaitCallback");
                    //}
                    foreach (var cmtT in processRequest)
                    {
                        if (cmtT.Check_SetBusy())
                        {
                            ThreadPool.QueueUserWorkItem(ProcessEachRequest, cmtT);
                        }
                    }
                }

            }
            catch (ThreadAbortException abex)
            {
                BLL.Loger.Log4Net.Info("主处理线程被Abort:" + abex.Message);
                ////add by qizq 2015-1-7 加监控入库日志
                //Entities.UserActionLog model = new UserActionLog();
                //model.CreateTime = System.DateTime.Now;
                ////model.CreateUserID = (int)userid;
                ////系统
                //model.OperUserType = 3;
                ////异常
                //model.LogInType = 3;
                //model.LogInfo = string.Format("主处理线程被Abort:" + abex.Message);
                //BLL.UserActionLog.Instance.Insert(model);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("线程内部错误Message:" + ex.Message);
                StartThread();
                Thread.Sleep(1000);
                BLL.Loger.Log4Net.Info("重新开启主处理线程");

            }
            finally
            {
                //this.ClearWaitRequest();
                //BLL.Loger.Log4Net.Info("线程遇到错误结束。。。。");
            }
        }

        /// <summary>
        /// 用于5S轮询 检测CometClient对象是否断线超时
        /// </summary>
        private void RemoveIdleCometClient()
        {
            while (true)
            {
                Thread.Sleep(5000);

                try
                {
                    //只判断本地的，坐席的对话网友有可能非本IIS上的。
                    //var t = stateManager.GetAllCometClients().ToList();
                    //string strLogMsg = string.Empty;
                    //CometClient cT = null;
                    //for (int i = t.Count - 1; i >= 0; i--)
                    //{
                    //    cT = t[i];
                    //    if (DateTime.Now.Subtract(cT.LastRequestTime).TotalSeconds < cT.ConnectionIdleSeconds ||
                    //        DateTime.Now.Subtract(cT.SendMessageTime).TotalSeconds < cT.ConnectionIdleSeconds)
                    //    {
                    //        continue;
                    //    }
                    //    if (cT.Type == 1)
                    //    {
                    //        //发送离线通知
                    //        this.stateManager.NoticeAgentRemoved(cT,(int)Entities.CloseType.AgentTimeOut);

                    //        strLogMsg =
                    //            string.Format(
                    //                "5S线程监控 断网超时移除对象：{0},name:{1},LastRequestTime:{2},当前时间：{3},超时时间：{4},超时设置时间：{5},最后发消息时间{6},最后收消息时间{7}",
                    //                cT.PrivateToken, cT.AgentNum ?? cT.DisplayName, cT.LastRequestTime.ToString("o"),
                    //                DateTime.Now.ToString("o"), DateTime.Now.Subtract(cT.LastRequestTime).TotalSeconds,
                    //                cT.ConnectionIdleSeconds, cT.SendMessageTime.ToString("G"),
                    //                cT.LastReceiveMsgTime.ToString("G"));
                    //    }
                    //    else
                    //    {
                    //        this.stateManager.NoticeNetFriendRemoved(cT, (int)Entities.CloseType.NetFrindTimeOut);
                    //        strLogMsg =
                    //            string.Format(
                    //                "5S线程监控 断网超时移除对象：{0},name:{1},LastRequestTime:{2},当前时间：{3},超时时间：{4},超时设置时间：{5}",
                    //                cT.PrivateToken, cT.AgentNum ?? cT.DisplayName, cT.LastRequestTime.ToString("o"),
                    //                DateTime.Now.ToString("o"), DateTime.Now.Subtract(cT.LastRequestTime).TotalSeconds,
                    //                cT.ConnectionIdleSeconds);
                    //    }
                    //    //如果对象最后消息时间和最后长连接时间 都超过了断网时间，则移除对象。                    

                    //    BLL.Loger.Log4Net.Info(strLogMsg);
                    //    this.stateManager.KillWaitRequest(cT);
                    //    this.stateManager.KillIdleCometClient(cT);


                    //    #region 写日志

                    //    //add by qizq 2015-1-7 加监控入库日志
                    //    Entities.UserActionLog model = new UserActionLog();
                    //    model.CreateTime = System.DateTime.Now;
                    //    model.OperUserType = 3;
                    //    if (cT.Type == 1)
                    //    {
                    //        //坐席断网超时
                    //        model.LogInType = 1;
                    //    }
                    //    else
                    //    {
                    //        //网友断网超时
                    //        model.LogInType = 9;
                    //    }
                    //    model.LogInfo = strLogMsg;

                    //    //BLL.UserActionLog.Instance.Insert(model);
                    //    BulkInserUserActionThread.EnQueueActionLogs(model);

                    //    //this.RemoveWaitRequest();

                    //    #endregion
                    // }
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Info("Timer,移除断网对象出错：" + ex.Message);
                }
            }
        }

        /// <summary>
        /// 网友空闲超时删除
        /// </summary>
        /// <param name="request"></param>
        /// <param name="netFrinedClient"></param>
        private void RemoveIdleCometWaitRequestForUserNoDailog(CometWaitRequest request, CometClient netFrinedClient)
        {
            try
            {
                //if (netFrinedClient != null)
                //{
                //BLL.Loger.Log4Net.Info("[RemoveIdleCometWaitRequestForUserNoDailog]网友空闲时间超时，清除对象clientid:" + netFrinedClient.PrivateToken);

                //var strLogMsg = string.Format("[RemoveIdleCometWaitRequestForUserNoDailog]网友空闲时间超时，清除对象clientid：{0},name:{1},LastRequestTime:{2},当前时间：{3},超时时间：{4},超时设置时间：{5},最后发消息时间{6},最后收消息时间{7}",
                //    netFrinedClient.PrivateToken, netFrinedClient.AgentNum ?? netFrinedClient.DisplayName, netFrinedClient.LastRequestTime.ToString("o"), DateTime.Now.ToString("o"), DateTime.Now.Subtract(netFrinedClient.LastRequestTime).TotalSeconds, netFrinedClient.ConnectionIdleSeconds, netFrinedClient.SendMessageTime.ToString("G"), netFrinedClient.LastReceiveMsgTime.ToString("G"));

                //BLL.Loger.Log4Net.Info(strLogMsg);
                //this.stateManager.NoticeNetFriendRemoved(netFrinedClient, (int)Entities.CloseType.NetFrindTimeOut);

                //this.stateManager.KillIdleCometClient(netFrinedClient);

                //#region 写日志

                ////add by qizq 2015-1-7 加监控入库日志
                //Entities.UserActionLog model = new UserActionLog();
                //model.CreateTime = System.DateTime.Now;
                ////model.CreateUserID = (int)userid;
                ////系统
                //model.OperUserType = 3;
                ////网友空闲超时
                //model.LogInType = 2;
                //model.LogInfo = "网友空闲时间超时，清除对象clientid:" + netFrinedClient.PrivateToken;
                //BulkInserUserActionThread.EnQueueActionLogs(model);
                //#endregion

                //}

                //给网友发消息，提示被移除
                //var errorMessage = new CometMessage()
                //{
                //    Name = "aspNetComet.error",
                //    MessageId = 0,
                //    Contents = ""
                //};
                //request.Result.CometMessages = new CometMessage[] { errorMessage };
                //request.Result.SetCompleted();

                //移除所有消息
                this.RemoveWaitRequest(request);

            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info(string.Format("方法：RemoveIdleCometWaitRequestForUserNoDailog中错误：{0}", ex.Message));
            }

        }

        private CometMessage[] CheckForServerPushMessages(CometWaitRequest request)
        {
            //
            //  ok, we we need to do is get the messages 
            //  that are stored in the state provider
            //一次长连接只能取一次数据。

            if (request.HadRetriveMsg == 0)
            {
                var cometMsgs = this.stateManager.StateProvider.GetMessages(request.ClientPrivateToken, request.LastMessageId);
                if (cometMsgs != null && cometMsgs.Length > 0)
                {
                    //BLL.Loger.Log4Net.Info(request.ClientPrivateToken + "取回" + cometMsgs.Length + "条消息，消息id大于" + request.LastMessageId + ",第一条类型是：" + cometMsgs[0].Name + ",发送者是：" + cometMsgs[0].FromToken);
                    request.HadRetriveMsg = 1;
                }
                for (int i = 0; i < cometMsgs.Length; i++)
                {
                    cometMsgs[i].ServiceIp = BLL.Util.IpToLong().ToString();
                }
                return cometMsgs;
            }
            else
            {
                BLL.Loger.Log4Net.Info(string.Format("方法CheckForServerPushMessages中，" + request.ClientPrivateToken + "获取Message失败，因为request.HadRetriveMsg = 1"));
            }
            return new CometMessage[] { };
        }



        public void Dispose()
        {
            if (mainTheThread != null)
            {
                mainTheThread.Abort();
            }
            //_rwLockSlim.Dispose();
        }
    }
}
