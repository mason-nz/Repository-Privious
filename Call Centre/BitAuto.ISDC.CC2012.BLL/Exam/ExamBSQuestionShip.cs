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
    /// ҵ���߼���ExamBSQuestionShip ��ժҪ˵����
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
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetExamBSQuestionShip(QueryExamBSQuestionShip query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ExamBSQuestionShip.Instance.GetExamBSQuestionShip(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.ExamBSQuestionShip.Instance.GetExamBSQuestionShip(new QueryExamBSQuestionShip(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.ExamBSQuestionShip GetExamBSQuestionShip(long BQID, long KLQID)
        {

            return Dal.ExamBSQuestionShip.Instance.GetExamBSQuestionShip(BQID, KLQID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
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
        /// ����һ������
        /// </summary>
        public void Insert(Entities.ExamBSQuestionShip model)
        {
            Dal.ExamBSQuestionShip.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Insert(SqlTransaction sqltran, Entities.ExamBSQuestionShip model)
        {
            Dal.ExamBSQuestionShip.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.ExamBSQuestionShip model)
        {
            return Dal.ExamBSQuestionShip.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ExamBSQuestionShip model)
        {
            return Dal.ExamBSQuestionShip.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(long BQID, long KLQID)
        {

            return Dal.ExamBSQuestionShip.Instance.Delete(BQID, KLQID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, long BQID, long KLQID)
        {

            return Dal.ExamBSQuestionShip.Instance.Delete(sqltran, BQID, KLQID);
        }

        #endregion

        /// <summary>
        /// ���ݴ���id�õ�һ������ʵ��List add by qizq 2012-9-3
        /// </summary>
        public List<Entities.ExamBSQuestionShip> GetExamBSQuestionShipList(long BQID)
        {
            return Dal.ExamBSQuestionShip.Instance.GetExamBSQuestionShipList(BQID);
        }

        /// <summary>
        /// ���ݴ���id�õ�һ��֪ʶ������DataTable add by qizq 2012-9-3
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

