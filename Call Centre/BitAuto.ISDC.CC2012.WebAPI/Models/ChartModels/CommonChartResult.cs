using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace BitAuto.ISDC.CC2012.WebAPI.Models.ChartModels
{
    [DataContract]
    public class CommonChartResult : CommonJsonResult
    {
        [DataMember]
        public new List<series> data { get; set; }
    }
}