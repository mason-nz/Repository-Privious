/********************************************************
*创建人：lixiong
*创建时间：2017/8/24 14:20:28
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;

namespace XYAuto.BUOC.BOP2017.Entities.GDT
{
    public class GdtHourlyRrportForZhy
    {
        //自增Id
        public int Id { get; set; }

        public int DemandBillNo { get; set; }

        //广点通代理商子客户（简称子客）账户id
        public int AccountId { get; set; }

        //获取日报表类型级别，可选值：ADVERTISER, CAMPAIGN, ADGROUP
        public int Level { get; set; }

        //小时(0-23)
        public int Hour { get; set; }

        //推广计划 id，当获取广告主维度报表时，该值无意义
        public int CampaignId { get; set; }

        //广告组 id，当获取广告主维度、推广计划维度报表时，该值无意义
        public int AdgroupId { get; set; }

        //曝光量
        public int Impression { get; set; }

        //点击量
        public int Click { get; set; }

        //消耗，单位为分
        public int Cost { get; set; }

        //APP 下载量
        public int Download { get; set; }

        //转化量（APP 安装量）
        public int Conversion { get; set; }

        //属于哪天的小时报销，日期格式，YYYY-mm-dd
        public DateTime Date { get; set; }

        //拉取时间
        public DateTime PullTime { get; set; }

        //订单量
        public int OrderQuantity { get; set; }

        //话单量
        public int BillOfQuantities { get; set; }
    }
}