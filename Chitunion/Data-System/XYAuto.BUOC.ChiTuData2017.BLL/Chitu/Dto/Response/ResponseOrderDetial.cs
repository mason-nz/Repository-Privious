/********************************************************
*创建人：hant
*创建时间：2017/12/22 9:51:53 
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
    public class ResponseOrderDetial
    {
        public int OrderId { get; set; }
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string ChannelName { get; set; }
        public string OrderType { get; set; }
        public string Status { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime EndTime { get; set; }
        public string UserIdentity { get; set; }
        public decimal CPCPrice { get; set; }
        public decimal CPLPrice { get; set; }
        public decimal TotalProfit { get; set; }
        public int StateOfSettlement { get; set; }
        public DateTime TimeOfSettlement { get; set; }
        public string MaterialUrl { get; set; }
        public string OrderUrl { get; set; }

        public List<AccountBalance> List { get; set; }
    }

    public class AccountBalance
    {
        public DateTime Date { get; set; }
        public int CPCCount { get; set; }
        public decimal CPCProfit { get; set; }
        public int CPLCount { get; set; }
        public decimal CPLProfit { get; set; }
        public decimal TotalProfit { get; set; }
    }
}
