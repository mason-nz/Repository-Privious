namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BusinessLine")]
    public partial class BusinessLine
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecID { get; set; }

        [StringLength(50)]
        public string BusinessLineID { get; set; }

        [Required]
        [StringLength(50)]
        public string Pid { get; set; }

        [Required]
        [StringLength(100)]
        public string BusinessLineName { get; set; }

        [StringLength(50)]
        public string AbbrName { get; set; }

        public int? Level { get; set; }

        public int? Status { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
