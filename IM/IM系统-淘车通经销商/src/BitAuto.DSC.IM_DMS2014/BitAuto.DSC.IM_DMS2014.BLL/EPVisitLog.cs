using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_DMS2014.Entities;
using BitAuto.DSC.IM_DMS2014.Entities.Constants;

namespace BitAuto.DSC.IM_DMS2014.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 业务逻辑类EPVisitLog 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-10-29 10:21:03 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class EPVisitLog : CommonBll
    {
        public static readonly new EPVisitLog Instance = new EPVisitLog();

        protected EPVisitLog()
        { }

        /// 按照查询条件查询
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetEPVisitLog(QueryEPVisitLog query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.EPVisitLog.Instance.GetEPVisitLog(query, order, currentPage, pageSize, out totalCount);
        }
        /// 获得数据列表
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.EPVisitLog.Instance.GetEPVisitLog(new QueryEPVisitLog(), string.Empty, 1, 1000000, out totalCount);
        }

        public DataTable GetDMSMemberByProvince(string provinceid)
        {
            return Dal.EPVisitLog.Instance.GetDMSMemberByProvince(provinceid);
        }
    }
}

