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
	/// 数据访问类TType。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:36 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class TType : DataBase
	{
		#region Instance
		public static readonly TType Instance = new TType();
		#endregion

		#region const
		private const string P_TTYPE_SELECT = "p_TType_Select";
		private const string P_TTYPE_INSERT = "p_TType_Insert";
		private const string P_TTYPE_UPDATE = "p_TType_Update";
		private const string P_TTYPE_DELETE = "p_TType_Delete";
		#endregion

		#region Contructor
		protected TType()
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
		public DataTable GetTType(QueryTType query, string order, int currentPage, int pageSize, out int totalCount)
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TTYPE_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.TType GetTType(int RecID)
		{
			QueryTType query = new QueryTType();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetTType(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleTType(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.TType LoadSingleTType(DataRow row)
		{
			Entities.TType model=new Entities.TType();

				if(row["RecID"].ToString()!="")
				{
					model.RecID=int.Parse(row["RecID"].ToString());
				}
				model.TypeDesName=row["TypeDesName"].ToString();
				model.TypeName=row["TypeName"].ToString();
				model.TypeDes=row["TypeDes"].ToString();
				return model;
		}
		#endregion

		#region Insert
		/// <summary>
		///  增加一条数据
		/// </summary>
		public int Insert(Entities.TType model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@TypeDesName", SqlDbType.NVarChar,20),
					new SqlParameter("@TypeName", SqlDbType.NVarChar,20),
					new SqlParameter("@TypeDes", SqlDbType.Char,10)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.TypeDesName;
			parameters[2].Value = model.TypeName;
			parameters[3].Value = model.TypeDes;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TTYPE_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		/// <summary>
		///  增加一条数据
		/// </summary>
		public int Insert(SqlTransaction sqltran, Entities.TType model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@TypeDesName", SqlDbType.NVarChar,20),
					new SqlParameter("@TypeName", SqlDbType.NVarChar,20),
					new SqlParameter("@TypeDes", SqlDbType.Char,10)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.TypeDesName;
			parameters[2].Value = model.TypeName;
			parameters[3].Value = model.TypeDes;

			SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_TTYPE_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(Entities.TType model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@TypeDesName", SqlDbType.NVarChar,20),
					new SqlParameter("@TypeName", SqlDbType.NVarChar,20),
					new SqlParameter("@TypeDes", SqlDbType.Char,10)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.TypeDesName;
			parameters[2].Value = model.TypeName;
			parameters[3].Value = model.TypeDes;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TTYPE_UPDATE,parameters);
		}
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.TType model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@TypeDesName", SqlDbType.NVarChar,20),
					new SqlParameter("@TypeName", SqlDbType.NVarChar,20),
					new SqlParameter("@TypeDes", SqlDbType.Char,10)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.TypeDesName;
			parameters[2].Value = model.TypeName;
			parameters[3].Value = model.TypeDes;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_TTYPE_UPDATE,parameters);
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

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TTYPE_DELETE,parameters);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, int RecID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
			parameters[0].Value = RecID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_TTYPE_DELETE,parameters);
		}
		#endregion

	}
}

