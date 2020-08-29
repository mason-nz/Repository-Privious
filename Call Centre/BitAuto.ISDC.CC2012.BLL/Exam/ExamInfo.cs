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
	/// ҵ���߼���ExamInfo ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-09-03 02:04:16 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ExamInfo
	{
		#region Instance
		public static readonly ExamInfo Instance = new ExamInfo();
		#endregion

		#region Contructor
		protected ExamInfo()
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
		public DataTable GetExamInfo(QueryExamInfo query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.ExamInfo.Instance.GetExamInfo(query,order,currentPage,pageSize,out totalCount);
		}

        public DataTable GetExamInfo2(string query, string order, int currentPage, int pageSize, out int totalCount)
        {
            int userid = BLL.Util.GetLoginUserID();
            return Dal.ExamInfo.Instance.GetExamInfo2(query, order, currentPage, pageSize, out totalCount, userid);
        }
		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.ExamInfo.Instance.GetExamInfo(new QueryExamInfo(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.ExamInfo GetExamInfo(long EIID)
		{
			
			return Dal.ExamInfo.Instance.GetExamInfo(EIID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByEIID(long EIID)
		{
			QueryExamInfo query = new QueryExamInfo();
			query.EIID = EIID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetExamInfo(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.ExamInfo model)
		{
			return Dal.ExamInfo.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.ExamInfo model)
		{
			return Dal.ExamInfo.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.ExamInfo model)
		{
			return Dal.ExamInfo.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.ExamInfo model)
		{
			return Dal.ExamInfo.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(long EIID)
		{
			
			return Dal.ExamInfo.Instance.Delete(EIID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, long EIID)
		{
			
			return Dal.ExamInfo.Instance.Delete(sqltran, EIID);
		}

		#endregion

        #region ��ȡ���д�����
        /// <summary>
        /// ��ȡ���д�����
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCreateUsers()
        {
            return Dal.ExamInfo.Instance.GetAllCreateUsers();
        }
        #endregion

        /// <summary>
        /// ������ĿID��ȡ�ɼ�
        /// </summary>
        /// <param name="eiid"></param>
        /// <returns></returns>
        public DataTable GetScoreListByEIID(string eiid)
        {
            return Dal.ExamInfo.Instance.GetScoreListByEIID(eiid);
        }
        /// <summary>
        /// ��ȡ�Ծ�������Ŀʹ�õĴ���
        /// </summary>
        /// <param name="epid"></param>
        /// <returns></returns>
        public int GetExamPaperUsedCount(long epid)
        {
            return Dal.ExamInfo.Instance.GetExamPaperUsedCount(epid);
        }
    }
}

