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
	/// ���ݷ�����CustCustomTableMapping��
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-07-27 10:30:13 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class CustCustomTableMapping : DataBase
	{
		#region Instance
		public static readonly CustCustomTableMapping Instance = new CustCustomTableMapping();
		#endregion

		#region const
		private const string P_CUSTCUSTOMTABLEMAPPING_SELECT = "p_CustCustomTableMapping_Select";
		private const string P_CUSTCUSTOMTABLEMAPPING_INSERT = "p_CustCustomTableMapping_Insert";
		private const string P_CUSTCUSTOMTABLEMAPPING_UPDATE = "p_CustCustomTableMapping_Update";
		private const string P_CUSTCUSTOMTABLEMAPPING_DELETE = "p_CustCustomTableMapping_Delete";
		#endregion

		#region Contructor
		protected CustCustomTableMapping()
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
		public DataTable GetCustCustomTableMapping(QueryCustCustomTableMapping query, string order, int currentPage, int pageSize, out int totalCount)
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTCUSTOMTABLEMAPPING_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.CustCustomTableMapping GetCustCustomTableMapping(long RecID)
		{
			QueryCustCustomTableMapping query = new QueryCustCustomTableMapping();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetCustCustomTableMapping(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleCustCustomTableMapping(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.CustCustomTableMapping LoadSingleCustCustomTableMapping(DataRow row)
		{
			Entities.CustCustomTableMapping model=new Entities.CustCustomTableMapping();

				if(row["RecID"].ToString()!="")
				{
					model.RecID=long.Parse(row["RecID"].ToString());
				}
				model.CustID=row["CustID"].ToString();
				if(row["ConsultID"].ToString()!="")
				{
					model.ConsultID=int.Parse(row["ConsultID"].ToString());
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
		///  ����һ������
		/// </summary>
		public int Insert(Entities.CustCustomTableMapping model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@ConsultID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.CustID;
			parameters[2].Value = model.ConsultID;
			parameters[3].Value = model.CreateTime;
			parameters[4].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTCUSTOMTABLEMAPPING_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Update(Entities.CustCustomTableMapping model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@ConsultID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.CustID;
			parameters[2].Value = model.ConsultID;
			parameters[3].Value = model.CreateTime;
			parameters[4].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTCUSTOMTABLEMAPPING_UPDATE,parameters);
		}
		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(long RecID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt)};
			parameters[0].Value = RecID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTCUSTOMTABLEMAPPING_DELETE,parameters);
		}
		#endregion

	}
}

