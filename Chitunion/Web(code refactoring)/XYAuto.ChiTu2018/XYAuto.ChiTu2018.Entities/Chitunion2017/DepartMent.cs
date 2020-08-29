namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DepartMent")]
    public partial class DepartMent
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecID { get; set; }

        [Key]
        [StringLength(50)]
        public string DepartID { get; set; }

        [Required]
        [StringLength(50)]
        public string Pid { get; set; }

        [Required]
        [StringLength(100)]
        public string DepartName { get; set; }

        [StringLength(50)]
        public string AbbrName { get; set; }

        public int? Level { get; set; }

        public int? Status { get; set; }

        public DateTime? CreateTime { get; set; }

        [StringLength(50)]
        public string BusinessLineID { get; set; }

        [StringLength(50)]
        public string DistrictID { get; set; }

        [StringLength(50)]
        public string Gid { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(50)]
        public string PostCode { get; set; }

        [StringLength(50)]
        public string Tele { get; set; }

        [StringLength(50)]
        public string Fax { get; set; }

        [StringLength(1000)]
        public string Remark { get; set; }

        public int? DepartType { get; set; }

        public int? DepartKind { get; set; }

        [StringLength(1000)]
        public string DepartPath { get; set; }

        [StringLength(10)]
        public string StopTime { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] TIMESTAMP { get; set; }

        public int? DepartCityType { get; set; }

        [StringLength(1000)]
        public string NamePath { get; set; }

        public int? BussinessCategory { get; set; }

        public int? ManagerID { get; set; }
    }
}
