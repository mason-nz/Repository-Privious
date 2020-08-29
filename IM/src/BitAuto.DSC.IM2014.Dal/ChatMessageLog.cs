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
	/// 数据访问类ChatMessageLog。
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
	public class ChatMessageLog : DataBase
	{
		#region Instance
		public static readonly ChatMessageLog Instance = new ChatMessageLog();
		#endregion

		#region const
		private const string P_CHATMESSAGELOG_SELECT = "p_ChatMessageLog_Select";
		private const string P_CHATMESSAGELOG_INSERT = "p_ChatMessageLog_Insert";
		private const string P_CHATMESSAGELOG_UPDATE = "p_ChatMessageLog_Update";
		private const string P_CHATMESSAGELOG_DELETE = "p_ChatMessageLog_Delete";
		#endregion

		#region Contructor
		protected ChatMessageLog()
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
		public DataTable GetChatMessageLog(QueryChatMessageLog query, string order, int currentPage, int pageSize, out int totalCount)
		{
			string where = string.Empty;

            if (!string.IsNullOrEmpty(query.BeginTime) && !string.IsNullOrEmpty(query.EndTime))
            {
                where += " and (CreateTime between '" + query.BeginTime  + " 00:00:00' and '" + query.EndTime + " 23:59:59')";
            }            
            if (!string.IsNullOrEmpty(query.Sender))
            {
                where += " and Sender='" + query.Sender + "'";
            }
            if (!string.IsNullOrEmpty(query.Receiver))
            {
                where += " and Receiver='" + query.Receiver + "'";
            }
            if (query.AllocID != Constant.INT_INVALID_VALUE)
            {
                where += " and AllocID=" + query.AllocID;
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CHATMESSAGELOG_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.ChatMessageLog GetChatMessageLog(long MessageID)
		{
			QueryChatMessageLog query = new QueryChatMessageLog();
			query.MessageID = MessageID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetChatMessageLog(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleChatMessageLog(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.ChatMessageLog LoadSingleChatMessageLog(DataRow row)
		{
			Entities.ChatMessageLog model=new Entities.ChatMessageLog();

				if(row["MessageID"].ToString()!="")
				{
					model.MessageID=long.Parse(row["MessageID"].ToString());
				}
				if(row["AllocID"].ToString()!="")
				{
					model.AllocID=long.Parse(row["AllocID"].ToString());
				}
				model.Sender=row["Sender"].ToString();
				model.Receiver=row["Receiver"].ToString();
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
		public long Insert(Entities.ChatMessageLog model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@MessageID", SqlDbType.BigInt,8),
					new SqlParameter("@AllocID", SqlDbType.BigInt,8),
					new SqlParameter("@Sender", SqlDbType.VarChar,36),
					new SqlParameter("@Receiver", SqlDbType.VarChar,36),
					new SqlParameter("@Content", SqlDbType.VarChar,2000),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.AllocID;
			parameters[2].Value = model.Sender;
			parameters[3].Value = model.Receiver;
			parameters[4].Value = model.Content;
			parameters[5].Value = model.Type;
			parameters[6].Value = model.Status;
			parameters[7].Value = model.CreateTime;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CHATMESSAGELOG_INSERT,parameters);
			return (long)parameters[0].Value;
		}
		/// <summary>
		///  增加一条数据
		/// </summary>
		public int Insert(SqlTransaction sqltran, Entities.ChatMessageLog model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@MessageID", SqlDbType.BigInt,8),
					new SqlParameter("@AllocID", SqlDbType.BigInt,8),
					new SqlParameter("@Sender", SqlDbType.VarChar,36),
					new SqlParameter("@Receiver", SqlDbType.VarChar,36),
					new SqlParameter("@Content", SqlDbType.VarChar,2000),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.AllocID;
			parameters[2].Value = model.Sender;
			parameters[3].Value = model.Receiver;
			parameters[4].Value = model.Content;
			parameters[5].Value = model.Type;
			parameters[6].Value = model.Status;
			parameters[7].Value = model.CreateTime;

			SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CHATMESSAGELOG_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(Entities.ChatMessageLog model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@MessageID", SqlDbType.BigInt,8),
					new SqlParameter("@AllocID", SqlDbType.BigInt,8),
					new SqlParameter("@Sender", SqlDbType.VarChar,36),
					new SqlParameter("@Receiver", SqlDbType.VarChar,36),
					new SqlParameter("@Content", SqlDbType.VarChar,2000),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
			parameters[0].Value = model.MessageID;
			parameters[1].Value = model.AllocID;
			parameters[2].Value = model.Sender;
			parameters[3].Value = model.Receiver;
			parameters[4].Value = model.Content;
			parameters[5].Value = model.Type;
			parameters[6].Value = model.Status;
			parameters[7].Value = model.CreateTime;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CHATMESSAGELOG_UPDATE,parameters);
		}
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.ChatMessageLog model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@MessageID", SqlDbType.BigInt,8),
					new SqlParameter("@AllocID", SqlDbType.BigInt,8),
					new SqlParameter("@Sender", SqlDbType.VarChar,36),
					new SqlParameter("@Receiver", SqlDbType.VarChar,36),
					new SqlParameter("@Content", SqlDbType.VarChar,2000),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
			parameters[0].Value = model.MessageID;
			parameters[1].Value = model.AllocID;
			parameters[2].Value = model.Sender;
			parameters[3].Value = model.Receiver;
			parameters[4].Value = model.Content;
			parameters[5].Value = model.Type;
			parameters[6].Value = model.Status;
			parameters[7].Value = model.CreateTime;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CHATMESSAGELOG_UPDATE,parameters);
		}
		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(long MessageID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@MessageID", SqlDbType.BigInt)};
			parameters[0].Value = MessageID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CHATMESSAGELOG_DELETE,parameters);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, long MessageID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@MessageID", SqlDbType.BigInt)};
			parameters[0].Value = MessageID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CHATMESSAGELOG_DELETE,parameters);
		}
		#endregion

	}
}

