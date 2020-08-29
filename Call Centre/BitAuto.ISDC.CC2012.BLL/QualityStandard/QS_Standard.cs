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
	/// 业务逻辑类QS_Standard 的摘要说明。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-04-25 09:42:37 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class QS_Standard
	{
		#region Instance
		public static readonly QS_Standard Instance = new QS_Standard();
		#endregion

		#region Contructor
		protected QS_Standard()
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
		public DataTable GetQS_Standard(QueryQS_Standard query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.QS_Standard.Instance.GetQS_Standard(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.QS_Standard.Instance.GetQS_Standard(new QueryQS_Standard(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.QS_Standard GetQS_Standard(int QS_SID)
		{
			
			return Dal.QS_Standard.Instance.GetQS_Standard(QS_SID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool IsExistsByQS_SID(int QS_SID)
		{
			QueryQS_Standard query = new QueryQS_Standard();
			query.QS_SID = QS_SID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetQS_Standard(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.QS_Standard model)
		{
			return Dal.QS_Standard.Instance.Insert(model);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.QS_Standard model)
		{
			return Dal.QS_Standard.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.QS_Standard model)
		{
			return Dal.QS_Standard.Instance.Update(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.QS_Standard model)
		{
			return Dal.QS_Standard.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(int QS_SID)
		{
			
			return Dal.QS_Standard.Instance.Delete(QS_SID);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, int QS_SID)
		{
			
			return Dal.QS_Standard.Instance.Delete(sqltran, QS_SID);
		}

		#endregion

	}
}

