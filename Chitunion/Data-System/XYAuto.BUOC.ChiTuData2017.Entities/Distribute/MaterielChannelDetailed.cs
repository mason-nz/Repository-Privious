using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Distribute
{
    //物料分发-渠道明细表（多个渠道的数据统计）
    public class MaterielChannelDetailed
    {
        //自增id
        public int Id { get; set; }

        //物料ID
        public int MaterielId { get; set; }

        public int DistributeId { get; set; }

        //渠道id
        public int ChannelId { get; set; }

        //渠道名称
        public string ChannelName { get; set; }

        //子渠道名称
        public string ChirldChannelName { get; set; }

        //统计时间（单位天）
        public DateTime Date { get; set; }

        //pv统计
        public long PV { get; set; }

        //uv统计
        public long UV { get; set; }

        //平均在线时长(秒)
        public double OnLineAvgTime { get; set; } = -1;

        //人均浏览页面数
        public int BrowsePageAvg { get; set; } = -1;

        //跳出率
        public decimal JumpProportion { get; set; } = -1;

        //询价数
        public int InquiryNumber { get; set; } = -1;

        //会话数
        public int SessionNumber { get; set; } = -1;

        //电话接通数
        public int TelConnectNumber { get; set; } = -1;

        //来源（赤兔，青鸟，经纪人）
        public int Source { get; set; }

        //状态（0正常）
        public int Status { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }

        public int DistributeDetailType { get; set; }
    }
}