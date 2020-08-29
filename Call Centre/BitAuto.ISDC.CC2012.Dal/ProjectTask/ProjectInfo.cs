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
    /// 数据访问类ProjectInfo。
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
    public class ProjectInfo : DataBase
    {
        public static readonly ProjectInfo Instance = new ProjectInfo();
        private const string P_PROJECTINFO_SELECT = "p_ProjectInfo_Select";
        private const string P_PROJECTINFO_INSERT = "p_ProjectInfo_Insert";
        private const string P_PROJECTINFO_UPDATE = "p_ProjectInfo_Update";
        private const string P_PROJECTINFO_DELETE = "p_ProjectInfo_Delete";

        protected ProjectInfo() { }

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
        public DataTable GetProjectInfo(QueryProjectInfo query, string order, int currentPage, int pageSize, out int totalCount, int? userID)
        {
            string where = GetWhereStr(query);
            if (userID.HasValue)
                where += UserGroupDataRigth.Instance.GetSqlRightstr("ProjectInfo", "BGID", "CreateUserID", userID.Value);

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
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTINFO_SELECT, parameters);
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
        public DataTable GetProjectInfo(QueryProjectInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return GetProjectInfo(query, order, currentPage, pageSize, out totalCount, null);
        }
        private static string GetWhereStr(QueryProjectInfo query)
        {
            string where = string.Empty;

            if (query.ProjectID != Constant.INT_INVALID_VALUE)
            {
                where += " AND ProjectInfo.ProjectID =" + query.ProjectID + "";
            }

            if (query.Name != Constant.STRING_INVALID_VALUE)
            {
                where += " AND ProjectInfo.Name like '%" + StringHelper.SqlFilter(query.Name) + "%'";
            }

            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " AND ProjectInfo.BGID=" + query.BGID;
            }

            if (query.PCatageID != Constant.INT_INVALID_VALUE)
            {
                where += " AND ProjectInfo.PCatageID=" + query.PCatageID;
            }

            if (query.Source != Constant.INT_INVALID_VALUE)
            {
                where += " AND ProjectInfo.Source=" + query.Source;
            }

            if (query.CreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND ProjectInfo.CreateUserID=" + query.CreateUserID;
            }

            if (query.BeginTime != Constant.STRING_INVALID_VALUE)
            {
                where += " And ProjectInfo.CreateTime>='" + DateTime.Parse(StringHelper.SqlFilter(query.BeginTime)).ToShortDateString() + "'";
            }
            if (query.EndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " And ProjectInfo.CreateTime<='" + DateTime.Parse(StringHelper.SqlFilter(query.EndTime)).ToShortDateString() + " 23:59:59'";
            }

            if (query.Statuss != Constant.STRING_INVALID_VALUE)
            {
                where += " AND ProjectInfo.Status in (" + Dal.Util.SqlFilterByInCondition(query.Statuss) + ")";
            }

            if (query.DemandID != Constant.STRING_INVALID_VALUE && query.DemandID != Constant.STRING_EMPTY_VALUE)
            {
                where += "AND ProjectInfo.DemandID='" + StringHelper.SqlFilter(query.DemandID) + "'";
            }

            if (query.Batch.HasValue)
            {
                where += "AND ProjectInfo.Batch='" + query.Batch.Value + "'";
            }
            if (!string.IsNullOrEmpty(query.ISAutoCall))
            {
                if (query.ISAutoCall == "1")
                {
                    //是
                    where += " AND ISNULL(b.ProjectID,0)>0";
                }
                else if (query.ISAutoCall == "0")
                {
                    //否
                    where += " AND ISNULL(b.ProjectID,0)=0";
                }
            }
            if (!string.IsNullOrEmpty(query.ACStatus))
            {
                where += " AND ISNULL(b.ProjectID,0)>0";
                where += " AND ISNULL(b.ACStatus,0) IN (" + Dal.Util.SqlFilterByInCondition(query.ACStatus) + ")";
            }
            if (!string.IsNullOrEmpty(query.wherePlus))
            {
                where += query.wherePlus;
            }
            return where;
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ProjectInfo GetProjectInfo(long ProjectID)
        {
            QueryProjectInfo query = new QueryProjectInfo();
            query.ProjectID = ProjectID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectInfo(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleProjectInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        public Entities.ProjectInfo LoadSingleProjectInfo(DataRow row)
        {
            Entities.ProjectInfo model = new Entities.ProjectInfo();

            if (row["ProjectID"].ToString() != "")
            {
                model.ProjectID = long.Parse(row["ProjectID"].ToString());
            }
            if (row["BGID"].ToString() != "")
            {
                model.BGID = int.Parse(row["BGID"].ToString());
            }
            if (row["SCID"].ToString() != "")
            {
                model.SCID = int.Parse(row["SCID"].ToString());
            }
            model.Name = row["Name"].ToString();
            model.Notes = row["Notes"].ToString();
            if (row["Source"].ToString() != "")
            {
                model.Source = int.Parse(row["Source"].ToString());
            }
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
            if (row["PCatageID"].ToString() != "")
            {
                model.PCatageID = int.Parse(row["PCatageID"].ToString());
            }
            if (row["IsOldData"].ToString() != "")
            {
                model.IsOldData = int.Parse(row["IsOldData"].ToString());
            }

            model.TTCode = row["TTCode"].ToString();
            model.DemandID = row["DemandID"].ToString();
            model.Batch = row["Batch"].ToString() == "" ? null : (int?)int.Parse(row["Batch"].ToString());
            model.ExpectedNum = row["ExpectedNum"].ToString() == "" ? null : (int?)int.Parse(row["ExpectedNum"].ToString());

            if (row["IsBlacklistCheck"].ToString() != "")
            {
                model.IsBlacklistCheck = int.Parse(row["IsBlacklistCheck"].ToString());
            }
            if (row["BlacklistCheckType"].ToString() != "")
            {
                model.BlacklistCheckType = int.Parse(row["BlacklistCheckType"].ToString());
            }
            return model;
        }
        #endregion


        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ProjectInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ProjectID", SqlDbType.Int,8),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,200),
					new SqlParameter("@Notes", SqlDbType.NVarChar,1000),
					new SqlParameter("@Source", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                    new SqlParameter("@PCatageID", SqlDbType.Int,4) ,
                    new SqlParameter("@TTCode",SqlDbType.NVarChar,20),         
                    new SqlParameter("@DemandID",SqlDbType.VarChar,20),
                    new SqlParameter("@Batch", SqlDbType.Int,4),
                    new SqlParameter("@ExpectedNum", SqlDbType.Int,4),
                    new SqlParameter("@IsBlacklistCheck", SqlDbType.Int,4),
                    new SqlParameter("@BlacklistCheckType", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.BGID;
            parameters[2].Value = model.SCID;
            parameters[3].Value = model.Name;
            parameters[4].Value = model.Notes;
            parameters[5].Value = model.Source;
            parameters[6].Value = model.Status;
            parameters[7].Value = model.CreateTime;
            parameters[8].Value = model.CreateUserID;
            parameters[9].Value = model.PCatageID;
            parameters[10].Value = model.TTCode;
            parameters[11].Value = model.DemandID;
            parameters[12].Value = model.Batch;
            parameters[13].Value = model.ExpectedNum;
            parameters[14].Value = model.IsBlacklistCheck;
            parameters[15].Value = model.BlacklistCheckType;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_PROJECTINFO_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.ProjectInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ProjectID", SqlDbType.Int,8),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,200),
					new SqlParameter("@Notes", SqlDbType.NVarChar,1000),
					new SqlParameter("@Source", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                    new SqlParameter("@PCatageID", SqlDbType.Int,4)  ,
                    new SqlParameter("@TTCode",SqlDbType.NVarChar,20),         
                    new SqlParameter("@DemandID",SqlDbType.VarChar,20),
                    new SqlParameter("@Batch", SqlDbType.Int,4),
                    new SqlParameter("@ExpectedNum", SqlDbType.Int,4),
                    new SqlParameter("@IsBlacklistCheck", SqlDbType.Int,4),
                    new SqlParameter("@BlacklistCheckType", SqlDbType.Int,4)};
            parameters[0].Value = model.ProjectID;
            parameters[1].Value = model.BGID;
            parameters[2].Value = model.SCID;
            parameters[3].Value = model.Name;
            parameters[4].Value = model.Notes;
            parameters[5].Value = model.Source;
            parameters[6].Value = model.Status;
            parameters[7].Value = model.CreateTime;
            parameters[8].Value = model.CreateUserID;
            parameters[9].Value = model.PCatageID;
            parameters[10].Value = model.TTCode;
            parameters[11].Value = model.DemandID;
            parameters[12].Value = model.Batch;
            parameters[13].Value = model.ExpectedNum;
            parameters[14].Value = model.IsBlacklistCheck;
            parameters[15].Value = model.BlacklistCheckType;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTINFO_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ProjectInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ProjectID", SqlDbType.Int,8),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,200),
					new SqlParameter("@Notes", SqlDbType.NVarChar,1000),
					new SqlParameter("@Source", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                    new SqlParameter("@PCatageID", SqlDbType.Int,4)   ,
                    new SqlParameter("@TTCode",SqlDbType.NVarChar,20),         
                    new SqlParameter("@DemandID",SqlDbType.VarChar,20),
                    new SqlParameter("@Batch", SqlDbType.Int,4),
                    new SqlParameter("@ExpectedNum", SqlDbType.Int,4),
                    new SqlParameter("@IsBlacklistCheck", SqlDbType.Int,4),
                    new SqlParameter("@BlacklistCheckType", SqlDbType.Int,4)};
            parameters[0].Value = model.ProjectID;
            parameters[1].Value = model.BGID;
            parameters[2].Value = model.SCID;
            parameters[3].Value = model.Name;
            parameters[4].Value = model.Notes;
            parameters[5].Value = model.Source;
            parameters[6].Value = model.Status;
            parameters[7].Value = model.CreateTime;
            parameters[8].Value = model.CreateUserID;
            parameters[9].Value = model.PCatageID;
            parameters[10].Value = model.TTCode;
            parameters[11].Value = model.DemandID;
            parameters[12].Value = model.Batch;
            parameters[13].Value = model.ExpectedNum;
            parameters[14].Value = model.IsBlacklistCheck;
            parameters[15].Value = model.BlacklistCheckType;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_PROJECTINFO_UPDATE, parameters);
        }

        public DataTable getCreateUser()
        {

            //DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, "SELECT DISTINCT CreateUserID FROM dbo.ProjectInfo");
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, "SELECT DISTINCT a.CreateUserID,isnull(b.TrueName,'') as TrueName FROM dbo.ProjectInfo AS a LEFT JOIN dbo.v_userinfo AS b ON a.CreateUserID = b.UserID where CreateUserID>0");
            return ds.Tables[0];
        }
        public DataTable GetDataByName(string ProjectName)
        {
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, "SELECT Name FROM dbo.ProjectInfo where name='" + StringHelper.SqlFilter(ProjectName) + "'");
            return ds.Tables[0];
        }

        /// <summary>
        /// 其他任务导出时得到客户信息
        /// </summary>
        /// <param name="custIDFieldName">自定义表中存储CustID的列名</param>
        /// <returns></returns>
        public DataTable GetExportCustInfoByTempt(string custIDFieldName, string projectID)
        {
            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@custIDFieldName", SqlDbType.VarChar, 100),
					new SqlParameter("@ProjectID", SqlDbType.VarChar, 20)
					};

            parameters[0].Value = custIDFieldName;
            parameters[1].Value = projectID;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ProjectInfo_GetCustInfoByExport", parameters);

            return ds.Tables[0];
        }

        public DataTable GetExportCustInfoByUnitTest(string custID)
        {
            DataSet ds;

            SqlParameter[] parameters = { 
                      new SqlParameter("@CustID", SqlDbType.VarChar, 20)
					};

            parameters[0].Value = custID;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ProjectInfo_GetExportCustInfoByUnitTest", parameters);

            return ds.Tables[0];
        }

        public int GetCRMCust(string custID)
        {
            string sqlStr = "select count(1) FROM    CRM2009.dbo.CustInfo AS ci where ci.custid ='" + StringHelper.SqlFilter(custID) + "'";
            Object o = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            return int.Parse(o.ToString());
        }

        public DataTable p_GerNoExistsCustID(string CustIDs)
        {
            DataSet ds = new DataSet();

            SqlParameter[] parameters = {
					new SqlParameter("@InputCustIDs", SqlDbType.VarChar)
					};

            parameters[0].Value = CustIDs;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_GerNoExistsCustID", parameters);

            return ds.Tables[0];
        }



        public void EndCCProjectForYiJiKe(long ProjectID)
        {

            SqlParameter[] parameters = {
					new SqlParameter("@ProjectID", SqlDbType.BigInt)
					};

            parameters[0].Value = ProjectID;

            SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ProjectInfoEndForYijiKe", parameters);

        }

        public void RevokeCJKProjectTask(long ProjectID)
        {

            SqlParameter[] parameters = {
					new SqlParameter("@ProjectID", SqlDbType.BigInt)
					};

            parameters[0].Value = ProjectID;

            SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_RevokeCJKProjectTask", parameters);

        }

        /// <summary>
        /// 某一区域完成目标数时,把状态置为：已撤销，并查询出处理量、成功量，和涉及项目ID、项目名称列表
        /// </summary>
        /// <param name="DemandID">需求单ID</param>
        /// <param name="AreaID">目标地区ID</param>
        /// <param name="DealCount">处理量</param>
        /// <param name="SuccessCount">处理量中的成功个数（成功量）</param>
        /// <returns></returns>
        public DataTable RevokeCJKTaskByDemandID_Area(string DemandID, int AreaID, out int DealCount, out int SuccessCount)
        {

            SqlParameter[] parameters = {
					new SqlParameter("@DemandID", SqlDbType.VarChar),
					new SqlParameter("@AreaID", SqlDbType.Int),
                    new SqlParameter("@DealCount", SqlDbType.Int),
                    new SqlParameter("@SuccessCount", SqlDbType.Int)
					};

            parameters[0].Value = DemandID;
            parameters[1].Value = AreaID;
            parameters[2].Direction = ParameterDirection.Output;
            parameters[3].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_RevokeCJKProjectTaskByAreaID", parameters);
            DealCount = (int)parameters[2].Value;
            SuccessCount = (int)parameters[3].Value;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// 获取项目的外呼状态
        /// <summary>
        /// 获取项目的外呼状态
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public int? GetProjectAutoCallACStatus(long projectid)
        {
            string sql = "SELECT ISNULL(b.ACStatus,0) FROM AutoCall_ProjectInfo b WHERE b.ProjectID=" + projectid;
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (obj == null)
                return null;
            int a = 0;
            if (int.TryParse(obj.ToString(), out a))
            {
                return a;
            }
            return null;
        }

        /// 获取项目的自动外呼相关信息
        /// <summary>
        /// 获取项目的自动外呼相关信息
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public DataTable GetProjectAutoCallInfo(long projectid)
        {
            string sql = @"SELECT ISNULL(a.ACStatus,0) as ACStatus,b.CallNum,a.SkillID,c.Name AS SkillName,a.ProjectID,
                                 d.ACTotalNum,d.IVRConnectNum,d.DisconnectNum
                                 FROM AutoCall_ProjectInfo a
                                 LEFT JOIN dbo.CallDisplay b ON a.CDID=b.CDID
                                 LEFT JOIN dbo.SkillGroup c ON a.SkillID=c.SGID         
                                 LEFT JOIN dbo.AutoCall_ProjectInfoStat d ON d.ProjectID = a.ProjectID
                                 WHERE a.ProjectID=" + projectid;
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }

        public Dictionary<string, int> GetProjectStatInfo(long projectid)
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            SqlParameter[] parameters = {
					new SqlParameter("@ProjectID", SqlDbType.BigInt){Value=projectid},
					new SqlParameter("@ZD_Total", SqlDbType.Int){Direction= ParameterDirection.Output},
                    new SqlParameter("@ZD_Not_JT", SqlDbType.Int){Direction= ParameterDirection.Output},
                    new SqlParameter("@ZD_JT", SqlDbType.Int){Direction= ParameterDirection.Output},

                    new SqlParameter("@RG_Total", SqlDbType.Int){Direction= ParameterDirection.Output},
                    new SqlParameter("@RG_Not_JT", SqlDbType.Int){Direction= ParameterDirection.Output},
                    new SqlParameter("@RG_JT", SqlDbType.Int){Direction= ParameterDirection.Output},

                    new SqlParameter("@WX", SqlDbType.Int){Direction= ParameterDirection.Output}
					};

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_CallRecord_ORIG_Task_Stat", parameters);
            dic["ZD_Total"] = CommonFunction.ObjectToInteger(parameters[1].Value);
            dic["ZD_Not_JT"] = CommonFunction.ObjectToInteger(parameters[2].Value);
            dic["ZD_JT"] = CommonFunction.ObjectToInteger(parameters[3].Value);

            dic["RG_Total"] = CommonFunction.ObjectToInteger(parameters[4].Value);
            dic["RG_Not_JT"] = CommonFunction.ObjectToInteger(parameters[5].Value);
            dic["RG_JT"] = CommonFunction.ObjectToInteger(parameters[6].Value);

            dic["WX"] = CommonFunction.ObjectToInteger(parameters[7].Value);

            return dic;
        }

        /// 结束项目时结束自动外呼
        /// <summary>
        /// 结束项目时结束自动外呼
        /// </summary>
        /// <param name="projectid"></param>
        public int EndAutoCallProject(long projectid)
        {
            string sql1 = @"UPDATE dbo.AutoCall_ProjectInfo
                                 SET ACStatus=" + (int)ProjectACStatus.P03_已结束 + @"
                                 WHERE ProjectID=" + projectid;
            string sql2 = @"UPDATE dbo.AutoCall_TaskInfo
                                 SET ACStatus=" + (int)TaskACStatus.T03_已结束 + @"
                                 WHERE ProjectID=" + projectid;
            int a = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql1);
            int b = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql2);
            return a;
        }

        /// <summary>
        /// 根据项目名称关键字，查找项目表，返回项目名称列表（带数据权限）
        /// </summary>
        /// <param name="projectName">项目名称关键字</param>
        /// <param name="userID">当前登录人ID</param>
        /// <returns>返回项目名称列表</returns>
        public DataTable GetProjectNames(string projectName, int bgid, int scid, int pCatageID, int userID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ProjectID,Name FROM dbo.ProjectInfo WHERE Status<>-1");
            if (bgid > 0)
            {
                sb.Append(" And BGID=" + bgid);
            }
            if (scid > 0)
            {
                sb.Append(" And SCID=" + scid);
            }
            if (pCatageID > 0)
            {
                sb.Append(" And PCatageID=" + pCatageID);
            }
            sb.Append(" AND Name LIKE '%" + StringHelper.SqlFilter(projectName) + "%'");
            sb.Append(UserGroupDataRigth.Instance.GetSqlRightstr("ProjectInfo", "BGID", "CreateUserID", userID));
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sb.Append(" ORDER BY Name").ToString());
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// 根据当前登录人取管辖分组下最近的项目
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetLastestProjectByUserID(QueryProjectInfo query, string order, int currentPage, int pageSize, out int totalCount, int userid)
        {
            string where = string.Empty;
            if (!string.IsNullOrEmpty(query.Name))
            {
                where += " and a.Name like '%" + StringHelper.SqlFilter(query.Name) + "%'";
            }
            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " and a.BGID= " + query.BGID;
            }
            if (!string.IsNullOrEmpty(query.Sources))
            {
                where += " and a.Source in (" + query.Sources + ")";
            }
            DataSet ds;
            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4),
                    new SqlParameter("@userid", SqlDbType.Int, 4)
					};
            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;
            parameters[5].Value = userid;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ProjectInfoByUserID_Select", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// 根据当前登录人取管辖分组所有项目的分配，提交以及接通，成功等统计量
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetB_ProjectReport(Entities.BusinessReport.QueryProjectReport query, string order, int currentPage, int pageSize, out int totalCount, int userid)
        {
            string where = string.Empty;
            string where1 = string.Empty;
            if (query.ProjectID != Constant.INT_INVALID_VALUE)
            {
                where += " and b.projectid=" + query.ProjectID;
                where1 += " and b.projectid=" + query.ProjectID;
            }
            if (!string.IsNullOrEmpty(query.AgentNum))
            {
                where += " and c.AgentNum= '" + StringHelper.SqlFilter(query.AgentNum) + "'";
                where1 += " and c.AgentNum= '" + StringHelper.SqlFilter(query.AgentNum) + "'";
            }
            if (query.UserID != Constant.INT_INVALID_VALUE)
            {
                where += " and c.userid= " + query.UserID;
                where1 += " and c.userid= " + query.UserID;

            }
            if (query.BeginTime != Constant.DATE_INVALID_VALUE && query.EndTime != Constant.DATE_INVALID_VALUE)
            {
                if (query.BusinessType == 6)
                {
                    where1 += " and a.LastUpdateTime>='" + query.BeginTime + "' and a.LastUpdateTime<Dateadd(day,1,'" + query.EndTime + "')";
                }
                if (query.BusinessType == 4)
                {
                    where1 += " and a.lastopttime>='" + query.BeginTime + "' and a.lastopttime<Dateadd(day,1,'" + query.EndTime + "')";
                }
            }
            order = " tjcount desc";
            DataSet ds;
            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 4000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4),
                    new SqlParameter("@BussinessType", SqlDbType.Int, 4),
                    new SqlParameter("@userid", SqlDbType.Int, 4),
                    new SqlParameter("@where1", SqlDbType.NVarChar, 4000),
					};
            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;
            parameters[5].Value = query.BusinessType;
            parameters[6].Value = userid;
            parameters[7].Value = where1;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ProjectBussiness_Select", parameters);

            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// 根据当前登录人取管辖分组所有项目的分配，提交以及接通，成功等统计量总计
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetB_ProjectReportSum(Entities.BusinessReport.QueryProjectReport query, int userid)
        {
            string where = string.Empty;
            string where1 = string.Empty;
            if (query.ProjectID != Constant.INT_INVALID_VALUE)
            {
                where += " and b.projectid=" + query.ProjectID;
                where1 += " and b.projectid=" + query.ProjectID;
            }
            if (!string.IsNullOrEmpty(query.AgentNum))
            {
                where += " and c.AgentNum= '" + StringHelper.SqlFilter(query.AgentNum) + "'";
                where1 += " and c.AgentNum= '" + StringHelper.SqlFilter(query.AgentNum) + "'";
            }
            if (query.UserID != Constant.INT_INVALID_VALUE)
            {
                where += " and c.userid= " + query.UserID;
                where1 += " and c.userid= " + query.UserID;

            }
            if (query.BeginTime != Constant.DATE_INVALID_VALUE && query.EndTime != Constant.DATE_INVALID_VALUE)
            {
                if (query.BusinessType == 6)
                {
                    where1 += " and a.LastUpdateTime>='" + query.BeginTime + "' and a.LastUpdateTime<=Dateadd(day,1,'" + query.EndTime + "')";
                }
                if (query.BusinessType == 4)
                {
                    where1 += " and a.lastopttime>='" + query.BeginTime + "' and a.lastopttime<=Dateadd(day,1,'" + query.EndTime + "')";
                }
            }
            DataSet ds;
            SqlParameter[] parameters = {
					new SqlParameter("@where", 

SqlDbType.NVarChar, 4000),
					
                    new SqlParameter("@BussinessType", SqlDbType.Int, 4),
                    new SqlParameter("@userid", SqlDbType.Int, 4),
                    new SqlParameter("@where1", SqlDbType.NVarChar, 4000),
					};
            parameters[0].Value = where;
            parameters[1].Value = query.BusinessType; ;
            parameters[2].Value = userid;
            parameters[3].Value = where1;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ProjectBussiness_Sum", parameters);
            return ds.Tables[0];
        }


        /// <summary>
        /// 根据当前登录人取管辖分组所有项目的分配，提交以及接通，成功等统计量
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetB_ProjectReport_Excel(Entities.BusinessReport.QueryProjectReport query, int userid)
        {
            string where = string.Empty;
            string where1 = string.Empty;
            if (query.ProjectID != Constant.INT_INVALID_VALUE)
            {
                where += " and b.projectid=" + query.ProjectID;
                where1 += " and b.projectid=" + query.ProjectID;
            }
            if (!string.IsNullOrEmpty(query.AgentNum))
            {
                where += " and c.AgentNum= '" + StringHelper.SqlFilter(query.AgentNum) + "'";
                where1 += " and c.AgentNum= '" + StringHelper.SqlFilter(query.AgentNum) + "'";
            }
            if (query.UserID != Constant.INT_INVALID_VALUE)
            {
                where += " and c.userid= " + query.UserID;
                where1 += " and c.userid= " + query.UserID;

            }
            if (query.BeginTime != Constant.DATE_INVALID_VALUE && query.EndTime != Constant.DATE_INVALID_VALUE)
            {
                if (query.BusinessType == 6)
                {
                    where1 += " and a.LastUpdateTime>='" + query.BeginTime + "' and a.LastUpdateTime<Dateadd(day,1,'" + query.EndTime + "')";
                }
                if (query.BusinessType == 4)
                {
                    where1 += " and a.lastopttime>='" + query.BeginTime + "' and a.lastopttime<Dateadd(day,1,'" + query.EndTime + "')";
                }
            }
            DataSet ds;
            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 4000),
					
                    new SqlParameter("@BussinessType", SqlDbType.Int, 4),
                    new SqlParameter("@userid", SqlDbType.Int, 4),
                    new SqlParameter("@where1", SqlDbType.NVarChar, 4000),
                    new SqlParameter("@projectid", SqlDbType.Int,4)
					};
            parameters[0].Value = where;
            parameters[1].Value = query.BusinessType;
            parameters[2].Value = userid;
            parameters[3].Value = where1;
            parameters[4].Value = query.ProjectID;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ProjectBussiness_Export", parameters);
            return ds.Tables[0];
        }

        /// <summary>
        /// 根据当前登录人取管辖分组所有项目的分配，提交以及接通，成功等统计量总计
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetB_ProjectReportSum_Excel(Entities.BusinessReport.QueryProjectReport query, int userid)
        {
            string where = string.Empty;
            string where1 = string.Empty;
            if (query.ProjectID != Constant.INT_INVALID_VALUE)
            {
                where += " and b.projectid=" + query.ProjectID;
                where1 += " and b.projectid=" + query.ProjectID;
            }
            if (!string.IsNullOrEmpty(query.AgentNum))
            {
                where += " and c.AgentNum= '" + StringHelper.SqlFilter(query.AgentNum) + "'";
                where1 += " and c.AgentNum= '" + StringHelper.SqlFilter(query.AgentNum) + "'";
            }
            if (query.UserID != Constant.INT_INVALID_VALUE)
            {
                where += " and c.userid= " + query.UserID;
                where1 += " and c.userid= " + query.UserID;

            }
            if (query.BeginTime != Constant.DATE_INVALID_VALUE && query.EndTime != Constant.DATE_INVALID_VALUE)
            {
                if (query.BusinessType == 6)
                {
                    where1 += " and a.LastUpdateTime>='" + query.BeginTime + "' and a.LastUpdateTime<=Dateadd(day,1,'" + query.EndTime + "')";
                }
                if (query.BusinessType == 4)
                {
                    where1 += " and a.lastopttime>='" + query.BeginTime + "' and a.lastopttime<=Dateadd(day,1,'" + query.EndTime + "')";
                }
            }
            DataSet ds;
            SqlParameter[] parameters = {
					new SqlParameter("@where", 

SqlDbType.NVarChar, 4000),
					
                    new SqlParameter("@BussinessType", SqlDbType.Int, 4),
                    new SqlParameter("@userid", SqlDbType.Int, 4),
                    new SqlParameter("@where1", SqlDbType.NVarChar, 4000),
                     new SqlParameter("@projectid", SqlDbType.Int,4)
					};
            parameters[0].Value = where;
            parameters[1].Value = query.BusinessType; ;
            parameters[2].Value = userid;
            parameters[3].Value = where1;
            parameters[4].Value = query.ProjectID;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ProjectBussiness_Sum_Export", parameters);
            return ds.Tables[0];
        }


        /// <summary>
        /// 根据项目和失败类型取具体失败原因
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="typestr"></param>
        /// <returns></returns>
        public DataTable GetProjectFailReason(int projectid, string typestr)
        {
            string sqlstr = "select TFValue from Projectinfo a left join tfield b on a.ttcode=b.ttcode where b.tfname='" + StringHelper.SqlFilter(typestr) + "' and projectid=" + projectid;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, null);
            return ds.Tables[0];
        }



        /// <summary>
        /// 根据当前登录人取管辖分组所有人回访记录，提交以及接通，成功等统计量
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetB_ReturnVisitReport(Entities.BusinessReport.QueryReturnVisitReport query, string order, int currentPage, int pageSize, out int totalCount)
        {

            //and a.[year]=2015 and a.[month]=10 and c.bgid =17
            //and b.createuserid=11447 and c.agentnum

            string where = string.Empty;

            if (!string.IsNullOrEmpty(query.AgentNum))
            {
                where += " and c.AgentNum= '" + StringHelper.SqlFilter(query.AgentNum) + "'";
            }
            if (query.UserID != Constant.INT_INVALID_VALUE)
            {
                where += " and b.createuserid= " + query.UserID;
            }
            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " and c.bgid= " + query.BGID;
            }

            order = " dyfzmembercount desc";
            DataSet ds;
            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 4000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4),
                    new SqlParameter("@dqtime", SqlDbType.DateTime, 4),
                    new SqlParameter("@year", SqlDbType.Int, 4),
                    new SqlParameter("@month", SqlDbType.Int, 4)
					};
            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;
            parameters[5].Value = Convert.ToDateTime(query.Year.ToString() + "-" + query.Month.ToString() + "-10");
            parameters[6].Value = query.Year;
            parameters[7].Value = query.Month;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ReturnVisitBussiness_Select", parameters);

            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// 根据当前登录人取管辖分组所有回访记录，提交以及接通，成功等统计量总计
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetB_ReturnVisitReportSum(Entities.BusinessReport.QueryReturnVisitReport query)
        {
            string where = string.Empty;
            string where1 = string.Empty;
            if (!string.IsNullOrEmpty(query.AgentNum))
            {
                where += " and c.AgentNum= '" + StringHelper.SqlFilter(query.AgentNum) + "'";
                where1 += " and cc.AgentNum= '" + StringHelper.SqlFilter(query.AgentNum) + "'";
            }
            if (query.UserID != Constant.INT_INVALID_VALUE)
            {
                where += " and b.createuserid= " + query.UserID;
                where1 += " and cc.userid= " + query.UserID;
            }
            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " and c.bgid= " + query.BGID;
            }
            DataSet ds;
            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 4000),
					
                    new SqlParameter("@dqtime", SqlDbType.DateTime, 4),
                    new SqlParameter("@year", SqlDbType.Int, 4),
                    new SqlParameter("@month", SqlDbType.Int, 4),
                    new SqlParameter("@where1", SqlDbType.NVarChar, 4000)
                    
					};
            parameters[0].Value = where;
            parameters[1].Value = Convert.ToDateTime(query.Year.ToString() + "-" + query.Month.ToString() + "-10");
            parameters[2].Value = query.Year;
            parameters[3].Value = query.Month;
            parameters[4].Value = where1;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ReturnVisitBussiness_Sum", parameters);
            return ds.Tables[0];
        }

        /// <summary>
        /// 根据当前登录人取管辖分组所有人回访记录，提交以及接通，成功等统计量
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetB_ReturnVisitReport_ForExcel(Entities.BusinessReport.QueryReturnVisitReport query)
        {

            //and a.[year]=2015 and a.[month]=10 and c.bgid =17
            //and b.createuserid=11447 and c.agentnum

            string where = string.Empty;

            if (!string.IsNullOrEmpty(query.AgentNum))
            {
                where += " and c.AgentNum= '" + StringHelper.SqlFilter(query.AgentNum) + "'";
            }
            if (query.UserID != Constant.INT_INVALID_VALUE)
            {
                where += " and b.createuserid= " + query.UserID;
            }
            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " and c.bgid= " + query.BGID;
            }
            DataSet ds;
            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 4000),
                    new SqlParameter("@dqtime", SqlDbType.DateTime, 4),
                    new SqlParameter("@year", SqlDbType.Int, 4),
                    new SqlParameter("@month", SqlDbType.Int, 4)
					};
            parameters[0].Value = where;
            parameters[1].Value = Convert.ToDateTime(query.Year.ToString() + "-" + query.Month.ToString() + "-10");
            parameters[2].Value = query.Year;
            parameters[3].Value = query.Month;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ReturnVisitBussiness_Excel", parameters);
            return ds.Tables[0];
        }

        /// <summary>
        /// 根据当前登录人取管辖分组所有回访记录，提交以及接通，成功等统计量总计
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetB_ReturnVisitReportSum_ForExcel(Entities.BusinessReport.QueryReturnVisitReport query)
        {
            string where = string.Empty;
            string where1 = string.Empty;
            if (!string.IsNullOrEmpty(query.AgentNum))
            {
                where += " and c.AgentNum= '" + StringHelper.SqlFilter(query.AgentNum) + "'";
                where1 += " and cc.AgentNum= '" + StringHelper.SqlFilter(query.AgentNum) + "'";
            }
            if (query.UserID != Constant.INT_INVALID_VALUE)
            {
                where += " and b.createuserid= " + query.UserID;
                where1 += " and cc.userid= " + query.UserID;
            }
            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " and c.bgid= " + query.BGID;
            }
            DataSet ds;
            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 4000),
                    new SqlParameter("@dqtime", SqlDbType.DateTime, 4),
                    new SqlParameter("@year", SqlDbType.Int, 4),
                    new SqlParameter("@month", SqlDbType.Int, 4),
                    new SqlParameter("@where1", SqlDbType.NVarChar, 4000)
                    
					};
            parameters[0].Value = where;
            parameters[1].Value = Convert.ToDateTime(query.Year.ToString() + "-" + query.Month.ToString() + "-10");
            parameters[2].Value = query.Year;
            parameters[3].Value = query.Month;
            parameters[4].Value = where1;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ReturnVisitBussiness_Sum_Excel", parameters);
            return ds.Tables[0];
        }
        /// <summary>
        /// 取CRM回访分类
        /// </summary>
        /// <param name="typestr"></param>
        /// <returns></returns>
        public DataTable GetACDictInfoByDictType(string typestr)
        {
            string sqlstr = "SELECT DictID,DictName FROM DictInfo WHERE DictType=" + StringHelper.SqlFilter(typestr) + " ORDER BY DictID";
            DataSet ds = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.Text, sqlstr, null);
            return ds.Tables[0];
        }

    }
}

