using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.LabelTask
{
    public class LB_Project
    {
        public int ProjectID { get; set; } = -2;
        public string Name { get; set; } = string.Empty;
        public int ProjectType { get; set; } = -2;
        public int TaskCount { get; set; } = 0;
        public int GenerateTaskCount { get; set; } = 0;
        public bool IsDeleted { get; set; } = false;
        public int Status { get; set; } = -2;
        public string UploadFileURL { get; set; } = string.Empty;
        public DateTime CreateTime { get; set; } = new DateTime(1990, 1, 1);
        public int CreateUserID { get; set; } = -2;
    }
}
