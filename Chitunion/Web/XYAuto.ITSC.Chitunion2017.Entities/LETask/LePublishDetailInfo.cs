using System;

namespace XYAuto.ITSC.Chitunion2017.Entities.LETask
{
    //广告位
    public class LePublishDetailInfo
    {

        //主键
        public int RecID { get; set; }

        //媒体分类（枚举）
        public int MediaType { get; set; }

        //媒体ID
        public int MediaID { get; set; }

        //位置维度1（枚举）
        public int ADPosition1 { get; set; }

        //位置维度2（枚举）
        public int ADPosition2 { get; set; }

        //位置维度3（枚举）
        public int ADPosition3 { get; set; }

        //价格
        public decimal Price { get; set; }

        //上下架状态
        public int PublishStatus { get; set; }

        public DateTime CreateTime { get; set; }

        public int CreateUserID { get; set; }


    }
}