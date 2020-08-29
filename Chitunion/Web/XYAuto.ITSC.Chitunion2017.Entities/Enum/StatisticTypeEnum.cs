using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Enum
{
    //统计类型 0:粉丝性别比 1:区域分布
    public enum StatisticTypeEnum
    {
        [Description("默认初始化")]
        Init = Entities.Constants.Constant.INT_INVALID_VALUE,

        [Description("粉丝性别比")]
        FansSexProportion = 0,

        [Description("粉丝分布区域")]
        FansAreaMapper = 1
    }
}