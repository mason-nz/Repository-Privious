namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GDT_Ads
    {
        public int Id { get; set; }

        public int? AccountId { get; set; }

        public int CampaignId { get; set; }

        public int AdgroupId { get; set; }

        public int? AdId { get; set; }

        [StringLength(200)]
        public string AdName { get; set; }

        [StringLength(50)]
        public string ConfiguredStatus { get; set; }

        [StringLength(50)]
        public string SystemStatus { get; set; }

        [StringLength(200)]
        public string RejectMessage { get; set; }

        public int? CreatedTime { get; set; }

        public int? LastModifiedTime { get; set; }

        public DateTime? PullTime { get; set; }
    }
}
