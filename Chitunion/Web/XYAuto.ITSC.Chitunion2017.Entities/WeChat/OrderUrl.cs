﻿/********************************************************
*创建人：hant
*创建时间：2018/1/25 17:27:16 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.WeChat
{
    public class OrderUrl
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public Result Result { get; set; }

    }
    public class Result
    {
        public string OrderUrl { get; set; }
        public int OrderId { get; set; }

        public string PasterUrl { get; set; }
    }
}
