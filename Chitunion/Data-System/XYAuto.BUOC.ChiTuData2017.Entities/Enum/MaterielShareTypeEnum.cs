/********************************************************
*创建人：lixiong
*创建时间：2017/9/13 13:28:47
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Enum
{
    public enum MaterielShareTypeEnum
    {
        [Description("微信")]
        WeChat = 4001,

        [Description("微信朋友圈")]
        WeChatFriends = 4002,

        [Description("QQ")]
        QQ = 4003
    }
}