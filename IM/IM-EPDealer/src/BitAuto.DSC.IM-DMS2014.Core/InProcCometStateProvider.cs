﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BitAuto.DSC.IM_DMS2014.Core.Messages;
using System.Diagnostics;
using BitAuto.DSC.IM_DMS2014.Entities;

namespace BitAuto.DSC.IM_DMS2014.Core
{
    /// <summary>
    /// Class InProcCometStateProvider
    /// 
    /// This class provides an implementation of ICometStateProvider that keeps the 
    /// information in memory.  This provider is not scalable as it will not run on a server
    /// farm but demonstrates how you should implemement the provider.
    /// </summary>
    public class InProcCometStateProvider : ICometStateProvider
    {
        /// <summary>
        /// Private class which holds the state of each connected client
        /// </summary>
        private class InProcCometClient
        {
            public CometClient CometClient;
            public Dictionary<long, CometMessage> dicMessages = new Dictionary<long, CometMessage>();
            public Dictionary<long, CometMessage> dicMessagesDeleted = new Dictionary<long, CometMessage>();
            public long NextMessageId = 1;
            private object lockObj = new object();

            public void AddMessage(long key, CometMessage value)
            {
                lock (lockObj)
                {
                    NextMessageId++;
                    this.dicMessages.Add(key, value);
                }
            }
            public void ClearMessageDeleted()
            {
                lock (lockObj)
                {
                    this.dicMessagesDeleted.Clear();
                }
            }
            public void AddMessageDeleted(long key, CometMessage value)
            {
                lock (lockObj)
                {
                    this.dicMessagesDeleted.Add(key, value);
                }
            }
            public Dictionary<long, CometMessage> GetAllMessagesDeleted()
            {
                lock (lockObj)
                {
                    return this.dicMessagesDeleted;
                }
            }

            public void RemoveMessage(long key)
            {
                lock (lockObj)
                {
                    this.dicMessages.Remove(key);
                }
            }

            public Dictionary<long, CometMessage> GetAllMessages()
            {
                lock (lockObj)
                {
                    return dicMessages;
                }
            }
        }

        /// <summary>
        /// Cache of clients
        /// </summary>
        //private Dictionary<long, InProcCometClient> publicClients = new Dictionary<long, InProcCometClient>();
        private Dictionary<string, InProcCometClient> privateClients = new Dictionary<string, InProcCometClient>();


        #region lock

        private static object state = new object();

        private void AddPrivateClients(string clientPrivateToken, InProcCometClient value)
        {
            lock (state)
            {
                this.privateClients.Add(clientPrivateToken, value);
            }

            BLL.Loger.Log4Net.Info(string.Format("新增comet对象：{0},类型：{1},", clientPrivateToken, value.CometClient.Type));

            //add by qizq 2015-1-7 加监控入库日志
            Entities.UserActionLog model = new UserActionLog();
            model.CreateTime = System.DateTime.Now;
            if (value.CometClient.Type == 1)
            {
                int _CreateUserID = 0;
                if (int.TryParse(clientPrivateToken, out _CreateUserID))
                {
                    model.CreateUserID = _CreateUserID;
                }
                model.OperUserType = value.CometClient.Type;

                //坐席登录
                model.LogInType = 6;
            }
            else
            {
                //客户登录
                model.LogInType = 4;
                model.TrueName = value.CometClient.MemberName;
                model.OperUserType = value.CometClient.Type;
            }
            model.LogInfo = string.Format("新增comet对象：{0},对象类型：{1},", clientPrivateToken, value.CometClient.Type == 1 ? "客服" : "经销商");
            BLL.UserActionLog.Instance.Insert(model);


        }

        public void RemovePrivateClients(string clientPrivateToken)
        {
            var tComnet = GetCometClient(clientPrivateToken);
            if (tComnet != null)
            {
                BLL.Loger.Log4Net.Info(string.Format("移除comet对象：{0},类型：{1},最后发消息时间:{2},最后长连接时间:{3}", clientPrivateToken,
                    tComnet.Type, tComnet.SendMessageTime.ToString("O"), tComnet.LastRequestTime.ToString("O")));


                //add by qizq 2015-1-7 加监控入库日志
                Entities.UserActionLog model = new UserActionLog();
                model.CreateTime = System.DateTime.Now;
                if (tComnet.Type == 1)
                {
                    int _CreateUserID = 0;
                    if (int.TryParse(clientPrivateToken, out _CreateUserID))
                    {
                        model.CreateUserID = _CreateUserID;
                    }
                    model.OperUserType = tComnet.Type;

                    //坐席退出
                    model.LogInType = 7;
                }
                else
                {
                    //客户退出
                    model.LogInType = 5;
                    model.OperUserType = tComnet.Type;
                    model.TrueName = tComnet.MemberName;
                }
                model.LogInfo = string.Format("移除comet对象：{0},对象类型：{1},最后发消息时间:{2},最后长连接时间:{3}", clientPrivateToken,
                    tComnet.Type == 1 ? "客服" : "经销商", tComnet.SendMessageTime.ToString("O"), tComnet.LastRequestTime.ToString("O"));
                BLL.UserActionLog.Instance.Insert(model);

            }

            lock (state)
            {
                this.privateClients.Remove(clientPrivateToken);
            }


            //BLL.Loger.Log4Net.Info("移除comet对象：" + clientPrivateToken);
        }

        public List<CometClient> GetAllCometClients()
        {
            lock (state)
            {
                return privateClients.Select(tC => tC.Value.CometClient).ToList();
            }

        }

        public CometClient GetCometClient(string clientPrivateToken)
        {
            lock (state)
            {
                if (privateClients.ContainsKey(clientPrivateToken))
                {
                    return this.privateClients[clientPrivateToken].CometClient;
                }
                return null;
            }
        }

        #endregion


        /*
        #region ReaderWriterLockSlim


        private ReaderWriterLockSlim _rwLockSlim = new ReaderWriterLockSlim();


        public void RemovePrivateClients(string clientPrivateToken)
        {
            _rwLockSlim.EnterWriteLock();
            try
            {
                this.privateClients.Remove(clientPrivateToken);
            }
            finally
            {
                _rwLockSlim.ExitWriteLock();
            }
        }

        private void AddPrivateClients(string clientPrivateToken, InProcCometClient value)
        {
            _rwLockSlim.EnterWriteLock();
            try
            {
                this.privateClients.Add(clientPrivateToken, value);
            }
            finally
            {
                _rwLockSlim.ExitWriteLock();
            }
        }


        public List<CometClient> GetAllCometClients()
        {
            if (_rwLockSlim.TryEnterReadLock(100))
            {
                try
                {
                    if (privateClients.Count > 0)
                    {
                        return privateClients.Select(tC => tC.Value.CometClient).ToList();
                    }
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Error("GetAllCometClients:" + ex.Message);
                }

                finally
                {
                    _rwLockSlim.ExitReadLock();
                }
            }
            return new List<CometClient>();
        }

        public CometClient GetCometClient(string clientPrivateToken)
        {
            //_rwLockSlim.EnterReadLock();
            if (_rwLockSlim.TryEnterReadLock(100))
            {
                try
                {
                    if (privateClients.Count > 0 && privateClients.ContainsKey(clientPrivateToken))
                    {
                        return this.privateClients[clientPrivateToken].CometClient;
                    }
                    return null;

                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Error("GetCometClient:" + ex.Message);
                }
                finally
                {
                    _rwLockSlim.ExitReadLock();
                }
            }            
            return null;
        }



        #endregion
        */
        #region ICometStateProvider Members

        /// <summary>
        /// Store the new client in memory
        /// </summary>
        /// <param name="cometClient"></param>
        public void InitializeClient(CometClient cometClient)
        {
            if (cometClient == null)
                throw new ArgumentNullException("cometClient");

            if (privateClients.ContainsKey(cometClient.PrivateToken))
                throw CometException.CometClientAlreadyExistsException();

            //lock (state)
            //{

            this.AddPrivateClients(cometClient.PrivateToken, new InProcCometClient()
            {
                CometClient = cometClient
            });
            //Debug.WriteLine("publicClients.Add clientid:" + cometClient.PrivateToken + "!");
            //}
        }

        /// <summary>
        /// Get the messages for a specific client
        /// </summary>
        /// <param name="clientPrivateToken"></param>
        /// <param name="lastMessageId"></param>
        /// <returns></returns>
        public CometMessage[] GetMessages(string clientPrivateToken, long lastMessageId)
        {
            if (string.IsNullOrEmpty(clientPrivateToken))
                throw new ArgumentNullException("clientPrivateToken");
            if (!privateClients.ContainsKey(clientPrivateToken))
            {
                System.Diagnostics.Debug.WriteLine("CometClientDoesNotExistException");
                return null;
            }
            InProcCometClient cometClient = privateClients[clientPrivateToken];
            List<long> toDelete = new List<long>();
            List<CometMessage> listCometMessages = new List<CometMessage>();
            var allMessages = cometClient.GetAllMessages();
            var allMessagesDelete = cometClient.GetAllMessagesDeleted();
            foreach (long key in allMessagesDelete.Keys)
            {
                if (key > lastMessageId)
                    listCometMessages.Add(allMessagesDelete[key]);
            }
            if (allMessagesDelete.Keys.Count > 0)
            {
                cometClient.ClearMessageDeleted();
            }
            foreach (long key in allMessages.Keys)
            {
                toDelete.Add(key);
                //把要删除的消息先保存在删除副本里
                cometClient.AddMessageDeleted(key, allMessages[key]);
            }
            foreach (long key in toDelete)
            {
                listCometMessages.Add(allMessages[key]);
                cometClient.RemoveMessage(key);
            }
            return listCometMessages.ToArray();


        }

        /// <summary>
        /// Send a message to a specific client
        /// </summary>
        /// <param name="clientPublicToken"></param>
        /// <param name="name"></param>
        /// <param name="contents"></param>
        public void SendMessage(string clientPrivateToken, string name, object contents)
        {
            if (string.IsNullOrEmpty(clientPrivateToken))
                throw new ArgumentNullException("clientPublicToken");

            if (contents == null)
                throw new ArgumentNullException("contents");


            if (!privateClients.ContainsKey(clientPrivateToken))
                throw CometException.CometClientDoesNotExistException();

            InProcCometClient cometClient = privateClients[clientPrivateToken];

            // ok, stick the message in the array
            var message = new CometMessage();

            message.Contents = contents;
            message.Name = name;
            message.MessageId = cometClient.NextMessageId;
            message.ProcessTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            cometClient.AddMessage(message.MessageId, message);
        }


        /// <summary>
        /// Send a message to all the clients
        /// </summary>
        /// <param name="name"></param>
        /// <param name="contents"></param>
        public void SendMessage(string name, object contents)
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


        /// <summary>
        /// Remove an idle client from the memory
        /// </summary>
        /// <param name="clientPrivateToken"></param>
        public void KillIdleCometClient(string clientPrivateToken)
        {
            //if (!this.privateClients.ContainsKey(clientPrivateToken))
            //    throw CometException.CometClientDoesNotExistException();

            //  get the client
            //InProcCometClient ipCometClient = this.privateClients[clientPrivateToken];


            this.RemovePrivateClients(clientPrivateToken);

            //this.publicClients.Remove(ipCometClient.CometClient.PublicToken);
            //Debug.WriteLine("privateClients.Remove clientid:" + ipCometClient.CometClient.PrivateToken + "!");
            //对象移除时把其对应的坐席分配表的结束时间更新，更新条件是agentid或userid等于当前人标识，并且结束时间是'9999-12-31' add by qizq 2014-3-11
            //BLL.AllocationAgent.Instance.UpdateEndTime(ipCometClient.CometClient.PrivateToken);


        }

        #endregion

        #region 自定义方法
        /// <summary>
        /// 设置坐席状态
        /// </summary>
        /// <param name="clientPrivateToken"></param>
        /// <param name="state"></param>
        public void SetAgentState(string clientPrivateToken, AgentStatus state)
        {
            var client = GetCometClient(clientPrivateToken);
            if (client != null)
            {
                this.privateClients[clientPrivateToken].CometClient.Status = state;
            }
            //if (this.privateClients.ContainsKey(clientPrivateToken))
            //{
            //    this.privateClients[clientPrivateToken].CometClient.Status = state;
            //}
        }

        /*
        /// <summary>
        /// 历史方法待移除。 当网友空闲时间已到根据网友标识，在与其聊天的坐席的在聊网友里将其移除,给坐席发送网友离席消息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="contents"></param>
        /// 
        public void RemoveDialogComeetByUserID(string clientPrivateToken)
        {
            //因为是网友端先掉线，所以首先将网友设置为离线，系统将不会给网友发离线消息
            GetCometClient(clientPrivateToken).Status = AgentStatus.Leaveline;
            lock (state)
            {
                foreach (InProcCometClient cometClient in privateClients.Values)
                {
                    //如果实体是坐席，遍历其在聊网友，将这个网友在在聊网友中移除
                    if (cometClient.CometClient.Type == (Int32)Entities.UserType.Agent)
                    {

                        cometClient.CometClient.RemoveTalkUser(Convert.ToInt64(clientPrivateToken));
                        //string[] talklist = cometClient.CometClient.TalkUserList;
                        //for (int i = 0; i < talklist.Length; i++)
                        //{
                        //    if (talklist[i] == clientPrivateToken)
                        //    {
                        //        //通知坐席网友离线
                        //        ChatMessage chatMessage = new ChatMessage();
                        //        chatMessage.From = clientPrivateToken;
                        //        chatMessage.Message = "离线";
                        //        string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MLline);
                        //        SendMessage(cometClient.CometClient.PrivateToken, messagetype, chatMessage);
                        //        //

                        //        cometClient.CometClient.RemoveUser(clientPrivateToken);
                        //        cometClient.CometClient.DialogCount = cometClient.CometClient.DialogCount - 1;
                        //        break;
                        //    }
                        //}
                    }
                }
            }
        }
        */
        /// <summary>
        /// 移除指定坐席下的网友，在坐席关闭再聊网友时使用
        /// </summary>
        /// <param name="wyToken"></param>
        /// <param name="zxToken"></param>
        public void RemoveSingleUser(string wyToken, string zxToken)
        {
            var zxComet = GetCometClient(zxToken);
            if (zxComet != null && GetCometClient(wyToken) != null)
            {
                zxComet.RemoveTalkUser(Convert.ToInt32(wyToken));

                //从内存中删除网友
            }
        }


        #endregion

        public void Dispose()
        {
            //_rwLockSlim.Dispose();
            privateClients.Clear();
        }
    }
}