using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using XYAuto.ITSC.Chitunion2017.LuceneMedia.LuceneManage;
using XYAuto.ITSC.Chitunion2017.LuceneMediaConsole.Common;
using XYAuto.ITSC.Chitunion2017.LuceneMediaConsole.LuceneManage;

namespace XYAuto.ITSC.Chitunion2017.LuceneMediaConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //TagVehicleLucene.Instance.AddTagVehicleIndex();

            Log4NetHelper.Debug("服务开始启动...");
            try
            {
                HostFactory.Run(x =>
                {

                    x.Service<ServiceManage>(s =>                        //2
                    {
                        s.ConstructUsing(name => new ServiceManage());     //3
                        s.WhenStarted(tc => tc.Start());              //4
                        s.WhenStopped(tc => tc.Stop());               //5
                    });
                    x.RunAsLocalSystem();
                    x.SetDescription("赤兔平台-定时更新向Lucene更新媒体索引信息"); //安装服务后，服务的描述
                    x.SetDisplayName("赤兔平台-索引更新服务");//显示名称
                    x.SetServiceName("赤兔平台现-索引更新服务"); //服务名称
                });

            }
            catch (Exception ex)
            {

                Log4NetHelper.Error("服务启动异常：", ex);
            }

        }

    }
}
