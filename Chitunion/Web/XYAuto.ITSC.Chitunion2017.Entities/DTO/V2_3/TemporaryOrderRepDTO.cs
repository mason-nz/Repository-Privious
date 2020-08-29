/********************************************************
*创建人：hant
*创建时间：2018/1/26 14:44:34 
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
    public class TemporaryOrderRepDTO
    {
        public int TaskId { get; set; }
        public string OrderUrl { get; set; }

        public string TaskName { get; set; }

        public string Synopsis { get; set; }

        public string ImgUrl { get; set; }
    }
}
