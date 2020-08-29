using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_SmartSearch
    {
        [Key]
        public int RecID { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Demand { get; set; }

        public int? Purposes { get; set; }

        public decimal? BudgetPrice { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BeginTime { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndTime { get; set; }

        [StringLength(200)]
        public string MaterialUrl { get; set; }

        public int? UserID { get; set; }

        public int? Status { get; set; }

        public DateTime? CreateTime { get; set; }

        [StringLength(200)]
        public string RejectCause { get; set; }

        [StringLength(100)]
        public string MaterialUrlName { get; set; }
    }
}
