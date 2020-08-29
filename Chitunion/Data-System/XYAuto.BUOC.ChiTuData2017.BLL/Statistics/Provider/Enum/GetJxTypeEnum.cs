/********************************************************
*创建人：lixiong
*创建时间：2017/11/24 13:26:52
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
    public enum GetJxTypeEnum
    {
        [Description("头部文章机洗入库")]
        jx_head,

        [Description("机洗入库的头部文章在渠道上的分布")]
        jx_head_qudao,

        [Description("头部文章机洗入库的转化")]
        jx_head_zh,

        [Description("机洗入库的头部文章在场景上的分布")]
        jx_head_wzcj,

        [Description("机洗入库的头部账号在场景上的分布")]
        jx_head_zhcj,

        [Description("机洗入库的头部文章在文章分值上的分布")]
        jx_head_wz_wzfz,

        [Description("机洗入库的头部账号在账号分值上的分布")]
        jx_head_zh_zhfz,

        [Description("腰部文章机洗入库")]
        jx_body,

        [Description("机洗入库的腰部文章在渠道上的分布")]
        jx_body_qudao,

        [Description("腰部文章机洗入库的转化")]
        jx_body_zh,

        [Description("机洗入库的腰部文章在文章类别上的分布")]
        jx_body_wzlb,

        [Description("机洗入库的腰部文章/账号在文章分值上的分布")]
        jx_body_fz
    }
}