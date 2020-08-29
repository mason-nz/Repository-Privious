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
	/// 业务逻辑类UserDataRigth 的摘要说明。
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
		/// 按照查询条件查询
		/// </summary>
		/// <param name="query">查询条件</param>
		/// <param name="order">排序</param>
		/// <param name="currentPage">页号,-1不分页</param>
		/// <param name="pageSize">每页记录数</param>
		/// <param name="totalCount">总行数</param>
		/// <returns>集合</returns>
		public DataTable GetUserDataRigth(QueryUserDataRigth query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.UserDataRigth.Instance.GetUserDataRigth(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.UserDataRigth.Instance.GetUserDataRigth(new QueryUserDataRigth(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
        /// 得到一个对象实体(.RightType 0没有分配，1本人，2全部)
		/// </summary>
		public Entities.UserDataRigth GetUserDataRigth(int UserID)
		{			
			return Dal.UserDataRigth.Instance.GetUserDataRigth(UserID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
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
		/// 增加一条数据
		/// </summary>
		public void Insert(Entities.UserDataRigth model)
		{
			Dal.UserDataRigth.Instance.Insert(model);
		}

		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.UserDataRigth model)
		{
			return Dal.UserDataRigth.Instance.Update(model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(int UserID)
		{
			
			return Dal.UserDataRigth.Instance.Delete(UserID);
		}

		#endregion

	}
}

