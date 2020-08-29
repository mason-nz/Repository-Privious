/********************************************************
*创建人：lixiong
*创建时间：2017/9/15 14:19:04
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using XYAuto.BUOC.ChiTuData2017.PullStatisticsData.AppSettings;
using XYAuto.BUOC.ChiTuData2017.PullStatisticsData.Scheduler.Registry;

namespace XYAuto.BUOC.ChiTuData2017.PullStatisticsData.Scheduler
{
    internal class CurrentRegistryScheduler : FluentScheduler.Registry
    {
        public readonly static PullDataSettings AppSettings = PullDataSettings.Instance;

        public CurrentRegistryScheduler()
        {
            //先拉取分发明细，然后再拉取统计信息
            Schedule<ZhyPullDistributeDetailSchduler>().WithName("ZhyPullDistributeDetailSchduler")
                .AndThen<ZhyPullDataScheduler>()
                .ToRunEvery(1).Days().At(AppSettings.AtEveryDayHourStartDistributeDetail, AppSettings.AtEveryDayHourEndDistributeDetail);

            //Schedule<ZhyPullDataScheduler>().WithName("ZhyPullDataScheduler")
            //  .ToRunEvery(1).Days().At(AppSettings.AtEveryDayHourStartDistributeDetail, AppSettings.AtEveryDayHourEndDistributeDetail);

            //青鸟汽车大全
            Schedule<QingNiaoPullDataScheduler>().WithName("QingNiaoPullDataScheduler")
                .ToRunEvery(1).Days().At(AppSettings.AtEveryDayHourStartStat, AppSettings.AtEveryDayHourEndStat);

            //检测是否存在分发数据
            Schedule<ZhyPullDistributeNoteScheduler>().WithName("ZhyPullDistributeNoteScheduler")
                 .ToRunEvery(1).Days().At(AppSettings.AtEveryDayRangeDistributeDetailVerifyIsExistStart, AppSettings.AtEveryDayRangeDistributeDetailVerifyIsExistEnd);
        }
    }
}