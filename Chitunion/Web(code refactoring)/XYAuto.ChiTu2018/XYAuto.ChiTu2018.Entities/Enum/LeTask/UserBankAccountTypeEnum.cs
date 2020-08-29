using System.ComponentModel;

namespace XYAuto.ChiTu2018.Entities.Enum.LeTask
{
    /// <summary>
    /// 提现帐号类型
    /// </summary>
    public enum UserBankAccountTypeEnum
    {
        None = Entities.Constants.Constant.INT_INVALID_VALUE,

        [Description("支付宝")]
        Zfb = 96001

    }
}
