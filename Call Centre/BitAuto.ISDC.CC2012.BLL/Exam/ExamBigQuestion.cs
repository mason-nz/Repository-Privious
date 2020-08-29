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
    /// ҵ���߼���ExamBigQuestion ��ժҪ˵����
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
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetExamBigQuestion(QueryExamBigQuestion query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ExamBigQuestion.Instance.GetExamBigQuestion(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.ExamBigQuestion.Instance.GetExamBigQuestion(new QueryExamBigQuestion(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.ExamBigQuestion GetExamBigQuestion(long BQID)
        {

            return Dal.ExamBigQuestion.Instance.GetExamBigQuestion(BQID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
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
        /// ����һ������
        /// </summary>
        public int Insert(Entities.ExamBigQuestion model)
        {
            return Dal.ExamBigQuestion.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ExamBigQuestion model)
        {
            return Dal.ExamBigQuestion.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.ExamBigQuestion model)
        {
            return Dal.ExamBigQuestion.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ExamBigQuestion model)
        {
            return Dal.ExamBigQuestion.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(long BQID)
        {

            return Dal.ExamBigQuestion.Instance.Delete(BQID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, long BQID)
        {

            return Dal.ExamBigQuestion.Instance.Delete(sqltran, BQID);
        }

        #endregion

        /// <summary>
        /// �����Ծ�ID�õ�һ������ʵ��List add by qizq 2012-9-3
        /// </summary>
        public List<Entities.ExamBigQuestion> GetExamBigQuestionList(long EPID)
        {
            return Dal.ExamBigQuestion.Instance.GetExamBigQuestionList(EPID);
        }
        ///add by qizq 2012-9-11
        /// <summary>
        /// �����Ծ�id,���ͣ��ж��Ƿ��и�����
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

