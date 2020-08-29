/********************************************************
*创建人：hant
*创建时间：2017/12/18 14:14:33 
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
    public class TaskList
    {
        public int TaskID { get; set; }
        public string Title { get; set; }
        public int CategoryID { get; set; }
        public string ImgUrl { get; set; }
        public decimal CPCPrice { get; set; }
        public decimal CPLPrice { get; set; }
        public int TaskType { get; set; }
        public string BeginTime { get; set; }
        public string EndTime { get; set; }
        public int Status { get; set; }
        public int TakeCount { get; set; }
        public int RuleCount { get; set; }
        public DateTime CreateTime { get; set; }

    }
}
