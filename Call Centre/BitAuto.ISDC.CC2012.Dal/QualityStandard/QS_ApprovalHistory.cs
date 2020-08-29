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
	/// 数据访问类QS_ApprovalHistory。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-04-25 09:42:35 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class QS_ApprovalHistory : DataBase
	{
		#region Instance
		public static readonly QS_ApprovalHistory Instance = new QS_ApprovalHistory();
		#endregion

		#region const
		private const string P_QS_APPROVALHISTORY_SELECT = "p_QS_ApprovalHistory_Select";
		private const string P_QS_APPROVALHISTORY_INSERT = "p_QS_ApprovalHistory_Insert";
		private const string P_QS_APPROVALHISTORY_UPDATE = "p_QS_ApprovalHistory_Update";
		private const string P_QS_APPROVALHISTORY_DELETE = "p_QS_ApprovalHistory_Delete";
		#endregion

		#region Contructor
		protected QS_ApprovalHistory()
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
		public DataTable GetQS_ApprovalHistory(QueryQS_ApprovalHistory query, string order, int currentPage, int pageSize, out int totalCount)
		{
			string where = string.Empty;
            if (query.QS_RID != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                where += " And QS_RID="+query.QS_RID;
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_APPROVALHISTORY_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.QS_ApprovalHistory GetQS_ApprovalHistory(int RecID)
		{
			QueryQS_ApprovalHistory query = new QueryQS_ApprovalHistory();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetQS_ApprovalHistory(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleQS_ApprovalHistory(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.QS_ApprovalHistory LoadSingleQS_ApprovalHistory(DataRow row)
		{
			Entities.QS_ApprovalHistory model=new Entities.QS_ApprovalHistory();

				if(row["RecID"].ToString()!="")
				{
					model.RecID=int.Parse(row["RecID"].ToString());
				}
				if(row["QS_RTID"].ToString()!="")
				{
					model.QS_RTID=int.Parse(row["QS_RTID"].ToString());
				}
				if(row["QS_RID"].ToString()!="")
				{
					model.QS_RID=int.Parse(row["QS_RID"].ToString());
				}
				model.Type=row["Type"].ToString();
				model.ApprovalType=row["ApprovalType"].ToString();
				if(row["ApprovalResult"].ToString()!="")
				{
					model.ApprovalResult=int.Parse(row["ApprovalResult"].ToString());
				}
				model.Remark=row["Remark"].ToString();
				if(row["Status"].ToString()!="")
				{
					model.Status=int.Parse(row["Status"].ToString());
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
		public int Insert(Entities.QS_ApprovalHistory model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@QS_RID", SqlDbType.Int,4),
					new SqlParameter("@Type", SqlDbType.Char,10),
					new SqlParameter("@ApprovalType", SqlDbType.Char,10),
					new SqlParameter("@ApprovalResult", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.VarChar,2000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.QS_RTID;
			parameters[2].Value = model.QS_RID;
			parameters[3].Value = model.Type;
			parameters[4].Value = model.ApprovalType;
			parameters[5].Value = model.ApprovalResult;
			parameters[6].Value = model.Remark;
			parameters[7].Value = model.Status;
			parameters[8].Value = model.CreateTime;
			parameters[9].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_APPROVALHISTORY_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		/// <summary>
		///  增加一条数据
		/// </summary>
		public int Insert(SqlTransaction sqltran, Entities.QS_ApprovalHistory model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@QS_RID", SqlDbType.Int,4),
					new SqlParameter("@Type", SqlDbType.Char,10),
					new SqlParameter("@ApprovalType", SqlDbType.Char,10),
					new SqlParameter("@ApprovalResult", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.VarChar,2000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.QS_RTID;
			parameters[2].Value = model.QS_RID;
			parameters[3].Value = model.Type;
			parameters[4].Value = model.ApprovalType;
			parameters[5].Value = model.ApprovalResult;
			parameters[6].Value = model.Remark;
			parameters[7].Value = model.Status;
			parameters[8].Value = model.CreateTime;
			parameters[9].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_QS_APPROVALHISTORY_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(Entities.QS_ApprovalHistory model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@QS_RID", SqlDbType.Int,4),
					new SqlParameter("@Type", SqlDbType.Char,10),
					new SqlParameter("@ApprovalType", SqlDbType.Char,10),
					new SqlParameter("@ApprovalResult", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.VarChar,2000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.QS_RTID;
			parameters[2].Value = model.QS_RID;
			parameters[3].Value = model.Type;
			parameters[4].Value = model.ApprovalType;
			parameters[5].Value = model.ApprovalResult;
			parameters[6].Value = model.Remark;
			parameters[7].Value = model.Status;
			parameters[8].Value = model.CreateTime;
			parameters[9].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_APPROVALHISTORY_UPDATE,parameters);
		}
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.QS_ApprovalHistory model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@QS_RID", SqlDbType.Int,4),
					new SqlParameter("@Type", SqlDbType.Char,10),
					new SqlParameter("@ApprovalType", SqlDbType.Char,10),
					new SqlParameter("@ApprovalResult", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.VarChar,2000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.QS_RTID;
			parameters[2].Value = model.QS_RID;
			parameters[3].Value = model.Type;
			parameters[4].Value = model.ApprovalType;
			parameters[5].Value = model.ApprovalResult;
			parameters[6].Value = model.Remark;
			parameters[7].Value = model.Status;
			parameters[8].Value = model.CreateTime;
			parameters[9].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_QS_APPROVALHISTORY_UPDATE,parameters);
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

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_APPROVALHISTORY_DELETE,parameters);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, int RecID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
			parameters[0].Value = RecID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_QS_APPROVALHISTORY_DELETE,parameters);
		}
		#endregion

	}
}

