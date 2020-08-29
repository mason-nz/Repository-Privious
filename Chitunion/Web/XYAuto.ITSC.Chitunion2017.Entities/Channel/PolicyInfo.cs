using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.Entities.Channel
{
    public class PolicyInfo
    {
        public PolicyInfo()
        {
            this.RebateType1 = (int)RebateType1Enum.无;

            this.SingleAccountSumType = Constants.Constant.INT_INVALID_VALUE;
            this.RebateType2 = Constants.Constant.INT_INVALID_VALUE;
            this.RebateDateType = Constants.Constant.INT_INVALID_VALUE;
        }

        public int PolicyID { get; set; }
        public int ChannelID { get; set; }
        public int Quota { get; set; }
        public bool QuotaIncludingEqual { get; set; }
        public int SingleAccountSum { get; set; }
        public int SingleAccountSumType { get; set; }
        public decimal PurchaseDiscount { get; set; }
        public int RebateType1 { get; set; }
        public int RebateType2 { get; set; }
        public decimal RebateValue { get; set; }
        public int RebateDateType { get; set; }
        public int Status { get; set; }
        public int CreateUserID { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdateTime { get; set; }

        //public bool IncludingEqual { get; set; }
    }
}
