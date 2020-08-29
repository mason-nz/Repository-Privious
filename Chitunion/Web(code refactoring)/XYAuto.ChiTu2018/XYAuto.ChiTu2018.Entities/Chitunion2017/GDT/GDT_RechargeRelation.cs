namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GDT_RechargeRelation
    {
        public int Id { get; set; }

        public int? DemandBillNo { get; set; }

        [Required]
        [StringLength(200)]
        public string RechargeNumber { get; set; }

        public decimal? Amount { get; set; }

        public int? CreateUserId { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? RechargeStatus { get; set; }
    }
}
