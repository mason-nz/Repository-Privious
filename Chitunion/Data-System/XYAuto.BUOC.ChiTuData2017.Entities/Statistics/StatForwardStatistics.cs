using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Statistics
{
    //转发数据统计（天汇总）
    public class StatForwardStatistics
    {
        //主键ID
        public int RecId { get; set; }

        //分发渠道ID
        public int ChannelId { get; set; }

        //分发渠道名称
        public string ChannelName { get; set; }

        //物料转发数量
        public int MaterialForwardCount { get; set; }

        //统计时间
        public int Date { get; set; }

        //状态（默认：0）
        public int Status { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }
    }
}