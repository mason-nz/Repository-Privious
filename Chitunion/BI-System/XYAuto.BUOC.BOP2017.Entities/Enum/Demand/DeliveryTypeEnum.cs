/********************************************************
*创建人：lixiong
*创建时间：2017/9/30 11:45:04
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.BOP2017.Entities.Enum.Demand
{
    public enum DeliveryTypeEnum
    {
        [Description("投放平台-广点通")]
        GDT = 95001,

        [Description("投放平台-今日头条")]
        TouTiao = 95002,
    }
}