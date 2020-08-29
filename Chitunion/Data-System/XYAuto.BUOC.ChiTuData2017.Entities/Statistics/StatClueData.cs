using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Statistics
{
    //线索数据明细
    public class StatClueData
    {
        //主键ID
        public int RecId { get; set; }

        //物料ID
        public int MaterialId { get; set; }

        //文章标题
        public string Title { get; set; }

        //落地页URL
        public string Url { get; set; }

        //物料类型
        public int MaterialType { get; set; }

        //物料类型名称
        public string MaterialName { get; set; }

        //分发渠道ID
        public int ChannelId { get; set; }

        //分发渠道名称
        public string ChannelName { get; set; }

        //场景ID
        public int SceneId { get; set; }

        //场景名称
        public string SceneName { get; set; }

        //账号分值（头部）
        public decimal AccountScore { get; set; }

        //文章分值（头部）
        public decimal ArticleScore { get; set; }

        //状态（默认：0）
        public int Status { get; set; }

        //询价数
        public int InqueryCount { get; set; }

        //会话数
        public int SessionCount { get; set; }

        //电话接通数
        public int TelConnectCount { get; set; }

        //线索生成时间
        public DateTime ClueTime { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }
    }
}