/********************************************************
*创建人：lixiong
*创建时间：2017/11/27 18:52:42
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Response.Grab;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Response.Primary
{
    public class RespCsZhuanHuaDto : RespGrabBaseDto
    {
        //public new List<StatTypeInfo> Info { get; set; }

        public List<StatInfo> TotalInfo { get; set; }
    }

    public class StatTypeInfo : StatInfo
    {
        public int TypeMatchId { get; set; }
    }
}