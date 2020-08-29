using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    /// <summary>
    /// 查询媒体详情、字典、验证存在参数
    /// </summary>
    public class QueryMediaParamsDTO
    {
        /// <summary>
        /// 媒体类型
        /// </summary>
        public MediaTypeEnum MediaType { get; set; }
        /// <summary>
        /// 媒体ID
        /// </summary>
        public int MediaID { get; set; }

        //查询字典和验证存在用以下两个属性
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Number { get; set; }
    }
}
