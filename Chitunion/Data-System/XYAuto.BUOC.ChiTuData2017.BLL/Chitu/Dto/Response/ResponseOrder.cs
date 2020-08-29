/********************************************************
*创建人：hant
*创建时间：2017/12/21 18:20:42 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu.Excel;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Chitu.Dto.Response
{

    public class ResponseOrderList
    {
        public int TotalCount { get; set; }
        public List<ResponseOrder> List { get; set; }

    }
    public class ResponseOrder
    {
        [ExcelTitle("订单ID")]
        public int OrderId { get; set; }
        [ExcelTitle("任务ID")]
        public int TaskID { get; set; }
        [ExcelTitle("订单渠道")]
        public string ChannelName { get; set; }
        [ExcelTitle("创建人")]
        public string UserIdentity { get; set; }
        [ExcelTitle("创建时间")]
        public DateTime CreateTime { get; set; }
        [ExcelTitle("结束时间")]
        public DateTime EndTime { get; set; }
        [ExcelTitle("点击单价")]
        public decimal CPCPrice { get; set; }
        [ExcelTitle("点击数量")]
        public int CPCCount { get; set; }
        [ExcelTitle("线索单价")]
        public decimal CPLPrice { get; set; }
        [ExcelTitle("线索数量")]
        public int CPLCount { get; set; }
        [ExcelTitle("实际收益")]
        public decimal TotalAmount { get; set; }
        [ExcelTitle("最高收益")]
        public decimal TotalProfit { get; set; }
        [ExcelTitle("订单类型")]
        public string OrderType { get; set; }


    }

    public class ResponseOrderChannel
    {
        [ExcelTitle("订单ID")]
        public int OrderId { get; set; }
        [ExcelTitle("任务ID")]
        public int TaskID { get; set; }
        [ExcelTitle("订单渠道")]
        public string ChannelName { get; set; }
        [ExcelTitle("创建人")]
        public string UserIdentity { get; set; }
        [ExcelTitle("创建时间")]
        public DateTime CreateTime { get; set; }
        [ExcelTitle("结束时间")]
        public DateTime EndTime { get; set; }
        [ExcelTitle("点击单价")]
        public decimal CPCPrice { get; set; }

        public int CPCCount { get; set; }
        [ExcelTitle("线索单价")]
        public decimal CPLPrice { get; set; }

        public int CPLCount { get; set; }
        [ExcelTitle("实际收益")]
        public decimal TotalAmount { get; set; }
        [ExcelTitle("最高收益")]
        public decimal TotalProfit { get; set; }
        [ExcelTitle("订单类型")]
        public string OrderType { get; set; }


    }
}
