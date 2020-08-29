using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using BitAuto.DSC.IM_2015.Core;
using BitAuto.DSC.IM_2015.Core.Messages;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.MainInterface;
using Newtonsoft.Json;
using System.Threading;

namespace BitAuto.DSC.IM_2015.MainWindowsService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class IMServices : IIMServices, IDisposable
    {
        public static MainIMService globalManager;


        public IMServices()
        {
            //globalManager = baseManager;
        }

        public ProxyAgentClient[] GetAllAgents()
        {
            return globalManager.DicAllAgents.Values.ToArray();
        }

        public ProxyNetFriend[] GetAllNetFriends()
        {
            return globalManager.DicAllNetFriends.Values.ToArray();
        }
        //更新网友实体，坐席回复时间
        public void UpdateNetFriendAgentReplayTime(string NetFriendKey)
        {
            if (globalManager.DicAllNetFriends != null)
            {
                if (globalManager.DicAllNetFriends.ContainsKey(NetFriendKey))
                {
                    ProxyNetFriend netfriend = globalManager.DicAllNetFriends[NetFriendKey];
                    if (netfriend != null)
                    {
                        netfriend.ConverSTime = DateTime.Now;
                        netfriend.CreateTime = DateTime.Now;
                    }
                }
            }
        }
        public ProxyAgentClient GetAgentByToken(int agentId)
        {
            if (globalManager.DicAllAgents.ContainsKey(agentId))
            {
                return globalManager.DicAllAgents[agentId];
            }
            return null;
        }

        public bool IsAgentExists(int agentId)
        {
            return globalManager.DicAllAgents.ContainsKey(agentId);
        }

        /// <summary>
        /// 添加坐席，
        /// </summary>
        /// <param name="agent"></param>
        /// <returns>
        /// 0 成功，-1：已经存在，-2：添加失败
        /// </returns>
        public int AddAgent(ProxyAgentClient agent)
        {
            if (agent == null) { return -2; }

            if (globalManager.DicAllAgents.ContainsKey(agent.AgentID))
            {
                return -1;
            }
            agent.TalkUserList = new ConcurrentDictionary<string, ProxyNetFriend>();
            if (globalManager.DicAllAgents.TryAdd(agent.AgentID, agent))
            {
                agent.LastActiveTime = DateTime.Now;
                BLL.Loger.Log4Net.Info(string.Format("AddAgent添加坐席{0}", agent.AgentToken));
                return 0;
            }
            else
            {
                return -2;
            }

        }

        public void RemoveAgent(int agentId, int closeType)
        {
            ProxyAgentClient agent;
            if (globalManager.DicAllAgents.ContainsKey(agentId))
            {
                globalManager.DicAllAgents.TryRemove(agentId, out agent);
                BLL.Loger.Log4Net.Info(string.Format("RemoveAgent移除坐席{0}", agentId));
                if (agent == null) return;
                //移除网友在聊网友。
                RemoveAgentTalkList(agent,closeType);
            }

            agent = null;

        }

        public CometMessage[] SwitchMessage(string ip, CometMessage[] messages)
        {
            //1.对于传来的消息，调用Receiver传送到相匹配IIS服务器消息池中
            if (messages != null && messages.Length > 0)
            {
                for (int i = 0; i < messages.Length; i++)
                {
                    globalManager.MainMessageReceiver.EnqueueToProcess(messages[i]);
                }
            }

            //2.从指定IP消息队列中取所有消息            
            if (globalManager.DicIPMessagePool.ContainsKey(ip) && !globalManager.DicIPMessagePool[ip].IsEmpty)
            {
                var resultList = globalManager.DicIPMessagePool[ip].ToArray();
                try
                {
                    //DicIISServices[keyValuePair.Key].ReceiveMessage(resultList.Select(s => s.Value).ToArray());
                    foreach (KeyValuePair<long, CometMessage> valuePair in resultList)
                    {
                        CometMessage nT = null;
                        globalManager.DicIPMessagePool[ip].TryRemove(valuePair.Key, out  nT);
                    }

                    return resultList.Select(s => s.Value).ToArray();
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Error(string.Format("IMServices.SwitchMessage中出错,                 {0}", ex.Message));
                    return null;
                }
            }






            return null;
            //return resultList.ToArray();
        }

        /// <summary>
        /// 设置坐席状态
        /// </summary>
        /// <param name="agentid"></param>
        /// <param name="status"></param>
        /// <returns> 0 成功，-1：不存在，-2：添加失败</returns>
        public int SetAgentStatus(int agentid, int status)
        {
            if (globalManager.DicAllAgents.ContainsKey(agentid))
            {
                BLL.Loger.Log4Net.Info(string.Format("SetAgentStatus修改坐席{0}状态为{1}", agentid, status));
                globalManager.DicAllAgents[agentid].Status = status;
                globalManager.DicAllAgents[agentid].LastActiveTime = DateTime.Now;
                //如果坐席设置为离线时，移除坐席下对话列表
                if (status == 2)
                {
                    RemoveAgentTalkList(globalManager.DicAllAgents[agentid],(int)Entities.CloseType.AgentClose);
                }
                return 0;
            }
            else
            {
                return -1;
            }

        }

        /// <summary>
        /// 要检测的前端网友状态，同时完成Core再聊网友的自检(Core中移除多余的再聊网友)
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public string CheckStates(int agentId, string tokens)
        {
            var strAgentState = IsAgentExists(agentId) ? " ,\"astatus\":1 "
                : " ,\"astatus\":0 ";

            string msg = string.Empty;
            if (!string.IsNullOrWhiteSpace(tokens))
            {
                msg = globalManager.CheckState(agentId, tokens.Split(','));
            }
            else
            {
                msg = "[]";
            }


            //msg = "{\"result\":\"sendok\",\"data\":" + msg + "}";
            return msg + strAgentState;
        }

        public int AddNetFriendWaitList(ProxyNetFriend netFriend)
        {
            BLL.Loger.Log4Net.Info(string.Format("AddNetFriendWaitList网友{0}进入队列", netFriend.Token));

            netFriend.LastActiveTime = DateTime.Now;
            return globalManager.MainAllocThreadMonitor.EnQueueWaitList(netFriend.BusinessLines, netFriend);
        }

        /// <summary>
        /// 从等待队列中移除
        /// </summary>
        /// <param name="token"></param>
        public void RemoveNetFriendFromWaitList(string strBusinessLine, string token)
        {
            BLL.Loger.Log4Net.Info(string.Format("RemoveNetFriendFromWaitList网友{0}从等待队列中移除", token));
            globalManager.MainAllocThreadMonitor.RemoveWaitList(strBusinessLine, token);
            //ProxyNetFriend netFriend;
            //if (globalManager.DicAllNetFriends.ContainsKey(token))
            //{
            //    globalManager.DicAllNetFriends.TryRemove(token, out netFriend);
            //    netFriend = null;
            //}
        }

        /// <summary>
        /// 移除网友,主动移除，被动移除
        /// </summary>
        /// <param name="token"></param>
        /// <param name="sendNetFrinedMsg">给网友发离线消息</param>
        /// <param name="sendAgentMsg">给坐席发离线消息</param>
        public void RemoveNetFriend(string token, bool sendNetFrinedMsg, bool sendAgentMsg, int CloseType)
        {
            try
            {
                //从对象列表中移除网友
                var netFriend = RemoveNetFriendFromList(token);
                if (netFriend == null) { return; }
                netFriend.CloseType = CloseType;
                BLL.Loger.Log4Net.Info(string.Format("RemoveNetFriend 坐席{0}移除网友{1}", netFriend.AgentToken, token));

                if (sendNetFrinedMsg)
                {
                    SendLeaveLineMsg(netFriend.AgentToken, netFriend.Token, string.Format("坐席{0}已离线.", netFriend.AgentToken), netFriend.IISIP, netFriend.CSID);
                }

                if (sendAgentMsg)
                {
                    SendLeaveLineMsg(netFriend.AgentToken, netFriend.AgentToken, string.Format("网友{0}已离线.", netFriend.NetFName), netFriend.IISIP, netFriend.CSID);
                }

                //更新会话状态
                UpdateConversation(netFriend);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("RemoveNetFriend                 {0}", ex.Message));
            }
        }

        /// <summary>
        /// 根据业务线取业务线排队网友
        /// </summary>
        /// <param name="businessLine"></param>
        /// <returns></returns>
        public List<ProxyNetFriend> GetCometClientsByBusinessLines(string businessLines)
        {
            try
            {
                List<ProxyNetFriend> clients = new List<ProxyNetFriend>();
                if (businessLines.IndexOf(',') > -1)
                {
                    string[] businesses = businessLines.Split(',');
                    for (int i = 0; i < businesses.Length; i++)
                    {
                        List<ProxyNetFriend> netfrinds = new List<ProxyNetFriend>();
                        netfrinds = globalManager.MainAllocThreadMonitor.GetCometClientsByBusinessLine(businesses[i]);
                        if (netfrinds != null && netfrinds.Count > 0)
                        {
                            for (int n = 0; n < netfrinds.Count; n++)
                            {
                                clients.Add(netfrinds[n]);
                            }
                        }
                    }
                }
                else
                {
                    return globalManager.MainAllocThreadMonitor.GetCometClientsByBusinessLine(businessLines);
                }
                return clients;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("GetCometClientsByBusinessLines                 {0}", ex.Message));
                return null;
            }
        }




        #region 内部函数

        private void SendLeaveLineMsg(string fromToken, string toToken, string messge, string ip, int csid)
        {
            ChatMessage chatMessageWy = new ChatMessage();
            chatMessageWy.From = fromToken;
            chatMessageWy.Message = messge;
            chatMessageWy.Time = DateTime.Now;
            chatMessageWy.CsID = csid;

            CometMessage cometMessage = new CometMessage()
            {
                Name = "MLline",
                Contents = BLL.Util.DataContractObject2Json(chatMessageWy, typeof(ChatMessage)),//JsonConvert.SerializeObject(chatMessageWy),
                FromToken = fromToken,
                ToToken = toToken,
                IISIP = ip
            };
            BLL.Loger.Log4Net.Info(string.Format("SendLeaveLineMsg          from:{0},to:{1},csid:{2}", fromToken, toToken, csid));
            globalManager.SendMessage(ip, cometMessage);
            //BLL.Loger.Log4Net.Info(string.Format("SendLeaveLineMsg “{0}”发送离线消息给“{1}”", fromToken, toToken));

        }


        private void RemoveAgentTalkList(ProxyAgentClient agent, int closeType)
        {
            try
            {
                BLL.Loger.Log4Net.Info(string.Format("RemoveAgentTalkList移除坐席{0}所有在聊网友", agent.AgentToken));
                agent.LastActiveTime = DateTime.Now;
                string messagetype = BLL.Util.GetEnumOptText(typeof(Entities.MessageType), (int)Entities.MessageType.MLline);

                //通知所有网友，坐席已离线
                foreach (ProxyNetFriend netFriend in agent.TalkUserList.Values.ToArray())
                {
                    //当网友坐席不在统一服务时，给网友发送离线消息。
                    if (!string.Equals(netFriend.IISIP, agent.IISIP, StringComparison.CurrentCultureIgnoreCase))
                    {
                        SendLeaveLineMsg(agent.AgentToken, netFriend.Token, string.Format("坐席{0}已离线.", agent.AgentID), netFriend.IISIP, netFriend.CSID);
                    }
                    netFriend.CloseType = closeType;
                    //更新对话
                    UpdateConversation(netFriend);
                    //把网友从会话清单中移除
                    if (globalManager.DicAllNetFriends.ContainsKey(netFriend.Token))
                    {
                        ProxyNetFriend netFrienddelete;
                        globalManager.DicAllNetFriends.TryRemove(netFriend.Token, out netFrienddelete);
                        netFrienddelete = null;
                    }
                }
                agent.TalkUserList.Clear();
                agent = null;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("RemoveAgentTalkList                 {0}", ex.Message));
            }
        }


        private void UpdateConversation(ProxyNetFriend netFriend)
        {
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
            netFriend.CSID = 0;
            //BLL.Conversations.Instance.CallBackUpdate(csEntity);
            //更新数据库改为异步
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                var cE = obj as Entities.Conversations;
                if (cE == null)
                {
                    return;
                }
                try
                {
                    BLL.Loger.Log4Net.Info(string.Format("UpdateConversation更新网友{0}会话状态", netFriend.Token));
                    BLL.Conversations.Instance.CallBackUpdate(cE);
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Error(string.Format("更新会话记录错误：CSID：{0},错误信息:{1}", cE.CSID, ex.Message));
                }
            }, csEntity);
        }


        //从网友列表，即对应网友的坐席的在聊列表中删除。
        private ProxyNetFriend RemoveNetFriendFromList(string token)
        {
            ProxyNetFriend netFriend = null;
            if (globalManager.DicAllNetFriends.ContainsKey(token))
            {
                globalManager.DicAllNetFriends.TryRemove(token, out netFriend);
                if (globalManager.DicAllAgents.ContainsKey(netFriend.AgentID))
                {
                    BLL.Loger.Log4Net.Info(string.Format("RemoveNetFriendFromList,坐席{0}移除在聊网友{1}", netFriend.AgentToken, netFriend.Token));
                    globalManager.DicAllAgents[netFriend.AgentID].TalkUserList.TryRemove(token, out netFriend);
                }
            }
            return netFriend;
        }

        #endregion

        /// <summary>
        /// 转移网友
        /// </summary>
        /// <param name="fromAgent"></param>
        /// <param name="toAgent"></param>
        /// <param name="netFriendToken"></param>
        /// <returns>
        /// -1：没找到对象
        /// </returns>
        public int TransferNetFriend(int fromAgent, int toAgent, string netFriendToken)
        {
            try
            {
                if (!globalManager.DicAllAgents.ContainsKey(toAgent) || !globalManager.DicAllNetFriends.ContainsKey(netFriendToken))
                {
                    return -1;
                }

                BLL.Loger.Log4Net.Info(string.Format("TransferNetFriend,坐席:{0}转移在聊网友:{1}给坐席:{2}", fromAgent, netFriendToken, toAgent));

                ProxyAgentClient toAgentClient = globalManager.DicAllAgents[toAgent];
                ProxyAgentClient fromAgentClient = globalManager.DicAllAgents[fromAgent];
                ProxyNetFriend netFriend = globalManager.DicAllNetFriends[netFriendToken];

                toAgentClient.LastActiveTime = DateTime.Now;
                fromAgentClient.LastActiveTime = DateTime.Now;
                netFriend.LastActiveTime = DateTime.Now;

                netFriend.IsTurnOut = true;

                //从当前在聊坐席中移除网友,并发送网友离线消息                

                fromAgentClient.TalkUserList.TryRemove(netFriendToken, out netFriend);

                SendLeaveLineMsg(netFriendToken, fromAgentClient.AgentToken, string.Format("网友{0}已转移.", netFriendToken), fromAgentClient.IISIP, netFriend.CSID);
                //转移网友
                netFriend.CloseType = (int)Entities.CloseType.TurnNetFrind;
                //更新会话状态
                UpdateConversation(netFriend);


                //添加网友到新坐席会话表
                toAgentClient.TalkUserList.TryAdd(netFriendToken, netFriend);
                //与新坐席创建会话
                globalManager.MainAllocThreadMonitor.CreateConversation(toAgentClient, netFriend, 1);

                #region 记录操作日志

                UserActionLog SourceLog = new UserActionLog()
                {
                    CreateTime = DateTime.Now,
                    CreateUserID = -2,
                    IP = string.Empty,
                    OperUserType = 1,
                    LogInType = 10, //转出
                    LogInfo = string.Format(" 从坐席 {0} 转出网友 {1} 给新坐席{2}. ", fromAgent, netFriendToken, toAgentClient)
                };
                UserActionLog TagertLog = new UserActionLog()
                {
                    CreateTime = DateTime.Now,
                    CreateUserID = -2,
                    IP = string.Empty,
                    OperUserType = 1,
                    LogInType = 9, //转入
                    LogInfo = string.Format(" 新坐席 {0} 从坐席{2}转入网友 {1} 给. ", toAgentClient, fromAgent, netFriendToken)
                };

                BulkInserUserActionThread.EnQueueActionLogs(SourceLog);
                BulkInserUserActionThread.EnQueueActionLogs(TagertLog);

                #endregion
                return 0;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("TransferNetFriend                 {0}", ex.Message));
                return -1;
            }
        }

        /// <summary>
        /// 清理指定IIS下所有的对象
        /// </summary>
        /// <param name="strIISIP"></param>
        public void ClearAllObjectByIp(string strIISIP)
        {
            try
            {
                BLL.Loger.Log4Net.Info(string.Format("IMServices.ClearAllObjectByIp                清除IIS:{0} 所有坐席网友.", strIISIP));

                ProxyAgentClient agentT;
                ProxyNetFriend netFriendT;
                foreach (ProxyAgentClient agent in globalManager.DicAllAgents.Values.ToArray())
                {
                    if (agent.IISIP == strIISIP)
                    {
                        RemoveAgent(agent.AgentID, (int)Entities.CloseType.SystemClose);
                    }
                }

                foreach (ProxyNetFriend netFriend in globalManager.DicAllNetFriends.Values.ToArray())
                {
                    if (netFriend.IISIP == strIISIP)
                    {
                        //应用程序application_end 属于系统关闭
                        RemoveNetFriend(netFriend.Token, true, true, (int)Entities.CloseType.SystemClose);
                    }
                }
                //this.DicAllAgents.Clear();
                //this.DicAllNetFriends.Clear();
                foreach (BusinessWaiteQueueInfo bwq in WaitNetFriendsAllocMonitor.DicListWaitAgents.Values)
                {
                    foreach (ProxyNetFriend nef in bwq.DicWaitNetFriends.Values)
                    {
                        if (nef.IISIP == strIISIP)
                        {
                            bwq.DicWaitNetFriends.TryRemove(nef.WaitNum, out netFriendT);
                            netFriendT = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("ClearAllObjectByIp                 {0}", ex.Message));
            }
        }

        public void ForceAgentLeave(int agentId,int closeType)
        {
            try
            {
                ProxyAgentClient agent;
                if (globalManager.DicAllAgents.ContainsKey(agentId))
                {
                    globalManager.DicAllAgents.TryRemove(agentId, out agent);
                    BLL.Loger.Log4Net.Info(string.Format("ForceAgentLeave       移除坐席{0}", agentId));
                    if (agent == null) return;

                    //移除非同一机器网友
                    RemoveAgentTalkList(agent, closeType);

                    //给坐席发送强制退出消息

                    CometMessage cometMessage = new CometMessage()
                    {
                        Name = "ForceAgentLeave",
                        Contents = "坐席被强制退出",
                        FromToken = "",
                        ToToken = agent.AgentToken,
                        IISIP = agent.IISIP
                    };

                    globalManager.SendMessage(agent.IISIP, cometMessage);
                }

                agent = null;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("ForceAgentLeave出错                {0}", ex.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agents">IIS内部坐席</param>
        /// <param name="netFriends">IIS内部网友</param>
        /// <param name="iisIP">IISIP</param>
        /// <param name="agents4Delete">IIS内部要被删除的坐席</param>
        /// <param name="netFnd4Delete">IIS内部要被删除的网友</param>
        /// <param name="CallbackExist">判断双工回调Client是否存在</param>
        public void SyncObj(int[] agents, string[] netFriends, string iisIP, out int[] agents4Delete, out string[] netFnd4Delete, out bool CallbackExist)
        {
            agents4Delete = null;
            netFnd4Delete = null;
            CallbackExist = true;
            try
            {
                CallbackExist = MainIMService.DicIISServices.ContainsKey(iisIP);


                //获取指定IP下所有的网友对象
                var dicIISAgents = new Dictionary<int, int>();
                var dicIISNetFriends = new Dictionary<string, int>();

                foreach (int s in agents)
                {
                    if (!dicIISAgents.ContainsKey(s))
                    {
                        dicIISAgents.Add(s, s);
                    }
                }

                foreach (string s in netFriends)
                {
                    if (!string.IsNullOrEmpty(s) && !dicIISNetFriends.ContainsKey(s))
                    {
                        dicIISNetFriends.Add(s, 0);
                    }
                }

                //移除同一个IIS下已经过期的坐席,WCF中有IIS没有的坐席，从WCF中移除
                foreach (int key in globalManager.DicAllAgents.Values.Where(w => w.IISIP == iisIP).Select(s => s.AgentID).ToArray())
                {
                    if (!dicIISAgents.ContainsKey(key))
                    {
                        ProxyAgentClient pa;
                        globalManager.DicAllAgents.TryRemove(key, out pa);
                        BLL.Loger.Log4Net.Info("移除同一个IIS下已经过期的坐席, WCF中有,IIS没有的坐席" + key);
                        pa = null;
                    }
                }

                //移除同一个IIS下已经过期的网友，WCF中有但在IIS中没有的，从WCF中移除
                foreach (string strToken in globalManager.DicAllNetFriends.Values.Where(w => w.IISIP == iisIP).Select(s => s.Token).ToArray())
                {
                    if (!dicIISNetFriends.ContainsKey(strToken))
                    {
                        ProxyNetFriend pef;
                        globalManager.DicAllNetFriends.TryRemove(strToken, out pef);
                        pef = null;

                        //从对话列表中移除
                        foreach (KeyValuePair<int, ProxyAgentClient> proxyAgentClient in globalManager.DicAllAgents)
                        {
                            if (proxyAgentClient.Value.TalkUserList.ContainsKey(strToken))
                            {
                                proxyAgentClient.Value.TalkUserList.TryRemove(strToken, out pef);
                                pef = null;
                                break;
                            }
                        }
                    }
                }


                //移除同一个IIS下各业务线等待队列中的对象,WCF等待队列中有但在IIS中没有的，从WCF等待队列中移除
                foreach (BusinessWaiteQueueInfo bwq in WaitNetFriendsAllocMonitor.DicListWaitAgents.Values.ToArray())
                {
                    //listWaitNetFriend.AddRange(bwq.DicWaitNetFriends.Values);
                    foreach (ProxyNetFriend nefT in bwq.DicWaitNetFriends.Values.Where(w => w.IISIP == iisIP).ToArray())
                    {
                        if (!dicIISNetFriends.ContainsKey(nefT.Token))
                        {
                            ProxyNetFriend pef;
                            bwq.DicWaitNetFriends.TryRemove(nefT.WaitNum, out pef);
                            pef = null;
                        }
                    }
                }
                List<int> lstAgents4Deleted = new List<int>();
                List<string> lstNetFnd4Deleted = new List<string>();

                //IIS中有，但WCF中没有的对象，要返回给IIS让其移除
                foreach (int agentT in agents)
                {
                    if (!globalManager.DicAllAgents.ContainsKey(agentT))
                    {
                        lstAgents4Deleted.Add(agentT);
                    }
                }

                //IIS有，但在WCF中没有的网友，要返回给IIS让其移除。
                foreach (var nefndT in netFriends)
                {
                    if (!globalManager.DicAllNetFriends.ContainsKey(nefndT))
                    {
                        bool isExists = false;
                        foreach (BusinessWaiteQueueInfo bwq in WaitNetFriendsAllocMonitor.DicListWaitAgents.Values.ToArray())
                        {
                            foreach (ProxyNetFriend nefT in bwq.DicWaitNetFriends.Values.ToArray())
                            {
                                if (nefT.Token == nefndT)
                                {
                                    isExists = true;
                                    break;
                                }
                            }
                            if (isExists)
                            {
                                break;
                            }
                        }
                        if (!isExists)
                        {
                            lstNetFnd4Deleted.Add(nefndT);
                        }
                    }
                }


                agents4Delete = lstAgents4Deleted.ToArray();
                netFnd4Delete = lstNetFnd4Deleted.ToArray();
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("IMServices.SyncObj             " + ex.Message);
            }
        } ///



        public int RegisterIIS(string strIISIP)
        {
            BLL.Loger.Log4Net.Info(string.Format("方法RegisterIIS"));
            try
            {
                if (!MainIMService.DicIISServices.ContainsKey(strIISIP))
                {
                    IIsMsgCallBackServices client = OperationContext.Current.GetCallbackChannel<IIsMsgCallBackServices>();
                    //OperationContext.Current.Channel.Closing += new EventHandler(Channel_Closing);//注册客户端关闭触发事件
                    MainIMService.DicIISServices.Add(strIISIP, client);
                }

            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("方法RegisterIIS失败              {0}", ex.Message));
                return -1;
            }

            return 0;
        }

        private void Channel_Closing(object sender, EventArgs e)
        {
            //foreach (var d in MainIMService.DicIISServices)
            //{
            //    if (d.Value == (IIsMsgCallBackServices)sender)//删除此关闭的客户端信息
            //    {
            //        MainIMService.DicIISServices.Remove(d.Key);
            //        break;
            //    }
            //}
        }

        public void WcfTest(int id)
        {
            if (null != OperationContext.Current.SessionId)
            {
                Console.WriteLine("SessionId is {0}", OperationContext.Current.SessionId);
            }
            else
            {
                Console.WriteLine("SessionId is null");
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("Time:{0};ThreadId is :{1}.Request Id is {2} Add Method Invoked,", DateTime.Now, Thread.CurrentThread.ManagedThreadId, id));
            Debug.WriteLine(string.Format("Time:{0};ThreadId is :{1}.Request Id is {2} Add Method Invoked,", DateTime.Now, Thread.CurrentThread.ManagedThreadId, id));

            //Thread.Sleep(5000);
            //Console.WriteLine("=========Excute finished=========");
            Console.WriteLine();
        }

        public void Dispose()
        {
            Console.WriteLine("IMServices Disposed,  Thread Id is {0}", System.Threading.Thread.CurrentThread.ManagedThreadId);
        }
    }
}
