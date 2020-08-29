/********************************************************
*创建人：lixiong
*创建时间：2017/11/27 14:28:13
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
    public enum GetCsTypeEnum
    {
        [Description("初筛-左边 嵌套环形")]
        cs_pie_nest,

        [Description("初筛-右边 正负条形图")]
        cs_bar_brush,

        [Description("初筛头部文章在场景上的分布")]
        cs_head_cj,

        [Description("初筛头部账号在场景上的分布")]
        cs_head_zhcj,

        [Description("初筛头部文章在文章分值上的分布")]
        cs_head_wzfz,

        [Description("初筛头部账号在账号分值上的分布")]
        cs_head_zhfz,

        [Description("文章初筛可用转化")]
        cs_head_wzzh,
    }
}