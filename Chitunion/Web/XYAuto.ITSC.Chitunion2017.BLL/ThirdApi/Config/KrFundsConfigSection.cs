using System;
using System.Configuration;

namespace XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Config
{
    public class KrFundsConfigSection : ConfigurationSection
    {
        public static string SectionName => "KrFundsConfig";



        [ConfigurationProperty("AppId", DefaultValue = "", IsRequired = true)]
        public string AppId
        {
            get
            {
                if (string.IsNullOrWhiteSpace((string)this["AppId"]))
                    throw new Exception(string.Format("{0}配置文件中AppId字段属于必须字段，请添加对应的内容", SectionName));
                return (string)this["AppId"];
            }
            set { this["AppId"] = value; }
        }

        /// <summary>
        /// 业务类型
        /// </summary>
        [ConfigurationProperty("TradeClassificationCode", DefaultValue = "", IsRequired = true)]
        public string TradeClassificationCode
        {
            get
            {
                if (string.IsNullOrWhiteSpace((string)this["TradeClassificationCode"]))
                    throw new Exception(string.Format("{0}配置文件中TradeClassificationCode字段属于必须字段，请添加对应的内容", SectionName));
                return (string)this["TradeClassificationCode"];
            }
            set { this["TradeClassificationCode"] = value; }
        }


        [ConfigurationProperty("IsTestPay", DefaultValue = "true", IsRequired = false)]
        public bool IsTestPay
        {
            get
            {
                return bool.Parse(this["IsTestPay"].ToString());
            }
            set { this["IsTestPay"] = value; }
        }

        [ConfigurationProperty("IsTestPayCallBack", DefaultValue = "false", IsRequired = false)]
        public bool IsTestPayCallBack
        {
            get
            {
                return bool.Parse(this["IsTestPayCallBack"].ToString());
            }
            set { this["IsTestPayCallBack"] = value; }
        }

        [ConfigurationProperty("PayRemark", DefaultValue = "赤兔联盟分发收益", IsRequired = false)]
        public string PayRemark
        {
            get { return this["PayRemark"].ToString(); }
            set { this["PayRemark"] = value; }
        }

        /// <summary>
        /// 同步支付返回错误测试
        /// </summary>
        [ConfigurationProperty("IsTestPaySync", DefaultValue = "false", IsRequired = false)]
        public bool IsTestPaySync
        {
            get
            {
                return bool.Parse(this["IsTestPaySync"].ToString());
    }

            set { this["IsTestPaySync"] = value; }
        }

    }
}
