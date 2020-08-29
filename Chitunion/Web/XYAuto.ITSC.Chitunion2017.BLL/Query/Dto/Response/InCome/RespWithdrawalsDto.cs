using System;

namespace XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Response.InCome
{
    public class RespWithdrawalsDto
    {

        public int Id { get; set; }
        public int RecId { get; set; }
        public string UserName { get; set; }
        public string UserTypeName { get; set; }
        public DateTime ApplicationDate { get; set; }
        public decimal WithdrawalsPrice { get; set; }
        public decimal IndividualTaxPeice { get; set; }
        public decimal PracticalPrice { get; set; }
        public string PayeeAccount { get; set; }
        public DateTime? PayDate { get; set; }
        public string Reason { get; set; }

        public int PayStatus { get; set; }
        public string PayStatusName { get; set; }
        public string TrueName { get; set; }

        public DateTime AuditTime { get; set; }
        public int AuditStatus { get; set; }
        public string AuditStatusName { get; set; }
        public string RejectMsg { get; set; }
        public bool IsLock { get; set; }
    }
}
