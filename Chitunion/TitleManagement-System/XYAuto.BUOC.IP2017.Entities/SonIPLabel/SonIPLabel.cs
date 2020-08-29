using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.Entities.SonIPLabel
{
    public class SonIPLabel
    {
        public int RecID { get; set; } = -2;
        public int SubIPID { get; set; } = -2;
        public string Name { get; set; } = string.Empty;
        public DateTime CreateTime { get; set; } = new DateTime(1900, 1, 1);
        public int CreateUserID { get; set; } = -2;
        public int BatchMediaID { get; set; } = -2;
    }
}
