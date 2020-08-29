using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using BitAuto.DSC.IM_DMS2014.Entities;

namespace BitAuto.DSC.IM_DMS2014.Core
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
        public event EventHandler<UserArgs> OnWaitUserAdded;
        public event EventHandler<UserArgs> OnWaitUserRemoved;

        public CometClient()
        {
            this.LastConBeginTime = new DateTime(1900, 1, 1);
            this.LastMessageTime = new DateTime(1900, 1, 1);
            this.ConverSTime = new DateTime(1900, 1, 1);
            //this.SendMessageTime = new DateTime(1900, 1, 1);
            this.SendMessageTime = DateTime.Now;
            this.LastRequestTime = DateTime.Now;
            this.WaitSTime = new DateTime(1900, 1, 1);
            this.LastActivity = new DateTime(1900, 1, 1);
        }

        //在聊客户
        public List<long> _talkuserlist = new List<long>();
        //等待客户
        public List<long> _waitUserList = new List<long>();

        private object state = new object();
        private object stateWait = new object();

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
        public long[] TalkUserList
        {
            get
            {
                lock (this.state)
                {
                    return _talkuserlist.ToArray();
                }
            }
        }
        //在排队网友
        public long[] WaitUserList
        {
            get
            {
                lock (this.stateWait)
                {
                    return _waitUserList.ToArray();
                }
            }
        }

        public int AllocId { get; set; }

        //获取第一位等待用于
        public long GetFirstWaitUser()
        {
            lock (stateWait)
            {
                if (_waitUserList.Count < 1)
                    return -2;
                return _waitUserList[0];

            }
        }

        //新增等待客户
        public void AddWaitUser(long userid)
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
        public void RemoveWaitUser(long userid)
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
        public long[] RemoveAllWaitUser()
        {
            //foreach (long t in this._waitUserList)
            //{
            //    RemoveWaitUser(t);
            //}
            long[] t;
            lock (stateWait)
            {
                t = _waitUserList.ToArray();
                this._waitUserList.Clear();
            }
            return t;
        }

        //新增聊天客户
        public void AddTalkUser(long userid)
        {
            lock (this.state)
            {
                _talkuserlist.Add(userid);
            }
            if (OnTalkUserAdded != null)
            {
                OnTalkUserAdded(this, new UserArgs(this._talkuserlist, userid.ToString(), this.PrivateToken));
            }
            BLL.Loger.Log4Net.Info(string.Format("当前坐席：PrivateToken：{0},添加网友：{1}", this.PrivateToken, userid));


            //add by qizq 2015-1-7 加监控入库日志
            Entities.UserActionLog model = new UserActionLog();
            model.CreateTime = System.DateTime.Now;
            //model.CreateUserID = (int)userid;
            //系统
            model.OperUserType = 3;
            //给坐席新增聊天客户
            model.LogInType = 8;
            model.LogInfo = string.Format("给坐席：PrivateToken：{0},添加网友：{1}", this.PrivateToken, userid);
            BLL.UserActionLog.Instance.Insert(model);


        }
        //删除聊天客户
        public void RemoveTalkUser(long userid)
        {
            if (!this._talkuserlist.Contains(userid))
                return;

            lock (this.state)
            {
                _talkuserlist.Remove(userid);
            }
            if (OnTalkUserRemoved != null)
            {
                OnTalkUserRemoved(this, new UserArgs(this._talkuserlist, userid.ToString(), this.PrivateToken));
            }
        }

        //删除所有聊天客户,当且仅当坐席离线时使用
        public long[] RemoveAllTalkUser()
        {
            long[] lstTaluUsers;
            lock (this.state)
            {
                lstTaluUsers = _talkuserlist.ToArray();
                _talkuserlist.Clear();
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

        public string AgentNum { get; set; }

        //在聊客服
        public string AgentID
        {
            get;
            set;
        }

        #region 附加自定义属性
        //易湃访问ID
        public string VisitID { get; set; }
        //经销商loginID
        public long LoginID { get; set; }
        //经销商编号
        public string MemberCode { get; set; }
        //经销商名称
        public string MemberName { get; set; }
        //最后访问title
        public string UserReferTitle { get; set; }
        //最后访问页面
        public string UserReferUrl { get; set; }
        //发起模块
        public string VisitRefer { get; set; }
        //联系人姓名
        public string ContractName { get; set; }
        //联系人职务
        public string ContractJob { get; set; }
        //联系人电话
        public string ContractPhone { get; set; }
        //联系人Email
        public string ContractEmail { get; set; }
        //地理位置
        private string _address = "";
        public string Address
        {
            get
            {
                return _address; //ProvinceName + " " + CityName + " " + CountyName;
            }
            set
            {
                _address = value;
            }
        }
        //省
        public int ProvinceID { get; set; }
        //城市
        public int CityID { get; set; }
        //区县
        public int CountyID { get; set; }
        //省
        public string ProvinceName { get; set; }
        //城市
        public string CityName { get; set; }
        //区县
        public string CountyName { get; set; }
        //所属城市群
        public string CityGroupId { get; set; }
        //所属城市群
        public string CityGroupName { get; set; }
        //大区id
        public string DistrictId { get; set; }
        //大区名称
        public string DistrictName { get; set; }
        //上次最后消息时间
        public DateTime LastMessageTime { get; set; }
        //上次会话开始时间(访问时间)
        public DateTime LastConBeginTime { get; set; }
        //分配坐席次数(访问次数)
        public int Distribution { get; set; }

        /// <summary>
        /// 在聊网友数上限值
        /// </summary>
        public int MaxDialogCount
        {
            get;
            set;
        }
        //最大同时排队数量
        public int MaxQueueN { get; set; }
        /// <summary>
        /// 坐席状态, 2 离线：1：在线
        /// </summary>
        public AgentStatus Status { get; set; }
        /// <summary>
        /// 客户端类别：1：客服，2:网友，0：系统 
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 在聊网友数-记数器
        /// </summary>
        public int DialogCount
        {
            get;
            set;
        }

        //等待开始时间
        public DateTime WaitSTime { get; set; }
        //本次会话开始时间
        public DateTime ConverSTime { get; set; }
        #endregion
    }
    public class UserArgs : EventArgs
    {
        public List<long> UserList;
        public string CurrentUserId = string.Empty;
        public string AgentId = string.Empty;
        public UserArgs(List<long> uList, string strCurrenUser, string strAgentId)
        {
            this.UserList = uList;
            this.CurrentUserId = strCurrenUser;
            this.AgentId = strAgentId;
        }
    }

}
