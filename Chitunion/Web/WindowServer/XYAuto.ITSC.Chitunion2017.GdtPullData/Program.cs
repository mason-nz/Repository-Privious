using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentScheduler;
using Newtonsoft.Json;
using Topshelf;
using XYAuto.ITSC.Chitunion2017.BLL.GDT;
using XYAuto.ITSC.Chitunion2017.GdtPullData.Scheduler;

namespace XYAuto.ITSC.Chitunion2017.GdtPullData
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            BLL.AutoMapperConfig.MediaMapperConfig.Configure();
            try
            {
                //JobManager.Initialize(new CurrentRegistryScheduler());
                TopShelfInit();
            }
            catch (Exception exception)
            {
                var msg = $"CurrentSchedulerServer is error:{exception.Message}," +
                          $"{System.Environment.NewLine}{exception.StackTrace ?? string.Empty}";
                BLL.Loger.Log4Net.Error(msg);
                BLL.Loger.GdtLogger.Error(msg);
            }
            Console.ReadLine();
        }

        public static void TopShelfInit()
        {
            HostFactory.Run(x =>
            {
                x.Service<CurrentSchedulerServer>(s =>
                {
                    s.ConstructUsing(name => new CurrentSchedulerServer());
                    s.WhenStarted((tc, hostControl) => tc.Start(hostControl));
                    s.WhenStopped((tc, hostControl) => tc.Stop(hostControl));
                });
                x.SetStopTimeout(new TimeSpan(0, 20, 0));
                x.RunAsLocalSystem();
                x.StartAutomaticallyDelayed();
                x.SetDescription("流量变现-广点通数据拉取服务（定时拉取帐号，账户，资金，广告，报表相关信息）");
                x.SetDisplayName("流量变现-广点通数据拉取服务");
                x.SetServiceName("流量变现-广点通数据拉取服务");
            });
        }
    }
}