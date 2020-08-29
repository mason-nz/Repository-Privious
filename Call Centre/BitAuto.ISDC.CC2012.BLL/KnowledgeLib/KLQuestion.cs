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
    /// 业务逻辑类KLQuestion 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-08-21 10:19:08 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class KLQuestion
    {
        #region Instance
        public static readonly KLQuestion Instance = new KLQuestion();
        #endregion

        #region Contructor
        protected KLQuestion()
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
        public DataTable GetKLQuestion(QueryKLQuestion query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.KLQuestion.Instance.GetKLQuestion(query, order, currentPage, pageSize, out totalCount);
        }

        public DataTable GetKLQuestion(QueryKnowledgeLib query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.KLQuestion.Instance.GetKLQuestion(query, order, currentPage, pageSize, out totalCount);
        }

        public DataTable GetKLQuestionManage(QueryKLQuestion query, string order, int currentPage, int pageSize, string wherePlus, out int totalCount)
        {
            return Dal.KLQuestion.Instance.GetKLQuestionMnage(query, order, currentPage, pageSize, wherePlus, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.KLQuestion.Instance.GetKLQuestion(new QueryKLQuestion(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.KLQuestion GetKLQuestion(long KLQID)
        {

            return Dal.KLQuestion.Instance.GetKLQuestion(KLQID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByKLQID(long KLQID)
        {
            QueryKLQuestion query = new QueryKLQuestion();
            query.KLQID = KLQID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetKLQuestion(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.KLQuestion model)
        {
            return Dal.KLQuestion.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.KLQuestion model)
        {
            return Dal.KLQuestion.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.KLQuestion model)
        {
            return Dal.KLQuestion.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.KLQuestion model)
        {
            return Dal.KLQuestion.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long KLQID)
        {

            return Dal.KLQuestion.Instance.Delete(KLQID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long KLQID)
        {

            return Dal.KLQuestion.Instance.Delete(sqltran, KLQID);
        }

        #endregion

        /// <summary>
        /// 此试题是否已经使用
        /// </summary>
        /// <param name="KLQID"></param>
        /// <returns></returns>
        public bool IsUsed(long KLQID)
        {
            return Dal.KLQuestion.Instance.IsUsed(KLQID);
        }

        /// <summary>
        /// 根据试题IDs获取试题
        /// </summary>
        /// <param name="SmallQIDs"></param>
        /// <param name="QustionType"></param>
        /// <param name="order"></param>
        /// <param name="p_2"></param>
        /// <param name="PageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetQuestionByIDs(string KCID, string QustionName, string SmallQIDs, string QustionType, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.KLQuestion.Instance.GetQuestionByIDs(KCID, QustionName, SmallQIDs, QustionType, order, currentPage, pageSize, Util.GetLoginUserID(),out totalCount);
        }
    }
}

