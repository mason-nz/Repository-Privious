﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.NUnitTest.ChituAPP
{
    [Serializable]
    [DataContract]
    public class JsonResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        [DataMember]
        public int Status { get; set; }

        /// <summary>
        /// 返回状态值的说明
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// 返回Json对象
        /// </summary>
        [DataMember]
        public object Result { get; set; }

        /// <summary>
        /// 根据授权Cookies，判断当前用户是否过期
        /// </summary>
        [DataMember]
        public bool? IsOverdue { get; set; }
    }
}
