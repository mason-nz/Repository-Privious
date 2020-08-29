/********************************************************
*创建人：hant
*创建时间：2017/12/19 11:01:56 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Task
{
    public class MediaChannel
    {
        /// <summary>
        /// AppKey + 时间段作为唯一标识
        /// </summary>
        public string AppKey { get; set; }

        public int Number { get; set; }
    }
}
