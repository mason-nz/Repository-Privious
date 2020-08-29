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
	/// 业务逻辑类OrderBuyCarInfo 的摘要说明。
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
		/// 按照查询条件查询
		/// </summary>
		/// <param name="query">查询条件</param>
		/// <param name="order">排序</param>
		/// <param name="currentPage">页号,-1不分页</param>
		/// <param name="pageSize">每页记录数</param>
		/// <param name="totalCount">总行数</param>
		/// <returns>集合</returns>
		public DataTable GetOrderBuyCarInfo(QueryOrderBuyCarInfo query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.OrderBuyCarInfo.Instance.GetOrderBuyCarInfo(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.OrderBuyCarInfo.Instance.GetOrderBuyCarInfo(new QueryOrderBuyCarInfo(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.OrderBuyCarInfo GetOrderBuyCarInfo(long TaskID)
		{
			
			return Dal.OrderBuyCarInfo.Instance.GetOrderBuyCarInfo(TaskID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
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
		/// 增加一条数据
		/// </summary>
		public void Insert(Entities.OrderBuyCarInfo model)
		{
			Dal.OrderBuyCarInfo.Instance.Insert(model);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Insert(SqlTransaction sqltran, Entities.OrderBuyCarInfo model)
		{
			Dal.OrderBuyCarInfo.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.OrderBuyCarInfo model)
		{
			return Dal.OrderBuyCarInfo.Instance.Update(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.OrderBuyCarInfo model)
		{
			return Dal.OrderBuyCarInfo.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(long TaskID)
		{
			
			return Dal.OrderBuyCarInfo.Instance.Delete(TaskID);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, long TaskID)
		{
			
			return Dal.OrderBuyCarInfo.Instance.Delete(sqltran, TaskID);
		}

		#endregion

	}
}

