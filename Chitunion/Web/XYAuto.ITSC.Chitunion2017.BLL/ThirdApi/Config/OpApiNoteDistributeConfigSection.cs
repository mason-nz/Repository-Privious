using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Config
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
