using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentScheduler;
using HibernatingRhinos.Profiler.Appender.EntityFramework;
using Topshelf;
using XYAuto.ChiTu2018.Service;
using XYAuto.ChiTu2018.Service.LE;
using XYAuto.ChiTu2018.Settlement.Scheduler;

namespace XYAuto.ChiTu2018.Settlement
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //EntityFrameworkProfiler.Initialize();

                //ioc 初始化
                OptionBootStarp boot = new OptionBootStarp(OptionBootStarpManage.GetAssemblies(LoadAssembType.Other));
                boot.Initialize();
                //JobManager.Initialize(new CurrentRegistryScheduler());

                TopShelfInit();
            }
            catch (Exception exception)
            {
                var msg = $" XYAuto.ArticleServices CurrentSchedulerServer is error:{exception.Message}," +
                          $"{System.Environment.NewLine}{exception.StackTrace ?? string.Empty}";
                CTUtils.Log.Log4NetHelper.Default().Error(msg);
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
                x.SetDescription("分发业务中心-服务-赤兔联盟支付对账");
                x.SetDisplayName("分发业务中心-服务-赤兔联盟支付对账");
                x.SetServiceName("分发业务中心-服务-赤兔联盟支付对账");
            });
        }
    }
}
