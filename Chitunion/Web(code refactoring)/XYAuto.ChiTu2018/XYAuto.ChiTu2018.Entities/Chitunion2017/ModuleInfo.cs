namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ModuleInfo")]
    public partial class ModuleInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecID { get; set; }

        [StringLength(50)]
        public string SysID { get; set; }

        public int? DomainID { get; set; }

        [Key]
        [StringLength(50)]
        public string ModuleID { get; set; }

        [StringLength(100)]
        public string ModuleName { get; set; }

        [StringLength(50)]
        public string PID { get; set; }

        public int? Level { get; set; }

        [StringLength(1000)]
        public string Intro { get; set; }

        [StringLength(200)]
        public string Url { get; set; }

        [StringLength(2000)]
        public string Links { get; set; }

        public int? OrderNum { get; set; }

        public int? Status { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }
    }
}
