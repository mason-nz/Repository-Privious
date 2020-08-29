using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.DSC.APPReport2016.Entities
{
    public class MemberArpu
    {
        public int Year { get; set; }
        public int Quarter { get; set; }
        public int ItemId { get; set; }
        public decimal Amount { get; set; }
        public int TotalDays { get; set; }
        public decimal Arpu { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
