using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Enum
{
    public enum MediaType
    {
        [Description("微信公众号")]
        WeiXin = 14001,

        [Description("APP")]
        APP = 14002,

        [Description("新浪微博")]
        WeiBo = 14003,

        [Description("视频")]
        Video = 14004,

        [Description("直播")]
        Broadcast = 14005,

        [Description("模板")]
        Template = 15000
    }

    public enum MediaRelationType
    {
        [Description("副表（附表）相关操作")]
        Attached = 0,

        [Description("基表（主表）相关操作")]
        BaseTable = 1
    }

    public enum MediaAreaMappingType
    {
        [Description("媒体-覆盖区域")]
        CoverageArea = 59001,

        [Description("媒体-区域媒体")]
        AreaMedia = 59002
    }
}