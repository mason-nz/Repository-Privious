using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.ISDC.CC2012.WebService.CC
{
    public class DictionaryDataCacheHelper
    {
        public static DictionaryDataCacheHelper Instance = new DictionaryDataCacheHelper();

        private string key = "";
        private string url = "";

        public DictionaryDataCacheHelper()
        {
            try
            {
                key = System.Configuration.ConfigurationManager.AppSettings["DictionaryDataCache_KEY"].ToString();//验证码
                url = System.Configuration.ConfigurationManager.AppSettings["DictionaryDataCache_URL"].ToString();//服务URL
            }
            catch
            {
            }
        }

        /// 重置缓存数据
        /// <summary>
        /// 重置缓存数据
        /// </summary>
        /// <returns></returns>
        public bool ResetDictionaryDataCache()
        {
            if (url == "" || key == "")
            {
                throw new Exception("DictionaryDataCache_KEY和DictionaryDataCache_URL配置不正确");
            }

            DictionaryDataCacheService.DictionaryDataCacheService server = new DictionaryDataCacheService.DictionaryDataCacheService();
            server.Url = url;
            return server.ResetDictionaryDataCache(key);
        }
    }
}
