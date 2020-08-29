/**
*----------Dragon be here!----------/
* 　　    ┏ ┓　.┏ ┓
* 　　┏━┛ ┻━━┛ ┻━━┓
* 　　┃　　    .　　  ┃ 
* 　　┃　 ┳┛　┗┳　 ┃
* 　　┃　　　　　　┃
* 　　┃　    ━┻━　　┃
* 　　┗━┓　　　  ┏━┛
* 　　    ┃  　　　┃
* 　　　 ┃  　　   ┗━━━━━━━━┓
* 　　　 ┃  　神兽保佑　  　  .┣┓
* 　　　 ┃　  永无BUG　　　 ┏┛
* 　　　 ┗┓┓┏━━━┳┓┏━━━━━━┛
* 　　　   ┗┻┛      ┗┻┛
*-------------------------------------/
*/
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topshelf;

namespace XYAuto.ITSC.Chitunion2017.WxDataSync
{
    class Program
    {
        static void Main(string[] args)
        {
            //SyncService service = new SyncService();
            //service.Run(null,null);

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
                x.SetDescription("微信授权同步,抓取任务推送,抓取数据同步");
                x.SetDisplayName("微信授权数据同步");
                x.SetServiceName("WxDataSync");
            });
        }
    }
}
