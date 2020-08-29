using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Enum
{
    public enum DataStatusEnum
    {
        [Description("启用/正常")]
        Active = 0,

        [Description("逻辑删除")]
        Delete = 1
    }
}