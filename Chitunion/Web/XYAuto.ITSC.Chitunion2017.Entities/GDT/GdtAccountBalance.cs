using System;

namespace XYAuto.ITSC.Chitunion2017.Entities.GDT
{
    //账户余额
    public class GdtAccountBalance
    {
        //自增Id
        public int Id { get; set; }

        //广点通代理商子客户（简称子客）账户id
        public int AccountId { get; set; }

        //资金账户类型，详见 [资金账户类型定义]
        public int FundType { get; set; }

        //余额，单位为分
        public int Balance { get; set; }

        //资金状态，详见[资金状态]
        public int FundStatus { get; set; }

        //当日花费，单位为分
        public int RealtimeCost { get; set; }

        //拉取时间
        public DateTime PullTime { get; set; }
    }
}