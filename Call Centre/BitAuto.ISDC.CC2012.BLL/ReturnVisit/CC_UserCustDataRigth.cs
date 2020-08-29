using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class CC_UserCustDataRigth
    {
        #region Instance
        public static readonly CC_UserCustDataRigth Instance = new CC_UserCustDataRigth();
        #endregion

        #region Contructor
        protected CC_UserCustDataRigth()
        {
        }
        #endregion
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="custUserMappingQuery">查询条件</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="totalCount">总行数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns>集合</returns>
        public DataTable GetCustUserMappingByUserID(QueryCC_CustUserMapping custUserMappingQuery, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.CC_UserCustDataRigth.Instance.GetCustUserMappingByUserID(custUserMappingQuery, order, currentPage, pageSize, out totalCount, BLL.Util.GetLoginUserID());
        }
        /// <summary>
        /// 统计客服各个标签下的数据
        /// </summary>
        /// <param name="custUserMappingQuery"></param>
        /// <param name="currentUserID"></param>
        /// <returns></returns>
        public DataTable GetCustUserMappingTagStatisticsByUserID(QueryCC_CustUserMapping custUserMappingQuery, int currentUserID)
        {
            return Dal.CC_UserCustDataRigth.Instance.GetCustUserMappingTagStatisticsByUserID(custUserMappingQuery, currentUserID);
        }
    }
}
