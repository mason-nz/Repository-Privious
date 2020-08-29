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
	/// ҵ���߼���UpdateOrderData ��ժҪ˵����
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
	public class UpdateOrderData
	{
		#region Instance
		public static readonly UpdateOrderData Instance = new UpdateOrderData();
		#endregion

		#region Contructor
		protected UpdateOrderData()
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
		public DataTable GetUpdateOrderData(QueryUpdateOrderData query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.UpdateOrderData.Instance.GetUpdateOrderData(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.UpdateOrderData.Instance.GetUpdateOrderData(new QueryUpdateOrderData(),string.Empty,1,1000000,out totalCount);
		}



		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.UpdateOrderData GetUpdateOrderData(int RecID)
		{
			
			return Dal.UpdateOrderData.Instance.GetUpdateOrderData(RecID);
		}

        public List<Entities.UpdateOrderData> GetUpdateOrderDataList(Entities.QueryUpdateOrderData query)
        {
            return Dal.UpdateOrderData.Instance.GetUpdateOrderDataList(query);
        }
		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByRecID(int RecID)
		{
			QueryUpdateOrderData query = new QueryUpdateOrderData();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetUpdateOrderData(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.UpdateOrderData model)
		{
			return Dal.UpdateOrderData.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.UpdateOrderData model)
		{
			return Dal.UpdateOrderData.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.UpdateOrderData model)
		{
			return Dal.UpdateOrderData.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.UpdateOrderData model)
		{
			return Dal.UpdateOrderData.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int RecID)
		{
			
			return Dal.UpdateOrderData.Instance.Delete(RecID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, int RecID)
		{
			
			return Dal.UpdateOrderData.Instance.Delete(sqltran, RecID);
		}

		#endregion

	}
}

