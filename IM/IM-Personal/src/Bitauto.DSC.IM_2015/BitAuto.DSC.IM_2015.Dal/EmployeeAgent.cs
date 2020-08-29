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
	/// 数据访问类EmployeeAgent。
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
	public class EmployeeAgent : DataBase
	{
		#region Instance
		public static readonly EmployeeAgent Instance = new EmployeeAgent();
		#endregion

		#region const
		private const string P_EMPLOYEEAGENT_SELECT = "p_EmployeeAgent_Select";
		private const string P_EMPLOYEEAGENT_INSERT = "p_EmployeeAgent_Insert";
		private const string P_EMPLOYEEAGENT_UPDATE = "p_EmployeeAgent_Update";
		private const string P_EMPLOYEEAGENT_DELETE = "p_EmployeeAgent_Delete";
		#endregion

		#region Contructor
		protected EmployeeAgent()
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
		public DataTable GetEmployeeAgent(QueryEmployeeAgent query, string order, int currentPage, int pageSize, out int totalCount)
		{
			string where = string.Empty;
            if(query.UserID!=Constant.INT_INVALID_VALUE)
		    {
		        where += " and userID="+query.UserID;
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EMPLOYEEAGENT_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

	    public string GetAgentNum(string strUserId)
	    {
	        var strCmd = "SELECT AgentNum FROM dbo.v_AgentInfo WHERE UserID=" + strUserId;
	        var reader = SqlHelper.ExecuteReader(CONNECTIONSTRINGS, CommandType.Text, strCmd);
	        while (reader.Read())
	        {
	            return reader[0].ToString();
	            break;
	        }
	        return string.Empty;
	    }

	    #endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.EmployeeAgent GetEmployeeAgent(int RecID)
		{
			QueryEmployeeAgent query = new QueryEmployeeAgent();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetEmployeeAgent(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleEmployeeAgent(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}

	    

	    private Entities.EmployeeAgent LoadSingleEmployeeAgent(DataRow row)
		{
			Entities.EmployeeAgent model=new Entities.EmployeeAgent();

				if(row["RecID"].ToString()!="")
				{
					model.RecID=int.Parse(row["RecID"].ToString());
				}
				if(row["UserID"].ToString()!="")
				{
					model.UserID=int.Parse(row["UserID"].ToString());
				}
				if(row["MaxDialogueN"].ToString()!="")
				{
					model.MaxDialogueN=int.Parse(row["MaxDialogueN"].ToString());
				}
				if(row["MaxQueueN"].ToString()!="")
				{
					model.MaxQueueN=int.Parse(row["MaxQueueN"].ToString());
				}
				if(row["LastLoginTime"].ToString()!="")
				{
					model.LastLoginTime=DateTime.Parse(row["LastLoginTime"].ToString());
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
		public int Insert(Entities.EmployeeAgent model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@MaxDialogueN", SqlDbType.Int,4),
					new SqlParameter("@MaxQueueN", SqlDbType.Int,4),
					new SqlParameter("@LastLoginTime", SqlDbType.DateTime),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.UserID;
			parameters[2].Value = model.MaxDialogueN;
			parameters[3].Value = model.MaxQueueN;
			parameters[4].Value = model.LastLoginTime;
			parameters[5].Value = model.CreateTime;
			parameters[6].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EMPLOYEEAGENT_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		/// <summary>
		///  增加一条数据
		/// </summary>
		public int Insert(SqlTransaction sqltran, Entities.EmployeeAgent model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@MaxDialogueN", SqlDbType.Int,4),
					new SqlParameter("@MaxQueueN", SqlDbType.Int,4),
					new SqlParameter("@LastLoginTime", SqlDbType.DateTime),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.UserID;
			parameters[2].Value = model.MaxDialogueN;
			parameters[3].Value = model.MaxQueueN;
			parameters[4].Value = model.LastLoginTime;
			parameters[5].Value = model.CreateTime;
			parameters[6].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_EMPLOYEEAGENT_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(Entities.EmployeeAgent model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@MaxDialogueN", SqlDbType.Int,4),
					new SqlParameter("@MaxQueueN", SqlDbType.Int,4),
					new SqlParameter("@LastLoginTime", SqlDbType.DateTime),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.UserID;
			parameters[2].Value = model.MaxDialogueN;
			parameters[3].Value = model.MaxQueueN;
			parameters[4].Value = model.LastLoginTime;
			parameters[5].Value = model.CreateTime;
			parameters[6].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EMPLOYEEAGENT_UPDATE,parameters);
		}
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.EmployeeAgent model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@MaxDialogueN", SqlDbType.Int,4),
					new SqlParameter("@MaxQueueN", SqlDbType.Int,4),
					new SqlParameter("@LastLoginTime", SqlDbType.DateTime),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.UserID;
			parameters[2].Value = model.MaxDialogueN;
			parameters[3].Value = model.MaxQueueN;
			parameters[4].Value = model.LastLoginTime;
			parameters[5].Value = model.CreateTime;
			parameters[6].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_EMPLOYEEAGENT_UPDATE,parameters);
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

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EMPLOYEEAGENT_DELETE,parameters);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, int RecID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
			parameters[0].Value = RecID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_EMPLOYEEAGENT_DELETE,parameters);
		}
		#endregion

        #region 数据权限公用方法
        /// <summary>
        /// 根据表名称，分组字段名称，当前人字段名称，当前登录人id，拼接数据权限条件
        /// </summary>
        /// <param name="tablename">表名称，或表别名</param>
        /// <param name="BgIDFileName">分组字段名称</param>
        /// <param name="UserIDFileName">个人权限字段</param>
        /// <param name="UserID">当前人id</param>
        /// <returns></returns>
        public string GetSqlRightstr(string tablename, string BgIDFileName, string UserIDFileName, int UserID)
        {
            string where = string.Empty;
            //where += "  and (" + tablename + "." + UserIDFileName + "='" + UserID + "'";
            string gourpstr = GetGroupStr(UserID);
            if (!string.IsNullOrEmpty(gourpstr))
            {
                where += " and " + tablename + "." + BgIDFileName + " in (" + gourpstr + ")";
            }
            where += ")";
            
            return where;
        }
        /// <summary>
        /// 根据人取人所对应分组串
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetGroupStr(int userid)
        {
            string groupstr = string.Empty;
            string sqlstr = "SELECT distinct groupstr=ISNULL(STUFF((SELECT ',' + RTRIM(UserGroupDataRigth.BGID) FROM UserGroupDataRigth where [dbo].UserGroupDataRigth.userid = f.userid FOR XML PATH('')), 1, 1, ''), '') FROM dbo.UserGroupDataRigth f WHERE UserID=" + userid;

            DataTable dt = SqlHelper.ExecuteDataset(ConnectionStrings_CC, CommandType.Text, sqlstr, null).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                groupstr = dt.Rows[0]["groupstr"].ToString();
            }
            return groupstr;
        }
        #endregion

    }
}

