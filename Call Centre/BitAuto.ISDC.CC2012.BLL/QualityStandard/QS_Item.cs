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
	/// ҵ���߼���QS_Item ��ժҪ˵����
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
	public class QS_Item
	{
		#region Instance
		public static readonly QS_Item Instance = new QS_Item();
		#endregion

		#region Contructor
		protected QS_Item()
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
		public DataTable GetQS_Item(QueryQS_Item query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.QS_Item.Instance.GetQS_Item(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.QS_Item.Instance.GetQS_Item(new QueryQS_Item(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.QS_Item GetQS_Item(int QS_IID)
		{
			
			return Dal.QS_Item.Instance.GetQS_Item(QS_IID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByQS_IID(int QS_IID)
		{
			QueryQS_Item query = new QueryQS_Item();
			query.QS_IID = QS_IID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetQS_Item(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.QS_Item model)
		{
			return Dal.QS_Item.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.QS_Item model)
		{
			return Dal.QS_Item.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.QS_Item model)
		{
			return Dal.QS_Item.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.QS_Item model)
		{
			return Dal.QS_Item.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int QS_IID)
		{
			
			return Dal.QS_Item.Instance.Delete(QS_IID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, int QS_IID)
		{
			
			return Dal.QS_Item.Instance.Delete(sqltran, QS_IID);
		}

		#endregion

	}
}

