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
    /// ҵ���߼���ProjectTask_SurveyAnswer ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-02-20 10:39:32 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ProjectTask_SurveyAnswer
    {
        #region Instance
        public static readonly ProjectTask_SurveyAnswer Instance = new ProjectTask_SurveyAnswer();
        #endregion

        #region Contructor
        protected ProjectTask_SurveyAnswer()
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
        public DataTable GetProjectTask_SurveyAnswer(QueryProjectTask_SurveyAnswer query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectTask_SurveyAnswer.Instance.GetProjectTask_SurveyAnswer(query, order, currentPage, pageSize, out totalCount);
        }
        public int GetProjectTask_SurveyAnswer_Count(QueryProjectTask_SurveyAnswer query)
        {
            return Dal.ProjectTask_SurveyAnswer.Instance.GetProjectTask_SurveyAnswer_Count(query);
        }
        /// <summary>
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetProjectTask_SurveyAnswer(SqlTransaction tran, QueryProjectTask_SurveyAnswer query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectTask_SurveyAnswer.Instance.GetProjectTask_SurveyAnswer(tran, query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.ProjectTask_SurveyAnswer.Instance.GetProjectTask_SurveyAnswer(new QueryProjectTask_SurveyAnswer(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.ProjectTask_SurveyAnswer GetProjectTask_SurveyAnswer(long RecID)
        {

            return Dal.ProjectTask_SurveyAnswer.Instance.GetProjectTask_SurveyAnswer(RecID);
        }

        /// <summary>
        /// ������ĿID��ȡ����Ŀ�Ĵ�����Ϣ
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public DataTable GetAnswerDetailByProjectID(int projectID, int siid)
        {
            return Dal.ProjectTask_SurveyAnswer.Instance.GetAnswerDetailByProjectID(projectID, siid);
        }

        /// <summary>
        /// ������������ѯ��¼����
        /// </summary>
        /// <param name="tran">����</param>
        /// <param name="query">��������</param>
        /// <returns>�м�¼����True�����򷵻�False</returns>
        public Entities.ProjectTask_SurveyAnswer GetProjectTask_SurveyAnswerByQuery(SqlTransaction tran, QueryProjectTask_SurveyAnswer query)
        {
            DataTable dt = Dal.ProjectTask_SurveyAnswer.Instance.GetProjectTask_SurveyAnswerByQuery(tran, query);
            if (dt!=null&&!string.IsNullOrEmpty(dt.Rows[0]["RecID"].ToString()))
            {
                return Dal.ProjectTask_SurveyAnswer.Instance.LoadSingleProjectTask_SurveyAnswer(dt.Rows[0]);
            }
            return null;
        }
        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByRecID(long RecID)
        {
            QueryProjectTask_SurveyAnswer query = new QueryProjectTask_SurveyAnswer();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectTask_SurveyAnswer(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.ProjectTask_SurveyAnswer model)
        {
            return Dal.ProjectTask_SurveyAnswer.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ProjectTask_SurveyAnswer model)
        {
            return Dal.ProjectTask_SurveyAnswer.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.ProjectTask_SurveyAnswer model)
        {
            return Dal.ProjectTask_SurveyAnswer.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ProjectTask_SurveyAnswer model)
        {
            return Dal.ProjectTask_SurveyAnswer.Instance.Update(sqltran, model);
        }

        public int UpdateCreateTimeAndStatus(SqlTransaction tran, Entities.ProjectTask_SurveyAnswer model)
        {
            return Dal.ProjectTask_SurveyAnswer.Instance.UpdateCreateTimeAndStatus(tran, model);
        }
        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(long RecID)
        {

            return Dal.ProjectTask_SurveyAnswer.Instance.Delete(RecID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, long RecID)
        {

            return Dal.ProjectTask_SurveyAnswer.Instance.Delete(sqltran, RecID);
        }

        #endregion


    }
}

