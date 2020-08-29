/********************************************************
*创建人：hant
*创建时间：2017/12/21 16:05:59 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Chitu.Dto.Response
{
    public class ResponseDataStatisticsByDate
    {
        public int TotalCount { get; set; }

        public List<StatisticsByData> List { get; set; }
    }

    public class StatisticsByData
    {
        public DateTime Date { get; set; }
        public int ChannelID { get; set; }
        public string ChannelName { get; set; }
        public decimal Profit { get; set; }
        public int OrderNumber { get; set; }
        public int StateOfSettlement { get; set; }
        public DateTime TimeOfSettlement { get; set; }
    }
}
