using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Statistics
{
    //初选数据明细
    public class StatPrimaryData
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

        //是否原创
        public bool IsOriginal { get; set; }

        //渠道ID
        public int ChannelId { get; set; }

        //渠道名称
        public string ChannelName { get; set; }

        //文章发布时间
        public DateTime ArticlePublishTime { get; set; }

        //文章抓取时间
        public DateTime ArticleSpiderTime { get; set; }

        //初选时间
        public DateTime PrimaryTime { get; set; }

        //场景ID
        public int SceneId { get; set; }

        //场景名称
        public string SceneName { get; set; }

        //账号
        public string Account { get; set; }

        //账号分值
        public decimal AccountSorce { get; set; }

        //文章分值
        public decimal ArticleScore { get; set; }

        //初筛状态
        public int ConditionId { get; set; }

        //初筛状态名称
        public string ConditionName { get; set; }

        //作废原因
        public string Reason { get; set; }

        //状态（默认：0）
        public int Status { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }
    }
}