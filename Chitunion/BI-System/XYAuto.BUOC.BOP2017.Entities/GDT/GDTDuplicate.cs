using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.BOP2017.Entities.GDT
{
    public class GDTDuplicate
    {
        public int ID { get; set; }
        public int ClueID { get; set; }
        public int Results { get; set; }
        public string Reason { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
