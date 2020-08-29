using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace BitAuto.ISDC.CC2012.WebAPI.Models.ChartModels
{
    [Serializable]
    [DataContract]
    //注意：此处为小写，因为前段HighChart只支持小写
    public class series
    {
        [DataMember]
        public string name;
        [DataMember]
        public string color;
        [DataMember]
        public int legendIndex;
        [DataMember]
        public List<ChartNode> data;
        [DataMember]
        public DataLabels dataLabels;
    }
}