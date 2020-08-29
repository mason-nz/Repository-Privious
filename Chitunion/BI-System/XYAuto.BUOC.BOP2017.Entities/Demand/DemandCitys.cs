using System;

namespace XYAuto.BUOC.BOP2017.Entities.Demand
{
    //需求省份城市关联表
    public class DemandCitys
    {
        //落地页id（自增）
        public int GroundId { get; set; }

        //需求单号
        public int DemandBillNo { get; set; }

        //车型id
        public int ProvinceId { get; set; }

        //省份名称（一般不会变动，可以冗余）
        public string ProvinceName { get; set; }

        //品牌id
        public int CityId { get; set; }

        //城市名称（一般不会变动，可以冗余）
        public string CityName { get; set; }

        //状态（0正常）
        public int Status { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }
    }
}