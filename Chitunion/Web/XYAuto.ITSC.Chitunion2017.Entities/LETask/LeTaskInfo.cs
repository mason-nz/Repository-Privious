using System;

namespace XYAuto.ITSC.Chitunion2017.Entities.LETask
{
    //任务表
    public class LeTaskInfo
    {


        //主键
        public int RecID { get; set; }

        //任务名称
        public string TaskName { get; set; }

        //计费规则名称
        public string BillingRuleName { get; set; }

        //物料链接URL地址
        public string MaterialUrl { get; set; }

        //物料ID
        public int MaterialID { get; set; }

        //枚举类型
        public int TaskType { get; set; }

        //（默认 20万）
        public int RuleCount { get; set; }

        //每领取一次减1
        public int TakeCount { get; set; }

        //任务开始时间
        public DateTime BeginTime { get; set; }

        //任务结束时间
        public DateTime EndTime { get; set; }

        public int Status { get; set; }

        //任务总金额
        public decimal TaskAmount { get; set; }

        //CPC单次金额
        public decimal CPCPrice { get; set; }

        //CPL单次金额
        public decimal CPLPrice { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; } = DateTime.Now;

        //图片地址
        public string ImgUrl { get; set; }

        //简介
        public string Synopsis { get; set; }

        //分类ID
        public int CategoryID { get; set; }

        //CPC最大金额
        public decimal CPCLimitPrice { get; set; }

        //CPL最大金额
        public decimal CPLLimitPrice { get; set; }


    }
}