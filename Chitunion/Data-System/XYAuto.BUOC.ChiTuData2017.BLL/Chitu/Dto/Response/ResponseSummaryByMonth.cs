/********************************************************
*创建人：hant
*创建时间：2017/12/22 10:41:35 
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
    public class ResponseSummaryByMonth
    {
        public int TotalCount { get; set; }

        public List<SummaryByMonth> List { get; set; }
    }

    public class SummaryByMonth
    {
        [ExcelTitle("月结ID")]
        public int RecID { get; set; }
        [ExcelTitle("月份")]
        public string Date { get; set; }
        [ExcelTitle("汇总日期")]
        public DateTime CreateTime { get; set; }
        public int ChannelID { get; set; }
        [ExcelTitle("订单渠道")]
        public string ChannelName { get; set; }
        [ExcelTitle("订单笔数")]
        public int OrderNumber { get; set; }
        [ExcelTitle("累计结算金额")]
        public decimal TotalAmount { get; set; }
        [ExcelTitle("结算状态")]
        public int StateOfSettlement { get; set; }
        [ExcelTitle("实际结算时间")]
        public DateTime TimeOfSettlement { get; set; }
        [ExcelTitle("操作人")]
        public string OperaterName { get; set; }
        public string FirstDay { get; set; }
        public string EndDay { get; set; }
    }


    public class SummaryByMonthOperater
    {
        [ExcelTitle("月结ID")]
        public int RecID { get; set; }
        [ExcelTitle("月份")]
        public string Date { get; set; }
        [ExcelTitle("汇总日期")]
        public DateTime CreateTime { get; set; }
        public int ChannelID { get; set; }
        [ExcelTitle("订单渠道")]
        public string ChannelName { get; set; }
        [ExcelTitle("订单笔数")]
        public int OrderNumber { get; set; }
        [ExcelTitle("累计结算金额")]
        public decimal TotalAmount { get; set; }
        [ExcelTitle("结算状态")]
        public int StateOfSettlement { get; set; }
        [ExcelTitle("实际结算时间")]
        public DateTime TimeOfSettlement { get; set; }
       
        public string OperaterName { get; set; }
        public DateTime FirstDay { get; set; }
        public DateTime EndDay { get; set; }
    }
}
