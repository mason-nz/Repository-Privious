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
	/// ҵ���߼���GO_FailureReason ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-11-04 09:34:13 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class GO_FailureReason
	{
		#region Instance
		public static readonly GO_FailureReason Instance = new GO_FailureReason();
		#endregion

		#region Contructor
		protected GO_FailureReason()
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
		public DataTable GetGO_FailureReason(QueryGO_FailureReason query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.GO_FailureReason.Instance.GetGO_FailureReason(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.GO_FailureReason.Instance.GetGO_FailureReason(new QueryGO_FailureReason(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.GO_FailureReason GetGO_FailureReason(int RecID)
		{
			
			return Dal.GO_FailureReason.Instance.GetGO_FailureReason(RecID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByRecID(int RecID)
		{
			QueryGO_FailureReason query = new QueryGO_FailureReason();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetGO_FailureReason(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.GO_FailureReason model)
		{
			return Dal.GO_FailureReason.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.GO_FailureReason model)
		{
			return Dal.GO_FailureReason.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.GO_FailureReason model)
		{
			return Dal.GO_FailureReason.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.GO_FailureReason model)
		{
			return Dal.GO_FailureReason.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int RecID)
		{
			
			return Dal.GO_FailureReason.Instance.Delete(RecID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, int RecID)
		{
			
			return Dal.GO_FailureReason.Instance.Delete(sqltran, RecID);
		}

		#endregion

        /// <summary>
        /// ��ѯʧ��ԭ��
        /// </summary>
        /// <returns></returns>
        public DataTable GetAll()
        {
            return Dal.GO_FailureReason.Instance.GetAll();
        }
	}
}

