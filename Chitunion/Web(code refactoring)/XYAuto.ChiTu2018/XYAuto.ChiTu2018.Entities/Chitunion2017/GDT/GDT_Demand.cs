namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GDT_Demand
    {
        [Key]
        public int DemandId { get; set; }

        public int? DemandBillNo { get; set; }

        [Required]
        [StringLength(500)]
        public string Name { get; set; }

        public int Status { get; set; }

        [Column(TypeName = "text")]
        public string BrandSerialJson { get; set; }

        [Column(TypeName = "text")]
        public string ProvinceCityJson { get; set; }

        [Column(TypeName = "text")]
        public string DistributorJson { get; set; }

        [StringLength(1000)]
        public string PromotionPolicy { get; set; }

        public decimal? TotalBudget { get; set; }

        public int? ClueNumber { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int AuditStatus { get; set; }

        public int? CreateUserId { get; set; }

        public int? UpdateUserId { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }
    }
}
