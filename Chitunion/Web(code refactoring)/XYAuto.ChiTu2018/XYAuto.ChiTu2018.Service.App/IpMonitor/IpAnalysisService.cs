/********************************
* 项目名称 ：XYAuto.ChiTu2018.Service.App.IpMonitor
* 类 名 称 ：IpAnalysisService
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/6/5 10:38:47
********************************/
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO.LE;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Service.App.Dto.IpMonitor;
using XYAuto.CTUtils.Html;
using XYAuto.CTUtils.Log;

namespace XYAuto.ChiTu2018.Service.App.IpMonitor
{
    public class IpAnalysisService
    {
        #region 单例
        private IpAnalysisService() { }

        public static IpAnalysisService instance = null;
        public static readonly object padlock = new object();

        public static IpAnalysisService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new IpAnalysisService();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        /// <summary>
        /// 根据IP获取区域地址
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public void GetAreaAddressByIp(string ip, string url)
        {
            try
            {
                string json = GetAreaByIp(ip);
                var jObject = JObject.Parse(json);
                if (jObject["result"] + string.Empty == "1")
                {
                    var entity = JsonConvert.DeserializeObject<LE_IP_RequestLog>(jObject["data"] + string.Empty);
                    entity.IP = ip;
                    entity.Url = url;
                    entity.CreateTime = DateTime.Now;
                    var resultInfo = new LeIpRequestLogBo().AddIpRequestLogInfo(entity);
                }
                else
                {
                    Log4NetHelper.Default().Info($"解析IP：{ip} 失败  原因：{jObject["msg"]}");
                }

            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Error($"解析IP：{ip} 报错", ex);
            }

        }
        public string GetAreaByIp(string IP)
        {

            HttpWebResponse hwr = HtmlHelper.HttpGetRequest($"http://api.sys.xingyuanauto.com/ip/GetAreaByIp?ipStr={IP}");

            string msg = HtmlHelper.GetResponseString(hwr);

            return msg;
        }
    }
}
