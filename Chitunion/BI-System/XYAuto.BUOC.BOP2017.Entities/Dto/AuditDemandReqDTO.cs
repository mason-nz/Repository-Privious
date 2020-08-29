using System.Collections.Generic;

namespace XYAuto.BUOC.BOP2017.Entities.Dto
{
    public class AuditDemandReqDto
    {
        public int DemandBillNo { get; set; }
        public int AuditStatus { get; set; }
        public string Reason { get; set; }

        public List<int> ADGroupList { get; set; }
        public int DeliveryId { get; set; }
    }
}