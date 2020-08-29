using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.Dal
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 数据访问类EmployeeAgent。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-08-02 10:01:54 Created
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
        private const string P_EMPLOYEEAGENT_SELECTEXCLUSIVE = "p_EmployeeAgent_SelectExclusive";
        #endregion

        #region Contructor
        protected EmployeeAgent()
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EMPLOYEEAGENT_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        public DataTable GetEmployeeAgentsByAgentNum(string AgentNum)
        {
            string strSql = "SELECT UserID FROM CC2012.dbo.EmployeeAgent WHERE AgentNum='" + AgentNum + "'";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
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

        /// <summary>
        /// 2014-06-19 毕帆
        /// 通过UserID和BGID得到一个对象实体
        /// </summary>
        public Entities.EmployeeAgent GetEmployeeAgentByUserIdAndBGID(int UserID, int BGID)
        {
            QueryEmployeeAgent query = new QueryEmployeeAgent();
            query.UserID = UserID;
            query.BGID = BGID;
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
            if (row["RegionID"].ToString() != "")
            {
                model.RegionID = int.Parse(row["RegionID"].ToString());
            }
            if (row["BusinessType"].ToString() != "")
            {
                model.BusinessType = int.Parse(row["BusinessType"].ToString());
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
					new SqlParameter("@AgentNum", SqlDbType.VarChar,50),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
				    new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                    new SqlParameter("@BGID", SqlDbType.Int,4),
                    new SqlParameter("@RegionID", SqlDbType.Int,4),
                    new SqlParameter("@BusinessType", SqlDbType.Int,4)
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.AgentNum;
            parameters[3].Value = model.CreateTime;
            parameters[4].Value = model.CreateUserID;
            parameters[5].Value = model.BGID;
            parameters[6].Value = model.RegionID;
            parameters[7].Value = model.BusinessType;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EMPLOYEEAGENT_INSERT, parameters);
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
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@AgentNum", SqlDbType.VarChar,50),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                    new SqlParameter("@BGID", SqlDbType.Int,4),
                    new SqlParameter("@RegionID", SqlDbType.Int,4),
                    new SqlParameter("@BusinessType", SqlDbType.Int,4)                                        };

            parameters[0].Value = model.UserID;
            parameters[1].Value = model.AgentNum;
            parameters[2].Value = model.CreateUserID;
            parameters[3].Value = model.BGID;
            parameters[4].Value = model.RegionID;
            parameters[5].Value = model.BusinessType;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EMPLOYEEAGENT_UPDATE, parameters);
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

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EMPLOYEEAGENT_DELETE, parameters);
        }
        #endregion

        /// 易集客-获取客服信息
        /// <summary>
        /// 易集客-获取客服信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bgid"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetKeFuByYiJiKe(string name, string bgid, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = "";
            //部门=运营服务中心
            where += " and ui.DepartID in (select ID from SysRightsManager.dbo.f_Cid('DP00805')) ";
            //用户包含新车业务线
            where += " and ui.BusinessLine & 1=1 and ui.userclass=7";
            //界面选择条件
            //用户名
            if (!string.IsNullOrEmpty(name))
            {
                where += "and ui.TrueName like '%" + Utils.StringHelper.SqlFilter(name) + "%'";
            }
            //用户组
            if (!string.IsNullOrEmpty(bgid) && bgid != "-1")
            {
                where += "and bg.BGID=" + Utils.StringHelper.SqlFilter(bgid);
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_EmployeeAgent_SelectByYiJiKe", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public int GetUserCountByGroup(string where)
        {
            int count = 0;
            string sqlStr = "SELECT COUNT(1) FROM EmployeeAgent as ea Left Join  SysRightsManager.dbo.UserInfo as ui On ea.UserID=ui.UserID WHERE 1=1 " + where;

            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            if (obj != DBNull.Value)
            {
                count = int.Parse(obj.ToString());
            }

            return count;
        }

        public void AddOrUpdateWBEmployee(string WBUserIDStr, int RegionID, int genNewAgentID_StartPoint)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@WBUserIDStr", SqlDbType.VarChar),
                    new SqlParameter("@RegionID", SqlDbType.Int) ,
                    new SqlParameter("@GenNewAgentID_StartPoint", SqlDbType.Int) 
                    };
            parameters[0].Value = WBUserIDStr;
            parameters[1].Value = RegionID;
            parameters[2].Value = genNewAgentID_StartPoint;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_EmployeeAgent_AddWB", parameters);

        }

        /// 获取当前登录人下管辖分组的人员
        /// <summary>
        /// 获取当前登录人下管辖分组的人员
        /// </summary>
        /// <returns></returns>
        public DataTable GetEmployeeAgentByLoginUser(int userid)
        {
            //string groupwhere = Dal.UserGroupDataRigth.Instance.GetGroupStr(userid);

            string whereDepart = "";
            string PartIDs = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("YichePartID");
            int DepCount = PartIDs.Split(',').Length;
            if (DepCount > 0)
            {
                whereDepart += " and (";
                for (int i = 0; i < DepCount; i++)
                {
                    if (i != 0)
                    {
                        whereDepart += " or ";
                    }
                    whereDepart += " b.DepartID in (select ID from SysRightsManager.dbo.f_Cid('" + PartIDs.Split(',')[i] + "')) ";
                }
                whereDepart += " )";
            }

            string sql = @"SELECT  a.UserID ,
                    a.AgentNum ,
                    b.TrueName ,
                    a.BGID ,
                    ( SELECT TOP 1
                    ri.RoleName
                    FROM      SysRightsManager.dbo.UserRole AS ur
                    LEFT JOIN SysRightsManager.dbo.RoleInfo AS ri ON ur.RoleID = ri.RoleID
                    WHERE     ur.Status = 0
                    AND ur.UserID = b.UserID
                    AND ri.Status = 0
                    AND ur.SysID = 'SYS024'
                    ) AS [RoleName] ,
                    ISNULL(c.taskcount, 0) TaskCount
                    FROM    dbo.EmployeeAgent a
                    INNER JOIN SysRightsManager.dbo.UserInfo b ON a.UserID = b.UserID
                    LEFT JOIN ( SELECT  UserID ,
                    COUNT(*) AS taskcount
                    FROM    ProjectTask_Employee
                    WHERE   PTID LIKE 'OTH%'
                    GROUP BY UserID
                    ) c ON a.UserID = c.UserID
                    WHERE   1 = 1
                    AND b.Status = 0 
                    AND ISNULL(a.AgentNum,'')<>''
                    AND a.BGID IN (SELECT BGID FROM UserGroupDataRigth WHERE USERID =" + userid + @") " + whereDepart + @" 
                    ORDER BY ISNULL(c.taskcount, 0) asc,a.AgentNum asc";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }

        /// 批量更新所属业务和所属分组
        /// <summary>
        /// 批量更新所属业务和所属分组
        /// </summary>
        /// <param name="uids"></param>
        /// <param name="btype"></param>
        /// <param name="bgid"></param>
        public void UpdateMutilEmployeeAgent(string uids, int btype, int bgid)
        {
            string sql = @"UPDATE EmployeeAgent SET BusinessType=" + btype + @",BGID=" + bgid + @"
                                        WHERE UserID IN (" + Dal.Util.SqlFilterByInCondition(uids) + ")";
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

        /// 查询用户信息（为IM系统）
        /// <summary>
        /// 查询用户信息（为IM系统）
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetEmployeeAgentForIM(int userid)
        {
            string sql = @"SELECT  a.UserID ,
                                        a.AgentNum ,
                                        b.TrueName ,
                                        a.BGID ,
                                        bg.Name AS BGName ,
                                        ( SELECT    LTRIM(BGID) + ','
                                            FROM      dbo.UserGroupDataRigth
                                            WHERE     UserID = a.UserID
                                        FOR
                                            XML PATH('')
                                        ) AS manageBgs ,
                                        ( SELECT    LTRIM(LineID) + ','
                                            FROM      dbo.BusinessGroupLineMapping
                                            WHERE     BGID = a.BGID
                                        FOR
                                            XML PATH('')
                                        ) AS LineIds
                                FROM    dbo.EmployeeAgent a
                                        LEFT JOIN SysRightsManager.dbo.UserInfo b ON a.UserID = b.UserID
                                        LEFT JOIN dbo.BusinessGroup bg ON a.BGID = bg.BGID
                                    WHERE   a.UserID = " + userid;
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }

        public string GetAgentNumberAndUserNameByUserId(int UserId)
        {
            string strSql = @"SELECT TOP 1 b.TrueName +';'+ a.AgentNum FROM dbo.EmployeeAgent AS a INNER JOIN SysRightsManager.dbo.UserInfo AS b
                            ON a.UserID = b.UserID
                            WHERE a.UserID=" + UserId;

            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            if (obj != null)
            {
                return obj.ToString();
            }
            else
            {
                return "";
            }
        }

        /// 获取所有有工号的坐席和全部分组
        /// <summary>
        /// 获取所有有工号的坐席和全部分组
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllEmployeeAgentAndBusinessGroup()
        {
            string whereDepart = GetDepertWhere();
            string sql1 = @"SELECT b.TrueName,a.UserID,a.AgentNum,a.BGID,a.RegionID,a.BusinessType 
                                    FROM dbo.EmployeeAgent a 
                                    INNER JOIN SysRightsManager.dbo.UserInfo b ON b.UserID = a.UserID
                                    WHERE b.Status=0 AND ISNULL(a.AgentNum,'')<>''
                                    " + whereDepart;
            string sql2 = "SELECT BGID,Name FROM BusinessGroup WHERE Status=0";

            DataTable dt1 = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql1).Tables[0];
            DataTable dt2 = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql2).Tables[0];

            dt1.DataSet.Tables.Remove(dt1);
            dt2.DataSet.Tables.Remove(dt2);

            dt1.TableName = "EmployeeAgent";
            dt2.TableName = "BusinessGroup";

            DataSet ds = new DataSet();
            ds.Tables.Add(dt1);
            ds.Tables.Add(dt2);

            return ds;
        }
        /// 获取部门限制条件
        /// <summary>
        /// 获取部门限制条件
        /// </summary>
        /// <returns></returns>
        public string GetDepertWhere()
        {
            string whereDepart = "";
            string PartIDs = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("PartID");
            int DepCount = PartIDs.Split(',').Length;
            if (DepCount > 0)
            {
                whereDepart += " and (";
                for (int i = 0; i < DepCount; i++)
                {
                    if (i != 0)
                    {
                        whereDepart += " or ";
                    }
                    whereDepart += " b.DepartID in (select ID from SysRightsManager.dbo.f_Cid('" + PartIDs.Split(',')[i] + "')) ";
                }
                whereDepart += " )";
            }
            return whereDepart;
        }

        public string GenNewAgentID(int startPoint)
        {
            string sql = string.Format(@"SELECT  CONVERT(INT, AgentNum) AS AgentNum
                                INTO    #AgentIDList
                                FROM    dbo.EmployeeAgent
                                WHERE   AgentNum != ''
                                AND CONVERT(INT, AgentNum) > {0}

                                SELECT  MIN(AgentNum) + 1
                                FROM    ( SELECT    AgentNum
                                FROM      #AgentIDList
                                UNION ALL
                                SELECT    {0}
                                ) AS a
                                WHERE   NOT EXISTS ( SELECT AgentNum
                                FROM   #AgentIDList
                                WHERE  AgentNum = a.AgentNum + 1 )
                                DROP TABLE #AgentIDList", startPoint);
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (obj != null)
            {
                return obj.ToString();
            }
            return string.Empty;
        }
        /// <summary>
        /// 清空指定工号，且员工ID不等于这个参数的数据
        /// </summary>
        /// <param name="agentNum">工号</param>
        /// <param name="userID">员工ID</param>
        /// <returns></returns>
        public int EmptyAgentNum(string agentNum, int userID)
        {
            string sql = string.Format(@"UPDATE EmployeeAgent SET AgentNum='' WHERE AgentNum='{0}' ", agentNum);
            if (userID > 0)
            {
                sql += "AND UserID!=" + userID;
            }
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }


        #region 专属坐席
        /// <summary>
        /// 查询专属坐席
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetEmployeeAgentExclusive(QueryEmployeeAgentExclusive query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            #region 数据权限判断
            if (query.LoginID != Constant.INT_INVALID_VALUE)
            {
                string whereDataRight = "";
                whereDataRight = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("EmployeeAgent", "EmployeeAgent", "BGID", "UserID", query.LoginID);

                where += whereDataRight;
            }
            #endregion

            #region 部门条件
            where += " and v_userinfo.Status=0 ";
            string PartIDs = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("YichePartID");
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
                    where += " DepartID in (select ID from SysRightsManager.dbo.f_Cid('" + PartIDs.Split(',')[i] + "')) ";
                }
                where += " )";
            }
            #endregion

            if (!string.IsNullOrEmpty(query.TrueName))
            {
                where += " and v_userinfo.TrueName like'%" + query.TrueName + "%'";
            }
            if (!string.IsNullOrEmpty(query.AgentNum))
            {
                where += " and AgentNum='" + Utils.StringHelper.SqlFilter(query.AgentNum) + "'";
            }
            if (query.UserID != Constant.INT_INVALID_VALUE)
            {
                where += " and UserID=" + query.UserID.ToString() + "";
            }
            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " and BGID=" + query.BGID.ToString() + "";
            }
            if (!string.IsNullOrEmpty(query.IsExclusive))
            {
                where += " and ";
                where += "( IsExclusive in (" + query.IsExclusive.ToString() + ")";

                if (query.IsExclusive.Contains("0"))//否
                {
                    where += " or IsExclusive is null or IsExclusive='' ";
                }


                where += " )";

            }
            //工号不能为空
            where += " and AgentNum is not null and AgentNum<>''";

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EMPLOYEEAGENT_SELECTEXCLUSIVE, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        /// <summary>
        /// 设置专属客服
        /// </summary>
        /// <param name="userids">用户id</param>
        /// <param name="isexclusive">是否专属（1是，0不是）</param>
        /// <returns></returns>
        public bool SetEmployeeAgentExclusive(string userids, string isexclusive)
        {

            //string sql = "update EmployeeAgent set IsExclusive=@IsExclusive where UserID in (@UserID)";

            //SqlParameter[] parameters = {
            //        new SqlParameter("@UserID",SqlDbType.NVarChar),
            //        new SqlParameter("@IsExclusive", SqlDbType.NVarChar),
            //        };
            //parameters[0].Value = userids;
            //parameters[1].Value = isexclusive;


            string sql = "update EmployeeAgent set IsExclusive=" + isexclusive + " where UserID in (" + Dal.Util.SqlFilterByInCondition(userids) + ")";


            int rows = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);

            return rows > 0 ? true : false;
        }
        /// <summary>
        /// 获取专属数据通过timestamp
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public DataTable GetEmployeeAgentExclusiveByTimestamp(long timestamp, int maxrow = -1)
        {
            string sql = @"	SELECT top " + maxrow + @" EmployeeAgent.UserID,AgentNum,BGID,IsExclusive,
				CAST([TIMESTAMP] AS bigint) as[TIMESTAMP],
				v_userinfo.TrueName as UserName
				FROM dbo.EmployeeAgent inner join v_userinfo on EmployeeAgent.UserID=v_userinfo.UserID
			    WHERE  AgentNum<>'' and AgentNum is not null and Timestamp>" + timestamp + " ORDER BY [TIMESTAMP]";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }


        /// 获取项目表最大时间戳
        /// <summary>
        /// 获取项目表最大时间戳
        /// </summary>
        /// <returns></returns>
        public long GetEmployeeAgentExclusiveMaxTimeStamp_XA()
        {
            string sql = "SELECT ISNULL(MAX([TIMESTAMP]),0) FROM dbo.EmployeeAgent";
            return CommonFunction.ObjectToLong(SqlHelper.ExecuteScalar(ConnectionStrings_Holly_Business, CommandType.Text, sql));
        }

        /// <summary>
        /// 清除西安临时表
        /// </summary>
        /// <returns></returns>
        public bool ClearEmployeeAgentExclusiveTemp_XA()
        {
            string sql = "delete from EmployeeAgent_Temp";
            int rows = SqlHelper.ExecuteNonQuery(ConnectionStrings_Holly_Business, CommandType.Text, sql);

            return rows > 0 ? true : false;
        }


        /// 从临时表更新-项目
        /// <summary>
        /// 从临时表更新-项目
        /// </summary>
        /// <returns></returns>
        public int[] UpdateEmployeeAgentExclusiveFromTemp_XA()
        {
            int mod = 0;
            int add = 0;
            //更新
            string modsql = @"update EmployeeAgent
				                set
				                AgentNum=tmp.AgentNum,
				                UserName=tmp.UserName,
				                BGID=tmp.BGID,
				                IsExclusive=tmp.IsExclusive,
				                [TIMESTAMP]=tmp.[TIMESTAMP],
				                LastSyncTime=GetDate()
				                from EmployeeAgent_Temp tmp
				                where  EmployeeAgent.UserID=tmp.UserID";
            mod = SqlHelper.ExecuteNonQuery(ConnectionStrings_Holly_Business, CommandType.Text, modsql);
            //新增
            string addsql = @"INSERT INTO EmployeeAgent(
                                        UserID,AgentNum,UserName,BGID,IsExclusive,[TIMESTAMP],LastSyncTime)
                                        SELECT 
                                        UserID,AgentNum,UserName,BGID,IsExclusive,[TIMESTAMP],GetDate()
                                        FROM EmployeeAgent_Temp
                                        WHERE UserID NOT IN (SELECT UserID FROM EmployeeAgent)";
            add = SqlHelper.ExecuteNonQuery(ConnectionStrings_Holly_Business, CommandType.Text, addsql);
            return new int[] { mod, add };
        }
        #endregion
    }
}

