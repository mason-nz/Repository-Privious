/********************************************************
*创建人：lixiong
*创建时间：2017/9/9 12:49:45
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
    public enum DistributeTypeEnum
    {
        //[Description("青鸟")]
        //QingNiao = 69001,

        [Description("全网域")]
        QuanWangYu = 73001,

        [Description("经纪人系统")]
        QingNiaoAgent = 73002,
    }

    public enum DistributeDataTypeEnum
    {
        [Description("日结汇总数据")]
        DailySummary = 1,

        [Description("日结详情")]
        SummaryDetails = 2,
    }
}