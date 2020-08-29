using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace BitAuto.ISDC.CC2012.WebAPI.Models.ChartModels
{
    [Serializable]
    [DataContract]
    public class Style
    {
        [DataMember]
        public string fontSize { get; set; }
        [DataMember]
        public string fontFamily { get; set; }
        [DataMember]
        public string fontWeight { get; set; }
        [DataMember]
        public string color { get; set; }
    }
}