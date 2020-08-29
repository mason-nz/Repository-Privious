using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.BOP2017.Entities.Enum.GDT;

namespace XYAuto.BUOC.BOP2017.Entities.Demand
{
    public class DemandRefundInfo
    {
        public string RechargeNumber { get; set; }
        public int DemandBillNo { get; set; }
        public int AccountId { get; set; }
        public TradeTypeEnum TradeType { get; set; }
        public int RechargeAmount { get; set; }
        public int SpendAmount { get; set; }
        public int TransferBackAmount { get; set; }
        public DateTime CreateTime { get; set; }
        public int CreateUserId { get; set; }
    }
}
