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
	/// 数据访问类AutoCall_ACDetail。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2015-09-14 09:57:57 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class AutoCall_ACDetail : DataBase
	{
		#region Instance
		public static readonly AutoCall_ACDetail Instance = new AutoCall_ACDetail();
		#endregion

		#region const
		private const string P_AUTOCALL_ACDETAIL_SELECT = "p_AutoCall_ACDetail_Select";
		private const string P_AUTOCALL_ACDETAIL_INSERT = "p_AutoCall_ACDetail_Insert";
		private const string P_AUTOCALL_ACDETAIL_UPDATE = "p_AutoCall_ACDetail_Update";
		private const string P_AUTOCALL_ACDETAIL_DELETE = "p_AutoCall_ACDetail_Delete";
		#endregion

		#region Contructor
		protected AutoCall_ACDetail()
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
		public DataTable GetAutoCall_ACDetail(QueryAutoCall_ACDetail query, string order, int currentPage, int pageSize, out int totalCount)
		{
			string where = string.Empty;

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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_AUTOCALL_ACDETAIL_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.AutoCall_ACDetail GetAutoCall_ACDetail(int RecID)
		{
			QueryAutoCall_ACDetail query = new QueryAutoCall_ACDetail();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetAutoCall_ACDetail(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleAutoCall_ACDetail(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.AutoCall_ACDetail LoadSingleAutoCall_ACDetail(DataRow row)
		{
			Entities.AutoCall_ACDetail model=new Entities.AutoCall_ACDetail();

				if(row["RecID"].ToString()!="")
				{
					model.RecID=int.Parse(row["RecID"].ToString());
				}
				if(row["ACStatus"].ToString()!="")
				{
					model.ACStatus=int.Parse(row["ACStatus"].ToString());
				}
				if(row["ACTID"].ToString()!="")
				{
					model.ACTID=int.Parse(row["ACTID"].ToString());
				}
				if(row["ProjectID"].ToString()!="")
				{
					model.ProjectID=long.Parse(row["ProjectID"].ToString());
				}
				if(row["BusinessRecID"].ToString()!="")
				{
					model.BusinessRecID=int.Parse(row["BusinessRecID"].ToString());
				}
				if(row["ReturnTime"].ToString()!="")
				{
					model.ReturnTime=DateTime.Parse(row["ReturnTime"].ToString());
				}
				if(row["ACResult"].ToString()!="")
				{
					model.ACResult=int.Parse(row["ACResult"].ToString());
				}
				if(row["CreateTime"].ToString()!="")
				{
					model.CreateTime=DateTime.Parse(row["CreateTime"].ToString());
				}
				//model.Timestamp=row["Timestamp"].ToString();
				return model;
		}
		#endregion

		#region Insert
		/// <summary>
		///  增加一条数据
		/// </summary>
		public int Insert(Entities.AutoCall_ACDetail model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@ACStatus", SqlDbType.Int,4),
					new SqlParameter("@ACTID", SqlDbType.Int,4),
					new SqlParameter("@ProjectID", SqlDbType.BigInt,8),
					new SqlParameter("@BusinessRecID", SqlDbType.Int,4),
					new SqlParameter("@ReturnTime", SqlDbType.DateTime),
					new SqlParameter("@ACResult", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
					//new SqlParameter("@Timestamp", SqlDbType.Timestamp,8)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.ACStatus;
			parameters[2].Value = model.ACTID;
			parameters[3].Value = model.ProjectID;
			parameters[4].Value = model.BusinessRecID;
			parameters[5].Value = model.ReturnTime;
			parameters[6].Value = model.ACResult;
			parameters[7].Value = model.CreateTime;
			//parameters[8].Value = model.Timestamp;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_AUTOCALL_ACDETAIL_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		/// <summary>
		///  增加一条数据
		/// </summary>
		public int Insert(SqlTransaction sqltran, Entities.AutoCall_ACDetail model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@ACStatus", SqlDbType.Int,4),
					new SqlParameter("@ACTID", SqlDbType.Int,4),
					new SqlParameter("@ProjectID", SqlDbType.BigInt,8),
					new SqlParameter("@BusinessRecID", SqlDbType.Int,4),
					new SqlParameter("@ReturnTime", SqlDbType.DateTime),
					new SqlParameter("@ACResult", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
					//new SqlParameter("@Timestamp", SqlDbType.Timestamp,8)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.ACStatus;
			parameters[2].Value = model.ACTID;
			parameters[3].Value = model.ProjectID;
			parameters[4].Value = model.BusinessRecID;
			parameters[5].Value = model.ReturnTime;
			parameters[6].Value = model.ACResult;
			parameters[7].Value = model.CreateTime;
			//parameters[8].Value = model.Timestamp;

			SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_AUTOCALL_ACDETAIL_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(Entities.AutoCall_ACDetail model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@ACStatus", SqlDbType.Int,4),
					new SqlParameter("@ACTID", SqlDbType.Int,4),
					new SqlParameter("@ProjectID", SqlDbType.BigInt,8),
					new SqlParameter("@BusinessRecID", SqlDbType.Int,4),
					new SqlParameter("@ReturnTime", SqlDbType.DateTime),
					new SqlParameter("@ACResult", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
					//new SqlParameter("@Timestamp", SqlDbType.Timestamp,8)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.ACStatus;
			parameters[2].Value = model.ACTID;
			parameters[3].Value = model.ProjectID;
			parameters[4].Value = model.BusinessRecID;
			parameters[5].Value = model.ReturnTime;
			parameters[6].Value = model.ACResult;
			parameters[7].Value = model.CreateTime;
			//parameters[8].Value = model.Timestamp;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_AUTOCALL_ACDETAIL_UPDATE,parameters);
		}
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.AutoCall_ACDetail model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@ACStatus", SqlDbType.Int,4),
					new SqlParameter("@ACTID", SqlDbType.Int,4),
					new SqlParameter("@ProjectID", SqlDbType.BigInt,8),
					new SqlParameter("@BusinessRecID", SqlDbType.Int,4),
					new SqlParameter("@ReturnTime", SqlDbType.DateTime),
					new SqlParameter("@ACResult", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
					//new SqlParameter("@Timestamp", SqlDbType.Timestamp,8)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.ACStatus;
			parameters[2].Value = model.ACTID;
			parameters[3].Value = model.ProjectID;
			parameters[4].Value = model.BusinessRecID;
			parameters[5].Value = model.ReturnTime;
			parameters[6].Value = model.ACResult;
			parameters[7].Value = model.CreateTime;
			//parameters[8].Value = model.Timestamp;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_AUTOCALL_ACDETAIL_UPDATE,parameters);
		}
		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(int RecID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
			parameters[0].Value = RecID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_AUTOCALL_ACDETAIL_DELETE,parameters);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, int RecID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
			parameters[0].Value = RecID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_AUTOCALL_ACDETAIL_DELETE,parameters);
		}
		#endregion

	}
}

