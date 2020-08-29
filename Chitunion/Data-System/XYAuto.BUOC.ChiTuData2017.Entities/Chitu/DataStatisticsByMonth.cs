/********************************************************
*创建人：hant
*创建时间：2017/12/22 10:12:15 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Chitu
{
    public class DataStatisticsByMonth
    {
        public int RecID { get; set; }
        public DateTime Date { get; set; }
        public int ChannelID { get; set; }

        public string ChannelName { get; set; }
        public int OrderNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public int StateOfSettlement { get; set; }
        public DateTime TimeOfSettlement { get; set; }
        public int OperaterId { get; set; }
        public string OperaterName { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
