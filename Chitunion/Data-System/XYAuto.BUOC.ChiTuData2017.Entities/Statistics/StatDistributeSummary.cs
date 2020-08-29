using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Statistics
{
    // 分发数据汇总（时间周期汇总）
    public class StatDistributeSummary
    {
        //主键ID
        public int RecId { get; set; }

        //分发渠道ID（枚举）
        public int ChannelId { get; set; }

        //分发渠道名称
        public string ChannelName { get; set; }

        //物料封装类型（枚举）
        public int MaterielTypeId { get; set; }

        //物料封装类型名称
        public string MaterialName { get; set; }

        //物料场景ID
        public int SceneId { get; set; }

        //物料场景名称
        public string SceneName { get; set; }

        //头部文章账号分值类型（枚举）
        public int AaScoreType { get; set; }

        //分发量
        public int DistributeCount { get; set; }

        //物料量
        public int MaterielCount { get; set; }

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