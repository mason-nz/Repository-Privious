/********************************************************
*创建人：hant
*创建时间：2018/1/12 13:01:49 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V2_3
{
    public class TaskRspDTO
    {
        public int TotalCount { get; set; }
        public int TaskID { get; set; } = -2;

        public List<TaskInfo> TaskInfo { get; set; }
    }

    public class TaskInfo
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string MaterialUrl { get; set; }
        public int MaterialID { get; set; } = -2;
        public string BillingRuleName { get; set; }
        public string Synopsis { get; set; }
        public string ImgUrl { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalPrice { get; set; }
        public int IsForward { get; set; }

        public decimal CPCPrice { get; set; }
        public string HeadImg2 { get; set; } = string.Empty;
        public string HeadImg3 { get; set; } = string.Empty;
        public int ReadCount { get; set; } = 0;
    }
}
