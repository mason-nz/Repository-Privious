namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ADDetailInfo")]
    public partial class ADDetailInfo
    {
        [Key]
        public int RecID { get; set; }

        [StringLength(20)]
        public string OrderID { get; set; }

        [StringLength(20)]
        public string SubOrderID { get; set; }

        public int? MediaType { get; set; }

        public int? MediaID { get; set; }

        public int? PubID { get; set; }

        public int? PubDetailID { get; set; }

        [StringLength(100)]
        public string ADLaunchIDs { get; set; }

        [StringLength(500)]
        public string ADLaunchStr { get; set; }

        public decimal? OriginalPrice { get; set; }

        public decimal? AdjustPrice { get; set; }

        public decimal? AdjustDiscount { get; set; }

        public decimal? PurchaseDiscount { get; set; }

        public decimal? SaleDiscount { get; set; }

        public int? ADLaunchDays { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? LastUpdateUserID { get; set; }

        public int? SaleArea { get; set; }

        public int? Holidays { get; set; }

        public bool? HasHoliday { get; set; }

        public decimal? SalePrice_Holiday { get; set; }

        public decimal? CostReferencePrice { get; set; }

        public decimal? OriginalReferencePrice { get; set; }

        public bool? EnableOriginPrice { get; set; }

        public decimal? CostPrice { get; set; }

        public decimal? FinalCostPrice { get; set; }
    }
}
