/********************************************************
*创建人：lixiong
*创建时间：2017/5/17 9:59:27
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
    public enum UserDetailInfoStatusEnum
    {
        [Description("数据正常")]
        Normal = 0,

        [Description("审核通过")]
        AuditPass = 2
    }
}