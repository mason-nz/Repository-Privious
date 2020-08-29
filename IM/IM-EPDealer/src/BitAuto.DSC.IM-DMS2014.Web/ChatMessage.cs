using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Runtime.Serialization;
namespace BitAuto.DSC.IM2014.Server.Web
{
    [DataContract(Name = "cm")]
    public class ChatMessage
    {
        [DataMember(Name = "f")]
        private string from;
        [DataMember(Name = "m")]
        private string message;

        [DataMember(Name = "UserReferURL")]
        private string userreferurl;
        [DataMember(Name = "AllocID")]
        private long allocid;
        [DataMember(Name = "LocalIP")]
        private string localip;
        [DataMember(Name = "Location")]
        private string location;
        [DataMember(Name = "EnterTime")]
        private string entertime;
        [DataMember(Name = "WaitTime")]
        private int waittime;
        [DataMember(Name = "TalkTime")]
        private string talktime;

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
        /// <summary>
        /// 坐席分配ID
        /// </summary>
        public long AllocID
        {
            get { return this.allocid; }
            set { this.allocid = value; }
        }
        
        /// <summary>
        /// 网友来源
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
    }
}