/********************************************************
*创建人：lixiong
*创建时间：2017/9/15 14:24:01
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentScheduler;
using Newtonsoft.Json;
using XYAuto.BUOC.ChiTuData2017.BLL.ExternalApi;
using XYAuto.BUOC.ChiTuData2017.BLL.ExternalApi.ZHY;
using XYAuto.BUOC.ChiTuData2017.Infrastruction;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Http;
using XYAuto.BUOC.ChiTuData2017.PullStatisticsData.AppSettings;

namespace XYAuto.BUOC.ChiTuData2017.PullStatisticsData.Scheduler.Registry
{
    internal class ZhyPullDataScheduler : IJob
    {
        private static LogicByMaterielStatistics _prvoider;

        public ZhyPullDataScheduler()
        {
            _prvoider = new LogicByMaterielStatistics(new DoHttpClient(new System.Net.Http.HttpClient()), new PullDataConfig()
            {
                DateOffset = PullDataSettings.Instance.ExcuteDateOffsetDistribute,//直接定位到-20天的节点
                PullDataQueryDateOffset = PullDataSettings.Instance.DistributeQueryDateOffset//当前时间节点+10天
            });
        }

        public void Execute()
        {
            Loger.Log4Net.Info($" ZhyPullDataScheduler PullStatistics start ...");
            Loger.ZhyLogger.Info($" ZhyPullDataScheduler PullStatistics start ...");
            try
            {
                _prvoider.LoopPullStatistics();
                //if (retValue.HasError)
                //{
                //    Loger.Log4Net.Error($" ZhyPullDataScheduler PullStatistics is error.{JsonConvert.SerializeObject(retValue)}");
                //}
            }
            catch (Exception exception)
            {
                Loger.Log4Net.Error($" ZhyPullDataScheduler PullStatistics is error .:{exception.Message}," +
                     $"{System.Environment.NewLine}{exception.StackTrace ?? string.Empty}");
            }

            Loger.ZhyLogger.Info($" ZhyPullDataScheduler PullStatistics completed ...");
            Loger.Log4Net.Info($" ZhyPullDataScheduler PullStatistics completed ...");
            //发送邮件通知
            var queryDay = DateTime.Now.AddDays(PullDataSettings.Instance.ExcuteDateOffsetStat).ToString("yyyy-MM-dd");
            EmailNotes.SendByMsg(_prvoider.RequestApiCount, _prvoider.RequestApiNotDataCount);
        }
    }
}