namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AreaInfo")]
    public partial class AreaInfo
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string AreaID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string PID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string AreaName { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(100)]
        public string AbbrName { get; set; }

        [Key]
        [Column(Order = 4)]
        public DateTime CreateTime { get; set; }

        public int? Level { get; set; }

        public int? Status { get; set; }
    }
}
