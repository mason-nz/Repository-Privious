/********************************************************
*创建人：lixiong
*创建时间：2017/6/9 9:58:34
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Enum
{
    public enum SaleAreaGroupTypeEnum
    {
        [Description("其他")]
        Other = -1,

        [Description("全国")]
        AllCountry = 0,

        [Description("城市")]
        Citys = 1,
    }
}