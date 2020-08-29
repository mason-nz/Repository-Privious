using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Statistics
{
    //车型匹配明细数据
    public class StatCarMatchData
    {
        //主键ID
        public int RecId { get; set; }

        //文章ID
        public int ArticelId { get; set; }

        //文章标题
        public string Title { get; set; }

        //文章URL
        public string Url { get; set; }

        //渠道ID
        public int ChannelId { get; set; }

        //渠道名称
        public string ChannelName { get; set; }

        //文章发布时间
        public DateTime ArticlePublishTime { get; set; }

        //文章抓取时间
        public DateTime ArticleSpiderTime { get; set; }

        //匹配车型时间
        public DateTime MatchCarTime { get; set; }

        //品牌ID
        public int BrandId { get; set; }

        //品牌名称
        public string BrandName { get; set; }

        //车型ID
        public int SerialId { get; set; }

        //车型名称
        public string SerialName { get; set; }

        //文章分值
        public decimal ArticleSorce { get; set; }

        //匹配状态
        public int MatchStatus { get; set; }

        //匹配状态名称
        public string MatchName { get; set; }

        //状态（默认：0）
        public int Status { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }
    }
}