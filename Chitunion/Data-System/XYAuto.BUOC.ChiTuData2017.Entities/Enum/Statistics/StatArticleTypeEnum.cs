/********************************************************
*创建人：lixiong
*创建时间：2017/11/23 14:57:06
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Enum.Statistics
{
    public enum StatArticleTypeEnum
    {
        None = -2,

        [Description("头部文章")]
        Head = 74001,

        [Description("腰部文章")]
        Body = 74002
    }
}