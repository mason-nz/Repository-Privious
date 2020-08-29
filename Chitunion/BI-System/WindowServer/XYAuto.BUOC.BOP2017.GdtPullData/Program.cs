using System;
using FluentScheduler;
using Newtonsoft.Json;
using Topshelf;
using XYAuto.BUOC.BOP2017.GdtPullData.AppSettings;
using XYAuto.BUOC.BOP2017.GdtPullData.Scheduler;
using XYAuto.BUOC.BOP2017.Infrastruction;

namespace XYAuto.BUOC.BOP2017.GdtPullData
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                BLL.AutoMapperConfig.MediaMapperConfig.Configure();
                //JobManager.Initialize(new CurrentRegistryScheduler());
                TopShelfInit();
            }
            catch (Exception exception)
            {
                var msg = $"CurrentSchedulerServer is error:{exception.Message}," +
                          $"{System.Environment.NewLine}{exception.StackTrace ?? string.Empty}";
                Loger.Log4Net.Error(msg);
                Loger.GdtLogger.Error(msg);
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
                x.SetDescription("流量变现-商业运营中心-广点通数据拉取服务（定时拉取帐号，账户，资金，广告，报表相关信息）");
                x.SetDisplayName("流量变现-商业运营中心-广点通数据拉取服务");
                x.SetServiceName("流量变现-商业运营中心-广点通数据拉取服务");
            });
        }
    }
}