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
	/// 业务逻辑类KLOptionLog 的摘要说明。
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
		/// 按照查询条件查询
		/// </summary>
		/// <param name="query">查询条件</param>
		/// <param name="order">排序</param>
		/// <param name="currentPage">页号,-1不分页</param>
		/// <param name="pageSize">每页记录数</param>
		/// <param name="totalCount">总行数</param>
		/// <returns>集合</returns>
		public DataTable GetKLOptionLog(QueryKLOptionLog query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.KLOptionLog.Instance.GetKLOptionLog(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.KLOptionLog.Instance.GetKLOptionLog(new QueryKLOptionLog(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.KLOptionLog GetKLOptionLog(long KLOptID)
		{
			
			return Dal.KLOptionLog.Instance.GetKLOptionLog(KLOptID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
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
		/// 增加一条数据
		/// </summary>
		public int  Insert(Entities.KLOptionLog model)
		{
			return Dal.KLOptionLog.Instance.Insert(model);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.KLOptionLog model)
		{
			return Dal.KLOptionLog.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.KLOptionLog model)
		{
			return Dal.KLOptionLog.Instance.Update(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.KLOptionLog model)
		{
			return Dal.KLOptionLog.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(long KLOptID)
		{
			
			return Dal.KLOptionLog.Instance.Delete(KLOptID);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, long KLOptID)
		{
			
			return Dal.KLOptionLog.Instance.Delete(sqltran, KLOptID);
		}

		#endregion

	}
}

