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
    /// 业务逻辑类StopCustApply 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-07-01 12:21:55 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class StopCustApply
    {
        #region Instance
        public static readonly StopCustApply Instance = new StopCustApply();
        #endregion

        #region Contructor
        protected StopCustApply()
        { }
        #endregion

        #region Select
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetStopCustApply(QueryStopCustApply query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.StopCustApply.Instance.GetStopCustApply(query, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// 无数据权限查询
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetStopCustApplyNoDR(QueryStopCustApply query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.StopCustApply.Instance.GetStopCustApplyNoDR(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.StopCustApply.Instance.GetStopCustApply(new QueryStopCustApply(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.StopCustApply GetStopCustApply(int RecID)
        {
            return Dal.StopCustApply.Instance.GetStopCustApply(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByRecID(int RecID)
        {
            QueryStopCustApply query = new QueryStopCustApply();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetStopCustApply(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.StopCustApply model)
        {
            return Dal.StopCustApply.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.StopCustApply model)
        {
            return Dal.StopCustApply.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.StopCustApply model)
        {
            return Dal.StopCustApply.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.StopCustApply model)
        {
            return Dal.StopCustApply.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {

            return Dal.StopCustApply.Instance.Delete(RecID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {

            return Dal.StopCustApply.Instance.Delete(sqltran, RecID);
        }

        #endregion

        public Entities.StopCustApply GetStopCustApplyByCrmStopCustApplyID(int CRMStopCustApplyID)
        {
            return Dal.StopCustApply.Instance.GetStopCustApplyByCrmStopCustApplyID(CRMStopCustApplyID);
        }

    }
}

