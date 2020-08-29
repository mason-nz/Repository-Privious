using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Statistics
{
    //机洗数据统计（天汇总）
    public class StatAutoStatistics
    {
        //主键ID
        public int RecId { get; set; }

        //抓取渠道ID
        public int ChannelId { get; set; }

        //抓取渠道名称
        public string ChannelName { get; set; }

        //头腰类型（枚举）
        public int ArticleType { get; set; }

        //文章数据
        public int ArticleCount { get; set; }

        //账号数量
        public int AccountCount { get; set; }

        //入库文章量（头部）
        public int StorageArticleCount { get; set; }

        //入库账号量（头部）
        public int StorageAccountCount { get; set; }

        //统计日期
        public DateTime Date { get; set; }

        //状态（默认：0）
        public int Status { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }
    }
}