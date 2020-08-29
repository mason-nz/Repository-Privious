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
	/// 数据访问类KLQAnswer。
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
	public class KLQAnswer : DataBase
	{
		#region Instance
		public static readonly KLQAnswer Instance = new KLQAnswer();
		#endregion

		#region const
		private const string P_KLQANSWER_SELECT = "p_KLQAnswer_Select";
		private const string P_KLQANSWER_INSERT = "p_KLQAnswer_Insert";
		private const string P_KLQANSWER_UPDATE = "p_KLQAnswer_Update";
		private const string P_KLQANSWER_DELETE = "p_KLQAnswer_Delete";
		#endregion

		#region Contructor
		protected KLQAnswer()
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
		public DataTable GetKLQAnswer(QueryKLQAnswer query, string order, int currentPage, int pageSize, out int totalCount)
		{
			string where = string.Empty;
            if (query.KLAOID != Constant.INT_INVALID_VALUE)
            {
                where += " And KLAOID="+query.KLAOID;
            }
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLQANSWER_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.KLQAnswer GetKLQAnswer(long KLQID,int KLAOID)
		{
			QueryKLQAnswer query = new QueryKLQAnswer();
			query.KLQID = KLQID;
			query.KLAOID = KLAOID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetKLQAnswer(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleKLQAnswer(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}

        /// <summary>
        /// 获取试题下的答案
        /// </summary>
        /// <param name="KLQID"></param>
        /// <returns></returns>
        public DataTable GetKLQAnswerByKLQID(long KLQID)
        {
            string sqlStr = "SELECT * FROM KLQAnswer WHERE KLQID=@KLQID";
            SqlParameter parameter = new SqlParameter("@KLQID", KLQID);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);

            return ds.Tables[0];
        }

		private Entities.KLQAnswer LoadSingleKLQAnswer(DataRow row)
		{
			Entities.KLQAnswer model=new Entities.KLQAnswer();

				if(row["KLQID"].ToString()!="")
				{
					model.KLQID=long.Parse(row["KLQID"].ToString());
				}
				if(row["KLAOID"].ToString()!="")
				{
					model.KLAOID=int.Parse(row["KLAOID"].ToString());
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
		public void Insert(Entities.KLQAnswer model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@KLQID", SqlDbType.BigInt,8),
					new SqlParameter("@KLAOID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.KLQID;
			parameters[1].Value = model.KLAOID;
			parameters[2].Value = model.CreateTime;
			parameters[3].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLQANSWER_INSERT,parameters);
		}
		/// <summary>
		///  增加一条数据
		/// </summary>
		public void Insert(SqlTransaction sqltran, Entities.KLQAnswer model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@KLQID", SqlDbType.BigInt,8),
					new SqlParameter("@KLAOID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.KLQID;
			parameters[1].Value = model.KLAOID;
			parameters[2].Value = model.CreateTime;
			parameters[3].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_KLQANSWER_INSERT,parameters);
		}
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(Entities.KLQAnswer model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@KLQID", SqlDbType.BigInt,8),
					new SqlParameter("@KLAOID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.KLQID;
			parameters[1].Value = model.KLAOID;
			parameters[2].Value = model.CreateTime;
			parameters[3].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLQANSWER_UPDATE,parameters);
		}
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.KLQAnswer model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@KLQID", SqlDbType.BigInt,8),
					new SqlParameter("@KLAOID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.KLQID;
			parameters[1].Value = model.KLAOID;
			parameters[2].Value = model.CreateTime;
			parameters[3].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLQANSWER_UPDATE,parameters);
		}
		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(long KLQID,int KLAOID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@KLQID", SqlDbType.BigInt),
					new SqlParameter("@KLAOID", SqlDbType.Int,4)};
			parameters[0].Value = KLQID;
			parameters[1].Value = KLAOID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLQANSWER_DELETE,parameters);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, long KLQID,int KLAOID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@KLQID", SqlDbType.BigInt),
					new SqlParameter("@KLAOID", SqlDbType.Int,4)};
			parameters[0].Value = KLQID;
			parameters[1].Value = KLAOID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_KLQANSWER_DELETE,parameters);
		}

        /// <summary>
        /// 根据试题ID删除试题下的答案
        /// </summary>
        /// <param name="sqltran"></param>
        /// <param name="KLQAID"></param>
        /// <returns></returns>
        public int Delete(SqlTransaction sqltran, long KLQAID)
        {
            string sqlStr = "DELETE FROM KLQAnswer Where KLQID=@KLQID";
            SqlParameter parameter = new SqlParameter("@KLQID", KLQAID);

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.Text, sqlStr, parameter);
        }
		#endregion

	}
}

