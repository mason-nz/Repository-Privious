using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.LETask
{
    public class LeChannelStatMonthRelation
    {
        public int RecId { get; set; }
        public int StatisticsId { get; set; }
        public int PayStatus { get; set; }
        public DateTime PayTime { get; set; }
        public int CreateUserId { get; set; }
        public DateTime CreateTime { get; set; }
        public int Status { get; set; }
        public string Reason { get; set; }
    }
}
