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
    /// 业务逻辑类QS_RulesTable 的摘要说明。
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
    public class QS_RulesTable
    {
        #region Instance
        public static readonly QS_RulesTable Instance = new QS_RulesTable();
        #endregion

        #region Contructor
        protected QS_RulesTable()
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
        public DataTable GetQS_RulesTable(QueryQS_RulesTable query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.QS_RulesTable.Instance.GetQS_RulesTable(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.QS_RulesTable.Instance.GetQS_RulesTable(new QueryQS_RulesTable(), string.Empty, 1, -1, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.QS_RulesTable GetQS_RulesTable(int QS_RTID)
        {

            return Dal.QS_RulesTable.Instance.GetQS_RulesTable(QS_RTID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByQS_RTID(int QS_RTID)
        {
            QueryQS_RulesTable query = new QueryQS_RulesTable();
            query.QS_RTID = QS_RTID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetQS_RulesTable(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.QS_RulesTable model)
        {
            return Dal.QS_RulesTable.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.QS_RulesTable model)
        {
            return Dal.QS_RulesTable.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.QS_RulesTable model)
        {
            return Dal.QS_RulesTable.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.QS_RulesTable model)
        {
            return Dal.QS_RulesTable.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int QS_RTID)
        {

            return Dal.QS_RulesTable.Instance.Delete(QS_RTID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int QS_RTID)
        {

            return Dal.QS_RulesTable.Instance.Delete(sqltran, QS_RTID);
        }

        #endregion

        /// <summary>
        /// 根据评分表ID取评分表明细，返回Dataset包括五个DataTable 分别是评分分类，质检项目，质检标准，扣分项，致命项
        /// </summary>
        /// <returns></returns>
        public DataSet GetRulesTableDetailByQS_RTID(int QS_RTID)
        {
            return Dal.QS_RulesTable.Instance.GetRulesTableDetailByQS_RTID(QS_RTID);
        }

        /// <summary>
        /// 根据评分表ID,质检成绩ID取评分表明细，返回Dataset包括五个DataTable 分别是评分分类，质检项目，质检标准，扣分项，致命项
        /// </summary>
        /// <returns></returns>
        public DataSet GetRulesTableDetailByQS_RTID(int QS_RTID, int QS_RID)
        {
            return Dal.QS_RulesTable.Instance.GetRulesTableDetailByQS_RTID(QS_RTID, QS_RID);
        }

         /// <summary>
        /// 根据RTID获取评分表类型
        /// </summary>
        /// <param name="rtid"></param>
        /// <returns></returns>
        public string GetScoreTypeByRTID(int rtid)
        {
            return Dal.QS_RulesTable.Instance.GetScoreTypeByRTID(rtid);
        }

        /// <summary>
        /// 判断指定评分表的名称是否已经存在（指定评分表类型）
        /// </summary>
        /// <param name="name"></param>
        /// <param name="scoreType"></param>
        /// <returns></returns>
        public bool IsRuleTableNameExist(string name, int scoreType)
        {
            return Dal.QS_RulesTable.Instance.IsRuleTableNameExist(name, scoreType);
        }
    }
}

