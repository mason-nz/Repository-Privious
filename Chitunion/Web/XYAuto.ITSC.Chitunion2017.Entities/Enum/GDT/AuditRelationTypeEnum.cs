/********************************************************
*创建人：lixiong
*创建时间：2017/8/22 18:01:26
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.Enum.GDT
{
    public enum AuditRelationTypeEnum
    {
        None = Entities.Constants.Constant.INT_INVALID_VALUE,

        [Description("广点通")]
        Gdt = 94001
    }
}