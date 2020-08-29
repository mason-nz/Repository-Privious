/********************************************************
*创建人：lixiong
*创建时间：2017/11/28 13:33:30
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
    public enum GetArtificialTypeEnum
    {
        [Description("人工清洗-头部-左边 嵌套环形")]
        rgqx_head_pie_nest,

        [Description("人工清洗-头部-右边 正负条形图")]
        rgqx_head_bar_brush,

        [Description("人工清洗-腰部-左边 嵌套环形")]
        rgqx_body_pie_nest,

        [Description("人工清洗-腰部-右边 正负条形图")]
        rgqx_body_bar_brush,

        [Description("清洗头部文章在场景上的分布")]
        rgqx_head_wzcj,

        [Description("清洗头部号在场景上的分布")]
        rgqx_head_zhcj,

        [Description("清洗头部文章在文章分值上的分布")]
        rgqx_head_wzfz,

        [Description("清洗头部账号在账号分值上的分布")]
        rgqx_head_zhfz,

        [Description("清洗腰部文章在文章类别上的分布")]
        rgqx_body_wzlb,
    }
}