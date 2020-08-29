namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ADOrderInfoExtend")]
    public partial class ADOrderInfoExtend
    {
        [Key]
        public int RecID { get; set; }

        [Required]
        [StringLength(20)]
        public string OrderID { get; set; }

        public decimal? BudgetTotal { get; set; }

        [StringLength(200)]
        public string OrderRemark { get; set; }

        [StringLength(200)]
        public string MasterBrand { get; set; }

        [StringLength(100)]
        public string CarBrand { get; set; }

        [StringLength(100)]
        public string CarSerial { get; set; }

        public DateTime? LaunchTime { get; set; }

        public bool? JKEntrance { get; set; }

        [StringLength(200)]
        public string MarketingPolices { get; set; }

        [StringLength(200)]
        public string MarketingUrl { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }

        public int? MasterID { get; set; }

        public int? BrandID { get; set; }

        public int? SerialID { get; set; }
    }
}
