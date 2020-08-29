/********************************************************
*创建人：lixiong
*创建时间：2017/8/18 14:22:34
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
    public class RespAdCampaignDto
    {
        [JsonProperty("campaign_id")]
        //赤兔用户id
        public int CampaignId { get; set; }

        [JsonProperty("campaign_name")]
        //推广计划名称，不可与名下其他推广计划重名
        public string CampaignName { get; set; }

        [JsonProperty("configured_status")]
        //客户设置的状态详见 [推广计划类型]
        public string ConfiguredStatus { get; set; }

        [JsonProperty("campaign_type")]
        public string CampaignType { get; set; }

        [JsonProperty("product_type")]
        //标的物类型，详见 [标的物类型]
        public string ProductType { get; set; }

        [JsonProperty("daily_budget")]
        //日消耗限额，单位为分，详见[日限额修改规则]
        public int DailyBudget { get; set; }

        [JsonProperty("budget_reach_date")]
        //日限额到达日期，值为最近一次触达日限额的日期，如无触达记录则为空，如：20170501，表示最近一次到达日限额的日期是 5 月 1 日，若今天是 5 月 1 日，则表示今天已经到达日限额
        public int BudgetReachDate { get; set; }

        [JsonProperty("created_time")]

        //创建时间（时间戳）
        public int CreatedTime { get; set; }

        [JsonProperty("last_modified_time")]
        //最后修改时间（时间戳）
        public int LastModifiedTime { get; set; }

        //拉取时间
        public DateTime PullTime { get; set; } = DateTime.Now;
    }
}