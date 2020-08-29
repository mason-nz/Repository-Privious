/********************************************************
*创建人：lixiong
*创建时间：2017/8/14 14:59:25
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Enum;

namespace XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Request
{
    public class ReqFundDto
    {
        public int AccountId { get; set; }
        public GdtFundTypeEnum FundType { get; set; }

        public GdtTradeTypeEnum TradeType { get; set; } = GdtTradeTypeEnum.None;

        /// <summary>
        /// 2017-04-24
        /// </summary>
        public string Date { get; set; }
    }

    public class ReqFundDetaileDto : PageInfo
    {
        public int AccountId { get; set; }
        public GdtFundTypeEnum FundType { get; set; }
        public DateRangeDto DateRange { get; set; }
    }

    public class DateRangeDto
    {
        [JsonProperty("start_date")]
        public string StartDate { get; set; }

        [JsonProperty("end_date")]
        public string EndDate { get; set; }
    }

    public class PageInfo
    {
        [JsonProperty("page")]
        public int Page { get; set; } = 1;

        [JsonProperty("page_size")]
        public int PageSize { get; set; } = 20;

        [JsonProperty("total_number")]
        public int TotalNumber { get; set; }

        [JsonProperty("total_page")]
        public int TotalPage { get; set; }
    }
}