using System;
using System.Configuration;

namespace XYAuto.ChiTu2018.Service.ThirdApi.Config
{
    /// <summary>
    /// 注释：WithdrawalsConfigSection
    /// 作者：lix
    /// 日期：2018/5/23 17:21:18
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class AppPostThirdApiConfigSection : ConfigurationSection
    {
        public static string SectionName => "AppPostThirdApiConfig";

        /// <summary>
        /// 提现申请url
        /// </summary>
        [ConfigurationProperty("PostWithdrawalsUrl", DefaultValue = "", IsRequired = false)]
        public string PostWithdrawalsUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace((string)this["PostWithdrawalsUrl"]))
                    throw new Exception(string.Format("{0}配置文件中PostWithdrawalsUrl字段属于必须字段，请添加对应的内容", SectionName));
                return (string)this["PostWithdrawalsUrl"];
            }
            set { this["PostWithdrawalsUrl"] = value; }
        }


        /// <summary>
        /// 提现申请-VerifyWithdrawalsClick
        /// </summary>
        [ConfigurationProperty("PostVerifyWithdrawalsClick", DefaultValue = "", IsRequired = false)]
        public string PostVerifyWithdrawalsClickUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace((string)this["PostVerifyWithdrawalsClickUrl"]))
                    throw new Exception(string.Format("{0}配置文件中PostVerifyWithdrawalsClickUrl字段属于必须字段，请添加对应的内容", SectionName));
                return (string)this["PostVerifyWithdrawalsClickUrl"];
            }
            set { this["PostVerifyWithdrawalsClickUrl"] = value; }
        }

        /// <summary>
        /// 提现申请-PostWithdrawalsPriceCalcUrl 个税计算
        /// </summary>
        [ConfigurationProperty("PostWithdrawalsPriceCalcUrl", DefaultValue = "", IsRequired = false)]
        public string PostWithdrawalsPriceCalcUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace((string)this["PostWithdrawalsPriceCalcUrl"]))
                    throw new Exception(string.Format("{0}配置文件中PostWithdrawalsPriceCalcUrl字段属于必须字段，请添加对应的内容", SectionName));
                return (string)this["PostWithdrawalsPriceCalcUrl"];
            }
            set { this["PostWithdrawalsPriceCalcUrl"] = value; }
        }

        /// <summary>
        /// 微信用户授权-PostWxUserOperationUrl 用户操作相关
        /// </summary>
        [ConfigurationProperty("PostWxUserOperationUrl", DefaultValue = "", IsRequired = false)]
        public string PostWxUserOperationUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace((string)this["PostWxUserOperationUrl"]))
                    throw new Exception(string.Format("{0}配置文件中PostWxUserOperationUrl字段属于必须字段，请添加对应的内容", SectionName));
                return (string)this["PostWxUserOperationUrl"];
            }
            set { this["PostWxUserOperationUrl"] = value; }
        }
    }
}
