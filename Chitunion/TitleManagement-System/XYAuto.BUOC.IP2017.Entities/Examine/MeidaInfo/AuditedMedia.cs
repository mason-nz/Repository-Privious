using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.Entities.Examine.MeidaInfo
{
    /// <summary>
    /// zlb 2017-10-20
    /// 媒体已审核类
    /// </summary>
    public class AuditedMedia: MediaInfo
    {
        /// <summary>
        /// 是否自营
        /// </summary>
        public bool SelfDoBusiness { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime SubmitTime { get; set; }
        /// <summary>
        /// 提交人
        /// </summary>
        public string SubmitMan { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime ExamineTime { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string ExamineMan { get; set; }
    }
}
