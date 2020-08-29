using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Channel
{
    public class ChannelInfo
    {
        public int ChannelID { get; set; }
        public string ChannelName { get; set; }
        public bool IncludingTax { get; set; }
        public DateTime CooperateBeginDate { get; set; }
        public DateTime CooperateEndDate { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public int CreateUserID { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}
