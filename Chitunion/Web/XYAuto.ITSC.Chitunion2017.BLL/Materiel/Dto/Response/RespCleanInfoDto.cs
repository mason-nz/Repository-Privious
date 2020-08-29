/********************************************************
*创建人：lixiong
*创建时间：2017/8/31 10:28:03
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.Materiel;

namespace XYAuto.ITSC.Chitunion2017.BLL.Materiel.Dto.Response
{
    public class RespCleanInfoDto
    {
        public TaskSchedulerDto Head { get; set; }
        public List<TaskSchedulerDto> Body { get; set; }
    }
}