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
	/// ҵ���߼���SurveyQuestion ��ժҪ˵����
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
	public class SurveyQuestion
	{
		#region Instance
		public static readonly SurveyQuestion Instance = new SurveyQuestion();
		#endregion

		#region Contructor
		protected SurveyQuestion()
		{}
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
		public DataTable GetSurveyQuestion(QuerySurveyQuestion query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.SurveyQuestion.Instance.GetSurveyQuestion(query,order,currentPage,pageSize,out totalCount);
		}

        /// <summary>
        /// ͳ��ѡ����
        /// </summary>
        /// <param name="SQID"></param>
        /// <param name="SPIID"></param>
        /// <returns></returns>
        public DataTable StatQuestionForMultipleChoice(int SQID,int SPIID)
        {
            return Dal.SurveyQuestion.Instance.StatQuestionForMultipleChoice(SQID, SPIID);
        }

        /// <summary>
        /// ͳ��ÿ��������ܵ÷�
        /// </summary>
        /// <param name="SQID"></param>
        /// <returns></returns>
        public int GetChoiceTotalScoreBySQID(int SQID)
        {
            return Dal.SurveyQuestion.Instance.GetChoiceTotalScoreBySQID(SQID);
        }
        /// <summary>
        /// ͳ�ƾ���ѡ��
        /// </summary>
        /// <param name="SQID"></param>
        /// <returns></returns>
        public DataTable StatQuestionForMatrixRadio(int SQID, int SPIID)
        {
            return Dal.SurveyQuestion.Instance.StatQuestionForMatrixRadio(SQID, SPIID);
        }

        /// <summary>
        /// ͳ�ƾ�������
        /// </summary>
        /// <param name="SQID"></param>
        /// <returns></returns>
        public DataTable StatQuestionForMatrixDropdown(int SQID, int SPIID)
        {
            return Dal.SurveyQuestion.Instance.StatQuestionForMatrixDropdown(SQID, SPIID);
        }
         /// <summary>
        /// ��ȡ��Ŀ��ĳ����������������������ķ�����
        /// </summary>
        /// <param name="SQID"></param>
        /// <param name="SPIID"></param>
        /// <returns></returns>
        public int GetQuestionForMatrixDropdownSumScore(int SQID, int SPIID)
        {
            return Dal.SurveyQuestion.Instance.GetQuestionForMatrixDropdownSumScore(SQID, SPIID);
        }
		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.SurveyQuestion.Instance.GetSurveyQuestion(new QuerySurveyQuestion(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.SurveyQuestion GetSurveyQuestion(int SQID)
		{
			
			return Dal.SurveyQuestion.Instance.GetSurveyQuestion(SQID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsBySQID(int SQID)
		{
			QuerySurveyQuestion query = new QuerySurveyQuestion();
			query.SQID = SQID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetSurveyQuestion(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.SurveyQuestion model)
		{
			return Dal.SurveyQuestion.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.SurveyQuestion model)
		{
			return Dal.SurveyQuestion.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.SurveyQuestion model)
		{
			return Dal.SurveyQuestion.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.SurveyQuestion model)
		{
			return Dal.SurveyQuestion.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int SQID)
		{
			
			return Dal.SurveyQuestion.Instance.Delete(SQID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, int SQID)
		{
			
			return Dal.SurveyQuestion.Instance.Delete(sqltran, SQID);
		}

		#endregion


        public List<Entities.SurveyQuestion> GetSurveyQuestionList(int siid)
        {
            return Dal.SurveyQuestion.Instance.GetSurveyQuestionList(siid);
        }
        /// <summary>
        /// ͨ��SIID��ȡ�ʾ�������Ϣ
        /// </summary>
        /// <param name="siid"></param>
        /// <returns></returns>
        public DataTable GetQuestionBySIID(int siid)
        {
            return Dal.SurveyQuestion.Instance.GetQuestionBySIID(siid);
        }
        /// <summary>
        /// ͨ��ProjectID��ȡ�ʾ����Ϣ
        /// </summary>
        /// <param name="siid"></param>
        /// <param name="typeID">typeID=1��������ϴ����typeID=2����������typeID=3���ͻ��ط�</param>
        /// <returns></returns>
        public DataTable GetAnswerInfoByProjectID(int ProjectID, int SIID, int typeID)
        {
            return Dal.SurveyQuestion.Instance.GetAnswerInfoByProjectID(ProjectID, SIID, typeID);
        }
    }
}

