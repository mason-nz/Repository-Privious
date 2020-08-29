using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BitAuto.DSC.IM_2015.Core
{
    /*
    /// <summary>
    /// CometMessage Class
    /// 
    /// This is a CometMessage that has been sent to the client, the DataContract names have been
    /// shortened to remove any bytes we dont need from the message (ok, did'nt save much, but we can do it!)
    /// </summary>'
    /// 
    
    [DataContract(Name = "cm")]
    public class CometMessage
    {
        //添加消息创建时间，已经处理时间，测试时使用
        public CometMessage()
        {
            CreateDateTime = DateTime.Now;
            ID = Guid.NewGuid();
            EndDate = new DateTime(1900, 1, 1);
            processTime =
                (DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1))).TotalMilliseconds
                    .ToString();
        }

        public DateTime CreateDateTime { get; set; }
        public Guid ID { get; set; }
        public DateTime EndDate { get; set; }

        [DataMember(Name = "mid")]
        private long messageId;
        [DataMember(Name = "n")]
        private string name;
        [DataMember(Name = "c")]
        private string contents;


        [DataMember(Name = "ct")]
        private string processTime;

        [DataMember(Name = "ft")]
        public string FromToken;

        [DataMember(Name = "tt")]
        public string ToToken;

        [DataMember(Name = "c1")]
        public string Message;

        [DataMember(Name = "isp")]
        public string IISIP;
        /// <summary>
        /// 消息处理时间（消息处理时，获取Message信息之后，赋予当前系统时间）
        /// </summary>
        public string ProcessTime
        {
            get { return this.processTime; }
            set { this.processTime = value; }
        }

        /// <summary>
        /// Gets or Sets the MessageId, used to track which message the Client last received
        /// </summary>
        public long MessageId
        {
            get { return this.messageId; }
            set { this.messageId = value; }
        }

        /// <summary>
        /// Gets or Sets the Content of the Message
        /// </summary>
        public string Contents
        {
            get { return this.contents; }
            set { this.contents = value; }
        }

        /// <summary>
        /// Gets or Sets the error message if this is a failure
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
    }
    */
}
