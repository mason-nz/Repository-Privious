using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Statistics
{
    //线索统计汇总（时间周期汇总）
    public class StatClueSummary
    {
        //主键ID
        public int RecId { get; set; }

        //分发渠道ID
        public int ChannelId { get; set; }

        //分发渠道名称
        public string ChannelName { get; set; }

        //账号场景ID
        public int SceneId { get; set; }

        //账号场景名称
        public string SceneName { get; set; }

        //头部文章账号类型（枚举）
        public int AaScoreType { get; set; }

        //线索类型ID（枚举）
        public int ClueTypeId { get; set; }

        //线索类型名称
        public string ClueTypeName { get; set; }

        //物料封装类型（枚举）
        public int MaterielTypeId { get; set; }

        //物料类型名称
        public string MaterialName { get; set; }

        //线索数量
        public int ClueCount { get; set; }

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