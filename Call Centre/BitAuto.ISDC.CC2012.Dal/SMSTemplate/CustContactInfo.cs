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
	/// ���ݷ�����CustContactInfo��
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-12-23 06:17:00 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class CustContactInfo : DataBase
	{
		#region Instance
		public static readonly CustContactInfo Instance = new CustContactInfo();
		#endregion

		#region const
		private const string P_CUSTCONTACTINFO_SELECT = "p_CustContactInfo_Select";
		private const string P_CUSTCONTACTINFO_INSERT = "p_CustContactInfo_Insert";
		private const string P_CUSTCONTACTINFO_UPDATE = "p_CustContactInfo_Update";
		private const string P_CUSTCONTACTINFO_DELETE = "p_CustContactInfo_Delete";
		#endregion

		#region Contructor
		protected CustContactInfo()
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
		public DataTable GetCustContactInfo(QueryCustContactInfo query, string order, int currentPage, int pageSize, out int totalCount)
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTCONTACTINFO_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.CustContactInfo GetCustContactInfo()
		{
			QueryCustContactInfo query = new QueryCustContactInfo();
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetCustContactInfo(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleCustContactInfo(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}

        public Entities.CustContactInfo GetCustContactInfo(int RecID)
        {
            string sqlStr = "SELECT * FROM CustContactInfo WHERE RecID=@RecID";
            SqlParameter parameter = new SqlParameter("@RecID", RecID);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return LoadSingleCustContactInfo(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ���ݸ����û�ID��ȡʵ��
        /// </summary>
        /// <param name="CustID"></param>
        /// <returns></returns>
        public Entities.CustContactInfo GetCustContactInfoByCustID(string CustID)
        {
            string sqlStr = "SELECT * FROM CustContactInfo WHERE CustID=@CustID";
            SqlParameter parameter = new SqlParameter("@CustID", CustID);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return LoadSingleCustContactInfo(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

		private Entities.CustContactInfo LoadSingleCustContactInfo(DataRow row)
		{
			Entities.CustContactInfo model=new Entities.CustContactInfo();

				if(row["RecID"].ToString()!="")
				{
					model.RecID=int.Parse(row["RecID"].ToString());
				}
				model.CustID=row["CustID"].ToString();
				model.CRMCustID=row["CRMCustID"].ToString();
				model.DepartMent=row["DepartMent"].ToString();
				model.Title=row["Title"].ToString();
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
		public void Insert(Entities.CustContactInfo model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@CRMCustID", SqlDbType.VarChar,50),
					new SqlParameter("@DepartMent", SqlDbType.VarChar,100),
					new SqlParameter("@Title", SqlDbType.VarChar,100),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.CustID;
			parameters[2].Value = model.CRMCustID;
			parameters[3].Value = model.DepartMent;
			parameters[4].Value = model.Title;
			parameters[5].Value = model.CreateTime;
			parameters[6].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTCONTACTINFO_INSERT,parameters);
		}
		/// <summary>
		///  ����һ������
		/// </summary>
		public void Insert(SqlTransaction sqltran, Entities.CustContactInfo model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@CRMCustID", SqlDbType.VarChar,50),
					new SqlParameter("@DepartMent", SqlDbType.VarChar,100),
					new SqlParameter("@Title", SqlDbType.VarChar,100),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.CustID;
			parameters[2].Value = model.CRMCustID;
			parameters[3].Value = model.DepartMent;
			parameters[4].Value = model.Title;
			parameters[5].Value = model.CreateTime;
			parameters[6].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CUSTCONTACTINFO_INSERT,parameters);
		}
		#endregion

		#region Update
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Update(Entities.CustContactInfo model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@CRMCustID", SqlDbType.VarChar,50),
					new SqlParameter("@DepartMent", SqlDbType.VarChar,100),
					new SqlParameter("@Title", SqlDbType.VarChar,100),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.CustID;
			parameters[2].Value = model.CRMCustID;
			parameters[3].Value = model.DepartMent;
			parameters[4].Value = model.Title;
			parameters[5].Value = model.CreateTime;
			parameters[6].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTCONTACTINFO_UPDATE,parameters);
		}
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.CustContactInfo model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@CRMCustID", SqlDbType.VarChar,50),
					new SqlParameter("@DepartMent", SqlDbType.VarChar,100),
					new SqlParameter("@Title", SqlDbType.VarChar,100),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.CustID;
			parameters[2].Value = model.CRMCustID;
			parameters[3].Value = model.DepartMent;
			parameters[4].Value = model.Title;
			parameters[5].Value = model.CreateTime;
			parameters[6].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CUSTCONTACTINFO_UPDATE,parameters);
		}
		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
        public int Delete(int RecID)
		{
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int)};
            parameters[0].Value = RecID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTCONTACTINFO_DELETE,parameters);
		}
		/// <summary>
		/// ɾ��һ������
		/// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
		{
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int)};
            parameters[0].Value = RecID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CUSTCONTACTINFO_DELETE,parameters);
		}
		#endregion

	}
}

