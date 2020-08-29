using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static XYAuto.BUOC.IP2017.Entities.ENUM.ENUM;

namespace XYAuto.BUOC.IP2017.Entities.DTO
{
    /// <summary>
    /// zlb 2017-10-19
    /// 查询媒体列表条件类
    /// </summary>
    public class ReqsAuditMediaDto
    {
        /// <summary>
        /// 媒体类型
        /// </summary>
        public int MediaType { get; set; }
        /// <summary>
        /// 分类ID
        /// </summary>
        public int DictId { get; set; } = -2;
        /// <summary>
        /// 提交开始日期
        /// </summary>
        public string StartDate { get; set; } = "";
        /// <summary>
        /// 提交结束日期
        /// </summary>
        public string EndDate { get; set; } = "";
        /// <summary>
        /// 媒体名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否自营（-2全部，0否，1是）
        /// </summary>
        public int SelfDoBusiness { get; set; } = -2;
        /// <summary>
        /// 页码
        /// </summary>
        public int PageSize { get; set; } = 20;
        /// <summary>
        /// 索引页
        /// </summary>
        public int PageIndex { get; set; } = 1;
    }
}
