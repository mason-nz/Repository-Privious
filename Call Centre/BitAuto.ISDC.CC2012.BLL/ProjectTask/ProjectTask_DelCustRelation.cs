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
	/// ҵ���߼���ProjectTask_DelCustRelation ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:31 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ProjectTask_DelCustRelation
	{
		#region Instance
        public static readonly ProjectTask_DelCustRelation Instance = new ProjectTask_DelCustRelation();
        #endregion

        #region Contructor
        protected ProjectTask_DelCustRelation()
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
        public DataTable GetProjectTask_DelCustRelation(QueryProjectTask_DelCustRelation query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectTask_DelCustRelation.Instance.GetProjectTask_DelCustRelation(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.ProjectTask_DelCustRelation.Instance.GetProjectTask_DelCustRelation(new QueryProjectTask_DelCustRelation(), string.Empty, 1, 2147483647, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.ProjectTask_DelCustRelation GetProjectTask_DelCustRelation(int RecID)
        {

            return Dal.ProjectTask_DelCustRelation.Instance.GetProjectTask_DelCustRelation(RecID);
        }
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.ProjectTask_DelCustRelation GetProjectTask_DelCustRelationByTID(string tid)
        {
            return Dal.ProjectTask_DelCustRelation.Instance.GetProjectTask_DelCustRelationByTID(tid);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByRecID(int RecID)
        {
            QueryProjectTask_DelCustRelation query = new QueryProjectTask_DelCustRelation();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectTask_DelCustRelation(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByTID(string tid)
        {
            QueryProjectTask_DelCustRelation query = new QueryProjectTask_DelCustRelation();
            query.PTID = tid;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectTask_DelCustRelation(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.ProjectTask_DelCustRelation model)
        {
            return Dal.ProjectTask_DelCustRelation.Instance.Insert(model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.ProjectTask_DelCustRelation model)
        {
            return Dal.ProjectTask_DelCustRelation.Instance.Update(model);
        }
        /// <summary>
        /// ����TID������CustID
        /// </summary>
        /// <param name="tid">TID</param>
        /// <param name="custid">CustID</param>
        public void UpdateCustIDByTID(string tid, string custid)
        {
            Entities.ProjectTask_DelCustRelation model = GetProjectTask_DelCustRelationByTID(tid);
            if (model != null)
            {
                model.CustID = custid;
                Update(model);
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int RecID)
        {
            return Dal.ProjectTask_DelCustRelation.Instance.Delete(RecID);
        }

        /// <summary>
        /// ���ݿͻ�ID��ɾ����������
        /// </summary>
        /// <param name="custID">�ͻ�ID</param>
        /// <returns></returns>
        public int DeleteByCustID(string custID)
        {
            return Dal.ProjectTask_DelCustRelation.Instance.DeleteByCustID(custID);
        }
        /// <summary>
        /// ��������ID��ɾ����������
        /// </summary>
        /// <param name="TID">����ID</param>
        /// <returns></returns>
        public int DeleteByTID(string tid)
        {
            return Dal.ProjectTask_DelCustRelation.Instance.DeleteByTID(tid);
        }
        #endregion

        /// <summary>
        /// ��������ID�����ر�ע��Ϣ
        /// </summary>
        /// <param name="tid">����ID</param>
        /// <returns>��ע��Ϣ</returns>
        public string GetRemarkByTID(string tid)
        {
            QueryProjectTask_DelCustRelation query = new QueryProjectTask_DelCustRelation();
            query.PTID = tid;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectTask_DelCustRelation(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return dt.Rows[0]["Remark"].ToString().Trim();
            }
            return string.Empty;
        }
	}
}

