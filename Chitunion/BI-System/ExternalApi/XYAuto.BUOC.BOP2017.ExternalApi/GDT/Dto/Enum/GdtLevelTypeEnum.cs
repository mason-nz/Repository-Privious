﻿/********************************************************
*创建人：lixiong
*创建时间：2017/8/15 17:14:27
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System.ComponentModel;

namespace XYAuto.BUOC.BOP2017.ExternalApi.GDT.Dto.Enum
{
    public enum GdtLevelTypeEnum
    {
        [Description("帐号层级")]
        ADVERTISER,

        [Description("推广计划层级")]
        CAMPAIGN,

        [Description("广告组层级")]
        ADGROUP
    }
}