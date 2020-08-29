using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Enum
{
    public enum PublishBasicStatusEnum
    {
        //V1_1
        默认 = Constants.Constant.INT_INVALID_VALUE,

        待审核 = 42001,
        已通过,
        已驳回,
        启用,
        停用,
        已过期,
        刊例过期1天,
        刊例过期3天,
        刊例过期7天,
        刊例过期30天,
        上架,
        下架,
    }
}