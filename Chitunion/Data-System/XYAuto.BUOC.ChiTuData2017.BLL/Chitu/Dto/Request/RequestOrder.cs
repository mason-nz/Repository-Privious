/********************************************************
*创建人：hant
*创建时间：2017/12/21 17:57:23 
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
    public class RequestOrder
    {

        public int Status { get; set; }
        public string BeginTime { get; set; }
        public string EndTime { get; set; }
        public int OrderType { get; set; }
        public int ChannelID { get; set; }
        public int OrderID { get; set; }
        public int TaskID { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
