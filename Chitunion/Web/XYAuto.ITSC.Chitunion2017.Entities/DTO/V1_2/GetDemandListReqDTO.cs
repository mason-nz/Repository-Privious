using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_2
{
    public class GetDemandListReqDTO
    {
        public string CreateUser { get; set; }
        public string BelongYY { get; set; }
        public string DemandName { get; set; }
        public string TabType { get; set; }
        public int AuditStatus { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
        public int PageSize { get; set; } = Entities.Constants.Constant.PageSize;
        public int PageIndex { get; set; } = 1;
    }
}
