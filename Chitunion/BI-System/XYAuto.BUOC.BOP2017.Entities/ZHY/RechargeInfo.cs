using System;

namespace XYAuto.BUOC.BOP2017.Entities.ZHY
{
    public class RechargeInfo
    {
        public string UserName { get; set; }
        public string RechargeNumber { get; set; }
        public int DemandBillNo { get; set; }
        public decimal RechargeAmount { get; set; }
        public DateTime CreateTime { get; set; }
        public string ExternalBillNo { get; set; }
        public int GdtAmount { get; set; }
        public string RechargeStatus { get; set; }
    }
}