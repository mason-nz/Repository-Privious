/********************************************************
*创建人：lixiong
*创建时间：2017/11/27 14:26:29
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Enum
{
    public enum CarMatchTypeEnum
    {
        [Description("已匹配车型")]
        Yes = 1,

        [Description("未匹配车型")]
        No = 2
    }
}