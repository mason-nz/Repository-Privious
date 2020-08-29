using System;

namespace XYAuto.BUOC.BOP2017.Entities.Demand
{
    //需求落地页加参管理（关联）
    public class DemandGroundDelivery
    {
        //加参id（自增）
        public int DeliveryId { get; set; }

        //需求单号
        public int DemandBillNo { get; set; }

        //落地页id
        public int GroundId { get; set; }

        //投放平台（广点通，今日头条）
        public int DeliveryType { get; set; }

        //广告版位（站点）
        public int AdSiteSet { get; set; }

        //广告创意
        public int AdCreative { get; set; }

        //广告名称
        public string AdName { get; set; }

        //推广地址（落地页管理里面的url）
        public string PromotionUrl { get; set; }

        //推广码
        public string PromotionUrlCode { get; set; }

        //状态（0正常）
        public int Status { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; } = DateTime.Now;

        //创建人
        public int CreateUserId { get; set; }

        /* 冗余 */

        public int AuditStatus { get; set; }

        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}