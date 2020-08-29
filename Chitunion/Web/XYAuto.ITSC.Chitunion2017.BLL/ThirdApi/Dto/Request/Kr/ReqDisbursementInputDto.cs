using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Dto.Enum;

namespace XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Dto.Request.Kr
{
    /// <summary>
    /// 创建转账申请单
    /// </summary>
    public class ReqDisbursementInputDto
    {
        [Necessary(MtName = "AppId")]
        public string AppId { get; set; }
        [Necessary(MtName = "BizDisbursementNo业务单号")]
        public string BizDisbursementNo { get; set; }
        [Necessary(MtName = "BizNo业务收款单号")]
        public string BizNo { get; set; }
        public string TradeClassificationCode { get; set; }
        [Necessary(MtName = "ContractNo合同单号")]
        public string ContractNo { get; set; }
        public CustomerAccountInput CustomerAccount { get; set; }


        [Necessary(MtName = "付款金额", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入付款金额")]
        public int Amount { get; set; }
        public int TaxAmount { get; set; }
        public int ActualAmount { get; set; }
        public string Remark { get; set; }

    }

    public class CustomerAccountInput
    {
        public string BizCustomerNo { get; set; }
        public KrAccountTypeEnum AccountType { get; set; }
        public string CardNo { get; set; }
        public KrIdTypeEnum IdType { get; set; }
        public string IdNo { get; set; }
        public string Mobile { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public KrCardTypeEnum CardType { get; set; }
        public string BankKey { get; set; }
        public string CVV2 { get; set; }
    }
}
