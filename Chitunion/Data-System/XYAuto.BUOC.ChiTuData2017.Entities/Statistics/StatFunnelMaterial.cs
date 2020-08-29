using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Statistics
{
    //漏斗物料
    public class StatFunnelMaterial
    {
        //主键ID
        public int RecId { get; set; }

        //渠道ID
        public int ChannelId { get; set; }

        //渠道名称
        public string ChannelName { get; set; }

        //场景ID
        public int SceneId { get; set; }

        //场景名称
        public string SceneName { get; set; }

        //头部文章账号类型（枚举）
        public int AaScoreType { get; set; }

        //封装
        public int Encapsulate { get; set; }

        //分发
        public int Distribute { get; set; }

        //转发
        public int Forward { get; set; }

        //线索
        public int Clue { get; set; }

        //统计开始时间
        public DateTime BeginTime { get; set; }

        //统计结束时间
        public DateTime EndTime { get; set; }

        //状态（默认：0）
        public int Status { get; set; }

        public DateTime CreateTime { get; set; }
    }
}