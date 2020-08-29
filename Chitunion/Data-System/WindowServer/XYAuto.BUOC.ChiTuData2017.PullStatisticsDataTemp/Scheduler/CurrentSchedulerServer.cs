/********************************************************
*创建人：lixiong
*创建时间：2017/9/15 14:16:20
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using FluentScheduler;
using Topshelf;
using XYAuto.BUOC.ChiTuData2017.Infrastruction;

namespace XYAuto.BUOC.ChiTuData2017.PullStatisticsDataTemp.Scheduler
{
    internal class CurrentSchedulerServer : ServiceControl
    {
        public bool Start(HostControl hostControl)
        {
            Loger.Log4Net.Info(" BUOC.ChiTuData2017 CurrentSchedulerServer服务开始");
            try
            {
                JobManager.Initialize(new CurrentRegistryScheduler());
            }
            catch (Exception exception)
            {
                var msg = $" BUOC.ChiTuData2017 CurrentSchedulerServer Start is error :{exception.Message}" +
                          $"{System.Environment.NewLine}{exception.StackTrace ?? string.Empty}";
                Loger.Log4Net.Error(msg);
            }
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            //JobManager.Stop();
            JobManager.StopAndBlock();
            Loger.Log4Net.Error(" BUOC.ChiTuData2017 CurrentSchedulerServer服务结束");
            return true;
        }
    }
}