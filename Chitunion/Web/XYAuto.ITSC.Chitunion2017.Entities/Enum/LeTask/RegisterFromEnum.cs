using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask
{
    /// <summary>
    /// 注册来源：PC，资源管理系统
    /// </summary>
    public enum RegisterFromEnum
    {
        未知 = Entities.Constants.Constant.INT_INVALID_VALUE,
        自营 = 3001,
        自助pc端 = 3002,
        库存 = 3003,
        智慧云 = 3004,
        外部渠道API账号 = 3005,
        赤兔联盟微信服务号 = 3006,
        H5 = 3007,
        APP = 3008
    }
}
