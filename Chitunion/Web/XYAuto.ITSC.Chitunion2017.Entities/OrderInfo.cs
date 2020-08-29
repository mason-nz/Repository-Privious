using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities
{
    /// <summary>
    /// 2017-02-27 张立彬
    /// 订单类
    /// </summary>
    public class OrderInfo
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNum { get; set; }
        /// <summary>
        /// 订单名称
        /// </summary>
        public string OrderName { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal OrderMoney { get; set; }
        /// <summary>
        /// 媒体类型ID
        /// </summary>
        public int MediaType { get; set; }
        /// <summary>
        /// 媒体类型
        /// </summary>
        public string MediaName { get; set; }
        /// <summary>
        /// 订单状态(文字描述)
        /// </summary>
        public string OrderState { get; set; }
        /// <summary>
        /// 订单来源
        /// </summary>
        public string OrderSource { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creater { get; set; }
        /// <summary>
        /// AE角色标识（0广告主身份 其他媒体主身份）
        /// </summary>
        public string OrderCreatSource { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 媒体账号
        /// </summary>
        public string MediaAccount { get; set; }
        /// <summary>
        /// CRM编号
        /// </summary>
        public string CrmNum { get; set; }
        /// <summary>
        /// 成本参考价
        /// </summary>
        public decimal CostReferPrice { get; set; }
    }
}
