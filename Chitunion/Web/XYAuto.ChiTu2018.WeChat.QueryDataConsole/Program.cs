using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using Senparc.Weixin.MP.Containers;
using Topshelf;

namespace XYAuto.ChiTu2018.WeChat.QueryDataConsole
{
    class Program
    {
        
        

        //private static string WeixinAppSecret = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("WeixinAppSecret", true);

        static void Main(string[] args)
        {
            //log4net.Config.XmlConfigurator.Configure();
            //ILog logger = LogManager.GetLogger(typeof(Program));


            HostFactory.Run(x =>
            {
                x.Service<WeChatDataService>(s =>
                {
                    s.ConstructUsing(name => new WeChatDataService());
                    s.WhenStarted(tc => tc.Strat());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();
                x.SetDescription("统计微信公众号数据服务");
                x.SetDisplayName("统计微信公众号数据服务");
                x.SetServiceName("StaticWechatGongZhongHaoData");
            });
            //List<string> openIds = new List<string>();

            //WeChatUser.UserService.Instance.GetOpenIdsByAppId(WeixinAppId, string.Empty, ref openIds);

            ////XYAuto.CTUtils.Log.Log4NetHelper.Default().Info("获取OpenIds列表数据为：\r\n" + JsonConvert.SerializeObject(openIds));

            //WeChatUser.UserService.Instance.GetUserInfoByOpenIds(WeixinAppId, openIds);


        }
    }
}
