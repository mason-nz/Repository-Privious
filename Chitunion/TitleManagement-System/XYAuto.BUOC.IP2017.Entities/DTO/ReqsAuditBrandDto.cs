using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.Entities.DTO
{
    public class ReqsAuditBrandDto
    {
        /// <summary>
        /// 品牌类型（1:主品牌 2：子品牌）
        /// </summary>
        public int BrandType { get; set; }
        /// <summary>
        /// 子品牌或车系ID（-2:全部）
        /// </summary>
        public int BrandId { get; set; }
        /// <summary>
        /// 提交开始日期
        /// </summary>
        public string StartDate { get; set; } = "";
        /// <summary>
        /// 提交结束日期
        /// </summary>
        public string EndDate { get; set; } = "";
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
