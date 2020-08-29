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
	/// ҵ���߼���ExamPaper ��ժҪ˵����
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
		/// ���ղ�ѯ������ѯ
		/// </summary>
		/// <param name="query">��ѯ����</param>
		/// <param name="order">����</param>
		/// <param name="currentPage">ҳ��,-1����ҳ</param>
		/// <param name="pageSize">ÿҳ��¼��</param>
		/// <param name="totalCount">������</param>
		/// <returns>����</returns>
		public DataTable GetExamPaper(QueryExamPaper query, string order, int currentPage, int pageSize,out int totalCount)
		{
			return Dal.ExamPaper.Instance.GetExamPaper(query,order,currentPage,pageSize, BLL.Util.GetLoginUserID(), out totalCount);
		}
        public DataTable GetExamPaperByExamList(QueryExamPaper query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ExamPaper.Instance.GetExamPaperByExamList(query, order, currentPage, pageSize, out totalCount);
        }
		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.ExamPaper.Instance.GetExamPaper(new QueryExamPaper(),string.Empty,1,1000000,0,out totalCount);
		}

		#endregion
        /// <summary>
        /// �õ�ExamPaperInfo
        /// </summary>
        /// <returns></returns>
        public Entities.ExamPaperInfo GetExamPaperInfo(long EPID)
        {
            Entities.ExamPaperInfo ExamPaperInfo = new ExamPaperInfo();
            //ȡ�Ծ�����ʵ��
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
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.ExamPaper GetExamPaper(long EPID)
		{
			
			return Dal.ExamPaper.Instance.GetExamPaper(EPID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
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
		/// ����һ������
		/// </summary>
		public int  Insert(Entities.ExamPaper model)
		{
			return Dal.ExamPaper.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.ExamPaper model)
		{
			return Dal.ExamPaper.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.ExamPaper model)
		{
			return Dal.ExamPaper.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.ExamPaper model)
		{
			return Dal.ExamPaper.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(long EPID)
		{
			
			return Dal.ExamPaper.Instance.Delete(EPID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, long EPID)
		{
			
			return Dal.ExamPaper.Instance.Delete(sqltran, EPID);
		}

		#endregion

        /// <summary>
        /// ��ȡ���д�����
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCreateUsers()
        {
            return Dal.ExamPaper.Instance.GetAllCreateUsers(BLL.Util.GetLoginUserID());
        }
    }
}

