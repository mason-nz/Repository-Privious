/********************************************************
*创建人：lixiong
*创建时间：2017/9/13 13:25:37
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Enum
{
    public enum MaterielClickTypeEnum
    {
        [Description("点击PV明细")]
        ClickPv = 6001,

        [Description("点击UV明细")]
        ClickUv = 6002,

        [Description("转发明细")]
        Share = 6003,
    }
}