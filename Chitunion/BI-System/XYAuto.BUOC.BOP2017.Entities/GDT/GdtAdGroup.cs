/********************************************************
*创建人：lixiong
*创建时间：2017/8/17 14:56:13
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;

namespace XYAuto.BUOC.BOP2017.Entities.GDT
{
    public class GdtAdGroup
    {
        //自增Id
        public int Id { get; set; }

        //广点通代理商子客户（简称子客）账户id
        public int AccountId { get; set; }

        //推广计划 id
        public int CampaignId { get; set; }

        //广告组 id
        public int AdgroupId { get; set; }

        //广告组名称，同一账户下名称不允许重复
        public string AdgroupName { get; set; }

        //客户设置的状态，详见[客户设置的状态]
        public int ConfiguredStatus { get; set; }

        //广告在系统中的状态，详见[推广计划、广告组、广告的系统状态]
        public int SystemStatus { get; set; }

        //审核消息
        public string RejectMessage { get; set; }

        //详见 [投放站点集合]
        public string SiteSet { get; set; }

        //广告目标类型，详见 [优化目标]
        public int OptimizationGoal { get; set; }

        //计费类型，详见 [扣费方式]
        public int BillingEvent { get; set; }

        //广告出价，单位为分，详见 [出价规则]
        public int BidAmount { get; set; }

        //广告出价，单位为分
        public int DailyBudget { get; set; }

        //开始投放日期，日期格式，YYYY-mm-dd，且日期小于等于end_date
        public DateTime BeginDate { get; set; }

        //结束投放日期，日期格式，YYYY-mm-dd，大于等于今天，且大于等于 begin_date
        public DateTime EndDate { get; set; }

        //投放时间段，格式为 48 * 7 位字符串，且都为 0 和 1，以半个小时为最小粒度，0 为不投放，1 为投放，不传此字段、全为 0、全为 1 均视为全时段投放
        public string TimeSeries { get; set; }

        //自定义分类，关键词用半角逗号','分隔，如：本地生活,餐饮
        public string CustomizedCategory { get; set; }

        //创建时间（时间戳）
        public int CreatedTime { get; set; }

        //最后修改时间（时间戳）
        public int LastModifiedTime { get; set; }

        //拉取时间
        public DateTime PullTime { get; set; }
    }
}