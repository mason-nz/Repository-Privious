namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChannelCostDetail")]
    public partial class ChannelCostDetail
    {
        [Key]
        public int DetailID { get; set; }

        public int? CostID { get; set; }

        public int? ChannelID { get; set; }

        public int? ADPosition1 { get; set; }

        public int? ADPosition2 { get; set; }

        public int? ADPosition3 { get; set; }

        public decimal? CostPrice { get; set; }

        public decimal? SalePrice { get; set; }

        public int? Status { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
