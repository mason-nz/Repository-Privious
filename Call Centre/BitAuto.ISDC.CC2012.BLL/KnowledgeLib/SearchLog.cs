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
	/// ҵ���߼���SearchLog ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-08-21 10:19:10 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class SearchLog
	{
		#region Instance
		public static readonly SearchLog Instance = new SearchLog();
		#endregion

		#region Contructor
		protected SearchLog()
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
		public DataTable GetSearchLog(QuerySearchLog query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.SearchLog.Instance.GetSearchLog(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.SearchLog.Instance.GetSearchLog(new QuerySearchLog(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.SearchLog GetSearchLog(long RecID)
		{
			
			return Dal.SearchLog.Instance.GetSearchLog(RecID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByRecID(long RecID)
		{
			QuerySearchLog query = new QuerySearchLog();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetSearchLog(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.SearchLog model)
		{
			return Dal.SearchLog.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.SearchLog model)
		{
			return Dal.SearchLog.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.SearchLog model)
		{
			return Dal.SearchLog.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.SearchLog model)
		{
			return Dal.SearchLog.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(long RecID)
		{
			
			return Dal.SearchLog.Instance.Delete(RecID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, long RecID)
		{
			
			return Dal.SearchLog.Instance.Delete(sqltran, RecID);
		}

		#endregion

	}
}

