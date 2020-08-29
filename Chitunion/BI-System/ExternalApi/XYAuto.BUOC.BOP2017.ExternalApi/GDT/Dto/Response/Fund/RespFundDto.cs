/********************************************************
*创建人：lixiong
*创建时间：2017/8/18 16:13:57
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using Newtonsoft.Json;

namespace XYAuto.BUOC.BOP2017.ExternalApi.GDT.Dto.Response.Fund
{
    public class RespFundDto
    {
        //广点通代理商子客户（简称子客）账户id
        [JsonProperty("account_id")]
        public int AccountId { get; set; }

        [JsonProperty("fund_type")]
        //资金账户类型，详见 [资金账户类型定义]
        public string FundType { get; set; }

        [JsonProperty("balance")]
        //余额，单位为分
        public int Balance { get; set; }

        [JsonProperty("fund_status")]
        //资金状态，详见[资金状态]
        public string FundStatus { get; set; }

        [JsonProperty("realtime_cost")]
        //当日花费，单位为分
        public int RealtimeCost { get; set; }
    }
}