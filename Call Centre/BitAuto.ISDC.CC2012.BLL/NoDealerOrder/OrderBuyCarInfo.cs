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
	/// ҵ���߼���OrderBuyCarInfo ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-09-21 10:33:31 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class OrderBuyCarInfo
	{
		#region Instance
		public static readonly OrderBuyCarInfo Instance = new OrderBuyCarInfo();
		#endregion

		#region Contructor
		protected OrderBuyCarInfo()
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
		public DataTable GetOrderBuyCarInfo(QueryOrderBuyCarInfo query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.OrderBuyCarInfo.Instance.GetOrderBuyCarInfo(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.OrderBuyCarInfo.Instance.GetOrderBuyCarInfo(new QueryOrderBuyCarInfo(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.OrderBuyCarInfo GetOrderBuyCarInfo(long TaskID)
		{
			
			return Dal.OrderBuyCarInfo.Instance.GetOrderBuyCarInfo(TaskID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByTaskID(long TaskID)
		{
			QueryOrderBuyCarInfo query = new QueryOrderBuyCarInfo();
			query.TaskID = TaskID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetOrderBuyCarInfo(query, string.Empty, 1, 1, out count);
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
		public void Insert(Entities.OrderBuyCarInfo model)
		{
			Dal.OrderBuyCarInfo.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Insert(SqlTransaction sqltran, Entities.OrderBuyCarInfo model)
		{
			Dal.OrderBuyCarInfo.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.OrderBuyCarInfo model)
		{
			return Dal.OrderBuyCarInfo.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.OrderBuyCarInfo model)
		{
			return Dal.OrderBuyCarInfo.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(long TaskID)
		{
			
			return Dal.OrderBuyCarInfo.Instance.Delete(TaskID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, long TaskID)
		{
			
			return Dal.OrderBuyCarInfo.Instance.Delete(sqltran, TaskID);
		}

		#endregion

	}
}

