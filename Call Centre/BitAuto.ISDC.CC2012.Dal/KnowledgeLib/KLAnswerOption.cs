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
	/// 数据访问类KLAnswerOption。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-08-21 10:19:08 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class KLAnswerOption : DataBase
	{
		#region Instance
		public static readonly KLAnswerOption Instance = new KLAnswerOption();
		#endregion

		#region const
		private const string P_KLANSWEROPTION_SELECT = "p_KLAnswerOption_Select";
		private const string P_KLANSWEROPTION_INSERT = "p_KLAnswerOption_Insert";
		private const string P_KLANSWEROPTION_UPDATE = "p_KLAnswerOption_Update";
		private const string P_KLANSWEROPTION_DELETE = "p_KLAnswerOption_Delete";
		#endregion

		#region Contructor
		protected KLAnswerOption()
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
		public DataTable GetKLAnswerOption(QueryKLAnswerOption query, string order, int currentPage, int pageSize, out int totalCount)
		{
			string where = string.Empty;
            if (query.KLQID != Constant.INT_INVALID_VALUE)
            {
                where += " And KLQID="+query.KLQID;
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLANSWEROPTION_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}
        /// <summary>
        /// 获取试题下的选项
        /// </summary>
        /// <param name="KLQID"></param>
        /// <returns></returns>
        public DataTable GetKLAnswerOptionByKLQID(long KLQID)
        {
            string sqlStr = "SELECT * FROM KLAnswerOption WHERE KLQID=@KLQID";
            SqlParameter parameter = new SqlParameter("@KLQID", KLQID);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);
            return ds.Tables[0];
        }

        /// <summary>
        /// 获取试题下的选项
        /// </summary>
        /// <param name="KLQID"></param>
        /// <returns></returns>
        public DataTable GetKLAnswerOptionByKLQID(long KLQID,string BQID)
        {
            string sqlStr = "SELECT *,'"+ BQID +"' as 'BQID' FROM KLAnswerOption WHERE KLQID=@KLQID";
            SqlParameter parameter = new SqlParameter("@KLQID", KLQID);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);
            return ds.Tables[0];
        }

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
        public Entities.KLAnswerOption GetKLAnswerOption(long KLAOID)
        {
            string sqlStr = "SELECT * FROM KLAnswerOption WHERE KLAOID=@KLAOID";
            SqlParameter parameter = new SqlParameter("@KLAOID", KLAOID);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return LoadSingleKLAnswerOption(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        
		private Entities.KLAnswerOption LoadSingleKLAnswerOption(DataRow row)
		{
			Entities.KLAnswerOption model=new Entities.KLAnswerOption();

				if(row["KLAOID"].ToString()!="")
				{
					model.KLAOID=long.Parse(row["KLAOID"].ToString());
				}
				if(row["KLQID"].ToString()!="")
				{
					model.KLQID=long.Parse(row["KLQID"].ToString());
				}
				model.Answer=row["Answer"].ToString();
				if(row["CreateTime"].ToString()!="")
				{
					model.CreateTime=DateTime.Parse(row["CreateTime"].ToString());
				}
				if(row["CreateUserID"].ToString()!="")
				{
					model.CreateUserID=int.Parse(row["CreateUserID"].ToString());
				}
				if(row["ModifyTime"].ToString()!="")
				{
					model.ModifyTime=DateTime.Parse(row["ModifyTime"].ToString());
				}
				if(row["ModifyUserID"].ToString()!="")
				{
					model.ModifyUserID=int.Parse(row["ModifyUserID"].ToString());
				}
				return model;
		}
		#endregion

		#region Insert
		/// <summary>
		///  增加一条数据
		/// </summary>
		public int Insert(Entities.KLAnswerOption model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@KLAOID", SqlDbType.BigInt,8),
					new SqlParameter("@KLQID", SqlDbType.BigInt,8),
					new SqlParameter("@Answer", SqlDbType.NVarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.KLQID;
			parameters[2].Value = model.Answer;
			parameters[3].Value = model.CreateTime;
			parameters[4].Value = model.CreateUserID;
			parameters[5].Value = model.ModifyTime;
			parameters[6].Value = model.ModifyUserID;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLANSWEROPTION_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		/// <summary>
		///  增加一条数据
		/// </summary>
		public int Insert(SqlTransaction sqltran, Entities.KLAnswerOption model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@KLAOID", SqlDbType.Int),
					new SqlParameter("@KLQID", SqlDbType.BigInt,8),
					new SqlParameter("@Answer", SqlDbType.NVarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.KLQID;
			parameters[2].Value = model.Answer;
			parameters[3].Value = model.CreateTime;
			parameters[4].Value = model.CreateUserID;
			parameters[5].Value = model.ModifyTime;
			parameters[6].Value = model.ModifyUserID;

			SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_KLANSWEROPTION_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(Entities.KLAnswerOption model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@KLAOID", SqlDbType.BigInt,8),
					new SqlParameter("@KLQID", SqlDbType.BigInt,8),
					new SqlParameter("@Answer", SqlDbType.NVarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.KLAOID;
			parameters[1].Value = model.KLQID;
			parameters[2].Value = model.Answer;
			parameters[3].Value = model.CreateTime;
			parameters[4].Value = model.CreateUserID;
			parameters[5].Value = model.ModifyTime;
			parameters[6].Value = model.ModifyUserID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLANSWEROPTION_UPDATE,parameters);
		}
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.KLAnswerOption model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@KLAOID", SqlDbType.BigInt,8),
					new SqlParameter("@KLQID", SqlDbType.BigInt,8),
					new SqlParameter("@Answer", SqlDbType.NVarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.KLAOID;
			parameters[1].Value = model.KLQID;
			parameters[2].Value = model.Answer;
			parameters[3].Value = model.CreateTime;
			parameters[4].Value = model.CreateUserID;
			parameters[5].Value = model.ModifyTime;
			parameters[6].Value = model.ModifyUserID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_KLANSWEROPTION_UPDATE,parameters);
		}
		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(long KLAOID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@KLAOID", SqlDbType.BigInt)};
			parameters[0].Value = KLAOID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLANSWEROPTION_DELETE,parameters);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, long KLAOID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@KLAOID", SqlDbType.BigInt)};
			parameters[0].Value = KLAOID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_KLANSWEROPTION_DELETE,parameters);
		}
		#endregion

	}
}

