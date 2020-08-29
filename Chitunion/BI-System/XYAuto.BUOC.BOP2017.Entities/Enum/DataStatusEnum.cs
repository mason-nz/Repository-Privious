using System.ComponentModel;

namespace XYAuto.BUOC.BOP2017.Entities.Enum
{
    public enum DataStatusEnum
    {
        [Description("启用/正常")]
        Active = 0,

        [Description("逻辑删除")]
        Delete = 1
    }
}