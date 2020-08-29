/********************************************************
*创建人：lixiong
*创建时间：2017/8/18 14:07:24
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Response.Ads
{
    public class RespAdGroupDto
    {
        [JsonProperty("account_id")]
        //广点通代理商子客户（简称子客）账户id
        public int AccountId { get; set; }

        [JsonProperty("campaign_id")]
        //推广计划 id
        public int CampaignId { get; set; }

        [JsonProperty("adgroup_id")]
        //广告组 id
        public int AdgroupId { get; set; }

        [JsonProperty("adgroup_name")]
        //广告组名称，同一账户下名称不允许重复
        public string AdgroupName { get; set; }

        [JsonProperty("configured_status")]
        //客户设置的状态，详见[客户设置的状态]
        public string ConfiguredStatus { get; set; }

        [JsonProperty("system_status")]
        //广告在系统中的状态，详见[推广计划、广告组、广告的系统状态]
        public string SystemStatus { get; set; }

        [JsonProperty("reject_message")]
        //审核消息
        public string RejectMessage { get; set; }

        //详见 [投放站点集合]
        public string[] SiteSet { get; set; }

        [JsonProperty("optimization_goal")]
        //广告目标类型，详见 [优化目标]
        public string OptimizationGoal { get; set; }

        [JsonProperty("billing_event")]
        //计费类型，详见 [扣费方式]
        public string BillingEvent { get; set; }

        [JsonProperty("bid_amount")]
        //广告出价，单位为分，详见 [出价规则]
        public int BidAmount { get; set; }

        [JsonProperty("daily_budget")]
        //广告出价，单位为分
        public int DailyBudget { get; set; }

        [JsonProperty("begin_date")]
        //开始投放日期，日期格式，YYYY-mm-dd，且日期小于等于end_date
        public string BeginDate { get; set; }

        [JsonProperty("end_date")]
        //结束投放日期，日期格式，YYYY-mm-dd，大于等于今天，且大于等于 begin_date
        public string EndDate { get; set; }

        [JsonProperty("time_series")]
        //投放时间段，格式为 48 * 7 位字符串，且都为 0 和 1，以半个小时为最小粒度，0 为不投放，1 为投放，不传此字段、全为 0、全为 1 均视为全时段投放
        public string TimeSeries { get; set; }

        [JsonProperty("customized_category")]
        //自定义分类，关键词用半角逗号','分隔，如：本地生活,餐饮
        public string CustomizedCategory { get; set; }

        [JsonProperty("created_time")]
        //创建时间（时间戳）
        public int CreatedTime { get; set; }

        [JsonProperty("last_modified_time")]
        //最后修改时间（时间戳）
        public int LastModifiedTime { get; set; }
    }
}