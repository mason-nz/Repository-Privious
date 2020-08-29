using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_DMS2014.Entities;
using BitAuto.DSC.IM_DMS2014.Entities.Constants;

namespace BitAuto.DSC.IM_DMS2014.Dal
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 数据访问类ConverSationDetail。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-10-29 10:21:01 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ConverSationDetail : DataBase
	{
		#region Instance
		public static readonly ConverSationDetail Instance = new ConverSationDetail();
		#endregion

		#region const
		private const string P_CONVERSATIONDETAIL_SELECT = "p_ConverSationDetail_Select";
		private const string P_CONVERSATIONDETAIL_INSERT = "p_ConverSationDetail_Insert";
		private const string P_CONVERSATIONDETAIL_UPDATE = "p_ConverSationDetail_Update";
		private const string P_CONVERSATIONDETAIL_DELETE = "p_ConverSationDetail_Delete";
		#endregion

		#region Contructor
		protected ConverSationDetail()
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
		public DataTable GetConverSationDetail(QueryConverSationDetail query, string order, int currentPage, int pageSize, out int totalCount)
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONVERSATIONDETAIL_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.ConverSationDetail GetConverSationDetail(int RecID)
		{
			QueryConverSationDetail query = new QueryConverSationDetail();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetConverSationDetail(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleConverSationDetail(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.ConverSationDetail LoadSingleConverSationDetail(DataRow row)
		{
			Entities.ConverSationDetail model=new Entities.ConverSationDetail();

				if(row["RecID"].ToString()!="")
				{
					model.RecID=int.Parse(row["RecID"].ToString());
				}
				if(row["CSID"].ToString()!="")
				{
					model.CSID=int.Parse(row["CSID"].ToString());
				}
				if(row["Sender"].ToString()!="")
				{
					model.Sender=int.Parse(row["Sender"].ToString());
				}
				model.Content=row["Content"].ToString();
				if(row["Type"].ToString()!="")
				{
					model.Type=int.Parse(row["Type"].ToString());
				}
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
		public int Insert(Entities.ConverSationDetail model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CSID", SqlDbType.Int,4),
					new SqlParameter("@Sender", SqlDbType.TinyInt,1),
					new SqlParameter("@Content", SqlDbType.VarChar,500),
					new SqlParameter("@Type", SqlDbType.TinyInt,1),
					new SqlParameter("@Status", SqlDbType.TinyInt,1),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.CSID;
			parameters[2].Value = model.Sender;
			parameters[3].Value = model.Content;
			parameters[4].Value = model.Type;
			parameters[5].Value = model.Status;
			parameters[6].Value = model.CreateTime;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONVERSATIONDETAIL_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		/// <summary>
		///  增加一条数据
		/// </summary>
		public int Insert(SqlTransaction sqltran, Entities.ConverSationDetail model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CSID", SqlDbType.Int,4),
					new SqlParameter("@Sender", SqlDbType.TinyInt,1),
					new SqlParameter("@Content", SqlDbType.VarChar,500),
					new SqlParameter("@Type", SqlDbType.TinyInt,1),
					new SqlParameter("@Status", SqlDbType.TinyInt,1),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.CSID;
			parameters[2].Value = model.Sender;
			parameters[3].Value = model.Content;
			parameters[4].Value = model.Type;
			parameters[5].Value = model.Status;
			parameters[6].Value = model.CreateTime;

			SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CONVERSATIONDETAIL_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(Entities.ConverSationDetail model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CSID", SqlDbType.Int,4),
					new SqlParameter("@Sender", SqlDbType.TinyInt,1),
					new SqlParameter("@Content", SqlDbType.VarChar,500),
					new SqlParameter("@Type", SqlDbType.TinyInt,1),
					new SqlParameter("@Status", SqlDbType.TinyInt,1),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.CSID;
			parameters[2].Value = model.Sender;
			parameters[3].Value = model.Content;
			parameters[4].Value = model.Type;
			parameters[5].Value = model.Status;
			parameters[6].Value = model.CreateTime;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONVERSATIONDETAIL_UPDATE,parameters);
		}
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.ConverSationDetail model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CSID", SqlDbType.Int,4),
					new SqlParameter("@Sender", SqlDbType.TinyInt,1),
					new SqlParameter("@Content", SqlDbType.VarChar,500),
					new SqlParameter("@Type", SqlDbType.TinyInt,1),
					new SqlParameter("@Status", SqlDbType.TinyInt,1),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.CSID;
			parameters[2].Value = model.Sender;
			parameters[3].Value = model.Content;
			parameters[4].Value = model.Type;
			parameters[5].Value = model.Status;
			parameters[6].Value = model.CreateTime;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CONVERSATIONDETAIL_UPDATE,parameters);
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

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONVERSATIONDETAIL_DELETE,parameters);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, int RecID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
			parameters[0].Value = RecID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CONVERSATIONDETAIL_DELETE,parameters);
		}
		#endregion

	}
}

