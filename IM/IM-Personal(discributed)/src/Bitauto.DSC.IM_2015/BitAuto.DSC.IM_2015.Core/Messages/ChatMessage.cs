using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BitAuto.DSC.IM_2015.Core.Messages
{
    [DataContract(Name = "cm")]
    public class ChatMessage
    {
        public ChatMessage()
        {
            this.time = DateTime.Now;
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

        [DataMember(Name = "ctn")]
        public string WYName;

        //联系人电话
        [DataMember(Name = "ctp")]
        public string contractphone;

        //访问记录ID
        [DataMember(Name = "vid")]
        public string VisitID;


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
    }
}
