using System;

namespace XYAuto.BUOC.BOP2017.Entities.GDT
{
    //接收智慧云回传的充值单，及时去拉取账户信息，然后传给智慧云接口
    public class GdtRechargeRelation
    {
        //自增Id
        public int Id { get; set; }

        //需求单号
        public int DemandBillNo { get; set; }

        //充值单号
        public string RechargeNumber { get; set; }

        //金额（单位为分，小数点2位）
        public decimal Amount { get; set; }

        //操作人
        public int CreateUserId { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}