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
        //public event EventHandler<UserArgs> OnTalkUserAdded;
        //public event EventHandler<UserArgs> OnTalkUserRemoved;
        private readonly object _locker = new object();

        //在聊客户
        private List<string> Talkuserlist = new List<string>();

        public CometClient()
        {
            //this.LastConBeginTime = new DateTime(1900, 1, 1);
            //this.LastSendMsgTime = new DateTime(1900, 1, 1);
            this.ConverSTime = new DateTime(1900, 1, 1);
            //this.SendMessageTime = new DateTime(1900, 1, 1);
            this.SendMessageTime = DateTime.Now;
            this.LastRequestTime = DateTime.Now;
            //this.WaitSTime = new DateTime(1900, 1, 1);
            //this.LastActivity = new DateTime(1900, 1, 1);
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
            /*
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
            */

        }
        //删除聊天客户
        public void RemoveTalkUser(string userid)
        {


            lock (this._locker)
            {
                if (!this.Talkuserlist.Contains(userid))
                    return;
                Talkuserlist.Remove(userid);
            }
            /*
            if (OnTalkUserRemoved != null)
            {
                OnTalkUserRemoved(this, new UserArgs(this.Talkuserlist, userid.ToString(), this.PrivateToken));
            }
            */
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
        public string PrivateToken;

        /// <summary>
        /// 记录网友对应坐席Token,type为坐席时等于PrivateToken,type为网友时，记录其对于坐席的PrivateToken
        /// </summary>
        public string AgentToken;



        /// <summary>
        /// Gets or Sets the display name of the client
        /// </summary>
        public string DisplayName
        {
            get;
            set;
        }
        ///// <summary>
        ///// Gets or Sets the last activity of the client
        ///// </summary>
        //public DateTime LastActivity
        //{
        //    get;
        //    set;
        //}
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

        public string IPIISWY;              //网友IIS
        public string IPIISAgent;           //坐席IIS
        //public bool IsNative = true;        //判断是否是本地IIS对象

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

        /// <summary>
        /// 最后发送消息时间
        /// </summary>
        public DateTime SendMessageTime;
        
        //网友最后发消息时间
        //public DateTime LastSendMsgTime { get; set; }

        //网友最后接受消息时间，网友计算空闲时间以【最后发消息时间,最后接受时间后者为准】，即如果网友收到坐席消息后，网友空闲时间延后
        public DateTime LastReceiveMsgTime { get; set; }



        /// <summary>
        /// 在聊网友数上限值
        /// </summary>
        public int MaxDialogCount
        {
            get;
            set;
        }

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

        }

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


    }
    //public class UserArgs : EventArgs
    //{
    //    public List<string> UserList;
    //    public string CurrentUserId = string.Empty;
    //    public string AgentId = string.Empty;
    //    //public string CSID = string.Empty;
    //    public UserArgs(List<string> uList, string strCurrenUser, string strAgentId)
    //    {
    //        this.UserList = uList;
    //        this.CurrentUserId = strCurrenUser;
    //        this.AgentId = strAgentId;
    //    }
    //}

}
