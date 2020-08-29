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
    /// ҵ���߼���QS_ResultDetail ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-04-25 09:42:36 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class QS_ResultDetail
    {
        #region Instance
        public static readonly QS_ResultDetail Instance = new QS_ResultDetail();
        #endregion

        #region Contructor
        protected QS_ResultDetail()
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
        public DataTable GetQS_ResultDetail(QueryQS_ResultDetail query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.QS_ResultDetail.Instance.GetQS_ResultDetail(query, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.QS_ResultDetail.Instance.GetQS_ResultDetail(new QueryQS_ResultDetail(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.QS_ResultDetail GetQS_ResultDetail(int QS_RDID)
        {

            return Dal.QS_ResultDetail.Instance.GetQS_ResultDetail(QS_RDID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByQS_RDID(int QS_RDID)
        {
            QueryQS_ResultDetail query = new QueryQS_ResultDetail();
            query.QS_RDID = QS_RDID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetQS_ResultDetail(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.QS_ResultDetail model)
        {
            return Dal.QS_ResultDetail.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.QS_ResultDetail model)
        {
            return Dal.QS_ResultDetail.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.QS_ResultDetail model)
        {
            return Dal.QS_ResultDetail.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.QS_ResultDetail model)
        {
            return Dal.QS_ResultDetail.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int QS_RDID)
        {

            return Dal.QS_ResultDetail.Instance.Delete(QS_RDID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int QS_RDID)
        {

            return Dal.QS_ResultDetail.Instance.Delete(sqltran, QS_RDID);
        }


        /// <summary>
        /// �����ʼ�ɼ�������ɾ����ϸ���߼�ɾ��
        /// </summary>
        /// <returns></returns>
        public int DeleteByQS_RID(int QS_RID)
        {
            return Dal.QS_ResultDetail.Instance.DeleteByQS_RID(QS_RID);
        }

        #endregion

        /// <summary>
        /// ���ݳɼ���id��ȡ�÷���������������������
        /// </summary>
        /// <param name="scoretype"></param>
        /// <param name="QS_RID"></param>
        /// <returns></returns>
        public DataTable GetQS_ResultForCalculate(string scoretype, int QS_RID, int QS_RTID, string ccOrIM)
        {
            return Dal.QS_ResultDetail.Instance.GetQS_ResultForCalculate(scoretype, QS_RID, QS_RTID, ccOrIM);
        }        
        public DataTable getDetailsByExport_IM(string where)
        {
            return Dal.QS_ResultDetail.Instance.getDetailsByExport_IM(where);
        }

        #region ���²�ѯ
        /// �ɼ������鵼����ѯ
        /// <summary>
        /// �ɼ������鵼����ѯ
        /// </summary>
        /// <param name="where"></param>
        /// <param name="tableEndName"></param>
        /// <returns></returns>
        public DataTable getDetailsByExport(string where, string tableEndName)
        {
            return Dal.QS_ResultDetail.Instance.getDetailsByExport(where, tableEndName);
        }
        #endregion
    }
}

