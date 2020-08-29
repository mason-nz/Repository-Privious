/********************************************************
*创建人：lixiong
*创建时间：2017/11/24 17:14:38
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Enum
{
    public enum GetCarMatchTypeEnum
    {
        [Description("车型匹配-左边 嵌套环形")]
        cxpp_pie_nest,

        [Description("车型匹配-右边 正负条形图")]
        cxpp_bar_brush,

        [Description("已匹配车型文章类别分布")]
        cxpp_yes,

        [Description("未匹配车型文章类别分布")]
        cxpp_no,
    }
}