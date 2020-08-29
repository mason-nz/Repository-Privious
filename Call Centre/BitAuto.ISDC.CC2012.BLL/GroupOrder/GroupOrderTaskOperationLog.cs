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
	/// ҵ���߼���GroupOrderTaskOperationLog ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-11-04 09:34:14 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class GroupOrderTaskOperationLog
	{
		#region Instance
		public static readonly GroupOrderTaskOperationLog Instance = new GroupOrderTaskOperationLog();
		#endregion

		#region Contructor
		protected GroupOrderTaskOperationLog()
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
		public DataTable GetGroupOrderTaskOperationLog(QueryGroupOrderTaskOperationLog query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.GroupOrderTaskOperationLog.Instance.GetGroupOrderTaskOperationLog(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.GroupOrderTaskOperationLog.Instance.GetGroupOrderTaskOperationLog(new QueryGroupOrderTaskOperationLog(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.GroupOrderTaskOperationLog GetGroupOrderTaskOperationLog(long RecID)
		{
			
			return Dal.GroupOrderTaskOperationLog.Instance.GetGroupOrderTaskOperationLog(RecID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByRecID(long RecID)
		{
			QueryGroupOrderTaskOperationLog query = new QueryGroupOrderTaskOperationLog();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetGroupOrderTaskOperationLog(query, string.Empty, 1, 1, out count);
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
		public long  Insert(Entities.GroupOrderTaskOperationLog model)
		{
			return Dal.GroupOrderTaskOperationLog.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.GroupOrderTaskOperationLog model)
		{
			return Dal.GroupOrderTaskOperationLog.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.GroupOrderTaskOperationLog model)
		{
			return Dal.GroupOrderTaskOperationLog.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.GroupOrderTaskOperationLog model)
		{
			return Dal.GroupOrderTaskOperationLog.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(long RecID)
		{
			
			return Dal.GroupOrderTaskOperationLog.Instance.Delete(RecID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, long RecID)
		{
			
			return Dal.GroupOrderTaskOperationLog.Instance.Delete(sqltran, RecID);
		}

		#endregion

	}
}
