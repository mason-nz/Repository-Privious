using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Distribute
{
    //物料分发明细表（物料ID以天为单位存储）
    public class MaterielDistributeDetailed
    {
        //自增id
        public int DistributeId { get; set; }

        //物料ID
        public int MaterielId { get; set; }

        //统计时间（单位天）
        public DateTime Date { get; set; }

        //pv统计
        public long PV { get; set; } = -1;

        //uv统计
        public long UV { get; set; } = -1;

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

        //创建人
        public int CreateUserId { get; set; }

        //分发url
        public string DistributeUrl { get; set; }

        public int DistributeDetailType { get; set; }

        public int ForwardNumber { get; set; }
    }
}