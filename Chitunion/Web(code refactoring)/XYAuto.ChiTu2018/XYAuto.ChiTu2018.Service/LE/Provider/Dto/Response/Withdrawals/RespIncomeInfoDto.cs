namespace XYAuto.ChiTu2018.Service.LE.Provider.Dto.Response.Withdrawals
{
    /// <summary>
    /// 注释：RespIncomeInfoDto
    /// 作者：lix
    /// 日期：2018/5/14 15:34:36
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class RespIncomeInfoDto
    {
        /// <summary>
        /// 可提现金额
        /// </summary>
        public decimal CanWithdrawalsMoney { get; set; }
        /// <summary>
        /// 累计收益
        /// </summary>
        public decimal EarningsPrice { get; set; }
        /// <summary>
        /// 已提现
        /// </summary>
        public decimal AlreadyWithdrawalsMoney { get; set; }
        /// <summary>
        /// 提现中
        /// </summary>
        public decimal WithdrawalsMoneyIng { get; set; }
    }
}
