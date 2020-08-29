using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;

namespace XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.AdOrder
{
    public class ReqOrderCoverImageDto : CreatePublishQueryBase
    {
        public string OrderName { get; set; }
        public int OrderStatus { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;


        public LeTaskTypeEnum TaskType { get; set; } = LeTaskTypeEnum.None;

        public int Category { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
    }
}
