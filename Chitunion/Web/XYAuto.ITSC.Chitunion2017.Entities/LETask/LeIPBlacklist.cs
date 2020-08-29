using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.LETask
{
    public class LeIPBlacklist
    {
        public int RecID { get; set; }
        public string IP { get; set; }
        public int ConstraintID { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
