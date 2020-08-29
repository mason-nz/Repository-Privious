/********************************************************
*创建人：lixiong
*创建时间：2017/10/10 14:05:42
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using XYAuto.BUOC.BOP2017.Infrastruction;

namespace XYAuto.BUOC.BOP2017.BLL.GDT.PullConfigLable
{
    public class PullConfigLableProvider
    {
        private readonly string _lableFileFolder = "PullConfigLable";

        public void SetConfig<T>(PullCategoryEnum pullCategoryEnum, ConfigBaseInfo<T> setValue)
        {
            File.WriteAllText(GetPhysicalPath(pullCategoryEnum), JsonConvert.SerializeObject(setValue), Encoding.UTF8);
        }

        public ConfigBaseInfo<T> GetConfig<T>(PullCategoryEnum pullCategoryEnum)
        {
            var resp = new ConfigBaseInfo<T>();
            try
            {
                var file = GetPhysicalPath(pullCategoryEnum);
                var content = File.ReadAllText(file, Encoding.UTF8);
                if (string.IsNullOrWhiteSpace(content))
                    return resp;
                resp = JsonConvert.DeserializeObject<ConfigBaseInfo<T>>(content);
                return resp;
            }
            catch (Exception exception)
            {
                Loger.Log4Net.Error($"PullConfigLableProvider GetConfig is error:{exception.Message}{exception.StackTrace ?? string.Empty}");
                return resp;
            }
        }

        public string GetPhysicalPath(PullCategoryEnum pullCategoryEnum)
        {
            var pathBase = GetConfigBaseFolder();
            switch (pullCategoryEnum)
            {
                case PullCategoryEnum.GdtAccunt:

                    return pathBase + "/" + GetFileName(PullCategoryEnum.GdtAccunt);

                default:
                    return string.Empty;
            }
        }

        public string GetFileName(PullCategoryEnum pullCategoryEnum)
        {
            switch (pullCategoryEnum)
            {
                case PullCategoryEnum.GdtAccunt:
                    //内容为：ConfigPageInfo
                    return $"config_lable_account.txt";

                default:
                    return string.Empty;
            }
        }

        public string GetConfigBaseFolder()
        {
            var pathBase = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _lableFileFolder);
            if (!Directory.Exists(pathBase))
                Directory.CreateDirectory(pathBase);
            return pathBase;
        }
    }

    public class ConfigBaseInfo<T>
    {
        public T Data { get; set; }

        public ConfigPageInfo ConfigPageInfo { get; set; }
    }

    public class ConfigPageInfo
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 20;
    }
}