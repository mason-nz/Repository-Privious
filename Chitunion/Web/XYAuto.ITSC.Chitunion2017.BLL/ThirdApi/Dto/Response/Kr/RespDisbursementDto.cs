using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Dto.Enum;

namespace XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Dto.Response.Kr
{
    /// <summary>
    /// 创建转账申请单返回值
    /// </summary>
    public class RespDisbursementDto
    {
        public long Id { get; set; }

        /// <summary>
        /// 付款申请单号
        /// </summary>
        public string DisbursementNo { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }
        /// <summary>
        /// 业务单号
        /// </summary>
        public string BizDisbursementNo { get; set; }
        /// <summary>
        /// 业务收款单号
        /// </summary>
        public string BizNo { get; set; }
        public long TradeClassificationId { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public TradeClassificationDto TradeClassification { get; set; }

        /// <summary>
        /// 合同单号
        /// </summary>
        public string ContractNo { get; set; }

        /// <summary>
        /// CustomerAccountId
        /// </summary>
        public long CustomerAccountId { get; set; }
        /// <summary>
        /// 合同单号
        /// </summary>
        public CustomerAccountDto CustomerAccount { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime SubmitTime { get; set; }

        /// <summary>
        /// 付款金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// TaxAmount
        /// </summary>
        public decimal TaxAmount { get; set; }
        /// <summary>
        /// ActualAmount
        /// </summary>
        public decimal ActualAmount { get; set; }

        /// <summary>
        /// 付款状态
        /// </summary>
        public KrDisbursementStatusEnum Status { get; set; }
        /// <summary>
        /// 付款状态名称
        /// </summary>
        public string StatusText { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 结算单ID
        /// </summary>
        public string SettlementId { get; set; }
        /// <summary>
        /// SettlementNo
        /// </summary>
        public string SettlementNo { get; set; }
        /// <summary>
        /// SettleTime
        /// </summary>
        public DateTime SettleTime { get; set; }
    }


    public class TradeClassificationDto
    {

    }

    public class CustomerAccountDto
    {
        public string BizCustomerNo { get; set; }
        public string CustomerName { get; set; }
        public KrAccountTypeEnum AccountType { get; set; }
        public string AccountTypeText { get; set; }
        public string CardNo { get; set; }
        public KrIdTypeEnum IdType { get; set; }
        public string CardTypeText { get; set; }
        public string IdNo { get; set; }
        public string Mobile { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public KrCardTypeEnum CardType { get; set; }
        public string IdTypeText { get; set; }
        public string CVV2 { get; set; }
    }
}
