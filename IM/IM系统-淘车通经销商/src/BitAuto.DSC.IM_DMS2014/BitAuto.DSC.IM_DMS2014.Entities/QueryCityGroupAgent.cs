using System;
using BitAuto.DSC.IM_DMS2014.Entities.Constants;

namespace BitAuto.DSC.IM_DMS2014.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// ʵ����QueryCityGroupAgent ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
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
        /// ����
        /// <summary>
        /// ����
        /// </summary>
        public string DistrictID { get; set; }
        /// ����Ⱥ
        /// <summary>
        /// ����Ⱥ
        /// </summary>
        public string CityGroupID { get; set; }
        /// ��ϯ
        /// <summary>
        /// ��ϯ
        /// </summary>
        public string UserID { get; set; }
        /// ����ϯ
        /// <summary>
        /// ����ϯ
        /// </summary>
        public string IsHave { get; set; }
    }
}

