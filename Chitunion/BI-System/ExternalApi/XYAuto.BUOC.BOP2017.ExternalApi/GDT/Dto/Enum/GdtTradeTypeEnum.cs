/********************************************************
*创建人：lixiong
*创建时间：2017/8/14 15:11:43
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System.ComponentModel;

namespace XYAuto.BUOC.BOP2017.ExternalApi.GDT.Dto.Enum
{
    /// <summary>
    /// 交易类型 | trade_type
    /// </summary>
    public enum GdtTradeTypeEnum
    {
        None,

        [Description("充值")]
        CHARGE,

        [Description("消费")]
        PAY,

        [Description("回划")]
        TRANSFER_BACK,

        [Description("过期")]
        EXPIRE,
    }
}