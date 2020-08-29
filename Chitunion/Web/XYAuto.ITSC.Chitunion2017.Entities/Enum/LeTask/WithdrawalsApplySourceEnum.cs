using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask
{
    /// <summary>
    /// 提现申请来源渠道
    /// </summary>
    public enum WithdrawalsApplySourceEnum
    {
        None = Entities.Constants.Constant.INT_INVALID_VALUE,
        Pc = 201001,
        WeiXin,
        H5,
        Android,
        Ios = 201005,
        [Description("活动_一元提现，注意：值 是不连续的")]
        ActivityOneYuanTiXian = 103009
    }
}
