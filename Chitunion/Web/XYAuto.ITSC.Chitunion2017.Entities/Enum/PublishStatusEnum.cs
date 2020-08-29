using System;
using System.Collections.Generic;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Enum
{
    /// <summary>
    /// ls
    /// </summary>
    public enum PublishStatusEnum
    {
        初始状态 = Constants.Constant.INT_INVALID_VALUE,
        新建 = 15001,
        申请上架 = 15002,
        已上架 = 15005,
        已下架
    }
}
