namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GDT_AccountInfo
    {
        [Key]
        public int GDTUserId { get; set; }

        public int AccountId { get; set; }

        public int DailyBudget { get; set; }

        public int? SystemStatus { get; set; }

        [StringLength(200)]
        public string RejectMessage { get; set; }

        [StringLength(200)]
        public string CorporationName { get; set; }

        [StringLength(100)]
        public string ContactPerson { get; set; }

        [StringLength(100)]
        public string ContactPersonTelephone { get; set; }

        public DateTime? PullTime { get; set; }
    }
}
