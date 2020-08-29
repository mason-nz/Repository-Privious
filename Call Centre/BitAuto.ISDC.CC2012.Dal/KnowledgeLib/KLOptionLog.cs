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
	/// ���ݷ�����KLOptionLog��
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-08-21 10:19:08 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class KLOptionLog : DataBase
	{
		#region Instance
		public static readonly KLOptionLog Instance = new KLOptionLog();
		#endregion

		#region const
		private const string P_KLOPTIONLOG_SELECT = "p_KLOptionLog_Select";
		private const string P_KLOPTIONLOG_INSERT = "p_KLOptionLog_Insert";
		private const string P_KLOPTIONLOG_UPDATE = "p_KLOptionLog_Update";
		private const string P_KLOPTIONLOG_DELETE = "p_KLOptionLog_Delete";
		#endregion

		#region Contructor
		protected KLOptionLog()
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
		public DataTable GetKLOptionLog(QueryKLOptionLog query, string order, int currentPage, int pageSize, out int totalCount)
		{
			string where = string.Empty;

            if (query.KLID != Constant.INT_INVALID_VALUE)
            {
                where += " and klid=" + query.KLID + "";
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLOPTIONLOG_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.KLOptionLog GetKLOptionLog(long KLOptID)
		{
			QueryKLOptionLog query = new QueryKLOptionLog();
			query.KLOptID = KLOptID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetKLOptionLog(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleKLOptionLog(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.KLOptionLog LoadSingleKLOptionLog(DataRow row)
		{
			Entities.KLOptionLog model=new Entities.KLOptionLog();

				if(row["KLOptID"].ToString()!="")
				{
					model.KLOptID=long.Parse(row["KLOptID"].ToString());
				}
				if(row["KLID"].ToString()!="")
				{
					model.KLID=long.Parse(row["KLID"].ToString());
				}
				if(row["OptStatus"].ToString()!="")
				{
					model.OptStatus=int.Parse(row["OptStatus"].ToString());
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
		///  ����һ������
		/// </summary>
		public int Insert(Entities.KLOptionLog model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@KLOptID", SqlDbType.BigInt,8),
					new SqlParameter("@KLID", SqlDbType.BigInt,8),
					new SqlParameter("@OptStatus", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.NVarChar,200),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.KLID;
			parameters[2].Value = model.OptStatus;
			parameters[3].Value = model.Remark;
			parameters[4].Value = model.CreateTime;
			parameters[5].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLOPTIONLOG_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Insert(SqlTransaction sqltran, Entities.KLOptionLog model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@KLOptID", SqlDbType.Int,8),
					new SqlParameter("@KLID", SqlDbType.Int,8),
					new SqlParameter("@OptStatus", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.NVarChar,200),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.KLID;
			parameters[2].Value = model.OptStatus;
			parameters[3].Value = model.Remark;
			parameters[4].Value = model.CreateTime;
			parameters[5].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(sqltran,  CommandType.StoredProcedure, P_KLOPTIONLOG_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Update(Entities.KLOptionLog model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@KLOptID", SqlDbType.BigInt,8),
					new SqlParameter("@KLID", SqlDbType.BigInt,8),
					new SqlParameter("@OptStatus", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.NVarChar,200),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.KLOptID;
			parameters[1].Value = model.KLID;
			parameters[2].Value = model.OptStatus;
			parameters[3].Value = model.Remark;
			parameters[4].Value = model.CreateTime;
			parameters[5].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLOPTIONLOG_UPDATE,parameters);
		}
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.KLOptionLog model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@KLOptID", SqlDbType.BigInt,8),
					new SqlParameter("@KLID", SqlDbType.BigInt,8),
					new SqlParameter("@OptStatus", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.NVarChar,200),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.KLOptID;
			parameters[1].Value = model.KLID;
			parameters[2].Value = model.OptStatus;
			parameters[3].Value = model.Remark;
			parameters[4].Value = model.CreateTime;
			parameters[5].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLOPTIONLOG_UPDATE,parameters);
		}
		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(long KLOptID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@KLOptID", SqlDbType.BigInt)};
			parameters[0].Value = KLOptID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLOPTIONLOG_DELETE,parameters);
		}
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, long KLOptID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@KLOptID", SqlDbType.BigInt)};
			parameters[0].Value = KLOptID;

			return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLOPTIONLOG_DELETE,parameters);
		}
		#endregion

	}
}

