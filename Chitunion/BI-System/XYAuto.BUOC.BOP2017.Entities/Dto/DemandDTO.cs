using System;
using System.Collections.Generic;

namespace XYAuto.BUOC.BOP2017.Entities.Dto
{
    public class DemandDTO : Entities.GDT.GdtDemand
    {
        public string CreateUserName { get; set; }
        public DateTime AuditTime { get; set; } = Entities.Constants.Constant.DATE_INVALID_VALUE;
        public string BelongYY { get; set; }
        public string AuditStatusName { get; set; }
        public int ADCount { get; set; }

        public List<string> BrandAndCarSerialList { get; set; } = new List<string>();
        public List<string> ProvinceAndCityList { get; set; } = new List<string>();
        public List<string> DistributorList { get; set; } = new List<string>();
        public string DuringDate { get; set; }
        public string ClueNumberStr { get; set; }

        public DateTime ADLastUpdateTime { get; set; } = Entities.Constants.Constant.DATE_INVALID_VALUE;
        public string RejectReason { get; set; }

        public DateTime RevokeTime { get; set; }//需求撤销时间
        public int DeliveryCount { get; set; }//加参落地页数量

        public string RechargeNumber { get; set; }
    }
}