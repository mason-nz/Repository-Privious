using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Statistics
{
    //车型匹配统计汇总（时间周期汇总）
    public class StatCarMatchSummary
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

        public string Category { get; set; }

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