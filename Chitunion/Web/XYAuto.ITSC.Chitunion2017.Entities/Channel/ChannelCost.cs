using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Channel
{
    public class ChannelCost
    {
        public int CostID { get; set; }
        public int MediaID { get; set; }
        public int ChannelID { get; set; }
        public int SaleStatus { get; set; }
        public decimal OriginalPrice { get; set; }
        public int Status { get; set; }
        public int CreateUserID { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdateTime { get; set; }

        public string WxNumber { get; set; }
        public string WxName { get; set; }
    }
}
