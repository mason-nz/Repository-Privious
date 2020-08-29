using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Statistics
{
    //机洗数据汇总（时间周期汇总）
    public class StatAutoSummary
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

        //文章账号分值类型（枚举）
        public int AaScoreType { get; set; }

        //头腰类型（枚举）
        public int ArticleType { get; set; }

        //抓取文章数量
        public int ArticleCount { get; set; }

        //抓取文章的账号数量
        public int AccountCount { get; set; }

        //入库文章量（头部）
        public int StorageArticleCount { get; set; }

        //入库账号量（头部）
        public int StorageAccountCount { get; set; }

        /// <summary>
        /// 腰部文章类别
        /// </summary>
        public string Category { get; set; }

        //统计开始时间
        public DateTime BeginTime { get; set; }

        //统计结束时间
        public DateTime EndTime { get; set; }

        //状态（默认：0）
        public int Status { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }

        /*冗余*/
        public int TypeId { get; set; }
        public string AAScoreTypeName { get; set; }
    }
}