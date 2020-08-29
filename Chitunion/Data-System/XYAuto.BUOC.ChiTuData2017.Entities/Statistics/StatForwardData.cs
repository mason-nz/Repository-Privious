using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Statistics
{
    //转发发数据明细
    public class StatForwardData
    {
        //主键ID
        public int RecId { get; set; }

        //物料ID
        public int MaterialId { get; set; }

        //标题
        public string Title { get; set; }

        //落地页URL
        public string Url { get; set; }

        //分发渠道ID
        public int ChannelId { get; set; }

        //分发渠道名称
        public string ChannelName { get; set; }

        //物料类型ID
        public int MaterialType { get; set; }

        //物料类型名称
        public string MaterialTypeName { get; set; }

        //封装时间
        public DateTime EncapsulateTime { get; set; }

        //分发时间
        public DateTime DistributeTime { get; set; }

        //转发时间
        public DateTime ForwardTime { get; set; }

        //账号分值（头部）
        public decimal AccountScore { get; set; }

        //文章分值（头部）
        public decimal ArticleScore { get; set; }

        //状态（默认：0）
        public int Status { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }
    }
}