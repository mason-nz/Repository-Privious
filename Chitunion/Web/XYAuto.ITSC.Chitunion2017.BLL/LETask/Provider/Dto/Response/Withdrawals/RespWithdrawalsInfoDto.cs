using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Response.Withdrawals
{


    public class RespWithdrawalsDetailsInfoDto
    {
        public RespWithdrawalsInfoDto WithdrawalsInfo { get; set; }
        public RespAuditInfoDto AuditInfo { get; set; }
    }

    public class RespAuditInfoDto
    {
        public string AuditUser { get; set; }
        public DateTime AuditTime { get; set; }
        public string RejectMsg { get; set; }
    }

    public class RespWithdrawalsInfoDto
    {
        public int RecId { get; set; }
        public string TrueName { get; set; }
        public string UserName { get; set; }
        public string UserTypeName { get; set; }
        public DateTime ApplicationDate { get; set; }
        public decimal WithdrawalsPrice { get; set; }
        public decimal IndividualTaxPeice { get; set; }
        public decimal PracticalPrice { get; set; }
        public string PayeeAccount { get; set; }
        public int PayStatus { get; set; }
        public string PayStatusName { get; set; }
        public DateTime PayDate { get; set; }
        public string Reason { get; set; }
        public string OrderNum { get; set; }
        public string RejectMsg { get; set; }
        public bool IsLock { get; set; }

    }
}
