using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.LabelTask.DTO
{
    public class ReqLabelListQueryDTO
    {
        public int projectType { get; set; } = -2;
        public string keyWord { get; set; } = string.Empty;
        public int Status { get; set; } = -2;
        public int auditUserID { get; set; } = -2;
        public DateTime submitBeginTime { get; set; } = new DateTime(1990, 1, 1);
        public DateTime submitEndTime { get; set; } = new DateTime(1990, 1, 1);
        public DateTime auditBeginTime { get; set; } = new DateTime(1990, 1, 1);
        public DateTime auditEndTime { get; set; } = new DateTime(1990, 1, 1);
        public int pageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 20;
    }
}
