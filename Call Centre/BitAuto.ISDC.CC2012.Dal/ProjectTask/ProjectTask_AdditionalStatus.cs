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
	/// ���ݷ�����ProjectTask_AdditionalStatus��
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:28 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ProjectTask_AdditionalStatus : DataBase
	{
		#region Instance
		public static readonly ProjectTask_AdditionalStatus Instance = new ProjectTask_AdditionalStatus();
		#endregion

		#region const
		private const string P_PROJECTTASK_ADDITIONALSTATUS_SELECT = "p_ProjectTask_AdditionalStatus_Select";
		private const string P_PROJECTTASK_ADDITIONALSTATUS_INSERT = "p_ProjectTask_AdditionalStatus_Insert";
		private const string P_PROJECTTASK_ADDITIONALSTATUS_UPDATE = "p_ProjectTask_AdditionalStatus_Update";
		private const string P_PROJECTTASK_ADDITIONALSTATUS_DELETE = "p_ProjectTask_AdditionalStatus_Delete";
		#endregion

		#region Contructor
		protected ProjectTask_AdditionalStatus()
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
		public DataTable GetProjectTask_AdditionalStatus(QueryProjectTask_AdditionalStatus query, string order, int currentPage, int pageSize, out int totalCount)
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_ADDITIONALSTATUS_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.ProjectTask_AdditionalStatus GetProjectTask_AdditionalStatus(string PTID)
		{
			QueryProjectTask_AdditionalStatus query = new QueryProjectTask_AdditionalStatus();
			query.PTID = PTID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetProjectTask_AdditionalStatus(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleProjectTask_AdditionalStatus(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.ProjectTask_AdditionalStatus LoadSingleProjectTask_AdditionalStatus(DataRow row)
		{
			Entities.ProjectTask_AdditionalStatus model=new Entities.ProjectTask_AdditionalStatus();

				model.PTID=row["PTID"].ToString();
				model.AdditionalStatus=row["AdditionalStatus"].ToString();
				model.Description=row["Description"].ToString();
				return model;
		}
		#endregion

		#region Insert
		/// <summary>
		///  ����һ������
		/// </summary>
		public void Insert(Entities.ProjectTask_AdditionalStatus model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@PTID", SqlDbType.VarChar,20),
					new SqlParameter("@AdditionalStatus", SqlDbType.VarChar,16),
					new SqlParameter("@Description", SqlDbType.NVarChar,32)};
			parameters[0].Value = model.PTID;
			parameters[1].Value = model.AdditionalStatus;
			parameters[2].Value = model.Description;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_ADDITIONALSTATUS_INSERT,parameters);
		}
		/// <summary>
		///  ����һ������
		/// </summary>
		public void Insert(SqlTransaction sqltran, Entities.ProjectTask_AdditionalStatus model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@PTID", SqlDbType.VarChar,20),
					new SqlParameter("@AdditionalStatus", SqlDbType.VarChar,16),
					new SqlParameter("@Description", SqlDbType.NVarChar,32)};
			parameters[0].Value = model.PTID;
			parameters[1].Value = model.AdditionalStatus;
			parameters[2].Value = model.Description;

			SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_PROJECTTASK_ADDITIONALSTATUS_INSERT,parameters);
		}
		#endregion

		#region Update
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Update(Entities.ProjectTask_AdditionalStatus model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@PTID", SqlDbType.VarChar,20),
					new SqlParameter("@AdditionalStatus", SqlDbType.VarChar,16),
					new SqlParameter("@Description", SqlDbType.NVarChar,32)};
			parameters[0].Value = model.PTID;
			parameters[1].Value = model.AdditionalStatus;
			parameters[2].Value = model.Description;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_ADDITIONALSTATUS_UPDATE,parameters);
		}
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.ProjectTask_AdditionalStatus model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@PTID", SqlDbType.VarChar,20),
					new SqlParameter("@AdditionalStatus", SqlDbType.VarChar,16),
					new SqlParameter("@Description", SqlDbType.NVarChar,32)};
			parameters[0].Value = model.PTID;
			parameters[1].Value = model.AdditionalStatus;
			parameters[2].Value = model.Description;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_PROJECTTASK_ADDITIONALSTATUS_UPDATE,parameters);
		}
		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(string PTID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@PTID", SqlDbType.VarChar,50)};
			parameters[0].Value = PTID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_ADDITIONALSTATUS_DELETE,parameters);
		}
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, string PTID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@PTID", SqlDbType.VarChar,50)};
			parameters[0].Value = PTID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_PROJECTTASK_ADDITIONALSTATUS_DELETE,parameters);
		}
		#endregion

	}
}

