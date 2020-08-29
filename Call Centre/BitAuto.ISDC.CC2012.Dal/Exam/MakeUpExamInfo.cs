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
	/// 数据访问类MakeUpExamInfo。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-09-03 02:04:21 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class MakeUpExamInfo : DataBase
	{
		#region Instance
		public static readonly MakeUpExamInfo Instance = new MakeUpExamInfo();
		#endregion

		#region const
		private const string P_MAKEUPEXAMINFO_SELECT = "p_MakeUpExamInfo_Select";
		private const string P_MAKEUPEXAMINFO_INSERT = "p_MakeUpExamInfo_Insert";
		private const string P_MAKEUPEXAMINFO_UPDATE = "p_MakeUpExamInfo_Update";
		private const string P_MAKEUPEXAMINFO_DELETE = "p_MakeUpExamInfo_Delete";
		#endregion

		#region Contructor
		protected MakeUpExamInfo()
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
		public DataTable GetMakeUpExamInfo(QueryMakeUpExamInfo query, string order, int currentPage, int pageSize, out int totalCount)
		{
			string where = string.Empty;
            if (query.EIID != Constant.INT_INVALID_VALUE)
            {
                where += " AND EIID=" + query.EIID;
            }

            if (query.MEIID != Constant.INT_INVALID_VALUE)
            {
                where += " AND MEIID=" + query.MEIID;
            }
            if (query.MakeUpEPID != Constant.INT_INVALID_VALUE)
            {
                where += " AND MakeUpEPID=" + query.MakeUpEPID;
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_MAKEUPEXAMINFO_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.MakeUpExamInfo GetMakeUpExamInfo(int MEIID)
		{
			QueryMakeUpExamInfo query = new QueryMakeUpExamInfo();
			query.MEIID = MEIID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetMakeUpExamInfo(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleMakeUpExamInfo(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}

        public Entities.MakeUpExamInfo GetMakeUpExamInfoByEIID(int EIID)
        {
            QueryMakeUpExamInfo query = new QueryMakeUpExamInfo();
            query.EIID = EIID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetMakeUpExamInfo(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleMakeUpExamInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }

		private Entities.MakeUpExamInfo LoadSingleMakeUpExamInfo(DataRow row)
		{
			Entities.MakeUpExamInfo model=new Entities.MakeUpExamInfo();

				if(row["MEIID"].ToString()!="")
				{
					model.MEIID=int.Parse(row["MEIID"].ToString());
				}
				if(row["EIID"].ToString()!="")
				{
					model.EIID=long.Parse(row["EIID"].ToString());
				}
				if(row["MakeUpEPID"].ToString()!="")
				{
					model.MakeUpEPID=int.Parse(row["MakeUpEPID"].ToString());
				}
				if(row["MakeUpExamStartTime"].ToString()!="")
				{
					model.MakeUpExamStartTime=DateTime.Parse(row["MakeUpExamStartTime"].ToString());
				}
				if(row["MakeupExamEndTime"].ToString()!="")
				{
					model.MakeupExamEndTime=DateTime.Parse(row["MakeupExamEndTime"].ToString());
				}
				if(row["JoinNum"].ToString()!="")
				{
					model.JoinNum=int.Parse(row["JoinNum"].ToString());
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
		public int Insert(Entities.MakeUpExamInfo model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@MEIID", SqlDbType.Int,4),
					new SqlParameter("@EIID", SqlDbType.BigInt,8),
					new SqlParameter("@MakeUpEPID", SqlDbType.Int,4),
					new SqlParameter("@MakeUpExamStartTime", SqlDbType.DateTime),
					new SqlParameter("@MakeupExamEndTime", SqlDbType.DateTime),
					new SqlParameter("@JoinNum", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.EIID;
			parameters[2].Value = model.MakeUpEPID;
			parameters[3].Value = model.MakeUpExamStartTime;
			parameters[4].Value = model.MakeupExamEndTime;
			parameters[5].Value = model.JoinNum;
			parameters[6].Value = model.CreateTime;
			parameters[7].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_MAKEUPEXAMINFO_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		/// <summary>
		///  增加一条数据
		/// </summary>
		public int Insert(SqlTransaction sqltran, Entities.MakeUpExamInfo model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@MEIID", SqlDbType.Int,4),
					new SqlParameter("@EIID", SqlDbType.BigInt,8),
					new SqlParameter("@MakeUpEPID", SqlDbType.Int,4),
					new SqlParameter("@MakeUpExamStartTime", SqlDbType.DateTime),
					new SqlParameter("@MakeupExamEndTime", SqlDbType.DateTime),
					new SqlParameter("@JoinNum", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.EIID;
			parameters[2].Value = model.MakeUpEPID;
			parameters[3].Value = model.MakeUpExamStartTime;
			parameters[4].Value = model.MakeupExamEndTime;
			parameters[5].Value = model.JoinNum;
			parameters[6].Value = model.CreateTime;
			parameters[7].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_MAKEUPEXAMINFO_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(Entities.MakeUpExamInfo model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@MEIID", SqlDbType.Int,4),
					new SqlParameter("@EIID", SqlDbType.BigInt,8),
					new SqlParameter("@MakeUpEPID", SqlDbType.Int,4),
					new SqlParameter("@MakeUpExamStartTime", SqlDbType.DateTime),
					new SqlParameter("@MakeupExamEndTime", SqlDbType.DateTime),
					new SqlParameter("@JoinNum", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.MEIID;
			parameters[1].Value = model.EIID;
			parameters[2].Value = model.MakeUpEPID;
			parameters[3].Value = model.MakeUpExamStartTime;
			parameters[4].Value = model.MakeupExamEndTime;
			parameters[5].Value = model.JoinNum;
			parameters[6].Value = model.CreateTime;
			parameters[7].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_MAKEUPEXAMINFO_UPDATE,parameters);
		}
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.MakeUpExamInfo model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@MEIID", SqlDbType.Int,4),
					new SqlParameter("@EIID", SqlDbType.BigInt,8),
					new SqlParameter("@MakeUpEPID", SqlDbType.Int,4),
					new SqlParameter("@MakeUpExamStartTime", SqlDbType.DateTime),
					new SqlParameter("@MakeupExamEndTime", SqlDbType.DateTime),
					new SqlParameter("@JoinNum", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.MEIID;
			parameters[1].Value = model.EIID;
			parameters[2].Value = model.MakeUpEPID;
			parameters[3].Value = model.MakeUpExamStartTime;
			parameters[4].Value = model.MakeupExamEndTime;
			parameters[5].Value = model.JoinNum;
			parameters[6].Value = model.CreateTime;
			parameters[7].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_MAKEUPEXAMINFO_UPDATE,parameters);
		}
		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(int EIID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@EIID", SqlDbType.Int,4)};
			parameters[0].Value = EIID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_MAKEUPEXAMINFO_DELETE,parameters);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, int EIID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@EIID", SqlDbType.Int,4)};
			parameters[0].Value = EIID;

			return SqlHelper.ExecuteNonQuery(sqltran,CommandType.StoredProcedure, P_MAKEUPEXAMINFO_DELETE,parameters);
		}
		#endregion

	}
}

