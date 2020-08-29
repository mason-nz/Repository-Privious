using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.DSC.IM_2015.Entities
{
    public class QueryAgentInfo
    {
        /// <summary>
        /// 区域
        /// </summary>
        public int RegionID { get; set; }
        /// <summary>
        /// 组
        /// </summary>
        public string BGIDs { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 城市群列表
        /// </summary>
        public string CityGroups { get; set; }
    }
}
