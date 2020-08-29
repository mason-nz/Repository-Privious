using System;

namespace XYAuto.ITSC.Chitunion2017.Entities.GDT
{
    //智慧云需求关联广告表，我们把维度设置到最细
    public class GdtDemandRelation
    {
        //自增Id
        public int Id { get; set; }

        //智慧云需求id
        public int DemandBillNo { get; set; }

        //广告id
        public int AdId { get; set; }

        //广告组id
        public int AdgroupId { get; set; }

        //创建人
        public DateTime CreateUserId { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }

        public int AccountId { get; set; }
    }
}