/********************************************************
*创建人：lixiong
*创建时间：2017/8/14 15:07:16
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Enum
{
    /// <summary>
    /// 资金状态 | fund_status
    /// </summary>
    public enum GdtFundStatusEnum
    {
        [Description("有效")]
        FUND_STATUS_NORMAL,

        [Description("余额不足")]
        FUND_STATUS_NOT_ENOUGH,

        [Description("资金冻结")]
        FUND_STATUS_FROZEN
    }
}