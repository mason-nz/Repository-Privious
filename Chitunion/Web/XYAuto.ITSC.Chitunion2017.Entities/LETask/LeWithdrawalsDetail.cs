using System;

namespace XYAuto.ITSC.Chitunion2017.Entities.LETask
{
    //提现明细
    public class LeWithdrawalsDetail
    {

        //主键
        public int RecID { get; set; }

        //提现总金额金额
        public decimal WithdrawalsPrice { get; set; }

        //个人缴税金额
        public decimal IndividualTaxPeice { get; set; }

        //实际付款金额
        public decimal PracticalPrice { get; set; }

        //收款人账号
        public string PayeeAccount { get; set; }

        public int Status { get; set; }

        public DateTime ApplicationDate { get; set; }

        public DateTime PayDate { get; set; }

        //关联订单表主键
        public int OrderID { get; set; }

        //登录用户ID
        public int PayeeID { get; set; }

        //原因备注
        public string Reason { get; set; }

        public DateTime CreateTime { get; set; } = DateTime.Now;

        public string UserName { get; set; }
        public string TrueName { get; set; }
        public string UserTypeName { get; set; }
        public string PayStatusName { get; set; }

        public string SyncResult { get; set; }
        public string AsynResult { get; set; }

        public int AuditStatus { get; set; }
        public int ApplySource { get; set; }

        public decimal AccumulatedIncome { get; set; }//累计收益
        public decimal HaveWithdrawals { get; set; }//已提现

        public int SyncPayStatus { get; set; }

        public bool IsLock { get; set; }

    }
}