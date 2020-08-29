using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.WeixinOAuth
{
    public class ReadStatistic_Weixin
    {
        public int RecID { get; set; }
        public int WxID { get; set; }
        public int ArticleType { get; set; }
        public int AverageReading { get; set; }
        public int MaxReading { get; set; }
        private DateTime fromdate = Constants.Constant.DATE_INVALID_VALUE;
        public DateTime FromDate {
            get { return fromdate; }
            set { fromdate = value; }
        }
        private DateTime enddate = Constants.Constant.DATE_INVALID_VALUE;
        public DateTime EndDate {
            get { return enddate; }
            set { enddate = value; }
        }

        public DateTime CreateTime { get; set; }
    }
}
