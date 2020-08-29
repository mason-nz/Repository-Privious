/********************************************************
*创建人：lixiong
*创建时间：2017/8/17 18:27:00
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
    public enum FundTypeEnum
    {
        [Description("GENERAL_CASH")]
        现金账户 = 81001,

        [Description("GENERAL_GIFT")]
        虚拟金账户 = 81002,

        [Description("GENERAL_SHARED")]
        分成账户 = 81003,

        [Description("BANK")]
        银证账户 = 81004,

        [Description("MYAPP_CHARGE")]
        应用宝记账 = 81005,

        [Description("MYAPP_CONSUME")]
        应用宝划账 = 81006
    }
}