/********************************************************
*创建人：hant
*创建时间：2017/12/21 14:44:22 
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
    public class ResposeDataStatistics
    {
        public decimal ProfitOfYesterday { get; set; }
        public decimal ProfitOfThisMonth { get; set; }
        public decimal ProfitLastMonth { get; set; }
        public decimal Accumulated { get; set; }
        public decimal SettledAmount { get; set; }

        public List<Channel> ChannelList { get; set; }
    }

    public class Channel
    {
        public string ChannelName { get; set; }
        public decimal ProfitOfYesterday { get; set; }
        public decimal ProfitOfThisMonth { get; set; }
        public decimal ProfitLastMonth { get; set; }
        public decimal Accumulated { get; set; }
        public decimal SettledAmount { get; set; }
    }
}
