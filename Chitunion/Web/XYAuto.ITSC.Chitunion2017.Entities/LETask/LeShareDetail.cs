using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.LETask
{
    public class LeShareDetail
    {
        public int RecId { get; set; }
        public int Type { get; set; }
        public string ShareURL { get; set; }
        public string OrderCoding { get; set; }
        public int CategoryId { get; set; }
        public int ShareResult { get; set; }
        public string IP { get; set; }
        public int Status { get; set; }
        public int CreateUserId { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
