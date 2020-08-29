namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GDT_AccountBalance
    {
        public int Id { get; set; }

        public int? AccountId { get; set; }

        public int FundType { get; set; }

        public int? Balance { get; set; }

        public int? FundStatus { get; set; }

        public int? RealtimeCost { get; set; }

        public DateTime? PullTime { get; set; }

        public int Status { get; set; }
    }
}
