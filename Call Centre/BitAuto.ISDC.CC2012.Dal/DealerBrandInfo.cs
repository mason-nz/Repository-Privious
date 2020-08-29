using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Dal
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 数据访问类DealerBrandInfo。
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
	public class DealerBrandInfo : DataBase
	{
		#region Instance
		public static readonly DealerBrandInfo Instance = new DealerBrandInfo();
		#endregion

		#region const
		private const string P_DEALERBRANDINFO_SELECT = "p_DealerBrandInfo_Select";
		private const string P_DEALERBRANDINFO_INSERT = "p_DealerBrandInfo_Insert";
		private const string P_DEALERBRANDINFO_UPDATE = "p_DealerBrandInfo_Update";
		private const string P_DEALERBRANDINFO_DELETE = "p_DealerBrandInfo_Delete";
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
			string where = string.Empty;

            if (query != null && !string.IsNullOrEmpty(query.CustID))
            {
                where += " and a.custid='" + Utils.StringHelper.SqlFilter(query.CustID)+"'";
            }
            if (query != null &&query.DealerID!=-2)
            {
                where += " and a.DealerID=" + query.DealerID;
            }
            if (query != null && query.BrandID != -2)
            {
                where += " and a.BrandID=" + query.BrandID;
            }

			DataSet ds;

			SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

			parameters[0].Value = where;
			parameters[1].Value = order;
			parameters[2].Value = pageSize;
			parameters[3].Value = currentPage;
			parameters[4].Direction = ParameterDirection.Output;

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_DEALERBRANDINFO_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.DealerBrandInfo GetDealerBrandInfo(string CustID,int DealerID,int BrandID)
		{
			QueryDealerBrandInfo query = new QueryDealerBrandInfo();
			query.CustID = CustID;
			query.DealerID = DealerID;
			query.BrandID = BrandID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetDealerBrandInfo(query, string.Empty, 0, 1, out count);
			if (count > 0)
			{
				return LoadSingleDealerBrandInfo(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.DealerBrandInfo LoadSingleDealerBrandInfo(DataRow row)
		{
			Entities.DealerBrandInfo model=new Entities.DealerBrandInfo();

				model.CustID=row["CustID"].ToString();
				if(row["DealerID"].ToString()!="")
				{
					model.DealerID=int.Parse(row["DealerID"].ToString());
				}
				if(row["BrandID"].ToString()!="")
				{
					model.BrandID=int.Parse(row["BrandID"].ToString());
				}
				if(row["CreateTime"].ToString()!="")
				{
					model.CreateTime=DateTime.Parse(row["CreateTime"].ToString());
				}
				if(row["CreateUserID"].ToString()!="")
				{
					model.CreateUserID=int.Parse(row["CreateUserID"].ToString());
				}
				return model;
		}
		#endregion

		#region Insert
		/// <summary>
		///  增加一条数据
		/// </summary>
		public void Insert(Entities.DealerBrandInfo model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@DealerID", SqlDbType.Int,4),
					new SqlParameter("@BrandID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.CustID;
			parameters[1].Value = model.DealerID;
			parameters[2].Value = model.BrandID;
			parameters[3].Value = model.CreateTime;
			parameters[4].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_DEALERBRANDINFO_INSERT,parameters);
		}
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
        //public int Update(Entities.DealerBrandInfo model)
        //{
        //    SqlParameter[] parameters = {
        //            new SqlParameter("@CustID", SqlDbType.VarChar,20),
        //            new SqlParameter("@DealerID", SqlDbType.Int,4),
        //            new SqlParameter("@BrandID", SqlDbType.Int,4),
        //            new SqlParameter("@CreateTime", SqlDbType.DateTime),
        //            new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
        //    parameters[0].Value = model.CustID;
        //    parameters[1].Value = model.DealerID;
        //    parameters[2].Value = model.BrandID;
        //    parameters[3].Value = model.CreateTime;
        //    parameters[4].Value = model.CreateUserID;

        //    return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_DEALERBRANDINFO_UPDATE,parameters);
        //}
		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(string CustID)
		{
            SqlParameter[] parameters = {
					new SqlParameter("@CustID", SqlDbType.VarChar,50)};
			parameters[0].Value = CustID;


			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_DEALERBRANDINFO_DELETE,parameters);
		}
		#endregion

	}
}

