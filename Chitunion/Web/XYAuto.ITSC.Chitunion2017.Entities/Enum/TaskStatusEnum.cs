/********************************************************
*创建人：lixiong
*创建时间：2017/8/30 15:20:51
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.Enum
{
    public enum TaskStatusEnum
    {
        [Description("待分配")]
        Pending = 95001,

        [Description("已分配")]
        Already = 95002,

        [Description("处理中")]
        Processing = 95003,

        [Description("已处理")]
        AlreadyProcessed = 95004,

        [Description("废弃/作废")]
        Useless = 95005,
    }
}