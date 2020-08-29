using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_DMS2014.Entities;
using BitAuto.DSC.IM_DMS2014.Entities.Constants;

namespace BitAuto.DSC.IM_DMS2014.Dal
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 数据访问类Emotions。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-10-29 10:21:02 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class Emotions : DataBase
	{
		#region Instance
		public static readonly Emotions Instance = new Emotions();
		#endregion

		#region const
		private const string P_EMOTIONS_SELECT = "p_Emotions_Select";
		private const string P_EMOTIONS_INSERT = "p_Emotions_Insert";
		private const string P_EMOTIONS_UPDATE = "p_Emotions_Update";
		private const string P_EMOTIONS_DELETE = "p_Emotions_Delete";
		#endregion

		#region Contructor
		protected Emotions()
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
		public DataTable GetEmotions(QueryEmotions query, string order, int currentPage, int pageSize, out int totalCount)
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EMOTIONS_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.Emotions GetEmotions(int RecID)
		{
			QueryEmotions query = new QueryEmotions();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetEmotions(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleEmotions(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.Emotions LoadSingleEmotions(DataRow row)
		{
			Entities.Emotions model=new Entities.Emotions();

				if(row["RecID"].ToString()!="")
				{
					model.RecID=int.Parse(row["RecID"].ToString());
				}
				model.EText=row["EText"].ToString();
				model.ECategory=row["ECategory"].ToString();
				model.EUrl=row["EUrl"].ToString();
				if(row["Status"].ToString()!="")
				{
					model.Status=int.Parse(row["Status"].ToString());
				}
				if(row["CreateTime"].ToString()!="")
				{
					model.CreateTime=DateTime.Parse(row["CreateTime"].ToString());
				}
				return model;
		}
		#endregion

		#region Insert
		/// <summary>
		///  增加一条数据
		/// </summary>
		public int Insert(Entities.Emotions model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@EText", SqlDbType.NVarChar,10),
					new SqlParameter("@ECategory", SqlDbType.VarChar,10),
					new SqlParameter("@EUrl", SqlDbType.VarChar,50),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.EText;
			parameters[2].Value = model.ECategory;
			parameters[3].Value = model.EUrl;
			parameters[4].Value = model.Status;
			parameters[5].Value = model.CreateTime;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EMOTIONS_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		/// <summary>
		///  增加一条数据
		/// </summary>
		public int Insert(SqlTransaction sqltran, Entities.Emotions model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@EText", SqlDbType.NVarChar,10),
					new SqlParameter("@ECategory", SqlDbType.VarChar,10),
					new SqlParameter("@EUrl", SqlDbType.VarChar,50),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.EText;
			parameters[2].Value = model.ECategory;
			parameters[3].Value = model.EUrl;
			parameters[4].Value = model.Status;
			parameters[5].Value = model.CreateTime;

			SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_EMOTIONS_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(Entities.Emotions model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@EText", SqlDbType.NVarChar,10),
					new SqlParameter("@ECategory", SqlDbType.VarChar,10),
					new SqlParameter("@EUrl", SqlDbType.VarChar,50),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.EText;
			parameters[2].Value = model.ECategory;
			parameters[3].Value = model.EUrl;
			parameters[4].Value = model.Status;
			parameters[5].Value = model.CreateTime;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EMOTIONS_UPDATE,parameters);
		}
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.Emotions model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@EText", SqlDbType.NVarChar,10),
					new SqlParameter("@ECategory", SqlDbType.VarChar,10),
					new SqlParameter("@EUrl", SqlDbType.VarChar,50),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.EText;
			parameters[2].Value = model.ECategory;
			parameters[3].Value = model.EUrl;
			parameters[4].Value = model.Status;
			parameters[5].Value = model.CreateTime;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_EMOTIONS_UPDATE,parameters);
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

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EMOTIONS_DELETE,parameters);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, int RecID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
			parameters[0].Value = RecID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_EMOTIONS_DELETE,parameters);
		}
		#endregion


        public DataTable GetEmotionInfoByECategory(string strWhere)
        {
            SqlParameter[] parameters = {
					    new SqlParameter("@where", SqlDbType.VarChar, 50)
					};

            parameters[0].Value = strWhere;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Emotions_Select", parameters);
            return ds.Tables[0];
        }

	}
}

