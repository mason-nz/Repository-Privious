/********************************************************
*创建人：hant
*创建时间：2017/12/19 15:59:43 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.BLL.TaskInfo.Dto.Response
{
    public class ResponseTaskMaterialList
    {
        public string Title { get; set; }
        public string MaterielUrl { get; set; }

        public Head Head { get; set; }

        public List<Waist> Waist { get; set; }

        public Foot Foot { get; set; }
    }

    public class Head
    {
        public string Content { get; set; }
    }

    public class Waist
    {
        public string Title { get; set; }
        public string Headimg { get; set; }
        public string Content { get; set; }
    }

    public class Foot
    {
        public string Content { get; set; }
    }
}
