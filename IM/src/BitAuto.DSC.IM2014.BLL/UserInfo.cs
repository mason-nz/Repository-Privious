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
	/// 业务逻辑类UserInfo 的摘要说明。
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
		/// 按照查询条件查询
		/// </summary>
		/// <param name="query">查询条件</param>
		/// <param name="order">排序</param>
		/// <param name="currentPage">页号,-1不分页</param>
		/// <param name="pageSize">每页记录数</param>
		/// <param name="totalCount">总行数</param>
		/// <returns>集合</returns>
		public DataTable GetUserInfo(QueryUserInfo query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.UserInfo.Instance.GetUserInfo(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.UserInfo.Instance.GetUserInfo(new QueryUserInfo(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.UserInfo GetUserInfo(string UserID)
		{
			
			return Dal.UserInfo.Instance.GetUserInfo(UserID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
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
		/// 增加一条数据
		/// </summary>
		public void Insert(Entities.UserInfo model)
		{
			Dal.UserInfo.Instance.Insert(model);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Insert(SqlTransaction sqltran, Entities.UserInfo model)
		{
			Dal.UserInfo.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.UserInfo model)
		{
			return Dal.UserInfo.Instance.Update(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.UserInfo model)
		{
			return Dal.UserInfo.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(string UserID)
		{
			
			return Dal.UserInfo.Instance.Delete(UserID);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, string UserID)
		{
			
			return Dal.UserInfo.Instance.Delete(sqltran, UserID);
		}

		#endregion

	}
}

