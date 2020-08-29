using System;
using System.Collections.Concurrent;
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
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Core.Messages;

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
        //private ReaderWriterLockSlim _rwLockSlim = new ReaderWriterLockSlim();
        private Thread mainTheThread;
        private System.Threading.Timer timerDiagnose;
        private static readonly object _locker = new object();
        //private List<CometWaitRequest> waitRequests = new List<CometWaitRequest>();
        private ConcurrentDictionary<string, CometWaitRequest> waitRequests = new ConcurrentDictionary<string, CometWaitRequest>();

        private CometStateManager stateManager;

        //public List<CometWaitRequest> WaitRequests
        //{
        //    get { return this.waitRequests; }
        //}

        public CometWaitRequest[] WaitRequestsArray
        {
            get
            {
                //if (_rwLockSlim.TryEnterReadLock(100))
                //{
                //    try
                //    {
                //        return waitRequests.ToArray();
                //    }
                //    finally
                //    {
                //        _rwLockSlim.ExitReadLock();
                //    }
                //}
                //BLL.Loger.Log4Net.Info("获取锁失败：WaitRequestsArray");
                //return null;
                return waitRequests.Values.ToArray();
            }
        }

        public void RemoveWaitRequest(CometWaitRequest cwr)
        {
            if (cwr == null) return;
            if (cwr.Result != null)
            {
                //cwr.Result.SetCompleted();
                RemoveWaitRequestByToken(cwr.ClientPrivateToken);
            }
            //if (waitRequests.ContainsKey(cwr.ClientPrivateToken))
            //{
            //    waitRequests.TryRemove(cwr.ClientPrivateToken, out cwr);
            //}

            //if (_rwLockSlim.TryEnterWriteLock(100))
            //{
            //    try
            //    {
            //        this.waitRequests.Remove(cwr);
            //    }
            //    finally
            //    {
            //        _rwLockSlim.ExitWriteLock();
            //    }
            //}
            //else
            //{
            //    BLL.Loger.Log4Net.Info("获取锁失败：RemoveWaitRequest");
            //    //throw new Exception("移除对象CometWaitRequest失败：获取排他锁超时");
            //}


        }

        public void RemoveWaitRequestByToken(string privateToken)
        {
            if (string.IsNullOrEmpty(privateToken)) return;

            CometWaitRequest cwr;
            if (waitRequests.ContainsKey(privateToken))
            {
                if (waitRequests.TryRemove(privateToken, out cwr) && cwr != null && cwr.Result != null)
                {
                    if (cwr.Result.CometMessages == null)
                    {
                        cwr.Result.CometMessages = new CometMessage[]
                    {
                        new CometMessage()
                        {
                            MessageId = 0,
                            Name = "Be Killed",
                            Contents = "Be Killed"
                        }
                    };
                    }

                    cwr.Result.SetCompleted();
                }
            }


            //var tArray = this.WaitRequestsArray;

            //foreach (CometWaitRequest tRequest in tArray)
            //{
            //    if (tRequest.ClientPrivateToken == privateToken)
            //    {

            //        tRequest.Result.CometMessages = new CometMessage[]
            //        {
            //            new CometMessage()
            //            {
            //                MessageId = 0,
            //                Name = "Be Killed",
            //                Contents = "Be Killed"
            //            }
            //        };
            //        this.RemoveWaitRequest(tRequest);
            //        break;
            //    }
            //}


        }
        /*
        public void ClearWaitRequest()
        {
            var sList = this.waitRequests.ToList();
            foreach (CometWaitRequest request in sList)
            {
                if (request.Result != null)
                {
                    request.Result.SetCompleted();
                }
            }

            _rwLockSlim.EnterWriteLock();
            try
            {
                this.waitRequests.Clear();
            }
            finally
            {
                _rwLockSlim.ExitWriteLock();
            }
        }
        */

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
                try
                {
                    //检测CometClient对象是否断线超时
                    this.RemoveIdleCometClient();
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Info("方法检测线程状态：" + ex.Message);

                }
                finally
                {
                    timerDiagnose.Change(5000, Timeout.Infinite);
                }

            }), null, 1000, Timeout.Infinite);
        }

        internal void QueueCometWaitRequest(CometWaitRequest request)
        {
            if (!waitRequests.ContainsKey(request.ClientPrivateToken))
            {
                waitRequests.TryAdd(request.ClientPrivateToken, request);
            }

            //_rwLockSlim.EnterWriteLock();
            //try
            //{
            //    this.waitRequests.Add(request);
            //}
            //finally
            //{

            //    _rwLockSlim.ExitWriteLock();
            //}
            //标记comet最后Request时间
            var comtT = this.stateManager.GetCometClient(request.ClientPrivateToken);
            if (comtT != null)
            {
                comtT.LastRequestTime = DateTime.Now;
            }
            //Debug.WriteLine(string.Format("进入请求队列{0},Time:{1}", request.ClientPrivateToken, DateTime.Now.ToString("G")));
        }
        /*
                internal void DeactivateCometWaitRequest(CometWaitRequest request)
                {
                    //lock (state)
                    //{
                    //this.waitRequests.Remove(request);
                    //  we disable the request, and we hope the
                    //  client should connect immediatly else we time it out!
                    request.DateDeactivated = DateTime.Now;
                    //BLL.Loger.Log4Net.Info(string.Format("DeactivateCometWaitRequest                 => PriveteToken:{0},DT:{1}", request.ClientPrivateToken, DateTime.Now.ToString("O")));
                    //}
                }
                */

        private void QueueCometWaitRequest_Finished(CometWaitRequest target)
        {
            try
            {
                if (target != null)
                {
                    this.RemoveWaitRequest(target);//请求结束时移除Request对象
                    //DeactivateCometWaitRequest(target);
                    if (target.Result != null)
                    {
                        target.Result.SetCompleted();
                    }
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

                CometClient cometClient = this.stateManager.StateProvider.GetCometClient(cometTRequest.ClientPrivateToken);


                //如果comet对象已删除，则请求也删除
                if (cometClient == null)
                {
                    this.RemoveWaitRequest(cometTRequest);
                    //给网友发消息，提示被移除
                    var errorMessage = new CometMessage()
                    {
                        Name = "aspNetComet.error",
                        MessageId = 0,
                        Contents = ""
                    };
                    cometTRequest.HadRetriveMsg = 1;
                    cometTRequest.Result.CometMessages = new CometMessage[] { errorMessage };

                    this.QueueCometWaitRequest_Finished(cometTRequest);
                    return;
                }

                //判断长连接是否超时
                if (DateTime.Now.Subtract(cometTRequest.DateTimeAdded).TotalSeconds >= cometClient.ConnectionTimeoutSeconds)
                {
                    cometTRequest.Result.CometMessages = new CometMessage[]
                    {
                        new CometMessage()
                        {
                            MessageId = 0,
                            Name = "aspNetComet.timeout",
                            Contents = null
                        }
                    };
                    cometTRequest.HadRetriveMsg = 1;
                    this.QueueCometWaitRequest_Finished(cometTRequest);
                    return;
                }

                //如果是网友，并且sendMessagetime不为空，并且最后发送消息到现在超过空闲时间，把网友移除
                if ((cometClient.Type == (Int32)Entities.UserType.User) && (DateTime.Now.Subtract(cometClient.SendMessageTime).TotalSeconds >= cometClient.SendMessageIdleSeconds) && cometClient.CSId > 0)
                {
                    RemoveIdleCometWaitRequestForUserNoDailog(cometTRequest, cometClient);
                    return;
                }

                //有消息时发送消息                                
                CometMessage[] messages = this.CheckForServerPushMessages(cometTRequest);


                if (messages != null && messages.Length > 0)
                {
                    cometTRequest.Result.CometMessages = messages;
                    this.QueueCometWaitRequest_Finished(cometTRequest);
                }


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
                cometTRequest.HadRetriveMsg = 1;
                this.QueueCometWaitRequest_Finished(cometTRequest);


            }
            finally
            {
                cometTRequest.SetIdle();
            }
        }

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
        /// 检测CometClient对象是否断线超时
        /// </summary>
        private void RemoveIdleCometClient()
        {
            try
            {
                var t = stateManager.GetAllCometClients();
                string strLogMsg = string.Empty;
                CometClient cT = null;
                for (int i = t.Count - 1; i >= 0; i--)
                {
                    cT = t[i];
                    if (DateTime.Now.Subtract(cT.LastRequestTime).TotalSeconds < cT.ConnectionIdleSeconds || DateTime.Now.Subtract(cT.SendMessageTime).TotalSeconds < cT.ConnectionIdleSeconds)
                    { continue; }
                    if (cT.Type == 1)
                    {
                        ////如果是坐席的话，如果最后最后接消息时间超过指定值才移除
                        //if (DateTime.Now.Subtract(cT.LastMessageTime).TotalSeconds < cT.ConnectionIdleSeconds)
                        //{
                        //    continue;
                        //}
                        strLogMsg = string.Format("5S线程监控 断网超时移除对象：{0},name:{1},LastRequestTime:{2},当前时间：{3},超时时间：{4},超时设置时间：{5},最后发消息时间{6},最后收消息时间{7}",
                        cT.PrivateToken, cT.AgentNum ?? cT.DisplayName, cT.LastRequestTime.ToString("o"), DateTime.Now.ToString("o"), DateTime.Now.Subtract(cT.LastRequestTime).TotalSeconds, cT.ConnectionIdleSeconds, cT.SendMessageTime.ToString("G"), cT.LastMessageTime.ToString("G"));
                    }
                    else
                    {
                        strLogMsg = string.Format("5S线程监控 断网超时移除对象：{0},name:{1},LastRequestTime:{2},当前时间：{3},超时时间：{4},超时设置时间：{5}",
                        cT.PrivateToken, cT.AgentNum ?? cT.DisplayName, cT.LastRequestTime.ToString("o"), DateTime.Now.ToString("o"), DateTime.Now.Subtract(cT.LastRequestTime).TotalSeconds, cT.ConnectionIdleSeconds);
                    }
                    //如果对象最后消息时间和最后长连接时间 都超过了断网时间，则移除对象。                    

                    BLL.Loger.Log4Net.Info(strLogMsg);
                    this.stateManager.KillIdleCometClient(cT);



                    //add by qizq 2015-1-7 加监控入库日志
                    Entities.UserActionLog model = new UserActionLog();
                    model.CreateTime = System.DateTime.Now;
                    model.OperUserType = 3;
                    if (cT.Type == 1)
                    {
                        //坐席断网超时
                        model.LogInType = 1;
                    }
                    else
                    {
                        //网友断网超时
                        model.LogInType = 9;
                    }
                    model.LogInfo = strLogMsg;

                    //BLL.UserActionLog.Instance.Insert(model);
                    BulkInserUserActionThread.EnQueueActionLogs(model);

                    //this.RemoveWaitRequest();
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("Timer,移除断网对象出错：" + ex.Message);
                //add by qizq 2015-1-7 加监控入库日志
                //Entities.UserActionLog model = new UserActionLog();
                //model.CreateTime = System.DateTime.Now;
                ////model.CreateUserID = (int)userid;
                ////系统
                //model.OperUserType = 3;
                ////异常
                //model.LogInType = 3;
                //model.LogInfo = string.Format("Timer,移除断网对象出错：" + ex.Message);
                //BLL.UserActionLog.Instance.Insert(model);
            }

        }

        /*
        private void RemoveIdleCometWaitRequest(CometWaitRequest request, CometClient cometClient)
        {
            try
            {
                if (cometClient == null)
                {
                    this.RemoveWaitRequest(request);
                    request = null;
                    return;
                }
                if (request == null)
                {
                    return;
                }

                if (request.DateDeactivated != null && DateTime.Now.Subtract(request.DateDeactivated.Value).TotalSeconds < cometClient.ConnectionIdleSeconds)
                { return; }


                cometClient.Status = AgentStatus.Leaveline;
                if (cometClient.Type == (int)Entities.UserType.Agent)
                {
                    BLL.Loger.Log4Net.Info("[CheckForIdleCometWaitRequest]坐席断网时间超时，清除对象clientid:" + cometClient.PrivateToken + "!");

                    //当网友空闲时间已到根据网友标识，在与其聊天的坐席的在聊网友里将其移除
                    this.stateManager.ReAllocateOffLineAgentUsers(cometClient);
                }
                else
                {
                    BLL.Loger.Log4Net.Info("[CheckForIdleCometWaitRequest]网友断网时间超时，清除对象clientid:" + cometClient.PrivateToken + "!");
                    //通知网友你已离线
                    var chatMessage = new ChatMessage();
                    chatMessage.Message = "你已离线";
                    chatMessage.Time = DateTime.Now;
                    string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MLline);
                    stateManager.SendMessage(cometClient.PrivateToken, messagetype, chatMessage);
                }

                this.stateManager.KillIdleCometClient(cometClient.PrivateToken);
                this.RemoveWaitRequest(request);
                request = null;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("方法：RemoveIdleCometWaitRequest: " + ex.Message);

                //add by qizq 2015-1-7 加监控入库日志
                //Entities.UserActionLog model = new UserActionLog();
                //model.CreateTime = System.DateTime.Now;
                ////model.CreateUserID = (int)userid;
                ////系统
                //model.OperUserType = 3;
                ////异常
                //model.LogInType = 3;
                //model.LogInfo = string.Format("方法：RemoveIdleCometWaitRequest: " + ex.Message);
                //BLL.UserActionLog.Instance.Insert(model);
            }


        }

        */

        private void RemoveIdleCometWaitRequestForUserNoDailog(CometWaitRequest request, CometClient cometClient)
        {
            try
            {
                if (cometClient != null)
                {
                    //BLL.Loger.Log4Net.Info("[CheckForIdleCometWaitRequestForUserNoDailog]网友空闲时间超时，清除对象clientid:" + cometClient.PrivateToken);

                    //add by qizq 2015-1-7 加监控入库日志
                    Entities.UserActionLog model = new UserActionLog();
                    model.CreateTime = System.DateTime.Now;
                    //model.CreateUserID = (int)userid;
                    //系统
                    model.OperUserType = 3;
                    //网友空闲超时
                    model.LogInType = 2;
                    model.LogInfo = "网友空闲时间超时，清除对象clientid:" + cometClient.PrivateToken;
                    //BLL.UserActionLog.Instance.Insert(model);
                    BulkInserUserActionThread.EnQueueActionLogs(model);



                    //当网友空闲时间已到根据网友标识，在与其聊天的坐席的在聊网友里将其移除                
                    this.stateManager.KillIdleCometClient(cometClient);
                }

                if (request != null)
                {
                    this.RemoveWaitRequest(request);
                    //给网友发消息，提示被移除
                    var errorMessage = new CometMessage()
                    {
                        Name = "aspNetComet.error",
                        MessageId = 0,
                        Contents = ""
                    };
                    request.Result.CometMessages = new CometMessage[] { errorMessage };
                    request.Result.SetCompleted();
                }

            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info(string.Format("方法：RemoveIdleCometWaitRequestForUserNoDailog中错误：{0}", ex.Message));

                ////add by qizq 2015-1-7 加监控入库日志
                //Entities.UserActionLog model = new UserActionLog();
                //model.CreateTime = System.DateTime.Now;
                ////model.CreateUserID = (int)userid;
                ////系统
                //model.OperUserType = 3;
                ////异常
                //model.LogInType = 3;
                //model.LogInfo = string.Format("方法：RemoveIdleCometWaitRequestForUserNoDailog中错误：{0}", ex.Message);
                //BLL.UserActionLog.Instance.Insert(model);
            }

        }

        private CometMessage[] CheckForServerPushMessages(CometWaitRequest request)
        {
            //
            //  ok, we we need to do is get the messages 
            //  that are stored in the state provider

            if (request.HadRetriveMsg == 0)
            {
                var cometMsgs = this.stateManager.StateProvider.GetMessages(request.ClientPrivateToken, request.LastMessageId);
                if (cometMsgs != null && cometMsgs.Length > 0)
                {
                    request.HadRetriveMsg = 1;
                }
                return cometMsgs;
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
