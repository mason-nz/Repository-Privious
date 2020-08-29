using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Enum
{
    public enum UploadFileEnum
    {
        [Description("帐号管理")]
        AccountManage = 33001,

        [Description("订单反馈数据")]
        OrderFeedbackData = 33002,

        [Description("媒体管理")]
        MediaManage = 33003,

        [Description("订单管理")]
        OrderManage = 33004,

        [Description("资质管理")]
        QualificationManage = 33012,

        [Description("模板管理")]
        AppAdTemplate = 33013
    }
}