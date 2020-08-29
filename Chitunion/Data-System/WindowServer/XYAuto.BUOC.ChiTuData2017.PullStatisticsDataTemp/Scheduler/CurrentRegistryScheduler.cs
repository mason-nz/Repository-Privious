/********************************************************
*创建人：lixiong
*创建时间：2017/9/15 14:19:04
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using XYAuto.BUOC.ChiTuData2017.PullStatisticsDataTemp.AppSettings;
using XYAuto.BUOC.ChiTuData2017.PullStatisticsDataTemp.Scheduler.Registry;

namespace XYAuto.BUOC.ChiTuData2017.PullStatisticsDataTemp.Scheduler
{
    internal class CurrentRegistryScheduler : FluentScheduler.Registry
    {
        public readonly static PullDataSettings AppSettings = PullDataSettings.Instance;

        public CurrentRegistryScheduler()
        {
            //Schedule<ZhyPullDataScheduler>().WithName("ZhyPullDataScheduler")
            //    .ToRunEvery(1).Days().At(AppSettings.AtEveryDayHourStart, AppSettings.AtEveryDayHourEnd);

            Schedule<ZhyPullDistributeDetailSchduler>().WithName("ZhyPullDistributeDetailSchduler")
                .ToRunEvery(1).Days().At(AppSettings.AtEveryDayHourStart, AppSettings.AtEveryDayHourEnd);
        }
    }
}