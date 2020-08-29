using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Statistics
{
    //线索统计（天汇总）
    public class StatClueStatistics
    {
        //主键ID
        public int RecId { get; set; }

        //分发渠道ID
        public int ChannelId { get; set; }

        //分发渠道名称
        public string ChannelName { get; set; }

        //线索类型ID（枚举）
        public int ClueTypeId { get; set; }

        //线索类型名称
        public string ClueTypeName { get; set; }

        //线索数量
        public int ClueCount { get; set; }

        //统计时间
        public DateTime Date { get; set; }

        //状态（默认：0）
        public int Status { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }
    }
}