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
    /// 业务逻辑类QS_Category 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-04-25 09:42:35 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class QS_Category
    {
        #region Instance
        public static readonly QS_Category Instance = new QS_Category();
        #endregion

        #region Contructor
        protected QS_Category()
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
        public DataTable GetQS_Category(QueryQS_Category query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.QS_Category.Instance.GetQS_Category(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获取分类名称并获取分类下的项目数
        /// </summary>
        /// <param name="QS_RTID"></param>
        /// <returns></returns>
        public DataTable GetQS_CategoryNameAndItemNum(int QS_RTID)
        {
            return Dal.QS_Category.Instance.GetQS_CategoryNameAndItemNum(QS_RTID);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.QS_Category.Instance.GetQS_Category(new QueryQS_Category(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.QS_Category GetQS_Category(int QS_CID)
        {

            return Dal.QS_Category.Instance.GetQS_Category(QS_CID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByQS_CID(int QS_CID)
        {
            QueryQS_Category query = new QueryQS_Category();
            query.QS_CID = QS_CID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetQS_Category(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.QS_Category model)
        {
            return Dal.QS_Category.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.QS_Category model)
        {
            return Dal.QS_Category.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.QS_Category model)
        {
            return Dal.QS_Category.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.QS_Category model)
        {
            return Dal.QS_Category.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int QS_CID)
        {

            return Dal.QS_Category.Instance.Delete(QS_CID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int QS_CID)
        {

            return Dal.QS_Category.Instance.Delete(sqltran, QS_CID);
        }

        #endregion

        /// <summary>
        /// 取总分数
        /// </summary>
        /// <param name="QS_RTID"></param>
        /// <returns></returns>
        public int GetSumScore(int QS_RTID)
        {
            return Dal.QS_Category.Instance.GetSumScore(QS_RTID);
        }

    }
}

