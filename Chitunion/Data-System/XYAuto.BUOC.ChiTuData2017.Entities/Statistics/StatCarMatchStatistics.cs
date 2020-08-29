using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Statistics
{
    //车型匹配统计数据（天汇总）
    public class StatCarMatchStatistics
    {
        //主键ID
        public int RecId { get; set; }

        //渠道ID
        public int ChannelId { get; set; }

        //渠道名称
        public string ChannelName { get; set; }

        //腰部文章匹配车型的文章数量
        public int MatchArticleCount { get; set; }

        //腰部文章未匹配车型的文章数量
        public int UnMatchArticleCount { get; set; }

        //统计时间
        public DateTime Date { get; set; }

        //状态（默认：0）
        public int Status { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }
    }
}