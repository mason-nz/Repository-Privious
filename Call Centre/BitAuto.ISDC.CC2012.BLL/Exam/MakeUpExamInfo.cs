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
	/// 业务逻辑类MakeUpExamInfo 的摘要说明。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-09-03 02:04:21 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class MakeUpExamInfo
	{
		#region Instance
		public static readonly MakeUpExamInfo Instance = new MakeUpExamInfo();
		#endregion

		#region Contructor
		protected MakeUpExamInfo()
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
		public DataTable GetMakeUpExamInfo(QueryMakeUpExamInfo query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.MakeUpExamInfo.Instance.GetMakeUpExamInfo(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.MakeUpExamInfo.Instance.GetMakeUpExamInfo(new QueryMakeUpExamInfo(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
        public Entities.MakeUpExamInfo GetMakeUpExamInfo(int MEIID)
		{
			
			return Dal.MakeUpExamInfo.Instance.GetMakeUpExamInfo(MEIID);
		}


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.MakeUpExamInfo GetMakeUpExamInfoByEIID(int EIID)
        {

            return Dal.MakeUpExamInfo.Instance.GetMakeUpExamInfoByEIID(EIID);
        }

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool IsExistsByMEIID(int MEIID)
		{
			QueryMakeUpExamInfo query = new QueryMakeUpExamInfo();
			query.MEIID = MEIID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetMakeUpExamInfo(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.MakeUpExamInfo model)
		{
			return Dal.MakeUpExamInfo.Instance.Insert(model);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.MakeUpExamInfo model)
		{
			return Dal.MakeUpExamInfo.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.MakeUpExamInfo model)
		{
			return Dal.MakeUpExamInfo.Instance.Update(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.MakeUpExamInfo model)
		{
			return Dal.MakeUpExamInfo.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(int EIID)
		{
			
			return Dal.MakeUpExamInfo.Instance.Delete(EIID);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, int EIID)
		{
			
			return Dal.MakeUpExamInfo.Instance.Delete(sqltran, EIID);
		}

		#endregion

	}
}

