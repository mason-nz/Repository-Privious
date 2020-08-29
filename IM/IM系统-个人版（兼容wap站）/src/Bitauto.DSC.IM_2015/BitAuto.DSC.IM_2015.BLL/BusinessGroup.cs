using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BitAuto.DSC.IM_2015.BLL
{
    public class BusinessGroup
    {
        public static readonly BusinessGroup Instance = new BusinessGroup();
        private BusinessGroup() { }

        /// <summary>
        /// 根据userid获取业务组标签
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="OnlyByGroup">是否只是按所在分组查询数据</param>
        /// <param name="OnlyByGroup">是否显示停用的标签</param>
        /// <returns></returns>
        public DataTable GetBusinessGroupTagsByUserID(int UserID)
        {
            return Dal.BusinessGroup.Instance.GetBusinessGroupTagsByUserID(UserID);
        }

    }
}
