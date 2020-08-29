/********************************************************
*创建人：lixiong
*创建时间：2017/11/28 15:48:50
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
    public enum GetDataViewTypeEnum
    {
        [Description("头部文章抓取")]
        grab_head,

        [Description("头部文章机洗入库")]
        jx_head,

        [Description("腰部文章抓取")]
        grab_body,

        [Description("腰部文章机洗入库")]
        jx_body,

        [Description("腰部文章车型匹配")]
        cxpp_body,

        [Description("物料封装")]
        wlfz,

        [Description("物料分发	")]
        wlff,

        [Description("转发	")]
        zf,

        [Description("线索获取")]
        xshq
    }
}