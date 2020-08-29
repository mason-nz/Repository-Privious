using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Statistics
{
    //漏斗腰部
    public class StatFunnelWaist
    {
        //主键ID
        public int RecId { get; set; }

        //渠道ID
        public int ChannelId { get; set; }

        //渠道名称
        public string ChannelName { get; set; }

        //文章类别ID
        public int ArticleType { get; set; }

        //文章类别名称
        public string ArticleTypeName { get; set; }

        //抓取文章数量
        public int SpiderCount { get; set; }

        //机洗保留文章数量
        public int AutoCleanCount { get; set; }

        //匹配车型文章数量
        public int MatchedCount { get; set; }

        //人工清洗文章保留数量
        public int ArtificialCount { get; set; }

        //封装使用文章数量
        public int EncapsulateCount { get; set; }

        //统计开始时间
        public DateTime BeginTime { get; set; }

        //统计结束时间
        public DateTime EndTime { get; set; }

        //状态（默认：0）
        public int Status { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }
    }
}