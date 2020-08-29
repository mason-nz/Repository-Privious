using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.WeixinOAuth
{
    public class UpdateStatistic_Weixin
    {
        public int RecID { get; set; }
        public int WxID { get; set; }
        public DateTime StatisticDate { get; set; }
        public int UpdateCount { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
