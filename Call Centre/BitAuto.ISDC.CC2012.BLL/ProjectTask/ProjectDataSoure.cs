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
    /// ҵ���߼���ProjectDataSoure ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-02-20 10:39:28 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ProjectDataSoure
    {
        #region Instance
        public static readonly ProjectDataSoure Instance = new ProjectDataSoure();
        #endregion

        #region Contructor
        protected ProjectDataSoure()
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
        public DataTable GetProjectDataSoure(QueryProjectDataSoure query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectDataSoure.Instance.GetProjectDataSoure(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.ProjectDataSoure.Instance.GetProjectDataSoure(new QueryProjectDataSoure(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.ProjectDataSoure GetProjectDataSoure(long PDSID)
        {

            return Dal.ProjectDataSoure.Instance.GetProjectDataSoure(PDSID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByPDSID(long PDSID)
        {
            QueryProjectDataSoure query = new QueryProjectDataSoure();
            query.PDSID = PDSID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectDataSoure(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.ProjectDataSoure model)
        {
            return Dal.ProjectDataSoure.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ProjectDataSoure model)
        {
            return Dal.ProjectDataSoure.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.ProjectDataSoure model)
        {
            return Dal.ProjectDataSoure.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ProjectDataSoure model)
        {
            return Dal.ProjectDataSoure.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(long PDSID)
        {

            return Dal.ProjectDataSoure.Instance.Delete(PDSID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, long PDSID)
        {

            return Dal.ProjectDataSoure.Instance.Delete(sqltran, PDSID);
        }

        #endregion


        public void DeleteByProjectID(SqlTransaction tran, int ProjectID)
        {
            Dal.ProjectDataSoure.Instance.DeleteByProjectID(tran, ProjectID);
        }

        public void UpdateStatusByProjectId(SqlTransaction tran, string status, int ProjectID)
        {
            Dal.ProjectDataSoure.Instance.UpdateStatusByProjectId(tran, status, ProjectID);
        }
        public void UpdateStatusByProjectId(SqlTransaction tran, string status, DataTable dt, int ProjectID)
        {
            Dal.ProjectDataSoure.Instance.UpdateStatusByProjectId(tran, status, dt, ProjectID);
        }

        /// <summary>
        /// ������ĿID,��ȡ�������ݵ�ID�͸���
        /// </summary>
        /// <param name="p"></param>
        /// <param name="DataCount"></param>
        /// <returns></returns>
        public string GetProjectDataSoureID(long ProjectID, out string DataCount, bool isReturn)
        {
            return Dal.ProjectDataSoure.Instance.GetProjectDataSoureID(ProjectID, out DataCount, isReturn);
        }

        public int GetDataCountByProjectID(long projectID)
        {
            int totalCount = 0;
            Entities.QueryProjectDataSoure query = new QueryProjectDataSoure();
            query.ProjectID = projectID;
            DataTable dt = BLL.ProjectDataSoure.Instance.GetProjectDataSoure(query, "", 1, 1, out totalCount);
            return totalCount;
        }
        /// ��ȡ�����ڵ�����
        /// <summary>
        /// ��ȡ�����ڵ�����
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<string> GetNotExistsDataByProjectID(long projectID, string ids)
        {
            return Dal.ProjectDataSoure.Instance.GetNotExistsDataByProjectID(projectID, ids);
        }
    }
}

