/********************************************************
*创建人：lixiong
*创建时间：2017/8/22 10:43:25
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentScheduler;
using XYAuto.ITSC.Chitunion2017.BLL.GDT;
using XYAuto.ITSC.Chitunion2017.GdtPullData.AppSettings;
using XYAuto.ITSC.Chitunion2017.GdtPullData.Provider;

namespace XYAuto.ITSC.Chitunion2017.GdtPullData.Scheduler
{
    internal class CurrentRegistryScheduler : Registry
    {
        public readonly static PullDataSettings AppSettings = PullDataSettings.Instance;

        /// <summary>
        /// 都是每个小时0分开始执行
        /// </summary>
        private void SetSchedulerEveryHoursList()
        {
            //都是每个小时0分开始执行

            //先加载账户的所有子客
            var startTime = AppSettings.AtEveryHourMinutesStart - 10 < 0
                ? 60 - 10
                : AppSettings.AtEveryHourMinutesStart - 10;
            Schedule<GdtAccuntScheduler>().ToRunEvery(1).Hours().At(startTime);

            #region 广告相关-每个小时0分钟同步一次

            //广告-获取广告组列表(因为会修改，所以，必须从头开始拉取数据)
            Schedule<GdtAdsScheduler.DoPullGetAdGroupList>()
                .WithName("DoPullGetAdGroupList")
                .AndThen<DemandStatusNotesSchduler.DemandAdGroupNotesSchduler>().ToRunEvery(1).Hours().At(AppSettings.AtEveryHourMinutesStart);

            //广告-获取推广计划
            Schedule<GdtAdsScheduler.DoPullGetAdCampaignsList>().WithName("DoPullGetAdCampaignsList").ToRunEvery(1).Hours().At(AppSettings.AtEveryHourMinutesStart);

            #endregion 广告相关-每个小时0分钟同步一次

            //以下都是：每个小时0分钟同步一次

            //账户-获取资金账户流水
            Schedule<GdtFundScheduler.DoPullGetFundStatementsDetailed>().WithName("DoPullGetFundStatementsDetailed").ToRunEvery(1).Hours().At(AppSettings.AtEveryHourMinutesStart);
            //账户-是否需要先获取所以的子客信息，然后一次轮询获取对应的账户信息
            Schedule<GdtFundScheduler.DoPullGetFundsInfo>().WithName("DoPullGetFundsInfo").ToRunEvery(1).Hours().At(AppSettings.AtEveryHourMinutesStart);
            //账户-获取今日消耗(定时每隔一个小时拉取，因为接口返回的是一条数据，不是流水，所以，我们自己得记录下来，形式一个报表)
            Schedule<GdtFundScheduler.DoPullGetRealtimeCost>().WithName("DoPullGetRealtimeCost").ToRunEvery(1).Hours().At(AppSettings.AtEveryHourMinutesStart);

            //以下都是：每个小时0分钟同步一次

            //报表-获取小时报表

            startTime = AppSettings.AtEveryHourMinutesStart + 2 > 60
                ? AppSettings.AtEveryHourMinutesStart + 2 - 60
                : AppSettings.AtEveryHourMinutesStart + 2;
            Schedule<GdtReportScheduler.DoPullGetReportHourly>()
                .AndThen<GdtReportScheduler.DoPullGetZhyHourlyReport>().WithName("DoPullGetReportHourly").ToRunEvery(1).Hours().At(startTime);
        }

        private void SetSchedulerList()
        {
            //测试用的
            //Schedule<DemandStatusNotesSchduler.DemandAdGroupNotesSchduler>().ToRunEvery(AppSettings.NormalIntverlTime).Minutes();
        }

        /// <summary>
        /// 每天定时
        /// </summary>
        private void SetSchedulerEveryDaysList()
        {
            //账户-获取资金账户日结明细
            //Schedule<GdtFundScheduler.DoPullGetFundStatementsDaily>().WithName("DoPullGetFundStatementsDaily").ToRunEvery(1).Days().At(AppSettings.AtEveryDayHourStart, AppSettings.AtEveryDayHourEnd);

            //报表-获取日报表
            //Schedule<GdtReportScheduler.DoPullGetReportDaily>().WithName("DoPullGetReportDaily").ToRunEvery(1).Days().At(AppSettings.AtEveryDayHourStart, AppSettings.AtEveryDayHourEnd);

            //授权-获取 Access Token 或刷新 Access Token
            var startTime = AppSettings.AtEveryDayHourStart - 1 <= 0
                ? 24 - 1
                : AppSettings.AtEveryDayHourStart - 1;
            Schedule<GdtTokenScheduler>().WithName("GdtTokenScheduler").ToRunEvery(1).Days().At(startTime, AppSettings.AtEveryDayHourEnd);

            var entTime = AppSettings.AtEveryDayHourEnd + 5 > 60
                ? AppSettings.AtEveryDayHourEnd + 5 - 60
                : AppSettings.AtEveryDayHourEnd + 5;
            //定时去统计需求过期。通知智慧云（因为需求是天的维度）
            Schedule<DemandStatusNotesSchduler.DemandOverdueNotesSchduler>().ToRunEvery(1).Days().At(AppSettings.AtEveryDayHourStart, entTime);
        }

        /// <summary>
        /// 这里存在一个小问题：
        /// </summary>
        public CurrentRegistryScheduler()
        {
            //这里存在一个小问题：
            //1.每个定时任务都是异步Task执行，现在都是定点准时执行
            //2.业务场景最先加载 --> 账户的所有子客 --> 然后才能继续.So.GdtAccuntScheduler 必须在所以定时任务之前几分钟执行，但是不确定什么时候能执行完成
            //3.轮询子客的列表，默认是倒叙，所以，如果子客没有完全同步下来，就开始了后面的任务，就会存在最后的子客没有被查询到

            //todo:解决办法
            //1:换成同步任务执行，不赞同，10个任务如果用同步后期数据量大可能会慢，如果一个小时没有执行完成呢？会阻塞
            //2:将现在子客的任务与后面的任务时间间隔拉长一点（现在是这种方案）

            //都是每个小时0分开始执行
            SetSchedulerEveryHoursList();
            //每天定时
            SetSchedulerEveryDaysList();

            //SetSchedulerList();

            //Schedule<SchedulerTest.DoFirst>().AndThen(() => { LoadTest(); }).ToRunNow().AndEvery(10).Seconds();
            //Schedule<SchedulerTest.DoFirst>().AndThen<SchedulerTest.DoSecond>().ToRunNow().AndEvery(10).Seconds();

            // 延迟一个指定时间间隔执行一次计划任务。（当然，这个间隔依然可以是秒、分、时、天、月、年等。）
            //Schedule<GdtAccuntScheduler>().ToRunOnceIn(5).Seconds();

            //帐号-立即执行每两秒一次的计划任务。（指定一个时间间隔运行，根据自己需求，可以是秒、分、时、天、月、年等。）
            //Schedule<GdtAccuntScheduler>().ToRunNow().AndEvery(AppSettings.NormalIntverlTime).Minutes();
        }

        private void LoadTest()
        {
            //Schedule<SchedulerTest.DoSecond>().ToRunNow().AndEvery(5).Seconds();

            Task.Run(() =>
            {
                Console.WriteLine($"this is DoSecond start.now: {DateTime.Now}");
                Thread.Sleep(3000);
                Console.WriteLine($"this is DoSecond completed.now: {DateTime.Now}");
            });

            Task.Run(() =>
            {
                Console.WriteLine($"this is DoThird start.now: {DateTime.Now}");
                Thread.Sleep(2000);
                Console.WriteLine($"this is DoThird completed.now: {DateTime.Now}");
            });
        }
    }
}