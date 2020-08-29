/********************************************************
*创建人：lixiong
*创建时间：2017/8/22 15:26:40
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentScheduler;
using Topshelf;
using XYAuto.ITSC.Chitunion2017.GdtPullData.Scheduler;

namespace XYAuto.ITSC.Chitunion2017.GdtPullData
{
    internal class CurrentSchedulerServer : ServiceControl
    {
        public bool Start(HostControl hostControl)
        {
            BLL.Loger.GdtLogger.Info("CurrentSchedulerServer服务开始");
            try
            {
                JobManager.Initialize(new CurrentRegistryScheduler());
            }
            catch (Exception exception)
            {
                var msg = $"CurrentSchedulerServer Start is error :{exception.Message}" +
                          $"{System.Environment.NewLine}{exception.StackTrace ?? string.Empty}";
                BLL.Loger.Log4Net.Error(msg);
                BLL.Loger.GdtLogger.Error(msg);
            }
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            //JobManager.Stop();
            JobManager.StopAndBlock();
            BLL.Loger.GdtLogger.Info("CurrentSchedulerServer服务结束");
            BLL.Loger.Log4Net.Error("CurrentSchedulerServer服务结束");
            return true;
        }
    }
}