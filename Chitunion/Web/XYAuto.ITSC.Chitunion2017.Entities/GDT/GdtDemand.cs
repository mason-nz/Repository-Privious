using System;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.GDT;

namespace XYAuto.ITSC.Chitunion2017.Entities.GDT
{
    //智慧云推送过来的需求表
    public class GdtDemand
    {
        //自增Id
        public int DemandId { get; set; }

        //需求单号
        public int DemandBillNo { get; set; }

        //需求名称
        public string Name { get; set; }

        //需求状态
        public int Status { get; set; }

        public string BrandSerialJson { get; set; }
        public string ProvinceCityJson { get; set; }
        public string DistributorJson { get; set; }

        //促销政策，
        public string PromotionPolicy { get; set; }

        //总投放预算
        public decimal TotalBudget { get; set; }

        //线索数量
        public int ClueNumber { get; set; }

        //开始时间（日期格式：YYYY-mm-dd）
        public DateTime BeginDate { get; set; }

        //结束时间 （日期格式：YYYY-mm-dd）
        public DateTime EndDate { get; set; }

        public DemandAuditStatusEnum AuditStatus { get; set; } = DemandAuditStatusEnum.PendingAudit;

        //需求提交人(手机号)
        public int CreateUserId { get; set; }

        //需求修改人(手机号)
        public int UpdateUserId { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; } = DateTime.Now;

        //修改时间
        public DateTime UpdateTime { get; set; } = DateTime.Now;
    }

    public class GdtDemandStatusNotes
    {
        //需求单号
        public int DemandBillNo { get; set; }

        public DemandAuditStatusEnum AuditStatus { get; set; }

        public int OrganizeId { get; set; }
    }
}