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
	/// 业务逻辑类ProjectTask_AdditionalStatus 的摘要说明。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:28 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ProjectTask_AdditionalStatus
	{
		#region Instance
		public static readonly ProjectTask_AdditionalStatus Instance = new ProjectTask_AdditionalStatus();
		#endregion

		#region Contructor
		protected ProjectTask_AdditionalStatus()
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
		public DataTable GetProjectTask_AdditionalStatus(QueryProjectTask_AdditionalStatus query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.ProjectTask_AdditionalStatus.Instance.GetProjectTask_AdditionalStatus(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.ProjectTask_AdditionalStatus.Instance.GetProjectTask_AdditionalStatus(new QueryProjectTask_AdditionalStatus(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.ProjectTask_AdditionalStatus GetProjectTask_AdditionalStatus(string PTID)
		{
			
			return Dal.ProjectTask_AdditionalStatus.Instance.GetProjectTask_AdditionalStatus(PTID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool IsExistsByPTID(string PTID)
		{
			QueryProjectTask_AdditionalStatus query = new QueryProjectTask_AdditionalStatus();
			query.PTID = PTID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetProjectTask_AdditionalStatus(query, string.Empty, 1, 1, out count);
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
		public void Insert(Entities.ProjectTask_AdditionalStatus model)
		{
			Dal.ProjectTask_AdditionalStatus.Instance.Insert(model);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Insert(SqlTransaction sqltran, Entities.ProjectTask_AdditionalStatus model)
		{
			Dal.ProjectTask_AdditionalStatus.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.ProjectTask_AdditionalStatus model)
		{
			return Dal.ProjectTask_AdditionalStatus.Instance.Update(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.ProjectTask_AdditionalStatus model)
		{
			return Dal.ProjectTask_AdditionalStatus.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(string PTID)
		{
			
			return Dal.ProjectTask_AdditionalStatus.Instance.Delete(PTID);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, string PTID)
		{
			
			return Dal.ProjectTask_AdditionalStatus.Instance.Delete(sqltran, PTID);
		}

		#endregion

	}
}

