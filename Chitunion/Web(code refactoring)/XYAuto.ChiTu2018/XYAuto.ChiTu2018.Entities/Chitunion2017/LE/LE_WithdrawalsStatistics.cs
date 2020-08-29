using System;
using System.ComponentModel.DataAnnotations;

namespace XYAuto.ChiTu2018.Entities.Chitunion2017.LE
{
    public partial class LE_WithdrawalsStatistics
    {
        [Key]
        public int RecID { get; set; }

        public decimal? AccumulatedIncome { get; set; }

        public decimal? HaveWithdrawals { get; set; }

        public decimal? WithdrawalsProcess { get; set; }

        public int? UserID { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? Status { get; set; }

        public decimal? RemainingAmount { get; set; }
    }
}
