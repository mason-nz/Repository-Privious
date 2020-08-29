/********************************************************
*创建人：lixiong
*创建时间：2017/8/18 14:43:08
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using Newtonsoft.Json;

namespace XYAuto.BUOC.BOP2017.ExternalApi.GDT.Dto.Response.Fund
{
    public class RespRealtimeCostDto
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

        /// <summary>
        /// 实时消耗，单位分
        /// </summary>
        [JsonProperty("cost")]
        public int Cost { get; set; }
    }
}