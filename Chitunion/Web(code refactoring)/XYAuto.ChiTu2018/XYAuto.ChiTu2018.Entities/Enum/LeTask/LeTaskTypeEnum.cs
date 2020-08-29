using System.ComponentModel;

namespace XYAuto.ChiTu2018.Entities.Enum.LeTask
{
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
