/********************************************************
*创建人：hant
*创建时间：2018/1/27 17:52:36 
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
    public class OrderInfoRspDTO
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
