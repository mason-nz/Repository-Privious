using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using XY.Framework.Runtime.Messaging;
using XY.Web.ApiProxy.Common;
using XY.Web.ApiProxy.Common.Security;
using FundsmanagementPublisher.Models;

namespace FundsmanagementPublisher.Models
{
    /// <summary>
    /// 客户账户
    /// </summary>
    public class CustomerAccountDto
    {
        /// <summary>
        /// 客户业务编码
        /// </summary>
        public String BizCustomerNo { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public String CustomerName { get; set; }

        /// <summary>
        /// 结算策略
        /// </summary>
        public SettlementPolicyDto SettlementPolicy { get; set; }

        /// <summary>
        /// 账户类型（银行卡、支付宝、微信）
        /// </summary>
        public AccountType AccountType { get; set; }

        /// <summary>
        /// 账户类型（银行卡、支付宝、微信）
        /// </summary>
        public String AccountTypeText { get; set; }

        /// <summary>
        /// 银行卡号
        /// </summary>
        public String CardNo { get; set; }

        /// <summary>
        /// 账户类型（对公、对私）
        /// </summary>
        public CardType CardType { get; set; }

        /// <summary>
        /// 账户类型（对公、对私）
        /// </summary>
        public String CardTypeText { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>
        public IdType IdType { get; set; }

        /// <summary>
        /// 证件类型名称
        /// </summary>
        public String IdTypeText { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public String IdNo { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public String Mobile { get; set; }

        /// <summary>
        /// 收款行编码
        /// </summary>
        public String BankCode { get; set; }

        /// <summary>
        /// 收款行名称
        /// </summary>
        public String BankName { get; set; }

        /// <summary>
        /// 信用卡CVV2
        /// </summary>
        public String CVV2 { get; set; }

    }

    /// <summary>
    /// 客户账户
    /// </summary>
    public class CustomerAccountInput
    {
        /// <summary>
        /// 收款客户编码
        /// string(50)
        /// required
        /// </summary>
        public String BizCustomerNo { get; set; }

        /// <summary>
        /// 客户名称
        /// string(252)
        /// required
        /// </summary>
        public String CustomerName { get; set; }

        /// <summary>
        /// 账户类型（借记卡、信用卡、支付宝、微信）
        /// required
        /// </summary>
        public AccountType AccountType { get; set; }

        /// <summary>
        /// 银行卡号
        /// AccountType=0||AccountType=1 必传
        /// string(50)
        /// </summary>
        public String CardNo { get; set; }

        /// <summary>
        /// 证件类型
        /// required
        /// </summary>
        public IdType? IdType { get; set; }

        /// <summary>
        /// 证件号码
        /// required
        /// string(50)
        /// </summary>
        public String IdNo { get; set; }

        /// <summary>
        /// 手机号码
        /// required
        /// string(11)
        /// </summary>
        public String Mobile { get; set; }

        /// <summary>
        /// 收款行编码
        /// CardType=1时，必传
        /// string(50)
        /// </summary>
        public String BankCode { get; set; }

        /// <summary>
        /// 收款行名称
        /// CardType=1时，必传
        /// string(252)
        /// </summary>
        public String BankName { get; set; }

        /// <summary>
        /// 账户类型（对公、对私）
        /// </summary>
        public CardType CardType { get; set; }

        /// <summary>
        /// 联行号
        /// optional
        /// string(50)
        /// </summary>
        public String BankKey { get; set; }

        /// <summary>
        /// 信用卡CVV2
        /// AccountType=1时，必传
        /// string(6)
        /// </summary>
        public String CVV2 { get; set; }

    }

    /// <summary>
    /// 付款单
    /// </summary>
    public class DisbursementDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// 付款申请单号
        /// </summary>
        public String DisbursementNo { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        public String AppId { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public String AppName { get; set; }

        /// <summary>
        /// 业务单号
        /// </summary>
        public String BizDisbursementNo { get; set; }

        /// <summary>
        /// 业务收款单号
        /// </summary>
        public String BizNo { get; set; }

        /// <summary>
        /// 业务类型ID
        /// </summary>
        public Int64 TradeClassificationId { get; set; }

        /// <summary>
        /// 业务类型ID
        /// </summary>
        public TradeClassificationDto TradeClassification { get; set; }

        /// <summary>
        /// 合同单号
        /// </summary>
        public String ContractNo { get; set; }

        /// <summary>
        /// 结算账户ID
        /// </summary>
        public Int64 CustomerAccountId { get; set; }

        /// <summary>
        /// 结算账户ID
        /// </summary>
        public CustomerAccountDto CustomerAccount { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime SubmitTime { get; set; }

        /// <summary>
        /// 付款金额
        /// </summary>
        public Decimal Amount { get; set; }

        /// <summary>
        /// 个税金额
        /// </summary>
        public Decimal TaxAmount { get; set; }

        /// <summary>
        /// 实际付款金额
        /// </summary>
        public Decimal ActualAmount { get; set; }

        /// <summary>
        /// 付款状态
        /// </summary>
        public DisbursementStatus Status { get; set; }

        /// <summary>
        /// 付款状态名称
        /// </summary>
        public String StatusText { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Remark { get; set; }

        /// <summary>
        /// 结算单ID
        /// </summary>
        public Int64 SettlementId { get; set; }

        /// <summary>
        /// 结算单号
        /// </summary>
        public String SettlementNo { get; set; }

        /// <summary>
        /// 结算时间
        /// </summary>
        public DateTime SettleTime { get; set; }

    }

    /// <summary>
    /// 付款单
    /// </summary>
    public class DisbursementInput
    {
        /// <summary>
        /// 应用ID
        /// string(50)
        /// required
        /// </summary>
        public String AppId { get; set; }

        /// <summary>
        /// 业务单号
        /// string(50)
        /// required
        /// </summary>
        public String BizDisbursementNo { get; set; }

        /// <summary>
        /// 业务收款单号
        /// string(50)
        /// optional
        /// </summary>
        public String BizNo { get; set; }

        /// <summary>
        /// 业务类型
        /// string(50)
        /// required
        /// </summary>
        public String TradeClassificationCode { get; set; }

        /// <summary>
        /// 合同单号
        /// string(50)
        /// required
        /// </summary>
        public String ContractNo { get; set; }

        /// <summary>
        /// 结算账户
        /// </summary>
        public CustomerAccountInput CustomerAccount { get; set; }

        /// <summary>
        /// 付款金额
        /// decimal(12,2)
        /// required
        /// </summary>
        public Decimal Amount { get; set; }

        /// <summary>
        /// 个税金额
        /// </summary>
        public Decimal TaxAmount { get; set; }

        /// <summary>
        /// 实际付款金额
        /// </summary>
        public Decimal ActualAmount { get; set; }

        /// <summary>
        /// 备注
        /// optional
        /// string(252)
        /// </summary>
        public String Remark { get; set; }

    }

    /// <summary>
    /// 线下收款
    /// </summary>
    public class OfflineReceiptDto
    {
        /// <summary>
        /// 付款凭证流水号
        /// </summary>
        public String EvidenceVoucherNo { get; set; }

        /// <summary>
        /// 付款人
        /// </summary>
        public String Payer { get; set; }

        /// <summary>
        /// 付款账号
        /// </summary>
        public String PayerBankAccount { get; set; }

        /// <summary>
        /// 付款银行
        /// </summary>
        public String PayerBankName { get; set; }

        /// <summary>
        /// 收款人
        /// </summary>
        public String Payee { get; set; }

        /// <summary>
        /// 收款账号
        /// </summary>
        public String PayeeBankAccount { get; set; }

        /// <summary>
        /// 收款银行
        /// </summary>
        public String PayeeBankName { get; set; }

        /// <summary>
        /// 付款凭证流水号
        /// </summary>
        public String EvidenceVoucher { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Remark { get; set; }

        /// <summary>
        /// 凭证金额
        /// </summary>
        public Decimal Amount { get; set; }

    }

    /// <summary>
    /// 线下收款单明细
    /// </summary>
    public class OfflineReceiptInput
    {
        /// <summary>
        /// 付款凭证流水号
        /// string(50)
        /// required
        /// </summary>
        public String EvidenceVoucherNo { get; set; }

        /// <summary>
        /// 付款人
        /// string(50)
        /// required
        /// </summary>
        public String Payer { get; set; }

        /// <summary>
        /// 付款账号
        /// string(50)
        /// required
        /// </summary>
        public String PayerBankAccount { get; set; }

        /// <summary>
        /// 付款银行
        /// string(252)
        /// required
        /// </summary>
        public String PayerBankName { get; set; }

        /// <summary>
        /// 收款人
        /// string(50)
        /// required
        /// </summary>
        public String Payee { get; set; }

        /// <summary>
        /// 收款账号
        /// string(50)
        /// required
        /// </summary>
        public String PayeeBankAccount { get; set; }

        /// <summary>
        /// 收款银行
        /// string(252)
        /// required
        /// </summary>
        public String PayeeBankName { get; set; }

        /// <summary>
        /// 付款凭证流水号
        /// string(50)
        /// required
        /// </summary>
        public String EvidenceVoucher { get; set; }

        /// <summary>
        /// 备注
        /// string(252)
        /// optional
        /// </summary>
        public String Remark { get; set; }

        /// <summary>
        /// 凭证金额
        /// decimal(12,2)
        /// required
        /// </summary>
        public Decimal Amount { get; set; }

    }

    /// <summary>
    /// 收款单审批记录
    /// </summary>
    public class ReceiptAduitRecordDto
    {
        /// <summary>
        /// 序号
        /// </summary>
        public Int32 OrderNo { get; set; }

        /// <summary>
        /// 审批结果
        /// </summary>
        public AuditResult AuditResult { get; set; }

        /// <summary>
        /// 审批结果名称
        /// </summary>
        public String AuditResultText { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Remark { get; set; }

        /// <summary>
        /// 审批人
        /// </summary>
        public String CreateBy { get; set; }

        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime CreateTime { get; set; }

    }

    /// <summary>
    /// 收款单明细
    /// </summary>
    public class ReceiptDetailDto
    {
        /// <summary>
        /// 业务类型
        /// </summary>
        public TradeClassificationDto TradeClassification { get; set; }

        /// <summary>
        /// 合同单号
        /// </summary>
        public String ContractNo { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public Decimal Amount { get; set; }

        /// <summary>
        /// 付款ID
        /// </summary>
        public Int64? DisbursementId { get; set; }

    }

    /// <summary>
    /// 收款单明细
    /// </summary>
    public class ReceiptDetailInput
    {
        /// <summary>
        /// 业务类型
        /// string(50)
        /// required
        /// </summary>
        public String TradeClassificationCode { get; set; }

        /// <summary>
        /// 合同单号
        /// string(50)
        /// required
        /// </summary>
        public String ContractNo { get; set; }

        /// <summary>
        /// 金额
        /// decimal(12,2)
        /// required
        /// </summary>
        public Decimal Amount { get; set; }

    }

    /// <summary>
    /// 收款单
    /// </summary>
    public class ReceiptDto
    {
        /// <summary>
        /// 收款单ID
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// 收款单号
        /// </summary>
        public String ReceiptNo { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        public String AppId { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public String AppName { get; set; }

        /// <summary>
        /// 业务单号
        /// </summary>
        public String BizNo { get; set; }

        /// <summary>
        /// 合同单号
        /// </summary>
        public String ContractNo { get; set; }

        /// <summary>
        /// 支付单号
        /// </summary>
        public String PaymentNo { get; set; }

        /// <summary>
        /// 业务批次号
        /// </summary>
        public String BatchNo { get; set; }

        /// <summary>
        /// 业务说明
        /// </summary>
        public String Descrition { get; set; }

        /// <summary>
        /// 付款时间
        /// </summary>
        public DateTime PaymentTime { get; set; }

        /// <summary>
        /// 付款客户编码
        /// </summary>
        public String BizCustomerNo { get; set; }

        /// <summary>
        /// 付款客户名称
        /// </summary>
        public String CustomerName { get; set; }

        /// <summary>
        /// 收款方式
        /// </summary>
        public PaymentType PaymentType { get; set; }

        /// <summary>
        /// 收款方式
        /// </summary>
        public String PaymentTypeText { get; set; }

        /// <summary>
        /// 收款通道
        /// </summary>
        public PaymentChannel PaymentChannel { get; set; }

        /// <summary>
        /// 收款通道
        /// </summary>
        public String PaymentChannelText { get; set; }

        /// <summary>
        /// 收款金额
        /// </summary>
        public Decimal Amount { get; set; }

        /// <summary>
        /// 折扣金额
        /// </summary>
        public Decimal DiscountAmount { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public Decimal PayedAmount { get; set; }

        /// <summary>
        /// 实收金额
        /// </summary>
        public Decimal ActualAmount { get; set; }

        /// <summary>
        /// 手续费
        /// </summary>
        public Decimal Fee { get; set; }

        /// <summary>
        /// 收款状态
        /// </summary>
        public ReceiptStatus Status { get; set; }

        /// <summary>
        /// 收款状态
        /// </summary>
        public String StatusText { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Remark { get; set; }

        /// <summary>
        /// 审批记录
        /// </summary>
        public ICollection<ReceiptAduitRecordDto> ReceiptAduitRecords { get; set; }

        /// <summary>
        /// 线下收款记录
        /// </summary>
        public ICollection<OfflineReceiptDto> OfflineReceipts { get; set; }

        /// <summary>
        /// 收款明细
        /// </summary>
        public ICollection<ReceiptDetailDto> ReceiptDetails { get; set; }

        /// <summary>
        /// 退款记录
        /// </summary>
        public ICollection<RefundDto> Refunds { get; set; }

    }

    /// <summary>
    /// 收款单
    /// </summary>
    public class ReceiptInput
    {
        /// <summary>
        /// 应用ID
        /// string(50)
        /// required
        /// </summary>
        public String AppId { get; set; }

        /// <summary>
        /// 业务单号
        /// string(50)
        /// required
        /// </summary>
        public String BizNo { get; set; }

        /// <summary>
        /// 支付单号
        /// string(50)
        /// required
        /// </summary>
        public String PaymentNo { get; set; }

        /// <summary>
        /// 业务批次号
        /// string(50)
        /// required
        /// </summary>
        public String BatchNo { get; set; }

        /// <summary>
        /// 业务说明
        /// string(200)
        /// required
        /// </summary>
        public String Descrition { get; set; }

        /// <summary>
        /// 付款时间
        /// 如：2017-12-11 14:24:57
        /// required
        /// </summary>
        public DateTime PaymentTime { get; set; }

        /// <summary>
        /// 付款客户编码
        /// </summary>
        public String BizCustomerNo { get; set; }

        /// <summary>
        /// 付款客户名称
        /// </summary>
        public String CustomerName { get; set; }

        /// <summary>
        /// 收款方式
        /// required
        /// </summary>
        public PaymentType PaymentType { get; set; }

        /// <summary>
        /// 收款通道
        /// required
        /// </summary>
        public PaymentChannel PaymentChannel { get; set; }

        /// <summary>
        /// 收款金额
        /// decimal(12,2)
        /// required
        /// </summary>
        public Decimal Amount { get; set; }

        /// <summary>
        /// 折扣金额
        /// decimal(12,2)
        /// required
        /// DiscountAmount小于Amount
        /// </summary>
        public Decimal DiscountAmount { get; set; }

        /// <summary>
        /// 支付金额
        /// decimal(12,2)
        /// required
        /// PayedAmount=Amount-DiscountAmount
        /// </summary>
        public Decimal PayedAmount { get; set; }

        /// <summary>
        /// 实收金额
        /// decimal(12,2)
        /// required
        /// ActualAmount小于等于PayedAmount
        /// </summary>
        public Decimal ActualAmount { get; set; }

        /// <summary>
        /// 手续费
        /// decimal(12,2)
        /// required
        /// Fee=PayedAmount-ActualAmount
        /// </summary>
        public Decimal Fee { get; set; }

        /// <summary>
        /// 备注
        /// string(200)
        /// optional
        /// </summary>
        public String Remark { get; set; }

        /// <summary>
        /// 线下收款记录
        /// 当PaymentType=1时，必传
        /// </summary>
        public List<OfflineReceiptInput> OfflineReceipts { get; set; }

        /// <summary>
        /// 收款明细
        /// required
        /// </summary>
        public List<ReceiptDetailInput> ReceiptDetails { get; set; }

    }

    /// <summary>
    /// </summary>
    public class RefundAduitRecordDto
    {
        /// <summary>
        /// 序号
        /// </summary>
        public Int32 OrderNo { get; set; }

        /// <summary>
        /// 审批结果
        /// </summary>
        public AuditResult AuditResult { get; set; }

        /// <summary>
        /// 审批结果名称
        /// </summary>
        public String AuditResultText { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Remark { get; set; }

        /// <summary>
        /// 审批人
        /// </summary>
        public String CreateBy { get; set; }

        /// <summary>
        /// 审批时间
        /// </summary>
        public DateTime CreateTime { get; set; }

    }

    /// <summary>
    /// 退款明细
    /// </summary>
    public class RefundDetailDto
    {
        /// <summary>
        /// 业务类型
        /// </summary>
        public TradeClassificationDto TradeClassification { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        public Decimal Amount { get; set; }

    }

    /// <summary>
    /// 退款明细
    /// </summary>
    public class RefundDetailInput
    {
        /// <summary>
        /// 业务类型
        /// string(50)
        /// required
        /// </summary>
        public String TradeClassificationCode { get; set; }

        /// <summary>
        /// 退款金额
        /// decimal(12,2)
        /// required
        /// </summary>
        public Decimal Amount { get; set; }

    }

    /// <summary>
    /// 退款单
    /// </summary>
    public class RefundDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// 退款单号
        /// </summary>
        public String RefundNo { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        public String AppId { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public String AppName { get; set; }

        /// <summary>
        /// 退款业务单号
        /// </summary>
        public String BizRefundNo { get; set; }

        /// <summary>
        /// 收款单号
        /// </summary>
        public String ReceiptNo { get; set; }

        /// <summary>
        /// 退款时间
        /// </summary>
        public DateTime RefundTime { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime SubmitTime { get; set; }

        /// <summary>
        /// 退款类型
        /// </summary>
        public RefundType RefundType { get; set; }

        /// <summary>
        /// 退款类型名称
        /// </summary>
        public String RefundTypeText { get; set; }

        /// <summary>
        /// 退款状态
        /// </summary>
        public RefundStatus RefundStatus { get; set; }

        /// <summary>
        /// 退款状态名称
        /// </summary>
        public String RefundStatusText { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        public Decimal RefundAmount { get; set; }

        /// <summary>
        /// 退款手续费
        /// </summary>
        public Decimal RefundFee { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Remark { get; set; }

        /// <summary>
        /// 退款明细
        /// </summary>
        public ICollection<RefundDetailDto> RefundDetails { get; set; }

        /// <summary>
        /// 审批记录
        /// </summary>
        public ICollection<RefundAduitRecordDto> RefundAduitRecords { get; set; }

    }

    /// <summary>
    /// 退款
    /// </summary>
    public class RefundInput
    {
        /// <summary>
        /// 业务系统ID
        /// string(50)
        /// required
        /// </summary>
        public String AppId { get; set; }

        /// <summary>
        /// 退款业务单号
        /// string(50)
        /// required
        /// </summary>
        public String BizRefundNo { get; set; }

        /// <summary>
        /// 收款单号
        /// string(50)
        /// required
        /// </summary>
        public String ReceiptNo { get; set; }

        /// <summary>
        /// 退款类型
        /// required
        /// </summary>
        public RefundType RefundType { get; set; }

        /// <summary>
        /// 退款金额
        /// decimal(12,2)
        /// required
        /// </summary>
        public Decimal RefundAmount { get; set; }

        /// <summary>
        /// 退款时间
        /// 格式：2017-12-11 14:24:57
        /// required
        /// </summary>
        public DateTime RefundTime { get; set; }

        /// <summary>
        /// 备注
        /// string(252)
        /// optional
        /// </summary>
        public String Remark { get; set; }

        /// <summary>
        /// 退款明细
        /// required
        /// </summary>
        public List<RefundDetailInput> RefundDetails { get; set; }

    }

    /// <summary>
    /// 结算审批记录
    /// </summary>
    public class SettlementAduitRecordDto
    {
        /// <summary>
        /// 序号
        /// </summary>
        public Int32 OrderNo { get; set; }

        /// <summary>
        /// 审批结果
        /// </summary>
        public AuditResult AuditResult { get; set; }

        /// <summary>
        /// 审批结果名称
        /// </summary>
        public String AuditResultText { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Remark { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public String CreateBy { get; set; }

        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime CreateTime { get; set; }

    }

    /// <summary>
    /// 结算单
    /// </summary>
    public class SettlementDto
    {
        /// <summary>
        /// 结算单ID
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// 结算单号
        /// </summary>
        public String SettlementNo { get; set; }

        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 结算时间
        /// </summary>
        public DateTime SettleTime { get; set; }

        /// <summary>
        /// 结算客户编码
        /// </summary>
        public Int64 CustomerAccountId { get; set; }

        /// <summary>
        /// 结算客户编码
        /// </summary>
        public CustomerAccountDto CustomerAccount { get; set; }

        /// <summary>
        /// 结算金额
        /// </summary>
        public Decimal Amount { get; set; }

        /// <summary>
        /// 个税金额
        /// </summary>
        public Decimal TaxAmount { get; set; }

        /// <summary>
        /// 实际付款金额
        /// </summary>
        public Decimal ActualAmount { get; set; }

        /// <summary>
        /// 手续费
        /// </summary>
        public Decimal Fee { get; set; }

        /// <summary>
        /// 结算状态
        /// </summary>
        public SettlementStatus Status { get; set; }

        /// <summary>
        /// 结算状态名称
        /// </summary>
        public String StatusText { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Remark { get; set; }

        /// <summary>
        /// 付款单
        /// </summary>
        public ICollection<DisbursementDto> Disbursements { get; set; }

        /// <summary>
        /// 结算审批记录
        /// </summary>
        public ICollection<SettlementAduitRecordDto> SettlementAduitRecords { get; set; }

    }

    /// <summary>
    /// 结算策略
    /// </summary>
    public class SettlementPolicyDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// 业务系统ID
        /// </summary>
        public Int32 AccessAppId { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        public String AppId { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public String AppName { get; set; }

        /// <summary>
        /// 策略编码
        /// </summary>
        public String PolicyNo { get; set; }

        /// <summary>
        /// 策略类型
        /// </summary>
        public PolicyType PolicyType { get; set; }

        /// <summary>
        /// 策略类型名称
        /// </summary>
        public String PolicyTypeText { get; set; }

        /// <summary>
        /// 策略规则
        /// </summary>
        public PolicyRole PolicyRole { get; set; }

        /// <summary>
        /// 策略规则名称
        /// </summary>
        public String PolicyRoleText { get; set; }

        /// <summary>
        /// 结算最小金额
        /// </summary>
        public Decimal SettleAmount { get; set; }

        /// <summary>
        /// 是否需要业务审核
        /// </summary>
        public Boolean IsBizAudit { get; set; }

        /// <summary>
        /// 是否需要审核
        /// </summary>
        public Boolean IsAudit { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        public PaymentType PaymentType { get; set; }

        /// <summary>
        /// 付款方式名称
        /// </summary>
        public String PaymentTypeText { get; set; }

        /// <summary>
        /// 付款通道
        /// </summary>
        public PaymentChannel PaymentChannel { get; set; }

        /// <summary>
        /// 付款通道名称
        /// </summary>
        public String PaymentChannelText { get; set; }

    }

    /// <summary>
    /// 结算单查询条件
    /// </summary>
    public class SettlementQueryPageInput
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AccessAppId { get; set; }

        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime? BeginCreateTime { get; set; }

        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime? EndCreateTime { get; set; }

        /// <summary>
        /// 结算状态
        /// </summary>
        public SettlementStatus? Status { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        public PaymentType? PaymentType { get; set; }

        /// <summary>
        /// 付款通道
        /// </summary>
        public PaymentChannel? PaymentChannel { get; set; }

        /// <summary>
        /// 结算客户
        /// </summary>
        public Int64 CustomerId { get; set; }

        /// <summary>
        /// </summary>
        public Int32 PageIndex { get; set; }

        /// <summary>
        /// </summary>
        public Int32 PageSize { get; set; }

    }

    /// <summary>
    /// 商品与服务分类
    /// </summary>
    public class TradeClassificationDto
    {
        /// <summary>
        /// 编码
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 是否代收
        /// </summary>
        public Boolean IsDisbursement { get; set; }

        /// <summary>
        /// 付款时是否校验收款
        /// </summary>
        public Boolean IsVerifyReceipt { get; set; }

    }

    /// <summary>
    /// </summary>
    public enum AccountType
    {
        /// <summary>
        /// </summary>
        借记卡 = 0,

        /// <summary>
        /// </summary>
        信用卡 = 1,

        /// <summary>
        /// </summary>
        微信 = 2,

        /// <summary>
        /// </summary>
        支付宝 = 3,

    }

    /// <summary>
    /// </summary>
    public enum AuditResult
    {
        /// <summary>
        /// </summary>
        已通过 = 1,

        /// <summary>
        /// </summary>
        未通过 = 0,

    }

    /// <summary>
    /// </summary>
    public enum CardType
    {
        /// <summary>
        /// </summary>
        对公 = 1,

        /// <summary>
        /// </summary>
        对私 = 2,

    }

    /// <summary>
    /// </summary>
    public enum DisbursementStatus
    {
        /// <summary>
        /// </summary>
        待处理 = 0,

        /// <summary>
        /// </summary>
        已锁定 = 2,

        /// <summary>
        /// </summary>
        已撤销 = 4,

        /// <summary>
        /// </summary>
        处理中 = 6,

        /// <summary>
        /// </summary>
        待审核 = 8,

        /// <summary>
        /// </summary>
        结算中 = 10,

        /// <summary>
        /// </summary>
        结算成功 = 12,

        /// <summary>
        /// </summary>
        结算失败 = 14,

    }

    /// <summary>
    /// </summary>
    public enum IdType
    {
        /// <summary>
        /// </summary>
        身份证号 = 1,

    }

    /// <summary>
    /// </summary>
    public enum PaymentChannel
    {
        /// <summary>
        /// </summary>
        微信 = 1,

        /// <summary>
        /// </summary>
        支付宝 = 2,

        /// <summary>
        /// </summary>
        拉卡拉 = 3,

        /// <summary>
        /// </summary>
        九派 = 4,

    }

    /// <summary>
    /// </summary>
    public enum PaymentType
    {
        /// <summary>
        /// </summary>
        线上 = 0,

        /// <summary>
        /// </summary>
        线下 = 1,

    }

    /// <summary>
    /// </summary>
    public enum PolicyRole
    {
        /// <summary>
        /// </summary>
        按单 = 1,

        /// <summary>
        /// </summary>
        按天 = 2,

        /// <summary>
        /// </summary>
        按自然周 = 3,

        /// <summary>
        /// </summary>
        按自然月 = 4,

        /// <summary>
        /// </summary>
        按金额 = 5,

    }

    /// <summary>
    /// </summary>
    public enum PolicyType
    {
        /// <summary>
        /// </summary>
        系统定义 = 0,

        /// <summary>
        /// </summary>
        业务自定义 = 1,

    }

    /// <summary>
    /// </summary>
    public enum ReceiptStatus
    {
        /// <summary>
        /// </summary>
        待提交 = 0,

        /// <summary>
        /// </summary>
        待审核 = 2,

        /// <summary>
        /// </summary>
        收款成功 = 4,

        /// <summary>
        /// </summary>
        收款失败 = 8,

    }

    /// <summary>
    /// </summary>
    public enum RefundStatus
    {
        /// <summary>
        /// </summary>
        待提交 = 0,

        /// <summary>
        /// </summary>
        待审核 = 2,

        /// <summary>
        /// </summary>
        退款中 = 4,

        /// <summary>
        /// </summary>
        退款成功 = 6,

        /// <summary>
        /// </summary>
        退款失败 = 8,

    }

    /// <summary>
    /// </summary>
    public enum RefundType
    {
        /// <summary>
        /// </summary>
        全额退款 = 1,

        /// <summary>
        /// </summary>
        部分退款 = 2,

    }

    /// <summary>
    /// </summary>
    public enum SettlementStatus
    {
        /// <summary>
        /// </summary>
        预结算 = -2,

        /// <summary>
        /// </summary>
        待提交 = 0,

        /// <summary>
        /// </summary>
        待审核 = 2,

        /// <summary>
        /// </summary>
        待结算 = 4,

        /// <summary>
        /// </summary>
        结算中 = 6,

        /// <summary>
        /// </summary>
        结算成功 = 8,

        /// <summary>
        /// </summary>
        结算失败 = 10,

    }

}

namespace FundsmanagementPublisher.Client
{
    public class BaseClient
    {
        protected static readonly ISignature Signature = new SignatureV2();
        protected static readonly HttpClient HttpClient = new HttpClient();
        private static string baseUrl = "";

        public BaseClient()
        {
            baseUrl = AppClient.Current.GetProjectBaseUrl("FundsmanagementPublisher");
        }

        public BaseClient(string baseUrl)
        {
            BaseClient.baseUrl = baseUrl;
        }

        public static string GetUrl(string method, Dictionary<string, string> urlArgs, KeyValuePair<string, string> bodyArg)
        {
            Dictionary<string, string> args = new Dictionary<string, string>(urlArgs);
            var appinfo = AppClient.Current.GetAppInfo("FundsmanagementPublisher");
            args.Add("appkey", appinfo.AppKey);
            args.Add("method", method);
            args.Add("sv", "v2");
            if (!string.IsNullOrWhiteSpace(bodyArg.Key))
            {
                args.Add(bodyArg.Key, bodyArg.Value);
            }
            string url = "";
            string sign = Signature.Create(args, appinfo.Token);
            if (baseUrl.ToLower().EndsWith("api"))
            {
                url = baseUrl + "/gateway?method=" + method + "&appkey=" + appinfo.AppKey;
            }
            else if (baseUrl.ToLower().EndsWith("api/"))
            {
                url = baseUrl + "gateway?method=" + method + "&appkey=" + appinfo.AppKey;
            }
            else if (baseUrl.ToLower().EndsWith("/"))
            {
                url = baseUrl + "api/gateway?method=" + method + "&appkey=" + appinfo.AppKey;
            }
            else
            {
                url = baseUrl + "/api/gateway?method=" + method + "&appkey=" + appinfo.AppKey;
            }
            url += "&sv=v2&sign=" + sign;

            if (urlArgs.Count > 0)
            {
                foreach (var arg in urlArgs)
                {
                    url += $"&{arg.Key}={arg.Value}";
                }
            }
            return url;
        }

        public static T GetResult<T>(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Exception($"找不到请求路径{response.RequestMessage.RequestUri}");
            }
            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                var error = JsonConvert.DeserializeObject<ApiMessage<string>>(response.Content.ReadAsStringAsync().Result);
                throw new Exception(error.ErrorMessage);
            }
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                string requestNo = Guid.NewGuid().ToString("N");
                Trace.TraceError($"日志编号：{requestNo}，接口错误内容：{result}");
                throw new Exception($"接口服务异常，可根据日志编号{requestNo}查找详细错误");
            }
            throw new Exception("请求异常");
        }
    }

    public class DisbursementClient : BaseClient
    {
        public DisbursementClient() { }
        public DisbursementClient(string url) : base(url) { }
        /// <summary>
        /// 创建付款申请单
        /// </summary>
        /// <param name="disbursement">
        /// 付款申请单
        /// </param>
        public ApiMessage<DisbursementDto> Create(DisbursementInput disbursement, Action<string> logAction)
        {
            var urlArgs = new Dictionary<string, string>();
            string url = GetUrl("Disbursement.Create", urlArgs, new KeyValuePair<string, string>("disbursement", JsonConvert.SerializeObject(disbursement)));
            HttpResponseMessage result = null;
            var startTime = DateTime.Now;
            result = HttpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(disbursement), Encoding.UTF8, "application/json")).Result;
            var requetLog = string.Format("{1}请求的url地址：{0}{1}执行的时间:{2}ms{1}请求的参数:{3}{1}返回的结果:{4}{1}",
                    url, System.Environment.NewLine, (DateTime.Now - startTime).TotalMilliseconds,
                JsonConvert.SerializeObject(disbursement), JsonConvert.SerializeObject(result.Content.ReadAsStringAsync().Result));
            logAction(requetLog);
            return GetResult<ApiMessage<DisbursementDto>>(result);
        }

        /// <summary>
        /// 根据单号查询付款申请单
        /// </summary>
        /// <param name="disbursementNo">
        /// 付款申请号
        /// </param>
        public ApiMessage<DisbursementDto> Query(String disbursementNo)
        {
            var urlArgs = new Dictionary<string, string>();
            urlArgs.Add("disbursementNo", disbursementNo.ToString());
            string url = GetUrl("Disbursement.Query", urlArgs, new KeyValuePair<string, string>());
            HttpResponseMessage result = null;
            result = HttpClient.GetAsync(url).Result;
            return GetResult<ApiMessage<DisbursementDto>>(result);
        }

    }

    public class ReceiptClient : BaseClient
    {
        public ReceiptClient() { }
        public ReceiptClient(string url) : base(url) { }
        /// <summary>
        /// 创建收款单
        /// </summary>
        /// <param name="receipt">
        /// 收款申请单
        /// </param>
        public ApiMessage<ReceiptDto> Create(ReceiptInput receipt)
        {
            var urlArgs = new Dictionary<string, string>();
            string url = GetUrl("Receipt.Create", urlArgs, new KeyValuePair<string, string>("receipt", JsonConvert.SerializeObject(receipt)));
            HttpResponseMessage result = null;
            result = HttpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(receipt), Encoding.UTF8, "application/json")).Result;
            return GetResult<ApiMessage<ReceiptDto>>(result);
        }

        /// <summary>
        /// 根据单号查询收款单
        /// </summary>
        /// <param name="receiptNo">
        /// 收款申请单号 BatchNo或者ReceiptNo
        /// </param>
        public ApiMessage<ReceiptDto> Query(String receiptNo)
        {
            var urlArgs = new Dictionary<string, string>();
            urlArgs.Add("receiptNo", receiptNo.ToString());
            string url = GetUrl("Receipt.Query", urlArgs, new KeyValuePair<string, string>());
            HttpResponseMessage result = null;
            result = HttpClient.GetAsync(url).Result;
            return GetResult<ApiMessage<ReceiptDto>>(result);
        }

    }

    public class RefundClient : BaseClient
    {
        public RefundClient() { }
        public RefundClient(string url) : base(url) { }
        /// <summary>
        /// 创建退款单
        /// </summary>
        /// <param name="refund">
        /// 退款申请单
        /// </param>
        public ApiMessage<RefundDto> Create(RefundInput refund)
        {
            var urlArgs = new Dictionary<string, string>();
            string url = GetUrl("Refund.Create", urlArgs, new KeyValuePair<string, string>("refund", JsonConvert.SerializeObject(refund)));
            HttpResponseMessage result = null;
            result = HttpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(refund), Encoding.UTF8, "application/json")).Result;
            return GetResult<ApiMessage<RefundDto>>(result);
        }

        /// <summary>
        /// 根据单号查询退款单
        /// </summary>
        /// <param name="refundNo">
        /// 退款申请单号 BizRefundNo 或者 RefundNo
        /// </param>
        public ApiMessage<RefundDto> Query(String refundNo)
        {
            var urlArgs = new Dictionary<string, string>();
            urlArgs.Add("refundNo", refundNo.ToString());
            string url = GetUrl("Refund.Query", urlArgs, new KeyValuePair<string, string>());
            HttpResponseMessage result = null;
            result = HttpClient.GetAsync(url).Result;
            return GetResult<ApiMessage<RefundDto>>(result);
        }

    }

    public class SettlementClient : BaseClient
    {
        public SettlementClient() { }
        public SettlementClient(string url) : base(url) { }
        /// <summary>
        /// 获取结算分页数据
        /// </summary>
        public ApiMessage<Tuple<Int32, List<SettlementDto>>> GetPage(SettlementQueryPageInput query, Action<string> logAction)
        {
            var urlArgs = new Dictionary<string, string>();
            string url = GetUrl("Settlement.GetPage", urlArgs, new KeyValuePair<string, string>("query", JsonConvert.SerializeObject(query)));
            HttpResponseMessage result = null;
            var startTime = DateTime.Now;
            result = HttpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, "application/json")).Result;
            var requetLog = string.Format("{1}请求的url地址：{0}{1}执行的时间:{2}ms{1}请求的参数:{3}{1}返回的结果:{4}{1}",
                   url, System.Environment.NewLine, (DateTime.Now - startTime).TotalMilliseconds,
               JsonConvert.SerializeObject(query), JsonConvert.SerializeObject(result.Content.ReadAsStringAsync().Result));
            logAction(requetLog);
            return GetResult<ApiMessage<Tuple<Int32, List<SettlementDto>>>>(result);
        }

    }

}