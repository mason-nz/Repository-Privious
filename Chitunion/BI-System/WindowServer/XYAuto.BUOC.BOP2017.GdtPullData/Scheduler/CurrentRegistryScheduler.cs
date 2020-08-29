/********************************************************
*创建人：lixiong
*创建时间：2017/8/22 10:43:25
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Threading;
using System.Threading.Tasks;
using FluentScheduler;
using XYAuto.BUOC.BOP2017.GdtPullData.AppSettings;
using XYAuto.BUOC.BOP2017.GdtPullData.Provider;

namespace XYAuto.BUOC.BOP2017.GdtPullData.Scheduler
{
    internal class CurrentRegistryScheduler : Registry
    {
        public readonly static PullDataSettings AppSettings = PullDataSettings.Instance;

        /// <summary>
        /// 都是每个小时0分开始执行
        /// </summary>
        private void SetSchedulerEveryHoursList()
        {
            #region 广告相关-每个小时0分钟同步一次

            //广告-获取推广计划
            Schedule<GdtAdsScheduler.DoPullGetAdCampaignsList>()
                .WithName("DoPullGetAdCampaignsList").ToRunEvery(1).Hours().At(AppSettings.AtEveryHourMinutesStart);

            #endregion 广告相关-每个小时0分钟同步一次

            //以下都是：每个小时0分钟同步一次

            //账户-获取资金账户流水
            Schedule<GdtFundScheduler.DoPullGetFundStatementsDetailed>()
                .WithName("DoPullGetFundStatementsDetailed").ToRunEvery(1).Hours().At(AppSettings.AtEveryHourMinutesStart);
            //账户-是否需要先获取所以的子客信息，然后一次轮询获取对应的账户信息
            Schedule<GdtFundScheduler.DoPullGetFundsInfo>()
                .WithName("DoPullGetFundsInfo").ToRunEvery(1).Hours().At(AppSettings.AtEveryHourMinutesStart);
            //账户-获取今日消耗(定时每隔一个小时拉取，因为接口返回的是一条数据，不是流水，所以，我们自己得记录下来，形式一个报表)
            Schedule<GdtFundScheduler.DoPullGetRealtimeCost>()
                .WithName("DoPullGetRealtimeCost").ToRunEvery(1).Hours().At(AppSettings.AtEveryHourMinutesStart);

            //以下都是：每个小时0分钟同步一次

            //广告-获取广告组列表(因为会修改，所以，必须从头开始拉取数据)
            //报表-获取小时报表
            Schedule<GdtAdsScheduler.DoPullGetAdGroupList>()
                .AndThen<GdtReportScheduler.DoPullGetReportHourly>()
                .AndThen<GdtReportScheduler.DoPullGetZhyHourlyReport>()
                .WithName("DoPullGetReportHourly")
                .ToRunEvery(1)
                .Hours()
                .At(AppSettings.AtEveryHourMinutesStart);
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
            Schedule<GdtTokenScheduler>().WithName("GdtTokenScheduler")
                .ToRunEvery(1).Days().At(AppSettings.AtEveryDayHourRangeAccessTokenStart, AppSettings.AtEveryDayHourRangeAccessTokenEnd);

            //日结报表
            Schedule<GdtReportScheduler.DoPullGetReportDaily>()
                 .ToRunEvery(1).Days().At(AppSettings.AtEveryDayHourStart, AppSettings.AtEveryDayHourEnd);
            //日结明细
            Schedule<GdtFundScheduler.DoPullGetFundStatementsDaily>()
                .ToRunEvery(1).Days().At(AppSettings.AtEveryDayHourStart, AppSettings.AtEveryDayHourEnd);

            //需求达到开始时间，自动设置为【投放中】状态，并通知智慧云
            Schedule<DemandStatusNotesSchduler.DemandOverdueNotesSchduler>()
                .ToRunEvery(1).Days().At(AppSettings.AtEveryDayHourStart, AppSettings.AtEveryDayHourEnd);
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
            Schedule<GdtAccuntScheduler>()
                .WithName("GdtAccuntScheduler")
                .ToRunEvery(1).Hours().At(AppSettings.AtEveryHourMinutesGdtAccuntSchedulerStart);

            SetSchedulerEveryHoursList();
            //每天定时
            SetSchedulerEveryDaysList();

            //测试凌晨的时间点是：24 还是 00    (经过测试，24 00 都是可以正常执行)
            //LoadTest();
        }

        private void LoadTest()
        {
            Schedule<SchedulerTest.DoFirst>().ToRunEvery(1).Days().At(24, 05);
            Schedule<SchedulerTest.DoSecond>().ToRunEvery(1).Days().At(00, 10);
        }
    }
}