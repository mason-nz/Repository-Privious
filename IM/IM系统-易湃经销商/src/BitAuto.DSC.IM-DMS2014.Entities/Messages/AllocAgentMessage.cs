using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace BitAuto.DSC.IM_DMS2014.Entities.Messages
{
    [DataContract(Name = "aam")]
    public class AllocAgentMessage
    {
        [DataMember(Name = "f")]
        private string from;
        [DataMember(Name = "m")]
        private string message;
        [DataMember(Name = "t")]
        private DateTime time;
        [DataMember(Name = "cs")]
        private int csid;
        [DataMember(Name = "cu")]
        private long custid;
        [DataMember(Name = "a")]
        private int agentid;
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

        //客户ID
        public long CustID
        {
            get
            {
                return this.custid;
            }
            set
            {
                this.custid = value;
            }

        }

        //会话ID
        public int CsID
        {
            get { return this.csid; }
            set { this.csid = value; }
        }

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

    }
}
