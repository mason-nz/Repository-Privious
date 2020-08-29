namespace XYAuto.ChiTu2018.Service.ThirdApi.Dto.Request
{
    /// <summary>
    /// 注释：ReqPostWithdrawlsDto
    /// 作者：lix
    /// 日期：2018/5/23 17:52:07
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class ReqPostWithdrawlsDto
    {
        /// <summary>
        /// 提现金额：必填
        /// </summary>
        public decimal WithdrawalsPrice { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 申请来源
        /// </summary>
        public int ApplySource { get; set; }

        /// <summary>
        /// 客户端ip
        /// </summary>
        public string Ip { get; set; }
    }
}
