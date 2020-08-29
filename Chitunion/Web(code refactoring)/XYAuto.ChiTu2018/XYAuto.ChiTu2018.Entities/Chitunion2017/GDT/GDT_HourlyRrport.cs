namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GDT_HourlyRrport
    {
        public int Id { get; set; }

        public int? AccountId { get; set; }

        public int? Level { get; set; }

        public int? Hour { get; set; }

        public int? CampaignId { get; set; }

        public int? AdgroupId { get; set; }

        public int? Impression { get; set; }

        public int? Click { get; set; }

        public int? Cost { get; set; }

        public int? Download { get; set; }

        public int? Conversion { get; set; }

        public int? Activation { get; set; }

        public int? AppPaymentCount { get; set; }

        public int? AppPaymentAmount { get; set; }

        public int? LikeOrComment { get; set; }

        public int? ImageClick { get; set; }

        public int? Follow { get; set; }

        public int? Share { get; set; }

        public DateTime? Date { get; set; }

        public DateTime? PullTime { get; set; }

        public int OrderQuantity { get; set; }

        public int BillOfQuantities { get; set; }
    }
}
