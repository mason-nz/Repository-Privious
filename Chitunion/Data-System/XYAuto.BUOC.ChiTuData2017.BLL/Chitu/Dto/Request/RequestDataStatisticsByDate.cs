/********************************************************
*创建人：hant
*创建时间：2017/12/21 15:41:00 
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
    public class RequestDataStatisticsByDate
    {
        public int ChannelID { get; set; }
        public string BeginDate { get; set; }
        public string EndDate { get; set; }
        public int StateOfSettlement { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
