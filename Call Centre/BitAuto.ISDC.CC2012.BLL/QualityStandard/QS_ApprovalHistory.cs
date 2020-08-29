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
	/// ҵ���߼���QS_ApprovalHistory ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-04-25 09:42:35 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class QS_ApprovalHistory
	{
		#region Instance
		public static readonly QS_ApprovalHistory Instance = new QS_ApprovalHistory();
		#endregion

		#region Contructor
		protected QS_ApprovalHistory()
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
		public DataTable GetQS_ApprovalHistory(QueryQS_ApprovalHistory query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.QS_ApprovalHistory.Instance.GetQS_ApprovalHistory(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.QS_ApprovalHistory.Instance.GetQS_ApprovalHistory(new QueryQS_ApprovalHistory(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.QS_ApprovalHistory GetQS_ApprovalHistory(int RecID)
		{
			
			return Dal.QS_ApprovalHistory.Instance.GetQS_ApprovalHistory(RecID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByRecID(int RecID)
		{
			QueryQS_ApprovalHistory query = new QueryQS_ApprovalHistory();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetQS_ApprovalHistory(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.QS_ApprovalHistory model)
		{
			return Dal.QS_ApprovalHistory.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.QS_ApprovalHistory model)
		{
			return Dal.QS_ApprovalHistory.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.QS_ApprovalHistory model)
		{
			return Dal.QS_ApprovalHistory.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.QS_ApprovalHistory model)
		{
			return Dal.QS_ApprovalHistory.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int RecID)
		{
			
			return Dal.QS_ApprovalHistory.Instance.Delete(RecID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, int RecID)
		{
			
			return Dal.QS_ApprovalHistory.Instance.Delete(sqltran, RecID);
		}

		#endregion

	}
}

