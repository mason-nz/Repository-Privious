﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.DSC.IM2014.Core
{
    /// <summary>
    /// This interface can be implemented to provide a custom state provider
    /// for the CometStateManager class.  Typical examples may be using SqlServer
    /// to enable the operation over a server farm
    /// </summary>
    public interface ICometStateProvider
    {
        /// <summary>
        /// Implementation of this method should store the cometClient instance in some sort
        /// of cache (eg Memory, Db etc..)
        /// </summary>
        /// <param name="cometClient"></param>
        void InitializeClient(CometClient cometClient);
        /// <summary>
        /// Imeplementation of this method should return all the messages that are queued
        /// for a specific client, it is only interested in messages that have a greater id than
        /// lastMessageId
        /// </summary>
        /// <param name="clientPrivateToken"></param>
        /// <param name="lastMessageId"></param>
        /// <returns></returns>
        CometMessage[] GetMessages(string clientPrivateToken, long lastMessageId);
        /// <summary>
        /// Implementation of this method should queue a message for the specific client
        /// </summary>
        /// <param name="clientPublicToken"></param>
        /// <param name="name"></param>
        /// <param name="contents"></param>
        void SendMessage(string clientPublicToken, string name, object contents);
        /// <summary>
        /// Implementation of this method should queue a message for all the clients
        /// </summary>
        /// <param name="name"></param>
        /// <param name="contents"></param>
        void SendMessage(string name, object contents);
        /// <summary>
        /// Implementation of this method should return a specific comet client
        /// </summary>
        /// <param name="clientPrivateToken"></param>
        /// <returns></returns>
        CometClient GetCometClient(string clientPrivateToken);
        /// <summary>
        /// Implementation of this method should remove a client from the cache
        /// </summary>
        /// <param name="clientPrivateToken"></param>
        void KillIdleCometClient(string clientPrivateToken);
        #region 自定义方法
        /// <summary>
        /// 设置坐席状态
        /// </summary>
        /// <param name="clientPrivateToken"></param>
        /// <param name="state"></param>
        void SetAgentState(string clientPrivateToken, int state);
        /// <summary>
        /// 根据网友标识把其在在聊网友中移除
        /// </summary>
        /// <param name="clientPrivateToken"></param>
        void RemoveDialogComeetByUserID(string clientPrivateToken);
        #endregion
    }
}
