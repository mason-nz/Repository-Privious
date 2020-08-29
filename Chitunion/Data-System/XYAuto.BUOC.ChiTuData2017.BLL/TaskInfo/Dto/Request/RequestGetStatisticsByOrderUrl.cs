/********************************************************
*创建人：hant
*创建时间：2017/12/20 10:32:00 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.BLL.TaskInfo.Dto.Request
{
    public class RequestGetStatisticsByOrderUrl:RequestBase
    {
        public string orderurl { get; set; }
        public string begindate { get; set; }
        public string enddate { get; set; }
        public string page_index { get; set; }

    }
}
