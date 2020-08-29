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
	/// 数据访问类TemplateCategory。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-07-27 10:30:22 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class TemplateCategory : DataBase
	{
		#region Instance
		public static readonly TemplateCategory Instance = new TemplateCategory();
		#endregion

		#region const
		private const string P_TEMPLATECATEGORY_SELECT = "p_TemplateCategory_Select";
		private const string P_TEMPLATECATEGORY_INSERT = "p_TemplateCategory_Insert";
		private const string P_TEMPLATECATEGORY_UPDATE = "p_TemplateCategory_Update";
		private const string P_TEMPLATECATEGORY_DELETE = "p_TemplateCategory_Delete";
        private const string P_TEMPLATECATEGORY_GETBYTEMPLATECATEGORYNAME = "p_TemplateCategory_GetByTemplateCategoryName";
		#endregion

		#region Contructor
		protected TemplateCategory()
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
        public DataTable GetTemplateCategory(QueryTemplateCategory query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.Level != Constant.INT_INVALID_VALUE)
            {
                where += " and level=" + query.Level;
            }
            if (query.Pid != Constant.INT_INVALID_VALUE)
            {
                where += " and pID=" + query.Pid;
            }
            if (query.Type != Constant.INT_INVALID_VALUE)
            {
                where += " and type=" + query.Type;
            }
            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " and RecID=" + query.RecID;
            }
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " and Status=" + query.Status;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TEMPLATECATEGORY_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        public DataTable GetTemplateCategoryByTemplateCategoryName(string firstTemplateCategoryName, string secondTemplateCategoryNames)
        {
            SqlParameter[] parameters ={ 
                                          new SqlParameter("@FirstCategoryName",SqlDbType.VarChar,50),
                                          new SqlParameter("@SecondCategoryNames", SqlDbType.VarChar,200)
                                       };
            parameters[0].Value = firstTemplateCategoryName.Trim();
            parameters[1].Value = secondTemplateCategoryNames.Trim();
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure
            , P_TEMPLATECATEGORY_GETBYTEMPLATECATEGORYNAME, parameters);
            return ds.Tables[0];
        }
		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.TemplateCategory GetTemplateCategory(int RecID)
		{
			QueryTemplateCategory query = new QueryTemplateCategory();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetTemplateCategory(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleTemplateCategory(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.TemplateCategory LoadSingleTemplateCategory(DataRow row)
		{
			Entities.TemplateCategory model=new Entities.TemplateCategory();

				if(row["RecID"].ToString()!="")
				{
					model.RecID=int.Parse(row["RecID"].ToString());
				}
				model.Name=row["Name"].ToString();
				if(row["Type"].ToString()!="")
				{
					model.Type=int.Parse(row["Type"].ToString());
				}
				if(row["Level"].ToString()!="")
				{
					model.Level=int.Parse(row["Level"].ToString());
				}
				if(row["Pid"].ToString()!="")
				{
					model.Pid=int.Parse(row["Pid"].ToString());
				}
				if(row["Status"].ToString()!="")
				{
					model.Status=int.Parse(row["Status"].ToString());
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
		public int Insert(Entities.TemplateCategory model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@Level", SqlDbType.Int,4),
					new SqlParameter("@Pid", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.Name;
			parameters[2].Value = model.Type;
			parameters[3].Value = model.Level;
			parameters[4].Value = model.Pid;
			parameters[5].Value = model.Status;
			parameters[6].Value = model.CreateTime;
			parameters[7].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TEMPLATECATEGORY_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(Entities.TemplateCategory model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@Level", SqlDbType.Int,4),
					new SqlParameter("@Pid", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.Name;
			parameters[2].Value = model.Type;
			parameters[3].Value = model.Level;
			parameters[4].Value = model.Pid;
			parameters[5].Value = model.Status;
			parameters[6].Value = model.CreateTime;
			parameters[7].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TEMPLATECATEGORY_UPDATE,parameters);
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

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TEMPLATECATEGORY_DELETE,parameters);
		}
		#endregion

	}
}

