namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChannelCost")]
    public partial class ChannelCost
    {
        [Key]
        public int CostID { get; set; }

        public int? MediaID { get; set; }

        public int? ChannelID { get; set; }

        public int? SaleStatus { get; set; }

        public decimal? OriginalPrice { get; set; }

        public int? Status { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? LastUpdateTime { get; set; }
    }
}
