using System;
using System.ComponentModel.DataAnnotations;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_IncomeStatisticsCategory
    {
        [Key]
        public int RecID { get; set; }

        public int? IncomeCategoryID { get; set; }

        public decimal? IncomePrice { get; set; }

        public int? UserID { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
