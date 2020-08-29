using System;

namespace XYAuto.ITSC.Chitunion2017.Entities.GDT
{
    //广点通直客账户表
    public class GdtAccountInfo
    {
        //自增Id
        public int GDTUserId { get; set; }

        //广点通代理商子客户（简称子客）账户id
        public int AccountId { get; set; }

        //日限额，单位为分，详见[日限额修改规则]
        public int DailyBudget { get; set; }

        //客户系统状态
        public int SystemStatus { get; set; }

        //审核消息
        public string RejectMessage { get; set; }

        //企业名称
        public string CorporationName { get; set; }

        //联系人姓名
        public string ContactPerson { get; set; }

        //联系人座机电话号码，格式为：区号-座机号，例如：0755-86013388
        public string ContactPersonTelephone { get; set; }

        //拉取时间
        public DateTime PullTime { get; set; } = DateTime.Now;
    }
}