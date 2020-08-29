using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request
{
    public class ReqChannelStatMonthPayDto
    {
        [Necessary(MtName = "StatisticsId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入StatisticsId")]
        public int StatisticsId { get; set; }
        [Necessary(MtName = "PayStatus", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入StatisticsId")]
        public int PayStatus { get; set; } = (int)ChannelStatMonthPayStatusEnum.已支付;
    }
}
