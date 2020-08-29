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
	/// ҵ���߼���LeadsTaskOperationLog ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-05-19 11:30:51 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class LeadsTaskOperationLog
	{
		#region Instance
		public static readonly LeadsTaskOperationLog Instance = new LeadsTaskOperationLog();
		#endregion

		#region Contructor
		protected LeadsTaskOperationLog()
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
		public DataTable GetLeadsTaskOperationLog(QueryLeadsTaskOperationLog query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.LeadsTaskOperationLog.Instance.GetLeadsTaskOperationLog(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.LeadsTaskOperationLog.Instance.GetLeadsTaskOperationLog(new QueryLeadsTaskOperationLog(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.LeadsTaskOperationLog GetLeadsTaskOperationLog(long RecID)
		{
			
			return Dal.LeadsTaskOperationLog.Instance.GetLeadsTaskOperationLog(RecID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByRecID(long RecID)
		{
			QueryLeadsTaskOperationLog query = new QueryLeadsTaskOperationLog();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetLeadsTaskOperationLog(query, string.Empty, 1, 1, out count);
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
        public long Insert(Entities.LeadsTaskOperationLog model)
		{
			return Dal.LeadsTaskOperationLog.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
        public long Insert(SqlTransaction sqltran, Entities.LeadsTaskOperationLog model)
		{
			return Dal.LeadsTaskOperationLog.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.LeadsTaskOperationLog model)
		{
			return Dal.LeadsTaskOperationLog.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.LeadsTaskOperationLog model)
		{
			return Dal.LeadsTaskOperationLog.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(long RecID)
		{
			
			return Dal.LeadsTaskOperationLog.Instance.Delete(RecID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, long RecID)
		{
			
			return Dal.LeadsTaskOperationLog.Instance.Delete(sqltran, RecID);
		}

		#endregion

	}
}

