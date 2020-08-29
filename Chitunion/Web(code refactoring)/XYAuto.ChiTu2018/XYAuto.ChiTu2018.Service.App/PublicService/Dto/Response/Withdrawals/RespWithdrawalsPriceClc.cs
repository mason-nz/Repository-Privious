namespace XYAuto.ChiTu2018.Service.App.PublicService.Dto.Response.Withdrawals
{
    public class RespWithdrawalsPriceClc
    {
        /// <summary>
        /// 提现金额
        /// </summary>
        public decimal WithdrawalsPrice { get; set; }
        /// <summary>
        /// 个税金额
        /// </summary>
        public decimal IndividualTaxPeice { get; set; }

        /// <summary>
        /// 实际付款
        /// </summary>
        public decimal PracticalPrice { get; set; }
    }
}
