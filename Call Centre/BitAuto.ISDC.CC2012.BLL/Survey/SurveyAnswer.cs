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
    /// ҵ���߼���SurveyAnswer ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-10-24 10:32:19 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class SurveyAnswer
    {
        #region Instance
        public static readonly SurveyAnswer Instance = new SurveyAnswer();
        #endregion

        #region Contructor
        protected SurveyAnswer()
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
        public DataTable GetSurveyAnswer(QuerySurveyAnswer query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.SurveyAnswer.Instance.GetSurveyAnswer(query, order, currentPage, pageSize, out totalCount);
        }

        public DataTable GetSurveyAnswerByTextDetail(QuerySurveyAnswer query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.SurveyAnswer.Instance.GetSurveyAnswerByTextDetail(query, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.SurveyAnswer.Instance.GetSurveyAnswer(new QuerySurveyAnswer(), string.Empty, 1, 1000000, out totalCount);
        }
        /// <summary>
        /// ��ȡ�μ��������Ա��
        /// </summary>
        /// <param name="SIID">����ID</param>
        /// <returns></returns>
        public int GetAnswerUserCountBySPIID(int SPIID)
        {
            return Dal.SurveyAnswer.Instance.GetAnswerUserCountBySPIID(SPIID);
        }
        /// <summary>
        /// ��ȡ�μ��������Ա��
        /// </summary>
        /// <param name="SQID">����ID</param>
        /// <returns></returns>
        public int GetAnswerUserCountBySQID(int SQID, int SPIID)
        {
            return Dal.SurveyAnswer.Instance.GetAnswerUserCountBySQID(SQID, SPIID);
        }

        /// <summary>
        /// ��ȡ�˴ε��������ϸ��Ϣ
        /// </summary>
        /// <param name="SPIID"></param>
        /// <returns></returns>
        public DataTable GetAnswerDetailBySPIID(int SPIID)
        {
            return Dal.SurveyAnswer.Instance.GetAnswerDetailBySPIID(SPIID);
        }

        /// <summary>
        /// ����PTID����ID��ȡ�˴ε��������ϸ��Ϣ  lxw
        /// </summary>
        /// <param name="PTID"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public DataTable GetAnswerDetailByPTID(string PTID, int projectID, int siid)
        {
            return Dal.SurveyAnswer.Instance.GetAnswerDetailByPTID(PTID, projectID, siid);
        }

        /// <summary>
        /// ����ReturnVisitCRMCustID�طÿͻ�ID��ȡ�˴ε��������ϸ��Ϣ  lxw
        /// </summary>
        /// <param name="PTID"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public DataTable GetAnswerDetailByReturnCustID(string ReturnCustID, int projectID, int siid)
        {
            return Dal.SurveyAnswer.Instance.GetAnswerDetailByReturnCustID(ReturnCustID, projectID, siid);
        }
        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.SurveyAnswer GetSurveyAnswer(int RecID)
        {

            return Dal.SurveyAnswer.Instance.GetSurveyAnswer(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsBySPIIDAndSIIDAndSQIDAndCreateUserID(int SPIID, int SIID, int SQID, int CreateUserID)
        {
            QuerySurveyAnswer query = new QuerySurveyAnswer();
            query.SPIID = SPIID;
            query.SIID = SIID;
            query.SQID = SQID;
            query.CreateUserID = CreateUserID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetSurveyAnswer(query, string.Empty, 1, 1, out count);
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
        public void Insert(Entities.SurveyAnswer model)
        {
            Dal.SurveyAnswer.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Insert(SqlTransaction sqltran, Entities.SurveyAnswer model)
        {
            Dal.SurveyAnswer.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.SurveyAnswer model)
        {
            return Dal.SurveyAnswer.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.SurveyAnswer model)
        {
            return Dal.SurveyAnswer.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int RecID)
        {

            return Dal.SurveyAnswer.Instance.Delete(RecID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {

            return Dal.SurveyAnswer.Instance.Delete(sqltran, RecID);
        }


        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int SIID, string PTID)
        {

            return Dal.SurveyAnswer.Instance.Delete(sqltran, SIID, PTID);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int DeleteByCustID(SqlTransaction sqltran, int SIID, string CustID)
        {

            return Dal.SurveyAnswer.Instance.DeleteByCustID(sqltran, SIID, CustID);
        }
        #endregion

        /// <summary>
        /// ͨ��Where�õ�AnswerContent
        /// </summary> 
        /// <param name="Where">����</param>
        /// <returns></returns>
        public string getAnswerBySQID(string Where)
        {
            return Dal.SurveyAnswer.Instance.getAnswerBySQID(Where);
        }
        /// <summary>
        /// ͨ��ProjectID���ʾ�ID�õ�����ID
        /// </summary> 
        /// <param name="typeID">���ͣ�1-������ϴ��2-��������</param>
        /// <param name="ProjectID">��ĿID</param>
        /// <param name="siid">�ʾ�ID</param>
        /// <returns></returns>
        public DataTable getPTIDByProject(int typeID, int ProjectID, int siid)
        {
            return Dal.SurveyAnswer.Instance.getPTIDByProject(typeID,ProjectID, siid);
        }

        /// <summary>
        /// ͨ��ProjectID���ʾ�ID�õ�����ID,add by qizq 2014-11-24,ͨ���������ύʱ�������������id��ֻ��������������������ϴ���������ύ��������������ɣ����ܻ��н����������Ҳ��������ύʱ�䣬��������Ʒ��Ƽû�п���������ϴҵ�����Դ˷�����������ϴ��������
        /// </summary> 
        /// <param name="typeID">���ͣ�1-������ϴ��2-��������</param>
        /// <param name="ProjectID">��ĿID</param>
        /// <param name="siid">�ʾ�ID</param>
        /// <returns></returns>
        public DataTable getPTIDByProject(int typeID, int ProjectID, int siid, string tasksubstart, string tasksubend)
        {
            return Dal.SurveyAnswer.Instance.getPTIDByProject(typeID, ProjectID, siid, tasksubstart, tasksubend);
        }

        /// <summary>
        /// ͨ��ProjectID�õ��ͻ�ID
        /// </summary> 
        /// <param name="ProjectID">��ĿID</param> 
        /// <param name="siid">�ʾ�ID</param> 
        /// <returns></returns>
        public DataTable getCustIDByProject(int ProjectID, int siid)
        {
            return Dal.SurveyAnswer.Instance.getCustIDByProject(ProjectID, siid);
        }

        /// <summary>
        /// ͨ��ProjectID�õ��ͻ�ID,���ļ��ύʱ����� by qizq 2014-11-25
        /// </summary> 
        /// <param name="ProjectID">��ĿID</param> 
        /// <param name="siid">�ʾ�ID</param> 
        /// <returns></returns>
        public DataTable getCustIDByProject(int ProjectID, int siid, string substart, string subend)
        {
            return Dal.SurveyAnswer.Instance.getCustIDByProject(ProjectID, siid,substart,subend);

        }
    }
}

