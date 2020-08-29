using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BitAuto.DSC.IM_DMS2014.Core.Messages
{
    [DataContract(Name = "cm")]
    public class ChatMessage
    {
        public ChatMessage()
        {
            this.time = DateTime.Now;
            this.converstime = DateTime.Now;
            this.lastconbegintime = DateTime.Now;
            this.lastmessagetime = DateTime.Now;
        }

        [DataMember(Name = "f")]
        private string from;
        [DataMember(Name = "m")]
        private string message;
        [DataMember(Name = "t")]
        private DateTime time;
        //会话ID
        [DataMember(Name = "cs")]
        private int csid;
        public string From
        {
            get { return this.from; }
            set { this.from = value; }
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
        //会话ID
        public int CsID
        {
            get { return this.csid; }
            set { this.csid = value; }
        }


        //经销商名称
        [DataMember(Name = "mn")]
        private string membername;
        //经销商名称
        public string MemberName
        {
            get { return membername; }
            set { membername = value; }
        }
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
        //经销商code
        [DataMember(Name = "mc")]
        private string membercode;
        public string MemberCode
        {
            get { return membercode; }
            set { membercode = value; }
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
        //客户ID
        [DataMember(Name = "lgid")]
        private long loginid;

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
