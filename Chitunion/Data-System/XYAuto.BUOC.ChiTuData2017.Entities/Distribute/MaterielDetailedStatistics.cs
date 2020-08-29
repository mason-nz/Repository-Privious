using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Distribute
{
    //物料详情明细表（分发明细表 下面的详情，以天为单位）
    public class MaterielDetailedStatistics
    {
        //自增id
        public int StatisticsId { get; set; }

        public string Title { get; set; }

        public DateTime Date { get; set; }

        //物料ID
        public int MaterielId { get; set; }

        //分发id
        public int DistributeId { get; set; }

        //文章id
        public int ArticleId { get; set; }

        //内容类型（头，腰，尾）
        public int ConentType { get; set; }

        public string ConentTypeName { get; set; }

        //文章类型（图文，商务专题）
        public int ArticleType { get; set; }

        public string ArticleTypeName { get; set; }

        //阅读数
        public int ReadNumber { get; set; } = -1;

        //创建人
        public int CreateUserId { get; set; }

        //pv统计
        public long PV { get; set; } = -1;

        //uv统计
        public long UV { get; set; } = -1;

        //状态（0正常）
        public int Status { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }

        //点赞数
        public int LikeNumber { get; set; } = -1;

        //转发数
        public int ForwardNumber { get; set; } = -1;

        //点击量
        public int ClickNumber { get; set; } = -1;

        //文章url
        public string Url { get; set; }

        //点击pv
        public long ClickPV { get; set; } = -1;

        //点击uv
        public long ClickUV { get; set; } = -1;

        //自定义类型
        public int CustomType { get; set; }

        //自定义位置
        public int CustomLocation { get; set; }
    }
}