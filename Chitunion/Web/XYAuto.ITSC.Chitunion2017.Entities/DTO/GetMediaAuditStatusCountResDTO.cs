using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    public class GetMediaAuditStatusCountResDTO
    {
        public int Waitting { get; set; }
        public int Pass { get; set; }
        public int Reject { get; set; }
    }
}
