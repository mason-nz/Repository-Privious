using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Threading;
using BitAuto.DSC.IM_2015.Entities;

namespace BitAuto.DSC.IM_2015.Core
{
    /// <summary>
    /// CometClient Class
    /// 
    /// This represents a logged in client within the COMET application.  This marked as a DataContract becuase
    /// it can be seralized to the client using JSON
    /// </summary>
    [DataContract]
    public class CometClient
    {
        public event EventHandler<UserArgs> OnTalkUserAdded;
        public event EventHandler<UserArgs> OnTalkUserRemoved;
        private readonly object _locker = new object();

        //在聊客户
        private List<string> Talkuserlist = new List<string>();

        public CometClient()
        {
            //this.LastConBeginTime = new DateTime(1900, 1, 1);
            this.LastMessageTime = new DateTime(1900, 1, 1);
            this.ConverSTime = new DateTime(1900, 1, 1);
            //this.SendMessageTime = new DateTime(1900, 1, 1);
            this.SendMessageTime = DateTime.Now;
            this.LastRequestTime = DateTime.Now;
            //this.WaitSTime = new DateTime(1900, 1, 1);
            this.LastActivity = new DateTime(1900, 1, 1);
        }



        /// <summary>
        /// 最后长连接结束时间
        /// </summary>
        public DateTime LastRequestTime { get; set; }

        /// <summary>
        /// 网友无发送消息超时时长
        /// </summary>
        public int SendMessageIdleSeconds
        {
            set;
            get;
        }
        /// <summary>
        /// 最后发送消息时间
        /// </summary>
        public DateTime SendMessageTime
        {
            get;
            set;
        }

        //在聊网友
        public string[] TalkUserList
        {
            get
            {
                lock (this._locker)
                {
                    return Talkuserlist.ToArray();
                }
            }
        }

        public int CSId { get; set; }
        //新增聊天客户
        public void AddTalkUser(string userid)
        {
            lock (this._locker)
            {
                Talkuserlist.Add(userid);
            }
            if (OnTalkUserAdded != null)
            {
                OnTalkUserAdded(this, new UserArgs(this.Talkuserlist, userid.ToString(), this.PrivateToken));
            }
            //BLL.Loger.Log4Net.Info(string.Format("方法：AddTalkUser：当前坐席：PrivateToken：{0},添加网友：{1}", this.PrivateToken, userid));


            //add by qizq 2015-1-7 加监控入库日志
            Entities.UserActionLog model = new UserActionLog();
            model.CreateTime = System.DateTime.Now;
            //model.CreateUserID = (int)userid;
            //系统
            model.OperUserType = 3;
            //给坐席新增聊天客户
            model.LogInType = 8;
            model.LogInfo = string.Format("给坐席：PrivateToken：{0},添加网友：{1}", this.PrivateToken, userid);
            //BLL.UserActionLog.Instance.Insert(model);
            BulkInserUserActionThread.EnQueueActionLogs(model);

        }
        //删除聊天客户
        public void RemoveTalkUser(string userid)
        {
            if (!this.Talkuserlist.Contains(userid))
                return;

            lock (this._locker)
            {
                Talkuserlist.Remove(userid);
            }
            if (OnTalkUserRemoved != null)
            {
                OnTalkUserRemoved(this, new UserArgs(this.Talkuserlist, userid.ToString(), this.PrivateToken));
            }
        }

        //删除所有聊天客户,当且仅当坐席离线时使用
        public string[] RemoveAllTalkUser()
        {
            string[] lstTaluUsers;
            lock (this._locker)
            {
                lstTaluUsers = Talkuserlist.ToArray();
                Talkuserlist.Clear();
            }
            return lstTaluUsers;

        }

        /// <summary>
        /// Gets or Sets the token used to identify the client to themselves
        /// </summary>
        public string PrivateToken
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or Sets the token used to identify the client to other clients
        /// </summary>
        public string PublicToken
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or Sets the display name of the client
        /// </summary>
        public string DisplayName
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or Sets the last activity of the client
        /// </summary>
        public DateTime LastActivity
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or Sets the ConnectionIdleSections property which is the number of seconds a connection will remain
        /// alive for without being connected to a client, after this time has expired the client will 
        /// be removed from the state manager
        /// </summary>
        public int ConnectionIdleSeconds
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or Sets the ConnectionTimeOutSections property which is the number of seconds a connection will remain
        /// alive for whilst being connected to a client, but without receiving any messages.  After a timeout has expired
        /// A client should restablish a connection to the server
        /// </summary>
        public int ConnectionTimeoutSeconds
        {
            get;
            set;
        }

        /// <summary>
        /// 工号
        /// </summary>
        public string AgentNum { get; set; }

        /// <summary>
        /// 0:未删除，1：已删除
        /// </summary>
        public int IsDeleted = 0;

        /// <summary>
        /// 标记是否在转接中，如果是的话，不发离线消息
        /// </summary>
        public int IsTransfering = 0;
        //在聊客服
        public string AgentID
        {
            get;
            set;
        }

        public string AgentName;

        #region 附加自定义属性

        public bool IsTurnIn = false;
        public bool IsTurnOut = false;
        public bool IsAgentReply = false;  //判断坐席是否已回应

        /// <summary>
        /// 所在分组
        /// </summary>
        public string InBGID;

        /// <summary>
        /// 所在分组名称
        /// </summary>
        public string InBGIDName;

        /// <summary>
        /// 管辖分组
        /// </summary>
        public string ManagedBGID { get; set; }


        /// <summary>
        /// 记录网友访问记录信息
        /// </summary>
        public UserVisitLog Userloginfo;

        //网友最后消息时间
        public DateTime LastMessageTime { get; set; }

        ////上次会话开始时间(访问时间)
        //public DateTime LastConBeginTime { get; set; }
        //分配坐席次数(访问次数)
        //public int Distribution { get; set; }

        /// <summary>
        /// 在聊网友数上限值
        /// </summary>
        public int MaxDialogCount
        {
            get;
            set;
        }
        //最大同时排队数量
        //public int MaxQueueN { get; set; }
        /// <summary>
        /// 坐席状态, 2 离线：1：在线,3:暂离,4:聊天中，5:关闭
        /// </summary>

        public int AgentStatusRecID;   //存储要更新的坐席状态ID
        private AgentStatus _status;
        public AgentStatus Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                //try
                //{
                //    if (Type == 1)
                //    {

                //    }
                //}
                //catch (Exception ex)
                //{
                //    BLL.Loger.Log4Net.Info("[更改坐席状态时出错] " + ex.Message);
                //}
            }
        }

        public void RecordAgentStatus(int nStatus)
        {
            if (this.Type != 1) { return; }
            try
            {
                this.AgentStatusRecID = BLL.AgentStatusDetail.Instance.AddAgentStatusData(Convert.ToInt32(this.AgentID), nStatus, AgentStatusRecID);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[更改坐席状态时出错] " + ex.Message);
            }

            //ThreadPool.QueueUserWorkItem(UpdateAgentStatus, new AgentID_Value() { AgentID = Convert.ToInt32(AgentID), Status = nStatus });
        }
        /*
        private struct AgentID_Value
        {
            public int AgentID;
            public int Status;
            public int Recid;
        }

        private void UpdateAgentStatus(object obj)
        {
            var ag = (AgentID_Value)obj;
            if (ag.AgentID <= 0)
            {
                return;
            }
            try
            {
                this.AgentStatusRecID = BLL.AgentStatusDetail.Instance.AddAgentStatusData(ag.AgentID, ag.Status, AgentStatusRecID);

            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("更新坐席{0}状态为{1}失败,原因:{2}", ag.AgentID, ag.Status, ex.Message));
            }

        }
        */
        /// <summary>
        /// 客户端类别：1：客服，2:网友，0：系统 
        /// </summary>
        public int Type { get; set; }

        //客服应答时间
        public DateTime AgentSTime { get; set; }
        //本次会话开始时间
        public DateTime ConverSTime { get; set; }

        public string BusinessLines { get; set; }

        #endregion

        #region 等待队列
        //public event EventHandler<UserArgs> OnWaitUserAdded;
        //public event EventHandler<UserArgs> OnWaitUserRemoved;

        //等待客户
        //public List<string> _waitUserList = new List<string>();
        //private object stateWait = new object();
        /*
               //在排队网友
               public string[] WaitUserList
               {
                   get
                   {
                       lock (this.stateWait)
                       {
                           return _waitUserList.ToArray();
                       }
                   }
               }
        //获取第一位等待用于
               public string GetFirstWaitUser()
               {
                   lock (stateWait)
                   {
                       if (_waitUserList.Count < 1)
                           return "-2";
                       return _waitUserList[0];

                   }
               }

               //新增等待客户
               public void AddWaitUser(string userid)
               {
                   lock (this.stateWait)
                   {
                       _waitUserList.Add(userid);
                   }
                   if (OnWaitUserAdded != null)
                   {
                       OnWaitUserAdded(this, new UserArgs(this._waitUserList, userid.ToString(), this.PrivateToken));
                   }
               }
               //删除等待客户
               public void RemoveWaitUser(string userid)
               {
                   if (!_waitUserList.Contains(userid))
                       return;

                   lock (this.stateWait)
                   {
                       _waitUserList.Remove(userid);
                   }
                   if (OnWaitUserRemoved != null)
                   {
                       OnWaitUserRemoved(this, new UserArgs(this._waitUserList, userid.ToString(), this.PrivateToken));
                   }
               }

               //删除全部等待客户,当且仅当坐席离线时使用
               public string[] RemoveAllWaitUser()
               {
                   //foreach (long t in this._waitUserList)
                   //{
                   //    RemoveWaitUser(t);
                   //}
                   string[] t;
                   lock (stateWait)
                   {
                       t = _waitUserList.ToArray();
                       this._waitUserList.Clear();
                   }
                   return t;
               }         
               */

        /*
       //访问记录中自增列
       public int VisitID { get; set; }

       //经销商loginID
       public string LoginID { get; set; }
       public string CustID { get; set; }
       public string UserName { get; set; }
       public bool Sex { get; set; }
       public string Phone { get; set; }

       //最后访问title
       public string UserReferTitle { get; set; }
       //最后访问页面
       public string UserReferUrl { get; set; }

       //省
       public int ProvinceID { get; set; }
       //城市
       public int CityID { get; set; }
       */
        #endregion

    }
    public class UserArgs : EventArgs
    {
        public List<string> UserList;
        public string CurrentUserId = string.Empty;
        public string AgentId = string.Empty;
        //public string CSID = string.Empty;
        public UserArgs(List<string> uList, string strCurrenUser, string strAgentId)
        {
            this.UserList = uList;
            this.CurrentUserId = strCurrenUser;
            this.AgentId = strAgentId;
        }
    }

}
