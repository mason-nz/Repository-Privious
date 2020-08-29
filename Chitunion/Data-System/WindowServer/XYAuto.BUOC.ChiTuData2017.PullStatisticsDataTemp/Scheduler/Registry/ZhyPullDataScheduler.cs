/********************************************************
*创建人：lixiong
*创建时间：2017/9/15 14:24:01
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Net.Http;
using FluentScheduler;
using Newtonsoft.Json;
using XYAuto.BUOC.ChiTuData2017.BLL.ExternalApi;
using XYAuto.BUOC.ChiTuData2017.BLL.ExternalApi.ZHY;
using XYAuto.BUOC.ChiTuData2017.Infrastruction;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Http;
using XYAuto.BUOC.ChiTuData2017.PullStatisticsDataTemp.AppSettings;

namespace XYAuto.BUOC.ChiTuData2017.PullStatisticsDataTemp.Scheduler.Registry
{
    internal class ZhyPullDataScheduler : IJob
    {
        private static LogicByMaterielStatisticsTemp _prvoider;

        public ZhyPullDataScheduler()
        {
            _prvoider = new LogicByMaterielStatisticsTemp(new DoHttpClient(new System.Net.Http.HttpClient()), new PullDataConfig()
            {
                DateOffset = PullDataSettings.Instance.ExcuteDateOffset,
                PullDataQueryDateOffset = PullDataSettings.Instance.PullDataQueryDateOffset
            });
        }

        public void Execute()
        {
            Loger.Log4Net.Info($" ZhyPullDataScheduler PullStatistics start ...");
            try
            {
                _prvoider.LoopPullStatistics();
            }
            catch (Exception exception)
            {
                Loger.Log4Net.Error($" ZhyPullDataScheduler PullStatistics is error .:{exception.Message}," +
                     $"{System.Environment.NewLine}{exception.StackTrace ?? string.Empty}");
            }

            Loger.Log4Net.Info($" ZhyPullDataScheduler PullStatistics completed ...");

            //发送邮件通知
            var queryDay = DateTime.Now.AddDays(PullDataSettings.Instance.ExcuteDateOffset).ToString("yyyy-MM-dd");
            EmailNotes.SendByLog("智慧云", queryDay);
        }
    }
}