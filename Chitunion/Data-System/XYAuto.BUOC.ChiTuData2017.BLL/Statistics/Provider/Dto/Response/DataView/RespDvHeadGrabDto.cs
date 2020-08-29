/********************************************************
*创建人：lixiong
*创建时间：2017/11/28 14:20:45
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Response.DataView
{
    public class RespDvHeadGrabDto<T, TB> : RespDataViewBaseDto
    {
        public DataPieDto<T> DataPie { get; set; }

        public DataBarDto<TB> DataBar { get; set; }
    }

    public class DataPieDto<T>
    {
        public string[] DataLegend { get; set; }
        public List<T> Data { get; set; }
    }

    public class DataBarDto<TB>
    {
        public string[] DataLegend { get; set; }
        public List<Grab.DicInfo> DicInfo { get; set; }
        public TB Data { get; set; }
    }
}