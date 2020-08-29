using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.BOP2017.Entities.ZHY
{
    public class ExceptionDataInfo
    {
        public string RechargeNumber { get; set; }
        public string DemandBillName { get; set; }
        public int DemandBillNo { get; set; }
        public int RechargeAmount { get; set; }
        public int SpendAmount { get; set; }
        public int TransferBackAmount { get; set; }
        public string ExceptionType { get; set; }
    }
}
