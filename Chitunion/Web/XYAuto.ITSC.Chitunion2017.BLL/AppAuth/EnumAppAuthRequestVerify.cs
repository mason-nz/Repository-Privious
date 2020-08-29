using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.AppAuth
{
    public enum EnumAppAuthRequestVerify
    {
        参数错误 = 1,
        参数不能为空 = 2,
        版本错误 = 3,
        签名错误 = 4,
        请求过期 = 5,
        AppID无效 = 6
    }
}
