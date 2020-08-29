using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.LETask
{
    public class LeWithdrawalsStatistics
    {
        public int RecID { get; set; }
        public decimal AccumulatedIncome { get; set; }
        public decimal HaveWithdrawals { get; set; }
        public decimal WithdrawalsProcess { get; set; }
        public int UserID { get; set; }
        public DateTime CreateTime { get; set; }
        public int Status { get; set; }
        public decimal RemainingAmount { get; set; }
    }
}
