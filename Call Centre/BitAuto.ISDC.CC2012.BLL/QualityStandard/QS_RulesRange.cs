using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 业务逻辑类QS_RulesRange 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-04-25 09:42:37 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class QS_RulesRange
    {
        public static readonly QS_RulesRange Instance = new QS_RulesRange();

        protected QS_RulesRange()
        { }

        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetQS_RulesRange(QueryQS_RulesRange query, string order, int currentPage, int pageSize, out int totalCount)
        {
            int userid = Util.GetLoginUserID();
            return Dal.QS_RulesRange.Instance.GetQS_RulesRange(query, userid, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            int userid = Util.GetLoginUserID();
            return Dal.QS_RulesRange.Instance.GetQS_RulesRange(new QueryQS_RulesRange(), userid, string.Empty, 1, 1000000, out totalCount);
        }
        /// 编辑应用范围
        /// <summary>
        /// 编辑应用范围
        /// </summary>
        /// <param name="BGID"></param>
        /// <param name="QS_RTID"></param>
        /// <param name="QS_IM_RTID"></param>
        /// <returns></returns>
        public int RangeManage(int BGID, int QS_RTID, int QS_IM_RTID)
        {
            return Dal.QS_RulesRange.Instance.RangeManage(BGID, QS_RTID, QS_IM_RTID, BLL.Util.GetLoginUserID());
        }
        /// 根据组获取实体
        /// <summary>
        /// 根据组获取实体
        /// </summary>
        /// <param name="BGID"></param>
        /// <returns></returns>
        public Entities.QS_RulesRange getModelByBGID(int BGID)
        {
            return Dal.QS_RulesRange.Instance.getModelByBGID(BGID);
        }
    }
}

