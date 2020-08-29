/********************************************************
*创建人：lixiong
*创建时间：2017/11/27 11:21:28
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Response.Grab;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Response.CarMatch
{
    public class RespCarHeadDto : RespGrabBaseDto
    {
    }

    public class RespCarHeadDto<T> : RespGrabBaseDto
    {
        public List<T> Data { get; set; }
    }

    public class RespCarHeadRightBarDto<T> : RespGrabBaseDto
    {
        public List<T> Data { get; set; }

        public List<Grab.DicInfo> DicInfo { get; set; }
    }

    public class RespCarHeadRightBarExtendDto<T> : RespGrabBaseDto
    {
        public List<T> Data { get; set; }

        public List<Grab.DicInfo> DicInfo { get; set; }

        public List<DicTypeInfo> DicInfoMatch { get; set; }
    }

    public class DicTypeInfo
    {
        public string Name { get; set; }
        public int TypeMatchId { get; set; }
    }
}