using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request
{
    public class ReqTaskReceiveDto
    {
        [Necessary(MtName = "TaskType", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入TaskType")]
        public int TaskType { get; set; }
        [Necessary(MtName = "TaskId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入TaskId")]
        public int TaskId { get; set; }
        public int MediaId { get; set; }
        public string IP { get; set; }

        public int ChannelId { get; set; } = (int)LeOrderChannelTypeEnum.赤兔;

        public string OrderUrl { get; set; }

        public int UserId { get; set; }

        public int ShareType { get; set; }

        public long PromotionChannelId { get; set; }
    }
}
