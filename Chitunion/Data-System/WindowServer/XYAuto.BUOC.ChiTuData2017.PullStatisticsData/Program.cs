using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentScheduler;
using Topshelf;
using XYAuto.BUOC.ChiTuData2017.Infrastruction;
using XYAuto.BUOC.ChiTuData2017.PullStatisticsData.AppSettings;
using XYAuto.BUOC.ChiTuData2017.PullStatisticsData.Scheduler;
using XYAuto.BUOC.ChiTuData2017.PullStatisticsData.Scheduler.Registry;

namespace XYAuto.BUOC.ChiTuData2017.PullStatisticsData
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                BLL.AutoMapperConfig.AutoMapperConfig.Configure();
                //JobManager.Initialize(new CurrentRegistryScheduler());
                TopShelfInit();
            }
            catch (Exception exception)
            {
                var msg = $" BUOC.ChiTuData2017 CurrentSchedulerServer is error:{exception.Message}," +
                          $"{System.Environment.NewLine}{exception.StackTrace ?? string.Empty}";
                Loger.Log4Net.Error(msg);
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
                x.SetDescription("流量变现-数据分析系统-物料分发数据拉取服务（定时拉取智慧云，青鸟物料统计相关信息）");
                x.SetDisplayName("流量变现-数据分析系统-物料分发数据拉取服务");
                x.SetServiceName("流量变现-数据分析系统-物料分发数据拉取服务");
            });
        }
    }
}