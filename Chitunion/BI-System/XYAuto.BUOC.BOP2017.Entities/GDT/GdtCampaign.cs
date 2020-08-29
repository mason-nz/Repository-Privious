using System;

namespace XYAuto.BUOC.BOP2017.Entities.GDT
{
    //广点通推广计划表
    public class GdtCampaign
    {
        //自增Id
        public int Id { get; set; }

        //赤兔用户id
        public int CampaignId { get; set; }

        //推广计划名称，不可与名下其他推广计划重名
        public string CampaignName { get; set; }

        //客户设置的状态详见 [推广计划类型]
        public int ConfiguredStatus { get; set; }

        //标的物类型，详见 [标的物类型]
        public int CampaignType { get; set; }

        //日消耗限额，单位为分，详见[日限额修改规则]
        public int DailyBudget { get; set; }

        //日限额到达日期，值为最近一次触达日限额的日期，如无触达记录则为空，如：20170501，表示最近一次到达日限额的日期是 5 月 1 日，若今天是 5 月 1 日，则表示今天已经到达日限额
        public int BudgetReachDate { get; set; }

        //广点通代理商子客户（简称子客）账户id
        public int AccountId { get; set; }

        //创建时间（时间戳）
        public int CreatedTime { get; set; }

        //最后修改时间（时间戳）
        public int LastModifiedTime { get; set; }

        //拉取时间
        public DateTime PullTime { get; set; }
    }
}