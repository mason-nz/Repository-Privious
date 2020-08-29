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
	/// 数据访问类ConsultSolveUserMapping。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-07-27 10:30:12 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ConsultSolveUserMapping : DataBase
	{
		#region Instance
		public static readonly ConsultSolveUserMapping Instance = new ConsultSolveUserMapping();
		#endregion

		#region const
		private const string P_CONSULTSOLVEUSERMAPPING_SELECT = "p_ConsultSolveUserMapping_Select";
		private const string P_CONSULTSOLVEUSERMAPPING_INSERT = "p_ConsultSolveUserMapping_Insert";
		private const string P_CONSULTSOLVEUSERMAPPING_UPDATE = "p_ConsultSolveUserMapping_Update";
		private const string P_CONSULTSOLVEUSERMAPPING_DELETE = "p_ConsultSolveUserMapping_Delete";
        private const string P_CONSULTSOLVEUSERMAPPING_GETBYTEMPLATECATEGORYNAME = "p_ConsultSolveUserMapping_GetByTemplateCategoryName";
		#endregion

		#region Contructor
		protected ConsultSolveUserMapping()
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
		public DataTable GetConsultSolveUserMapping(QueryConsultSolveUserMapping query, string order, int currentPage, int pageSize, out int totalCount)
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONSULTSOLVEUSERMAPPING_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

        /// <summary>
        /// 根据模版分类名称查询受理人员
        /// </summary>
        /// <param name="templateCategoryName"></param>
        /// <returns></returns>
        public DataTable GetEmailConsultSolveUserByTemplateCategoryName(string firstTemplateCategoryName, string secondTemplateCategoryNames)
        {
            SqlParameter[] parameters ={ 
                                          new SqlParameter("@FirstCategoryName",SqlDbType.VarChar,50),
                                          new SqlParameter("@SecondCategoryNames", SqlDbType.VarChar,200)
                                       };
            parameters[0].Value = firstTemplateCategoryName.Trim();
            parameters[1].Value = secondTemplateCategoryNames.Trim();
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure
            , P_CONSULTSOLVEUSERMAPPING_GETBYTEMPLATECATEGORYNAME, parameters);
            return ds.Tables[0];
        }

        /// <summary>
        /// 根据模版ID查询相关信息
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public DataTable GetConsultSolveUserByTemplateID(int templateId)
        {
            string sqlStr = "SELECT * FROM ConsultSolveUserMapping WHERE TemplateID=@TemplateID";
            SqlParameter paramter = new SqlParameter("@TemplateID",templateId);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, paramter);
            return ds.Tables[0];
        }

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.ConsultSolveUserMapping GetConsultSolveUserMapping(int RecID)
		{
			QueryConsultSolveUserMapping query = new QueryConsultSolveUserMapping();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetConsultSolveUserMapping(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleConsultSolveUserMapping(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.ConsultSolveUserMapping LoadSingleConsultSolveUserMapping(DataRow row)
		{
			Entities.ConsultSolveUserMapping model=new Entities.ConsultSolveUserMapping();

				if(row["RecID"].ToString()!="")
				{
					model.RecID=int.Parse(row["RecID"].ToString());
				}
				if(row["TemplateID"].ToString()!="")
				{
					model.TemplateID=int.Parse(row["TemplateID"].ToString());
				}
				if(row["SolveUserEID"].ToString()!="")
				{
					model.SolveUserEID=int.Parse(row["SolveUserEID"].ToString());
				}
				if(row["SolveUserID"].ToString()!="")
				{
					model.SolveUserID=int.Parse(row["SolveUserID"].ToString());
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
		public int Insert(Entities.ConsultSolveUserMapping model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@TemplateID", SqlDbType.Int,4),
					new SqlParameter("@SolveUserEID", SqlDbType.Int,4),
					new SqlParameter("@SolveUserID", SqlDbType.Int,4),
					new SqlParameter("@SolveUserEName", SqlDbType.VarChar),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.TemplateID;
			parameters[2].Value = model.SolveUserEID;
			parameters[3].Value = model.SolveUserID;
			parameters[4].Value = model.SolveUserEName;
			parameters[5].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONSULTSOLVEUSERMAPPING_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(Entities.ConsultSolveUserMapping model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@TemplateID", SqlDbType.Int,4),
					new SqlParameter("@SolveUserEID", SqlDbType.Int,4),
					new SqlParameter("@SolveUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.TemplateID;
			parameters[2].Value = model.SolveUserEID;
			parameters[3].Value = model.SolveUserID;
			parameters[4].Value = model.CreateTime;
			parameters[5].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONSULTSOLVEUSERMAPPING_UPDATE,parameters);
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

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONSULTSOLVEUSERMAPPING_DELETE,parameters);
		}
		#endregion

	}
}

