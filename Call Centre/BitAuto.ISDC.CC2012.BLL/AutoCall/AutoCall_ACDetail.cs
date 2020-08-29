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
	/// 业务逻辑类AutoCall_ACDetail 的摘要说明。
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
		/// 按照查询条件查询
		/// </summary>
		/// <param name="query">查询条件</param>
		/// <param name="order">排序</param>
		/// <param name="currentPage">页号,-1不分页</param>
		/// <param name="pageSize">每页记录数</param>
		/// <param name="totalCount">总行数</param>
		/// <returns>集合</returns>
		public DataTable GetAutoCall_ACDetail(QueryAutoCall_ACDetail query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.AutoCall_ACDetail.Instance.GetAutoCall_ACDetail(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.AutoCall_ACDetail.Instance.GetAutoCall_ACDetail(new QueryAutoCall_ACDetail(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.AutoCall_ACDetail GetAutoCall_ACDetail(int RecID)
		{
			
			return Dal.AutoCall_ACDetail.Instance.GetAutoCall_ACDetail(RecID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
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
		/// 增加一条数据
		/// </summary>
		public int  Insert(Entities.AutoCall_ACDetail model)
		{
			return Dal.AutoCall_ACDetail.Instance.Insert(model);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.AutoCall_ACDetail model)
		{
			return Dal.AutoCall_ACDetail.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.AutoCall_ACDetail model)
		{
			return Dal.AutoCall_ACDetail.Instance.Update(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.AutoCall_ACDetail model)
		{
			return Dal.AutoCall_ACDetail.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(int RecID)
		{
			
			return Dal.AutoCall_ACDetail.Instance.Delete(RecID);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, int RecID)
		{
			
			return Dal.AutoCall_ACDetail.Instance.Delete(sqltran, RecID);
		}

		#endregion

	}
}

