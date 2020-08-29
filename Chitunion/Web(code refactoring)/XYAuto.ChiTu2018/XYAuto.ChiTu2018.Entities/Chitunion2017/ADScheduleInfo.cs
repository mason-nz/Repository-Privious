namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ADScheduleInfo")]
    public partial class ADScheduleInfo
    {
        [Key]
        public int ADSID { get; set; }

        public int? ADDetailID { get; set; }

        [StringLength(20)]
        public string OrderID { get; set; }

        [StringLength(20)]
        public string SubOrderID { get; set; }

        public int? MediaID { get; set; }

        public int? PubID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BeginData { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndData { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }
    }
}
