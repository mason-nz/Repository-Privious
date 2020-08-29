using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.Entities.Examine.CarBrandInfo
{
    /// <summary>
    /// zlb 2017-10-20
    ///审核品牌类
    /// </summary>
    public class CarBrandInfo
    {
        /// <summary>
        /// 类型
        /// </summary>
        public int TaskType { get; set; }
        /// <summary>
        /// 审核批次
        /// </summary>
        public int BatchID { get; set; }
        /// <summary>
        /// 品牌或车系名称
        /// </summary>
        public string BrandName { get; set; }
        /// <summary>
        /// 所属品牌
        /// </summary>
        public string BrandType { get; set; }

    }
}
