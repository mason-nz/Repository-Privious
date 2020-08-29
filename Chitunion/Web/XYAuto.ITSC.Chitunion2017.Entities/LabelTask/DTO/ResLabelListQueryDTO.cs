using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.LabelTask.DTO
{
    public class ResLabelListQueryDTO
    {
        public List<ResLabelModelQueryDTO> List { get; set; }
        public int TotalCount { get; set; } = 0;
    }
    public class ResLabelModelQueryDTO : LB_Task
    {
        public string AuditUserName { get; set; } = string.Empty;
        public int ProjectType { get; set; } = -2;
    }
}
