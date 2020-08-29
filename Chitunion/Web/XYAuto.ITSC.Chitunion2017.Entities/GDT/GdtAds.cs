using System;

namespace XYAuto.ITSC.Chitunion2017.Entities.GDT
{
    //广告表
    public class GdtAds
    {
        //自增Id
        public int Id { get; set; }

        //广点通代理商子客户（简称子客）账户id
        public int AccountId { get; set; }

        //推广计划 id
        public int CampaignId { get; set; }

        //广告组 id
        public int AdgroupId { get; set; }

        //广告 id
        public int AdId { get; set; }

        //广告名称，同一账户下名称不允许重复
        public string AdName { get; set; }

        //客户设置的状态，详见[客户设置的状态]
        public string ConfiguredStatus { get; set; }

        //广告在系统中的状态，详见[推广计划、广告组、广告的系统状态]
        public string SystemStatus { get; set; }

        //审核消息
        public string RejectMessage { get; set; }

        //创建时间（时间戳）
        public int CreatedTime { get; set; }

        //最后修改时间（时间戳）
        public int LastModifiedTime { get; set; }

        //拉取时间
        public DateTime PullTime { get; set; }
    }
}