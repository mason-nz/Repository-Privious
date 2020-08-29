/********************************
* 项目名称 ：XYAuto.ChiTu2018.Utils.Enum.Profit
* 类 名 称 ：ProfitTypeEnum
* 描    述 ：枚举
* 作    者 ：zhangjl
* 创建时间 ：2018/5/10 12:00:15
********************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Entities.Enum.Profit
{
    public enum ProfitTypeEnum
    {
        订单统计 = 103001,
        邀请红包统计 = 103002,
        签到红包统计 = 103003,
        提现申请海报奖励 = 103004,
        新用户奖励文章 = 103005,
        新用户奖励海报,
        活动收益 = 103007,
        签到抽奖 = 103008
    }
}
