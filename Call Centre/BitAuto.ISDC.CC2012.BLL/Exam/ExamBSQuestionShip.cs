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
    /// 业务逻辑类ExamBSQuestionShip 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-03 02:04:15 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ExamBSQuestionShip
    {
        #region Instance
        public static readonly ExamBSQuestionShip Instance = new ExamBSQuestionShip();
        #endregion

        #region Contructor
        protected ExamBSQuestionShip()
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
        public DataTable GetExamBSQuestionShip(QueryExamBSQuestionShip query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ExamBSQuestionShip.Instance.GetExamBSQuestionShip(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.ExamBSQuestionShip.Instance.GetExamBSQuestionShip(new QueryExamBSQuestionShip(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ExamBSQuestionShip GetExamBSQuestionShip(long BQID, long KLQID)
        {

            return Dal.ExamBSQuestionShip.Instance.GetExamBSQuestionShip(BQID, KLQID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByBQIDAndKLQID(long BQID, long KLQID)
        {
            QueryExamBSQuestionShip query = new QueryExamBSQuestionShip();
            query.BQID = BQID;
            query.KLQID = KLQID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetExamBSQuestionShip(query, string.Empty, 1, 1, out count);
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
        public void Insert(Entities.ExamBSQuestionShip model)
        {
            Dal.ExamBSQuestionShip.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Insert(SqlTransaction sqltran, Entities.ExamBSQuestionShip model)
        {
            Dal.ExamBSQuestionShip.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.ExamBSQuestionShip model)
        {
            return Dal.ExamBSQuestionShip.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ExamBSQuestionShip model)
        {
            return Dal.ExamBSQuestionShip.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long BQID, long KLQID)
        {

            return Dal.ExamBSQuestionShip.Instance.Delete(BQID, KLQID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long BQID, long KLQID)
        {

            return Dal.ExamBSQuestionShip.Instance.Delete(sqltran, BQID, KLQID);
        }

        #endregion

        /// <summary>
        /// 根据大题id得到一个对象实体List add by qizq 2012-9-3
        /// </summary>
        public List<Entities.ExamBSQuestionShip> GetExamBSQuestionShipList(long BQID)
        {
            return Dal.ExamBSQuestionShip.Instance.GetExamBSQuestionShipList(BQID);
        }

        /// <summary>
        /// 根据大题id得到一个知识库试题DataTable add by qizq 2012-9-3
        /// </summary>
        public DataTable GetKLQuestionData(long BQID)
        {
            return Dal.ExamBSQuestionShip.Instance.GetKLQuestionData(BQID);
        }

        public void Delete(long BQID)
        {
             Dal.ExamBSQuestionShip.Instance.Delete(BQID);
        }
    }
}

