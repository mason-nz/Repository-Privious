using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM2014.Entities;
using BitAuto.DSC.IM2014.Entities.Constants;

namespace BitAuto.DSC.IM2014.BLL
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// ҵ���߼���UserInfo ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-03-05 10:05:58 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class UserInfo
	{
		#region Instance
		public static readonly UserInfo Instance = new UserInfo();
		#endregion

		#region Contructor
		protected UserInfo()
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
		public DataTable GetUserInfo(QueryUserInfo query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.UserInfo.Instance.GetUserInfo(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.UserInfo.Instance.GetUserInfo(new QueryUserInfo(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.UserInfo GetUserInfo(string UserID)
		{
			
			return Dal.UserInfo.Instance.GetUserInfo(UserID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByUserID(string UserID)
		{
			QueryUserInfo query = new QueryUserInfo();
			query.UserID = UserID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetUserInfo(query, string.Empty, 1, 1, out count);
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
		public void Insert(Entities.UserInfo model)
		{
			Dal.UserInfo.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Insert(SqlTransaction sqltran, Entities.UserInfo model)
		{
			Dal.UserInfo.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.UserInfo model)
		{
			return Dal.UserInfo.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.UserInfo model)
		{
			return Dal.UserInfo.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(string UserID)
		{
			
			return Dal.UserInfo.Instance.Delete(UserID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, string UserID)
		{
			
			return Dal.UserInfo.Instance.Delete(sqltran, UserID);
		}

		#endregion

	}
}

