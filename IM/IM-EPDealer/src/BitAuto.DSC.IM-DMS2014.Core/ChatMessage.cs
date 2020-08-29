using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BitAuto.DSC.IM_DMS2014.Core
{
    [DataContract(Name = "cm")]
    public class ChatMessage
    {
        [DataMember(Name = "f")]
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
