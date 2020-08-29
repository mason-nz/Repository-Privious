using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Enums
{
    public enum VerifyDataTypeEnum
    {
        [Description("校验媒体weixin信息是否存在")]
        VerifyOfWeiXinById = 1,
        [Description("校验媒体weixin互动参数信息是否存在")]
        VerifyOfInteractionWeiXinByMediaId = 2,
         [Description("校验媒体WeiBo信息是否存在")]
        VerifyOfWeiBoById = 3,
        [Description("校验媒体WeiBo互动参数信息是否存在")]
        VerifyOfInteractionWeiBoByMediaId = 4,
        [Description("校验媒体新增和编辑的角色")]
        VerifyOfMediaOperateRoleCheckRights = 5
    }
}
