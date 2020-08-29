using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentScheduler;
using Topshelf;

namespace XYAuto.ChiTu2018.Settlement.Scheduler
{
    /// <summary>
    /// 注释：CurrentSchedulerServer
    /// 作者：lix
    /// 日期：2018/5/22 11:07:29
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    internal class CurrentSchedulerServer
    {
        public bool Start(HostControl hostControl)
        {
            XYAuto.CTUtils.Log.Log4NetHelper.Default().Info(" XYAuto.ChiTu2018.Settlement CurrentSchedulerServer服务开始");
            try
            {
                JobManager.Initialize(new CurrentRegistryScheduler());
            }
            catch (Exception exception)
            {
                var msg = $" XYAuto.ChiTu2018.Settlement CurrentSchedulerServer Start is error :{exception.Message}" +
                          $"{System.Environment.NewLine}{exception.StackTrace ?? string.Empty}";
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Error(msg);
            }
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            //JobManager.Stop();
            JobManager.StopAndBlock();
            XYAuto.CTUtils.Log.Log4NetHelper.Default().Error(" XYAuto.ChiTu2018.Settlement CurrentSchedulerServer服务结束");
            return true;
        }
    }
}
