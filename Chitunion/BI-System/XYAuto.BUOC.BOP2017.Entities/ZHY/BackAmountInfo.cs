using System;

namespace XYAuto.BUOC.BOP2017.Entities.ZHY
{
    public class BackAmountInfo
    {
        public string UserName { get; set; }
        public string RechargeNumber { get; set; }
        public int DemandBillNo { get; set; }
        public string DemandStatus { get; set; }
        public decimal RechargeAmount { get; set; }
        public int ConsumeAmount { get; set; }

        public decimal DemandBackAmount
        {
            get
            {
                return (RechargeAmount - ConsumeAmount);
            }
        }

        public int BackAmount { get; set; }
        public DateTime CreateTime { get; set; }
        public string ExternalBillNo { get; set; }
        public string HandleStatus { get; set; }
    }
}