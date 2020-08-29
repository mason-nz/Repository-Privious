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
        /// 所有再聊网友
        /// </summary>
        public ConcurrentDictionary<string, ProxyNetFriend> DicAllNetFriends = new ConcurrentDictionary<string, ProxyNetFriend>();

        //private static readonly object _locker = new object();

        private static Timer SyncDownMsgTimer;

        /// <summary>
        /// 存储每个IIS对应的消息池。键：IP，
        /// </summary>
        public ConcurrentDictionary<string, ConcurrentDictionary<long, CometMessage>> DicIPMessagePool = new ConcurrentDictionary<string, ConcurrentDictionary<long, CometMessage>>();


        public static Dictionary<string, IIsMsgCallBackServices> DicIISServices = new Dictionary<string, IIsMsgCallBackServices>();

        public MainIMService()
        {
            InitializeComponent();

            MainAllocThreadMonitor = new WaitNetFriendsAllocMonitor(this);

            MainMessageReceiver = new UserWorkQueue<CometMessage>() { ISSequential = false };

            IMServices.globalManager = this;

            MainMessageReceiver.DoUserWork += new EventHandler<EnqueueEventArgs<CometMessage>>(MainMessageReceiver_DoUserWork);
            //SyncDownMsgTimer = new Timer(obj => SyncDownMsg(), null, 2000, Timeout.Infinite);
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
                if (!DicIPMessagePool.ContainsKey(e.Item.IISIP))
                {
                    DicIPMessagePool.TryAdd(e.Item.IISIP, new ConcurrentDictionary<long, CometMessage>());
                }

                DicIPMessagePool[e.Item.IISIP].TryAdd(Interlocked.Increment(ref this.nMessageNum), e.Item);// .Enqueue(e.Item);

                if (e.Item.Name == "ChatMessage")
                {
                    if (DicAllNetFriends.ContainsKey(e.Item.FromToken))
                    {
                        DicAllNetFriends[e.Item.FromToken].LastMessageTime = DateTime.Now;
                    }
                }

                if (!string.IsNullOrWhiteSpace(e.Item.FromToken))
                {
                    if (DicAllNetFriends.ContainsKey(e.Item.FromToken))
                    {
                        DicAllNetFriends[e.Item.FromToken].LastActiveTime = DateTime.Now;
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
            if (!DicIPMessagePool.ContainsKey(IP))
            {
                DicIPMessagePool.TryAdd(IP, new ConcurrentDictionary<long, CometMessage>());
            }
            DicIPMessagePool[IP].TryAdd(Interlocked.Increment(ref this.nMessageNum), message);

            //更新最后会话时间
            if (!string.IsNullOrWhiteSpace(message.FromToken))
            {
                if (DicAllNetFriends.ContainsKey(message.FromToken))
                {
                    DicAllNetFriends[message.FromToken].LastActiveTime = DateTime.Now;
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
