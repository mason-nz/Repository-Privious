using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_8
{
    public class GetChannelListResDTO
    {
        public List<ChannelItem> List { get; set; }
        public int TotalCount { get; set; }
    }

    public class ChannelItem
    {
        public int ChannelID { get; set; }
        public string ChannelName { get; set; }
        public int WxCount { get; set; }
        public decimal TotalPrice { get; set; }
        public string CooperateDate { get; set; }
        public string StatusName { get; set; }
        public string CreateUserName { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
