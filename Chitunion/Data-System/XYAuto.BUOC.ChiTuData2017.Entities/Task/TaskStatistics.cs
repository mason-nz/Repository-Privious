/********************************************************
*创建人：hant
*创建时间：2017/12/20 9:35:20 
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
    public class TaskStatistics
    {
        public string Date { get; set; }
        public int TaskId { get; set; }
        public string OrderUrl { get; set; }
        public int PV { get; set; }

        public int UV { get; set; }
        public int Clue { get; set; }
    }
}
