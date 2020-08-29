using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace XYAuto.BUOC.IP2017.Entities.Examine.MeidaInfo
{
    /// <summary>
    /// zlb 2017-10-20
    /// 媒体待审核类
    /// </summary>
    public class PendingAuditMedia : MediaInfo
    {
        /// <summary>
        /// 提交数量
        /// </summary>
        public int SubmitCount { get; set; }
        /// <summary>
        /// 领取数量
        /// </summary>
        public int ReceiveCount { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime SubmitTime { get; set; }
    }
}
