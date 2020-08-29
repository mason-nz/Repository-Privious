using log4net;
using Senparc.Weixin.MP.Containers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace XYAuto.BUOC.Chitunion2018.SendWechatUserMsg
{
    /// <summary>
    /// 查询DB数据，发送微信模板消息服务，每天跑一次
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            ILog logger = LogManager.GetLogger(typeof(Program));
            ////注册公众号
            //AccessTokenContainer.Register(
            //    System.Configuration.ConfigurationManager.AppSettings["WeixinAppId"],
            //    System.Configuration.ConfigurationManager.AppSettings["WeixinAppSecret"],
            //    "【ChiTu】公众号");

            HostFactory.Run(x =>
            {
                x.Service<SyncService>(s =>
                {
                    s.ConstructUsing(name => new SyncService());
                    s.WhenStarted(tc => tc.Strat());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();
                x.SetDescription("发送微信用户数据服务");
                x.SetDisplayName("发送微信用户数据服务");
                x.SetServiceName("SendWechatUserData");
            });


            //#region 前一天未进行分发订单用户
            //DataSet ds = XYAuto.ITSC.Chitunion2017.BLL.WechatUserMsg.WechatUmsg.Instance.NoOrderOpenIds();
            //if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            //{
            //    ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("ToOpenIdsForNoOrder_OneDay-----方法开始");
            //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    {
            //        DataRow row = ds.Tables[0].Rows[i];
            //        if (row["openid"].ToString() != "")
            //        {
            //            new SyncService().ToOpenIdsForNoOrder_OneDay(row["openid"].ToString(), row["nickname"].ToString(), row["CreateTime"].ToString() == "" ? "" : Convert.ToDateTime(row["CreateTime"]).ToString("yyyy-MM-dd HH:mm"));
            //        }
            //    }
            //    ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("ToOpenIdsForNoOrder_OneDay-----方法结束");
            //}
            //else
            //{
            //    ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("没有查询到前天未分发订单用户");
            //}
            //#endregion


            //DataTable dt = XYAuto.ITSC.Chitunion2017.BLL.WechatUserMsg.WechatUmsg.Instance.OrderWeekProfitOpenIds();
            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("ToOpenIdsForOrderWeekProfit-----方法开始");
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        DataRow row = dt.Rows[i];
            //        if (row["openid"].ToString() != "")
            //        {
            //            new SyncService().ToOpenIdsForOrderWeekProfit(row["openid"].ToString(), row["nickname"].ToString(), row["TimeSlot"].ToString(), row["TotalMoney"].ToString());
            //        }
            //    }
            //    ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("ToOpenIdsForOrderWeekProfit-----方法结束");
            //}
            //else
            //{
            //    ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("没有查询到前一周有分发收益的用户");
            //}
            //string date = DateTime.Now.GetDateTimeFormats('f')[0].ToString();
            //new SyncService().ToOpenIdsForActivity("非常抱歉，系统正在抢修，请您耐心等待～", "为什么赤兔网页无法访问？", date, "由于赤兔系统升级导致页面无法正常访问，我们正在抢修，给您带来的不便，敬请谅解！", "了解修复进度，请戳此处>>", "https://mp.weixin.qq.com/s/8mj7ru8nNdWsP484swb_HA");
        }

    }
}
