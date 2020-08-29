using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.BLL
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 业务逻辑类GroupLabel 的摘要说明。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-10-29 10:21:03 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class GroupLabel
	{
		#region Instance
		public static readonly GroupLabel Instance = new GroupLabel();
		#endregion

		#region Contructor
		protected GroupLabel()
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
		public DataTable GetGroupLabel(QueryGroupLabel query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.GroupLabel.Instance.GetGroupLabel(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.GroupLabel.Instance.GetGroupLabel(new QueryGroupLabel(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.GroupLabel GetGroupLabel(int RecID)
		{
			
			return Dal.GroupLabel.Instance.GetGroupLabel(RecID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool IsExistsByRecID(int RecID)
		{
			QueryGroupLabel query = new QueryGroupLabel();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetGroupLabel(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.GroupLabel model)
		{
			return Dal.GroupLabel.Instance.Insert(model);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.GroupLabel model)
		{
			return Dal.GroupLabel.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.GroupLabel model)
		{
			return Dal.GroupLabel.Instance.Update(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.GroupLabel model)
		{
			return Dal.GroupLabel.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(int RecID)
		{
			
			return Dal.GroupLabel.Instance.Delete(RecID);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, int RecID)
		{
			
			return Dal.GroupLabel.Instance.Delete(sqltran, RecID);
		}

		#endregion

        public DataTable GetLabelConfig(string where)
        {
            return Dal.GroupLabel.Instance.GetLabelConfig(where);
        }

        /// <summary>
        /// 批量插入业务组-标签中间表数据
        /// </summary>
        /// <param name="bgid"></param>
        /// <param name="ltids"></param>
        /// <param name="userid"></param>
        public void SaveDataBatch(int bgid, string ltids, int userid)
        {
            Dal.GroupLabel.Instance.SaveDataBatch(bgid, ltids, userid);
        }
	}
}

