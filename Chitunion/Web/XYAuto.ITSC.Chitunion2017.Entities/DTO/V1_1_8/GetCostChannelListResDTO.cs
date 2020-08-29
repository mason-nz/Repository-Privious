using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_8
{
    public class GetCostChannelListResDTO
    {
        public List<GetCostChannelItem> List { get; set; }
    }

    public class GetCostChannelItem
    {
        public int ChannelID { get; set; }
        public string ChannelName { get; set; }
    }
}
