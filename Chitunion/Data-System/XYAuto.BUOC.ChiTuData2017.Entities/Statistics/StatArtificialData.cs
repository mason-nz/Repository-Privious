using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Statistics
{
    //机洗数据明细
    public class StatArtificialData
    {
        //主键ID
        public int RecId { get; set; }

        //文章ID
        public int ArticleId { get; set; }

        //文章标题
        public string Title { get; set; }

        //文章URL
        public string Url { get; set; }

        //头腰类型（枚举）
        public int ArticleType { get; set; }

        //渠道ID
        public int ChannelId { get; set; }

        //渠道名称
        public string ChannelName { get; set; }

        //文章发布时间
        public DateTime ArticlePublishTime { get; set; }

        //文章抓取时间
        public DateTime ArticleSpiderTime { get; set; }

        //清洗时间
        public DateTime CleanTime { get; set; }

        //场景ID
        public int SceneId { get; set; }

        //场景名称
        public string SceneName { get; set; }

        //账号
        public string AccountName { get; set; }

        //账号分值
        public decimal AccountScore { get; set; }

        //文章分值
        public decimal ArticleScore { get; set; }

        //原因
        public string Reason { get; set; }

        //状态（默认：0）
        public int Status { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }
    }
}