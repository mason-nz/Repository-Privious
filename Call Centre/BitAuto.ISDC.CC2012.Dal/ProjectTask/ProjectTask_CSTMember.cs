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
	/// 数据访问类ProjectTask_CSTMember。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:29 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ProjectTask_CSTMember : DataBase
	{
		#region Instance
		public static readonly ProjectTask_CSTMember Instance = new ProjectTask_CSTMember();
		#endregion

		#region const
		private const string P_PROJECTTASK_CSTMEMBER_SELECT = "p_ProjectTask_CSTMember_Select";
		private const string P_PROJECTTASK_CSTMEMBER_INSERT = "p_ProjectTask_CSTMember_Insert";
		private const string P_PROJECTTASK_CSTMEMBER_UPDATE = "p_ProjectTask_CSTMember_Update";
		private const string P_PROJECTTASK_CSTMEMBER_DELETE = "p_ProjectTask_CSTMember_Delete";
        private const string P_PROJECTTASK_CSTMEMBER_SELECT_BY_ID = "p_ProjectTask_CSTMember_select_by_id";
        private const string P_PROJECTTASK_CSTMEMBER_SELECT_CREATESOURCEBYCC = "p_ProjectTask_CSTMember_Select_CreateSourceByCC";
        private const string P_PROJECTTASK_CSTMEMBER_SELECT_STATCREATESOURCEBYCC = "p_ProjectTask_CSTMember_Select_StatCreateSourceByCC";
		#endregion

		#region Contructor
		protected ProjectTask_CSTMember()
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
		public DataTable GetProjectTask_CSTMember(QueryProjectTask_CSTMember query, string order, int currentPage, int pageSize, out int totalCount)
		{
            string where = string.Empty;
            if (query.FullName != Constant.STRING_INVALID_VALUE)
            {
                where += " And FullName='" + StringHelper.SqlFilter(query.FullName) + "'";
            }
            if (query.ShortName != Constant.STRING_INVALID_VALUE)
            {
                where += " And ShortName='" + StringHelper.SqlFilter(query.ShortName) + "'";
            }
            if (query.VendorCode != Constant.STRING_INVALID_VALUE)
            {
                where += " And VendorCode='" + StringHelper.SqlFilter(query.VendorCode) + "'";
            }
            if (query.ID != Constant.INT_INVALID_VALUE)
            {
                where += " And ID=" + query.ID;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_CSTMEMBER_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
		}

        /// <summary>
        /// 根据任务编号查询车商通会员信息
        /// </summary>
        /// <param name="TID"></param>
        /// <returns></returns>
        public List<Entities.ProjectTask_CSTMember> GetProjectTask_CSTMemberByTID(string TID)
        {
            List<Entities.ProjectTask_CSTMember> list = new List<Entities.ProjectTask_CSTMember>();
            string sqlStr = "SELECT * FROM ProjectTask_CSTMember WHERE status=0 and  PTID='" + StringHelper.SqlFilter(TID)+"'";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                Entities.ProjectTask_CSTMember cstMember = new Entities.ProjectTask_CSTMember();
                cstMember = LoadSingleProjectTask_CSTMember(dr);
                list.Add(cstMember);
            }

            return list;
        }

        /// <summary>
        /// 按照车商通唯一标记ID查询符合条件的一条记录
        /// </summary>
        public Entities.ProjectTask_CSTMember GetProjectTask_CSTMember(int memberId)
        {
            DataSet ds;
            SqlParameter[] parameters = {
                new SqlParameter("@ID", SqlDbType.Int,4)
            };

            parameters[0].Value = memberId;
            //绑定存储过程参数

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_CSTMEMBER_SELECT_BY_ID, parameters);

            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Entities.ProjectTask_CSTMember m;
                    m = LoadSingleProjectTask_CSTMember(ds.Tables[0].Rows[0]);
                    return m;
                }
            }
            return null;
        }


        /// <summary>
        /// 得到对象ID
        /// </summary>
        public int GetIDByCSTRecID(string CSTRecID)
        {
            int ID = -1;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  * from ProjectTask_CSTMember ");
            strSql.Append(" where OriginalCSTRecID=@OriginalCSTRecID");
            SqlParameter[] parameters = {
					new SqlParameter("@OriginalCSTRecID", SqlDbType.VarChar,10)};
            parameters[0].Value = CSTRecID;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ID"] != null && ds.Tables[0].Rows[0]["ID"].ToString() != "")
                {
                    ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                }

            }
            return ID;
        }

        /// <summary>
        /// 得到对象ID
        /// </summary>
        public string GetIDByCSTTID(int ID)
        {
            string tID = string.Empty;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  * from ProjectTask_CSTMember ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["PTID"] != null && ds.Tables[0].Rows[0]["PTID"].ToString() != "")
                {
                    tID = ds.Tables[0].Rows[0]["PTID"].ToString();
                }

            }
            return tID;
        }

        private static Entities.ProjectTask_CSTMember LoadSingleProjectTask_CSTMember(DataRow row)
        {
            Entities.ProjectTask_CSTMember model = new Entities.ProjectTask_CSTMember();


            if (row["ID"] != DBNull.Value)
            {
                model.ID = Convert.ToInt32(row["ID"].ToString());
            }

            if (row["PTID"] != DBNull.Value)
            {
                model.PTID = row["PTID"].ToString();
            }

            if (row["OriginalCSTRecID"] != DBNull.Value)
            {
                model.OriginalCSTRecID = row["OriginalCSTRecID"].ToString();
            }

            if (row["VendorCode"] != DBNull.Value)
            {
                model.VendorCode = row["VendorCode"].ToString();
            }

            if (row["FullName"] != DBNull.Value)
            {
                model.FullName = row["FullName"].ToString();
            }

            if (row["ShortName"] != DBNull.Value)
            {
                model.ShortName = row["ShortName"].ToString();
            }

            if (row["VendorClass"] != DBNull.Value)
            {
                model.VendorClass = int.Parse(row["VendorClass"].ToString());
            }

            if (row["SuperId"] != DBNull.Value)
            {
                model.SuperId = int.Parse(row["SuperId"].ToString());
            }

            if (row["ProvinceID"] != DBNull.Value)
            {
                model.ProvinceID = row["ProvinceID"].ToString();
            }

            if (row["CityID"] != DBNull.Value)
            {
                model.CityID = row["CityID"].ToString();
            }

            if (row["CountyID"] != DBNull.Value)
            {
                model.CountyID = row["CountyID"].ToString();
            }

            if (row["Address"] != DBNull.Value)
            {
                model.Address = row["Address"].ToString();
            }

            if (row["PostCode"] != DBNull.Value)
            {
                model.PostCode = row["PostCode"].ToString();
            }

            if (row["TrafficInfo"] != DBNull.Value)
            {
                model.TrafficInfo = row["TrafficInfo"].ToString();
            }

            if (row["CreateTime"] != DBNull.Value)
            {
                model.CreateTime = Convert.ToDateTime(row["CreateTime"].ToString());
            }

            if (row["CreateUserID"] != DBNull.Value)
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }

            if (row["Status"] != DBNull.Value)
            {
                model.Status = Convert.ToInt32(row["Status"].ToString());
            }

            //if (row["SyncStatus"] != DBNull.Value)
            //{
            //    model.SyncStatus = Convert.ToInt32(row["SyncStatus"].ToString());
            //}

            return model;
        }
		#endregion
        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ProjectTask_CSTMember GetProjectTask_CSTMemberModel(int ID)
        {
            QueryProjectTask_CSTMember query = new QueryProjectTask_CSTMember();
            query.ID = ID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectTask_CSTMember(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleProjectTask_CSTMember(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        #endregion


		#region Insert
		/// <summary>
		///  增加一条数据
		/// </summary>
		public int Insert(Entities.ProjectTask_CSTMember model)
		{
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@PTID", SqlDbType.VarChar),
					new SqlParameter("@VendorCode", SqlDbType.NVarChar,50),
					new SqlParameter("@FullName", SqlDbType.NVarChar,200),
					new SqlParameter("@ShortName", SqlDbType.NVarChar,50),
					new SqlParameter("@VendorClass", SqlDbType.Int,4),
					new SqlParameter("@SuperId", SqlDbType.Int,4),
					new SqlParameter("@ProvinceID", SqlDbType.NVarChar,20),
					new SqlParameter("@CityID", SqlDbType.NVarChar,20),
					new SqlParameter("@CountyID", SqlDbType.VarChar,20),
					new SqlParameter("@Address", SqlDbType.VarChar,300),
					new SqlParameter("@PostCode", SqlDbType.Char,6),
					new SqlParameter("@TrafficInfo", SqlDbType.VarChar,1024),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.PTID;
            parameters[2].Value = model.VendorCode;
            parameters[3].Value = model.FullName;
            parameters[4].Value = model.ShortName;
            parameters[5].Value = model.VendorClass;
            parameters[6].Value = model.SuperId;
            parameters[7].Value = model.ProvinceID;
            parameters[8].Value = model.CityID;
            parameters[9].Value = model.CountyID;
            parameters[10].Value = model.Address;
            parameters[11].Value = model.PostCode;
            parameters[12].Value = model.TrafficInfo;
            parameters[13].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_CSTMEMBER_INSERT, parameters);
            return (int)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(Entities.ProjectTask_CSTMember model)
		{
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@OriginalCSTRecID", SqlDbType.VarChar,10),
					new SqlParameter("@VendorCode", SqlDbType.NVarChar,50),
					new SqlParameter("@FullName", SqlDbType.NVarChar,200),
					new SqlParameter("@ShortName", SqlDbType.NVarChar,50),
					new SqlParameter("@VendorClass", SqlDbType.Int,4),
					new SqlParameter("@SuperId", SqlDbType.Int,4),
					new SqlParameter("@ProvinceID", SqlDbType.NVarChar,20),
					new SqlParameter("@CityID", SqlDbType.NVarChar,20),
					new SqlParameter("@CountyID", SqlDbType.VarChar,20),
					new SqlParameter("@Address", SqlDbType.VarChar,300),
					new SqlParameter("@PostCode", SqlDbType.Char,6),
					new SqlParameter("@TrafficInfo", SqlDbType.VarChar,1024)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.OriginalCSTRecID;
            parameters[2].Value = model.VendorCode;
            parameters[3].Value = model.FullName;
            parameters[4].Value = model.ShortName;
            parameters[5].Value = model.VendorClass;
            parameters[6].Value = model.SuperId;
            parameters[7].Value = model.ProvinceID;
            parameters[8].Value = model.CityID;
            parameters[9].Value = model.CountyID;
            parameters[10].Value = model.Address;
            parameters[11].Value = model.PostCode;
            parameters[12].Value = model.TrafficInfo;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_CSTMEMBER_UPDATE, parameters);
		}
		#endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int ID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_CSTMEMBER_DELETE, parameters);
        }

        /// <summary>
        /// 根据任务ID删除新增的车商通会员
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public int DeleteNewProjectTask_CSTMemberByTID(string tid)
        {
            string sql = "UPDATE ProjectTask_CSTMember SET Status=-1  WHERE PTID=@PTID And (OriginalCSTRecID is null or OriginalCSTRecID='') ";
            SqlParameter[] parameters ={
                                           new SqlParameter("@PTID",tid)
                                      };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }
        /// <summary>
        /// 根据任务ID，更新表CC_CSTMember，字段Status为-1
        /// </summary>
        /// <param name="tid">任务ID</param>
        /// <returns></returns>
        public int DeleteByTID(string tid)
        {
            string sqlStr = string.Format("DELETE FROM ProjectTask_CSTMember WHERE Status=0 AND PTID='{0}'", StringHelper.SqlFilter(tid));
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
        }
        #endregion

        #region
        /// <summary>
        /// 是否存在同一名称的会员
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool IsExistSameName(string where)
        {
            string sql = "SELECT ID FROM ProjectTask_CSTMember WHERE 1=1  " + where;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 根据条件查询从呼叫中心系统创建的车商通会员列表
        /// </summary>
        /// <param name="query">查询实体</param>
        /// <param name="order">排序字段</param>
        /// <param name="index">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns>返回DataTable</returns>
        public DataTable GetProjectTask_CSTMemberBySourceCC(QueryProjectTask_CSTMember query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = GetProjectTask_CSTMemberBySourceCCWhere(query);

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.VarChar, 8000),
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_CSTMEMBER_SELECT_CREATESOURCEBYCC, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        private static string GetProjectTask_CSTMemberBySourceCCWhere(QueryProjectTask_CSTMember query)
        {
            string where = string.Empty;
            if (query.FullName != Constant.STRING_INVALID_VALUE)
            {
                where += " And ccst.FullName Like '%" + StringHelper.SqlFilter(query.FullName) + "%'";
            }
            if (query.ShortName != Constant.STRING_INVALID_VALUE)
            {
                where += " And ccst.ShortName Like '%" + StringHelper.SqlFilter(query.ShortName) + "%'";
            }
            if (query.CustName != Constant.STRING_INVALID_VALUE)
            {
                where += " And cc.CustName Like '%" + StringHelper.SqlFilter(query.CustName) + "%'";
            }
            if (query.CustID != Constant.STRING_INVALID_VALUE)
            {
                where += " And ct.CRMCustID = '" + StringHelper.SqlFilter(query.CustID) + "'";
            }
            if (query.ApplyStartTime != Constant.STRING_INVALID_VALUE)
            {
                where += " And cst.ApplyTime >= '" + StringHelper.SqlFilter(query.ApplyStartTime) + " 0:0:0'";
            }
            if (query.ApplyEndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " And cst.ApplyTime <= '" + StringHelper.SqlFilter(query.ApplyEndTime) + " 23:59:59'";
            }
            if (query.ApplyUserName != Constant.STRING_INVALID_VALUE)
            {
                where += " And cst.CreateUserID IN (SELECT UserID FROM Crm2009.dbo.v_userinfo WHERE TrueName LIKE '%" + StringHelper.SqlFilter(query.ApplyUserName) + "%') ";
            }
            if (query.MemberOptStartTime != Constant.STRING_INVALID_VALUE)
            {
                where += " And cst.SyncTime >= '" + StringHelper.SqlFilter(query.MemberOptStartTime) + " 0:0:0'";
            }
            if (query.MemberOptEndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " And cst.SyncTime <= '" + StringHelper.SqlFilter(query.MemberOptEndTime) + " 23:59:59'";
            }
            if (query.CSTSyncStatus != Constant.STRING_INVALID_VALUE)
            {
                where += " And cst.SyncStatus IN (" + Dal.Util.SqlFilterByInCondition(query.CSTSyncStatus) + ") ";
            }
            if (query.CSTMemberCreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " And cst.CreateUserID = " + query.CSTMemberCreateUserID;
            }
            if (query.CSTMemberApplyUserID != Constant.INT_INVALID_VALUE)
            {
                where += " And cst.CreateUserID = " + query.CSTMemberApplyUserID;
            }
            if (query.CSTStatus != Constant.STRING_INVALID_VALUE)
            {
                where += " And cst.Status IN (" + Dal.Util.SqlFilterByInCondition(query.CSTStatus) + ") ";
            }
            return where;
        }
        public DataTable StatProjectTask_CSTMemberBySourceCC(Entities.QueryProjectTask_CSTMember query)
        {
            string where = GetProjectTask_CSTMemberBySourceCCWhere(query);

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.VarChar, 8000)
					};

            parameters[0].Value = where;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_CSTMEMBER_SELECT_STATCREATESOURCEBYCC, parameters);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// 获取要导出的排期信息
        /// </summary>
        /// <param name="MemberStr"></param>
        /// <returns></returns>
        public DataTable GetOrderInfo(string MemberStr)
        {
            string sqlStr = "SELECT "
                            + " [会员ID]=m.CstMemberID,"
                            + " [会员名称]=m.FullName,"
                            + " [会员省份]=m.ProvinceName,"
                            + " [会员城市]=m.CityName,"
                            + " [会员区县]=m.CountyName,"
                            + " [合作名称]=MemberCode+'('+AdDateCode+')',"
                            + " [销售类型]=CASE usestyle WHEN '4001' THEN '销售' ELSE '' END,"
                            + " [执行周期]=BeginTime+'至'+EndTime,"
                            + " [排期创建时间]=CONVERT(varchar(50),CreateTime)"
                            + " FROM MJ2009.dbo.orderinfo"
                            + " LEFT JOIN "
                            + " ("
                                + " SELECT DISTINCT me.CstMemberID,me.FullName,"
                                + " (SELECT AreaName FROM Crm2009.dbo.AreaInfo WHERE AreaID=me.ProvinceID) ProvinceName,"
                                + " (SELECT AreaName FROM Crm2009.dbo.AreaInfo WHERE AreaID=me.CityID) CityName,"
                                + " (SELECT AreaName FROM Crm2009.dbo.AreaInfo WHERE AreaID=me.CountyID) CountyName"
                                 + " FROM  Crm2009.dbo.CstMember me  WHERE CstMemberID<>-2 "
                            + " ) m ON MJ2009.dbo.OrderInfo.MemberCode=m.CstMemberID"
                            + " WHERE ordertype =6003 AND Status IN (1003,1007) AND usestyle=4001 "
                            + " AND m.CstMemberID IN"
                            + " ("
                            + Dal.Util.SqlFilterByInCondition(MemberStr)
                            + " )";

            DataSet ds;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);

            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

	}
}

