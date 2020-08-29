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
	/// 业务逻辑类OtherTaskWorkOrderMapping 的摘要说明。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-09-05 03:30:11 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class OtherTaskWorkOrderMapping
	{
		#region Instance
		public static readonly OtherTaskWorkOrderMapping Instance = new OtherTaskWorkOrderMapping();
		#endregion

		#region Contructor
		protected OtherTaskWorkOrderMapping()
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
		public DataTable GetOtherTaskWorkOrderMapping(QueryOtherTaskWorkOrderMapping query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.OtherTaskWorkOrderMapping.Instance.GetOtherTaskWorkOrderMapping(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.OtherTaskWorkOrderMapping.Instance.GetOtherTaskWorkOrderMapping(new QueryOtherTaskWorkOrderMapping(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.OtherTaskWorkOrderMapping GetOtherTaskWorkOrderMapping(int RecID)
		{
			
			return Dal.OtherTaskWorkOrderMapping.Instance.GetOtherTaskWorkOrderMapping(RecID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool IsExistsByRecID(int RecID)
		{
			QueryOtherTaskWorkOrderMapping query = new QueryOtherTaskWorkOrderMapping();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetOtherTaskWorkOrderMapping(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.OtherTaskWorkOrderMapping model)
		{
			return Dal.OtherTaskWorkOrderMapping.Instance.Insert(model);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.OtherTaskWorkOrderMapping model)
		{
			return Dal.OtherTaskWorkOrderMapping.Instance.Insert(sqltran, model);
		}

		#endregion

        //#region Update
        ///// <summary>
        ///// 更新一条数据
        ///// </summary>
        //public int Update(Entities.OtherTaskWorkOrderMapping model)
        //{
        //    return Dal.OtherTaskWorkOrderMapping.Instance.Update(model);
        //}

        ///// <summary>
        ///// 更新一条数据
        ///// </summary>
        //public int Update(SqlTransaction sqltran, Entities.OtherTaskWorkOrderMapping model)
        //{
        //    return Dal.OtherTaskWorkOrderMapping.Instance.Update(sqltran, model);
        //}

        //#endregion

        //#region Delete
        ///// <summary>
        ///// 删除一条数据
        ///// </summary>
        //public int Delete(int RecID)
        //{
			
        //    return Dal.OtherTaskWorkOrderMapping.Instance.Delete(RecID);
        //}

        ///// <summary>
        ///// 删除一条数据
        ///// </summary>
        //public int Delete(SqlTransaction sqltran, int RecID)
        //{
			
        //    return Dal.OtherTaskWorkOrderMapping.Instance.Delete(sqltran, RecID);
        //}

        //#endregion

	}
}

