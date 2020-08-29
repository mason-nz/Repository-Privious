using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.LETask
{
    public class LeDisbursementPay
    {
        public int RecId { get; set; }
        public int WithdrawalsId { get; set; }
        public string DisbursementNo { get; set; }
        public string BizDisbursementNo { get; set; }
        public string BizNo { get; set; }
        public string ContractNo { get; set; }
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; }
        public int Status { get; set; }

        public decimal WithdrawalsPrice { get; set; }
        public int PayeeID { get; set; }

        public decimal AccumulatedIncome { get; set; }
        public decimal HaveWithdrawals { get; set; }
        public decimal WithdrawalsProcess { get; set; }
        public decimal RemainingAmount { get; set; }
        public int PayStatus { get; set; }

    }
}
