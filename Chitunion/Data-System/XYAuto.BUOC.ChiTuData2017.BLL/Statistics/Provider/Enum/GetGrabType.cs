/********************************************************
*创建人：lixiong
*创建时间：2017/11/23 16:30:41
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
using Newtonsoft.Json.Converters;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Enum
{
    public enum GetGrabType
    {
        [Description("头部文章抓取")]
        grab_head,

        [Description("抓取的头部文章在渠道上的分布")]
        grab_head_qudao,

        [Description("抓取的头部文章在场景上的分布")]
        grab_head_wzcj,

        [Description("抓取的头部账号在场景上的分布")]
        grab_head_zhcj,

        [Description("腰部文章抓取")]
        grab_body,

        [Description("抓取的腰部文章在渠道上的分布")]
        grab_body_qudao,

        [Description("抓取的腰部文章在文章类别上的分布")]
        grab_body_wzfl
    }
}