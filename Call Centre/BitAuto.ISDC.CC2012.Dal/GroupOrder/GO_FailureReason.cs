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
	/// 数据访问类GO_FailureReason。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-11-04 09:34:13 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class GO_FailureReason : DataBase
	{
		#region Instance
		public static readonly GO_FailureReason Instance = new GO_FailureReason();
		#endregion

		#region const
        private const string P_GO_FAILUREREASON_SELECT = "P_GO_FAILUREREASON_SELECT";
		private const string P_GO_FAILUREREASON_INSERT = "p_GO_FailureReason_Insert";
		private const string P_GO_FAILUREREASON_UPDATE = "p_GO_FailureReason_Update";
		private const string P_GO_FAILUREREASON_DELETE = "p_GO_FailureReason_Delete";
		#endregion

		#region Contructor
		protected GO_FailureReason()
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
		public DataTable GetGO_FailureReason(QueryGO_FailureReason query, string order, int currentPage, int pageSize, out int totalCount)
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_GO_FAILUREREASON_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.GO_FailureReason GetGO_FailureReason(int RecID)
		{
			QueryGO_FailureReason query = new QueryGO_FailureReason();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetGO_FailureReason(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleGO_FailureReason(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.GO_FailureReason LoadSingleGO_FailureReason(DataRow row)
		{
			Entities.GO_FailureReason model=new Entities.GO_FailureReason();

				if(row["RecID"].ToString()!="")
				{
					model.RecID=int.Parse(row["RecID"].ToString());
				}
				model.ReasonName=row["ReasonName"].ToString();
				return model;
		}
		#endregion

		#region Insert
		/// <summary>
		///  增加一条数据
		/// </summary>
		public int Insert(Entities.GO_FailureReason model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@ReasonName", SqlDbType.VarChar,200)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.ReasonName;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_GO_FAILUREREASON_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		/// <summary>
		///  增加一条数据
		/// </summary>
		public int Insert(SqlTransaction sqltran, Entities.GO_FailureReason model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@ReasonName", SqlDbType.VarChar,200)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.ReasonName;

			SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_GO_FAILUREREASON_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(Entities.GO_FailureReason model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@ReasonName", SqlDbType.VarChar,200)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.ReasonName;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_GO_FAILUREREASON_UPDATE,parameters);
		}
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.GO_FailureReason model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@ReasonName", SqlDbType.VarChar,200)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.ReasonName;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_GO_FAILUREREASON_UPDATE,parameters);
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

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_GO_FAILUREREASON_DELETE,parameters);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, int RecID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
			parameters[0].Value = RecID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_GO_FAILUREREASON_DELETE,parameters);
		}
		#endregion

        /// <summary>
        /// 查询失败原因
        /// </summary>
        /// <returns></returns>
        public DataTable GetAll()
        {
            string sql = "SELECT * FROM dbo.GO_FailureReason";

            DataSet ds;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);

            return ds.Tables[0];
        }
	}
}

