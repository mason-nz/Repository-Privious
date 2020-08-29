using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Enum
{
    public enum AppTemplateEnum
    {
        默认 = Constants.Constant.INT_INVALID_VALUE,

        待审核 = 48001,
        已通过,
        已驳回,
        已删除
    }

    public enum AppTemplateAdFormEnum
    {
        Banner = 51001,
        信息流 = 51002,
        启动页,
        异形,
        通栏,
        焦点图,
        消息推送通知,
        冠名,
        帖子,
        弹窗,
        文字链,
        背景图
    }
}