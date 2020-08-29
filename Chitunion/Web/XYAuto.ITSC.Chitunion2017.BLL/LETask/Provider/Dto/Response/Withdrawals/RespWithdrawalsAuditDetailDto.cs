using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Response.Withdrawals
{
    public class RespWithdrawalsAuditDetailDto
    {
        public int RecId { get; set; }
        public string TrueName { get; set; }
        public string UserName { get; set; }
        public string UserTypeName { get; set; }
        public DateTime ApplicationDate { get; set; }

        public string PayeeAccount { get; set; }

        public decimal AccumulatedIncome { get; set; }//累计收益
        public decimal HaveWithdrawals { get; set; }//已提现

        public decimal WithdrawalsPrice { get; set; }//申请提现金额
        public decimal IndividualTaxPeice { get; set; }//个税金额
        public decimal PracticalPrice { get; set; }//实际付款

        public int PayeeID { get; set; }//申请人


        public int AuditStatus { get; set; }

        public bool IsInsideEmployee { get; set; } = false;//是否为内部员工，true为内部员工，false不是内部员工
        public bool IsLock { get; set; }
    }
}
