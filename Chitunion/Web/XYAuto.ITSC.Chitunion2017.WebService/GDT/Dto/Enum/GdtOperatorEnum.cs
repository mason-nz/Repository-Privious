/********************************************************
*创建人：lixiong
*创建时间：2017/8/24 13:15:53
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Enum
{
    public enum GdtOperatorEnum
    {
        [Description("等于，values")]
        EQUALS,

        [Description("包含")]
        CONTAINS,

        [Description("	小于等于")]
        LESS_EQUALS,

        [Description("小于")]
        LESS,

        [Description("大于等于")]
        GREATER_EQUALS,

        [Description("大于")]
        GREATER,
    }
}