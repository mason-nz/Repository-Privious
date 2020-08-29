using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BitAuto.DSC.IM_2015.Core.Messages;
using System.Diagnostics;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.MainInterface;

namespace BitAuto.DSC.IM_2015.Core
{

    public class InProcCometStateProvider : ICometStateProvider
    {
        /// <summary>
        /// Private class which holds the state of each connected client
        /// </summary>
        //private class InProcCometClient
        //{
        //    public CometClient CometClient;



        //    /// <summary>
        //    /// 记录上次请求返回的消息，用来避免消息丢失
        //    /// </summary>
        //    //public ConcurrentQueue<CometMessage> queueMessagesDeleted = new ConcurrentQueue<CometMessage>();
        //    public ConcurrentDictionary<long, CometMessage> queueMessagesDeleted = new ConcurrentDictionary<long, CometMessage>();
        //    public long NextMessageId = 1;


        //    //public void AddMessage(CometMessage value)
        //    //{
        //    //    value.MessageId = Interlocked.Increment(ref NextMessageId);
        //    //    if (this.dicMessages.TryAdd(value.MessageId, value))
        //    //    {
        //    //        //if (value.Name == "MAllocAgent")
        //    //        //{}
        //    //        //BLL.Loger.Log4Net.Info(string.Format("方法InProcCometStateProvider.AddMessage，对象:{0}, 消息个数 {1},当前消息ID：{2},消息名称:{3}", CometClient.PrivateToken, this.dicMessages.Count, value.MessageId, value.Name));

        //    //    }
        //    //    else
        //    //    {
        //    //        BLL.Loger.Log4Net.Error(string.Format("方法InProcCometStateProvider.AddMessage 失败，token:{0}  中消息个数 {1},当前消息ID：{2},消息名称:{3}", CometClient.PrivateToken, this.dicMessages.Count, value.MessageId, value.Name));
        //    //    }

        //    //}

        //    //public void RemoveMessage(long key)
        //    //{
        //    //    CometMessage c;
        //    //    this.dicMessages.TryRemove(key, out c);

        //    //}


        //}
        public ConcurrentDictionary<string, ConcurrentDictionary<long, CometMessage>> dicMessages = new ConcurrentDictionary<string, ConcurrentDictionary<long, CometMessage>>();
        //private ConcurrentDictionary<string, InProcCometClient> privateCUClients = new ConcurrentDictionary<string, InProcCometClient>();


        #region lock

        //private static readonly object _locker = new object();

        //private void AddPrivateClients(string clientPrivateToken, InProcCometClient value)
        //{

        //    this.privateCUClients.TryAdd(clientPrivateToken, value);

        //    #region 添加动作日志
        //    //BLL.Loger.Log4Net.Info(string.Format("新增comet对象：{0},类型：{1},", clientPrivateToken, value.CometClient.Type));

        //    //add by qizq 2015-1-7 加监控入库日志
        //    Entities.UserActionLog model = new UserActionLog();
        //    model.CreateTime = System.DateTime.Now;
        //    if (value.CometClient.Type == 1)
        //    {
        //        int _CreateUserID = 0;
        //        if (int.TryParse(clientPrivateToken, out _CreateUserID))
        //        {
        //            model.CreateUserID = _CreateUserID;
        //        }
        //        model.OperUserType = value.CometClient.Type;

        //        //坐席登录
        //        model.LogInType = 6;
        //    }
        //    else
        //    {
        //        //客户登录
        //        model.LogInType = 4;
        //        //model.TrueName = value.CometClient.MemberName;
        //        model.OperUserType = value.CometClient.Type;
        //    }
        //    model.LogInfo = string.Format("新增comet对象：{0},对象类型：{1},", clientPrivateToken, value.CometClient.Type == 1 ? "客服" : "网友");
        //    //BLL.UserActionLog.Instance.Insert(model);
        //    BulkInserUserActionThread.EnQueueActionLogs(model);
        //    #endregion


        //}

        //public void KillIdleCometClient(string clientPrivateToken)
        //{
        //    var tComnet = GetCometClient(clientPrivateToken);

        //    #region 添加动作日志
        //    if (tComnet != null)
        //    {
        //        BLL.Loger.Log4Net.Info(string.Format("InProcCometStateProvider.KillIdleCometClient  移除comet对象：{0},类型：{1},最后发消息时间:{2},最后长连接时间:{3}", clientPrivateToken,
        //            tComnet.Type, tComnet.SendMessageTime.ToString("O"), tComnet.LastRequestTime.ToString("O")));


        //        //add by qizq 2015-1-7 加监控入库日志
        //        Entities.UserActionLog model = new UserActionLog();
        //        model.CreateTime = System.DateTime.Now;
        //        if (tComnet.Type == 1)
        //        {
        //            int _CreateUserID = 0;
        //            if (int.TryParse(clientPrivateToken, out _CreateUserID))
        //            {
        //                model.CreateUserID = _CreateUserID;
        //            }
        //            model.OperUserType = tComnet.Type;

        //            //坐席退出
        //            model.LogInType = 7;
        //            //修改坐席状态结束时间
        //            BLL.AgentStatusDetail.Instance.UpdateAgentLastStatus(tComnet.AgentStatusRecID);
        //        }
        //        else
        //        {
        //            //客户退出
        //            model.LogInType = 5;
        //            model.OperUserType = tComnet.Type;
        //            //model.TrueName = tComnet.MemberName;
        //        }
        //        model.LogInfo = string.Format("InProcCometStateProvider.KillIdleCometClient 移除comet对象：{0},对象类型：{1},最后发消息时间:{2},最后长连接时间:{3}", clientPrivateToken,
        //            tComnet.Type == 1 ? "客服" : "网友", tComnet.SendMessageTime.ToString("O"), tComnet.LastRequestTime.ToString("O"));
        //        //BLL.UserActionLog.Instance.Insert(model);
        //        BulkInserUserActionThread.EnQueueActionLogs(model);
        //    }
        //    #endregion

        //    InProcCometClient c;
        //    this.privateCUClients.TryRemove(clientPrivateToken, out c);
        //    c = null;

        //}

        //public List<CometClient> GetAllCometClients()
        //{
        //    return this.privateCUClients.Values.Select(tC => tC.CometClient).ToList();
        //}

        //public CometClient GetCometClient(string clientPrivateToken)
        //{
        //    InProcCometClient comet;
        //    if (this.privateCUClients.TryGetValue(clientPrivateToken, out comet))
        //    {
        //        return comet.CometClient;
        //    }
        //    return null;

        //}

        #endregion


        #region ICometStateProvider Members


        public void RemoveDicMessage()
        {
            dicMessages.Clear();
        }


        /// <summary>
        /// Store the new client in memory
        /// </summary>
        /// <param name="cometClient"></param>
        //public void InitializeClient(CometClient cometClient)
        //{
        //    if (cometClient == null)
        //        throw new ArgumentNullException("cometClient");

        //    if (this.privateCUClients.ContainsKey(cometClient.PrivateToken))
        //    {
        //        return;
        //        //throw CometException.CometClientAlreadyExistsException();
        //    }

        //    this.AddPrivateClients(cometClient.PrivateToken, new InProcCometClient()
        //    {
        //        CometClient = cometClient
        //    });
        //    //Debug.WriteLine("publicClients.Add clientid:" + cometClient.PrivateToken + "!");

        //}

        /// <summary>
        /// Get the messages for a specific client
        /// </summary>
        /// <param name="clientPrivateToken"></param>
        /// <param name="lastMessageId"></param>
        /// <returns></returns>
        public CometMessage[] GetMessages(string clientPrivateToken, long lastMessageId)
        {
            //BLL.Loger.Log4Net.Info(string.Format("方法InProcCometStateProvider.GetMessages,获取{0},大于{1}的消息。", clientPrivateToken, lastMessageId));

            //if (!this.privateCUClients.ContainsKey(clientPrivateToken))
            //{
            //    return null;
            //}

            //InProcCometClient cometClient = this.privateCUClients[clientPrivateToken];
            //if (cometClient == null)
            //{
            //    return null;
            //}
            //CometMessage mess;
            //CometMessage cometMessage = null;

            //防止消息丢失，消息攒够5条删除一次
            //var allMessagesDelete = cometClient.queueMessagesDeleted.Keys.ToArray();
            //CometMessage cMDelete = null;
            //for (int i = 0; i < allMessagesDelete.Length; i++)
            //{
            //    cMDelete = null;
            //    cMDelete = cometClient.queueMessagesDeleted[allMessagesDelete[i]];
            //    if (allMessagesDelete[i] > lastMessageId && cMDelete != null)
            //    {
            //        listCometMessages.Add(cMDelete);
            //    }
            //}
            //if (allMessagesDelete.Length > 0)
            //{
            //    cometClient.queueMessagesDeleted.Clear();
            //}
            //while (cometClient.queueMessagesDeleted.TryDequeue(out cometMessage))
            //{
            //    if (cometMessage.MessageId > lastMessageId)
            //    {
            //        listCometMessages.Add(cometMessage);
            //    }
            //}
            List<CometMessage> listCometMessages = new List<CometMessage>();
            if (dicMessages.ContainsKey(clientPrivateToken) && !dicMessages[clientPrivateToken].IsEmpty)
            {
                var resultList = dicMessages[clientPrivateToken].ToArray();

                try
                {
                    //DicIISServices[keyValuePair.Key].ReceiveMessage(resultList.Select(s => s.Value).ToArray());
                    foreach (KeyValuePair<long, CometMessage> valuePair in resultList)
                    {
                        CometMessage nT = null;
                        if (dicMessages[clientPrivateToken].TryRemove(valuePair.Key, out  nT))
                        {
                            if (nT != null)
                            {
                                if (nT.MessageId > lastMessageId)
                                {
                                    listCometMessages.Add(nT);
                                }
                            }
                        }
                        else
                        {
                            BLL.Loger.Log4Net.Error(string.Format("方法InProcCometStateProvider.GetMessages，对象{0},移除key{1}失败,LastMessageId：{2}", clientPrivateToken, valuePair.Key, lastMessageId));
                        }

                    }
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Error(string.Format("InProcCometStateProvider.GetMessages中出错, 对象{0},LastMessageId：{1},错误信息：{2}", clientPrivateToken, lastMessageId, ex.Message));
                    return null;
                }

            }
            return listCometMessages.ToArray();
        }


        /*
        /// <summary>
        /// Send a message to a specific client
        /// </summary>
        /// <param name="clientPublicToken"></param>
        /// <param name="name"></param>
        /// <param name="contents"></param>
        public void SendMessage(string clientPrivateToken, string name, string contents, string IISIP)
        {
            if (string.IsNullOrEmpty(clientPrivateToken))
                throw new ArgumentNullException("clientPublicToken");

            if (contents == null)
                throw new ArgumentNullException("contents");
            var message = new CometMessage();
            message.Contents = contents;
            message.Name = name;
            message.ProcessTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            //Debug.WriteLine(string.Format("InProcCometStateProvider开始发送消息到对象{0}", DateTime.Now.ToString("G")));
            message.IISIP = IISIP;
            if (this.privateCUClients.ContainsKey(clientPrivateToken))
            {
                InProcCometClient cometClient = this.privateCUClients[clientPrivateToken];
                message.MessageId = Interlocked.Increment(ref cometClient.NextMessageId);
                cometClient.AddMessage(message.MessageId, message);
            }
        }
        */
        public void AddMessage(string clientPrivateToken, CometMessage message)
        {
            try
            {
                if (!dicMessages.ContainsKey(clientPrivateToken))
                {
                    dicMessages.TryAdd(clientPrivateToken, new ConcurrentDictionary<long, CometMessage>());
                }
                dicMessages[clientPrivateToken].TryAdd(message.MessageId, message);

            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info(string.Format("方法InProcCometStateProvider.AddMessage中出错，错误信息", ex.Message));
            }
        }
        /// <summary>
        /// Send a message to all the clients
        /// </summary>
        /// <param name="name"></param>
        /// <param name="contents"></param>
        public void SendMessage(string name, string contents)
        {
            /*
            if (contents == null)
                throw new ArgumentNullException("contents");

            //lock (state)
            //{
            _rwLockSlim.EnterUpgradeableReadLock();
            foreach (InProcCometClient cometClient in privateClients.Values)
            {
                // ok, stick the message in the array
                CometMessage message = new CometMessage();

                message.Contents = contents;
                message.Name = name;
                message.MessageId = cometClient.NextMessageId;

                cometClient.AddMessage(message.MessageId, message);
                //  increment
                //cometClient.NextMessageId++;
                //_rwLockSlim.EnterWriteLock();
                //cometClient.Messages.Add(message.MessageId, message);
                //_rwLockSlim.ExitWriteLock();
            }
            _rwLockSlim.ExitUpgradeableReadLock();
            //}
            */
        }

        //public void SendMessage(CometMessage cometMessage)
        //{
        //    if (cometMessage == null) { return; }

        //    if (this.privateCUClients.ContainsKey(cometMessage.ToToken))
        //    {
        //        InProcCometClient cometClient = this.privateCUClients[cometMessage.ToToken];
        //        cometClient.CometClient.LastReceiveMsgTime = DateTime.Now;
        //        cometClient.AddMessage(cometMessage);
        //    }
        //}

        #endregion

        #region 自定义方法
        ///// <summary>
        ///// 设置坐席状态
        ///// </summary>
        ///// <param name="clientPrivateToken"></param>
        ///// <param name="state"></param>
        //public void SetAgentState(string clientPrivateToken, AgentStatus state)
        //{
        //    var client = GetCometClient(clientPrivateToken);
        //    if (client != null)
        //    {
        //        client.Status = state;
        //        client.RecordAgentStatus((int)state);
        //    }
        //}

        ///// <summary>
        ///// 移除指定坐席下的网友，在坐席关闭再聊网友时使用
        ///// </summary>
        ///// <param name="wyToken"></param>
        ///// <param name="zxToken"></param>
        //public void RemoveSingleUser(string wyToken, string zxToken)
        //{
        //    var zxComet = GetCometClient(zxToken);
        //    if (zxComet != null && GetCometClient(wyToken) != null)
        //    {
        //        zxComet.RemoveTalkUser(wyToken);

        //        //从内存中删除网友
        //    }
        //}


        #endregion

        public void Dispose()
        {
            //this.privateCUClients.Clear();
        }



    }
}
