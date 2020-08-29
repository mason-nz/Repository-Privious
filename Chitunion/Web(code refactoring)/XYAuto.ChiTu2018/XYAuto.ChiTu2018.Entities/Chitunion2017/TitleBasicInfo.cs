namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TitleBasicInfo")]
    public partial class TitleBasicInfo
    {
        [Key]
        public int TitleID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int? Type { get; set; }

        public int? Status { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }
    }
}
