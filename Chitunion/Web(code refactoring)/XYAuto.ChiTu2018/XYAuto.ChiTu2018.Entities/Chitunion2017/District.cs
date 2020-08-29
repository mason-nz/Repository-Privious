namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("District")]
    public partial class District
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecID { get; set; }

        [StringLength(50)]
        public string DistrictID { get; set; }

        [Required]
        [StringLength(50)]
        public string Pid { get; set; }

        [Required]
        [StringLength(100)]
        public string DistrictName { get; set; }

        [StringLength(50)]
        public string AbbrName { get; set; }

        public int? Level { get; set; }

        public int? Status { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
