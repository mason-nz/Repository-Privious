using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using System.Data.SqlClient;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 业务逻辑类UserActionLog 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-07-04 10:22:38 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class UserActionLog
    {
        #region Instance
        public static readonly UserActionLog Instance = new UserActionLog();
        #endregion

        #region Contructor
        protected UserActionLog()
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
        public DataTable GetUserActionLog(QueryUserActionLog query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.UserActionLog.Instance.GetUserActionLog(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.UserActionLog.Instance.GetUserActionLog(new QueryUserActionLog(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.UserActionLog GetUserActionLog(int RecID)
        {

            return Dal.UserActionLog.Instance.GetUserActionLog(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByRecID(int RecID)
        {
            QueryUserActionLog query = new QueryUserActionLog();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetUserActionLog(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.UserActionLog model)
        {
            return Dal.UserActionLog.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="sqltran">事务</param>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Insert(SqlTransaction sqltran, Entities.UserActionLog model)
        {
            return Dal.UserActionLog.Instance.Insert(sqltran,model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.UserActionLog model)
        {
            return Dal.UserActionLog.Instance.Update(model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {

            return Dal.UserActionLog.Instance.Delete(RecID);
        }

        #endregion

    }
}
