using System.ComponentModel;

namespace XYAuto.ChiTu2018.Service.App.Task
{
    /// <summary>
    /// 注释：Enum
    /// 作者：lihf
    /// 日期：2018/5/11 11:57:43
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public enum LeShareDetailTypeEnum
    {
        物料 = 202001,
        邀请,
        其他,
        首次欢迎分享,
        签到分享,
        提现分享
    }

    public enum LeTaskTypeEnum
    {
        [Description("无")]
        None = -2,

        [Description("内容分发")]
        ContentDistribute = 192001,

        [Description("贴片广告")]
        CoverImage = 192002,
    }
}
