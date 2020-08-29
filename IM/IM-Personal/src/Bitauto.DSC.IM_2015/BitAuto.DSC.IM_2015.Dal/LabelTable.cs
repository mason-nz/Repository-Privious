using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.Dal
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 数据访问类LabelTable。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-10-29 10:21:04 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class LabelTable : DataBase
	{
		#region Instance
		public static readonly LabelTable Instance = new LabelTable();
		#endregion

		#region const
		private const string P_LABELTABLE_SELECT = "p_LabelTable_Select";
		private const string P_LABELTABLE_INSERT = "p_LabelTable_Insert";
		private const string P_LABELTABLE_UPDATE = "p_LabelTable_Update";
		private const string P_LABELTABLE_DELETE = "p_LabelTable_Delete";
		#endregion

		#region Contructor
		protected LabelTable()
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
		public DataTable GetLabelTable(QueryLabelTable query, string order, int currentPage, int pageSize, out int totalCount)
		{
			string where = string.Empty;

            if (query.LTID != Constant.INT_INVALID_VALUE)
            {
                where += " and LTID="+ query.LTID;
            }

            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " and Status="+ query.Status;
            }
            if (query.AreaType != Constant.INT_INVALID_VALUE)
            {
                where += " and AreaType=" + query.AreaType;
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_LABELTABLE_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}
        /// <summary>
        /// 获取全部标签 及 标识是否所属当前组
        /// </summary>
        /// <param name="bgid"></param>
        /// <returns></returns>
        public DataTable GetLabelTableByBGID(int bgid, int region)
        {
            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@BGID", SqlDbType.Int, 4),
                    new SqlParameter("@Region", SqlDbType.Int,4)
					};

            parameters[0].Value = bgid;
            parameters[1].Value = region;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_LabelConfig_PopList", parameters);
            return ds.Tables[0];
        }
		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.LabelTable GetLabelTable(int LTID)
		{
			QueryLabelTable query = new QueryLabelTable();
			query.LTID = LTID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetLabelTable(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleLabelTable(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.LabelTable LoadSingleLabelTable(DataRow row)
		{
			Entities.LabelTable model=new Entities.LabelTable();

				if(row["LTID"].ToString()!="")
				{
					model.LTID=int.Parse(row["LTID"].ToString());
				}
				model.Name=row["Name"].ToString();
				if(row["Status"].ToString()!="")
				{
					model.Status=int.Parse(row["Status"].ToString());
				}
				if(row["SortNum"].ToString()!="")
				{
					model.SortNum=int.Parse(row["SortNum"].ToString());
				}
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
		public int Insert(Entities.LabelTable model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@LTID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.VarChar,100),
                    new SqlParameter("@AreaType", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@SortNum", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.Name;
            parameters[2].Value = model.AreaType;
			parameters[3].Value = model.Status;
			parameters[4].Value = model.SortNum;
			parameters[5].Value = model.CreateTime;
			parameters[6].Value = model.CreateUserID;
			parameters[7].Value = model.ModifyTime;
			parameters[8].Value = model.ModifyUserID;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_LABELTABLE_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		/// <summary>
		///  增加一条数据
		/// </summary>
		public int Insert(SqlTransaction sqltran, Entities.LabelTable model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@LTID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.VarChar,100),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@SortNum", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.Name;
			parameters[2].Value = model.Status;
			parameters[3].Value = model.SortNum;
			parameters[4].Value = model.CreateTime;
			parameters[5].Value = model.CreateUserID;
			parameters[6].Value = model.ModifyTime;
			parameters[7].Value = model.ModifyUserID;

			SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_LABELTABLE_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(Entities.LabelTable model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@LTID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.VarChar,100),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@SortNum", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.LTID;
			parameters[1].Value = model.Name;
			parameters[2].Value = model.Status;
			parameters[3].Value = model.SortNum;
			parameters[4].Value = model.CreateTime;
			parameters[5].Value = model.CreateUserID;
			parameters[6].Value = model.ModifyTime;
			parameters[7].Value = model.ModifyUserID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_LABELTABLE_UPDATE,parameters);
		}
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.LabelTable model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@LTID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.VarChar,100),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@SortNum", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.LTID;
			parameters[1].Value = model.Name;
			parameters[2].Value = model.Status;
			parameters[3].Value = model.SortNum;
			parameters[4].Value = model.CreateTime;
			parameters[5].Value = model.CreateUserID;
			parameters[6].Value = model.ModifyTime;
			parameters[7].Value = model.ModifyUserID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_LABELTABLE_UPDATE,parameters);
		}
		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(int LTID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@LTID", SqlDbType.Int,4)};
			parameters[0].Value = LTID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_LABELTABLE_DELETE,parameters);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, int LTID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@LTID", SqlDbType.Int,4)};
			parameters[0].Value = LTID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_LABELTABLE_DELETE,parameters);
		}
		#endregion

        /// 上下移动数据
        /// <summary>
        /// 上下移动数据
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="sortnum"></param>
        /// <param name="type">1上-1下</param>
        /// <returns></returns>
        public bool MoveUpOrDown(int recid, int sortnum, int type)
        {
            int next_recid = 0, next_sortnum = 0;
            string sql = "";
            //上移
            if (type > 0)
            {
                //获取前一位数据
                sql = "select top 1 ltid,sortnum from dbo.LabelTable where status=0 and sortnum<" + sortnum + " order by sortnum desc";
            }
            //下移
            else if (type < 0)
            {
                //获取后一位数据
                sql = "select top 1 ltid,sortnum from dbo.LabelTable where status=0 and sortnum>" + sortnum + " order by sortnum asc";
            }
            else return false;
            //查询数据
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            if (dt == null || dt.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                next_recid = CommonFunc.ObjectToInteger(dt.Rows[0]["ltid"]);
                next_sortnum = CommonFunc.ObjectToInteger(dt.Rows[0]["sortnum"]);
            }
            //交换数据
            string sql1 = "update dbo.LabelTable set sortnum=" + next_sortnum + " where ltid=" + recid;
            string sql2 = "update dbo.LabelTable set sortnum=" + sortnum + " where ltid=" + next_recid;
            int i = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql1);
            i += SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql2);
            return i == 2;
        }
	}
}

