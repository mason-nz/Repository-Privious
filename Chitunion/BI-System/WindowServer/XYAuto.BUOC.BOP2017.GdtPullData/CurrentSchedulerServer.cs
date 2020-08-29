/********************************************************
*创建人：lixiong
*创建时间：2017/8/22 15:26:40
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using FluentScheduler;
using Topshelf;
using XYAuto.BUOC.BOP2017.GdtPullData.Scheduler;
using XYAuto.BUOC.BOP2017.Infrastruction;

namespace XYAuto.BUOC.BOP2017.GdtPullData
{
    internal class CurrentSchedulerServer : ServiceControl
    {
        public bool Start(HostControl hostControl)
        {
            Loger.GdtLogger.Info("CurrentSchedulerServer服务开始");
            try
            {
                JobManager.Initialize(new CurrentRegistryScheduler());
            }
            catch (Exception exception)
            {
                var msg = $"CurrentSchedulerServer Start is error :{exception.Message}" +
                          $"{System.Environment.NewLine}{exception.StackTrace ?? string.Empty}";
                Loger.Log4Net.Error(msg);
                Loger.GdtLogger.Error(msg);
            }
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            //JobManager.Stop();
            JobManager.StopAndBlock();
            Loger.GdtLogger.Info("CurrentSchedulerServer服务结束");
            Loger.Log4Net.Error("CurrentSchedulerServer服务结束");
            return true;
        }
    }
}