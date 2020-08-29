/********************************************************
*创建人：hant
*创建时间：2017/12/18 15:32:20 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace XYAuto.BUOC.ChiTuData2017.TaskAPI.Common
{
    [Serializable]
    [DataContract]
    public class JsonResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        [DataMember]
        public string code { get; set; }

        /// <summary>
        /// 返回状态值的说明
        /// </summary>
        [DataMember]
        public string msg { get; set; }

        /// <summary>
        /// 返回Json对象
        /// </summary>
        [DataMember]
        public object data { get; set; }
    }
}