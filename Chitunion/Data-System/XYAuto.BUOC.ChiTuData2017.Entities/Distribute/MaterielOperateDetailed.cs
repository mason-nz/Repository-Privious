using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Distribute
{
    //点击明细，转发明细
    public class MaterielOperateDetailed
    {
        //自增id
        public int Id { get; set; }

        //物料ID
        public int MaterielId { get; set; }

        //物料详情数据统计id
        public int StatisticsId { get; set; }

        //枚举类型id
        public int DicId { get; set; }

        //点击类型（uv，pv，转发）
        public int ClickType { get; set; }

        //点击类型对应的值
        public long ClickValue { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }

        //状态（0正常）
        public int Status { get; set; }

        //内容类型（头，腰，尾）
        public int ConentType { get; set; }

        //文章id（冗余，做标记用）
        public int ArticleId { get; set; }
    }
}