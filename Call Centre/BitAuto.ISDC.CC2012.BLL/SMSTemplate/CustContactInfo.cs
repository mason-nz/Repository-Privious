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
	/// 业务逻辑类CustContactInfo 的摘要说明。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-12-23 06:17:00 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class CustContactInfo
	{
		#region Instance
		public static readonly CustContactInfo Instance = new CustContactInfo();
		#endregion

		#region Contructor
		protected CustContactInfo()
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
		public DataTable GetCustContactInfo(QueryCustContactInfo query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.CustContactInfo.Instance.GetCustContactInfo(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.CustContactInfo.Instance.GetCustContactInfo(new QueryCustContactInfo(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.CustContactInfo GetCustContactInfo(int RecID)
		{
			//该表无主键信息，请自定义主键/条件字段
			return Dal.CustContactInfo.Instance.GetCustContactInfo(RecID);
		}

        /// <summary>
        /// 根据个人用户ID获取实体
        /// </summary>
        /// <param name="CustID"></param>
        /// <returns></returns>
        public Entities.CustContactInfo GetCustContactInfoByCustID(string CustID)
        {
            return Dal.CustContactInfo.Instance.GetCustContactInfoByCustID(CustID);
        }

		#endregion

		#region IsExists

		#endregion

		#region Insert
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Insert(Entities.CustContactInfo model)
		{
			Dal.CustContactInfo.Instance.Insert(model);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Insert(SqlTransaction sqltran, Entities.CustContactInfo model)
		{
			Dal.CustContactInfo.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.CustContactInfo model)
		{
			return Dal.CustContactInfo.Instance.Update(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.CustContactInfo model)
		{
			return Dal.CustContactInfo.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(int RecID)
		{
			//该表无主键信息，请自定义主键/条件字段
			return Dal.CustContactInfo.Instance.Delete(RecID);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran,int RecID)
		{
			//该表无主键信息，请自定义主键/条件字段
			return Dal.CustContactInfo.Instance.Delete(sqltran,RecID);
		}

		#endregion

	}
}

