using System;

namespace XYAuto.ITSC.Chitunion2017.Entities.GDT
{
    //广点通资金账户流水
    public class GdtStatementsDetailed
    {
        //自增Id
        public int Id { get; set; }

        //广点通代理商子客户（简称子客）账户id
        public int AccountId { get; set; }

        //交易类型，详见 [交易类型]
        public int TradeType { get; set; }

        public int FundType { get; set; }

        //金额，单位为分
        public int Amount { get; set; }

        //查询日期，日期格式：YYYY-mm-dd，只支持今天或昨天的数据查询
        public DateTime Date { get; set; }

        //调用方订单号，需要有调用方标示前缀，且要保证在同一个广告主下唯一，如 aaaa-123-456
        public string ExternalBillNo { get; set; }

        //描述信息
        public string Description { get; set; }

        //拉取时间
        public DateTime PullTime { get; set; }
    }
}