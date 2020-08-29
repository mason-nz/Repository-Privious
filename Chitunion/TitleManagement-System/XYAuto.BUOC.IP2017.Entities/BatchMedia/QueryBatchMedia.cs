using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.Entities.BatchMedia
{
    public class QueryBatchMedia
    {
        public int BatchMediaID { get; set; } = -2;
        public int TaskType { get; set; } = -2;
        public int BatchAuditID { get; set; } = -2;
        public int MediaType { get; set; } = -2;
        public string MediaName { get; set; } = string.Empty;
        public string MediaNumber { get; set; } = string.Empty;
        public string HeadImg { get; set; } = string.Empty;
        public bool IsSelfDo { get; set; } = false;
        public string HomeUrl { get; set; } = string.Empty;
        public int MediaID { get; set; } = -2;
        public int BrandID { get; set; } = -2;
        public int SerialID { get; set; } = -2;
        public int Status { get; set; } = -2;
        public DateTime SubmitTime { get; set; } = new DateTime(1900, 1, 1);
        public DateTime AuditTime { get; set; } = new DateTime(1900, 1, 1);
        public int AuditUserID { get; set; } = -2;
        public DateTime CreateTime { get; set; } = new DateTime(1900, 1, 1);
        public int CurrentUserID { get; set; } = -2;
    }
}
