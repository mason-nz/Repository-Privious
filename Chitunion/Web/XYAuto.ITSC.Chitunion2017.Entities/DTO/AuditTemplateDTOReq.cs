using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    public class AuditTemplateDTOReq
    {
        public int TemplateID { get; set; }
        public string RejectReason { get; set; }
        public int OpType { get; set; }
    }
}
