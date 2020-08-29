using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Senparc.Weixin.MP.Containers;

namespace XYAuto.ChiTu2018.WeChat.QueryDataConsole
{
    /// <summary>
    /// 注释：WeChatDataService
    /// 作者：masj
    /// 日期：2018/5/26 19:14:50
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class WeChatDataService
    {
        private System.Timers.Timer timer = null;
        private readonly int _serviceInterval = ConfigurationManager.AppSettings["ServiceInterval"] == "" ? 60 : Convert.ToInt16(ConfigurationManager.AppSettings["ServiceInterval"]);
        private readonly string _operateTimer = ConfigurationManager.AppSettings["OperateTimer"] == "" ? string.Empty : ConfigurationManager.AppSettings["OperateTimer"].ToString();
        //private static string WeixinAppIdAndSecret = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("WeixinAppIdAndSecret", true);
        private static List<Tuple<int, string, string, string>> IdAndSecrets = null;//GetWeChatIdAndSecrets(WeixinAppIdAndSecret);

        private List<Tuple<int, string, string, string>> GetWeChatIdAndSecrets()
        {
            List<Tuple<int, string, string, string>> temp = new List<Tuple<int, string, string, string>>();
            DataTable dt = XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeixinInfo.Instance.GetList();
            if (dt != null)
            {
                //string[] list = weixinAppIdAndSecret.Split('|');
                foreach (DataRow dr in dt.Rows)
                {
                    //string[] vals = idAndSecrets.Split(',');
                    int id = int.Parse(dr["RecID"].ToString());
                    string name = dr["Name"].ToString();
                    string appId = dr["AppID"].ToString();
                    string appSecrets = dr["AppSecret"].ToString();
                    temp.Add(new Tuple<int, string, string, string>(id, name, appId, appSecrets));
                }
            }
            return temp;
        }

        //private static string WeixinAppId = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("WeixinAppId", true);
        //private static string WeixinAppSecret = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("WeixinAppSecret", true);

        public WeChatDataService()
        {
            IdAndSecrets = GetWeChatIdAndSecrets();
            timer = new System.Timers.Timer(_serviceInterval * 1000) { AutoReset = true };
            timer.Elapsed += (sender, eventArgs) => Run(sender, eventArgs);
        }

        public void Strat()
        {
            XYAuto.CTUtils.Log.Log4NetHelper.Default().Info("统计微信公众号数据-----服务开始");
            timer.Start();
            //Run(null,null);
        }

        public void Stop()
        {
            XYAuto.CTUtils.Log.Log4NetHelper.Default().Info("统计微信公众号数据-----服务结束");
            timer.Stop();
        }


        public void Run(object obj, ElapsedEventArgs e)
        {
            XYAuto.CTUtils.Log.Log4NetHelper.Default().Info("统计微信公众号数据-----Run......");

            string currentTime = e.SignalTime.Hour.ToString("00") + ":" + e.SignalTime.Minute.ToString("00");
            XYAuto.CTUtils.Log.Log4NetHelper.Default().Info(string.Format($"统计微信公众号数据-----CurrentTime:{currentTime}"));
            if (currentTime == _operateTimer)
                StaticDataForDay();

        }

        private void StaticDataForDay()
        {
            if (IdAndSecrets != null && IdAndSecrets.Count > 0)
            {
                foreach (var idAndSecret in IdAndSecrets)
                {
                    string appId = idAndSecret.Item3;
                    string secrets = idAndSecret.Item4;
                    AccessTokenContainer.Register(appId, secrets);
                }
            }
            Dictionary<string, DataTable> dic = new Dictionary<string, DataTable>();
            foreach (var idAndSecret in IdAndSecrets)
            {
                string AppName = idAndSecret.Item2;
                string WeixinAppId = idAndSecret.Item3;
                int userTypeId = idAndSecret.Item1;
                //string secrets = idAndSecret.Item3;

                List<string> openIds = new List<string>();

                WeChatUser.UserService.Instance.GetOpenIdsByAppId(WeixinAppId, string.Empty, ref openIds);

                //XYAuto.CTUtils.Log.Log4NetHelper.Default().Info("获取OpenIds列表数据为：\r\n" + JsonConvert.SerializeObject(openIds));

                DataTable dt = WeChatUser.UserService.Instance.GetUserInfoByOpenIds(WeixinAppId, openIds, userTypeId);

                if (dt != null && dt.Rows.Count > 0)
                {
                    dic.Add($"{AppName}微信服务号-粉丝明细数据", dt);
                    XYAuto.CTUtils.Log.Log4NetHelper.Default().Info($"{AppName}微信服务号-粉丝明细数据-----方法结束");
                    //更新表LE_WeiXinUser，字段UserType内容
                    XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeixinInfo.Instance.UpdateUserType(openIds, userTypeId);
                }
            }

            if (dic.Count > 0)
            {
                string FileName = $"微信服务号-粉丝明细数据" + DateTime.Now.ToString("yyyy-MM-dd") + ".xls";
                string excelName = "";
                if (NpoiExcel.ExportToExcel(dic, FileName, out excelName))
                {
                    string[] userEmail = ConfigurationManager.AppSettings["ReceiveErrorEmail"].Split(';');
                    if (userEmail != null && userEmail.Length > 0)
                    {
                        if (File.Exists(excelName))
                        {
                            ITSC.Chitunion2017.BLL.EmailHelper.Instance.SendWechatReportDataMail(userEmail, excelName);
                            File.Delete(excelName);
                        }
                        else
                        {
                            XYAuto.CTUtils.Log.Log4NetHelper.Default().Info("微信服务号-粉丝明细数据-查无数据");
                        }
                    }
                }
                else
                {
                    XYAuto.CTUtils.Log.Log4NetHelper.Default().Info("StaticDataForDay:统计微信数据插入excel失败");
                }
            }


        }
    }
}
