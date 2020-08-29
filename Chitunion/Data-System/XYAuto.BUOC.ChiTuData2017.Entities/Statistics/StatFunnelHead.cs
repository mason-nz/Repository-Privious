using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Statistics
{
    //漏斗头部分析
    public class StatFunnelHead
    {
        //主键ID
        public int RecId { get; set; }

        //场景ID
        public int SceneId { get; set; }

        //场景名称
        public string SceneName { get; set; }

        //渠道ID
        public int ChannelId { get; set; }

        //渠道名称
        public string ChannelName { get; set; }

        //头部文章账号类型（枚举）
        public int AaScoreType { get; set; }

        //抓取文章量
        public int SpiderArticleCount { get; set; }

        //抓取账号量
        public int SpiderAccountCount { get; set; }

        //机洗文章数量
        public int AutoArticleCount { get; set; }

        //机洗账号数量
        public int AutoAccountCount { get; set; }

        //初筛保留文章
        public int PrimaryArticleCount { get; set; }

        //初筛保留账号
        public int PrimaryAccountCount { get; set; }

        //清洗保留文章
        public int ArtificialArticleCount { get; set; }

        //清洗保留账号
        public int ArtificialAccountCount { get; set; }

        //封装使用文章
        public int EncapsulateArticleCount { get; set; }

        //封装使用账号
        public int EncapsulateAccountCount { get; set; }

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