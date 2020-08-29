/********************************************************
*创建人：lixiong
*创建时间：2017/8/15 16:52:00
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Request;

namespace XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Response.Report
{
    public class RespPageInfo<T>
    {
        [JsonProperty("list")]
        public T List { get; set; }

        [JsonProperty("page_info")]
        public PageInfo PageInfo { get; set; }
    }

    public class RespReportDto
    {
        [JsonProperty("hour")]
        public int Hour { get; set; }

        /// <summary>
        /// 日期格式，YYYY-mm-dd
        /// </summary>
        [JsonProperty("date")]
        public string Date { get; set; }

        //推广计划 id，当获取广告主维度报表时，该值无意义
        [JsonProperty("campaign_id")]
        public int CampaignId { get; set; }

        //广告组 id，当获取广告主维度、推广计划维度报表时，该值无意义
        [JsonProperty("adgroup_id")]
        public int AdgroupId { get; set; }

        //曝光量
        [JsonProperty("impression")]
        public int Impression { get; set; }

        //点击量
        [JsonProperty("click")]
        public int Click { get; set; }

        //消耗，单位为分
        [JsonProperty("cost")]
        public int Cost { get; set; }

        //APP 下载量
        [JsonProperty("download")]
        public int Download { get; set; }

        //转化量（APP 安装量）
        [JsonProperty("conversion")]
        public int Conversion { get; set; }

        //APP 激活量，仅在广告主回传对应转化数据后有数据
        [JsonProperty("activation")]
        public int Activation { get; set; }

        //APP 付费行为次数，仅在广告主回传对应转化数据后有数据
        [JsonProperty("app_payment_count")]
        public int AppPaymentCount { get; set; }

        //APP 付费总金额，单位为分，仅在广告主回传对应转化数据后有数据
        [JsonProperty("app_payment_amount")]
        public int AppPaymentAmount { get; set; }

        //微信朋友圈赞和评论数，仅微信朋友圈广告有数据
        [JsonProperty("like_or_comment")]
        public int LikeOrComment { get; set; }

        //微信朋友圈图片点击量，仅微信朋友圈广告有数据
        [JsonProperty("image_click")]
        public int ImageClick { get; set; }

        //微信朋友圈关注量，仅微信朋友圈广告有数据
        [JsonProperty("follow")]
        public int Follow { get; set; }

        //微信朋友圈转发量，仅微信朋友圈广告有数据
        [JsonProperty("share")]
        public int Share { get; set; }
    }
}