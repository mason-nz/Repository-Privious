using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Senparc.Weixin.Entities;

namespace XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Response.ChannelStat
{
    public class RespChannelStatDto
    {
        public int StatisticsId { get; set; }
        [JsonIgnore]
        public DateTime DtMonth { get; set; }
        [JsonIgnore]
        public DateTime DtSummaryDate { get; set; }
        public string Month { get; set; }
        public string SummaryDate { get; set; }
        public string ChannelName { get; set; }
        public decimal TotalAmount { get; set; }
        public int PayStatus { get; set; }
        public string PayStatusName { get; set; }
        public DateTime? PayTime { get; set; }
        public int OrderNumber { get; set; }
    }
}
