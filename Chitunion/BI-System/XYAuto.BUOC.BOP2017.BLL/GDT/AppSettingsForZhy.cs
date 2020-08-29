/********************************************************
*创建人：lixiong
*创建时间：2017/8/17 15:10:52
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Configuration;

namespace XYAuto.BUOC.BOP2017.BLL.GDT
{
    public class AppSettingsForZhy
    {
        public static readonly AppSettingsForZhy Instance = new AppSettingsForZhy();

        public AppSettingsForZhy()
        {
            ZhySignOff = GetSetting("ZhySignOff").Equals("true", StringComparison.OrdinalIgnoreCase);
            ZhyApiUrl = GetSetting("ZhyApiUrl");
            ZhyApiAppKey = GetSetting("ZhyApiAppKey");
            ZhyApiAppSecret = GetSetting("ZhyApiAppSecret");
        }

        public bool ZhySignOff { get; set; }
        public string ZhyApiAppKey { get; set; }
        public string ZhyApiAppSecret { get; set; }
        public string ZhyApiUrl { get; private set; }

        private string GetSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}