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
	/// 业务逻辑类ExamPaper 的摘要说明。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-09-03 02:04:17 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ExamPaper
	{
		#region Instance
		public static readonly ExamPaper Instance = new ExamPaper();
		#endregion

		#region Contructor
		protected ExamPaper()
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
		public DataTable GetExamPaper(QueryExamPaper query, string order, int currentPage, int pageSize,out int totalCount)
		{
			return Dal.ExamPaper.Instance.GetExamPaper(query,order,currentPage,pageSize, BLL.Util.GetLoginUserID(), out totalCount);
		}
        public DataTable GetExamPaperByExamList(QueryExamPaper query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ExamPaper.Instance.GetExamPaperByExamList(query, order, currentPage, pageSize, out totalCount);
        }
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.ExamPaper.Instance.GetExamPaper(new QueryExamPaper(),string.Empty,1,1000000,0,out totalCount);
		}

		#endregion
        /// <summary>
        /// 得到ExamPaperInfo
        /// </summary>
        /// <returns></returns>
        public Entities.ExamPaperInfo GetExamPaperInfo(long EPID)
        {
            Entities.ExamPaperInfo ExamPaperInfo = new ExamPaperInfo();
            //取试卷主表实体
            ExamPaperInfo.ExamPaper=Dal.ExamPaper.Instance.GetExamPaper(EPID);
            List<Entities.ExamBigQuestioninfo> ExamBigQuestioninfoList= null;
            List<Entities.ExamBigQuestion> ExamBigQuestionList = BLL.ExamBigQuestion.Instance.GetExamBigQuestionList(EPID);
            if (ExamBigQuestionList != null & ExamBigQuestionList.Count > 0)
            {
                ExamBigQuestioninfoList = new List<ExamBigQuestioninfo>();
                for (int i = 0; i < ExamBigQuestionList.Count; i++)
                {
                    Entities.ExamBigQuestioninfo ExamBigQuestioninfo= new ExamBigQuestioninfo();
                    ExamBigQuestioninfo.ExamBigQuestion = ExamBigQuestionList[i];
                    List<Entities.ExamBSQuestionShip> ExamBSQuestionShipList = null;

                    ExamBSQuestionShipList = BLL.ExamBSQuestionShip.Instance.GetExamBSQuestionShipList(ExamBigQuestionList[i].BQID);
                    ExamBigQuestioninfo.ExamBSQuestionShipList = ExamBSQuestionShipList;
                    ExamBigQuestioninfoList.Add(ExamBigQuestioninfo);
                }

            }
            ExamPaperInfo.ExamBigQuestioninfoList = ExamBigQuestioninfoList;
            return ExamPaperInfo;
        }

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.ExamPaper GetExamPaper(long EPID)
		{
			
			return Dal.ExamPaper.Instance.GetExamPaper(EPID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool IsExistsByEPID(long EPID)
		{
			QueryExamPaper query = new QueryExamPaper();
			query.EPID = EPID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetExamPaper(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.ExamPaper model)
		{
			return Dal.ExamPaper.Instance.Insert(model);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.ExamPaper model)
		{
			return Dal.ExamPaper.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.ExamPaper model)
		{
			return Dal.ExamPaper.Instance.Update(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.ExamPaper model)
		{
			return Dal.ExamPaper.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(long EPID)
		{
			
			return Dal.ExamPaper.Instance.Delete(EPID);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, long EPID)
		{
			
			return Dal.ExamPaper.Instance.Delete(sqltran, EPID);
		}

		#endregion

        /// <summary>
        /// 获取所有创建人
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCreateUsers()
        {
            return Dal.ExamPaper.Instance.GetAllCreateUsers(BLL.Util.GetLoginUserID());
        }
    }
}

