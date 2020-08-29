/********************************************************
*创建人：lixiong
*创建时间：2017/8/17 11:29:58
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.GDT;

namespace XYAuto.ITSC.Chitunion2017.Entities.Query.GDT
{
    public class ReportQuery<T> : QueryPageBase<T>
    {
        public int DemandBillNo { get; set; }
        public int AccountId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
        public string Date { get; set; }
        public ReportLevelEnum Level { get; set; } = ReportLevelEnum.None;
    }
}