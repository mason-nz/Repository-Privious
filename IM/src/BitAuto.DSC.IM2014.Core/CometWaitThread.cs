using System;
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

namespace BitAuto.DSC.IM2014.Core
{
    /// <summary>
    /// Class CometWaitThread
    /// 
    /// This class contains an implementation of the thread pool that controls the
    /// CometWaitRequest objects and returns specified messages, errors or timeout messages
    /// back to the client in a controlled and scalable fasion
    /// </summary>
    public class CometWaitThread
    {
        private object state = new object();
        private List<CometWaitRequest> waitRequests = new List<CometWaitRequest>();
        private CometStateManager stateManager;

        public List<CometWaitRequest> WaitRequests
        {
            get { return this.waitRequests; }
        }

        public CometWaitRequest[] CometRequests
        {
            get
            {
                lock (this.state)
                {
                    return waitRequests.ToArray();
                }
            }
        }

        public CometWaitThread(CometStateManager stateManager)
        {
            //  get the state manager
            this.stateManager = stateManager;

            Thread t = new Thread(new ThreadStart(QueueCometWaitRequest_WaitCallback));
            t.IsBackground = false;
            t.Start();
        }

        internal void QueueCometWaitRequest(CometWaitRequest request)
        {
            lock (this.state)
            {
                waitRequests.Add(request);
            }
        }

        internal void DeactivateCometWaitRequest(CometWaitRequest request)
        {
            lock (state)
            {
                //this.waitRequests.Remove(request);
                //  we disable the request, and we hope the
                //  client should connect immediatly else we time it out!
                request.DateDeactivated = DateTime.Now;
            }
        }

        private void QueueCometWaitRequest_Finished(object target)
        {
            CometWaitRequest request = target as CometWaitRequest;
            request.Result.SetCompleted();
        }

        private void QueueCometWaitRequest_WaitCallback()
        {
            //  here we are...
            //  in a loop

            while (true)
            {
                //Debug.WriteLine(string.Format("QueueCometWaitRequest_WaitCallback Tick: {0} {1} ", Thread.CurrentThread.IsThreadPoolThread, Thread.CurrentThread.ManagedThreadId));

                CometWaitRequest[] processRequest;

                lock (this.state)
                {
                    processRequest = waitRequests.ToArray();
                }

                //  we have no more wait requests left, so we want exis
                /*if (processRequest.Length == 0)
                    break;*/

                if (processRequest.Length == 0)
                {
                    //  sleep for this time
                    Thread.Sleep(50);
                }
                else
                {

                    for (int i = 0; i < processRequest.Length; i++)
                    {
                        try
                        {
                            CometClient cometClient = this.stateManager.StateProvider.GetCometClient(processRequest[i].ClientPrivateToken);

                            if (processRequest[i].Active && cometClient != null)
                            {
                                Thread.Sleep(100);

                                //  timed out so remove from the queue
                                if (DateTime.Now.Subtract(processRequest[i].DateTimeAdded).TotalSeconds >= cometClient.ConnectionTimeoutSeconds)
                                {
                                    //  dequeue the request 
                                    DeactivateCometWaitRequest(processRequest[i]);

                                    //  get the message
                                    CometMessage timeoutMessage = new CometMessage()
                                        {
                                            MessageId = 0,
                                            Name = "aspNetComet.timeout",
                                            Contents = null,
                                            ProcessTime = DateTime.Now.ToString()
                                        };

                                    //
                                    //  ok, we we timeout the message
                                    processRequest[i].Result.CometMessages = new CometMessage[] { timeoutMessage };
                                    //  call the message
                                    this.QueueCometWaitRequest_Finished(processRequest[i]);
                                }
                                //如果是网友，并且sendMessagetime不为空，并且上次发送消息到现在超过空闲时间，把网友移除
                                else if ((cometClient.Type == (Int32)Entities.UserType.User) && (DateTime.Compare(cometClient.SendMessageTime, Convert.ToDateTime("0001-1-1 0:00:00")) != 0) && (DateTime.Now.Subtract(cometClient.SendMessageTime).TotalSeconds >= cometClient.SendMessageIdleSeconds))
                                {
                                    CheckForIdleCometWaitRequestForUserNoDailog(processRequest[i], cometClient);
                                }
                                else
                                {
                                    CometMessage[] messages = this.CheckForServerPushMessages(processRequest[i]);

                                    if (messages != null && messages.Length > 0)
                                    {
                                        //  we have our message
                                        processRequest[i].Result.CometMessages = messages;
                                        //  and return!
                                        //  dequeue the request
                                        DeactivateCometWaitRequest(processRequest[i]);
                                        //  queue the response on another ASP.NET Worker thread
                                        this.QueueCometWaitRequest_Finished(processRequest[i]);
                                    }
                                }
                            }
                            else
                            {
                                Thread.Sleep(100);// 添加次方法，避免页面关闭时，服务器一直轮询当前线程中所有的请求对象 add by qizq 2014-3-6
                                //  this is an inactive 
                                this.CheckForIdleCometWaitRequest(processRequest[i], cometClient);
                            }
                        }
                        catch (Exception ex)
                        {
                            BLL.Loger.Log4Net.Info("遍历请求错误Message:" + ex.Message);
                            BLL.Loger.Log4Net.Info("错误StackTrace:" + ex.StackTrace);

                            if (processRequest[i].Active)
                            {
                                //  ok, this one has screwed up, so
                                //  we need to dequeue the request from ASP.NET, basically disable it and return
                                //  dequeue the request 
                                DeactivateCometWaitRequest(processRequest[i]);

                                //  get the message
                                CometMessage errorMessage = new CometMessage()
                                {
                                    MessageId = 0,
                                    Name = "aspNetComet.error",
                                    Contents = ex.Message
                                };

                                //
                                //  ok, we we timeout the message
                                processRequest[i].Result.CometMessages = new CometMessage[] { errorMessage };
                                //  call the message
                                this.QueueCometWaitRequest_Finished(processRequest[i]);
                            }
                            else
                            {
                                Thread.Sleep(100);// 添加次方法，避免页面关闭时，服务器一直轮询当前线程中所有的请求对象，Add=Masj,Date=2014-02-28
                                //  this is not an active request, so we dequeue it from the
                                //  thread
                                this.DequeueCometWaitRequest(processRequest[i].ClientPrivateToken);
                            }
                        }
                    }
                }
            }
        }

        private void CheckForIdleCometWaitRequest(CometWaitRequest request, CometClient cometClient)
        {
            lock (state)
            {
                if (DateTime.Now.Subtract(request.DateDeactivated.Value).TotalSeconds >= cometClient.ConnectionIdleSeconds)
                {
                    if (cometClient.Type == (int)Entities.UserType.Agent)
                    {
                        Debug.WriteLine("[CheckForIdleCometWaitRequest]坐席断网时间超时，清除对象clientid:" + cometClient.PrivateToken + "!");
                        BLL.Loger.Log4Net.Info("[CheckForIdleCometWaitRequest]坐席断网时间超时，清除对象clientid:" + cometClient.PrivateToken + "!");
                    }
                    else
                    {
                        Debug.WriteLine("[CheckForIdleCometWaitRequest]网友断网时间超时，清除对象clientid:" + cometClient.PrivateToken + "!");
                        BLL.Loger.Log4Net.Info("[CheckForIdleCometWaitRequest]网友断网时间超时，清除对象clientid:" + cometClient.PrivateToken + "!");
                    }
                    //当网友空闲时间已到根据网友标识，在与其聊天的坐席的在聊网友里将其移除
                    this.stateManager.RemoveDialogComeetByUserID(cometClient.PrivateToken);
                    //  ok, this dude has timed out, so we remove it
                    this.stateManager.KillIdleCometClient(cometClient.PrivateToken);
                    //  and deque the request
                    this.waitRequests.Remove(request);
                }
            }
        }

        private void CheckForIdleCometWaitRequestForUserNoDailog(CometWaitRequest request, CometClient cometClient)
        {
            lock (state)
            {
                Debug.WriteLine("[CheckForIdleCometWaitRequestForUserNoDailog]网友空闲时间超时，清除对象clientid:" + cometClient.PrivateToken + "!");
                BLL.Loger.Log4Net.Info("[CheckForIdleCometWaitRequestForUserNoDailog]网友空闲时间超时，清除对象clientid:" + cometClient.PrivateToken + "!");
                //当网友空闲时间已到根据网友标识，在与其聊天的坐席的在聊网友里将其移除
                this.stateManager.RemoveDialogComeetByUserID(cometClient.PrivateToken);
                //  ok, this dude has timed out, so we remove it
                this.stateManager.KillIdleCometClient(cometClient.PrivateToken);
            }
        }

        private CometMessage[] CheckForServerPushMessages(CometWaitRequest request)
        {
            //
            //  ok, we we need to do is get the messages 
            //  that are stored in the state provider
            return this.stateManager.StateProvider.GetMessages(request.ClientPrivateToken, request.LastMessageId);
        }


        internal void DequeueCometWaitRequest(string privateToken)
        {
            lock (state)
            {
                for (int i = 0; i < this.waitRequests.Count; i++)
                {
                    CometWaitRequest request = this.waitRequests[i];

                    if (request.ClientPrivateToken == privateToken)
                    {
                        //  remove it
                        this.waitRequests.Remove(request);
                        break;
                    }
                }
            }
        }
    }
}
