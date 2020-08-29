using System;
using System.Collections.Generic;

namespace XYAuto.ChiTu2018.Service.App.Task.Dto.GetOrderInfo
{
    /// <summary>
    /// 注释：ResDto
    /// 作者：lihf
    /// 日期：2018/5/14 11:30:20
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class ResDto
    {
        public int OrderId { get; set; }
        // 订单名称
        public string OrderName { get; set; }
        // 计费规则
        public string BillingRuleName { get; set; }
        // 领取时间
        public DateTime ReceiveTime { get; set; }
        // 专属链接
        public string OrderUrl { get; set; }

        public string TaskName { get; set; }

        public string Synopsis { get; set; }

        public string ImgUrl { get; set; }

        public decimal CPCUnitPrice { get; set; }
        // 订单收入
        public List<List> List { get; set; }
        // 汇总
        public Extend Extend { get; set; }
    }
    public class List
    {
        public DateTime Date { get; set; }

        public int CPCCount { get; set; }

        public decimal? CPCTotalPrice { get; set; }
    }

    public class Extend
    {
        public int TotalCPCCount { get; set; }

        public decimal? TotalCPCTotalPrice { get; set; }
    }
}
