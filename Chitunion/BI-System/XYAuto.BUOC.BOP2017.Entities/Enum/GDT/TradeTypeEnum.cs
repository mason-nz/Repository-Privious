/********************************************************
*创建人：lixiong
*创建时间：2017/10/9 11:31:35
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.BOP2017.Entities.Enum.GDT
{
    public enum TradeTypeEnum
    {
        [Description("充值")]
        CHARGE = 84001,

        [Description("消费")]
        PAY = 84002,

        [Description("回划")]
        TRANSFER_BACK = 84003,

        [Description("过期")]
        EXPIRE = 84004,
    }
}