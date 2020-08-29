using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BitAuto.DSC.IM2014.Core
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
        [DataMember]
        private string privateToken;
        [DataMember]
        private string publicToken;
        [DataMember]
        private string displayName;
        [DataMember]
        private DateTime lastActivity;
        [DataMember]
        private int connectionIdleSeconds;
        [DataMember]
        private int connectionTimeoutSeconds;

        #region 附加自定义属性
        /// <summary>
        /// 网友无发送消息超时时长
        /// </summary>
        [DataMember]
        private int sendmessageIdleSeconds;
        public int SendMessageIdleSeconds
        {
            get { return this.sendmessageIdleSeconds; }
            set { this.sendmessageIdleSeconds = value; }
        }
        [DataMember]
        private int status;
        [DataMember]
        private int maxdialogcount;
        [DataMember]
        private int type;
        [DataMember]
        private int dialogcount;
        [DataMember]
        private string userreferurl;
        [DataMember]
        private string localip;
        [DataMember]
        private string location;
        [DataMember]
        private string locationid;
        [DataMember]
        private string entertime;
        [DataMember]
        private int waittime;
        [DataMember]
        private string talktime;
        [DataMember]
        private List<string> talkuserlist = new List<string>();
        [DataMember]
        private DateTime sendmessagetime;
        /// <summary>
        /// 最后发送消息时间
        /// </summary>
        public DateTime SendMessageTime
        {
            get { return this.sendmessagetime; }
            set { this.sendmessagetime = value; }
        }
        //在聊网友
        public string[] TalkUserList
        {
            get
            {
                lock (this.state)
                {
                    return talkuserlist.ToArray();
                }
            }
        }
        private object state = new object();

        #endregion
        public void addUser(string userid)
        {
            lock (this.state)
            {
                talkuserlist.Add(userid);
            }
        }
        public void RemoveUser(string userid)
        {
            lock (this.state)
            {
                talkuserlist.Remove(userid);
            }
        }
        public void RemoveAllUser()
        {
            lock (this.state)
            {
                talkuserlist.Clear();
            }
        }
        /// <summary>
        /// Gets or Sets the token used to identify the client to themselves
        /// </summary>
        public string PrivateToken
        {
            get { return this.privateToken; }
            set { this.privateToken = value; }
        }

        /// <summary>
        /// Gets or Sets the token used to identify the client to other clients
        /// </summary>
        public string PublicToken
        {
            get { return this.publicToken; }
            set { this.publicToken = value; }
        }

        /// <summary>
        /// Gets or Sets the display name of the client
        /// </summary>
        public string DisplayName
        {
            get { return this.displayName; }
            set { this.displayName = value; }
        }

        /// <summary>
        /// Gets or Sets the last activity of the client
        /// </summary>
        public DateTime LastActivity
        {
            get { return this.lastActivity; }
            set { this.lastActivity = value; }
        }

        /// <summary>
        /// Gets or Sets the ConnectionIdleSections property which is the number of seconds a connection will remain
        /// alive for without being connected to a client, after this time has expired the client will 
        /// be removed from the state manager
        /// </summary>
        public int ConnectionIdleSeconds
        {
            get { return this.connectionIdleSeconds; }
            set { this.connectionIdleSeconds = value; }
        }

        /// <summary>
        /// Gets or Sets the ConnectionTimeOutSections property which is the number of seconds a connection will remain
        /// alive for whilst being connected to a client, but without receiving any messages.  After a timeout has expired
        /// A client should restablish a connection to the server
        /// </summary>
        public int ConnectionTimeoutSeconds
        {
            get { return this.connectionTimeoutSeconds; }
            set { this.connectionTimeoutSeconds = value; }
        }

        #region 附加自定义属性
        [DataMember]
        /// <summary>
        /// 坐席状态
        /// </summary>
        public int Status
        {
            get { return this.status; }
            set { this.status = value; }
        }
        /// <summary>
        /// 在聊网友数上限值
        /// </summary>
        public int MaxDialogCount
        {
            get { return this.maxdialogcount; }
            set { this.maxdialogcount = value; }
        }
        /// <summary>
        /// 客户端类别：网友、坐席
        /// </summary>
        public int Type
        {
            get { return this.type; }
            set { this.type = value; }
        }
        /// <summary>
        /// 在聊网友数-记数器
        /// </summary>
        public int DialogCount
        {
            get { return this.dialogcount; }
            set { this.dialogcount = value; }
        }
        /// <summary>
        /// 网友涞源
        /// </summary>
        public string UserReferURL
        {
            get { return this.userreferurl; }
            set { this.userreferurl = value; }
        }
        /// <summary>
        /// ip地址
        /// </summary>
        public string LocalIP
        {
            get { return this.localip; }
            set { this.localip = value; }
        }
        /// <summary>
        /// 地理位置
        /// </summary>
        public string Location
        {
            get { return this.location; }
            set { this.location = value; }
        }
        /// <summary>
        /// 地理位置id
        /// </summary>
        public string LocationID
        {
            get { return this.locationid; }
            set { this.locationid = value; }
        }
        /// <summary>
        /// 进入时间
        /// </summary>
        public string EnterTime
        {
            get { return this.entertime; }
            set { this.entertime = value; }
        }
        /// <summary>
        /// 等待时长
        /// </summary>
        public int WaitTime
        {
            get { return this.waittime; }
            set { this.waittime = value; }
        }
        /// <summary>
        /// 对话开始时间
        /// </summary>
        public string TalkTime
        {
            get { return this.talktime; }
            set { this.talktime = value; }
        }
        #endregion
    }
}
