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
	/// ҵ���߼���QS_DeadOrAppraisal ��ժҪ˵����
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
	public class QS_DeadOrAppraisal
	{
		#region Instance
		public static readonly QS_DeadOrAppraisal Instance = new QS_DeadOrAppraisal();
		#endregion

		#region Contructor
		protected QS_DeadOrAppraisal()
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
		public DataTable GetQS_DeadOrAppraisal(QueryQS_DeadOrAppraisal query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.QS_DeadOrAppraisal.Instance.GetQS_DeadOrAppraisal(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.QS_DeadOrAppraisal.Instance.GetQS_DeadOrAppraisal(new QueryQS_DeadOrAppraisal(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.QS_DeadOrAppraisal GetQS_DeadOrAppraisal(int QS_DAID)
		{
			
			return Dal.QS_DeadOrAppraisal.Instance.GetQS_DeadOrAppraisal(QS_DAID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByQS_DAID(int QS_DAID)
		{
			QueryQS_DeadOrAppraisal query = new QueryQS_DeadOrAppraisal();
			query.QS_DAID = QS_DAID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetQS_DeadOrAppraisal(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.QS_DeadOrAppraisal model)
		{
			return Dal.QS_DeadOrAppraisal.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.QS_DeadOrAppraisal model)
		{
			return Dal.QS_DeadOrAppraisal.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.QS_DeadOrAppraisal model)
		{
			return Dal.QS_DeadOrAppraisal.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.QS_DeadOrAppraisal model)
		{
			return Dal.QS_DeadOrAppraisal.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int QS_DAID)
		{
			
			return Dal.QS_DeadOrAppraisal.Instance.Delete(QS_DAID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, int QS_DAID)
		{
			
			return Dal.QS_DeadOrAppraisal.Instance.Delete(sqltran, QS_DAID);
		}

		#endregion

	}
}

