/********************************************************
*创建人：lixiong
*创建时间：2017/10/27 20:17:43
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.ExternalApi.Config
{
    public class OpApiNoteDistributeConfigSection : ConfigurationSection
    {
        public static string SectionName => "OpApiNoteDistributeConfig";

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