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
    /// ҵ���߼���CRMCustForNextVisit ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-04-17 10:45:55 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CRMCustForNextVisit
    {
        #region Instance
        public static readonly CRMCustForNextVisit Instance = new CRMCustForNextVisit();
        #endregion

        #region Contructor
        protected CRMCustForNextVisit()
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
        public DataTable GetCRMCustForNextVisit(QueryCRMCustForNextVisit query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.CRMCustForNextVisit.Instance.GetCRMCustForNextVisit(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.CRMCustForNextVisit.Instance.GetCRMCustForNextVisit(new QueryCRMCustForNextVisit(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.CRMCustForNextVisit GetCRMCustForNextVisit(string CrmCustID, int userid)
        {
            return Dal.CRMCustForNextVisit.Instance.GetCRMCustForNextVisit(CrmCustID, userid);
        }

        #endregion

        #region Insert
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Insert(Entities.CRMCustForNextVisit model)
        {
            Dal.CRMCustForNextVisit.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Insert(SqlTransaction sqltran, Entities.CRMCustForNextVisit model)
        {
            Dal.CRMCustForNextVisit.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.CRMCustForNextVisit model)
        {
            return Dal.CRMCustForNextVisit.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.CRMCustForNextVisit model)
        {
            return Dal.CRMCustForNextVisit.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(string CrmCustID, int userid)
        {

            return Dal.CRMCustForNextVisit.Instance.Delete(CrmCustID, userid);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, string CrmCustID, int userid)
        {

            return Dal.CRMCustForNextVisit.Instance.Delete(sqltran, CrmCustID, userid);
        }

        #endregion

        /// ��մ�������
        /// <summary>
        /// ��մ�������
        /// </summary>
        public void ClearErrorDataByCust()
        {
            Dal.CRMCustForNextVisit.Instance.ClearErrorDataByCust();
        }

    }
}

