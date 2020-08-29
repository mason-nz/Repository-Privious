/********************************************************
*创建人：lixiong
*创建时间：2017/11/29 9:47:23
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Request
{
    public class ReqStatTabGrabDto
    {
        public int LatelyDays { get; set; } = 7;

        public string Category { get; set; }
        public string TabType { get; set; }
    }
}