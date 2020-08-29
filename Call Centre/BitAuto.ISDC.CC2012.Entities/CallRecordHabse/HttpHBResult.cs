using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.ISDC.CC2012.Entities
{
    public class HttpHBResult
    {
        /// 返回结果
        /// <summary>
        /// 返回结果
        /// </summary>
        public bool Result { get; set; }
        /// 查询耗时
        /// <summary>
        /// 查询耗时
        /// </summary>
        public long QueryMilliseconds { get; set; }
        /// 错误原因
        /// <summary>
        /// 错误原因
        /// </summary>
        public string Error { get; set; }
        /// 查询数据
        /// <summary>
        /// 查询数据
        /// </summary>
        public string Data { get; set; }
    }
}
