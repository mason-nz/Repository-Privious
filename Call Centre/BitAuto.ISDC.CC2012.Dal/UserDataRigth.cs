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
	/// 数据访问类UserDataRigth。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-08-02 10:01:54 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class UserDataRigth : DataBase
	{
		#region Instance
		public static readonly UserDataRigth Instance = new UserDataRigth();
		#endregion

		#region const
		private const string P_USERDATARIGTH_SELECT = "p_UserDataRigth_Select";
		private const string P_USERDATARIGTH_INSERT = "p_UserDataRigth_Insert";
		private const string P_USERDATARIGTH_UPDATE = "p_UserDataRigth_Update";
		private const string P_USERDATARIGTH_DELETE = "p_UserDataRigth_Delete";
		#endregion

		#region Contructor
		protected UserDataRigth()
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
		public DataTable GetUserDataRigth(QueryUserDataRigth query, string order, int currentPage, int pageSize, out int totalCount)
		{
			string where = string.Empty;

            if (query.UserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND UserID=" + query.UserID.ToString() ;
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERDATARIGTH_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}
		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.UserDataRigth GetUserDataRigth(int UserID)
		{
			QueryUserDataRigth query = new QueryUserDataRigth();
			query.UserID = UserID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetUserDataRigth(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleUserDataRigth(dt.Rows[0]);
			}
			else
			{
                Entities.UserDataRigth model = new Entities.UserDataRigth();
                model.RightType = 0;
                return model;
			}
		}


		private Entities.UserDataRigth LoadSingleUserDataRigth(DataRow row)
		{
			Entities.UserDataRigth model=new Entities.UserDataRigth();

				if(row["UserID"].ToString()!="")
				{
					model.UserID=int.Parse(row["UserID"].ToString());
				}
				if(row["RightType"].ToString()!="")
				{
					model.RightType=int.Parse(row["RightType"].ToString());
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
		public void Insert(Entities.UserDataRigth model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@RightType", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.UserID;
			parameters[1].Value = model.RightType;
			parameters[2].Value = model.CreateTime;
			parameters[3].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERDATARIGTH_INSERT,parameters);
		}
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(Entities.UserDataRigth model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
                    new SqlParameter("@RightType",SqlDbType.Int),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.UserID;
            parameters[1].Value = model.RightType;
			parameters[2].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERDATARIGTH_UPDATE,parameters);
		}
		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(int UserID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)};
			parameters[0].Value = UserID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERDATARIGTH_DELETE,parameters);
		}
		#endregion

	}
}

