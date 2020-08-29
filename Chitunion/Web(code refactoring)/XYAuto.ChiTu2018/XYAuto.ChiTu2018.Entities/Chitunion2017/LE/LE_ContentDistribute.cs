using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_ContentDistribute
    {
        [Key]
        public int RecID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Link { get; set; }

        [StringLength(4000)]
        public string Synopsis { get; set; }

        [StringLength(200)]
        public string ImgUrl { get; set; }

        public int? Platform { get; set; }

        public int? BillingMode { get; set; }

        public decimal? BudgetPrice { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BeginTime { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndTime { get; set; }

        public int? UserID { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? Status { get; set; }

        [StringLength(200)]
        public string RejectCause { get; set; }
    }
}
