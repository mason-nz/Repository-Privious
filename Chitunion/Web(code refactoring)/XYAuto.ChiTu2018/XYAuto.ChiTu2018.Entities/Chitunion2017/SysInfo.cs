namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SysInfo")]
    public partial class SysInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecID { get; set; }

        [Key]
        [StringLength(50)]
        public string SysID { get; set; }

        [StringLength(200)]
        public string SysName { get; set; }

        [StringLength(200)]
        public string SysURL { get; set; }

        public int? Status { get; set; }

        [StringLength(1000)]
        public string Intro { get; set; }

        public int? OrderNum { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }
    }
}
