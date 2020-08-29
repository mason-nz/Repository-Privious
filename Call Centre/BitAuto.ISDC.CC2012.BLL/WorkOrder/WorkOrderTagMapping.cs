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
	/// ҵ���߼���WorkOrderTagMapping ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-08-23 10:24:22 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class WorkOrderTagMapping
	{
		#region Instance
		public static readonly WorkOrderTagMapping Instance = new WorkOrderTagMapping();
		#endregion

		#region Contructor
		protected WorkOrderTagMapping()
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
		public DataTable GetWorkOrderTagMapping(QueryWorkOrderTagMapping query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.WorkOrderTagMapping.Instance.GetWorkOrderTagMapping(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.WorkOrderTagMapping.Instance.GetWorkOrderTagMapping(new QueryWorkOrderTagMapping(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.WorkOrderTagMapping GetWorkOrderTagMapping(int RecID)
		{
			
			return Dal.WorkOrderTagMapping.Instance.GetWorkOrderTagMapping(RecID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByRecID(int RecID)
		{
			QueryWorkOrderTagMapping query = new QueryWorkOrderTagMapping();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetWorkOrderTagMapping(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.WorkOrderTagMapping model)
		{
			return Dal.WorkOrderTagMapping.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.WorkOrderTagMapping model)
		{
			return Dal.WorkOrderTagMapping.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.WorkOrderTagMapping model)
		{
			return Dal.WorkOrderTagMapping.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.WorkOrderTagMapping model)
		{
			return Dal.WorkOrderTagMapping.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int RecID)
		{
			
			return Dal.WorkOrderTagMapping.Instance.Delete(RecID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, int RecID)
		{
			
			return Dal.WorkOrderTagMapping.Instance.Delete(sqltran, RecID);
		}

        public int DeleteByOrderID(string OrderID)
        {
            return Dal.WorkOrderTagMapping.Instance.DeleteByOrderID(OrderID);
        }
		#endregion

	}
}

