using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.Entities.BatchMediaAudit
{
    public class QueryBatchMediaAudit
    {
        public int TaskType { get; set; } = -2;
        public int MediaType { get; set; } = -2;
        public string MediaName { get; set; } = string.Empty;
        public string MediaNumber { get; set; } = string.Empty;
        public string HeadImg { get; set; } = string.Empty;
        public string HomeUrl { get; set; } = string.Empty;
        public int BrandID { get; set; } = -2;
        public int SerialID { get; set; } = -2;
        public int Status { get; set; } = -2;
    }
}
