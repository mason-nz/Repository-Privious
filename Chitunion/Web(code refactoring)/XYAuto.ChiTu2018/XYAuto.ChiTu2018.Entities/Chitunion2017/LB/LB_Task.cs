namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LB_Task
    {
        [Key]
        public int TaskID { get; set; }

        [StringLength(100)]
        public string ProjectID { get; set; }

        public int? MediaType { get; set; }

        public int? MediaID { get; set; }

        [StringLength(100)]
        public string MediaName { get; set; }

        [StringLength(100)]
        public string MediaNum { get; set; }

        public int? ArticleID { get; set; }

        [StringLength(100)]
        public string ArticleTitle { get; set; }

        public int? KeyWord { get; set; }

        public int? Summary { get; set; }

        public int? Status { get; set; }

        public DateTime? SubmitTime { get; set; }

        public DateTime? AuditTime { get; set; }

        public int? AuditUserID { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }
    }
}
