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
	/// 业务逻辑类UserMessage 的摘要说明。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-03-05 10:05:59 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class UserMessage
	{
		#region Instance
		public static readonly UserMessage Instance = new UserMessage();
		#endregion

		#region Contructor
		protected UserMessage()
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
		public DataTable GetUserMessage(QueryUserMessage query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.UserMessage.Instance.GetUserMessage(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.UserMessage.Instance.GetUserMessage(new QueryUserMessage(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.UserMessage GetUserMessage(long RecID)
		{
			
			return Dal.UserMessage.Instance.GetUserMessage(RecID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool IsExistsByRecID(long RecID)
		{
			QueryUserMessage query = new QueryUserMessage();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetUserMessage(query, string.Empty, 1, 1, out count);
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
		public long  Insert(Entities.UserMessage model)
		{
			return Dal.UserMessage.Instance.Insert(model);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.UserMessage model)
		{
			return Dal.UserMessage.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.UserMessage model)
		{
			return Dal.UserMessage.Instance.Update(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.UserMessage model)
		{
			return Dal.UserMessage.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(long RecID)
		{
			
			return Dal.UserMessage.Instance.Delete(RecID);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, long RecID)
		{
			
			return Dal.UserMessage.Instance.Delete(sqltran, RecID);
		}

		#endregion

	}
}

