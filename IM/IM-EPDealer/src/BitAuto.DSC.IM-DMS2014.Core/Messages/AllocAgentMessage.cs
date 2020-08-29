using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BitAuto.DSC.IM2014.Core.Messages
{
    /// <summary>
    /// 给网友分配坐席消息,或给坐席分配网友
    /// </summary>
    [DataContract(Name = "aam")]
    public class AllocAgentMessage
    {
        public AllocAgentMessage()
        {
            this.time = new DateTime(1900, 1, 1);
            this.converstime = new DateTime(1900, 1, 1);
            this.lastmessagetime = new DateTime(1900, 1, 1);
            this.lastconbegintime = new DateTime(1900, 1, 1);


        }

        //发送人
        [DataMember(Name = "uid")]
        private string userid;
        //消息文本
        [DataMember(Name = "m")]
        private string message;
        //发送时间
        [DataMember(Name = "t")]
        private DateTime time;
        //会话ID
        [DataMember(Name = "cs")]
        private int csid;
        //客户ID
        [DataMember(Name = "lgid")]
        private long loginid;
        //坐席ID
        [DataMember(Name = "a")]
        private int agentid;

        [DataMember(Name = "anum")]
        private string agentnum;

        //经销商code
        [DataMember(Name = "mc")]
        private string membercode;

        //经销商名称
        [DataMember(Name = "mn")]
        private string membername;
        //本次会话开始时间
        [DataMember(Name = "cst")]
        private DateTime converstime;
        //地理位置
        [DataMember(Name = "adr")]
        private string address;
        //所属城市群
        [DataMember(Name = "cgn")]
        private string citygroupname;
        //上次最后消息时间
        [DataMember(Name = "lmt")]
        private DateTime lastmessagetime;
        //上次会话开始时间(访问时间)
        [DataMember(Name = "lct")]
        private DateTime lastconbegintime;
        //分配坐席次数(访问次数)
        [DataMember(Name = "dst")]
        private int distribution;

        //联系人姓名
        [DataMember(Name = "ctp")]
        private string contractphone;

        [DataMember(Name = "ctn")]
        private string contractname;
        //联系人职务
        [DataMember(Name = "ctj")]
        private string contractjob;
        //最后访问title
        [DataMember(Name = "uft")]
        private string userrefertitle;
        //坐席ID
        public int AgentID
        {
            get
            {
                return this.agentid;
            }
            set
            {
                this.agentid = value;
            }
        }

        //坐席工号
        public string AgentNum
        {
            get
            {
                return this.agentnum;
            }
            set
            {
                this.agentnum = value;
            }
        }

        //客户ID
        public long LoginID
        {
            get
            {
                return this.loginid;
            }
            set
            {
                this.loginid = value;
            }

        }

        //会话ID
        public int CsID
        {
            get { return this.csid; }
            set { this.csid = value; }
        }

        public string UserId
        {
            get { return this.userid; }
            set { this.userid = value; }
        }

        public string Message
        {
            get { return this.message; }
            set { this.message = value; }
        }
        public DateTime Time
        {
            get { return this.time; }
            set { this.time = value; }
        }



        //经销商名称
        public string MemberName
        {
            get { return membername; }
            set { membername = value; }
        }

        public string MemberCode
        {
            get { return membercode; }
            set { membercode = value; }
        }

        //本次会话开始时间
        public DateTime ConverSTime
        {
            get { return converstime; }
            set { converstime = value; }
        }
        //地理位置
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        //所属城市群
        public string CityGroupName
        {
            get { return citygroupname; }
            set { citygroupname = value; }
        }
        //上次最后消息时间
        public DateTime LastMessageTime
        {
            get { return lastmessagetime; }
            set { lastmessagetime = value; }
        }
        //上次会话开始时间(访问时间)
        public DateTime LastConBeginTime
        {
            get { return lastconbegintime; }
            set { lastconbegintime = value; }
        }
        //分配坐席次数(访问次数)
        public int Distribution
        {
            get { return distribution; }
            set { distribution = value; }
        }
        //联系人姓名
        public string ContractName
        {
            get { return contractname; }
            set { contractname = value; }
        }
        //联系人职务
        public string ContractJob
        {
            get { return contractjob; }
            set { contractjob = value; }
        }
        //最后访问title
        public string UserReferTitle
        {
            get { return userrefertitle; }
            set { userrefertitle = value; }
        }

        public string ContractPhone
        {
            get { return contractphone; }
            set { contractphone = value; }
        }
    }
}
