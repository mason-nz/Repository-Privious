using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace BitAuto.ISDC.CC2012.WebAPI.Models
{
    [Serializable]
    [DataContract]
    public class CommonJsonResult
    {
        [DataMember]
        public bool Success { get; set; }
        [DataMember]
        public object data { get; set; }
        [DataMember]
        public int ErrorNumber { get; set; }
        [DataMember]
        public string ErrorMsg { get; set; }
    }
}