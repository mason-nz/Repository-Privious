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
	/// 数据访问类AgentStatusDetail。
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
	public class AgentStatusDetail : DataBase
	{
		#region Instance
		public static readonly AgentStatusDetail Instance = new AgentStatusDetail();
		#endregion

		#region const
		private const string P_AGENTSTATUSDETAIL_SELECT = "p_AgentStatusDetail_Select";
		private const string P_AGENTSTATUSDETAIL_INSERT = "p_AgentStatusDetail_Insert";
		private const string P_AGENTSTATUSDETAIL_UPDATE = "p_AgentStatusDetail_Update";
		private const string P_AGENTSTATUSDETAIL_DELETE = "p_AgentStatusDetail_Delete";
		#endregion

		#region Contructor
		protected AgentStatusDetail()
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
		public DataTable GetAgentStatusDetail(QueryAgentStatusDetail query, string order, int currentPage, int pageSize, out int totalCount)
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_AGENTSTATUSDETAIL_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.AgentStatusDetail GetAgentStatusDetail(long RecID)
		{
			QueryAgentStatusDetail query = new QueryAgentStatusDetail();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetAgentStatusDetail(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleAgentStatusDetail(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.AgentStatusDetail LoadSingleAgentStatusDetail(DataRow row)
		{
			Entities.AgentStatusDetail model=new Entities.AgentStatusDetail();

				if(row["RecID"].ToString()!="")
				{
					model.RecID=long.Parse(row["RecID"].ToString());
				}
				if(row["AgentID"].ToString()!="")
				{
					model.AgentID=row["AgentID"].ToString();
				}
				if(row["AgentStatus"].ToString()!="")
				{
					model.AgentStatus=int.Parse(row["AgentStatus"].ToString());
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
        public long Insert(Entities.AgentStatusDetail model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@AgentID", SqlDbType.VarChar,50),
					new SqlParameter("@AgentStatus", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.AgentID;
			parameters[2].Value = model.AgentStatus;
			parameters[3].Value = model.CreateTime;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_AGENTSTATUSDETAIL_INSERT,parameters);
			return (long)parameters[0].Value;
		}
		/// <summary>
		///  增加一条数据
		/// </summary>
		public int Insert(SqlTransaction sqltran, Entities.AgentStatusDetail model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@AgentID", SqlDbType.VarChar,50),
					new SqlParameter("@AgentStatus", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.AgentID;
			parameters[2].Value = model.AgentStatus;
			parameters[3].Value = model.CreateTime;

			SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_AGENTSTATUSDETAIL_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(Entities.AgentStatusDetail model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@AgentID", SqlDbType.VarChar,50),
					new SqlParameter("@AgentStatus", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.AgentID;
			parameters[2].Value = model.AgentStatus;
			parameters[3].Value = model.CreateTime;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_AGENTSTATUSDETAIL_UPDATE,parameters);
		}
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.AgentStatusDetail model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@AgentID", SqlDbType.VarChar,50),
					new SqlParameter("@AgentStatus", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.AgentID;
			parameters[2].Value = model.AgentStatus;
			parameters[3].Value = model.CreateTime;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_AGENTSTATUSDETAIL_UPDATE,parameters);
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

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_AGENTSTATUSDETAIL_DELETE,parameters);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, long RecID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt)};
			parameters[0].Value = RecID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_AGENTSTATUSDETAIL_DELETE,parameters);
		}
		#endregion

	}
}

