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
	/// 业务逻辑类DealerBrandInfo 的摘要说明。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-07-27 10:30:16 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class DealerBrandInfo
	{
		#region Instance
		public static readonly DealerBrandInfo Instance = new DealerBrandInfo();
		#endregion

		#region Contructor
		protected DealerBrandInfo()
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
		public DataTable GetDealerBrandInfo(QueryDealerBrandInfo query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.DealerBrandInfo.Instance.GetDealerBrandInfo(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.DealerBrandInfo.Instance.GetDealerBrandInfo(new QueryDealerBrandInfo(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.DealerBrandInfo GetDealerBrandInfo(string CustID,int DealerID,int BrandID)
		{
			
			return Dal.DealerBrandInfo.Instance.GetDealerBrandInfo(CustID,DealerID,BrandID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool IsExistsByCustIDAndDealerIDAndBrandID(string CustID,int DealerID,int BrandID)
		{
			QueryDealerBrandInfo query = new QueryDealerBrandInfo();
			query.CustID = CustID;
			query.DealerID = DealerID;
			query.BrandID = BrandID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetDealerBrandInfo(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}


        public bool IsExistsByCustIDAndDealerIDAndBrandID(string CustID)
        {
            QueryDealerBrandInfo query = new QueryDealerBrandInfo();
            query.CustID = CustID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetDealerBrandInfo(query, string.Empty, 1, 1, out count);
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
		public void Insert(Entities.DealerBrandInfo model)
		{
			Dal.DealerBrandInfo.Instance.Insert(model);
		}

		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
        //public int Update(Entities.DealerBrandInfo model)
        //{
        //    return Dal.DealerBrandInfo.Instance.Update(model);
        //}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(string CustID)
		{
			
			return Dal.DealerBrandInfo.Instance.Delete(CustID);
		}

		#endregion

	}
}

