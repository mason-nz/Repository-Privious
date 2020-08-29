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
    /// 数据访问类ProjectSurveyMapping。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-02-20 10:39:28 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ProjectSurveyMapping : DataBase
    {
        #region Instance
        public static readonly ProjectSurveyMapping Instance = new ProjectSurveyMapping();
        #endregion

        #region const
        private const string P_PROJECTSURVEYMAPPING_SELECT = "p_ProjectSurveyMapping_Select";
        private const string P_PROJECTSURVEYMAPPING_INSERT = "p_ProjectSurveyMapping_Insert";
        private const string P_PROJECTSURVEYMAPPING_UPDATE = "p_ProjectSurveyMapping_Update";
        private const string P_PROJECTSURVEYMAPPING_DELETE = "p_ProjectSurveyMapping_Delete";
        #endregion

        #region Contructor
        protected ProjectSurveyMapping()
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
        public DataTable GetProjectSurveyMapping(QueryProjectSurveyMapping query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.ProjectID != Constant.INT_INVALID_VALUE)
            {
                where += " and ProjectSurveyMapping.ProjectID=" + query.ProjectID + "";
            }
            if (query.SIID != Constant.INT_INVALID_VALUE)
            {
                where += " and ProjectSurveyMapping.SIID=" + query.SIID + "";
            }
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " and ProjectSurveyMapping.Status=" + query.Status + "";
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTSURVEYMAPPING_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        /// <summary>
        /// 按照查询条件查询(调查项目问卷页面)
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetProjectSurveyMappingForList(QueryProjectSurveyMapping query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.ProjectID != Constant.INT_INVALID_VALUE)
            {
                where += " and psm.ProjectID=" + query.ProjectID;
            }
            if (query.SIID != Constant.INT_INVALID_VALUE)
            {
                where += " and psm.SIID=" + query.SIID;
            }
            if (query.SName != Constant.STRING_INVALID_VALUE)
            {
                where += " and SurveyInfo.Name like '%" + StringHelper.SqlFilter(query.SName) + "%'";
            }
            if (query.PName != Constant.STRING_INVALID_VALUE)
            {
                where += " and ProjectInfo.Name like '%" + StringHelper.SqlFilter(query.PName) + "%'";
            }
            if (query.SBGID != Constant.INT_INVALID_VALUE)
            {
                where += " and SurveyInfo.BGID=" + query.SBGID;
            }
            if (query.SSCID != Constant.INT_INVALID_VALUE)
            {
                where += " and SurveyInfo.SCID=" + query.SSCID;
            }

            if (query.PBGID != Constant.INT_INVALID_VALUE)
            {
                where += " and ProjectInfo.BGID=" + query.PBGID;
            }
            if (query.PSCID != Constant.INT_INVALID_VALUE)
            {
                where += " and ProjectInfo.PCatageID=" + query.PSCID;
            }
            #region 数据权限判断
            if (query.LoginID != Constant.INT_INVALID_VALUE)
            {
                where += Dal.UserGroupDataRigth.Instance.GetSqlRightstr("ProjectInfo", "BGID", "CreateUserID", query.LoginID);
            }
            #endregion

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ProjectSurveyMapping_SelectForList", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// 根据任务id取调查问卷
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public DataTable GetSurveyinfoByTaskID(string TaskID)
        {
            string strsql = "select a.projectid,c.PTID,b.*,a.begindate,a.endDate from ProjectTaskinfo c join dbo.ProjectSurveyMapping a on c.projectID=a.projectid join surveyinfo b on a.siid=b.siid where b.status!=0 and b.status!=-1 and ptid='" + StringHelper.SqlFilter(TaskID) + "' and a.status=0";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strsql, null);
            return ds.Tables[0];
        }

        /// <summary>
        /// 根据任务id取调查问卷
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public DataTable GetSurveyinfoByOtherTaskID(string TaskID)
        {
            string strsql = "select a.projectid,c.PTID,b.*,a.begindate,a.endDate from OtherTaskInfo c join dbo.ProjectSurveyMapping a on c.projectID=a.projectid join surveyinfo b on a.siid=b.siid where b.status!=0 and b.status!=-1 and ptid='" + StringHelper.SqlFilter(TaskID) + "' and a.status=0";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strsql, null);
            return ds.Tables[0];
        }

        /// <summary>
        /// 根据项目取调查问卷
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public DataTable GetSurveyinfoByCustID(string CustID)
        {
            string strsql = "select max(a.projectid) ProjectID,a.siid,b.name,max(begindate) begindate,max(enddate) enddate from ProjectSurveyMapping a left join SurveyInfo b on a.siid=b.siid where projectid in (select projectid from ProjectDataSoure where source=3 and Relationid='" + StringHelper.SqlFilter(CustID) + "') and a.status=0 group by a.siid,b.name";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strsql, null);
            return ds.Tables[0];
        }


        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ProjectSurveyMapping GetProjectSurveyMapping(long ProjectID, int SIID)
        {
            QueryProjectSurveyMapping query = new QueryProjectSurveyMapping();
            query.ProjectID = ProjectID;
            query.SIID = SIID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectSurveyMapping(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleProjectSurveyMapping(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.ProjectSurveyMapping LoadSingleProjectSurveyMapping(DataRow row)
        {
            Entities.ProjectSurveyMapping model = new Entities.ProjectSurveyMapping();

            if (row["ProjectID"].ToString() != "")
            {
                model.ProjectID = long.Parse(row["ProjectID"].ToString());
            }
            if (row["SIID"].ToString() != "")
            {
                model.SIID = int.Parse(row["SIID"].ToString());
            }
            model.BeginDate = row["BeginDate"].ToString();
            model.EndDate = row["EndDate"].ToString();
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public void Insert(Entities.ProjectSurveyMapping model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ProjectID", SqlDbType.Int,8),
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@BeginDate", SqlDbType.VarChar,20),
					new SqlParameter("@EndDate", SqlDbType.VarChar,20),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.ProjectID;
            parameters[1].Value = model.SIID;
            parameters[2].Value = model.BeginDate;
            parameters[3].Value = model.EndDate;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTSURVEYMAPPING_INSERT, parameters);
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public void Insert(SqlTransaction sqltran, Entities.ProjectSurveyMapping model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ProjectID", SqlDbType.Int,8),
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@BeginDate", SqlDbType.VarChar,20),
					new SqlParameter("@EndDate", SqlDbType.VarChar,20),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.ProjectID;
            parameters[1].Value = model.SIID;
            parameters[2].Value = model.BeginDate;
            parameters[3].Value = model.EndDate;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_PROJECTSURVEYMAPPING_INSERT, parameters);
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.ProjectSurveyMapping model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ProjectID", SqlDbType.Int,8),
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@BeginDate", SqlDbType.VarChar,20),
					new SqlParameter("@EndDate", SqlDbType.VarChar,20),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.ProjectID;
            parameters[1].Value = model.SIID;
            parameters[2].Value = model.BeginDate;
            parameters[3].Value = model.EndDate;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTSURVEYMAPPING_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ProjectSurveyMapping model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ProjectID", SqlDbType.Int,8),
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@BeginDate", SqlDbType.VarChar,20),
					new SqlParameter("@EndDate", SqlDbType.VarChar,20),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.ProjectID;
            parameters[1].Value = model.SIID;
            parameters[2].Value = model.BeginDate;
            parameters[3].Value = model.EndDate;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_PROJECTSURVEYMAPPING_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long ProjectID, int SIID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ProjectID", SqlDbType.BigInt),
					new SqlParameter("@SIID", SqlDbType.Int,4)};
            parameters[0].Value = ProjectID;
            parameters[1].Value = SIID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTSURVEYMAPPING_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long ProjectID, int SIID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ProjectID", SqlDbType.BigInt),
					new SqlParameter("@SIID", SqlDbType.Int,4)};
            parameters[0].Value = ProjectID;
            parameters[1].Value = SIID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_PROJECTSURVEYMAPPING_DELETE, parameters);
        }
        #endregion

    }
}

