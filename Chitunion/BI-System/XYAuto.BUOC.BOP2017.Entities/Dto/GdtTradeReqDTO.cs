using System.Collections.Generic;

namespace XYAuto.BUOC.BOP2017.Entities.Dto
{
    public class GdtTradeReqDTO
    {
        public int TradeType { get; set; }
        public int DemandBillNo { get; set; }
        public List<string> GdtNumList { get; set; }
    }
}