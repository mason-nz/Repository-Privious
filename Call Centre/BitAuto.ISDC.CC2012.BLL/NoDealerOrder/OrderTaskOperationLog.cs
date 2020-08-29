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
	/// ҵ���߼���OrderTaskOperationLog ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-09-21 10:33:33 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class OrderTaskOperationLog
	{
		#region Instance
		public static readonly OrderTaskOperationLog Instance = new OrderTaskOperationLog();
		#endregion

		#region Contructor
		protected OrderTaskOperationLog()
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
		public DataTable GetOrderTaskOperationLog(QueryOrderTaskOperationLog query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.OrderTaskOperationLog.Instance.GetOrderTaskOperationLog(query,order,currentPage,pageSize,out totalCount);
		}
        /// <summary>
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetOrderTaskOperationLogHaveCall(QueryOrderTaskOperationLog query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.OrderTaskOperationLog.Instance.GetOrderTaskOperationLogHaveCall(query, order, currentPage, pageSize, out totalCount);
        }

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.OrderTaskOperationLog.Instance.GetOrderTaskOperationLog(new QueryOrderTaskOperationLog(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.OrderTaskOperationLog GetOrderTaskOperationLog(long RecID)
		{
			
			return Dal.OrderTaskOperationLog.Instance.GetOrderTaskOperationLog(RecID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByRecID(long RecID)
		{
			QueryOrderTaskOperationLog query = new QueryOrderTaskOperationLog();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetOrderTaskOperationLog(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.OrderTaskOperationLog model)
		{
			return Dal.OrderTaskOperationLog.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.OrderTaskOperationLog model)
		{
			return Dal.OrderTaskOperationLog.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.OrderTaskOperationLog model)
		{
			return Dal.OrderTaskOperationLog.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.OrderTaskOperationLog model)
		{
			return Dal.OrderTaskOperationLog.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(long RecID)
		{
			
			return Dal.OrderTaskOperationLog.Instance.Delete(RecID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, long RecID)
		{
			
			return Dal.OrderTaskOperationLog.Instance.Delete(sqltran, RecID);
		}

		#endregion

	}
}

