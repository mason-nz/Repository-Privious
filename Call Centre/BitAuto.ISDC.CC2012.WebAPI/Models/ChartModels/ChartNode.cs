using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace BitAuto.ISDC.CC2012.WebAPI.Models.ChartModels
{
    [Serializable]
    [DataContract]
    public class ChartNode
    {
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string color { get; set; }
        [DataMember]
        public bool sliced { get; set; }
        [DataMember]
        public int x { get; set; }
        [DataMember]
        public double y { get; set; }
    }
}