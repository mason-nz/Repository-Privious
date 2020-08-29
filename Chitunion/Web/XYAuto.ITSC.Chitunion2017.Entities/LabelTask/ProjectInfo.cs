using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static XYAuto.ITSC.Chitunion2017.Entities.LabelTask.ENUM;

namespace XYAuto.ITSC.Chitunion2017.Entities.LabelTask
{
    public class ProjectInfo
    {
        public int ProjectID { get; set; }
        public string Name { get; set; }
        public EnumProjectType ProjectType { get; set; }
        public EnumProjectStatus Status { get; set; }
        public int TaskCount { get; set; }
        public int GenerateTaskCount { get; set; }
        public int DealCount { get; set; }
        public int NoDealCount { get; set; }
        public int SingleCount { get; set; }
        public int ExamineCount { get; set; }
        public string CreatorName { get; set; }
        public DateTime CreateTime { get; set; }
        public string UploadFileName { get; set; }
        public string UploadFileURL { get; set; }
    }
}
