using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_TaskInfo
    {
        [Key]
        public int RecID { get; set; }

        [StringLength(100)]
        public string TaskName { get; set; }

        [StringLength(50)]
        public string BillingRuleName { get; set; }

        [StringLength(200)]
        public string MaterialUrl { get; set; }

        public int? MaterialID { get; set; }

        public int? TaskType { get; set; }

        public int? RuleCount { get; set; }

        public int? TakeCount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BeginTime { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndTime { get; set; }

        public int? Status { get; set; }

        public decimal? TaskAmount { get; set; }

        public decimal? CPCPrice { get; set; }

        public decimal? CPLPrice { get; set; }

        public DateTime? CreateTime { get; set; }

        [StringLength(200)]
        public string ImgUrl { get; set; }

        [StringLength(1024)]
        public string Synopsis { get; set; }

        public int? CategoryID { get; set; }

        public decimal? CPCLimitPrice { get; set; }

        public decimal? CPLLimitPrice { get; set; }
    }
}
