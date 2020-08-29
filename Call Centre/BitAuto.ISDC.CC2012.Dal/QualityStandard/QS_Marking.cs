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
	/// 数据访问类QS_Marking。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-04-25 09:42:36 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class QS_Marking : DataBase
	{
		#region Instance
		public static readonly QS_Marking Instance = new QS_Marking();
		#endregion

		#region const
		private const string P_QS_MARKING_SELECT = "p_QS_Marking_Select";
		private const string P_QS_MARKING_INSERT = "p_QS_Marking_Insert";
		private const string P_QS_MARKING_UPDATE = "p_QS_Marking_Update";
		private const string P_QS_MARKING_DELETE = "p_QS_Marking_Delete";
		#endregion

		#region Contructor
		protected QS_Marking()
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
		public DataTable GetQS_Marking(QueryQS_Marking query, string order, int currentPage, int pageSize, out int totalCount)
		{
			string where = string.Empty;

            if (query.QS_SID != Constant.INT_INVALID_VALUE)
            {
                where += " And QS_SID=" + query.QS_SID;
            }
            if (query.QS_IID != Constant.INT_INVALID_VALUE)
            {
                where += " And QS_IID=" + query.QS_IID;
            }
            if (query.QS_CID != Constant.INT_INVALID_VALUE)
            {
                where += " And QS_CID=" + query.QS_CID;
            }
            if (query.QS_RTID != Constant.INT_INVALID_VALUE)
            {
                where += " And QS_RTID=" + query.QS_RTID;
            }
            if (query.MarkingItemName != Constant.STRING_INVALID_VALUE)
            {
                where += " And MarkingItemName  like '%" + StringHelper.SqlFilter(query.MarkingItemName) + "%'";
            }
            
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " And Status=" + query.Status;
            }
            if (query.CreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " And CreateUserID=" + query.CreateUserID;
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_MARKING_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.QS_Marking GetQS_Marking(int QS_MID)
		{
			QueryQS_Marking query = new QueryQS_Marking();
			query.QS_MID = QS_MID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetQS_Marking(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleQS_Marking(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.QS_Marking LoadSingleQS_Marking(DataRow row)
		{
			Entities.QS_Marking model=new Entities.QS_Marking();

				if(row["QS_RTID"].ToString()!="")
				{
					model.QS_RTID=int.Parse(row["QS_RTID"].ToString());
				}
				if(row["QS_CID"].ToString()!="")
				{
					model.QS_CID=int.Parse(row["QS_CID"].ToString());
				}
				if(row["QS_MID"].ToString()!="")
				{
					model.QS_MID=int.Parse(row["QS_MID"].ToString());
				}
				if(row["QS_IID"].ToString()!="")
				{
					model.QS_IID=int.Parse(row["QS_IID"].ToString());
				}
				if(row["QS_SID"].ToString()!="")
				{
					model.QS_SID=int.Parse(row["QS_SID"].ToString());
				}
				if(row["ScoreType"].ToString()!="")
				{
					model.ScoreType=int.Parse(row["ScoreType"].ToString());
				}
				model.MarkingItemName=row["MarkingItemName"].ToString();
				if(row["Score"].ToString()!="")
				{
					model.Score=int.Parse(row["Score"].ToString());
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
				if(row["Sort"].ToString()!="")
				{
					model.Sort=int.Parse(row["Sort"].ToString());
				}
				return model;
		}
		#endregion

		#region Insert
		/// <summary>
		///  增加一条数据
		/// </summary>
		public int Insert(Entities.QS_Marking model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@QS_CID", SqlDbType.Int,4),
					new SqlParameter("@QS_MID", SqlDbType.Int,4),
					new SqlParameter("@QS_IID", SqlDbType.Int,4),
					new SqlParameter("@QS_SID", SqlDbType.Int,4),
					new SqlParameter("@ScoreType", SqlDbType.Int,4),
					new SqlParameter("@MarkingItemName", SqlDbType.NVarChar,500),
					new SqlParameter("@Score", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@Sort", SqlDbType.Int,4)};
			parameters[0].Value = model.QS_RTID;
			parameters[1].Value = model.QS_CID;
			parameters[2].Direction = ParameterDirection.Output;
			parameters[3].Value = model.QS_IID;
			parameters[4].Value = model.QS_SID;
			parameters[5].Value = model.ScoreType;
			parameters[6].Value = model.MarkingItemName;
			parameters[7].Value = model.Score;
			parameters[8].Value = model.Status;
			parameters[9].Value = model.CreateTime;
			parameters[10].Value = model.CreateUserID;
			parameters[11].Value = model.Sort;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_MARKING_INSERT,parameters);
			return (int)parameters[2].Value;
		}
		/// <summary>
		///  增加一条数据
		/// </summary>
		public int Insert(SqlTransaction sqltran, Entities.QS_Marking model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@QS_CID", SqlDbType.Int,4),
					new SqlParameter("@QS_MID", SqlDbType.Int,4),
					new SqlParameter("@QS_IID", SqlDbType.Int,4),
					new SqlParameter("@QS_SID", SqlDbType.Int,4),
					new SqlParameter("@ScoreType", SqlDbType.Int,4),
					new SqlParameter("@MarkingItemName", SqlDbType.NVarChar,500),
					new SqlParameter("@Score", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@Sort", SqlDbType.Int,4)};
			parameters[0].Value = model.QS_RTID;
			parameters[1].Value = model.QS_CID;
			parameters[2].Direction = ParameterDirection.Output;
			parameters[3].Value = model.QS_IID;
			parameters[4].Value = model.QS_SID;
			parameters[5].Value = model.ScoreType;
			parameters[6].Value = model.MarkingItemName;
			parameters[7].Value = model.Score;
			parameters[8].Value = model.Status;
			parameters[9].Value = model.CreateTime;
			parameters[10].Value = model.CreateUserID;
			parameters[11].Value = model.Sort;

			SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_QS_MARKING_INSERT,parameters);
			return (int)parameters[2].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(Entities.QS_Marking model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@QS_CID", SqlDbType.Int,4),
					new SqlParameter("@QS_MID", SqlDbType.Int,4),
					new SqlParameter("@QS_IID", SqlDbType.Int,4),
					new SqlParameter("@QS_SID", SqlDbType.Int,4),
					new SqlParameter("@ScoreType", SqlDbType.Int,4),
					new SqlParameter("@MarkingItemName", SqlDbType.NVarChar,500),
					new SqlParameter("@Score", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					 
					new SqlParameter("@Sort", SqlDbType.Int,4)};
			parameters[0].Value = model.QS_RTID;
			parameters[1].Value = model.QS_CID;
			parameters[2].Value = model.QS_MID;
			parameters[3].Value = model.QS_IID;
			parameters[4].Value = model.QS_SID;
			parameters[5].Value = model.ScoreType;
			parameters[6].Value = model.MarkingItemName;
			parameters[7].Value = model.Score;
			parameters[8].Value = model.Status;
			 
			parameters[9].Value = model.Sort;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_MARKING_UPDATE,parameters);
		}
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.QS_Marking model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@QS_CID", SqlDbType.Int,4),
					new SqlParameter("@QS_MID", SqlDbType.Int,4),
					new SqlParameter("@QS_IID", SqlDbType.Int,4),
					new SqlParameter("@QS_SID", SqlDbType.Int,4),
					new SqlParameter("@ScoreType", SqlDbType.Int,4),
					new SqlParameter("@MarkingItemName", SqlDbType.NVarChar,500),
					new SqlParameter("@Score", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
				 
					new SqlParameter("@Sort", SqlDbType.Int,4)};
			parameters[0].Value = model.QS_RTID;
			parameters[1].Value = model.QS_CID;
			parameters[2].Value = model.QS_MID;
			parameters[3].Value = model.QS_IID;
			parameters[4].Value = model.QS_SID;
			parameters[5].Value = model.ScoreType;
			parameters[6].Value = model.MarkingItemName;
			parameters[7].Value = model.Score;
			parameters[8].Value = model.Status;
		 
			parameters[9].Value = model.Sort;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_QS_MARKING_UPDATE,parameters);
		}
		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(int QS_MID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@QS_MID", SqlDbType.Int,4)};
			parameters[0].Value = QS_MID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_MARKING_DELETE,parameters);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, int QS_MID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@QS_MID", SqlDbType.Int,4)};
			parameters[0].Value = QS_MID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_QS_MARKING_DELETE,parameters);
		}
		#endregion

	}
}

