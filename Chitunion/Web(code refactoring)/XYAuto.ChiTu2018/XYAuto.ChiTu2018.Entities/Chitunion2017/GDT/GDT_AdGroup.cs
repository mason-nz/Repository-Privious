namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GDT_AdGroup
    {
        public int Id { get; set; }

        public int? AccountId { get; set; }

        public int CampaignId { get; set; }

        public int AdgroupId { get; set; }

        [StringLength(200)]
        public string AdgroupName { get; set; }

        public int? ConfiguredStatus { get; set; }

        public int? SystemStatus { get; set; }

        [StringLength(200)]
        public string RejectMessage { get; set; }

        [StringLength(1000)]
        public string SiteSet { get; set; }

        public int? OptimizationGoal { get; set; }

        public int? BillingEvent { get; set; }

        public int? BidAmount { get; set; }

        public int? DailyBudget { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(500)]
        public string TimeSeries { get; set; }

        [StringLength(200)]
        public string CustomizedCategory { get; set; }

        public int? CreatedTime { get; set; }

        public int? LastModifiedTime { get; set; }

        public DateTime? PullTime { get; set; }
    }
}
