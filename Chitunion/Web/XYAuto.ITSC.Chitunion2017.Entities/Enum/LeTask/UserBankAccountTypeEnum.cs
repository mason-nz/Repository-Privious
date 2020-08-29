using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask
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
