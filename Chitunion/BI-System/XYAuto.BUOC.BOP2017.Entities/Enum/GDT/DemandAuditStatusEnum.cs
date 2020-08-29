/********************************************************
*创建人：lixiong
*创建时间：2017/8/16 14:21:02
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System.ComponentModel;

namespace XYAuto.BUOC.BOP2017.Entities.Enum.GDT
{
    public enum DemandAuditStatusEnum
    {
        None = Entities.Constants.Constant.INT_INVALID_VALUE,

        [Description("待审核")]
        PendingAudit = 89001,

        [Description("已驳回")]
        Rejected = 89002,

        [Description("待投放")]
        PendingPutIn = 89003,

        [Description("投放中")]
        Puting = 89004,

        [Description("已终止")]
        Terminated = 89005,

        [Description("已结束")]
        IsOver = 89006,

        [Description("已撤销")]
        Revoke = 89007,
    }
}