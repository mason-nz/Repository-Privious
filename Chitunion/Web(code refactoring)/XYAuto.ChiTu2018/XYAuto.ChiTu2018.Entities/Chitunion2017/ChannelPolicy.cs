namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChannelPolicy")]
    public partial class ChannelPolicy
    {
        [Key]
        public int PolicyID { get; set; }

        public int? ChannelID { get; set; }

        public int? Quota { get; set; }

        public bool? QuotaIncludingEqual { get; set; }

        public int? SingleAccountSum { get; set; }

        public int? SingleAccountSumType { get; set; }

        public decimal? PurchaseDiscount { get; set; }

        public int? RebateType1 { get; set; }

        public int? RebateType2 { get; set; }

        public decimal? RebateValue { get; set; }

        public int? RebateDateType { get; set; }

        public int? Status { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? LastUpdateTime { get; set; }
    }
}
