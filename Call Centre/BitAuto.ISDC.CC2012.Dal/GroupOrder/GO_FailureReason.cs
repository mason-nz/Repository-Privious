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
	/// ���ݷ�����GO_FailureReason��
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
		/// ���ղ�ѯ������ѯ
		/// </summary>
		/// <param name="query">��ѯ����</param>
		/// <param name="order">����</param>
		/// <param name="currentPage">ҳ��,-1����ҳ</param>
		/// <param name="pageSize">ÿҳ��¼��</param>
		/// <param name="totalCount">������</param>
		/// <returns>����</returns>
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
		/// �õ�һ������ʵ��
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
		///  ����һ������
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
		///  ����һ������
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
		///  ����һ������
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
		///  ����һ������
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
		/// ɾ��һ������
		/// </summary>
		public int Delete(int RecID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
			parameters[0].Value = RecID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_GO_FAILUREREASON_DELETE,parameters);
		}
		/// <summary>
		/// ɾ��һ������
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
        /// ��ѯʧ��ԭ��
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

