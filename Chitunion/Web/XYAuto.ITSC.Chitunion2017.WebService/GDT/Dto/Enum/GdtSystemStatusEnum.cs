/********************************************************
*创建人：lixiong
*创建时间：2017/8/25 9:57:52
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
    public enum GdtSystemStatusEnum
    {
        None,

        [Description("有效")]
        AD_STATUS_NORMAL,

        [Description("待审核")]
        AD_STATUS_PENDING,

        [Description("审核不通过")]
        AD_STATUS_DENIED,

        [Description("封停")]
        AD_STATUS_FROZEN,

        [Description("准备资源")]
        AD_STATUS_PREPARE,
    }
}