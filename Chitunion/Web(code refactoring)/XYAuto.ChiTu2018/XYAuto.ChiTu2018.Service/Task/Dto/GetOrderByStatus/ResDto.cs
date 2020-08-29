/// <summary>
/// 注释：ResDto
/// 作者：lihf
/// 日期：2018/5/10 17:13:54
/// 版权所有：Copyright  2018 行圆汽车-分发业务中心
/// </summary>

using System;
using System.Collections.Generic;

namespace XYAuto.ChiTu2018.Service.Task.Dto.GetOrderByStatus
{
    public class ResDto
    {
        public int TotalCount { get; set; }
        public List<Order> List { get; set; }
    }

    public class Order
    {
        public int TaskID { get; set; }
        public int OrderId { get; set; }

        public decimal TotalAmount { get; set; }

        public string OrderName { get; set; }
        public string OrderUrl { get; set; }

        public DateTime CreateTime { get; set; }

        public decimal CPCUnitPrice { get; set; }
    }
}
