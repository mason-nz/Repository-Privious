/********************************************************
*创建人：lixiong
*创建时间：2017/11/23 15:37:17
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Response.Grab
{
    public class RespGrabHeadDto : RespGrabBaseDto
    {
        public List<StatData> Data { get; set; }
    }

    public class RespGrabHeadDto<T> : RespGrabBaseDto
    {
        public List<T> Data { get; set; }
    }

    public class StatData
    {
        public string name { get; set; }
        public int[] data { get; set; }
    }
}