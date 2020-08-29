namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RoleModule")]
    public partial class RoleModule
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecID { get; set; }

        [Required]
        [StringLength(50)]
        public string SysID { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string RoleID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string ModuleID { get; set; }

        [StringLength(500)]
        public string Intro { get; set; }

        public int? Status { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }
    }
}
