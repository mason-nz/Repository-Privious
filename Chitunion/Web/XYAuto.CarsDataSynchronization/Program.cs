using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topshelf;
using System.IO;
using log4net.Config;

namespace XYAuto.CarsDataSynchronization
{
    class Program
    {
        static void Main(string[] args)
        {

            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);
            HostFactory.Run(x =>
            {
                x.Service<CarsSyncDataService>(s =>
                    {
                        s.ConstructUsing(name => new CarsSyncDataService());
                        s.WhenStarted(tc => tc.Start());
                        s.WhenStopped(tc => tc.Stop());
                    });
                x.RunAsLocalSystem();
                x.SetDescription("同步汽车车系数据；同步汽车品牌数据");
                x.SetDisplayName("车型数据同步");
                x.SetServiceName("CarsDataSynchronization");
            });
            //x.Service<CarsSyncDataService>();
            //x.SetDescription("根据接口查询【行圆汽车公司】下所有的子部门并同步至新库的部门表中；根据接口查询每个子部门下所有的人员信息并经过处理器对某些字段处理之后同步至新库的人员表中。");
            //x.SetDisplayName("同步部门人员服务");
            //x.SetServiceName("SynDataDepartEmployeeService");
            //x.StartAutomatically();
            //x.EnablePauseAndContinue();
            //});
        }
    }
}
