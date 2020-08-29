using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask
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
