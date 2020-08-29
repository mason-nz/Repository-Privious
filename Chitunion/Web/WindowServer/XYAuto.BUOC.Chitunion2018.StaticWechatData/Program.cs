using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace XYAuto.BUOC.Chitunion2018.StaticWechatData
{
    class Program
    {
        static void Main(string[] args)
        {
            
            log4net.Config.XmlConfigurator.Configure();
            ILog logger = LogManager.GetLogger(typeof(Program));
            HostFactory.Run(x =>
            {
                x.Service<SyncService>(s =>
                {
                    s.ConstructUsing(name => new SyncService());
                    s.WhenStarted(tc => tc.Strat());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();
                x.SetDescription("统计微信数据服务");
                x.SetDisplayName("统计微信数据服务");
                x.SetServiceName("StaticWechatData");
            });

        }
    }
}
