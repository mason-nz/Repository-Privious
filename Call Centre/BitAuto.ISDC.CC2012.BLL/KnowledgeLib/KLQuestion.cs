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
    /// ҵ���߼���KLQuestion ��ժҪ˵����
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
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
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
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.KLQuestion.Instance.GetKLQuestion(new QueryKLQuestion(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.KLQuestion GetKLQuestion(long KLQID)
        {

            return Dal.KLQuestion.Instance.GetKLQuestion(KLQID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
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
        /// ����һ������
        /// </summary>
        public int Insert(Entities.KLQuestion model)
        {
            return Dal.KLQuestion.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.KLQuestion model)
        {
            return Dal.KLQuestion.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.KLQuestion model)
        {
            return Dal.KLQuestion.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.KLQuestion model)
        {
            return Dal.KLQuestion.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(long KLQID)
        {

            return Dal.KLQuestion.Instance.Delete(KLQID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, long KLQID)
        {

            return Dal.KLQuestion.Instance.Delete(sqltran, KLQID);
        }

        #endregion

        /// <summary>
        /// �������Ƿ��Ѿ�ʹ��
        /// </summary>
        /// <param name="KLQID"></param>
        /// <returns></returns>
        public bool IsUsed(long KLQID)
        {
            return Dal.KLQuestion.Instance.IsUsed(KLQID);
        }

        /// <summary>
        /// ��������IDs��ȡ����
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

