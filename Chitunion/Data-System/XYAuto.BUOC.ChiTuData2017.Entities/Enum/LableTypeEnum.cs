/********************************************************
*创建人：lixiong
*创建时间：2017/9/12 19:26:38
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
    /// <summary>
    /// 此枚举属于外部系统的
    /// </summary>
    public enum LableTypeEnum
    {
        //[Description("分类")]
        //Classification = 65001,

        //[Description("场景")]
        //Scene = 65001,

        [Description("IP")]
        Ip = 6001,

        [Description("子IP")]
        ChildIp = 6002,

        [Description("标签")]
        Lable = 6003,
    }

    /// <summary>
    /// 头部文章内容类型
    /// </summary>
    public enum HeadContentTypeEnum
    {
        图文 = 66001
    }

    public enum BodyContentTypeEnum
    {
        视频 = 67001,
        信息流 = 67002
    }
}