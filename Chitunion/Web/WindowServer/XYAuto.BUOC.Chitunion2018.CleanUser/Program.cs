using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace XYAuto.BUOC.Chitunion2018.CleanUser
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            ILog logger = LogManager.GetLogger(typeof(Program));

            //new SyncService().SyncTask();
            //Console.ReadKey();
            HostFactory.Run(x =>
            {
                x.Service<SyncService>(s =>
                {
                    s.ConstructUsing(name => new SyncService());
                    s.WhenStarted(tc => tc.Strat());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();
                x.SetDescription("合并用户数据-赤兔");
                x.SetDisplayName("合并用户数据-赤兔");
                x.SetServiceName("ClassificationUserSync");
            });
            //new SyncService().SyncTask();
        }
    }
}
