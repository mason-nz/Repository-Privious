/********************************************************
*创建人：lixiong
*创建时间：2017/8/14 15:00:40
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System.ComponentModel;

namespace XYAuto.BUOC.BOP2017.ExternalApi.GDT.Dto.Enum
{
    /// <summary>
    /// 资金账户类型 | fund_type
    /// </summary>
    public enum GdtFundTypeEnum
    {
        [Description("现金账户")]
        GENERAL_CASH,

        [Description("虚拟金账户")]
        GENERAL_GIFT,

        [Description("分成账户")]
        GENERAL_SHARED,

        [Description("银证账户")]
        BANK,

        [Description("应用宝记账")]
        MYAPP_CHARGE,

        [Description("应用宝划账")]
        MYAPP_CONSUME
    }
}