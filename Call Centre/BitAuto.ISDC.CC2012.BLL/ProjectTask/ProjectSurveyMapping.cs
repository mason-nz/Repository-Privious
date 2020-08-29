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
    /// ҵ���߼���ProjectSurveyMapping ��ժҪ˵����
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
    public class ProjectSurveyMapping
    {
        #region Instance
        public static readonly ProjectSurveyMapping Instance = new ProjectSurveyMapping();
        #endregion

        #region Contructor
        protected ProjectSurveyMapping()
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
        public DataTable GetProjectSurveyMapping(QueryProjectSurveyMapping query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectSurveyMapping.Instance.GetProjectSurveyMapping(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ���ղ�ѯ������ѯ(������Ŀ�ʾ�ҳ��)
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetProjectSurveyMappingForList(QueryProjectSurveyMapping query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectSurveyMapping.Instance.GetProjectSurveyMappingForList(query, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.ProjectSurveyMapping.Instance.GetProjectSurveyMapping(new QueryProjectSurveyMapping(), string.Empty, 1, 1000000, out totalCount);
        }

        /// <summary>
        /// ��������idȡ�����ʾ�
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public DataTable GetSurveyinfoByTaskID(string TaskID)
        {
            return Dal.ProjectSurveyMapping.Instance.GetSurveyinfoByTaskID(TaskID);
        }

        /// <summary>
        /// ������������idȡ�����ʾ�
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public DataTable GetSurveyinfoByOtherTaskID(string TaskID)
        {
            return Dal.ProjectSurveyMapping.Instance.GetSurveyinfoByOtherTaskID(TaskID);
        }

        /// <summary>
        /// ���ݿͻ�idȡ�����ʾ�
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public DataTable GetSurveyinfoByCustID(string CustID)
        {
            return Dal.ProjectSurveyMapping.Instance.GetSurveyinfoByCustID(CustID);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.ProjectSurveyMapping GetProjectSurveyMapping(long ProjectID, int SIID)
        {

            return Dal.ProjectSurveyMapping.Instance.GetProjectSurveyMapping(ProjectID, SIID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByProjectIDAndSIID(long ProjectID, int SIID)
        {
            QueryProjectSurveyMapping query = new QueryProjectSurveyMapping();
            query.ProjectID = ProjectID;
            query.SIID = SIID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectSurveyMapping(query, string.Empty, 1, 1, out count);
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
        public void Insert(Entities.ProjectSurveyMapping model)
        {
            Dal.ProjectSurveyMapping.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Insert(SqlTransaction sqltran, Entities.ProjectSurveyMapping model)
        {
            Dal.ProjectSurveyMapping.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.ProjectSurveyMapping model)
        {
            return Dal.ProjectSurveyMapping.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ProjectSurveyMapping model)
        {
            return Dal.ProjectSurveyMapping.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(long ProjectID, int SIID)
        {

            return Dal.ProjectSurveyMapping.Instance.Delete(ProjectID, SIID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, long ProjectID, int SIID)
        {

            return Dal.ProjectSurveyMapping.Instance.Delete(sqltran, ProjectID, SIID);
        }

        #endregion


        public List<Entities.ProjectSurveyMapping> GetProjectSurveyMappingList(QueryProjectSurveyMapping query, out int totalCount)
        {
            List<Entities.ProjectSurveyMapping> list = new List<Entities.ProjectSurveyMapping>();

            DataTable dt = BLL.ProjectSurveyMapping.Instance.GetProjectSurveyMapping(query, "", 1, 999, out totalCount);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Entities.ProjectSurveyMapping model = new Entities.ProjectSurveyMapping();
                    model.ProjectID = int.Parse(dr["ProjectID"].ToString());
                    model.SIID = int.Parse(dr["SIID"].ToString());
                    model.BeginDate = dr["BeginDate"].ToString();
                    model.EndDate = dr["EndDate"].ToString();
                    model.Status = int.Parse(dr["Status"].ToString());
                    model.CreateUserID = int.Parse(dr["CreateUserID"].ToString());
                    model.CreateTime = DateTime.Parse(dr["CreateTime"].ToString());

                    list.Add(model);
                }
            }

            return list;
        }
    }
}

