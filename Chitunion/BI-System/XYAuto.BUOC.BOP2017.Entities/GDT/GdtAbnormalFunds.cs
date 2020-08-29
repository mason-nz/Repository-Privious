using System;

namespace XYAuto.BUOC.BOP2017.Entities.GDT
{
    //资金异常表
    public class GdtAbnormalFunds
    {
        //自增Id
        public int Id { get; set; }

        //充值单号
        public string RechargeNumber { get; set; }

        //需求单号
        public int DemandBillNo { get; set; }

        //广点通代理商子客户（简称子客）账户id
        public int AccountId { get; set; }

        //交易类型，详见 [交易类型]
        public int TradeType { get; set; }

        //充值金额，单位为分
        public decimal RechargeAmount { get; set; }

        //花费金额，单位为分
        public int SpendAmount { get; set; }

        //回划金额，单位为分
        public int TransferBackAmount { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; } = DateTime.Now;

        //创建人
        public int CreateUserId { get; set; }
    }
}