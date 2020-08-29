using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Enum
{
    /// <summary>
    /// APP刊例状态枚举
    /// </summary>
    public enum AppPublishStatus
    {

        默认 = Constants.Constant.INT_INVALID_VALUE,

        待提交 = 49001,
        待审核 = 49002,
        已驳回 = 49003,
        已上架 = 49004,
        已下架 = 49005,
        未上架
    }
}
