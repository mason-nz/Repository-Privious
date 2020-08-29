using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_AccountBalance
    {
        [Key]
        public int RecID { get; set; }

        public int? CPCCount { get; set; }

        public int? CPLCount { get; set; }

        public int? PVCount { get; set; }

        public int? OrderID { get; set; }

        public int? Status { get; set; }

        public DateTime? CreateTime { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StatisticsTime { get; set; }

        public int? CreateUserID { get; set; }

        public decimal? CPCTotalPrice { get; set; }

        public decimal? CPLTotalPrice { get; set; }

        public decimal? TotalMoney { get; set; }

        public int? CPCShowCount { get; set; }

        public int? CPLShowCount { get; set; }
    }
}
