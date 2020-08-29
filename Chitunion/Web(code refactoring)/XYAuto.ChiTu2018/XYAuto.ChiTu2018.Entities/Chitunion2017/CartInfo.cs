namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CartInfo")]
    public partial class CartInfo
    {
        [Key]
        public int CartID { get; set; }

        public int? MediaType { get; set; }

        public int? MediaID { get; set; }

        public int? PubID { get; set; }

        public int? PubDetailID { get; set; }

        public bool? IsSelected { get; set; }

        [StringLength(20)]
        public string OrderID { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }

        public int? SaleArea { get; set; }

        public int? ADLaunchDays { get; set; }
    }
}
