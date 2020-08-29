using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;

namespace XYAuto.ITSC.Chitunion2017.BLL.Recommend
{
    public class FilterHomeMediaCountConfig : ConfigurationSection
    {
        public static string SectionName
        {
            get { return "FilterHomeMediaCountConfig"; }
        }

        [ConfigurationProperty("FilterAppPublishCount", DefaultValue = "5", IsRequired = true)]
        public int FilterAppPublishCount
        {
            get { return this["FilterAppPublishCount"].ToString().ToInt(5); }
            set { this["FilterAppPublishCount"] = value; }
        }

        [ConfigurationProperty("FilterWeiXinPublishCount", DefaultValue = "5", IsRequired = true)]
        public int FilterWeiXinPublishCount
        {
            get { return this["FilterWeiXinPublishCount"].ToString().ToInt(5); }
            set { this["FilterWeiXinPublishCount"] = value; }
        }

        [ConfigurationProperty("FilterWeiBoPublishCount", DefaultValue = "5", IsRequired = true)]
        public int FilterWeiBoPublishCount
        {
            get { return this["FilterWeiBoPublishCount"].ToString().ToInt(5); }
            set { this["FilterWeiBoPublishCount"] = value; }
        }

        [ConfigurationProperty("FilterVideoPublishCount", DefaultValue = "5", IsRequired = true)]
        public int FilterVideoPublishCount
        {
            get { return this["FilterVideoPublishCount"].ToString().ToInt(5); }
            set { this["FilterVideoPublishCount"] = value; }
        }

        [ConfigurationProperty("FilterBroadcastPublishCount", DefaultValue = "5", IsRequired = true)]
        public int FilterBroadcastPublishCount
        {
            get { return this["FilterBroadcastPublishCount"].ToString().ToInt(5); }
            set { this["FilterBroadcastPublishCount"] = value; }
        }
    }
}