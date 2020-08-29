using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.BOP2017.Entities.ZHY
{
    public class DemandBillSpendInfo
    {
        public int DemandBillNo { get; set; }
        public string DemandBillName { get; set; }
        public decimal RechargeAmount { get; set; }
        public int SpendAmount { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
