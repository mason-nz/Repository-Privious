namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LB_Project
    {
        [Key]
        public int ProjectID { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public int? ProjectType { get; set; }

        public int? TaskCount { get; set; }

        public int? Status { get; set; }

        [StringLength(200)]
        public string UploadFileURL { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }

        public bool? IsDeleted { get; set; }

        public int? GenerateTaskCount { get; set; }
    }
}
