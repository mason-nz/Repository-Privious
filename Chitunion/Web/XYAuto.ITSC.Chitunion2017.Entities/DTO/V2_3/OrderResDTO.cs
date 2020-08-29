/********************************************************
*创建人：hant
*创建时间：2018/1/25 15:54:49 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V2_3
{
    public class OrderResDTO
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
