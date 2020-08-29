/********************************************************
*创建人：lixiong
*创建时间：2017/11/23 20:29:44
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
    public class RespGrabHeadCjDto : RespGrabHeadQuDao
    {
        public List<DicInfo> DicInfo { get; set; }
    }

    public class DicInfo
    {
        public int TypeId { get; set; }
        public string Name { get; set; }
    }
}