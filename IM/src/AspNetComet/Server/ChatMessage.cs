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

namespace Server
{
    [DataContract(Name="cm")]
    public class ChatMessage
    {
        [DataMember(Name="f")]
        private string from;
        [DataMember(Name = "m")]
        private string message;

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

    }
}
