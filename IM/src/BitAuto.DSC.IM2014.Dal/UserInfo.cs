using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM2014.Entities;
using BitAuto.DSC.IM2014.Entities.Constants;

namespace BitAuto.DSC.IM2014.Dal
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 数据访问类UserInfo。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-03-05 10:05:58 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class UserInfo : DataBase
	{
		#region Instance
		public static readonly UserInfo Instance = new UserInfo();
		#endregion

		#region const
		private const string P_USERINFO_SELECT = "p_UserInfo_Select";
		private const string P_USERINFO_INSERT = "p_UserInfo_Insert";
		private const string P_USERINFO_UPDATE = "p_UserInfo_Update";
		private const string P_USERINFO_DELETE = "p_UserInfo_Delete";
		#endregion

		#region Contructor
		protected UserInfo()
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
		public DataTable GetUserInfo(QueryUserInfo query, string order, int currentPage, int pageSize, out int totalCount)
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERINFO_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.UserInfo GetUserInfo(string UserID)
		{
			QueryUserInfo query = new QueryUserInfo();
			query.UserID = UserID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetUserInfo(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleUserInfo(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.UserInfo LoadSingleUserInfo(DataRow row)
		{
			Entities.UserInfo model=new Entities.UserInfo();

				model.UserID=row["UserID"].ToString();
				if(row["Status"].ToString()!="")
				{
					model.Status=int.Parse(row["Status"].ToString());
				}
				if(row["CreateTime"].ToString()!="")
				{
					model.CreateTime=DateTime.Parse(row["CreateTime"].ToString());
				}
				return model;
		}
		#endregion

		#region Insert
		/// <summary>
		///  增加一条数据
		/// </summary>
		public void Insert(Entities.UserInfo model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.VarChar,36),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
			parameters[0].Value = model.UserID;
			parameters[1].Value = model.Status;
			parameters[2].Value = model.CreateTime;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERINFO_INSERT,parameters);
		}
		/// <summary>
		///  增加一条数据
		/// </summary>
		public void Insert(SqlTransaction sqltran, Entities.UserInfo model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.VarChar,36),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
			parameters[0].Value = model.UserID;
			parameters[1].Value = model.Status;
			parameters[2].Value = model.CreateTime;

			SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_USERINFO_INSERT,parameters);
		}
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(Entities.UserInfo model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.VarChar,36),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
			parameters[0].Value = model.UserID;
			parameters[1].Value = model.Status;
			parameters[2].Value = model.CreateTime;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERINFO_UPDATE,parameters);
		}
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.UserInfo model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.VarChar,36),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
			parameters[0].Value = model.UserID;
			parameters[1].Value = model.Status;
			parameters[2].Value = model.CreateTime;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_USERINFO_UPDATE,parameters);
		}
		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(string UserID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.VarChar,50)};
			parameters[0].Value = UserID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERINFO_DELETE,parameters);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, string UserID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.VarChar,50)};
			parameters[0].Value = UserID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_USERINFO_DELETE,parameters);
		}
		#endregion

	}
}

