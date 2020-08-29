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
	/// 数据访问类LeadsTaskOperationLog。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-05-19 11:30:51 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class LeadsTaskOperationLog : DataBase
	{
		#region Instance
		public static readonly LeadsTaskOperationLog Instance = new LeadsTaskOperationLog();
		#endregion

		#region const
		private const string P_LEADSTASKOPERATIONLOG_SELECT = "p_LeadsTaskOperationLog_Select";
		private const string P_LEADSTASKOPERATIONLOG_INSERT = "p_LeadsTaskOperationLog_Insert";
		private const string P_LEADSTASKOPERATIONLOG_UPDATE = "p_LeadsTaskOperationLog_Update";
		private const string P_LEADSTASKOPERATIONLOG_DELETE = "p_LeadsTaskOperationLog_Delete";
		#endregion

		#region Contructor
		protected LeadsTaskOperationLog()
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
		public DataTable GetLeadsTaskOperationLog(QueryLeadsTaskOperationLog query, string order, int currentPage, int pageSize, out int totalCount)
		{
			string where = string.Empty;

			DataSet ds;
            where += " and OperationStatus!=-2 and TaskStatus!=-2";
            if (query.TaskID != Constant.STRING_INVALID_VALUE)
            {
                where += " and TaskID='" + StringHelper.SqlFilter(query.TaskID) + "'";
            }

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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_LEADSTASKOPERATIONLOG_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.LeadsTaskOperationLog GetLeadsTaskOperationLog(long RecID)
		{
			QueryLeadsTaskOperationLog query = new QueryLeadsTaskOperationLog();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetLeadsTaskOperationLog(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleLeadsTaskOperationLog(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.LeadsTaskOperationLog LoadSingleLeadsTaskOperationLog(DataRow row)
		{
			Entities.LeadsTaskOperationLog model=new Entities.LeadsTaskOperationLog();

				if(row["RecID"].ToString()!="")
				{
					model.RecID=long.Parse(row["RecID"].ToString());
				}
				model.TaskID=row["TaskID"].ToString();
				if(row["OperationStatus"].ToString()!="")
				{
					model.OperationStatus=int.Parse(row["OperationStatus"].ToString());
				}
				if(row["TaskStatus"].ToString()!="")
				{
					model.TaskStatus=int.Parse(row["TaskStatus"].ToString());
				}
				model.Remark=row["Remark"].ToString();
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
		public long Insert(Entities.LeadsTaskOperationLog model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@TaskID", SqlDbType.VarChar,50),
					new SqlParameter("@OperationStatus", SqlDbType.Int,4),
					new SqlParameter("@TaskStatus", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.NVarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.TaskID;
			parameters[2].Value = model.OperationStatus;
			parameters[3].Value = model.TaskStatus;
			parameters[4].Value = model.Remark;
			parameters[5].Value = model.CreateTime;
			parameters[6].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_LEADSTASKOPERATIONLOG_INSERT,parameters);
			return (long)parameters[0].Value;
		}
		/// <summary>
		///  增加一条数据
		/// </summary>
        public long Insert(SqlTransaction sqltran, Entities.LeadsTaskOperationLog model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@TaskID", SqlDbType.VarChar,50),
					new SqlParameter("@OperationStatus", SqlDbType.Int,4),
					new SqlParameter("@TaskStatus", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.NVarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.TaskID;
			parameters[2].Value = model.OperationStatus;
			parameters[3].Value = model.TaskStatus;
			parameters[4].Value = model.Remark;
			parameters[5].Value = model.CreateTime;
			parameters[6].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_LEADSTASKOPERATIONLOG_INSERT,parameters);
            return (long)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(Entities.LeadsTaskOperationLog model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@TaskID", SqlDbType.VarChar,50),
					new SqlParameter("@OperationStatus", SqlDbType.Int,4),
					new SqlParameter("@TaskStatus", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.NVarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.TaskID;
			parameters[2].Value = model.OperationStatus;
			parameters[3].Value = model.TaskStatus;
			parameters[4].Value = model.Remark;
			parameters[5].Value = model.CreateTime;
			parameters[6].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_LEADSTASKOPERATIONLOG_UPDATE,parameters);
		}
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.LeadsTaskOperationLog model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@TaskID", SqlDbType.VarChar,50),
					new SqlParameter("@OperationStatus", SqlDbType.Int,4),
					new SqlParameter("@TaskStatus", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.NVarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.TaskID;
			parameters[2].Value = model.OperationStatus;
			parameters[3].Value = model.TaskStatus;
			parameters[4].Value = model.Remark;
			parameters[5].Value = model.CreateTime;
			parameters[6].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_LEADSTASKOPERATIONLOG_UPDATE,parameters);
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

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_LEADSTASKOPERATIONLOG_DELETE,parameters);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, long RecID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt)};
			parameters[0].Value = RecID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_LEADSTASKOPERATIONLOG_DELETE,parameters);
		}
		#endregion

	}
}

