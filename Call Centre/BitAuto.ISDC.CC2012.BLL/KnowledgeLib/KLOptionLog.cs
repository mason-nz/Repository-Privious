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
	/// ҵ���߼���KLOptionLog ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-08-21 10:19:08 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class KLOptionLog
	{
		#region Instance
		public static readonly KLOptionLog Instance = new KLOptionLog();
		#endregion

		#region Contructor
		protected KLOptionLog()
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
		public DataTable GetKLOptionLog(QueryKLOptionLog query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.KLOptionLog.Instance.GetKLOptionLog(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.KLOptionLog.Instance.GetKLOptionLog(new QueryKLOptionLog(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.KLOptionLog GetKLOptionLog(long KLOptID)
		{
			
			return Dal.KLOptionLog.Instance.GetKLOptionLog(KLOptID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByKLOptID(long KLOptID)
		{
			QueryKLOptionLog query = new QueryKLOptionLog();
			query.KLOptID = KLOptID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetKLOptionLog(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.KLOptionLog model)
		{
			return Dal.KLOptionLog.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.KLOptionLog model)
		{
			return Dal.KLOptionLog.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.KLOptionLog model)
		{
			return Dal.KLOptionLog.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.KLOptionLog model)
		{
			return Dal.KLOptionLog.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(long KLOptID)
		{
			
			return Dal.KLOptionLog.Instance.Delete(KLOptID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, long KLOptID)
		{
			
			return Dal.KLOptionLog.Instance.Delete(sqltran, KLOptID);
		}

		#endregion

	}
}

