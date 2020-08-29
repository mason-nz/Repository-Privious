/********************************************************
*创建人：lixiong
*创建时间：2017/8/15 17:13:40
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using XYAuto.BUOC.BOP2017.ExternalApi.GDT.Dto.Enum;

namespace XYAuto.BUOC.BOP2017.ExternalApi.GDT.Dto.Request
{
    public class ReqReportDto : PageInfo
    {
        //[JsonConverter(typeof(StringEnumConverter))]
        public GdtLevelTypeEnum Level { get; set; }

        [JsonProperty("system_status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public GdtSystemStatusEnum SystemStatus { get; set; }

        public int AccountId { get; set; } = -2;
        public DateRangeDto DateRange { get; set; }

        /// <summary>
        /// 获取小时报表的时候用
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 广告组 id
        /// </summary>
        public int AdGroupId { get; set; } = -2;

        /// <summary>
        /// 推广计划 id
        /// </summary>
        public int CampaignId { get; set; } = -2;

        /// <summary>
        /// 聚合参数，例：["time","hour"]，
        /// </summary>
        public string[] GroupBy { get; set; }

        /// <summary>
        /// filtering=[{"field":"system_status", "operator": "EQUALS", "values":["AD_STATUS_PENDING"]}, {"field":"adgroup_name", "operator": "CONTAINS", "values": ["广告"]}]
        /// </summary>
        [JsonProperty("filtering")]
        public List<GdtFilteringDto> Filtering { get; set; }
    }

    public class GdtFilteringDto
    {
        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("operator")]
        [JsonConverter(typeof(StringEnumConverter))]
        public GdtOperatorEnum Operator { get; set; }

        [JsonProperty("values")]
        public string[] Values { get; set; }
    }
}