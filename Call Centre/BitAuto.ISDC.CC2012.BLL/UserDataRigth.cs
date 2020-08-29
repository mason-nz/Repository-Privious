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
	/// ҵ���߼���UserDataRigth ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-08-02 10:01:54 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class UserDataRigth
	{
		#region Instance
		public static readonly UserDataRigth Instance = new UserDataRigth();
		#endregion

		#region Contructor
		protected UserDataRigth()
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
		public DataTable GetUserDataRigth(QueryUserDataRigth query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.UserDataRigth.Instance.GetUserDataRigth(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.UserDataRigth.Instance.GetUserDataRigth(new QueryUserDataRigth(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
        /// �õ�һ������ʵ��(.RightType 0û�з��䣬1���ˣ�2ȫ��)
		/// </summary>
		public Entities.UserDataRigth GetUserDataRigth(int UserID)
		{			
			return Dal.UserDataRigth.Instance.GetUserDataRigth(UserID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByUserID(int UserID)
		{
			QueryUserDataRigth query = new QueryUserDataRigth();
			query.UserID = UserID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetUserDataRigth(query, string.Empty, 1, 1, out count);
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
		public void Insert(Entities.UserDataRigth model)
		{
			Dal.UserDataRigth.Instance.Insert(model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.UserDataRigth model)
		{
			return Dal.UserDataRigth.Instance.Update(model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int UserID)
		{
			
			return Dal.UserDataRigth.Instance.Delete(UserID);
		}

		#endregion

	}
}

