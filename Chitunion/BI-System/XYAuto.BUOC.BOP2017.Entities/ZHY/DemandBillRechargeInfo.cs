using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.BOP2017.Entities.ZHY
{
    public class DemandBillRechargeInfo
    {
        public string UserName { get; set; }
        public string RechargeNumber { get; set; }
        public int DemandBillNo { get; set; }
        public decimal RechargeAmount { get; set; }
        public DateTime RequestTime { get; set; }
        public DateTime? ConfirmTime { get; set; }

    }
}
