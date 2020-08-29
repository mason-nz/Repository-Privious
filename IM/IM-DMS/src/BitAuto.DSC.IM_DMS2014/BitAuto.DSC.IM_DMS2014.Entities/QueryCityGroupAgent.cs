using System;
using BitAuto.DSC.IM_DMS2014.Entities.Constants;

namespace BitAuto.DSC.IM_DMS2014.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类QueryCityGroupAgent 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-10-29 10:21:00 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class QueryCityGroupAgent
    {
        /// 大区
        /// <summary>
        /// 大区
        /// </summary>
        public string DistrictID { get; set; }
        /// 城市群
        /// <summary>
        /// 城市群
        /// </summary>
        public string CityGroupID { get; set; }
        /// 坐席
        /// <summary>
        /// 坐席
        /// </summary>
        public string UserID { get; set; }
        /// 无坐席
        /// <summary>
        /// 无坐席
        /// </summary>
        public string IsHave { get; set; }
    }
}

