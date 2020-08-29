using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.ISDC.CC2012.Entities.CallReport
{
    public class QueryRoutepoint
    {
        /// <summary>
        /// 业务类型
        /// </summary>
        public string BusinessType { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 统计方式
        /// </summary>
        public string ShowTime { get; set; }
    }
}
