using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.Utils;
using System.Data.SqlClient;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class ProjectTask_AuditContrastInfo : DataBase
    {
        #region Instance
        public static readonly ProjectTask_AuditContrastInfo Instance = new ProjectTask_AuditContrastInfo();
        #endregion

        #region const
        private const string P_PROJECTTASL_AUDITCONTRASTINFO_SELECT = "p_ProjectTask_AuditContrastInfo_Select";
        private const string P_PROJECTTASL_AUDITCONTRASTINFO_SELECTFORCHANGE = "p_ProjectTask_AuditContrastInfo_SelectForChange";
        private const string P_PROJECTTASL_AUDITCONTRASTINFO_INSERT = "p_ProjectTask_AuditContrastInfo_Insert";
        private const string P_PROJECTTASL_AUDITCONTRASTINFO_CSTSELECT = "p_ProjectTask_AuditContrastInfo_CSTSelect";
        private const string P_PROJECTTASL_AUDITCONTRASTINFO_UPDATE = "p_ProjectTask_AuditContrastInfo_Update";
        private const string P_PROJECTTASL_AUDITCONTRASTINFO_BATCHUPDATE_EXPORTSTATUS = "p_ProjectTask_AuditContrastInfo_BatchUpdate_ExportStatus";
        private const string P_PROJECTTASL_AUDITCONTRASTINFO_SELECTBYCONTION = "p_ProjectTask_AuditContrastInfo_SelectByContion";
        //private const string P_ProjectTask_AuditContrastInfo_DELETE = "p_ProjectTask_AuditContrastInfo_Delete";
        #endregion

        #region Contructor
        protected ProjectTask_AuditContrastInfo()
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
        public DataTable GetProjectTask_AuditContrastInfo(QueryProjectTask_AuditContrastInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            //modify by qizhiqiang 2012-4-25 把车商通包含进来
            if (query.CustIDORMemberID != Constant.STRING_INVALID_VALUE)
            {
                //where += " And ((ac.CustID='" + StringHelper.SqlFilter(query.CustIDORMemberID) + "' And ac.DMSMemberID IS NULL) OR (ac.DMSMemberID IN (SELECT CONVERT(VARCHAR(50),ID) FROM DMSMember WHERE MemberCode='" + StringHelper.SqlFilter(query.CustIDORMemberID) + "')))";
                where += " And ((ac.CustID='" + StringHelper.SqlFilter(query.CustIDORMemberID) + "' And ac.DMSMemberID IS NULL) or (ac.DMSMemberID IN (SELECT CONVERT(VARCHAR(50),ID) FROM Crm2009.dbo.DMSMember WHERE MemberCode='" + StringHelper.SqlFilter(query.CustIDORMemberID) + "')) OR (ac.DMSMemberID IN (SELECT CONVERT(VARCHAR(50),CSTRecID) FROM Crm2009.dbo.cstMember WHERE cstmemberid='" + StringHelper.SqlFilter(query.CustIDORMemberID) + "')))";
            }
            if (query.CustNameORMemberName != Constant.STRING_INVALID_VALUE)
            {
                //where += " And ((ac.CustID IN (SELECT CustID FROM CustInfo WHERE CustName='" + StringHelper.SqlFilter(query.CustNameORMemberName) + "') And ac.DMSMemberID IS NULL) OR (ac.DMSMemberID IN (SELECT CONVERT(VARCHAR(50),ID) FROM DMSMember WHERE Name='" + StringHelper.SqlFilter(query.CustNameORMemberName) + "')))";
                where += " And ((ac.CustID IN (SELECT CustID FROM Crm2009.dbo.CustInfo WHERE CustName='" + StringHelper.SqlFilter(query.CustNameORMemberName) + "') And ac.DMSMemberID IS NULL) OR (ac.DMSMemberID IN (SELECT CONVERT(VARCHAR(50),ID) FROM Crm2009.dbo.DMSMember WHERE Name='" + StringHelper.SqlFilter(query.CustNameORMemberName) + "')) or (ac.DMSMemberID IN (SELECT CONVERT(VARCHAR(50),CSTRecID) FROM Crm2009.dbo.cstMember WHERE fullName='" + StringHelper.SqlFilter(query.CustNameORMemberName) + "')))";

            }
            if (query.PTID != Constant.INT_INVALID_VALUE)
            {
                where += " And ac.PTID='" + query.PTID+"'";
            }
            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " And ac.RecID =" + query.RecID;
            }
            if (query.CreateStartDate != Constant.STRING_INVALID_VALUE)
            {
                where += " And ac.CreateTime >='" + StringHelper.SqlFilter(query.CreateStartDate) + " 0:0:0'";
            }
            if (query.CreateEndDate != Constant.STRING_INVALID_VALUE)
            {
                where += " And ac.CreateTime <='" + StringHelper.SqlFilter(query.CreateEndDate) + " 23:59:59'";
            }
            if (query.ExportStatus != Constant.INT_INVALID_VALUE)
            {
                where += " And ac.ExportStatus=" + query.ExportStatus;
            }
            if (query.ContrastType != Constant.INT_INVALID_VALUE)
            {
                where += " And ac.ContrastType=" + query.ContrastType;
            }
            if (query.DisposeStatus != Constant.INT_INVALID_VALUE)
            {
                where += " And ac.DisposeStatus=" + query.DisposeStatus;
            }
            if (query.IsNullDisposeStatus != null)
            {
                if (query.IsNullDisposeStatus.Value)
                {
                    where += " And ac.DisposeStatus IS NULL";
                }
                else
                {
                    where += " And ac.DisposeStatus IS Not NULL";
                }
                where += " And ac.DisposeStatus IS NULL";
            }
            if (query.SeatTrueName != Constant.STRING_INVALID_VALUE)
            {
                where += @" And ac.PTID IN (
                          SELECT PTID FROM dbo.ProjectTask_Employee WHERE Status=0 AND UserID IN (SELECT UserID FROM v_userinfo WHERE TrueName Like '%" + StringHelper.SqlFilter(query.SeatTrueName) + "%')) ";
            }

            //add by qizq  添加会员地区

            if (query.MemberProvinceID != Constant.STRING_INVALID_VALUE)
            {
                where += " and (dms.ProvinceID='" + SqlFilter(query.MemberProvinceID) + "' OR cst.ProvinceID='" + SqlFilter(query.MemberProvinceID) + "') ";
            }
            if (query.MemberCityID != Constant.STRING_INVALID_VALUE)
            {
                where += " and (dms.CityID='" + SqlFilter(query.MemberCityID) + "' OR cst.CityID='" + SqlFilter(query.MemberCityID) + "') ";
            }
            if (query.MemberCountyID != Constant.STRING_INVALID_VALUE)
            {
                where += " and (dms.CountyID='" + SqlFilter(query.MemberCountyID) + "' OR cst.CountyID='" + SqlFilter(query.MemberCountyID) + "') ";
            }

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASL_AUDITCONTRASTINFO_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        //add by qizhiqiang 2012-4-27
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetProjectTask_AuditContrastInfoForChange(QueryProjectTask_AuditContrastInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;
            //modify by qizhiqiang 2012-4-25 把车商通包含进来
            if (query.CustIDORMemberID != Constant.STRING_INVALID_VALUE)
            {
                //where += " And ((ac.CustID='" + StringHelper.SqlFilter(query.CustIDORMemberID) + "' And ac.DMSMemberID IS NULL) OR (ac.DMSMemberID IN (SELECT CONVERT(VARCHAR(50),ID) FROM DMSMember WHERE MemberCode='" + StringHelper.SqlFilter(query.CustIDORMemberID) + "')))";
                where += " And ((ac.CustID='" + StringHelper.SqlFilter(query.CustIDORMemberID) + "' And ac.DMSMemberID IS NULL) or (ac.DMSMemberID IN (SELECT CONVERT(VARCHAR(50),ID) FROM Crm2009.dbo.DMSMember WHERE MemberCode='" + StringHelper.SqlFilter(query.CustIDORMemberID) + "')) OR (ac.DMSMemberID IN (SELECT CONVERT(VARCHAR(50),CSTRecID) FROM Crm2009.dbo.cstMember WHERE cstmemberid='" + StringHelper.SqlFilter(query.CustIDORMemberID) + "')))";
            }
            if (query.CustNameORMemberName != Constant.STRING_INVALID_VALUE)
            {
                //where += " And ((ac.CustID IN (SELECT CustID FROM CustInfo WHERE CustName='" + StringHelper.SqlFilter(query.CustNameORMemberName) + "') And ac.DMSMemberID IS NULL) OR (ac.DMSMemberID IN (SELECT CONVERT(VARCHAR(50),ID) FROM DMSMember WHERE Name='" + StringHelper.SqlFilter(query.CustNameORMemberName) + "')))";
                where += " And ((ac.CustID IN (SELECT CustID FROM Crm2009.dbo.CustInfo WHERE CustName='" + StringHelper.SqlFilter(query.CustNameORMemberName) + "') And ac.DMSMemberID IS NULL) OR (ac.DMSMemberID IN (SELECT CONVERT(VARCHAR(50),ID) FROM Crm2009.dbo.DMSMember WHERE Name='" + StringHelper.SqlFilter(query.CustNameORMemberName) + "')) or (ac.DMSMemberID IN (SELECT CONVERT(VARCHAR(50),CSTRecID) FROM Crm2009.dbo.cstMember WHERE fullName='" + StringHelper.SqlFilter(query.CustNameORMemberName) + "')))";

            }
            if (query.PTID != Constant.INT_INVALID_VALUE)
            {
                where += " And ac.PTID='" + query.PTID+"'";
            }
            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " And ac.RecID =" + query.RecID;
            }
            if (query.CreateStartDate != Constant.STRING_INVALID_VALUE)
            {
                where += " And ac.CreateTime >='" + StringHelper.SqlFilter(query.CreateStartDate) + " 0:0:0'";
            }
            if (query.CreateEndDate != Constant.STRING_INVALID_VALUE)
            {
                where += " And ac.CreateTime <='" + StringHelper.SqlFilter(query.CreateEndDate) + " 23:59:59'";
            }
            if (query.ExportStatus != Constant.INT_INVALID_VALUE)
            {
                where += " And ac.ExportStatus=" + query.ExportStatus;
            }
            if (query.ContrastType != Constant.INT_INVALID_VALUE)
            {
                where += " And ac.ContrastType=" + query.ContrastType;
            }
            if (query.DisposeStatus != Constant.INT_INVALID_VALUE)
            {
                where += " And ac.DisposeStatus=" + query.DisposeStatus;
            }
            if (query.IsNullDisposeStatus != null)
            {
                if (query.IsNullDisposeStatus.Value)
                {
                    where += " And ac.DisposeStatus IS NULL";
                }
                else
                {
                    where += " And ac.DisposeStatus IS Not NULL";
                }
                where += " And ac.DisposeStatus IS NULL";
            }
            if (query.SeatTrueName != Constant.STRING_INVALID_VALUE)
            {
                where += @" And ac.PTID IN (
                          SELECT PTID FROM dbo.ProjectTask_Employee WHERE Status=0 AND UserID IN (SELECT UserID FROM Crm2009.dbo.v_userinfo WHERE TrueName Like '%" + StringHelper.SqlFilter(query.SeatTrueName) + "%')) ";
            }
            //modify by qizhiqiang 2012-4-26去掉经营范围
            //if (query.CarType != Constant.STRING_EMPTY_VALUE)
            //{
            //    where += " And CustInfo.CarType in (" + query.CarType + ")";
            //}
            if (query.TaskBatch != -1 && query.TaskBatch != Constant.INT_INVALID_VALUE)   //-1代表全部轮次
            {
                where += " And ProjectTaskInfo.Batch=" + query.TaskBatch;
            }

            //where += "  and (Case when ac.DMSMemberID is not null then (((ac.DMSMemberID IN (SELECT CONVERT(VARCHAR(50),ID) FROM DMSMember where MemberCode<>'' and MemberCode<>'0' and MemberCode<>'-2')) OR (ac.DMSMemberID IN (SELECT CONVERT(VARCHAR(50),CSTRecID) FROM cstMember where  CONVERT(VARCHAR(50),cstMemberid)<>'' and CONVERT(VARCHAR(50),cstMemberid)<>'0' and CONVERT(VARCHAR(50),cstMemberid)<>'-2'))))";

            where += " and ((ac.DMSMemberID is null) or (exists(select membercode from Crm2009.dbo.DMSMember where CONVERT(VARCHAR(50),ID)=ac.DMSMemberID and  membercode<>'' and membercode<>'0' and membercode<>'-2')) or (exists(select CONVERT(VARCHAR(50),cstmemberid) from Crm2009.dbo.cstMember where CSTRecID=ac.DMSMemberID and  CONVERT(VARCHAR(50),cstmemberid)<>'' and CONVERT(VARCHAR(50),cstmemberid)<>'0' and CONVERT(VARCHAR(50),cstmemberid)<>'-2')))";

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASL_AUDITCONTRASTINFO_SELECT, parameters);
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
        public DataTable GetCC_AuditContrastCSTInfo(QueryProjectTask_AuditContrastInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.CustIDORMemberID != Constant.STRING_INVALID_VALUE)
            {
                where += " And ((ac.CustID='" + StringHelper.SqlFilter(query.CustIDORMemberID) + "' And ac.DMSMemberID IS NULL) OR (ac.DMSMemberID IN (SELECT CONVERT(VARCHAR(50),CSTRecID) FROM CstMember WHERE VendorCode='" + StringHelper.SqlFilter(query.CustIDORMemberID) + "')))";
            }
            if (query.CustNameORMemberName != Constant.STRING_INVALID_VALUE)
            {
                where += " And ((ac.CustID IN (SELECT CustID FROM Crm2009.dbo.CustInfo WHERE CustName='" + StringHelper.SqlFilter(query.CustNameORMemberName) + "') And ac.DMSMemberID IS NULL) OR (ac.DMSMemberID IN (SELECT CONVERT(VARCHAR(50),CSTRecID) FROM Crm2009.dbo.CstMember WHERE FullName='" + StringHelper.SqlFilter(query.CustNameORMemberName) + "')))";
            }
            if (query.PTID != Constant.INT_INVALID_VALUE)
            {
                where += " And ac.PTID='" + query.PTID+"'";
            }
            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " And ac.RecID =" + query.RecID;
            }
            if (query.CreateStartDate != Constant.STRING_INVALID_VALUE)
            {
                where += " And ac.CreateTime >='" + StringHelper.SqlFilter(query.CreateStartDate) + " 0:0:0'";
            }
            if (query.CreateEndDate != Constant.STRING_INVALID_VALUE)
            {
                where += " And ac.CreateTime <='" + StringHelper.SqlFilter(query.CreateEndDate) + " 23:59:59'";
            }
            if (query.ExportStatus != Constant.INT_INVALID_VALUE)
            {
                where += " And ac.ExportStatus=" + query.ExportStatus;
            }
            if (query.ContrastType != Constant.INT_INVALID_VALUE)
            {
                where += " And ac.ContrastType=" + query.ContrastType;
            }
            if (query.DisposeStatus != Constant.INT_INVALID_VALUE)
            {
                where += " And ac.DisposeStatus=" + query.DisposeStatus;
            }
            if (query.IsNullDisposeStatus != null)
            {
                if (query.IsNullDisposeStatus.Value)
                {
                    where += " And ac.DisposeStatus IS NULL";
                }
                else
                {
                    where += " And ac.DisposeStatus IS Not NULL";
                }
                where += " And ac.DisposeStatus IS NULL";
            }
            if (query.SeatTrueName != Constant.STRING_INVALID_VALUE)
            {
                where += @" And ac.PTID IN (
                          SELECT PTID FROM dbo.ProjectTask_Employee WHERE Status=0 AND UserID IN (SELECT UserID FROM Crm2009.dbo.v_userinfo WHERE TrueName Like '%" + StringHelper.SqlFilter(query.SeatTrueName) + "%')) ";
            }

            if (query.CarType != Constant.STRING_EMPTY_VALUE)
            {
                where += " And Crm2009.dbo.CustInfo.CarType in (" + Dal.Util.SqlFilterByInCondition(query.CarType) + ")";
            }
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASL_AUDITCONTRASTINFO_CSTSELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }


        /// <summary>
        /// 根据会员名称和变更类型获取变更信息
        /// </summary>
        /// <param name="dmsMemberId"></param>
        /// <param name="contrastType"></param>
        /// <returns></returns>
        public DataTable GetProjectTask_AuditContrastInfo(string dmsMemberId, int contrastType, string contrastField)
        {
            SqlParameter[] parameters ={
                                           new SqlParameter("@DMSMemberID",dmsMemberId),
                                           new SqlParameter("@ContrastType",contrastType),
                                           new SqlParameter("@ContrastFiled",contrastField)
                                      };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASL_AUDITCONTRASTINFO_SELECTBYCONTION, parameters);
            return ds.Tables[0];
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ProjectTask_AuditContrastInfo GetProjectTask_AuditContrastInfo(int RecID)
        {
            QueryProjectTask_AuditContrastInfo query = new QueryProjectTask_AuditContrastInfo();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectTask_AuditContrastInfo(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleProjectTask_AuditContrastInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.ProjectTask_AuditContrastInfo LoadSingleProjectTask_AuditContrastInfo(DataRow row)
        {
            Entities.ProjectTask_AuditContrastInfo model = new Entities.ProjectTask_AuditContrastInfo();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            if (row["PTID"].ToString() != "")
            {
                model.PTID = row["PTID"].ToString();
            }
            model.CustID = row["CustID"].ToString();
            model.DMSMemberID = row["DMSMemberID"].ToString();
            model.ContrastInfoInside = row["ContrastInfoInside"].ToString();
            model.ContrastInfo = row["ContrastInfo"].ToString();
            if (row["ExportStatus"].ToString() != "")
            {
                model.ExportStatus = int.Parse(row["ExportStatus"].ToString());
            }
            if (row["DisposeStatus"].ToString() != "")
            {
                model.DisposeStatus = int.Parse(row["DisposeStatus"].ToString());
            }
            if (row["ContrastType"].ToString() != "")
            {
                model.ContrastType = int.Parse(row["ContrastType"].ToString());
            }
            model.Remark = row["Remark"].ToString();
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            if (row["DisposeTime"].ToString() != "")
            {
                model.DisposeTime = DateTime.Parse(row["DisposeTime"].ToString());
            }
            return model;
        }

        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.ProjectTask_AuditContrastInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@PTID", SqlDbType.VarChar),
					new SqlParameter("@CustID", SqlDbType.VarChar,50),
					new SqlParameter("@DMSMemberID", SqlDbType.VarChar,50),
					new SqlParameter("@ContrastInfoInside", SqlDbType.VarChar,8000),
					new SqlParameter("@ContrastInfo", SqlDbType.VarChar,8000),
					new SqlParameter("@ExportStatus", SqlDbType.Int,4),
					new SqlParameter("@ContrastType", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                    new SqlParameter("@DisposeStatus", SqlDbType.Int,4),
                    new SqlParameter("@Remark", SqlDbType.VarChar,2000),
                    new SqlParameter("@DisposeTime", SqlDbType.DateTime)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.PTID;
            parameters[2].Value = model.CustID;
            parameters[3].Value = model.DMSMemberID;
            parameters[4].Value = model.ContrastInfoInside;
            parameters[5].Value = model.ContrastInfo;
            parameters[6].Value = model.ExportStatus;
            parameters[7].Value = model.ContrastType;
            parameters[8].Value = model.CreateTime;
            parameters[9].Value = model.CreateUserID;
            parameters[10].Value = model.DisposeStatus;
            parameters[11].Value = model.Remark;
            parameters[12].Value = model.DisposeTime;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASL_AUDITCONTRASTINFO_INSERT, parameters);
            return (int)parameters[0].Value;
        }

        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.ProjectTask_AuditContrastInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@PTID", SqlDbType.VarChar),
					new SqlParameter("@CustID", SqlDbType.VarChar,50),
					new SqlParameter("@DMSMemberID", SqlDbType.VarChar,50),
					new SqlParameter("@ContrastInfoInside", SqlDbType.VarChar,8000),
					new SqlParameter("@ContrastInfo", SqlDbType.VarChar,8000),
					new SqlParameter("@ExportStatus", SqlDbType.Int,4),
					new SqlParameter("@ContrastType", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                    new SqlParameter("@DisposeStatus", SqlDbType.Int,4),
                    new SqlParameter("@Remark", SqlDbType.VarChar,2000),
                    new SqlParameter("@DisposeTime", SqlDbType.DateTime)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.PTID;
            parameters[2].Value = model.CustID;
            parameters[3].Value = model.DMSMemberID;
            parameters[4].Value = model.ContrastInfoInside;
            parameters[5].Value = model.ContrastInfo;
            parameters[6].Value = model.ExportStatus;
            parameters[7].Value = model.ContrastType;
            parameters[8].Value = model.CreateTime;
            parameters[9].Value = model.CreateUserID;
            parameters[10].Value = model.DisposeStatus;
            parameters[11].Value = model.Remark;
            parameters[12].Value = model.DisposeTime;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASL_AUDITCONTRASTINFO_UPDATE, parameters);
        }
        /// <summary>
        /// 根据查询条件，批量更新导出状态
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="exportStatus">导出状态（0-未导出，1-已导出）</param>
        public int BatchUpdateExportStatusByWhere(QueryProjectTask_AuditContrastInfo query, int exportStatus)
        {
            string where = string.Empty;

            if (query.CreateStartDate != Constant.STRING_INVALID_VALUE)
            {
                where += " And CreateTime >='" + StringHelper.SqlFilter(query.CreateStartDate) + " 0:0:0'";
            }
            if (query.CreateEndDate != Constant.STRING_INVALID_VALUE)
            {
                where += " And CreateTime <='" + StringHelper.SqlFilter(query.CreateEndDate) + " 23:59:59'";
            }
            if (query.ExportStatus != Constant.INT_INVALID_VALUE)
            {
                where += " And ExportStatus=" + query.ExportStatus;
            }
            if (query.ContrastType != Constant.INT_INVALID_VALUE)
            {
                where += " And ContrastType=" + query.ContrastType;
            }
            SqlParameter[] parameters = {
					new SqlParameter("@Where",  SqlDbType.VarChar, 8000),
					new SqlParameter("@ExportStatus", SqlDbType.Int,4)};
            parameters[0].Value = where;
            parameters[1].Value = exportStatus;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASL_AUDITCONTRASTINFO_BATCHUPDATE_EXPORTSTATUS, parameters);
        }


        #endregion

        #region Delete
        /// <summary>
        /// 删除该记录
        /// </summary>
        /// <param name="tid">任务ID</param>
        /// <param name="dd">对照类型ID</param>
        /// <returns></returns>
        public int DeleteByTIDAndContrastType(int tid, int contrastTypeID)
        {
            string sql = string.Format("DELETE FROM ProjectTask_AuditContrastInfo WHERE PTID={0} And ContrastType={1}",
                                    tid, contrastTypeID);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }
        #endregion

        //add by qizhiqiang 2012-5-16 根据membercode判断是否有排期
        /// <summary>
        /// 根据memberCode判断会员是否有排期
        /// </summary>
        /// <param name="membercode"></param>
        /// <returns></returns>
        public bool HavePeiQiByDMSMemberCode(string membercode)
        {
            bool flag = false;
            string sqlstr = "select * from Crm2009.dbo.Dmsmember where membercode='" + StringHelper.SqlFilter(membercode) + "' and  cooperated=1";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, null);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = null;
                dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    flag = true;
                }
            }
            return flag;
        }

        /// <summary>
        /// 取所有有排期车易通变更记录
        /// </summary>
        /// <returns></returns>
        public DataTable GetnineTypeProjectTask_AuditContrastInfo()
        {
            string sqlstr = "select * from ProjectTask_AuditContrastInfo where contrasttype=9";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, null);
            return ds.Tables[0];
        }
        /// <summary>
        /// 取无排期已开通车易通变更记录
        /// </summary>
        /// <returns></returns>
        public DataTable GetNoPeiQiProjectTask_AuditContrastInfo()
        {
            string sqlstr = "select a.* from ProjectTask_AuditContrastInfo a join Crm2009.dbo.Dmsmember b on a.Dmsmemberid=b.id where contrasttype=9 and (b.cooperated=0 or b.cooperated IS NULL) and b.status=0";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, null);
            return ds.Tables[0];
        }
    }
}
