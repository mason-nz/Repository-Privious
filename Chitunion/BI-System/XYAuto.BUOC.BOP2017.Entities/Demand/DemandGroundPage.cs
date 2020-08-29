using System;

namespace XYAuto.BUOC.BOP2017.Entities.Demand
{
    //需求落地页管理
    public class DemandGroundPage
    {
        //落地页id（自增）
        public int GroundId { get; set; }

        //需求单号
        public int DemandBillNo { get; set; }

        //品牌id
        public int BrandId { get; set; }

        //车型id
        public int SerielId { get; set; }

        //省份id
        public int ProvinceId { get; set; }

        //城市id
        public int CityId { get; set; }

        //落地页url
        public string PromotionUrl { get; set; }

        //状态（0正常）
        public int Status { get; set; } = 0;

        //创建时间
        public DateTime CreateTime { get; set; } = DateTime.Now;

        //创建人
        public int CreateUserId { get; set; }

        /*冗余*/

        public int DeliveryCount { get; set; }

        public int AuditStatus { get; set; }

        public string CarInfo { get; set; }
        public string CityInfo { get; set; }
    }
}