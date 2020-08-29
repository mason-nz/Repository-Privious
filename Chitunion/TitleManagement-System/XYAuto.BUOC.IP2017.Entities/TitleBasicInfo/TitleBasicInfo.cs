using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.Entities.TitleBasicInfo
{
    public class TitleBasicInfo
    {
        public int TitleID { get; set; } = -2;
        public string Name { get; set; } = string.Empty;
        public int Type { get; set; } = -2;
        public int Status { get; set; } = 0;
        public DateTime CreateTime { get; set; } = new DateTime(1900, 1, 1);
        public int CreateUserID { get; set; } = -2;
    }
}
