using System;

namespace XYAuto.ITSC.Chitunion2017.Entities.GDT
{
    //广点通资金账户日结明细
    public class GdtStatementDaily
    {
        //自增Id
        public int Id { get; set; }

        //广点通代理商子客户（简称子客）账户id
        public int AccountId { get; set; }

        //资金账户类型，详见 [资金账户类型定义]
        public int FundType { get; set; }

        //交易类型，详见 [交易类型]
        public int TradeType { get; set; }

        //交易时间（时间戳）
        public int Time { get; set; }

        //金额，单位为分
        public int Amount { get; set; }

        //描述信息
        public string Description { get; set; }

        //拉取时间
        public DateTime PullTime { get; set; }

        public DateTime Date { get; set; }
    }
}