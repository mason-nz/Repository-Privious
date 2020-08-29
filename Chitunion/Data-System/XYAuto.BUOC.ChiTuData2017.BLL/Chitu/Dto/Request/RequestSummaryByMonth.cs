/********************************************************
*创建人：hant
*创建时间：2017/12/22 10:33:57 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Chitu.Dto.Request
{
    public class RequestSummaryByMonth
    {
       public string BeginTime {get;set;}
        public string EndTime { get;set;}
        public int ChannelID { get;set;}
        public int StateOfSettlement { get;set;}
        public int PageIndex { get;set;}
        public int PageSize { get;set;}
    }
}
