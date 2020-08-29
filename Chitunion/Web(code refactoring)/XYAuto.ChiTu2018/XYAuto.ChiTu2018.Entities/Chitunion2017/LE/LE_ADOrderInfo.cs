using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XYAuto.ChiTu2018.Entities.Extend;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_ADOrderInfo
    {
        [Key]
        public int RecID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BeginTime { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndTime { get; set; }

        public decimal? TotalAmount { get; set; }

        public int? Status { get; set; }

        public int? OrderType { get; set; }

        [StringLength(200)]
        public string OrderName { get; set; }

        [StringLength(200)]
        public string OrderUrl { get; set; }

        [StringLength(200)]
        public string PasterUrl { get; set; }

        public int? UserID { get; set; }

        public int? TaskID { get; set; }

        public DateTime? CreateTime { get; set; }

        [StringLength(50)]
        public string BillingRuleName { get; set; }

        [StringLength(50)]
        public string OrderCoding { get; set; }

        public int? MediaType { get; set; }

        public int? MediaID { get; set; }

        public int? ChannelID { get; set; }

        [StringLength(50)]
        public string UserIdentity { get; set; }

        public decimal? CPCUnitPrice { get; set; }

        public decimal? CPLUnitPrice { get; set; }

        public int? StatisticsStatus { get; set; }

        [StringLength(50)]
        public string IP { get; set; }

        public long? PromotionChannelID { get; set; }
    }

    /// <summary>
    /// ²éÑ¯Ìõ¼þ
    /// </summary>
    //public sealed class LE_ADOrderInfoQuery : Pagination
    //{
    //    public int UserID { get; set; } = -2;
    //    public int Status { get; set; } = -2;
    //}
}
