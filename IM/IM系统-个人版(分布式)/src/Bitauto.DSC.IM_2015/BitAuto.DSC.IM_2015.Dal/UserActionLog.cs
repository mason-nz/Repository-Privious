using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.Dal
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// ���ݷ�����UserActionLog��
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-10-29 10:21:04 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class UserActionLog : DataBase
	{
		#region Instance
		public static readonly UserActionLog Instance = new UserActionLog();
		#endregion

		#region const
        private const string P_USERACTIONLOG_SELECT = "p_UserActionLog_Select";
        private const string P_USERACTIONLOG_INSERT = "p_UserActionLog_Insert";
		private const string P_USERACTIONLOG_UPDATE = "p_UserActionLog_Update";
		private const string P_USERACTIONLOG_DELETE = "p_UserActionLog_Delete";
		#endregion

		#region Contructor
		protected UserActionLog()
		{}
		#endregion

		#region Select
		/// <summary>
		/// ���ղ�ѯ������ѯ
		/// </summary>
		/// <param name="query">��ѯ����</param>
		/// <param name="order">����</param>
		/// <param name="currentPage">ҳ��,-1����ҳ</param>
		/// <param name="pageSize">ÿҳ��¼��</param>
		/// <param name="totalCount">������</param>
		/// <returns>����</returns>
		public DataTable GetUserActionLog(QueryUserActionLog query, string order, int currentPage, int pageSize, out int totalCount)
		{
			string where = string.Empty;

            if (query.LogInfo != Constant.STRING_INVALID_VALUE)
            {
                where += " and a.LogInfo like '%" + StringHelper.SqlFilter(query.LogInfo) + "%'";
            }
            if (query.OperUserType != Constant.INT_INVALID_VALUE)
            {
                where += " and a.OperUserType="+query.OperUserType;
            }
            if (query.LogInType != Constant.INT_INVALID_VALUE)
            {
                where += " and a.LogInType=" + query.LogInType;
            }
            if (query.StartTime != Constant.STRING_INVALID_VALUE && query.EndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and a.createtime>='" + StringHelper.SqlFilter(query.StartTime) + "' and a.createtime<='" + StringHelper.SqlFilter(query.EndTime) + " 23:59:59'";
            }
            if (query.OperUserName != Constant.STRING_INVALID_VALUE)
            {
                where += " and (a.TrueName='" + StringHelper.SqlFilter(query.OperUserName) + "' or b.TrueName='" + StringHelper.SqlFilter(query.OperUserName) + "')";
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERACTIONLOG_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.UserActionLog GetUserActionLog(int RecID)
		{
			QueryUserActionLog query = new QueryUserActionLog();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetUserActionLog(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleUserActionLog(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.UserActionLog LoadSingleUserActionLog(DataRow row)
		{
			Entities.UserActionLog model=new Entities.UserActionLog();

				if(row["RecID"].ToString()!="")
				{
					model.RecID=int.Parse(row["RecID"].ToString());
				}
				model.LogInfo=row["LogInfo"].ToString();
				model.IP=row["IP"].ToString();
				if(row["CreateTime"].ToString()!="")
				{
					model.CreateTime=DateTime.Parse(row["CreateTime"].ToString());
				}
				if(row["CreateUserID"].ToString()!="")
				{
					model.CreateUserID=int.Parse(row["CreateUserID"].ToString());
				}
				model.TrueName=row["TrueName"].ToString();
				return model;
		}
		#endregion

		#region Insert
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Insert(Entities.UserActionLog model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
                    new SqlParameter("@OperUserType",SqlDbType.Int,4),
                    new SqlParameter("@LogInType",SqlDbType.Int,4),
					new SqlParameter("@LogInfo", SqlDbType.VarChar,2000),
					new SqlParameter("@IP", SqlDbType.VarChar,20),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@TrueName", SqlDbType.VarChar,50)};
			parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.OperUserType;
            parameters[2].Value = model.LogInType;
            parameters[3].Value = model.LogInfo;
			parameters[4].Value = model.IP;
			parameters[5].Value = model.CreateTime;
			parameters[6].Value = model.CreateUserID;
			parameters[7].Value = model.TrueName;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERACTIONLOG_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Insert(SqlTransaction sqltran, Entities.UserActionLog model)
		{
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
                    new SqlParameter("@OperUserType",SqlDbType.Int,4),
                    new SqlParameter("@LogInType",SqlDbType.Int,4),
					new SqlParameter("@LogInfo", SqlDbType.VarChar,2000),
					new SqlParameter("@IP", SqlDbType.VarChar,20),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@TrueName", SqlDbType.VarChar,50)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.OperUserType;
            parameters[2].Value = model.LogInType;
            parameters[3].Value = model.LogInfo;
            parameters[4].Value = model.IP;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.TrueName;

			SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_USERACTIONLOG_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Update(Entities.UserActionLog model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@LogInfo", SqlDbType.VarChar,2000),
					new SqlParameter("@IP", SqlDbType.VarChar,20),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@TrueName", SqlDbType.VarChar,50)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.LogInfo;
			parameters[2].Value = model.IP;
			parameters[3].Value = model.CreateTime;
			parameters[4].Value = model.CreateUserID;
			parameters[5].Value = model.TrueName;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERACTIONLOG_UPDATE,parameters);
		}
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.UserActionLog model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@LogInfo", SqlDbType.VarChar,2000),
					new SqlParameter("@IP", SqlDbType.VarChar,20),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@TrueName", SqlDbType.VarChar,50)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.LogInfo;
			parameters[2].Value = model.IP;
			parameters[3].Value = model.CreateTime;
			parameters[4].Value = model.CreateUserID;
			parameters[5].Value = model.TrueName;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_USERACTIONLOG_UPDATE,parameters);
		}
		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int RecID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
			parameters[0].Value = RecID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERACTIONLOG_DELETE,parameters);
		}
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, int RecID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
			parameters[0].Value = RecID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_USERACTIONLOG_DELETE,parameters);
		}
		#endregion

	}
}

