namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GDT_RealtimeCost
    {
        public int Id { get; set; }

        public int? AccountId { get; set; }

        public int Level { get; set; }

        public int? Cost { get; set; }

        public DateTime? Date { get; set; }

        public int? CampaignId { get; set; }

        public int? AdgroupId { get; set; }

        public DateTime? PullTime { get; set; }
    }
}
