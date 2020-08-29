using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_2
{
    public class GdtTradeReqDTO
    {
        public int TradeType { get; set; }
        public int DemandBillNo { get; set; }
        public List<string> GdtNumList { get; set; }

    }
}
