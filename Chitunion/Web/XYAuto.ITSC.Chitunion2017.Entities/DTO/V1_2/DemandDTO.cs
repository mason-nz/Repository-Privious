using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_2
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
    }


}
