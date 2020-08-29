/********************************************************
*创建人：lixiong
*创建时间：2017/11/23 18:57:49
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
    public class RespGrabHeadQuDao : RespGrabBaseDto
    {
        public List<StatKeyValueData> Data { get; set; }
    }

    public class StatKeyValueBaseData
    {
        public int value { get; set; }
        public string name { get; set; }
    }

    public class StatKeyValueData : StatKeyValueBaseData
    {
        public int TypeId { get; set; }
    }
}