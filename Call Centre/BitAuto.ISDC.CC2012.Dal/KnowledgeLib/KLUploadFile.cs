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
	/// 数据访问类KLUploadFile。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-08-21 10:19:09 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class KLUploadFile : DataBase
	{
		#region Instance
		public static readonly KLUploadFile Instance = new KLUploadFile();
		#endregion

		#region const
		private const string P_KLUPLOADFILE_SELECT = "p_KLUploadFile_Select";
		private const string P_KLUPLOADFILE_INSERT = "p_KLUploadFile_Insert";
		private const string P_KLUPLOADFILE_UPDATE = "p_KLUploadFile_Update";
		private const string P_KLUPLOADFILE_DELETE = "p_KLUploadFile_Delete";
		#endregion

		#region Contructor
		protected KLUploadFile()
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
		public DataTable GetKLUploadFile(QueryKLUploadFile query, string order, int currentPage, int pageSize, out int totalCount)
		{
			string where = string.Empty;

            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " AND RecID=" + query.RecID;
            }
            if (query.KLID != Constant.INT_INVALID_VALUE)
            {
                where += " AND KLID=" + query.KLID;
            }
            if (query.FilePath != Constant.STRING_INVALID_VALUE)
            {
                where += " AND FilePath='" + StringHelper.SqlFilter(query.FilePath.ToString())+"'";
            }
            if (query.Filename != Constant.STRING_INVALID_VALUE)
            {
                where += " AND Filename like '%" + StringHelper.SqlFilter(query.Filename.ToString())+"%'";
            }
            if (query.ExtendName != Constant.STRING_INVALID_VALUE)
            {
                where += " AND ExtendName='" + StringHelper.SqlFilter(query.ExtendName.ToString())+"'";
            }
            if (query.FileSize != Constant.INT_INVALID_VALUE)
            {
                where += " AND FileSize=" + query.FileSize;
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLUPLOADFILE_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.KLUploadFile GetKLUploadFile(long RecID)
		{
			QueryKLUploadFile query = new QueryKLUploadFile();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetKLUploadFile(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleKLUploadFile(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.KLUploadFile LoadSingleKLUploadFile(DataRow row)
		{
			Entities.KLUploadFile model=new Entities.KLUploadFile();

				if(row["RecID"].ToString()!="")
				{
					model.RecID=long.Parse(row["RecID"].ToString());
				}
				if(row["KLID"].ToString()!="")
				{
					model.KLID=long.Parse(row["KLID"].ToString());
				}
				model.FilePath=row["FilePath"].ToString();
				model.Filename=row["Filename"].ToString();
				model.ExtendName=row["ExtendName"].ToString();
				if(row["FileSize"].ToString()!="")
				{
					model.FileSize=int.Parse(row["FileSize"].ToString());
				}
				if(row["ClickCount"].ToString()!="")
				{
					model.ClickCount=int.Parse(row["ClickCount"].ToString());
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
		public int Insert(Entities.KLUploadFile model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@KLID", SqlDbType.BigInt,8),
					new SqlParameter("@FilePath", SqlDbType.VarChar,1000),
					new SqlParameter("@Filename", SqlDbType.VarChar,100),
					new SqlParameter("@ExtendName", SqlDbType.VarChar,50),
					new SqlParameter("@FileSize", SqlDbType.Int,4),
					new SqlParameter("@ClickCount", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.KLID;
			parameters[2].Value = model.FilePath;
			parameters[3].Value = model.Filename;
			parameters[4].Value = model.ExtendName;
			parameters[5].Value = model.FileSize;
			parameters[6].Value = model.ClickCount;
			parameters[7].Value = model.CreateTime;
			parameters[8].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLUPLOADFILE_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		/// <summary>
		///  增加一条数据
		/// </summary>
		public int Insert(SqlTransaction sqltran, Entities.KLUploadFile model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,8),
					new SqlParameter("@KLID", SqlDbType.Int,8),
					new SqlParameter("@FilePath", SqlDbType.VarChar,1000),
					new SqlParameter("@Filename", SqlDbType.VarChar,100),
					new SqlParameter("@ExtendName", SqlDbType.VarChar,50),
					new SqlParameter("@FileSize", SqlDbType.Int,4),
					new SqlParameter("@ClickCount", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.KLID;
			parameters[2].Value = model.FilePath;
			parameters[3].Value = model.Filename;
			parameters[4].Value = model.ExtendName;
			parameters[5].Value = model.FileSize;
			parameters[6].Value = model.ClickCount;
			parameters[7].Value = model.CreateTime;
			parameters[8].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(sqltran,CommandType.StoredProcedure, P_KLUPLOADFILE_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(Entities.KLUploadFile model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,8),
					new SqlParameter("@KLID", SqlDbType.Int,8),
					new SqlParameter("@FilePath", SqlDbType.VarChar,1000),
					new SqlParameter("@Filename", SqlDbType.VarChar,100),
					new SqlParameter("@ExtendName", SqlDbType.VarChar,50),
					new SqlParameter("@FileSize", SqlDbType.Int,4),
					new SqlParameter("@ClickCount", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.KLID;
			parameters[2].Value = model.FilePath;
			parameters[3].Value = model.Filename;
			parameters[4].Value = model.ExtendName;
			parameters[5].Value = model.FileSize;
			parameters[6].Value = model.ClickCount;
			parameters[7].Value = model.CreateTime;
			parameters[8].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLUPLOADFILE_UPDATE,parameters);
		}
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.KLUploadFile model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,8),
					new SqlParameter("@KLID", SqlDbType.Int,8),
					new SqlParameter("@FilePath", SqlDbType.VarChar,1000),
					new SqlParameter("@Filename", SqlDbType.VarChar,100),
					new SqlParameter("@ExtendName", SqlDbType.VarChar,50),
					new SqlParameter("@FileSize", SqlDbType.Int,4),
					new SqlParameter("@ClickCount", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.KLID;
			parameters[2].Value = model.FilePath;
			parameters[3].Value = model.Filename;
			parameters[4].Value = model.ExtendName;
			parameters[5].Value = model.FileSize;
			parameters[6].Value = model.ClickCount;
			parameters[7].Value = model.CreateTime;
			parameters[8].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(sqltran,  CommandType.StoredProcedure, P_KLUPLOADFILE_UPDATE,parameters);
		}
		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(long RecID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt)};
			parameters[0].Value = RecID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLUPLOADFILE_DELETE,parameters);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, long RecID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int)};
			parameters[0].Value = RecID;

			return SqlHelper.ExecuteNonQuery(sqltran,  CommandType.StoredProcedure, P_KLUPLOADFILE_DELETE,parameters);
		}
        /// <summary>
        /// 通过KLID删除一条数据
        /// </summary>
        public int DeleteByKLID(SqlTransaction sqltran, long KLID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@KLID", SqlDbType.Int)};
            parameters[0].Value = KLID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, "P_KLUPLOADFILE_DELETEByKLID", parameters);
        }
		#endregion

	}
}

