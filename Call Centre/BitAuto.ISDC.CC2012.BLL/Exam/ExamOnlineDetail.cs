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
	/// ҵ���߼���ExamOnlineDetail ��ժҪ˵����
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
	public class ExamOnlineDetail
	{
		#region Instance
		public static readonly ExamOnlineDetail Instance = new ExamOnlineDetail();
		#endregion

		#region Contructor
		protected ExamOnlineDetail()
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
		public DataTable GetExamOnlineDetail(QueryExamOnlineDetail query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.ExamOnlineDetail.Instance.GetExamOnlineDetail(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.ExamOnlineDetail.Instance.GetExamOnlineDetail(new QueryExamOnlineDetail(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.ExamOnlineDetail GetExamOnlineDetail(long EOLDID)
		{
			
			return Dal.ExamOnlineDetail.Instance.GetExamOnlineDetail(EOLDID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByEOLDID(long EOLDID)
		{
			QueryExamOnlineDetail query = new QueryExamOnlineDetail();
			query.EOLDID = EOLDID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetExamOnlineDetail(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.ExamOnlineDetail model)
		{
			return Dal.ExamOnlineDetail.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.ExamOnlineDetail model)
		{
			return Dal.ExamOnlineDetail.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.ExamOnlineDetail model)
		{
			return Dal.ExamOnlineDetail.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.ExamOnlineDetail model)
		{
			return Dal.ExamOnlineDetail.Instance.Update(sqltran, model);
		}

		#endregion

        #region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(long EOLDID)
		{
			
			return Dal.ExamOnlineDetail.Instance.Delete(EOLDID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, long EOLDID)
		{
			
			return Dal.ExamOnlineDetail.Instance.Delete(sqltran, EOLDID);
		}
         
		#endregion

        /// <summary>
        /// add by qizq ����С�����
        /// </summary>
        /// <param name="EOLID"></param>
        /// <param name="BQID"></param>
        /// <param name="KLQID"></param>
        /// <param name="Score"></param>
        public void UpdateByEOLID(string EOLID, string BQID, string KLQID, string Score)
        {
            Dal.ExamOnlineDetail.Instance.UpdateByEOLID(EOLID,BQID,KLQID,Score);
        }

	}
}

