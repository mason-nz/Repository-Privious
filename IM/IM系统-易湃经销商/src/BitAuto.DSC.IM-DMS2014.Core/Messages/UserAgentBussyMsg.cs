using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BitAuto.DSC.IM2014.Core.Messages
{
    /// <summary>
    /// 给网友分配坐席，坐席全忙消息
    /// </summary>
    [DataContract(Name = "UserAgentBussyMsg")]
    public class UserAgentBussyMsg
    {
        [DataMember(Name = "UserID")]
        private string userid;
        [DataMember(Name = "WaitCount")]
        private int waitcount;

        /// <summary>
        /// 网友ID
        /// </summary>
        public string UserID
        {
            get { return this.userid; }
            set { this.userid = value; }
        }

        /// <summary>
        /// 网友前面等待人数
        /// </summary>
        public int WaitCount
        {
            get { return this.waitcount; }
            set { this.waitcount = value; }
        }
    }
}
