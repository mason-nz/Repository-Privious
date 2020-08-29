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
	/// 业务逻辑类QS_Marking 的摘要说明。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-04-25 09:42:36 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class QS_Marking
	{
		#region Instance
		public static readonly QS_Marking Instance = new QS_Marking();
		#endregion

		#region Contructor
		protected QS_Marking()
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
		public DataTable GetQS_Marking(QueryQS_Marking query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.QS_Marking.Instance.GetQS_Marking(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.QS_Marking.Instance.GetQS_Marking(new QueryQS_Marking(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.QS_Marking GetQS_Marking(int QS_MID)
		{
			
			return Dal.QS_Marking.Instance.GetQS_Marking(QS_MID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool IsExistsByQS_MID(int QS_MID)
		{
			QueryQS_Marking query = new QueryQS_Marking();
			query.QS_MID = QS_MID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetQS_Marking(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.QS_Marking model)
		{
			return Dal.QS_Marking.Instance.Insert(model);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.QS_Marking model)
		{
			return Dal.QS_Marking.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.QS_Marking model)
		{
			return Dal.QS_Marking.Instance.Update(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.QS_Marking model)
		{
			return Dal.QS_Marking.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(int QS_MID)
		{
			
			return Dal.QS_Marking.Instance.Delete(QS_MID);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, int QS_MID)
		{
			
			return Dal.QS_Marking.Instance.Delete(sqltran, QS_MID);
		}

		#endregion

	}
}

