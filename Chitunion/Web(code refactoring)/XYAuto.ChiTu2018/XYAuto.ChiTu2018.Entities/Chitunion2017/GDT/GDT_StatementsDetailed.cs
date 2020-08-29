namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GDT_StatementsDetailed
    {
        public int Id { get; set; }

        public int? AccountId { get; set; }

        public int TradeType { get; set; }

        public int? FundType { get; set; }

        public int? Amount { get; set; }

        public DateTime? Date { get; set; }

        [StringLength(100)]
        public string ExternalBillNo { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public DateTime? PullTime { get; set; }
    }
}
