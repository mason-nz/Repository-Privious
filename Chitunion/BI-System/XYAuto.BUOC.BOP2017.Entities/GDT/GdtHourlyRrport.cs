using System;

namespace XYAuto.BUOC.BOP2017.Entities.GDT
{
    //小时报表
    public class GdtHourlyRrport
    {
        //自增Id
        public int Id { get; set; }

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

        //APP 激活量，仅在广告主回传对应转化数据后有数据
        public int Activation { get; set; }

        //APP 付费行为次数，仅在广告主回传对应转化数据后有数据
        public int AppPaymentCount { get; set; }

        //APP 付费总金额，单位为分，仅在广告主回传对应转化数据后有数据
        public int AppPaymentAmount { get; set; }

        //微信朋友圈赞和评论数，仅微信朋友圈广告有数据
        public int LikeOrComment { get; set; }

        //微信朋友圈图片点击量，仅微信朋友圈广告有数据
        public int ImageClick { get; set; }

        //微信朋友圈关注量，仅微信朋友圈广告有数据
        public int Follow { get; set; }

        //微信朋友圈转发量，仅微信朋友圈广告有数据
        public int Share { get; set; }

        //属于哪天的小时报销，日期格式，YYYY-mm-dd
        public DateTime Date { get; set; }

        //拉取时间
        public DateTime PullTime { get; set; }

        //订单量
        public int OrderQuantity { get; set; }

        //话单量
        public int BillOfQuantities { get; set; }

        public int PV { get; set; }

        public int UV { get; set; }

        public int ClueCount { get; set; }

        public decimal CluePrice { get; set; }
    }
}