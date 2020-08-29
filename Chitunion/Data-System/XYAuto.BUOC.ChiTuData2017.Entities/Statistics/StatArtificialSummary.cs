using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Statistics
{
    //人工清洗数据汇总（时间周期汇总）
    public class StatArtificialSummary
    {
        //主键ID
        public int RecId { get; set; }

        //渠道ID
        public int ChannelId { get; set; }

        //渠道名称
        public string ChannelName { get; set; }

        //账号场景ID
        public int SceneId { get; set; }

        //账号场景名称
        public string SceneName { get; set; }

        //文章账号分值类型（枚举）只统计头部文章的数据
        public int AaScoreType { get; set; }

        //头腰类型（枚举）
        public int ArticleType { get; set; }

        //状态ID
        public int ConditionId { get; set; }

        //状态名称（可用、作废、置为腰）
        public string ConditionName { get; set; }

        //文章数量
        public int ArticleCount { get; set; }

        //账号数量
        public int AccountCount { get; set; }

        //统计开始时间
        public DateTime BeginTime { get; set; }

        //统计结束时间
        public DateTime EndTime { get; set; }

        //状态（默认：0）
        public int Status { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }

        public string Category { get; set; }
        public string AAScoreTypeName { get; set; }
    }
}