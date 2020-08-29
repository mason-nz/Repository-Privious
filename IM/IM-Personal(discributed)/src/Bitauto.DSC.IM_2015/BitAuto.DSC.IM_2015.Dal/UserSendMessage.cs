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
    public class UserSendMessage : DataBase
    {
        #region Instance
        public static readonly UserSendMessage Instance = new UserSendMessage();
        #endregion

        /// <summary>
        /// 查询用户下的用户组
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetUserGroupDataRigthByUserID(int userId)
        {
            string sqlStr = "SELECT ugdr.*,Name FROM UserGroupDataRigth AS ugdr JOIN BusinessGroup AS bg ON ugdr.BGID=bg.BGID WHERE UserID=@UserID";
            SqlParameter parameter = new SqlParameter("UserID", userId);
            DataSet ds = SqlHelper.ExecuteDataset(ConnectionStrings_CC, CommandType.Text, sqlStr, parameter);
            return ds.Tables[0];
        }


        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.SMSTemplate GetSMSTemplate(int RecID)
        {
            string sqlStr = "SELECT * FROM SMSTemplate WHERE RecID=@RecID";
            SqlParameter parameter = new SqlParameter("@RecID", RecID);
            DataSet ds = SqlHelper.ExecuteDataset(ConnectionStrings_CC, CommandType.Text, sqlStr, parameter);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return LoadSingleSMSTemplate(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.SMSTemplate LoadSingleSMSTemplate(DataRow row)
        {
            Entities.SMSTemplate model = new Entities.SMSTemplate();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            if (row["BGID"].ToString() != "")
            {
                model.BGID = int.Parse(row["BGID"].ToString());
            }
            if (row["SCID"].ToString() != "")
            {
                model.SCID = int.Parse(row["SCID"].ToString());
            }
            model.Title = row["Title"].ToString();
            model.Content = row["Content"].ToString();
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

        #region Select
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
            where += "  and (" + StringHelper.SqlFilter(tablename) + "." + StringHelper.SqlFilter(UserIDFileName) + "='" + UserID + "'";
            string gourpstr = GetGroupStr(UserID);
            if (!string.IsNullOrEmpty(gourpstr))
            {
                where += " or " + StringHelper.SqlFilter(tablename) + "." + StringHelper.SqlFilter(BgIDFileName) + " in (" + StringHelper.SqlFilter(gourpstr) + ")";
            }
            where += ")";

            return where;
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
        public DataTable GetSMSTemplate(QuerySMSTemplate query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            #region 数据权限判断
            if (query.LoginID != Constant.INT_INVALID_VALUE)
            {
                string whereDataRight = "";
                whereDataRight = GetSqlRightstr("a", "BGID", "CreateUserID", (Int32)query.LoginID);

                where += whereDataRight;
            }
            #endregion
            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.BGID=" + query.BGID;
            }
            if (query.SCID != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.SCID=" + query.SCID;
            }
            if (query.CreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.CreateUserID=" + query.CreateUserID;
            }
            if (query.Title != Constant.STRING_INVALID_VALUE)
            {
                where += " AND a.title like '%" + StringHelper.SqlFilter(query.Title) + "%'";
            }
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.Status=" + query.Status;
            }
            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.RecID=" + query.RecID;
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

            ds = SqlHelper.ExecuteDataset(ConnectionStrings_CC, CommandType.StoredProcedure, "p_SMSTemplate_Select", parameters);
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
        public DataTable GetSurveyCategory(QuerySurveyCategory query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.SelectType != Constant.INT_INVALID_VALUE)
            {
                //为1，则筛选登陆人管理的所属业务组的信息
                if (query.SelectType == 1 && query.LoginID != Constant.INT_INVALID_VALUE)
                {
                    where += " AND SurveyCategory.BGID IN ( Select BGID From UserGroupDataRigth gdr Where SurveyCategory.BGID=gdr.BGID AND UserID=" + query.LoginID + ")";
                }
            }
            if (query.GroupName != Constant.STRING_INVALID_VALUE)
            {
                where += " AND BusinessGroup.NAME='" + StringHelper.SqlFilter(query.GroupName) + "'";
            }

            if (query.Name != Constant.STRING_INVALID_VALUE)
            {
                where += " AND SurveyCategory.Name like '%" + StringHelper.SqlFilter(query.Name) + "%'";
            }
            if (query.SCID != Constant.INT_INVALID_VALUE)
            {
                where += " AND SurveyCategory.SCID=" + query.SCID;
            }
            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " AND SurveyCategory.BGID=" + query.BGID;
            }
            if (query.Level != Constant.INT_INVALID_VALUE)
            {
                where += " AND SurveyCategory.Level=" + query.Level;
            }
            if (query.Pid != Constant.INT_INVALID_VALUE)
            {
                where += " AND SurveyCategory.Pid=" + query.Pid;
            }
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " AND SurveyCategory.Status=" + query.Status;
            }
            if (query.TypeId != Constant.INT_INVALID_VALUE)
            {
                where += " AND SurveyCategory.TypeId=" + query.TypeId;
            }
            if (query.IsFilterStop)
            {
                where += " AND  SurveyCategory.Status<>1";
            }
            //add by qizq 2014-4-17 状态不等于某值，用于过滤等于-3（固定的分类）的
            if (query.NoStatus != Constant.INT_INVALID_VALUE)
            {
                where += " AND  SurveyCategory.Status<>" + query.NoStatus;
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

            ds = SqlHelper.ExecuteDataset(ConnectionStrings_CC, CommandType.StoredProcedure, "p_SurveyCategory_Select", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        /// <summary>
        /// 获取坐席数据
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetEmployeeAgent(int userid)
        {
            string sql = @"SELECT  BusinessGroup.* ,
                                    EmployeeAgent.UserID
                                    FROM    EmployeeAgent
                                    INNER JOIN dbo.BusinessGroup ON EmployeeAgent.BGID = BusinessGroup.BGID 
                                    WHERE BusinessGroup.Status=0 AND EmployeeAgent.UserID=" + userid;
            DataSet ds = SqlHelper.ExecuteDataset(ConnectionStrings_CC, CommandType.Text, sql);
            return ds.Tables[0];
        }

        public DataTable GetInUseBusinessGroup(int userid)
        {
            string sqlStr = @"SELECT bg.*,CallNum FROM BusinessGroup as bg Left Join CallDisplay as cd on bg.CDID=cd.CDID where bg.Status=0 AND bg.BGID IN ( SELECT BGID
                         FROM   dbo.UserGroupDataRigth ugd
                         WHERE  ugd.UserID =  " + userid + ") ";
            DataSet ds = SqlHelper.ExecuteDataset(ConnectionStrings_CC, CommandType.Text, sqlStr);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 根据组串获取业务组数据
        /// </summary>
        /// <param name="bgids"></param>
        /// <returns></returns>
        public DataTable GetBusinessGroupByBGIDS(string bgids)
        {
            string sqlStr = "SELECT Name,BGID FROM dbo.BusinessGroup WHERE BGID IN(" + StringHelper.SqlFilter(bgids) + ")";

            DataSet ds = SqlHelper.ExecuteDataset(ConnectionStrings_CC, CommandType.Text, sqlStr);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
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
            string where = GetWhereStr(query);

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

            ds = SqlHelper.ExecuteDataset(ConnectionStrings_CC, CommandType.StoredProcedure, "p_ProjectInfo_Select", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        private static string GetWhereStr(QueryProjectInfo query)
        {
            string where = string.Empty;

            if (query.ProjectID != Constant.INT_INVALID_VALUE)
            {
                where += " AND ProjectID =" + query.ProjectID + "";
            }

            if (query.Name != Constant.STRING_INVALID_VALUE)
            {
                where += " AND Name like '%" + StringHelper.SqlFilter(query.Name) + "%'";
            }

            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " AND BGID=" + query.BGID;
            }

            if (query.PCatageID != Constant.INT_INVALID_VALUE)
            {
                where += " AND PCatageID=" + query.PCatageID;
            }

            if (query.Source != Constant.INT_INVALID_VALUE)
            {
                where += " AND Source=" + query.Source;
            }

            if (query.CreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND CreateUserID=" + query.CreateUserID;
            }

            if (query.BeginTime != Constant.STRING_INVALID_VALUE)
            {
                where += " And CreateTime>='" + DateTime.Parse(query.BeginTime).ToShortDateString() + "'";
            }
            if (query.EndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " And CreateTime<='" + DateTime.Parse(query.EndTime).ToShortDateString() + " 23:59:59'";
            }

            if (query.Statuss != Constant.STRING_INVALID_VALUE)
            {
                where += " AND Status in (" + StringHelper.SqlFilter(query.Statuss) + ")";
            }

            if (query.DemandID != Constant.STRING_INVALID_VALUE && query.DemandID != Constant.STRING_EMPTY_VALUE)
            {
                where += "AND DemandID='" + StringHelper.SqlFilter(query.DemandID) + "'";
            }

            if (query.Batch.HasValue)
            {
                where += "AND Batch='" + query.Batch.Value + "'";
            }

            return where;
        }


        /// <summary>
        /// 通过UserID得到一个对象实体
        /// </summary>
        public Entities.EmployeeAgent GetEmployeeAgentByUserID(int UserID)
        {
            QueryEmployeeAgent query = new QueryEmployeeAgent();
            query.UserID = UserID;
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
            Entities.EmployeeAgent model = new Entities.EmployeeAgent();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            if (row["UserID"].ToString() != "")
            {
                model.UserID = int.Parse(row["UserID"].ToString());
            }
            if (row["BGID"].ToString() != "")
            {
                model.BGID = int.Parse(row["BGID"].ToString());
            }
            model.AgentNum = row["AgentNum"].ToString();
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            //if (row["RegionID"].ToString() != "")
            //{
            //    model.RegionID = int.Parse(row["RegionID"].ToString());
            //}
            //if (row["BusinessType"].ToString() != "")
            //{
            //    model.BusinessType = int.Parse(row["BusinessType"].ToString());
            //}
            return model;
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
        public DataTable GetEmployeeAgent(QueryEmployeeAgent query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;
            if (!string.IsNullOrEmpty(query.AgentNum))
            {
                where += " and AgentNum='" + Utils.StringHelper.SqlFilter(query.AgentNum) + "'";
            }
            if (query.UserID != Constant.INT_INVALID_VALUE)
            {
                where += " and ea.UserID=" + query.UserID.ToString() + "";
            }
            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " and ea.BGID=" + query.BGID.ToString() + "";
            }

            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " and ea.RecID=" + query.RecID.ToString() + "";
            }
            if (query.AgentName != Constant.STRING_INVALID_VALUE)
            {
                where += " And TrueName like '%" + Utils.StringHelper.SqlFilter(query.AgentName) + "%'";
            }
            if (query.RegionID != Constant.INT_INVALID_VALUE)
            {
                where += " and ea.RegionID=" + query.RegionID.ToString() + "";
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

            ds = SqlHelper.ExecuteDataset(ConnectionStrings_CC, CommandType.StoredProcedure, "p_EmployeeAgent_Select", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// <summary>
        ///  增加一条数据
        /// </summary>
        public void Insert(Entities.SMSSendHistory model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@TemplateID", SqlDbType.Int,4),
					new SqlParameter("@Phone", SqlDbType.VarChar,23),
					new SqlParameter("@Content", SqlDbType.VarChar,500),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@CRMCustID", SqlDbType.VarChar,20),
					new SqlParameter("@TaskType", SqlDbType.Int,4),
					new SqlParameter("@TaskID", SqlDbType.VarChar,20)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.BGID;
            parameters[2].Value = model.TemplateID;
            parameters[3].Value = model.Phone;
            parameters[4].Value = model.Content;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.CreateUserID;
            parameters[8].Value = model.CustID;
            parameters[9].Value = model.CRMCustID;
            parameters[10].Value = model.TaskType;
            parameters[11].Value = model.TaskID;
            SqlHelper.ExecuteNonQuery(ConnectionStrings_CC, CommandType.StoredProcedure, "p_SMSSendHistory_Insert", parameters);
        }
        /// <summary>
        /// 取所有工单业务类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetWOrderBusiType()
        {
            string sqlStr = "select RecID,BusiTypeName from WOrderBusiType where Status!=-1 order by SortNum asc";
            return SqlHelper.ExecuteDataset(ConnectionStrings_CC, CommandType.Text, sqlStr, null).Tables[0];
        }

        /// <summary>
        /// 根据工单业务类型取工单标签
        /// </summary>
        /// <returns></returns>
        public DataTable GetWOrderTab(int wOrderBusiType)
        {

            string sqlStr = "select RecID ,BusiTypeID,TagName ,PID ,SortNum,Status FROM WOrderTag where Status=1";
            if (wOrderBusiType != 0)
            {
                sqlStr += " and BusiTypeID=" + wOrderBusiType;
            }
            return SqlHelper.ExecuteDataset(ConnectionStrings_CC, CommandType.Text, sqlStr, null).Tables[0];
        }

    }



}
