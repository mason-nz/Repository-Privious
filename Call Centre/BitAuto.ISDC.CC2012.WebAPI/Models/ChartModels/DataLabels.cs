using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace BitAuto.ISDC.CC2012.WebAPI.Models.ChartModels
{
    [Serializable]
    [DataContract]
    public class DataLabels
    {
        [DataMember]
        public bool enabled { get; set; }
        [DataMember]
        public int rotation { get; set; }
        [DataMember]
        public string color { get; set; }
        [DataMember]
        public string align { get; set; }
        [DataMember]
        public int x { get; set; }
        [DataMember]
        public int y { get; set; }
        [DataMember]
        public Style style { get; set; }
    }
}