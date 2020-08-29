using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BitAuto.DSC.IM_2015.MainInterface
{
    [DataContract(Name = "cm")]
    public class CometMessage
    {
        //添加消息创建时间，已经处理时间，测试时使用
        public CometMessage()
        {
            CreateDateTime = DateTime.Now;
            //ID = Guid.NewGuid();
            EndDate = new DateTime(1900, 1, 1);
            ProcessTime = (DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1))).TotalMilliseconds.ToString();
        }

        public DateTime CreateDateTime { get; set; }
        //public Guid ID { get; set; }
        public DateTime EndDate { get; set; }

        [DataMember(Name = "mid")]
        public long MessageId;
        
        [DataMember(Name = "n")]
        public string Name;

        [DataMember(Name = "c")]
        public string Contents;

        [DataMember(Name = "ct")]
        public string ProcessTime;

        [DataMember(Name = "ft")]
        public string FromToken;

        [DataMember(Name = "tt")]
        public string ToToken;

        //[DataMember(Name = "c1")]
        //public string Message;

        [DataMember(Name = "isp")]
        public string IISIP;



    }
}
