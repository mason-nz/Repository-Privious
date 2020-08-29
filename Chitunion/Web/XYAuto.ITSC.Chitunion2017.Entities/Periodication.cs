using System;
using System.Collections.Generic;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities
{
    /// <summary>
    /// 2017-02-22 张立彬
    /// 刊例信息
    /// </summary>
    public class Periodication
    {
        /// <summary>
        /// 媒体ID
        /// </summary>
        public int MediaID { get; set; }
        /// <summary>
        /// 媒体名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 媒体账号或APP类型
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 执行周期开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 执行周期结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 采购折扣
        /// </summary>
        public decimal PurchaseDiscount { get; set; }
        /// <summary>
        /// 销售折扣
        /// </summary>
        public decimal SaleDiscount { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public string CheckTime { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string CheckUser { get; set; }
        /// <summary>
        /// 审核结果
        /// </summary>
        public string CheckResult { get; set; }
        /// <summary>
        /// 审核后状态或者驳回原因（根据审核结果判断）
        /// </summary>
        public string CheckedStatus { get; set; }
        /// <summary>
        /// 广告维度集合
        /// </summary>
        public List<PeriodicationFirst> Detail = new List<PeriodicationFirst>();
    }
    /// <summary>
    /// 2017-02-22 张立彬
    /// 广告位维度
    /// </summary>
    public class PeriodicationMin
    {
        /// <summary>
        /// 维度ID组合
        /// </summary>
        public string Combdimension { get; set; }
        /// <summary>
        /// 二级广告维度
        /// </summary>
        public string Second { get; set; }
        /// <summary>
        /// 三级广告维度
        /// </summary>
        public string Third { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
    }
    /// <summary>
    ///  广告维度集合
    /// </summary>
    public class PeriodicationFirst
    {
        public string First { get; set; }
        public List<PeriodicationMin> SecondDescrit = new List<PeriodicationMin>();
    }

}
