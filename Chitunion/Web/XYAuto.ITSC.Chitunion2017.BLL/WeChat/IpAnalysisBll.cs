/********************************
* 项目名称 ：XYAuto.ITSC.Chitunion2017.BLL.WeChat
* 类 名 称 ：IpAnalysisBll
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/25 10:05:07
********************************/
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Dal.APP;
using XYAuto.ITSC.Chitunion2017.Entities.APP;
using XYAuto.ITSC.Chitunion2017.WebService.IP;

namespace XYAuto.ITSC.Chitunion2017.BLL.WeChat
{
    public class IpAnalysisBll
    {
        #region 单例
        private IpAnalysisBll() { }

        public static IpAnalysisBll instance = null;
        public static readonly object padlock = new object();

        public static IpAnalysisBll Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new IpAnalysisBll();
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
                string json = new IpAnalysisHelper().GetAreaByIp(ip);
                var jObject = JObject.Parse(json);
                if (jObject["result"] + string.Empty == "1")
                {
                    var entity = JsonConvert.DeserializeObject<IpRequestLogModel>(jObject["data"] + string.Empty);
                    entity.IP = ip;
                    entity.Url = url;
                    IpManageDal.Instance.AddIpRequestLog(entity);
                }
                else
                {
                    Loger.Log4Net.Info($"解析IP：{ip} 失败  原因：{jObject["msg"]}");
                }

            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error($"解析IP：{ip} 报错", ex);
            }

        }
    }
}
