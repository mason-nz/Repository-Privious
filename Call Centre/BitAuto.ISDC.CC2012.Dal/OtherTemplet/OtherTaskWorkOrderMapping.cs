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
	/// ���ݷ�����OtherTaskWorkOrderMapping��
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-09-05 03:30:11 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class OtherTaskWorkOrderMapping : DataBase
	{
		#region Instance
		public static readonly OtherTaskWorkOrderMapping Instance = new OtherTaskWorkOrderMapping();
		#endregion

		#region const
		private const string P_OTHERTASKWORKORDERMAPPING_SELECT = "p_OtherTaskWorkOrderMapping_Select";
		private const string P_OTHERTASKWORKORDERMAPPING_INSERT = "p_OtherTaskWorkOrderMapping_Insert";
		private const string P_OTHERTASKWORKORDERMAPPING_UPDATE = "p_OtherTaskWorkOrderMapping_Update";
		private const string P_OTHERTASKWORKORDERMAPPING_DELETE = "p_OtherTaskWorkOrderMapping_Delete";
		#endregion

		#region Contructor
		protected OtherTaskWorkOrderMapping()
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
		public DataTable GetOtherTaskWorkOrderMapping(QueryOtherTaskWorkOrderMapping query, string order, int currentPage, int pageSize, out int totalCount)
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_OTHERTASKWORKORDERMAPPING_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.OtherTaskWorkOrderMapping GetOtherTaskWorkOrderMapping(int RecID)
		{
			QueryOtherTaskWorkOrderMapping query = new QueryOtherTaskWorkOrderMapping();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetOtherTaskWorkOrderMapping(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleOtherTaskWorkOrderMapping(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.OtherTaskWorkOrderMapping LoadSingleOtherTaskWorkOrderMapping(DataRow row)
		{
			Entities.OtherTaskWorkOrderMapping model=new Entities.OtherTaskWorkOrderMapping();

				if(row["RecID"].ToString()!="")
				{
					model.RecID=int.Parse(row["RecID"].ToString());
				}
				model.PTID=row["PTID"].ToString();
				model.OrderID=row["OrderID"].ToString();
				model.PCRMCustID=row["PCRMCustID"].ToString();
				model.OCRMCustID=row["OCRMCustID"].ToString();
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
		public int Insert(Entities.OtherTaskWorkOrderMapping model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@PTID", SqlDbType.VarChar,20),
					new SqlParameter("@OrderID", SqlDbType.VarChar,20),
					new SqlParameter("@PCRMCustID", SqlDbType.VarChar,20),
					new SqlParameter("@OCRMCustID", SqlDbType.VarChar,20),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.PTID;
			parameters[2].Value = model.OrderID;
			parameters[3].Value = model.PCRMCustID;
			parameters[4].Value = model.OCRMCustID;
			parameters[5].Value = model.CreateTime;
			parameters[6].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_OTHERTASKWORKORDERMAPPING_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Insert(SqlTransaction sqltran, Entities.OtherTaskWorkOrderMapping model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@PTID", SqlDbType.VarChar,20),
					new SqlParameter("@OrderID", SqlDbType.VarChar,20),
					new SqlParameter("@PCRMCustID", SqlDbType.VarChar,20),
					new SqlParameter("@OCRMCustID", SqlDbType.VarChar,20),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.PTID;
			parameters[2].Value = model.OrderID;
			parameters[3].Value = model.PCRMCustID;
			parameters[4].Value = model.OCRMCustID;
			parameters[5].Value = model.CreateTime;
			parameters[6].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_OTHERTASKWORKORDERMAPPING_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Update(Entities.OtherTaskWorkOrderMapping model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@PTID", SqlDbType.VarChar,20),
					new SqlParameter("@OrderID", SqlDbType.VarChar,20),
					new SqlParameter("@PCRMCustID", SqlDbType.VarChar,20),
					new SqlParameter("@OCRMCustID", SqlDbType.VarChar,20),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.PTID;
			parameters[2].Value = model.OrderID;
			parameters[3].Value = model.PCRMCustID;
			parameters[4].Value = model.OCRMCustID;
			parameters[5].Value = model.CreateTime;
			parameters[6].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_OTHERTASKWORKORDERMAPPING_UPDATE,parameters);
		}
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.OtherTaskWorkOrderMapping model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@PTID", SqlDbType.VarChar,20),
					new SqlParameter("@OrderID", SqlDbType.VarChar,20),
					new SqlParameter("@PCRMCustID", SqlDbType.VarChar,20),
					new SqlParameter("@OCRMCustID", SqlDbType.VarChar,20),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.PTID;
			parameters[2].Value = model.OrderID;
			parameters[3].Value = model.PCRMCustID;
			parameters[4].Value = model.OCRMCustID;
			parameters[5].Value = model.CreateTime;
			parameters[6].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_OTHERTASKWORKORDERMAPPING_UPDATE,parameters);
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

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_OTHERTASKWORKORDERMAPPING_DELETE,parameters);
		}
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, int RecID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
			parameters[0].Value = RecID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_OTHERTASKWORKORDERMAPPING_DELETE,parameters);
		}
		#endregion

	}
}

