using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_2
{
    public class MapGetRightReqDTO
    {
        public int DemandBillNo { get; set; }
        public int ADGroupID { get; set; }
        public int DateType { get; set; }
        public int DataType { get; set; }
        public DateTime BeginDate { get; set; } = Entities.Constants.Constant.DATE_INVALID_VALUE;
        public DateTime EndDate { get; set; } = Entities.Constants.Constant.DATE_INVALID_VALUE;
        public int OrderBy { get; set; }
        public int IsDesc { get; set; }

        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = Entities.Constants.Constant.PageSize;
    }
}
