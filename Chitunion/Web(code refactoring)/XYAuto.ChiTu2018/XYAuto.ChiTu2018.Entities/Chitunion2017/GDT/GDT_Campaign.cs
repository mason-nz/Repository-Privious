namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GDT_Campaign
    {
        public int Id { get; set; }

        public int CampaignId { get; set; }

        [Required]
        [StringLength(200)]
        public string CampaignName { get; set; }

        public int AccountId { get; set; }

        public int? ConfiguredStatus { get; set; }

        public int? CampaignType { get; set; }

        public int? DailyBudget { get; set; }

        public int? BudgetReachDate { get; set; }

        public int? CreatedTime { get; set; }

        public int? LastModifiedTime { get; set; }

        public DateTime? PullTime { get; set; }
    }
}
