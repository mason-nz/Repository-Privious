using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Enum
{
    /// <summary>
    /// ls
    /// </summary>
    public enum AuditStatusEnum
    {
        初始状态 = Entities.Constants.Constant.INT_INVALID_VALUE,
        新建 = 15001,
        待审核,
        已通过,
        驳回
    }

    public enum MediaAuditStatusEnum
    {
        [Description("初始化默认值")]
        Initialization = Entities.Constants.Constant.INT_INVALID_VALUE,

        [Description("待审核")]
        PendingAudit = 43001,

        [Description("已通过")]
        AlreadyPassed = 43002,

        [Description("驳回")]
        RejectNotPass = 43003,

        [Description("假通过")]
         FakePassed = 43004,
    }

    public enum MediaPublishStatusEnum
    {
        [Description("初始化默认值")]
        Initialization = Entities.Constants.Constant.INT_INVALID_VALUE,

        [Description("上架")]
        UpOnshelf = 44001,

        [Description("下架")]
        OffTheShelf = 44002,
    }
}