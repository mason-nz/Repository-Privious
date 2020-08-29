using System;

namespace XYAuto.BUOC.BOP2017.Entities.Demand
{
    //需求品牌车型关联表
    public class DemandCarSeriel
    {
        //落地页id（自增）
        public int GroundId { get; set; }

        //需求单号
        public int DemandBillNo { get; set; }

        //车型id
        public int SerielId { get; set; }

        //车型名称（一般不会变动，可以冗余）
        public string SerielName { get; set; }

        //品牌id
        public int BrandId { get; set; }

        //子品牌名称（一般不会变动，可以冗余）
        public string BrandName { get; set; }

        //状态（0正常）
        public int Status { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }
    }
}