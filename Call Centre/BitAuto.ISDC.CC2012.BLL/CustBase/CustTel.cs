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
	/// 业务逻辑类CustTel 的摘要说明。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-07-27 10:30:15 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class CustTel
	{
		#region Instance
		public static readonly CustTel Instance = new CustTel();
		#endregion

		#region Contructor
		protected CustTel()
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
		public DataTable GetCustTel(QueryCustTel query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.CustTel.Instance.GetCustTel(query,order,currentPage,pageSize,out totalCount);
		}

        /// <summary>
        /// 获取客户下的所有电话
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        public DataTable GetCustTel(string custId)
        {
            return Dal.CustTel.Instance.GetCustTel(custId);
        }
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.CustTel.Instance.GetCustTel(new QueryCustTel(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.CustTel GetCustTel(string CustID,string Tel)
		{
			return Dal.CustTel.Instance.GetCustTel(CustID,Tel);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool IsExistsByCustIDAndTel(string CustID,string Tel)
		{
			QueryCustTel query = new QueryCustTel();
			query.CustID = CustID;
			query.Tel = Tel;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetCustTel(query, string.Empty, 1, 1, out count);
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
		public void Insert(Entities.CustTel model)
		{
			Dal.CustTel.Instance.Insert(model);
		}

	/// <summary>
		/// 增加一条数据
		/// </summary>
        public void Insert(SqlTransaction sqltran, Entities.CustTel model)
		{
            Dal.CustTel.Instance.Insert(sqltran,model);
		}

        /// <summary>
        /// 批量插入电话
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="tels"></param>
        public void Insert(string custId, string tels)
        {
            string[] telArry = tels.Split(',');
            foreach (string tel in telArry)
            {
                Entities.CustTel model = new Entities.CustTel();
                model.CreateTime = DateTime.Now;
                model.CreateUserID = BLL.Util.GetLoginUserID();
                model.CustID = custId;
                model.Tel = tel;
                BLL.CustTel.Instance.Insert(model);
            }
        }
		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.CustTel model)
		{
			return Dal.CustTel.Instance.Update(model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(string CustID)
		{
			
			return Dal.CustTel.Instance.Delete(CustID);
		}

		#endregion

	}
}

