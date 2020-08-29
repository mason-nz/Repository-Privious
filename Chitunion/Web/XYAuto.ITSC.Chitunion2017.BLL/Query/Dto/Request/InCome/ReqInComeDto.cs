using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto;

namespace XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.InCome
{
    public class ReqInComeDto : CreatePublishQueryBase
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int OrderType { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
        public int PayType { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
    }
}
