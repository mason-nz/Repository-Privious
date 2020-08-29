/********************************************************
*创建人：lixiong
*创建时间：2017/8/25 15:29:19
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System.ComponentModel;

namespace XYAuto.BUOC.BOP2017.Entities.Enum.GDT
{
    public enum DemandStatusNotesTypeEnum
    {
        [Description("根据需求过期处理")]
        DemandOverdue,

        [Description("根据广告组客户设置状态处理")]
        AdGroupStatus
    }
}