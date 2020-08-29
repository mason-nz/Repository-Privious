using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Channel
{
    public class ChannelCostDetail
    {
        public int DetailID { get; set; }
        public int CostID { get; set; }
        public int ChannelID { get; set; }
        public int ADPosition1 { get; set; }
        public int ADPosition2 { get; set; }
        public int ADPosition3 { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public int Status { get; set; }
        public int CreateUserID { get; set; }
        public DateTime CreateTime { get; set; }

        public int MediaID { get; set; }
        public string ADPosition { get; set; }
        public string WxName { get; set; }
        public string WxNumber { get; set; }
        public decimal OriginalPrice { get; set; }

        public string ChannelName { get; set; }
        public DateTime CooperateBeginDate { get; set; }
        public DateTime CooperateEndDate { get; set; }

    }
}
