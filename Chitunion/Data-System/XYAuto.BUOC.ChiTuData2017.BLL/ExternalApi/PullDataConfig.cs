/********************************************************
*创建人：lixiong
*创建时间：2017/10/17 10:06:41
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.BLL.ExternalApi
{
    public class PullDataConfig
    {
        public string Date { get; set; }
        public int DateOffset { get; set; } = -1;

        public int PullDataQueryDateOffset { get; set; } = 8;
    }
}