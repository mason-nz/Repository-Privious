using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Entities.Enum.User
{
    /// <summary>
    /// 注释：ReqMessageType
    /// 作者：lix
    /// 日期：2018/6/8 16:39:00
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public enum ReqMessageTypeEnum
    {
        无 = Entities.Constants.Constant.INT_INVALID_VALUE,
        邀请 = 1,
        Pc端登录 = 2,

        场景 = 3,
        Android,
        Ios
    }
}
