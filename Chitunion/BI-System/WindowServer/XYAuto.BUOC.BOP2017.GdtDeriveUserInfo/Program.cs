using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using XYAuto.BUOC.BOP2017.Infrastruction;

namespace XYAuto.BUOC.BOP2017.GdtDeriveUserInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                Loger.Log4Net.Info("拉取DeriveUserInfo开始");
                x.Service<ServiceRunner>();

                x.RunAsLocalSystem();

                x.SetDescription("DeriveUserInfo拉取");
                x.SetDisplayName("DeriveUserInfo");
                x.SetServiceName("DeriveUserInfo");

                x.EnablePauseAndContinue();

            });
        }
    }
}
