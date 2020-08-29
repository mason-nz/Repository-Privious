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
	/// ҵ���߼���QS_Standard ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-04-25 09:42:37 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class QS_Standard
	{
		#region Instance
		public static readonly QS_Standard Instance = new QS_Standard();
		#endregion

		#region Contructor
		protected QS_Standard()
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
		public DataTable GetQS_Standard(QueryQS_Standard query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.QS_Standard.Instance.GetQS_Standard(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.QS_Standard.Instance.GetQS_Standard(new QueryQS_Standard(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.QS_Standard GetQS_Standard(int QS_SID)
		{
			
			return Dal.QS_Standard.Instance.GetQS_Standard(QS_SID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByQS_SID(int QS_SID)
		{
			QueryQS_Standard query = new QueryQS_Standard();
			query.QS_SID = QS_SID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetQS_Standard(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.QS_Standard model)
		{
			return Dal.QS_Standard.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.QS_Standard model)
		{
			return Dal.QS_Standard.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.QS_Standard model)
		{
			return Dal.QS_Standard.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.QS_Standard model)
		{
			return Dal.QS_Standard.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int QS_SID)
		{
			
			return Dal.QS_Standard.Instance.Delete(QS_SID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, int QS_SID)
		{
			
			return Dal.QS_Standard.Instance.Delete(sqltran, QS_SID);
		}

		#endregion

	}
}

