/********************************************************
*创建人：lixiong
*创建时间：2017/9/11 19:20:08
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Boolean;

namespace XYAuto.BUOC.ChiTuData2017.ExternalApi.Config
{
    public class ZhyApiConfigSection : ConfigurationSection
    {
        public static string SectionName => "ZhyApiConfig";

        [ConfigurationProperty("AppKey", DefaultValue = "", IsRequired = true)]
        public string AppKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace((string)this["AppKey"]))
                    throw new Exception(string.Format("{0}配置文件中AppKey字段属于必须字段，请添加对应的内容", SectionName));
                return (string)this["AppKey"];
            }
            set { this["AppKey"] = value; }
        }

        [ConfigurationProperty("Secret", DefaultValue = "", IsRequired = true)]
        public string Secret
        {
            get
            {
                if (string.IsNullOrWhiteSpace((string)this["Secret"]))
                    throw new Exception(string.Format("{0}配置文件中Secret字段属于必须字段，请添加对应的内容", SectionName));
                return (string)this["Secret"];
            }
            set { this["Secret"] = value; }
        }

        [ConfigurationProperty("SignOn", DefaultValue = "true", IsRequired = false)]
        public bool SignOn
        {
            get
            {
                return Parse(this["SignOn"].ToString());
            }
            set { this["SignOn"] = value; }
        }

        [ConfigurationProperty("ApiUrl", DefaultValue = "", IsRequired = true)]
        public string ApiUrl
        {
            get
            {
                return (string)this["ApiUrl"];
            }
            set { this["ApiUrl"] = value; }
        }
    }
}