/********************************************************
*创建人：hant
*创建时间：2018/1/12 13:00:13 
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
    public class TaskResDTO
    {
        public string OpenID { get; set; }
        public int PageIndex { get; set; } = -2;
        public int PageSize { get; set; } = -2;
        public int SceneID { get; set; }
        public int UserID { get; set; }
        /// <summary>
        /// true：PageIndex传任务列表每一页的第一个ID
        /// false:PageIndex传任务列表上一页的最后一个ID
        /// </summary>
        public bool isFirstIdOnPage { get; set; } = false;

        public bool IsNewUser { get; set; }

        public string BeginTime { get; set; }
    }
}
