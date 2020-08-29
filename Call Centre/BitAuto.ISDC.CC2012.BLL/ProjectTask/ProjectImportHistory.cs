using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;using BitAuto.ISDC.CC2012.Entities;
using System.Data;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class ProjectImportHistory
    {
        #region Instance
        public static readonly ProjectImportHistory Instance = new ProjectImportHistory();
        #endregion
        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.ProjectImportHistory model)
        {
            return Dal.ProjectImportHistory.Instance.Insert(model);
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ProjectImportHistory model)
        {
            return Dal.ProjectImportHistory.Instance.Insert(sqltran, model);
        }
        #endregion

        public int Delete(SqlTransaction sqltran, int ProjectID)
        {
            return Dal.ProjectImportHistory.Instance.Delete(sqltran,ProjectID);
        }
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
        public DataTable GetProjectImportHistory(QueryProjectImportHistory query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectImportHistory.Instance.GetProjectImportHistory(query, order, currentPage, pageSize, out totalCount);
        }

        #endregion
    }
}
