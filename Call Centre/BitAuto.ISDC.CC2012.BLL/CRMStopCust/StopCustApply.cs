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
    /// ҵ���߼���StopCustApply ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-07-01 12:21:55 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class StopCustApply
    {
        #region Instance
        public static readonly StopCustApply Instance = new StopCustApply();
        #endregion

        #region Contructor
        protected StopCustApply()
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
        public DataTable GetStopCustApply(QueryStopCustApply query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.StopCustApply.Instance.GetStopCustApply(query, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// ������Ȩ�޲�ѯ
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetStopCustApplyNoDR(QueryStopCustApply query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.StopCustApply.Instance.GetStopCustApplyNoDR(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.StopCustApply.Instance.GetStopCustApply(new QueryStopCustApply(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.StopCustApply GetStopCustApply(int RecID)
        {
            return Dal.StopCustApply.Instance.GetStopCustApply(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByRecID(int RecID)
        {
            QueryStopCustApply query = new QueryStopCustApply();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetStopCustApply(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.StopCustApply model)
        {
            return Dal.StopCustApply.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.StopCustApply model)
        {
            return Dal.StopCustApply.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.StopCustApply model)
        {
            return Dal.StopCustApply.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.StopCustApply model)
        {
            return Dal.StopCustApply.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int RecID)
        {

            return Dal.StopCustApply.Instance.Delete(RecID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {

            return Dal.StopCustApply.Instance.Delete(sqltran, RecID);
        }

        #endregion

        public Entities.StopCustApply GetStopCustApplyByCrmStopCustApplyID(int CRMStopCustApplyID)
        {
            return Dal.StopCustApply.Instance.GetStopCustApplyByCrmStopCustApplyID(CRMStopCustApplyID);
        }

    }
}

