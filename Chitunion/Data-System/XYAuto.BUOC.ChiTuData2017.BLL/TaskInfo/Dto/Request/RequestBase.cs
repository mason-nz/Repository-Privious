﻿/********************************************************
*创建人：hant
*创建时间：2017/12/19 14:00:48 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.BLL.TaskInfo.Dto.Request
{
    public class RequestBase
    {
        public string appId { get; set; }
        public string appkey { get; set; }
        public string sign { get; set; }
        public string version { get; set; }
        public string timestamp { get; set; }
    }
}
