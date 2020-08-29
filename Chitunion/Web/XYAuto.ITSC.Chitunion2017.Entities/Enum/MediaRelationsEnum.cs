/********************************************************
*创建人：lixiong
*创建时间：2017/6/7 16:48:52
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
    public enum MediaRelationsEnum
    {
        [Description("代理")]
        Proxy = 50001,

        [Description("自有")]
        Own = 50002,

        [Description("自营")]
        SelfSupport = 50003
    }
}