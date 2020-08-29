﻿/********************************************************
*创建人：lixiong
*创建时间：2017/11/27 11:24:29
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Response.Grab;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Response.CarMatch
{
    /// <summary>
    /// 车型匹配，区分id
    /// </summary>
    public class StatKeyValueCarMatchData : StatKeyValueData
    {
        public int TypeMatchId { get; set; }
    }

    public class StatTypeData : StatData
    {
        public int TypeId { get; set; }
        public int TypeMatchId { get; set; }
    }
}