namespace XYAuto.ChiTu2018.Entities.Chitunion2017
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SubADInfo")]
    public partial class SubADInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecID { get; set; }

        [StringLength(20)]
        public string OrderID { get; set; }

        [Key]
        [StringLength(20)]
        public string SubOrderID { get; set; }

        public int? MediaType { get; set; }

        public int? MediaID { get; set; }

        public decimal? TotalAmount { get; set; }

        public int? Status { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? LastUpdateUserID { get; set; }

        public int? ChannelID { get; set; }

        public decimal? CostTotal { get; set; }

        [StringLength(200)]
        public string PostingAddress { get; set; }

        public int? CityExtendID { get; set; }
    }
}
