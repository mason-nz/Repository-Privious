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
	/// ҵ���߼���AutoCall_ACDetail ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2015-09-14 09:57:57 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class AutoCall_ACDetail
	{
		#region Instance
		public static readonly AutoCall_ACDetail Instance = new AutoCall_ACDetail();
		#endregion

		#region Contructor
		protected AutoCall_ACDetail()
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
		public DataTable GetAutoCall_ACDetail(QueryAutoCall_ACDetail query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.AutoCall_ACDetail.Instance.GetAutoCall_ACDetail(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.AutoCall_ACDetail.Instance.GetAutoCall_ACDetail(new QueryAutoCall_ACDetail(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.AutoCall_ACDetail GetAutoCall_ACDetail(int RecID)
		{
			
			return Dal.AutoCall_ACDetail.Instance.GetAutoCall_ACDetail(RecID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByRecID(int RecID)
		{
			QueryAutoCall_ACDetail query = new QueryAutoCall_ACDetail();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetAutoCall_ACDetail(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.AutoCall_ACDetail model)
		{
			return Dal.AutoCall_ACDetail.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.AutoCall_ACDetail model)
		{
			return Dal.AutoCall_ACDetail.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.AutoCall_ACDetail model)
		{
			return Dal.AutoCall_ACDetail.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.AutoCall_ACDetail model)
		{
			return Dal.AutoCall_ACDetail.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int RecID)
		{
			
			return Dal.AutoCall_ACDetail.Instance.Delete(RecID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, int RecID)
		{
			
			return Dal.AutoCall_ACDetail.Instance.Delete(sqltran, RecID);
		}

		#endregion

	}
}

