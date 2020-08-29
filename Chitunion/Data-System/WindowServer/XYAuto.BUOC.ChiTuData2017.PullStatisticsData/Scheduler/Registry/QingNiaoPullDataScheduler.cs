/********************************************************
*创建人：lixiong
*创建时间：2017/9/15 14:26:06
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentScheduler;
using XYAuto.BUOC.ChiTuData2017.BLL.ExternalApi;
using XYAuto.BUOC.ChiTuData2017.BLL.ExternalApi.QingNiao;
using XYAuto.BUOC.ChiTuData2017.Infrastruction;
using XYAuto.BUOC.ChiTuData2017.PullStatisticsData.AppSettings;

namespace XYAuto.BUOC.ChiTuData2017.PullStatisticsData.Scheduler.Registry
{
    internal class QingNiaoPullDataScheduler : IJob
    {
        private static MaterielStatisticsProviderExt _provider;

        public QingNiaoPullDataScheduler()
        {
            _provider = new MaterielStatisticsProviderExt(new PullDataConfig()
            {
                DateOffset = PullDataSettings.Instance.ExcuteDateOffsetStat
            });
        }

        public void Execute()
        {
            Loger.Log4Net.Info($" QingNiaoPullDataScheduler PullStatistics start ...");
            try
            {
                _provider.LoopPullStatistics();
            }
            catch (Exception exception)
            {
                Loger.Log4Net.Error($" QingNiaoPullDataScheduler PullStatistics is error .:{exception.Message}," +
                        $"{System.Environment.NewLine}{exception.StackTrace ?? string.Empty}");
            }

            Loger.Log4Net.Info($" QingNiaoPullDataScheduler PullStatistics completed ...");

            //发送邮件通知
            var queryDay = DateTime.Now.AddDays(PullDataSettings.Instance.ExcuteDateOffsetStat).ToString("yyyy-MM-dd");

            EmailNotes.SendByLog("青鸟", queryDay);
        }
    }
}