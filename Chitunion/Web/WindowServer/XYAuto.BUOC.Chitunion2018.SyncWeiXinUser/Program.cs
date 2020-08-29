

using Topshelf;

namespace XYAuto.BUOC.Chitunion2018.SyncWeiXinUser
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {

                x.Service<ServiceRunner>();

                x.RunAsLocalSystem();

                x.SetDescription("同步微信用户");
                x.SetDisplayName("SyncWeiXinUser");
                x.SetServiceName("SyncWeiXinUser");

                x.EnablePauseAndContinue();

            });
        }
    }
}
