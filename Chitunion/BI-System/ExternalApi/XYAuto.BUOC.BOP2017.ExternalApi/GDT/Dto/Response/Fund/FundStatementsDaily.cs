/********************************************************
*创建人：lixiong
*创建时间：2017/8/14 15:28:29
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using Newtonsoft.Json;

namespace XYAuto.BUOC.BOP2017.ExternalApi.GDT.Dto.Response.Fund
{
    /// <summary>
    /// 获取资金账户日结明细
    /// </summary>
    public class FundStatementsDailyDto
    {
        /// <summary>
        /// 资金账户类型
        /// </summary>
        [JsonProperty("fund_type")]
        public string FundType { get; set; }

        /// <summary>
        /// 交易类型
        /// </summary>
        [JsonProperty("trade_type")]
        public string TradeType { get; set; }

        /// <summary>
        /// 查询日期，日期格式：YYYY-mm-dd，只支持今天或昨天的数据查询
        /// </summary>
        [JsonProperty("date")]
        public string Date { get; set; }

        /// <summary>
        /// 交易时间（时间戳）
        /// </summary>
        [JsonProperty("time")]
        public int Time { get; set; }

        /// <summary>
        /// 金额，单位为分
        /// </summary>
        [JsonProperty("amount")]
        public int Amount { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// 调用方订单号，需要有调用方标示前缀，须要保证在同一个广告主下唯一，不支持大写字母
        /// </summary>
        [JsonProperty("external_bill_no")]
        public string ExternalBillNo { get; set; }
    }
}