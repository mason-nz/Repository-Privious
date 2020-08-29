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
    /// 业务逻辑类ExamBigQuestion 的摘要说明。
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
    public class ExamBigQuestion
    {
        #region Instance
        public static readonly ExamBigQuestion Instance = new ExamBigQuestion();
        #endregion

        #region Contructor
        protected ExamBigQuestion()
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
        public DataTable GetExamBigQuestion(QueryExamBigQuestion query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ExamBigQuestion.Instance.GetExamBigQuestion(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.ExamBigQuestion.Instance.GetExamBigQuestion(new QueryExamBigQuestion(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ExamBigQuestion GetExamBigQuestion(long BQID)
        {

            return Dal.ExamBigQuestion.Instance.GetExamBigQuestion(BQID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByBQID(long BQID)
        {
            QueryExamBigQuestion query = new QueryExamBigQuestion();
            query.BQID = BQID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetExamBigQuestion(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.ExamBigQuestion model)
        {
            return Dal.ExamBigQuestion.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ExamBigQuestion model)
        {
            return Dal.ExamBigQuestion.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.ExamBigQuestion model)
        {
            return Dal.ExamBigQuestion.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ExamBigQuestion model)
        {
            return Dal.ExamBigQuestion.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long BQID)
        {

            return Dal.ExamBigQuestion.Instance.Delete(BQID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long BQID)
        {

            return Dal.ExamBigQuestion.Instance.Delete(sqltran, BQID);
        }

        #endregion

        /// <summary>
        /// 根据试卷ID得到一个对象实体List add by qizq 2012-9-3
        /// </summary>
        public List<Entities.ExamBigQuestion> GetExamBigQuestionList(long EPID)
        {
            return Dal.ExamBigQuestion.Instance.GetExamBigQuestionList(EPID);
        }
        ///add by qizq 2012-9-11
        /// <summary>
        /// 根据试卷id,题型，判断是否有该题型
        /// </summary>
        /// <returns></returns>
        public bool HaveAskCategoryByEPID(string epid, int askcategory)
        {
            DataTable dt = null;
            dt = Dal.ExamBigQuestion.Instance.HaveAskCategoryByEPID(epid, askcategory);
            if (dt != null && dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}

