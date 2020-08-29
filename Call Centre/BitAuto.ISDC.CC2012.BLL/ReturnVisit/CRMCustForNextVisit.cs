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
    /// 业务逻辑类CRMCustForNextVisit 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-04-17 10:45:55 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CRMCustForNextVisit
    {
        #region Instance
        public static readonly CRMCustForNextVisit Instance = new CRMCustForNextVisit();
        #endregion

        #region Contructor
        protected CRMCustForNextVisit()
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
        public DataTable GetCRMCustForNextVisit(QueryCRMCustForNextVisit query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.CRMCustForNextVisit.Instance.GetCRMCustForNextVisit(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.CRMCustForNextVisit.Instance.GetCRMCustForNextVisit(new QueryCRMCustForNextVisit(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.CRMCustForNextVisit GetCRMCustForNextVisit(string CrmCustID, int userid)
        {
            return Dal.CRMCustForNextVisit.Instance.GetCRMCustForNextVisit(CrmCustID, userid);
        }

        #endregion

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Insert(Entities.CRMCustForNextVisit model)
        {
            Dal.CRMCustForNextVisit.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Insert(SqlTransaction sqltran, Entities.CRMCustForNextVisit model)
        {
            Dal.CRMCustForNextVisit.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.CRMCustForNextVisit model)
        {
            return Dal.CRMCustForNextVisit.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.CRMCustForNextVisit model)
        {
            return Dal.CRMCustForNextVisit.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string CrmCustID, int userid)
        {

            return Dal.CRMCustForNextVisit.Instance.Delete(CrmCustID, userid);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, string CrmCustID, int userid)
        {

            return Dal.CRMCustForNextVisit.Instance.Delete(sqltran, CrmCustID, userid);
        }

        #endregion

        /// 清空错误数据
        /// <summary>
        /// 清空错误数据
        /// </summary>
        public void ClearErrorDataByCust()
        {
            Dal.CRMCustForNextVisit.Instance.ClearErrorDataByCust();
        }

    }
}

