using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.ISDC.CC2012.Entities
{
    public class ClientLogRequireQuery
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Name { get; set; }
        public string Vendor { get; set; }
        public string Online { get; set; }
    }
}
