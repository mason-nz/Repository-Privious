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
    /// 数据访问类ProjectTask_ReturnVisit。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-03-07 03:04:35 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ProjectTask_ReturnVisit : DataBase
    {
        #region Instance
        public static readonly ProjectTask_ReturnVisit Instance = new ProjectTask_ReturnVisit();
        #endregion

        #region const
        private const string P_RETURNVISITReCord_SELECT = "p_returnvisitrecord_select";
        private const string P_PROJECTTASK_RETURNVISIT_SELECT = "p_ProjectTask_ReturnVisit_Select";
        private const string P_PROJECTTASK_RETURNVISIT_INSERT = "p_ProjectTask_ReturnVisit_Insert";
        private const string P_PROJECTTASK_RETURNVISIT_UPDATE = "p_ProjectTask_ReturnVisit_Update";
        private const string P_PROJECTTASK_RETURNVISIT_DELETE = "p_ProjectTask_ReturnVisit_Delete";


        #endregion

        #region Contructor
        protected ProjectTask_ReturnVisit()
        { }
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
        public DataTable GetProjectTask_ReturnVisit(QueryProjectTask_ReturnVisit query, string order, int currentPage, int pageSize, out int totalCount)
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_RETURNVISIT_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }


        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetReturnVisitReCordList(QueryReturnVisitRecord query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;
            //url += '&CustName=' + escape(txtSearchCustName);
            //       url += '&CustID=' + escape(txtCustID);
            //       url += '&Province=' + escape(ddlSearchProvince);
            //       url += '&City=' + escape(ddlSearchCity);
            //       url += '&County=' + escape(ddlSearchCounty);
            //       url += '&CustType=' + escape(selCustType);
            //       url += '&StartTime=' + escape(txtStartTime);
            //       url += '&EndTime=' + escape(txtEndTime);
            //       url += '&TypeID=' + escape(TypeID);
            //       url += '&VisitType=' + escape(selVisitType);
            //       url += '&VisitUserid=' + escape(SelectUserid);
            //       url += '&ProjectName=' + escapeStr(txtSearchProjectName);

            where = GetWhereByQuery(query, where);

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_RETURNVISITReCord_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        private static string GetWhereByQuery(QueryReturnVisitRecord query, string where)
        {
            if (query.CustName != Constant.STRING_INVALID_VALUE)
            {
                where += " and CustInfo.CustName like '%" + Utils.StringHelper.SqlFilter(query.CustName) + "%'";
            }
            if (query.CustID != Constant.STRING_INVALID_VALUE)
            {
                where += " and CustInfo.custid='" + Utils.StringHelper.SqlFilter(query.CustID) + "'";
            }
            if (query.ProvinceID != Constant.STRING_INVALID_VALUE)
            {
                where += " and CustInfo.provinceID='" + Utils.StringHelper.SqlFilter(query.ProvinceID) + "'";
            }
            if (query.CityID != Constant.STRING_INVALID_VALUE)
            {
                where += " and CustInfo.CityID='" + Utils.StringHelper.SqlFilter(query.CityID) + "'";
            }
            if (query.CountyID != Constant.STRING_INVALID_VALUE)
            {
                where += " and CustInfo.CountyID='" + Utils.StringHelper.SqlFilter(query.CountyID) + "'";
            }
            if (query.CustType != Constant.STRING_INVALID_VALUE && query.CustType != "-2")
            {
                where += " and CustInfo.TypeID='" + Utils.StringHelper.SqlFilter(query.CustType) + "'";
            }
            if (query.StartTime != Constant.DATE_INVALID_VALUE && query.EndTime != Constant.DATE_INVALID_VALUE)
            {
                where += " and (cast( ReturnVisit.begintime as DateTime)>='" + query.StartTime + "' and cast( ReturnVisit.begintime as DateTime)<='" + query.EndTime + "')";
            }
            if (query.TypeID != Constant.STRING_INVALID_VALUE && query.TypeID != "0")
            {
                int number = 0;
                if (int.TryParse(query.TypeID, out number))
                {
                    where += " and ReturnVisit.BusinessLine&" + number + "=" + number;
                }
            }
            if (query.VisitType != Constant.STRING_INVALID_VALUE && query.VisitType != "-2")
            {
                where += " and ReturnVisit.VisitType='" + Utils.StringHelper.SqlFilter(query.VisitType) + "'";
            }
            if (query.VisitUserid != Constant.STRING_INVALID_VALUE)
            {
                where += " and ReturnVisit.createUserID='" + Utils.StringHelper.SqlFilter(query.VisitUserid) + "'";
            }
            if (query.ProjectName != Constant.STRING_INVALID_VALUE)
            {
                where += " and CustInfo.CustID in (select custid from MJ2009.dbo.OrderInfo where projectName='" + SqlFilter(query.ProjectName) + "' and status>=0) ";
            }
            return where;
        }


        #endregion

        public DataTable GetReturnVisitReCordListExport(QueryReturnVisitRecord query)
        {
            string where = string.Empty;

            where = GetWhereByQuery(query, where);

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),					
					};

            parameters[0].Value = where;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_returnvisitrecord_export", parameters);
            return ds.Tables[0];
        }

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ProjectTask_ReturnVisit GetProjectTask_ReturnVisit(long RecID)
        {
            QueryProjectTask_ReturnVisit query = new QueryProjectTask_ReturnVisit();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectTask_ReturnVisit(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleProjectTask_ReturnVisit(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.ProjectTask_ReturnVisit LoadSingleProjectTask_ReturnVisit(DataRow row)
        {
            Entities.ProjectTask_ReturnVisit model = new Entities.ProjectTask_ReturnVisit();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = long.Parse(row["RecID"].ToString());
            }
            model.CRMCustID = row["CRMCustID"].ToString();
            model.Title = row["Title"].ToString();
            if (row["RVType"].ToString() != "")
            {
                model.RVType = int.Parse(row["RVType"].ToString());
            }
            if (row["ContactInfoUserID"].ToString() != "")
            {
                model.ContactInfoUserID = int.Parse(row["ContactInfoUserID"].ToString());
            }
            model.Remark = row["Remark"].ToString();
            model.ContactInfotitle = row["ContactInfotitle"].ToString();
            model.Begintime = row["Begintime"].ToString();
            model.Endtime = row["Endtime"].ToString();
            if (row["Createtime"].ToString() != "")
            {
                model.Createtime = DateTime.Parse(row["Createtime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            model.CreateuserDepart = row["CreateuserDepart"].ToString();
            if (row["UserClass"].ToString() != "")
            {
                model.UserClass = int.Parse(row["UserClass"].ToString());
            }
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["LastUpdateTime"].ToString() != "")
            {
                model.LastUpdateTime = DateTime.Parse(row["LastUpdateTime"].ToString());
            }
            model.RVresult = row["RVresult"].ToString();
            if (row["RVQuestionStatus"].ToString() != "")
            {
                model.RVQuestionStatus = int.Parse(row["RVQuestionStatus"].ToString());
            }
            model.RVQuestionRemark = row["RVQuestionRemark"].ToString();
            model.MemberId = row["MemberId"].ToString();
            model.VisitType = row["VisitType"].ToString();
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.ProjectTask_ReturnVisit model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CRMCustID", SqlDbType.VarChar,50),
					new SqlParameter("@Title", SqlDbType.VarChar,50),
					new SqlParameter("@RVType", SqlDbType.Int,4),
					new SqlParameter("@ContactInfoUserID", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.VarChar,500),
					new SqlParameter("@ContactInfotitle", SqlDbType.VarChar,100),
					new SqlParameter("@Begintime", SqlDbType.VarChar,50),
					new SqlParameter("@Endtime", SqlDbType.VarChar,50),
					new SqlParameter("@Createtime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateuserDepart", SqlDbType.VarChar,50),
					new SqlParameter("@UserClass", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
					new SqlParameter("@RVresult", SqlDbType.VarChar,500),
					new SqlParameter("@RVQuestionStatus", SqlDbType.Int,4),
					new SqlParameter("@RVQuestionRemark", SqlDbType.VarChar,500),
					new SqlParameter("@MemberId", SqlDbType.VarChar,50),
					new SqlParameter("@VisitType", SqlDbType.VarChar,50),
                                        new SqlParameter("@TypeID", SqlDbType.VarChar,20)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.CRMCustID;
            parameters[2].Value = model.Title;
            parameters[3].Value = model.RVType;
            parameters[4].Value = model.ContactInfoUserID;
            parameters[5].Value = model.Remark;
            parameters[6].Value = model.ContactInfotitle;
            parameters[7].Value = model.Begintime;
            parameters[8].Value = model.Endtime;
            parameters[9].Value = model.Createtime;
            parameters[10].Value = model.CreateUserID;
            parameters[11].Value = model.CreateuserDepart;
            parameters[12].Value = model.UserClass;
            parameters[13].Value = model.Status;
            parameters[14].Value = model.LastUpdateTime;
            parameters[15].Value = model.RVresult;
            parameters[16].Value = model.RVQuestionStatus;
            parameters[17].Value = model.RVQuestionRemark;
            parameters[18].Value = model.MemberId;
            parameters[19].Value = model.VisitType;
            parameters[20].Value = model.TypeID;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_RETURNVISIT_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ProjectTask_ReturnVisit model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CRMCustID", SqlDbType.VarChar,50),
					new SqlParameter("@Title", SqlDbType.VarChar,50),
					new SqlParameter("@RVType", SqlDbType.Int,4),
					new SqlParameter("@ContactInfoUserID", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.VarChar,500),
					new SqlParameter("@ContactInfotitle", SqlDbType.VarChar,100),
					new SqlParameter("@Begintime", SqlDbType.VarChar,50),
					new SqlParameter("@Endtime", SqlDbType.VarChar,50),
					new SqlParameter("@Createtime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateuserDepart", SqlDbType.VarChar,50),
					new SqlParameter("@UserClass", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
					new SqlParameter("@RVresult", SqlDbType.VarChar,500),
					new SqlParameter("@RVQuestionStatus", SqlDbType.Int,4),
					new SqlParameter("@RVQuestionRemark", SqlDbType.VarChar,500),
					new SqlParameter("@MemberId", SqlDbType.VarChar,50),
						new SqlParameter("@VisitType", SqlDbType.VarChar,50),
                                        new SqlParameter("@TypeID", SqlDbType.VarChar,20)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.CRMCustID;
            parameters[2].Value = model.Title;
            parameters[3].Value = model.RVType;
            parameters[4].Value = model.ContactInfoUserID;
            parameters[5].Value = model.Remark;
            parameters[6].Value = model.ContactInfotitle;
            parameters[7].Value = model.Begintime;
            parameters[8].Value = model.Endtime;
            parameters[9].Value = model.Createtime;
            parameters[10].Value = model.CreateUserID;
            parameters[11].Value = model.CreateuserDepart;
            parameters[12].Value = model.UserClass;
            parameters[13].Value = model.Status;
            parameters[14].Value = model.LastUpdateTime;
            parameters[15].Value = model.RVresult;
            parameters[16].Value = model.RVQuestionStatus;
            parameters[17].Value = model.RVQuestionRemark;
            parameters[18].Value = model.MemberId;
            parameters[19].Value = model.VisitType;
            parameters[20].Value = model.TypeID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_PROJECTTASK_RETURNVISIT_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.ProjectTask_ReturnVisit model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@CRMCustID", SqlDbType.VarChar,50),
					new SqlParameter("@Title", SqlDbType.VarChar,50),
					new SqlParameter("@RVType", SqlDbType.Int,4),
					new SqlParameter("@ContactInfoUserID", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.VarChar,500),
					new SqlParameter("@ContactInfotitle", SqlDbType.VarChar,100),
					new SqlParameter("@Begintime", SqlDbType.VarChar,50),
					new SqlParameter("@Endtime", SqlDbType.VarChar,50),
					new SqlParameter("@Createtime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateuserDepart", SqlDbType.VarChar,50),
					new SqlParameter("@UserClass", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
					new SqlParameter("@RVresult", SqlDbType.VarChar,500),
					new SqlParameter("@RVQuestionStatus", SqlDbType.Int,4),
					new SqlParameter("@RVQuestionRemark", SqlDbType.VarChar,500),
					new SqlParameter("@MemberId", SqlDbType.VarChar,50),
					new SqlParameter("@VisitType", SqlDbType.VarChar,50)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.CRMCustID;
            parameters[2].Value = model.Title;
            parameters[3].Value = model.RVType;
            parameters[4].Value = model.ContactInfoUserID;
            parameters[5].Value = model.Remark;
            parameters[6].Value = model.ContactInfotitle;
            parameters[7].Value = model.Begintime;
            parameters[8].Value = model.Endtime;
            parameters[9].Value = model.Createtime;
            parameters[10].Value = model.CreateUserID;
            parameters[11].Value = model.CreateuserDepart;
            parameters[12].Value = model.UserClass;
            parameters[13].Value = model.Status;
            parameters[14].Value = model.LastUpdateTime;
            parameters[15].Value = model.RVresult;
            parameters[16].Value = model.RVQuestionStatus;
            parameters[17].Value = model.RVQuestionRemark;
            parameters[18].Value = model.MemberId;
            parameters[19].Value = model.VisitType;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_RETURNVISIT_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ProjectTask_ReturnVisit model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@CRMCustID", SqlDbType.VarChar,50),
					new SqlParameter("@Title", SqlDbType.VarChar,50),
					new SqlParameter("@RVType", SqlDbType.Int,4),
					new SqlParameter("@ContactInfoUserID", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.VarChar,500),
					new SqlParameter("@ContactInfotitle", SqlDbType.VarChar,100),
					new SqlParameter("@Begintime", SqlDbType.VarChar,50),
					new SqlParameter("@Endtime", SqlDbType.VarChar,50),
					new SqlParameter("@Createtime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateuserDepart", SqlDbType.VarChar,50),
					new SqlParameter("@UserClass", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
					new SqlParameter("@RVresult", SqlDbType.VarChar,500),
					new SqlParameter("@RVQuestionStatus", SqlDbType.Int,4),
					new SqlParameter("@RVQuestionRemark", SqlDbType.VarChar,500),
					new SqlParameter("@MemberId", SqlDbType.VarChar,50),
					new SqlParameter("@VisitType", SqlDbType.VarChar,50)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.CRMCustID;
            parameters[2].Value = model.Title;
            parameters[3].Value = model.RVType;
            parameters[4].Value = model.ContactInfoUserID;
            parameters[5].Value = model.Remark;
            parameters[6].Value = model.ContactInfotitle;
            parameters[7].Value = model.Begintime;
            parameters[8].Value = model.Endtime;
            parameters[9].Value = model.Createtime;
            parameters[10].Value = model.CreateUserID;
            parameters[11].Value = model.CreateuserDepart;
            parameters[12].Value = model.UserClass;
            parameters[13].Value = model.Status;
            parameters[14].Value = model.LastUpdateTime;
            parameters[15].Value = model.RVresult;
            parameters[16].Value = model.RVQuestionStatus;
            parameters[17].Value = model.RVQuestionRemark;
            parameters[18].Value = model.MemberId;
            parameters[19].Value = model.VisitType;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_PROJECTTASK_RETURNVISIT_UPDATE, parameters);
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

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_RETURNVISIT_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_PROJECTTASK_RETURNVISIT_DELETE, parameters);
        }
        #endregion

        /// <summary>
        /// 根据客户id取cc客户负责人列表
        /// 只有一个负责员工
        /// </summary>
        /// <param name="custid"></param>
        /// <returns></returns>
        public DataTable GetCustUserForCCOne(string custid)
        {
            DataTable dt = null;

            string where = "";
            string PartIDs = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("PartID");
            int DepCount = PartIDs.Split(',').Length;
            if (DepCount > 0)
            {
                where += " and (";
                for (int i = 0; i < DepCount; i++)
                {
                    if (i != 0)
                    {
                        where += " or ";
                    }
                    where += " ui.DepartID in (select ID from SysRightsManager.dbo.f_Cid('" + PartIDs.Split(',')[i] + "')) ";
                }
                where += " )";
            }

            string sqlstr = "select custid,a.userid from crm2009.dbo.[CustUserMapping] a where custid='" + StringHelper.SqlFilter(custid) + "' and userid in(SELECT ea.UserID FROM dbo.EmployeeAgent ea LEFT JOIN CRM2009.dbo.v_userinfo ui ON ui.UserID = ea.UserID " +
                           " where 1=1 " + where + ")";

            dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, null).Tables[0];

            return dt;
        }


        /// <summary>
        /// 根据客户id取cc客户负责人列表
        /// </summary>
        /// <param name="custid"></param>
        /// <returns></returns>
        public DataTable GetCustUserForCC(string custIds, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = "";

            string[] custIdsArry = custIds.Split(',');
            foreach (string custid in custIdsArry)
            {
                where += "'" + custid + "',";
            }

            if (where.EndsWith(","))
            {
                where = where.Substring(0, where.Length - 1);
                //where = " and custid in(" + where + ")";
                where = "(" + where + ")";
            }


            DataSet ds;
            SqlParameter[] parameters = {
                new SqlParameter("@where", SqlDbType.NVarChar,4000),
                new SqlParameter("@order", SqlDbType.NVarChar,20),
			    new SqlParameter("@pagesize", SqlDbType.Int,4),
			    new SqlParameter("@indexpage", SqlDbType.Int,4),
			    new SqlParameter("@totalRecorder", SqlDbType.Int,4)
            };
            parameters[4].Direction = ParameterDirection.Output;
            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_GetCustUserForCC", parameters);

            totalCount = int.Parse(parameters[4].Value.ToString());

            return ds.Tables[0];
        }


        /// <summary>
        /// 删除cc客户负责人
        /// </summary>
        /// <param name="CustID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int DeleteCustUserMappingForCC(string CustID, string UserID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CustID", SqlDbType.VarChar),
                                        new SqlParameter("@UserID", SqlDbType.VarChar),
                                        new SqlParameter("@OperateTime", SqlDbType.DateTime)};
            parameters[0].Value = CustID;
            parameters[1].Value = UserID;
            parameters[2].Value = System.DateTime.Now;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_CustUserMapping_delete_ForNewCC", parameters);
        }



        /// <summary>
        ///在回访临时表增加一条数据
        /// </summary>
        public void InsertReturnVisitCallReCord(string CRMCustID, string SessionID)
        {
            DataTable dt = GetReturnVisitCallReCord(CRMCustID, SessionID);
            if (dt != null && dt.Rows.Count == 0)
            {
                string strSql = "insert into dbo.ReturnVisitCallReCord values('" + StringHelper.SqlFilter(CRMCustID) + "','" + StringHelper.SqlFilter(SessionID) + "')";
                SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
            }
        }



        /// <summary>
        /// 取回访临时表信息
        /// </summary>
        /// <param name="CRMCustID"></param>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        public DataTable GetReturnVisitCallReCord(string CRMCustID, string SessionID)
        {
            string strSql = "select * from ReturnVisitCallReCord where CRMCustID='" + Utils.StringHelper.SqlFilter(CRMCustID) + "' and SessionID='" + StringHelper.SqlFilter(SessionID) + "'";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return ds.Tables[0];
        }
        /// <summary>
        /// 取回访临时表信息
        /// </summary>
        /// <param name="CRMCustID"></param>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        public DataTable GetReturnVisitCallReCord(string CRMCustID)
        {
            string strSql = "select * from ReturnVisitCallReCord where CRMCustID='" + Utils.StringHelper.SqlFilter(CRMCustID) + "'";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return ds.Tables[0];
        }

        /// <summary>
        /// 取回访临时表信息
        /// </summary>
        /// <param name="CRMCustID"></param>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        public void DeleteReturnVisitCallReCord(string CRMCustID)
        {
            string strSql = "delete from ReturnVisitCallReCord where CRMCustID='" + Utils.StringHelper.SqlFilter(CRMCustID) + "'";
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }


        public void Delete(string custid)
        {
            string strSql = "DELETE from dbo.ProjectTask_ReturnVisit WHERE CRMCustID='" + Utils.StringHelper.SqlFilter(custid) + "' and status=0";
            SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }

        public DataTable GetTable(string custid)
        {
            string strSql = "select * from ProjectTask_ReturnVisit where CRMCustID='" + Utils.StringHelper.SqlFilter(custid) + "' and status=0";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return ds.Tables[0];
        }

        public void updateStatus(string custid)
        {
            string strSql = " update ProjectTask_ReturnVisit set status=-1 where CRMCustID='" + Utils.StringHelper.SqlFilter(custid) + "'";
            SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }


        public void InsertCCAddCRMContractLogForRV(int ContractID, int userid)
        {
            string strSql = " insert into CCAddCRMContractLogForRV(CRMContractID,CreateUserID,CreateTime,Status) values(" + ContractID + "," + userid + ",'" + System.DateTime.Now + "',0)";
            SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        public void DeleteCCAddCRMContractLogForRV(int ContractID)
        {
            string strSql = " update CCAddCRMContractLogForRV set Status=-1 where CRMContractID=" + ContractID + "";
            SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        /// <summary>
        /// 判断联系人记录是否是从CC添加的
        /// </summary>
        /// <param name="ContractID"></param>
        /// <returns></returns>
        public bool CCAddCRMContractLogForRVIsHave(int ContractID)
        {
            bool flag = false;
            string strSql = "select * from CCAddCRMContractLogForRV where CRMContractID=" + ContractID + " and Status=0";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                flag = true;
            }
            return flag;
        }


        /// <summary>
        /// 按照查询条件查询   
        /// </summary> 
        /// <returns>表集合</returns>
        public DataTable GetJiCaiProjectByName(string pName, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;
            if (!string.IsNullOrEmpty(pName))
            {
                where += string.Format(" and ProjectName like '%{0}%'", StringHelper.SqlFilter(pName));
            }
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
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_JiCaiProject_select", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// 取访问分类
        /// </summary>
        /// <returns></returns>
        public DataTable GetVisitType()
        {
            string sqlstr = "select  DictID,DictName from DictInfo where DictType='101' ORDER BY DictID";
            DataSet ds = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.Text, sqlstr, null);
            return ds.Tables[0];
        }
        /// <summary>
        /// 取回访信息根据回访id
        /// </summary>
        /// <returns></returns>
        public DataTable GetVisitInfoByRVID(string RVID)
        {
            string sqlstr = "select ReturnVisit.*,DictInfo.DictName,CustInfo.CustName,ContactInfo.CName,v_userinfo.truename,DMSMember.Name From  ReturnVisit as ReturnVisit left join CustInfo as CustInfo on CustInfo.custid=ReturnVisit.CustID left join v_userinfo as v_userinfo on v_userinfo.userid=ReturnVisit.createUserID left join ContactInfo as ContactInfo on ReturnVisit.ContactInfoUserID=contactInfo.ID left join DictInfo as DictInfo on ReturnVisit.VisitType=DictInfo.DictID " +
                            "left join (SELECT * FROM dbo.DMSMember WHERE MemberCode <> '')DMSMember on ReturnVisit.memberid=DMsMember.MemberCode where CustInfo.status>=0 and ReturnVisit.status>=0 and ReturnVisit.RID='" + Utils.StringHelper.SqlFilter(RVID) + "'";
            DataSet ds = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.Text, sqlstr, null);
            return ds.Tables[0];
        }


        #region 客户回访功能调整
        /// <summary>
        /// 是否存在该客户在该业务线上所分配的运营客服(userclass=7)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="memberIdList"></param>
        /// <returns></returns>
        public bool IsExistsCustMember(int userId, string memberIdList)
        {
            try
            {
                StringBuilder build = new StringBuilder();
                build.Append(@"select COUNT(1) mapcount from CustUserMapping A left join
                           SysRightsManager.[dbo].UserInfo B
                           ON A.UserID = B.[UserID]
                           where CustID in (" + Util.SqlFilterByInCondition(memberIdList) + ")" + @"
                           AND userclass=7

                           AND (BusinessLine & 1=1 ) AND (select TOP 1 BusinessLine from   SysRightsManager.[dbo].UserInfo
                           where UserID=@UserID) &1=1"); //只有新车一个客户分配一个运营客服

                //AND BusinessLine=(  
                //select TOP 1 BusinessLine from   SysRightsManager.[dbo].UserInfo
                //where UserID=@UserID)");
                SqlParameter[] parameters = {
                                        new SqlParameter("@UserID", SqlDbType.Int)};
                parameters[0].Value = userId;

                object obj = SqlHelper.ExecuteScalar(ConnectionStrings_CRM, CommandType.Text, build.ToString(), parameters);
                if (obj != null)
                {
                    int rows = Int32.Parse(obj.ToString());
                    if (rows > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 删除cc客户该业务线的运营客服(userclass=7)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="memberIdList"></param>
        /// <returns></returns>
        public int DeleteCustMemberOfBL(int userId, string memberIdList)
        {
            StringBuilder build = new StringBuilder();
            build.Append(@"delete from CustUserMapping WHERE ID IN(
                           SELECT A.ID FROM CustUserMapping A left join
                           SysRightsManager.[dbo].UserInfo B
                           ON A.UserID = B.[UserID]
                           where CustID in  (" + Util.SqlFilterByInCondition(memberIdList) + ")" + @"
                           AND userclass=7

                           AND (BusinessLine & 1=1 )  AND (select TOP 1 BusinessLine from   SysRightsManager.[dbo].UserInfo
                           where UserID=@UserID) &1=1 )");  //只有新车一个客户分配一个运营客服

            //AND BusinessLine=(  
            //select TOP 1 BusinessLine from  SysRightsManager.[dbo].UserInfo
            //where UserID=@UserID))");
            SqlParameter[] parameters = {
                                        new SqlParameter("@UserID", SqlDbType.Int)};
            parameters[0].Value = userId;

            return SqlHelper.ExecuteNonQuery(ConnectionStrings_CRM, CommandType.Text, build.ToString(), parameters);
        }


        /// <summary>
        /// 是否是运营客服(userclass=7)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsYYKF(int userId)
        {
            try
            {
                StringBuilder build = new StringBuilder();
                build.Append(@"select COUNT(1) mapcount from 
                           SysRightsManager.[dbo].UserInfo
                           where userclass=7
                           and UserID=@UserID");
                SqlParameter[] parameters = {
                                        new SqlParameter("@UserID", SqlDbType.Int)};
                parameters[0].Value = userId;

                object obj = SqlHelper.ExecuteScalar(ConnectionStrings_CRM, CommandType.Text, build.ToString(), parameters);
                if (obj != null)
                {
                    int rows = Int32.Parse(obj.ToString());
                    if (rows > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}

