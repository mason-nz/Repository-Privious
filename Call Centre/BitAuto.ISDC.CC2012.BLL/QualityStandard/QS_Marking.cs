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
	/// ҵ���߼���QS_Marking ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-04-25 09:42:36 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class QS_Marking
	{
		#region Instance
		public static readonly QS_Marking Instance = new QS_Marking();
		#endregion

		#region Contructor
		protected QS_Marking()
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
		public DataTable GetQS_Marking(QueryQS_Marking query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.QS_Marking.Instance.GetQS_Marking(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.QS_Marking.Instance.GetQS_Marking(new QueryQS_Marking(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.QS_Marking GetQS_Marking(int QS_MID)
		{
			
			return Dal.QS_Marking.Instance.GetQS_Marking(QS_MID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByQS_MID(int QS_MID)
		{
			QueryQS_Marking query = new QueryQS_Marking();
			query.QS_MID = QS_MID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetQS_Marking(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.QS_Marking model)
		{
			return Dal.QS_Marking.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.QS_Marking model)
		{
			return Dal.QS_Marking.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.QS_Marking model)
		{
			return Dal.QS_Marking.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.QS_Marking model)
		{
			return Dal.QS_Marking.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int QS_MID)
		{
			
			return Dal.QS_Marking.Instance.Delete(QS_MID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, int QS_MID)
		{
			
			return Dal.QS_Marking.Instance.Delete(sqltran, QS_MID);
		}

		#endregion

	}
}

