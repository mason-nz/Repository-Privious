using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto;

namespace XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.ChannelStat
{
    public class ReqChannelStatDto : CreatePublishQueryBase
    {
        public int PayStatus { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
        public int ChannelId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
        public string SummaryDate { get; set; }
    }
}
