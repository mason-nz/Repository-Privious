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
	/// 数据访问类GroupLabel。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-10-29 10:21:03 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class GroupLabel : DataBase
	{
		#region Instance
		public static readonly GroupLabel Instance = new GroupLabel();
		#endregion

		#region const
		private const string P_GROUPLABEL_SELECT = "p_GroupLabel_Select";
		private const string P_GROUPLABEL_INSERT = "p_GroupLabel_Insert";
		private const string P_GROUPLABEL_UPDATE = "p_GroupLabel_Update";
		private const string P_GROUPLABEL_DELETE = "p_GroupLabel_Delete";
		#endregion

		#region Contructor
		protected GroupLabel()
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
		public DataTable GetGroupLabel(QueryGroupLabel query, string order, int currentPage, int pageSize, out int totalCount)
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_GROUPLABEL_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.GroupLabel GetGroupLabel(int RecID)
		{
			QueryGroupLabel query = new QueryGroupLabel();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetGroupLabel(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleGroupLabel(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.GroupLabel LoadSingleGroupLabel(DataRow row)
		{
			Entities.GroupLabel model=new Entities.GroupLabel();

				if(row["RecID"].ToString()!="")
				{
					model.RecID=int.Parse(row["RecID"].ToString());
				}
				if(row["BGID"].ToString()!="")
				{
					model.BGID=int.Parse(row["BGID"].ToString());
				}
				if(row["LTID"].ToString()!="")
				{
					model.LTID=int.Parse(row["LTID"].ToString());
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
		public int Insert(Entities.GroupLabel model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@LTID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.BGID;
			parameters[2].Value = model.LTID;
			parameters[3].Value = model.CreateTime;
			parameters[4].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_GROUPLABEL_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		/// <summary>
		///  增加一条数据
		/// </summary>
		public int Insert(SqlTransaction sqltran, Entities.GroupLabel model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@LTID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.BGID;
			parameters[2].Value = model.LTID;
			parameters[3].Value = model.CreateTime;
			parameters[4].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_GROUPLABEL_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(Entities.GroupLabel model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@LTID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.BGID;
			parameters[2].Value = model.LTID;
			parameters[3].Value = model.CreateTime;
			parameters[4].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_GROUPLABEL_UPDATE,parameters);
		}
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.GroupLabel model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@LTID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.BGID;
			parameters[2].Value = model.LTID;
			parameters[3].Value = model.CreateTime;
			parameters[4].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_GROUPLABEL_UPDATE,parameters);
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

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_GROUPLABEL_DELETE,parameters);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, int RecID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
			parameters[0].Value = RecID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_GROUPLABEL_DELETE,parameters);
		}
		#endregion

        public DataTable GetLabelConfig(string where)
        {
            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000)
					};

            parameters[0].Value = where;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_LabelConfig_SelectList",parameters);
			return ds.Tables[0];
        }
        /// <summary>
        /// 批量插入业务组-标签中间表数据
        /// </summary>
        /// <param name="bgid"></param>
        /// <param name="ltids"></param>
        /// <param name="userid"></param>
        public void SaveDataBatch(int bgid,string ltids,int userid)
        {
            string sql = "DELETE FROM dbo.GroupLabel WHERE BGID="+ bgid;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);

            DataTable newdt = new DataTable();
            newdt.Columns.Add("BGID",typeof(int));
            newdt.Columns.Add("LTID", typeof(int));
            newdt.Columns.Add("CreateTime", typeof(DateTime));
            newdt.Columns.Add("CreateUserID", typeof(int));

            string[] arrayltid = ltids.Split(',');
            foreach (string ltid in arrayltid)
            {
                DataRow row = newdt.NewRow();
                row["BGID"] = bgid;
                row["LTID"] = ltid;
                row["CreateTime"] = DateTime.Now;
                row["CreateUserID"] = userid;

                newdt.Rows.Add(row);
            }

            IList<SqlBulkCopyColumnMapping> list = new List<SqlBulkCopyColumnMapping>();
            list.Add(new SqlBulkCopyColumnMapping("BGID", "BGID"));
            list.Add(new SqlBulkCopyColumnMapping("LTID", "LTID"));
            list.Add(new SqlBulkCopyColumnMapping("CreateTime", "CreateTime"));
            list.Add(new SqlBulkCopyColumnMapping("CreateUserID", "CreateUserID"));

            CommonDal.Instance.BulkCopyToDB(newdt, CONNECTIONSTRINGS, "dbo.GroupLabel", 2000, list);
        }
	}
}

