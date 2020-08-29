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
	/// 业务逻辑类CarMasterBrand 的摘要说明。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-12-11 03:57:10 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class CarMasterBrand
	{
		#region Instance
		public static readonly CarMasterBrand Instance = new CarMasterBrand();
		#endregion

		#region Contructor
		protected CarMasterBrand()
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
        public DataTable GetCarMasterBrand(string query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.CarMasterBrand.Instance.GetCarMasterBrand(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.CarMasterBrand.Instance.GetCarMasterBrand(string.Empty, string.Empty, 1, 1000000, out totalCount);
        }

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.CarMasterBrand GetCarMasterBrand(int MasterBrandID)
		{
			
			return Dal.CarMasterBrand.Instance.GetCarMasterBrand(MasterBrandID);
		}

		#endregion

        //#region IsExists
        ///// <summary>
        ///// 是否存在该记录
        ///// </summary>
        //public bool IsExistsByMasterBrandID(int MasterBrandID)
        //{
        //    QueryCarMasterBrand query = new QueryCarMasterBrand();
        //    query.MasterBrandID = MasterBrandID;
        //    DataTable dt = new DataTable();
        //    int count = 0;
        //    dt = GetCarMasterBrand(query, string.Empty, 1, 1, out count);
        //    if (count > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //#endregion

		#region Insert
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Insert(Entities.CarMasterBrand model)
		{
			Dal.CarMasterBrand.Instance.Insert(model);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Insert(SqlTransaction sqltran, Entities.CarMasterBrand model)
		{
			Dal.CarMasterBrand.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.CarMasterBrand model)
		{
			return Dal.CarMasterBrand.Instance.Update(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.CarMasterBrand model)
		{
			return Dal.CarMasterBrand.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(int MasterBrandID)
		{
			
			return Dal.CarMasterBrand.Instance.Delete(MasterBrandID);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, int MasterBrandID)
		{
			
			return Dal.CarMasterBrand.Instance.Delete(sqltran, MasterBrandID);
		}

		#endregion


        internal void DeleteTable()
        {
            Dal.CarMasterBrand.Instance.DeleteTable();
        }

        internal DataTable GetAllListFromCrm2009()
        {
            return Dal.CarMasterBrand.Instance.GetAllListFromCrm2009();
        }
    }
}

