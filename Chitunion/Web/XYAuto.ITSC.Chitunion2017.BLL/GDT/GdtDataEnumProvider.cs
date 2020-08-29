/********************************************************
*创建人：lixiong
*创建时间：2017/8/21 10:09:38
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Support;

namespace XYAuto.ITSC.Chitunion2017.BLL.GDT
{
    public class GdtDataEnumProvider
    {
        /// <summary>
        /// 客户系统状态 | system_status
        /// </summary>
        /// <returns></returns>
        public static int GetDicSystemStatus(string key)
        {
            key = key.ToUpper();

            var dic = CacheHelper<Dictionary<string, int>>.Get(HttpRuntime.Cache,
                        () => $"GetDicSystemStatus", GetDicSystemStatus, null, 30);
            //var dic = GetDicSystemStatus();
            if (dic.ContainsKey(key))
                return dic[key];
            return -2;
        }

        /// <summary>
        /// 客户系统状态 | system_status
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> GetDicSystemStatus()
        {
            return new Dictionary<string, int>()
            {
                {"CUSTOMER_STATUS_NORMAL"  ,80001},
                {"CUSTOMER_STATUS_PENDING"    ,80002},
                {"CUSTOMER_STATUS_DENIED" ,80003},
                {"CUSTOMER_STATUS_FROZEN" ,80004},
                {"CUSTOMER_STATUS_SUSPEND"    ,80005},
                {"CUSTOMER_STATUS_MATERIAL_PREPARED"    ,80006},
                {"CUSTOMER_STATUS_DELETED"    ,80007},
                {"CUSTOMER_STATUS_UNREGISTERED"     ,80008}
            };
        }

        /// <summary>
        /// 资金账户类型 | fund_type
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetDicFundType(string key)
        {
            key = key.ToUpper();

            var dic = CacheHelper<Dictionary<string, int>>.Get(HttpRuntime.Cache,
                        () => $"GetDicFundType", GetDicFundType, null, 30);
            //var dic = GetDicSystemStatus();
            if (dic.ContainsKey(key))
                return dic[key];
            return -2;
        }

        /// <summary>
        /// 资金账户类型 | fund_type
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> GetDicFundType()
        {
            return new Dictionary<string, int>()
            {
                {"GENERAL_CASH"  ,81001},
                {"GENERAL_GIFT"    ,81002},
                {"GENERAL_SHARED" ,81003},
                {"BANK" ,81004},
                {"MYAPP_CHARGE"    ,81005},
                {"MYAPP_CONSUME"    ,81006}
            };
        }

        /// <summary>
        /// 资金状态 | fund_status
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetDicFundStatus(string key)
        {
            key = key.ToUpper();

            var dic = CacheHelper<Dictionary<string, int>>.Get(HttpRuntime.Cache,
                        () => $"GetDicFundStatus", GetDicFundStatus, null, 30);
            //var dic = GetDicSystemStatus();
            if (dic.ContainsKey(key))
                return dic[key];
            return -2;
        }

        /// <summary>
        /// 资金状态 | fund_status
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> GetDicFundStatus()
        {
            return new Dictionary<string, int>()
            {
                {"FUND_STATUS_NORMAL"  ,82001},
                {"FUND_STATUS_NOT_ENOUGH"    ,82002},
                {"FUND_STATUS_FROZEN" ,82003}
            };
        }

        /// <summary>
        /// 交易类型 | trade_type
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetDicTradeType(string key)
        {
            key = key.ToUpper();

            var dic = CacheHelper<Dictionary<string, int>>.Get(HttpRuntime.Cache,
                        () => $"GetDicTradeType", GetDicTradeType, null, 30);
            //var dic = GetDicSystemStatus();
            if (dic.ContainsKey(key))
                return dic[key];
            return -2;
        }

        /// <summary>
        /// 交易类型 | trade_type
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> GetDicTradeType()
        {
            return new Dictionary<string, int>()
            {
                {"CHARGE"  ,84001},
                {"PAY"    ,84002},
                {"TRANSFER_BACK" ,84003},
                {"EXPIRE" ,84004}
            };
        }

        /// <summary>
        /// 推广计划类型 | campaign_type
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetDicCampaignType(string key)
        {
            key = key.ToUpper();

            var dic = CacheHelper<Dictionary<string, int>>.Get(HttpRuntime.Cache,
                        () => $"GetDicCampaignType", GetDicCampaignType, null, 30);
            //var dic = GetDicSystemStatus();
            if (dic.ContainsKey(key))
                return dic[key];
            return -2;
        }

        /// <summary>
        /// 推广计划类型 | campaign_type
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> GetDicCampaignType()
        {
            return new Dictionary<string, int>()
            {
                {"CAMPAIGN_TYPE_NORMAL"  ,85001},
                {"CAMPAIGN_TYPE_WECHAT_OFFICIAL_ACCOUNTS"    ,85002},
                {"CAMPAIGN_TYPE_WECHAT_MOMENTS" ,85003}
            };
        }

        /// <summary>
        /// 客户设置的状态| configured_status
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetDicConfiguredStatus(string key)
        {
            key = key.ToUpper();

            var dic = CacheHelper<Dictionary<string, int>>.Get(HttpRuntime.Cache,
                        () => $"GetDicConfiguredStatus", GetDicConfiguredStatus, null, 30);
            //var dic = GetDicSystemStatus();
            if (dic.ContainsKey(key))
                return dic[key];
            return -2;
        }

        /// <summary>
        /// 客户设置的状态| configured_status
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> GetDicConfiguredStatus()
        {
            return new Dictionary<string, int>()
            {
                {"AD_STATUS_NORMAL"  ,86001},
                {"AD_STATUS_SUSPEND"    ,86002},
            };
        }

        /// <summary>
        /// 推广计划、广告组、广告的系统状态 | system_status
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetDicAdSystemStatus(string key)
        {
            key = key.ToUpper();

            var dic = CacheHelper<Dictionary<string, int>>.Get(HttpRuntime.Cache,
                        () => $"GetDicAdSystemStatus", GetDicAdSystemStatus, null, 30);
            //var dic = GetDicSystemStatus();
            if (dic.ContainsKey(key))
                return dic[key];
            return -2;
        }

        /// <summary>
        /// 推广计划、广告组、广告的系统状态 | system_status
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> GetDicAdSystemStatus()
        {
            return new Dictionary<string, int>()
            {
                {"AD_STATUS_NORMAL"  ,87001},
                {"AD_STATUS_PENDING"    ,87002},
                {"AD_STATUS_DENIED" ,87003},
                {"AD_STATUS_FROZEN" ,87004},
                {"AD_STATUS_PREPARE"    ,87005}
            };
        }

        /// <summary>
        /// 站点集合 | site_set
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetDicSiteSet(string key)
        {
            key = key.ToUpper();

            var dic = CacheHelper<Dictionary<string, int>>.Get(HttpRuntime.Cache,
                        () => $"GetDicSiteSet", GetDicSiteSet, null, 30);
            //var dic = GetDicSystemStatus();
            if (dic.ContainsKey(key))
                return dic[key];
            return -2;
        }

        /// <summary>
        /// 站点集合 | site_set
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> GetDicSiteSet()
        {
            return new Dictionary<string, int>()
            {
                {"SITE_SET_QZONE"  ,88001},
                {"SITE_SET_QQCLIENT"    ,88002},
                {"SITE_SET_MUSIC" ,88003},
                {"SITE_SET_MOBILE_UNION" ,88004},
                {"SITE_SET_QQCOM"    ,88005},
                {"SITE_SET_WECHAT"    ,88005},
                {"SITE_SET_MOBILE_INNER"    ,88005}
            };
        }

        /// <summary>
        /// 优化目标 | optimization_goal
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetDicOptimizationGoal(string key)
        {
            key = key.ToUpper();

            var dic = CacheHelper<Dictionary<string, int>>.Get(HttpRuntime.Cache,
                        () => $"GetDicOptimizationGoal", GetDicOptimizationGoal, null, 30);
            //var dic = GetDicSystemStatus();
            if (dic.ContainsKey(key))
                return dic[key];
            return -2;
        }

        /// <summary>
        /// 优化目标 | optimization_goal
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> GetDicOptimizationGoal()
        {
            return new Dictionary<string, int>()
            {
                {"OPTIMIZATIONGOAL_CLICK"  ,92001},
                {"OPTIMIZATIONGOAL_IMPRESSION"    ,92002},
            };
        }

        /// <summary>
        /// 扣费方式 | billing_event
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetDicBillingEvent(string key)
        {
            key = key.ToUpper();

            var dic = CacheHelper<Dictionary<string, int>>.Get(HttpRuntime.Cache,
                        () => $"GetDicBillingEvent", GetDicBillingEvent, null, 30);
            //var dic = GetDicSystemStatus();
            if (dic.ContainsKey(key))
                return dic[key];
            return -2;
        }

        /// <summary>
        /// 扣费方式 | billing_event
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> GetDicBillingEvent()
        {
            return new Dictionary<string, int>()
            {
                {"BILLINGEVENT_CLICK"  ,93001},
                {"BILLINGEVENT_IMPRESSION"    ,93002},
            };
        }

        /// <summary>
        /// 获取日报表类型级别，可选值：ADVERTISER, CAMPAIGN, ADGROUP
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetDicLevel(string key)
        {
            key = key.ToUpper();

            var dic = CacheHelper<Dictionary<string, int>>.Get(HttpRuntime.Cache,
                        () => $"GetDicLevel", GetDicLevel, null, 30);
            //var dic = GetDicSystemStatus();
            if (dic.ContainsKey(key))
                return dic[key];
            return -2;
        }

        /// <summary>
        /// 获取日报表类型级别，可选值：ADVERTISER, CAMPAIGN, ADGROUP
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> GetDicLevel()
        {
            return new Dictionary<string, int>()
            {
                {"ADVERTISER"  ,83001},
                {"CAMPAIGN"    ,83002},
                {"ADGROUP"    ,83003},
            };
        }
    }
}