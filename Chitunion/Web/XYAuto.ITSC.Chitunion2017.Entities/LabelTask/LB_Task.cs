using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.LabelTask
{
    public class LB_Task
    {
        public int TaskID { get; set; } = -2;
        public int ProjectID { get; set; } = -2;
        public int MediaType { get; set; } = -2;
        public int MediaID { get; set; } = -2;
        public string MediaNum { get; set; } = string.Empty;
        public string MediaName { get; set; } = string.Empty;
        public int ArticleID { get; set; } = -2;
        public string ArticleTitle { get; set; } = string.Empty;
        public int Status { get; set; } = -2;
        public DateTime SubmitTime { get; set; } = new DateTime(1990, 1, 1);
        public DateTime AuditTime { get; set; } = new DateTime(1990, 1, 1);
        [JsonIgnore]
        public int AuditUserID { get; set; } = -2;
        public DateTime CreateTime { get; set; } = new DateTime(1990, 1, 1);
        [JsonIgnore]
        public int CreateUserID { get; set; } = -2;
        [JsonIgnore]
        public int TaskCount { get; set; } = 0;
    }
    public class LB_TaskModel
    {
        public int TaskID { get; set; } = -2;
        public int ProjectID { get; set; } = -2;
        public int MediaType { get; set; } = -2;
        public int MediaID { get; set; } = -2;
        public string MediaName { get; set; } = string.Empty;
        public string MediaNum { get; set; } = string.Empty;
        public int ArticleID { get; set; } = -2;
        public string ArticleTitle { get; set; } = string.Empty;
        public int Status { get; set; } = -2;
        public DateTime SubmitTime { get; set; } = new DateTime(1990, 1, 1);
        public DateTime AuditTime { get; set; } = new DateTime(1990, 1, 1);
        public int AuditUserID { get; set; } = -2;
        public DateTime CreateTime { get; set; } = new DateTime(1990, 1, 1);
        public int CreateUserID { get; set; } = -2;
    }
}
