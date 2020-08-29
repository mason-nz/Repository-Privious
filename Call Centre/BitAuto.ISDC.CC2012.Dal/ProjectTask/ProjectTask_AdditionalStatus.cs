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
	/// 数据访问类ProjectTask_AdditionalStatus。
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
		/// 按照查询条件查询
		/// </summary>
		/// <param name="query">查询条件</param>
		/// <param name="order">排序</param>
		/// <param name="currentPage">页号,-1不分页</param>
		/// <param name="pageSize">每页记录数</param>
		/// <param name="totalCount">总行数</param>
		/// <returns>集合</returns>
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
		/// 得到一个对象实体
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
		///  增加一条数据
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
		///  增加一条数据
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
		///  更新一条数据
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
		///  更新一条数据
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
		/// 删除一条数据
		/// </summary>
		public int Delete(string PTID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@PTID", SqlDbType.VarChar,50)};
			parameters[0].Value = PTID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_ADDITIONALSTATUS_DELETE,parameters);
		}
		/// <summary>
		/// 删除一条数据
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

