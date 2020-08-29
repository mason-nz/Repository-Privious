using System.ComponentModel;

namespace XYAuto.ChiTu2018.Entities.Enum.LeTask
{
    /// <summary>
    /// 提现状态
    /// </summary>
    public enum WithdrawalsStatusEnum
    {
        [Description("无")]
        无 = -2,
        [Description("支付中/待审核")]
        支付中 = 195001,

        [Description("支付失败")]
        支付失败 = 195002,

        [Description("已支付")]
        已支付 = 195003,
    }
}
