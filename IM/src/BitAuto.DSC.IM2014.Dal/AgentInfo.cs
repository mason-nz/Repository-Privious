using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM2014.Entities;
using BitAuto.DSC.IM2014.Entities.Constants;
using BitAuto.Utils.Config;
using BitAuto.DSC.IM2014.Dal.GetAgenInfoServiceReference;

namespace BitAuto.DSC.IM2014.Dal
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 数据访问类AgentInfo。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-03-05 10:05:57 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class AgentInfo : DataBase
	{
		#region Instance
		public static readonly AgentInfo Instance = new AgentInfo();
		#endregion

		#region const
		private const string P_AGENTINFO_SELECT = "p_AgentInfo_Select";
		private const string P_AGENTINFO_INSERT = "p_AgentInfo_Insert";
		private const string P_AGENTINFO_UPDATE = "p_AgentInfo_Update";
		private const string P_AGENTINFO_DELETE = "p_AgentInfo_Delete";
		#endregion

		#region Contructor
		protected AgentInfo()
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
		public DataTable GetAgentInfo(QueryAgentInfo query, string order, int currentPage, int pageSize, out int totalCount)
		{
			string where = string.Empty;

            if (query.AgentID != Constant.STRING_INVALID_VALUE)
            {
                where += " and agentid='" + query.AgentID + "'";
            }

            if (query.DomainAccount != Constant.STRING_EMPTY_VALUE)
            {
                where += " and DomainAccount='" + query.DomainAccount + "'";
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_AGENTINFO_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.AgentInfo GetAgentInfo(string AgentID)
		{
			QueryAgentInfo query = new QueryAgentInfo();
			query.AgentID = AgentID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetAgentInfo(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleAgentInfo(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.AgentInfo LoadSingleAgentInfo(DataRow row)
		{
			Entities.AgentInfo model=new Entities.AgentInfo();

				if(row["AgentID"].ToString()!="")
				{
					model.AgentID=row["AgentID"].ToString();
				}
				if(row["AgentStatus"].ToString()!="")
				{
					model.AgentStatus=int.Parse(row["AgentStatus"].ToString());
				}
				if(row["Status"].ToString()!="")
				{
					model.Status=int.Parse(row["Status"].ToString());
				}
				if(row["Priority"].ToString()!="")
				{
					model.Priority=int.Parse(row["Priority"].ToString());
				}
				if(row["MaxDialogCount"].ToString()!="")
				{
					model.MaxDialogCount=int.Parse(row["MaxDialogCount"].ToString());
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
		public void Insert(Entities.AgentInfo model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@AgentID", SqlDbType.VarChar,50),
					new SqlParameter("@AgentStatus", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@Priority", SqlDbType.Int,4),
					new SqlParameter("@MaxDialogCount", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                    new SqlParameter("@DomainAccount", SqlDbType.NVarChar,50),
                    new SqlParameter("@UserName", SqlDbType.VarChar,50)};
			parameters[0].Value = model.AgentID;
			parameters[1].Value = model.AgentStatus;
			parameters[2].Value = model.Status;
			parameters[3].Value = model.Priority;
			parameters[4].Value = model.MaxDialogCount;
			parameters[5].Value = model.CreateTime;
			parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.DomainAccount;
            parameters[8].Value = model.UserName;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_AGENTINFO_INSERT,parameters);
		}
		/// <summary>
		///  增加一条数据
		/// </summary>
		public void Insert(SqlTransaction sqltran, Entities.AgentInfo model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@AgentID", SqlDbType.VarChar,50),
					new SqlParameter("@AgentStatus", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@Priority", SqlDbType.Int,4),
					new SqlParameter("@MaxDialogCount", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.AgentID;
			parameters[1].Value = model.AgentStatus;
			parameters[2].Value = model.Status;
			parameters[3].Value = model.Priority;
			parameters[4].Value = model.MaxDialogCount;
			parameters[5].Value = model.CreateTime;
			parameters[6].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_AGENTINFO_INSERT,parameters);
		}
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(Entities.AgentInfo model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@AgentID", SqlDbType.VarChar,50),
					new SqlParameter("@AgentStatus", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@Priority", SqlDbType.Int,4),
					new SqlParameter("@MaxDialogCount", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.AgentID;
			parameters[1].Value = model.AgentStatus;
			parameters[2].Value = model.Status;
			parameters[3].Value = model.Priority;
			parameters[4].Value = model.MaxDialogCount;
			parameters[5].Value = model.CreateTime;
			parameters[6].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_AGENTINFO_UPDATE,parameters);
		}
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.AgentInfo model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@AgentID", SqlDbType.VarChar,50),
					new SqlParameter("@AgentStatus", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@Priority", SqlDbType.Int,4),
					new SqlParameter("@MaxDialogCount", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.AgentID;
			parameters[1].Value = model.AgentStatus;
			parameters[2].Value = model.Status;
			parameters[3].Value = model.Priority;
			parameters[4].Value = model.MaxDialogCount;
			parameters[5].Value = model.CreateTime;
			parameters[6].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_AGENTINFO_UPDATE,parameters);
		}
		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(int AgentID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@AgentID", SqlDbType.VarChar,50)};
			parameters[0].Value = AgentID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_AGENTINFO_DELETE,parameters);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, int AgentID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@AgentID", SqlDbType.VarChar,50)};
			parameters[0].Value = AgentID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_AGENTINFO_DELETE,parameters);
		}
		#endregion

        /// <summary>
        /// 判断用户是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool IsExistAgentIMID(string username)
        {
            string sql = "SELECT COUNT(*) FROM dbo.AgentInfo WHERE AgentID='"+ username + "'";
            int count = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 根据域名从CC取数据初始化AgentInfo
        /// </summary>
        /// <param name="domainaccount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool InitAgentInfo(string domainaccount,out string username,out string agentid,out string msg)
        {
            msg = "";
            username = "";
            agentid = "";
            #region 登录时进行跨库访问数据，这样耦合性太高，现改为根据CC接口访问
            //string sql = "SELECT  ea.UserID UserID ,ui.ADName DomainAccount ,ui.TrueName UserName,ea.AgentNum "+
            //             "FROM    CC2012.dbo.EmployeeAgent ea "+
            //             "LEFT JOIN CRM2009.dbo.v_userinfo ui ON ui.UserID = ea.UserID WHERE ui.ADName='"+ domainaccount + "'";

            //DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            //DataTable dt = null;
            //if (ds != null && ds.Tables[0] != null)
            //{
            //    dt = ds.Tables[0];
            //    if (dt.Rows.Count > 0)
            //    {
            //        Entities.AgentInfo model = new Entities.AgentInfo();
            //        if (string.IsNullOrEmpty(dt.Rows[0]["AgentNum"].ToString()))
            //        {
            //            msg = "坐席工号不存在!";
            //            return false;
            //        }
            //        model.AgentID = dt.Rows[0]["AgentNum"].ToString();
            //        model.AgentStatus = 1;
            //        model.Status = 0;
            //        model.Priority = 0;
            //        model.MaxDialogCount = 30;
            //        model.CreateTime = DateTime.Now;
            //        model.CreateUserID = Convert.ToInt32(dt.Rows[0]["UserID"]);
            //        model.DomainAccount = domainaccount;
            //        model.UserName = dt.Rows[0]["UserName"].ToString();
            //        username = model.UserName;
            //        agentid = model.AgentID;
            //        Insert(model);
            //    }
            //    else
            //    {
            //        msg = "用户在CC系统中不存在!";
            //        return false;
            //    }
            //}
            //else
            //{
            //    msg = "用户在CC系统中不存在!";
            //    return false;
            //}

            //return true;
            #endregion
            string AgentInfoAuthorizeCode = ConfigurationUtil.GetAppSettingValue("AgentInfoAuthorizeCode");
            int total = 0;
            AgentInfoCondition condition = new AgentInfoCondition();
            condition.ADName = domainaccount;
            GetCallRecordListSoapClient client = new GetCallRecordListSoapClient();
            DataTable dt = client.GetAgentInfoByCondition(AgentInfoAuthorizeCode, condition, 9999, 1, ref total, ref msg);
            if (dt != null && dt.Rows.Count > 0)
            {
                Entities.AgentInfo model = new Entities.AgentInfo();
                if (string.IsNullOrEmpty(dt.Rows[0]["AgentNum"].ToString()))
                {
                    msg = "坐席工号不存在!";
                    return false;
                }
                model.AgentID = dt.Rows[0]["AgentNum"].ToString();
                model.AgentStatus = 1;
                model.Status = 0;
                model.Priority = 0;
                model.MaxDialogCount = 30;
                model.CreateTime = DateTime.Now;
                model.CreateUserID = Convert.ToInt32(dt.Rows[0]["UserID"]);
                model.DomainAccount = domainaccount;
                model.UserName = dt.Rows[0]["TrueName"].ToString();
                username = model.UserName;
                agentid = model.AgentID;
                Insert(model);
            }
            else
            {
                msg = "用户在CC系统中不存在!";
                return false;
            }

            return true;
        }
	}
}

