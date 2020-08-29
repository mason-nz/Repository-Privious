using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.Settlement.ConfigEntity;
using XYAuto.ChiTu2018.Settlement.Scheduler.Registry;

namespace XYAuto.ChiTu2018.Settlement.Scheduler
{
    /// <summary>
    /// 注释：CurrentRegistryScheduler
    /// 作者：lix
    /// 日期：2018/5/22 11:07:41
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class CurrentRegistryScheduler : FluentScheduler.Registry
    {
        public readonly static ConfigEntityBase Configs = ConfigEntity.ConfigSettings.GetConfig();
        public CurrentRegistryScheduler()
        {
            //库容支付对账
            Schedule<SettlementKrPayScheduler>().WithName("SettlementKrPayScheduler")
                .ToRunEvery(1).Days().At(Configs.AtEveryDayRangeForHour, Configs.AtEveryDayRangeForMinutes);
        }
    }
}
