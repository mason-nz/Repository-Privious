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
	/// 数据访问类ProjectTask_DMSMember。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:31 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ProjectTask_DMSMember : DataBase
	{
		#region Instance
		public static readonly ProjectTask_DMSMember Instance = new ProjectTask_DMSMember();
		#endregion

		#region const
		private const string P_PROJECTTASK_DMSMEMBER_SELECT = "p_ProjectTask_DMSMember_Select";
        public const string P_PROJECTTASK_DMSMEMBER_SELECT_BY_ID = "p_ProjectTask_DMSMember_select_by_id";
		private const string P_PROJECTTASK_DMSMEMBER_INSERT = "p_ProjectTask_DMSMember_Insert";
		private const string P_PROJECTTASK_DMSMEMBER_UPDATE = "p_ProjectTask_DMSMember_Update";
		private const string P_PROJECTTASK_DMSMEMBER_DELETE = "p_ProjectTask_DMSMember_Delete";
        public const string P_PROJECTTASK_DMSMEMBER_SELECT_CREATESOURCEBYCC = "p_ProjectTask_DMSMember_Select_CreateSourceByCC";
        public const string P_PROJECTTASK_DMSMEMBER_SELECT_STATCREATESOURCEBYCC = "p_ProjectTask_DMSMember_Select_StatCreateSourceByCC";
        public const string P_PROJECTTASK_DMSMEMBER_UPDATE_ORIGINALDMSMEMBERID_BY_ID = "p_ProjectTask_DMSMember_Update_OriginalDMSMemberID_By_ID";
		#endregion

		#region Contructor
		protected ProjectTask_DMSMember()
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
		public DataTable GetProjectTask_DMSMember(QueryProjectTask_DMSMember query, string order, int currentPage, int pageSize, out int totalCount)
		{
            StringBuilder where = new StringBuilder();

            if (query.PTID!=Constant.STRING_INVALID_VALUE)
            {
                where.Append(string.Format(" and ProjectTask_DMSMember.PTID = '{0}'", StringHelper.SqlFilter(query.PTID)));
            }


            DataSet ds;
            SqlParameter[] parameters = {
                new SqlParameter("@where", SqlDbType.VarChar,8000),
			    new SqlParameter("@order", SqlDbType.NVarChar,100),
			    new SqlParameter("@pagesize", SqlDbType.Int,4),
			    new SqlParameter("@page", SqlDbType.Int,4),
			    new SqlParameter("@totalRecorder", SqlDbType.Int,4)
             };

            parameters[0].Value = where.ToString();
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_DMSMEMBER_SELECT, parameters);

            totalCount = int.Parse(parameters[4].Value.ToString());

            return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
        public Entities.ProjectTask_DMSMember GetProjectTask_DMSMember(int memberId)
        {
            DataSet ds;
            SqlParameter[] parameters = {
                new SqlParameter("@ID", SqlDbType.Int,4)
            };

            parameters[0].Value = memberId;
            //绑定存储过程参数

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_DMSMEMBER_SELECT_BY_ID, parameters);

            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Entities.ProjectTask_DMSMember m;
                    m = LoadSingleProjectTask_DMSMember(ds.Tables[0].Rows[0]);
                    LoadBrandInfoOfMember(m);
                    return m;
                }
            }
            return null;
        }

        private void LoadBrandInfoOfMember(Entities.ProjectTask_DMSMember m)
        {
            StringBuilder sb1 = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();

            //品牌
            DataTable dtdb = Dal.ProjectTask_DMSMember_Brand.Instance.GetProjectTask_DMSMember_MainBrand(m.MemberID);
            foreach (DataRow drdb in dtdb.Rows)
            {
                sb1.Append(drdb["BrandID"].ToString() + ",");
                sb2.Append(drdb["BrandName"].ToString() + ",");
            }
            m.BrandIDs = sb1.ToString().Trim(',');
            m.BrandNames = sb2.ToString().Trim(',');

            //附加子品牌
            sb1 = new StringBuilder();
            sb2 = new StringBuilder();
            dtdb = Dal.ProjectTask_DMSMember_Brand.Instance.GetProjectTask_DMSMember_SerialBrand(m.MemberID);
            foreach (DataRow drdb in dtdb.Rows)
            {
                sb1.Append(drdb["SerialID"].ToString() + ",");
                sb2.Append(drdb["SerialName"].ToString() + ",");
            }
            m.SerialIds = sb1.ToString().Trim(',');
            m.SerialNames = sb2.ToString().Trim(',');

        }

        private static Entities.ProjectTask_DMSMember LoadSingleProjectTask_DMSMember(DataRow row)
        {
            Entities.ProjectTask_DMSMember model = new Entities.ProjectTask_DMSMember();


            if (row["MemberID"] != DBNull.Value)
            {
                model.MemberID = Convert.ToInt32(row["MemberID"].ToString());
            }

            if (row["PTID"] != DBNull.Value)
            {
                model.PTID = row["PTID"].ToString();
            }

            if (row["OriginalDMSMemberID"] != DBNull.Value)
            {
                model.OriginalDMSMemberID = row["OriginalDMSMemberID"].ToString();
            }

            if (row["MemberCode"] != DBNull.Value)
            {
                model.MemberCode = row["MemberCode"].ToString();
            }

            if (row["Name"] != DBNull.Value)
            {
                model.Name = row["Name"].ToString();
            }

            if (row["Abbr"] != DBNull.Value)
            {
                model.Abbr = row["Abbr"].ToString();
            }
            if (row["MemberType"] != DBNull.Value)
            {
                model.MemberType = row["MemberType"].ToString();
            }
            if (row["Phone"] != DBNull.Value)
            {
                model.Phone = row["Phone"].ToString();
            }

            if (row["Fax"] != DBNull.Value)
            {
                model.Fax = row["Fax"].ToString();
            }

            if (row["CompanyWebSite"] != DBNull.Value)
            {
                model.CompanyWebSite = row["CompanyWebSite"].ToString();
            }

            if (row["Email"] != DBNull.Value)
            {
                model.Email = row["Email"].ToString();
            }

            if (row["Postcode"] != DBNull.Value)
            {
                model.Postcode = row["Postcode"].ToString();
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

            if (row["ContactAddress"] != DBNull.Value)
            {
                model.ContactAddress = row["ContactAddress"].ToString();
            }

            if (row["Longitude"] != DBNull.Value)
            {
                model.Longitude = row["Longitude"].ToString();
            }

            if (row["Lantitude"] != DBNull.Value)
            {
                model.Lantitude = row["Lantitude"].ToString();
            }

            if (row["TrafficInfo"] != DBNull.Value)
            {
                model.TrafficInfo = row["TrafficInfo"].ToString();
            }

            if (row["BrandGroupID"] != DBNull.Value)
            {
                model.BrandGroupID = Convert.ToInt32(row["BrandGroupID"].ToString());
            }

            if (row["EnterpriseBrief"] != DBNull.Value)
            {
                model.EnterpriseBrief = row["EnterpriseBrief"].ToString();
            }

            if (row["Remarks"] != DBNull.Value)
            {
                model.Remarks = row["Remarks"].ToString();
            }

            if (row["Status"] != DBNull.Value)
            {
                model.Status = Convert.ToInt32(row["Status"].ToString());
            }

            if (row["SyncStatus"] != DBNull.Value)
            {
                model.SyncStatus = Convert.ToInt32(row["SyncStatus"].ToString());
            }

            return model;
        }
		#endregion

		#region Insert
		/// <summary>
		///  增加一条数据
		/// </summary>
		public int Insert(Entities.ProjectTask_DMSMember model)
		{
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@PTID", SqlDbType.VarChar),
					new SqlParameter("@OriginalDMSMemberID", SqlDbType.VarChar,50),
					new SqlParameter("@Name", SqlDbType.VarChar,256),
					new SqlParameter("@Abbr", SqlDbType.VarChar,64),
					new SqlParameter("@Phone", SqlDbType.VarChar,256),
					new SqlParameter("@Fax", SqlDbType.VarChar,128),
					new SqlParameter("@CompanyWebSite", SqlDbType.VarChar,256),
					new SqlParameter("@Email", SqlDbType.VarChar,256),
					new SqlParameter("@Postcode", SqlDbType.VarChar,32),
					new SqlParameter("@ProvinceID", SqlDbType.VarChar,20),
					new SqlParameter("@CityID", SqlDbType.VarChar,20),
					new SqlParameter("@CountyID", SqlDbType.VarChar,20),
					new SqlParameter("@ContactAddress", SqlDbType.VarChar,256),
					new SqlParameter("@Longitude", SqlDbType.VarChar,64),
					new SqlParameter("@Lantitude", SqlDbType.VarChar,64),
					new SqlParameter("@TrafficInfo", SqlDbType.VarChar,1024),
					new SqlParameter("@BrandGroupID", SqlDbType.Int,4),
					new SqlParameter("@EnterpriseBrief", SqlDbType.VarChar,1024),
					new SqlParameter("@Remarks", SqlDbType.VarChar,256),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@MemberType", SqlDbType.VarChar,20)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.PTID;
            parameters[2].Value = model.OriginalDMSMemberID;
            parameters[3].Value = model.Name;
            parameters[4].Value = model.Abbr;
            parameters[5].Value = model.Phone;
            parameters[6].Value = model.Fax;
            parameters[7].Value = model.CompanyWebSite;
            parameters[8].Value = model.Email;
            parameters[9].Value = model.Postcode;
            parameters[10].Value = model.ProvinceID;
            parameters[11].Value = model.CityID;
            parameters[12].Value = model.CountyID;
            parameters[13].Value = model.ContactAddress;
            parameters[14].Value = model.Longitude;
            parameters[15].Value = model.Lantitude;
            parameters[16].Value = model.TrafficInfo;
            parameters[17].Value = model.BrandGroupID;
            parameters[18].Value = model.EnterpriseBrief;
            parameters[19].Value = model.Remarks;
            parameters[20].Value = model.Status;
            parameters[21].Value = model.MemberType;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_DMSMEMBER_INSERT, parameters);

            return Convert.ToInt32(parameters[0].Value.ToString());
		}
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(Entities.ProjectTask_DMSMember model)
		{
            SqlParameter[] parameters = {
					new SqlParameter("@MemberID", SqlDbType.Int,4),
					new SqlParameter("@PTID", SqlDbType.VarChar),
					new SqlParameter("@OriginalDMSMemberID", SqlDbType.VarChar,50),
					new SqlParameter("@Name", SqlDbType.VarChar,256),
					new SqlParameter("@Abbr", SqlDbType.VarChar,64),
					new SqlParameter("@Phone", SqlDbType.VarChar,256),
					new SqlParameter("@Fax", SqlDbType.VarChar,128),
					new SqlParameter("@CompanyWebSite", SqlDbType.VarChar,256),
					new SqlParameter("@Email", SqlDbType.VarChar,256),
					new SqlParameter("@Postcode", SqlDbType.VarChar,32),
					new SqlParameter("@ProvinceID", SqlDbType.VarChar,20),
					new SqlParameter("@CityID", SqlDbType.VarChar,20),
					new SqlParameter("@CountyID", SqlDbType.VarChar,20),
					new SqlParameter("@ContactAddress", SqlDbType.VarChar,256),
					new SqlParameter("@Longitude", SqlDbType.VarChar,64),
					new SqlParameter("@Lantitude", SqlDbType.VarChar,64),
					new SqlParameter("@TrafficInfo", SqlDbType.VarChar,1024),
					new SqlParameter("@BrandGroupID", SqlDbType.Int,4),
					new SqlParameter("@EnterpriseBrief", SqlDbType.VarChar,1024),
					new SqlParameter("@Remarks", SqlDbType.VarChar,256),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@MemberType", SqlDbType.VarChar,20)};
            parameters[0].Value = model.MemberID;
            parameters[1].Value = model.PTID;
            parameters[2].Value = model.OriginalDMSMemberID;
            parameters[3].Value = model.Name;
            parameters[4].Value = model.Abbr;
            parameters[5].Value = model.Phone;
            parameters[6].Value = model.Fax;
            parameters[7].Value = model.CompanyWebSite;
            parameters[8].Value = model.Email;
            parameters[9].Value = model.Postcode;
            parameters[10].Value = model.ProvinceID;
            parameters[11].Value = model.CityID;
            parameters[12].Value = model.CountyID;
            parameters[13].Value = model.ContactAddress;
            parameters[14].Value = model.Longitude;
            parameters[15].Value = model.Lantitude;
            parameters[16].Value = model.TrafficInfo;
            parameters[17].Value = model.BrandGroupID;
            parameters[18].Value = SqlFilter(model.EnterpriseBrief);
            parameters[19].Value = model.Remarks;
            parameters[20].Value = model.Status;
            parameters[21].Value = model.MemberType;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_DMSMEMBER_UPDATE, parameters);
		}
		#endregion

        public int Delete(int memberId)
        {
            SqlParameter[] parameters = {
			    new SqlParameter("@MemberID", SqlDbType.Int,4)
            };
            parameters[0].Value = memberId;
           return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_DMSMEMBER_DELETE, parameters);
        }

        public List<Entities.ProjectTask_DMSMember> GetProjectTask_DMSMemberByTID(string taskId)
        {
            List<Entities.ProjectTask_DMSMember> list = new List<Entities.ProjectTask_DMSMember>();

            int tc = -1;
            Entities.QueryProjectTask_DMSMember q = new QueryProjectTask_DMSMember();
            q.PTID = StringHelper.SqlFilter(taskId);
            DataTable dt = this.GetProjectTask_DMSMember(q, " ProjectTask_DMSMember.MemberID desc", 1, 10000, out tc);

            if (tc > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Entities.ProjectTask_DMSMember m = LoadSingleProjectTask_DMSMember(dr);
                    LoadBrandInfoOfMember(m);

                    list.Add(m);
                }
            }

            return list;
        }


        /// <summary>
        /// 根据条件查询从呼叫中心系统创建的会员列表
        /// </summary>
        /// <param name="query">查询实体</param>
        /// <param name="order">排序字段</param>
        /// <param name="index">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns>返回DataTable</returns>
        public DataTable GetProjectTask_DMSMemberBySourceCC(QueryProjectTask_DMSMember query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = GetProjectTask_DMSMemberBySourceCCWhere(query);

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_DMSMEMBER_SELECT_CREATESOURCEBYCC, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        private static string GetProjectTask_DMSMemberBySourceCCWhere(QueryProjectTask_DMSMember query)
        {
            string where = string.Empty;
            if (query.MemberName != Constant.STRING_INVALID_VALUE)
            {
                where += " And cdms.Name Like '%" + StringHelper.SqlFilter(query.MemberName) + "%'";
            }
            if (query.MemberAbbr != Constant.STRING_INVALID_VALUE)
            {
                where += " And cdms.Abbr Like '%" + StringHelper.SqlFilter(query.MemberAbbr) + "%'";
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
                where += " And dms.ApplyTime >= '" + StringHelper.SqlFilter(query.ApplyStartTime) + " 0:0:0'";
            }
            if (query.ApplyEndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " And dms.ApplyTime <= '" + StringHelper.SqlFilter(query.ApplyEndTime) + " 23:59:59'";
            }
            if (query.ApplyUserName != Constant.STRING_INVALID_VALUE)
            {
                where += " And dms.ApplyUserID IN (SELECT UserID FROM Crm2009.dbo.v_userinfo WHERE TrueName LIKE '%" + StringHelper.SqlFilter(query.ApplyUserName) + "%') ";
            }
            if (query.MemberOptStartTime != Constant.STRING_INVALID_VALUE)
            {
                where += " And dms.SyncTime >= '" + StringHelper.SqlFilter(query.MemberOptStartTime) + " 0:0:0'";
            }
            if (query.MemberOptEndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " And dms.SyncTime <= '" + StringHelper.SqlFilter(query.MemberOptEndTime) + " 23:59:59'";
            }
            //if (query.DefaultDMSSyncStatus != Constant.STRING_INVALID_VALUE)
            //{
            //    where += " And (dms.SyncStatus IN (" + query.DefaultDMSSyncStatus + "))";
            //}
            //else 
            if (query.DMSSyncStatus != Constant.STRING_INVALID_VALUE)
            {
                where += " And dms.SyncStatus IN (" + Dal.Util.SqlFilterByInCondition(query.DMSSyncStatus) + ") ";
            }
            if (query.DMSMemberCreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " And dms.CreateUserID = " + query.DMSMemberCreateUserID;
            }
            if (query.DMSMemberApplyUserID != Constant.INT_INVALID_VALUE)
            {
                where += " And dms.ApplyUserID = " + query.DMSMemberApplyUserID;
            }
            if (query.DMSStatus != Constant.STRING_INVALID_VALUE)
            {
                where += " And dms.Status IN (" + Dal.Util.SqlFilterByInCondition(query.DMSStatus) + ") ";
            }
             
            return where;
        }

        public DataTable StatProjectTask_DMSMemberBySourceCC(Entities.QueryProjectTask_DMSMember query)
        {
            string where = GetProjectTask_DMSMemberBySourceCCWhere(query);

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.VarChar, 8000)
					};

            parameters[0].Value = where;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_DMSMEMBER_SELECT_STATCREATESOURCEBYCC, parameters);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// 根据核实会员ID，更新CRM库中DMSMember表主键ID
        /// </summary>
        /// <param name="id">核实会员ID</param>
        /// <param name="originalDMSMemberID">DMSMember表主键ID</param>
        /// <returns></returns>
        public int UpdateOriginalDMSMemberIDByID(int id, string originalDMSMemberID)
        {
            if (!string.IsNullOrEmpty(originalDMSMemberID.Trim()))
            {
                SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int, 4),
                    new SqlParameter("@OriginalDMSMemberID", SqlDbType.VarChar, 50)
					};

                parameters[0].Value = id;
                parameters[1].Value = originalDMSMemberID.Trim();

                return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure,P_PROJECTTASK_DMSMEMBER_UPDATE_ORIGINALDMSMEMBERID_BY_ID, parameters);
            }
            return -1;
        }

        public void DeleteByTID(string tid)
        {
            string sql = string.Format("Delete FROM ProjectTask_DMSMember Where PTID='{0}'", StringHelper.SqlFilter(tid));
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

        /// <summary>
        /// 根据crm会员id取ccid
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public int GetIDByOriginalDmsMemberID(string orgid)
        {
            int id = 0;
            string sql = "select id from ProjectTask_DMSMember where originaldmsmemberid='" + StringHelper.SqlFilter(orgid) + "'";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, null);
            DataTable dt = null;
            dt = ds.Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                id = int.Parse(dt.Rows[0]["id"].ToString());
            }
            return id;
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public List<Entities.ProjectTask_DMSMember> GetDMSMemberByCustID(string CustID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  MemberID,MemberCode,Abbr,CustID,Name from ProjectTask_DMSMember ");
            strSql.Append(" where CustID=@CustID and status=0 and SyncStatus='170002' Order By CreateTime");
            SqlParameter[] parameters = {
					new SqlParameter("@CustID", SqlDbType.VarChar,50)};
            parameters[0].Value = CustID;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);

            List<Entities.ProjectTask_DMSMember> list = new List<Entities.ProjectTask_DMSMember>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(LoadSingleProjectTask_DMSMember(dr));
            }
            return list;
        }


        /// <summary>
        /// 根据条件查询从呼叫中心系统创建的会员列表
        /// </summary>
        /// <param name="query">查询实体</param>
        /// <param name="order">排序字段</param>
        /// <param name="index">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns>返回DataTable</returns>
        public DataTable GetCC_DMSMembersBySourceCC(QueryProjectTask_DMSMember query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = GetCC_DMSMembersBySourceCCWhere(query);

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ProjectTask_DMSMember_Select_CreateSourceByCC", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
         
        private static string GetCC_DMSMembersBySourceCCWhere(QueryProjectTask_DMSMember query)
        {
            string where = string.Empty;
            if (query.MemberName != Constant.STRING_INVALID_VALUE)
            {
                where += " And cdms.Name Like '%" + StringHelper.SqlFilter(query.MemberName) + "%'";
            }
            if (query.MemberAbbr != Constant.STRING_INVALID_VALUE)
            {
                where += " And cdms.Abbr Like '%" + StringHelper.SqlFilter(query.MemberAbbr) + "%'";
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
                where += " And dms.ApplyTime >= '" + StringHelper.SqlFilter(query.ApplyStartTime) + " 0:0:0'";
            }
            if (query.ApplyEndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " And dms.ApplyTime <= '" + StringHelper.SqlFilter(query.ApplyEndTime) + " 23:59:59'";
            }
            if (query.ApplyUserName != Constant.STRING_INVALID_VALUE)
            {
                where += " And dms.ApplyUserID IN (SELECT UserID FROM dbo.v_userinfo WHERE TrueName LIKE '%" + StringHelper.SqlFilter(query.ApplyUserName) + "%') ";
            }
            if (query.MemberOptStartTime != Constant.STRING_INVALID_VALUE)
            {
                where += " And dms.SyncTime >= '" + StringHelper.SqlFilter(query.MemberOptStartTime) + " 0:0:0'";
            }
            if (query.MemberOptEndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " And dms.SyncTime <= '" + StringHelper.SqlFilter(query.MemberOptEndTime) + " 23:59:59'";
            } 
            if (query.DMSSyncStatus != Constant.STRING_INVALID_VALUE)
            {
                where += " And dms.SyncStatus IN (" + Dal.Util.SqlFilterByInCondition(query.DMSSyncStatus) + ") ";
            }
            if (query.DMSMemberCreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " And dms.CreateUserID = " + query.DMSMemberCreateUserID;
            }
            if (query.DMSMemberApplyUserID != Constant.INT_INVALID_VALUE)
            {
                where += " And dms.ApplyUserID = " + query.DMSMemberApplyUserID;
            }
            if (query.DMSStatus != Constant.STRING_INVALID_VALUE)
            {
                where += " And dms.Status IN (" + Dal.Util.SqlFilterByInCondition(query.DMSStatus) + ") ";
            }
            return where;
        }

        public DataTable StatCC_DMSMembersBySourceCC(QueryProjectTask_DMSMember query)
        {
            string where = GetProjectTask_DMSMemberBySourceCCWhere(query);

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.VarChar, 8000)
					};

            parameters[0].Value = where;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ProjectTask_DMSMember_Select_StatCreateSourceByCC", parameters);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

	}
}

