using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask
{
    /// <summary>
    /// 注册方式：帐号密码，微信
    /// </summary>
    public enum RegisterTypeEnum
    {
        未知 = Entities.Constants.Constant.INT_INVALID_VALUE,
        帐号密码 = 199001,
        微信 = 199002,
        手机号 = 199003
    }
}
