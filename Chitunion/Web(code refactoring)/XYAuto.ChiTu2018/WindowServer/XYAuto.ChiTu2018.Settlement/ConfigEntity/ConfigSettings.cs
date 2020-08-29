using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XYAuto.ChiTu2018.Settlement.ConfigEntity
{
    /// <summary>
    /// 注释：ConfigSettings
    /// 作者：lix
    /// 日期：2018/5/22 11:13:07
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
   public class ConfigSettings
    {
        public static ConfigEntityBase GetConfig()
        {
            var config = GetSetting("SettlementConfig");
            return JsonConvert.DeserializeObject<ConfigEntityBase>(config);
        }

        private static string GetSetting(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key];
            }
            catch (Exception exception)
            {
                throw new Exception($"XYAuto.ChiTu2018.Settlement AppSettings 请配置相关参数，{exception.Message},{exception.StackTrace ?? string.Empty}");
            }
        }
    }
}
