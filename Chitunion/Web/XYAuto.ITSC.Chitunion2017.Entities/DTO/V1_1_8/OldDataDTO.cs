using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Channel;
using XYAuto.ITSC.Chitunion2017.Entities.Media;
using XYAuto.ITSC.Chitunion2017.Entities.Publish;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_8
{
    public class OldDataDTO
    {
        public List<MediaWeixin> MediaList { get; set; }
        public List<MediaCommonlyClass> CategoryList { get; set; }
        public List<PublishRemark> RemarkList { get; set; }
        public List<MediaAreaMapping> AreaList { get; set; }
        public List<ChannelCost> CostList { get; set; }
        public List<ChannelCostDetail> PriceList { get; set; }
    }
}
