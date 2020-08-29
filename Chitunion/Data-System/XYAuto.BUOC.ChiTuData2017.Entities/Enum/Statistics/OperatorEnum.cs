/********************************************************
*创建人：lixiong
*创建时间：2017/11/23 20:16:40
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
    public enum OperatorEnum
    {
        无 = Entities.Constants.Constant.INT_INVALID_VALUE,

        [Description("=")]
        等于,

        [Description("<=")]
        小于等于,

        [Description("<")]
        小于,

        [Description(">=")]
        大于等于,

        [Description(">")]
        大于,

        [Description("<>")]
        不等于,

        [Description(" IS NOT NULL ")]
        不等于NULL,

        [Description(" IS NULL ")]
        等于NULL,
    }
}