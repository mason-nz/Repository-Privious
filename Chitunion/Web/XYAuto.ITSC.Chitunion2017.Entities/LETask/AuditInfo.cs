using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.LETask
{
    public class AuditInfo
    {
        public int Id { get; set; }
        public int RelationType { get; set; }
        public int RelationId { get; set; }
        public int AuditStatus { get; set; }
        public string RejectMsg { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public int CreateUserId { get; set; }
        public string CreateUserName { get; set; }
        public string AuditStatusName { get; set; }
    }
}
