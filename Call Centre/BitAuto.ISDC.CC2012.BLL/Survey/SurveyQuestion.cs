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
	/// 业务逻辑类SurveyQuestion 的摘要说明。
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
		/// 按照查询条件查询
		/// </summary>
		/// <param name="query">查询条件</param>
		/// <param name="order">排序</param>
		/// <param name="currentPage">页号,-1不分页</param>
		/// <param name="pageSize">每页记录数</param>
		/// <param name="totalCount">总行数</param>
		/// <returns>集合</returns>
		public DataTable GetSurveyQuestion(QuerySurveyQuestion query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.SurveyQuestion.Instance.GetSurveyQuestion(query,order,currentPage,pageSize,out totalCount);
		}

        /// <summary>
        /// 统计选择题
        /// </summary>
        /// <param name="SQID"></param>
        /// <param name="SPIID"></param>
        /// <returns></returns>
        public DataTable StatQuestionForMultipleChoice(int SQID,int SPIID)
        {
            return Dal.SurveyQuestion.Instance.StatQuestionForMultipleChoice(SQID, SPIID);
        }

        /// <summary>
        /// 统计每个试题的总得分
        /// </summary>
        /// <param name="SQID"></param>
        /// <returns></returns>
        public int GetChoiceTotalScoreBySQID(int SQID)
        {
            return Dal.SurveyQuestion.Instance.GetChoiceTotalScoreBySQID(SQID);
        }
        /// <summary>
        /// 统计矩阵单选题
        /// </summary>
        /// <param name="SQID"></param>
        /// <returns></returns>
        public DataTable StatQuestionForMatrixRadio(int SQID, int SPIID)
        {
            return Dal.SurveyQuestion.Instance.StatQuestionForMatrixRadio(SQID, SPIID);
        }

        /// <summary>
        /// 统计矩阵下拉
        /// </summary>
        /// <param name="SQID"></param>
        /// <returns></returns>
        public DataTable StatQuestionForMatrixDropdown(int SQID, int SPIID)
        {
            return Dal.SurveyQuestion.Instance.StatQuestionForMatrixDropdown(SQID, SPIID);
        }
         /// <summary>
        /// 获取项目下某个矩阵下拉问题所有问题的分数和
        /// </summary>
        /// <param name="SQID"></param>
        /// <param name="SPIID"></param>
        /// <returns></returns>
        public int GetQuestionForMatrixDropdownSumScore(int SQID, int SPIID)
        {
            return Dal.SurveyQuestion.Instance.GetQuestionForMatrixDropdownSumScore(SQID, SPIID);
        }
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.SurveyQuestion.Instance.GetSurveyQuestion(new QuerySurveyQuestion(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.SurveyQuestion GetSurveyQuestion(int SQID)
		{
			
			return Dal.SurveyQuestion.Instance.GetSurveyQuestion(SQID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
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
		/// 增加一条数据
		/// </summary>
		public int  Insert(Entities.SurveyQuestion model)
		{
			return Dal.SurveyQuestion.Instance.Insert(model);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.SurveyQuestion model)
		{
			return Dal.SurveyQuestion.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.SurveyQuestion model)
		{
			return Dal.SurveyQuestion.Instance.Update(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.SurveyQuestion model)
		{
			return Dal.SurveyQuestion.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(int SQID)
		{
			
			return Dal.SurveyQuestion.Instance.Delete(SQID);
		}

		/// <summary>
		/// 删除一条数据
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
        /// 通过SIID获取问卷问题信息
        /// </summary>
        /// <param name="siid"></param>
        /// <returns></returns>
        public DataTable GetQuestionBySIID(int siid)
        {
            return Dal.SurveyQuestion.Instance.GetQuestionBySIID(siid);
        }
        /// <summary>
        /// 通过ProjectID获取问卷答案信息
        /// </summary>
        /// <param name="siid"></param>
        /// <param name="typeID">typeID=1：数据清洗任务；typeID=2：其他任务；typeID=3：客户回访</param>
        /// <returns></returns>
        public DataTable GetAnswerInfoByProjectID(int ProjectID, int SIID, int typeID)
        {
            return Dal.SurveyQuestion.Instance.GetAnswerInfoByProjectID(ProjectID, SIID, typeID);
        }
    }
}

