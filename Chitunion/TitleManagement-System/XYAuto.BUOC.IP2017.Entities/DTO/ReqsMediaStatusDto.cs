using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.Entities.DTO
{
    public class ReqsMediaStatusDto
    {
        /// <summary>
        /// 审核批次ID
        /// </summary>
        public int BatchAuditID { get; set; }
        /// <summary>
        /// 审核状态（1002审核取消（待审核），1003（列表点击审核）审核中）
        /// </summary>
        public int ExamineStatus { get; set; }
    }
}
