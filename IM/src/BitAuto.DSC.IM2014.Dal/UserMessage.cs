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
	/// 数据访问类UserMessage。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-03-05 10:05:59 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class UserMessage : DataBase
	{
		#region Instance
		public static readonly UserMessage Instance = new UserMessage();
		#endregion

		#region const
		private const string P_USERMESSAGE_SELECT = "p_UserMessage_Select";
		private const string P_USERMESSAGE_INSERT = "p_UserMessage_Insert";
		private const string P_USERMESSAGE_UPDATE = "p_UserMessage_Update";
		private const string P_USERMESSAGE_DELETE = "p_UserMessage_Delete";
		#endregion

		#region Contructor
		protected UserMessage()
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
		public DataTable GetUserMessage(QueryUserMessage query, string order, int currentPage, int pageSize, out int totalCount)
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERMESSAGE_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.UserMessage GetUserMessage(long RecID)
		{
			QueryUserMessage query = new QueryUserMessage();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetUserMessage(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleUserMessage(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.UserMessage LoadSingleUserMessage(DataRow row)
		{
			Entities.UserMessage model=new Entities.UserMessage();

				if(row["RecID"].ToString()!="")
				{
					model.RecID=long.Parse(row["RecID"].ToString());
				}
				model.UserID=row["UserID"].ToString();
				model.Content=row["Content"].ToString();
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
		public long Insert(Entities.UserMessage model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@UserID", SqlDbType.VarChar,36),
					new SqlParameter("@Content", SqlDbType.VarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
                    new SqlParameter("@TypeID",SqlDbType.Int,4),
                    new SqlParameter("@Email", SqlDbType.VarChar,200),
                    new SqlParameter("@UserName", SqlDbType.NVarChar,200),
                    new SqlParameter("@Phone", SqlDbType.VarChar,100)
                                        };
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.UserID;
			parameters[2].Value = model.Content;
			parameters[3].Value = model.CreateTime;
            parameters[4].Value = model.TypeID;
            parameters[5].Value = model.Email;
            parameters[6].Value = model.UserName;
            parameters[7].Value = model.Pheone;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERMESSAGE_INSERT,parameters);
			return (long)parameters[0].Value;
		}
		/// <summary>
		///  增加一条数据
		/// </summary>
		public int Insert(SqlTransaction sqltran, Entities.UserMessage model)
		{
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@UserID", SqlDbType.VarChar,36),
					new SqlParameter("@Content", SqlDbType.VarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
                    new SqlParameter("@TypeID",SqlDbType.Int,4),
                    new SqlParameter("@Email", SqlDbType.VarChar,200),
                    new SqlParameter("@UserName", SqlDbType.NVarChar,200),
                    new SqlParameter("@Phone", SqlDbType.VarChar,100)
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.Content;
            parameters[3].Value = model.CreateTime;
            parameters[4].Value = model.TypeID;
            parameters[5].Value = model.Email;
            parameters[6].Value = model.UserName;
            parameters[7].Value = model.Pheone;

			SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_USERMESSAGE_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(Entities.UserMessage model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@UserID", SqlDbType.VarChar,36),
					new SqlParameter("@Content", SqlDbType.VarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.UserID;
			parameters[2].Value = model.Content;
			parameters[3].Value = model.CreateTime;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERMESSAGE_UPDATE,parameters);
		}
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.UserMessage model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@UserID", SqlDbType.VarChar,36),
					new SqlParameter("@Content", SqlDbType.VarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.UserID;
			parameters[2].Value = model.Content;
			parameters[3].Value = model.CreateTime;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_USERMESSAGE_UPDATE,parameters);
		}
		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(long RecID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt)};
			parameters[0].Value = RecID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERMESSAGE_DELETE,parameters);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, long RecID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt)};
			parameters[0].Value = RecID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_USERMESSAGE_DELETE,parameters);
		}
		#endregion

	}
}

