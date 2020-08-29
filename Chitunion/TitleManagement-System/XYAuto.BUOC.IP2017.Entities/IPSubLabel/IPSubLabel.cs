using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.Entities.IPSubLabel
{
    public class IPSubLabel
    {
        public int SubIPID { get; set; } = -2;
        public int LabelID { get; set; } = -2;
        public int TitleID { get; set; } = -2;
        public DateTime CreateTime { get; set; } = new DateTime(1900, 1, 1);
        public int CreateUserID { get; set; } = -2;
        public int BatchMediaID { get; set; } = -2;
    }
}
