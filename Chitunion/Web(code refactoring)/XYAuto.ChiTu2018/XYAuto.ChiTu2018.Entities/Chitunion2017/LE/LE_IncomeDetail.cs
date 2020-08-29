using System;
using System.ComponentModel.DataAnnotations;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_IncomeDetail
    {
        [Key]
        public int RecID { get; set; }

        public DateTime? IncomeTime { get; set; }

        public int? UserID { get; set; }

        public int? CategoryID { get; set; }

        [StringLength(100)]
        public string DetailDescription { get; set; }

        public decimal? IncomePrice { get; set; }

        public int? ClickCount { get; set; }
    }
}
