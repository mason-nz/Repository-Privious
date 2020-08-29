using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BitAuto.Utils.Data;
using System.Data.SqlClient;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Dal
{

    public class AgentTimeState : DataBase
    {
        public static readonly AgentTimeState Instance = new AgentTimeState();
        protected AgentTimeState()
        {
        }

        public DataTable GetAllStateFromDB()
        {
            //string sqlText = "SELECT AgentName,AgentID,State,AgentAuxState,StartTime,AGTime,ExtensionNum FROM CAgent WHERE GroupName LIKE '%198热线组%'";
            //8：企业热线组,9:个人热线组
            string sqlText = "SELECT c.AgentName,c.AgentID,c.State,c.AgentAuxState,c.StartTime,c.AGTime,c.ExtensionNum,ea.AgentNum FROM CAgent c LEFT JOIN dbo.EmployeeAgent ea ON c.AgentID=ea.UserID WHERE c.GroupName IN(8,9)";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlText);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 查询坐席实时状态
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetAllStateFromDB(QueryAgentState query, string order, int currentPage, int pageSize, out int totalCount)
        {
            totalCount = 0;
            string where = string.Empty;

            #region 数据权限判断
            if (query.LoginID != Constant.INT_INVALID_VALUE)
            {
                string whereDataRight = "";
                whereDataRight = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("ea", "ea", "BGID", "UserID", query.LoginID);

                where += whereDataRight;
            }
            #endregion

            if (!string.IsNullOrEmpty(query.AgentName))
            {
                where += " and c.AgentName='" + StringHelper.SqlFilter(query.AgentName) + "'";
            }

            if (query.AgentAuxState != Constant.INT_INVALID_VALUE)
            {
                where += " and c.AgentAuxState=" + query.AgentAuxState;
            }

            if (query.AgentNum != Constant.STRING_EMPTY_VALUE)
            {
                where += " and ea.AgentNum='" + StringHelper.SqlFilter(query.AgentNum) + "'";
            }

            if (query.UserID != Constant.INT_INVALID_VALUE)
            {
                where += " and c.AgentID=" + query.UserID;
            }

            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " and ea.BGID=" + query.BGID;
            }

            if (query.ExtensionNum != Constant.STRING_EMPTY_VALUE)
            {
                where += " and c.ExtensionNum='" + StringHelper.SqlFilter(query.ExtensionNum) + "'";
            }

            if (query.GroupName != Constant.STRING_EMPTY_VALUE)
            {
                where += " and ea.GroupName='" + StringHelper.SqlFilter(query.GroupName) + "'";
            }

            if (query.State != Constant.INT_INVALID_VALUE)
            {
                where += " and c.State=" + query.State;
            }


            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 4000),
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

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_AgentState_Select", parameters);
            totalCount = (int)(parameters[4].Value);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public DataTable GetAllStateStatisticsFromDB(QueryAgentState query, string order, int currentPage, int pageSize, out int totalCount)
        {
            totalCount = 0;
            string where = string.Empty;

            #region 数据权限判断
            if (query.LoginID != Constant.INT_INVALID_VALUE)
            {
                string whereDataRight = "";
                whereDataRight = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("ea", "ca", "BGID", "AgentID", query.LoginID);

                where += whereDataRight;
            }
            #endregion

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 4000),
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

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_AgentState_SelectStatistics", parameters);
            totalCount = (int)(parameters[4].Value);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        #region 插入坐席临时状态记录

        public bool InsertAgentState2DB(int state, int agentAuxState, int callType, int agTime, DateTime startTime, string agentid, string truename, string groupname, string extensionnum)
        {
            //将当前时间转换成时间戳s
            DateTime dtstart = new DateTime(1970, 1, 1);
            DateTime dtnow = DateTime.Now;
            agTime = (int)(dtnow - dtstart).TotalSeconds;

            string sql = "";
            sql = "SELECT COUNT(*) FROM CAgent WHERE AgentID='" + StringHelper.SqlFilter(agentid) + "'";

            System.Data.DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, System.Data.CommandType.Text, sql);
            if (Convert.ToInt16(ds.Tables[0].Rows[0][0]) > 0)
            {
                sql = "UPDATE CAgent SET State=" + state + ",agentAuxState=" + agentAuxState + ",CallType=" + callType + ",StartTime='" + startTime + "'" + ",AgentName='" + StringHelper.SqlFilter(truename) + "'" + ",GroupName='" + StringHelper.SqlFilter(groupname) + "'" + ",ExtensionNum='" + StringHelper.SqlFilter(extensionnum) + "'" +
                      "WHERE AgentID='" + StringHelper.SqlFilter(agentid) + "'";
            }
            else
            {
                sql = "INSERT CAgent(AgentName,AgentID,State,AgentAuxState,CallType,AGTime,StartTime,GroupName,ExtensionNum) " +
                       "VALUES('" + StringHelper.SqlFilter(truename) + "','" + StringHelper.SqlFilter(agentid) + "'," + state + "," + agentAuxState + "," + callType + "," + agTime + ",'" + startTime + "','" + StringHelper.SqlFilter(groupname) + "','" + StringHelper.SqlFilter(extensionnum) + "')";
            }

            int ival = -2;
            ival = BitAuto.Utils.Data.SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, System.Data.CommandType.Text, sql);

            if (ival < 0)
                return false;

            return true;
        }

        #endregion

        #region 更新坐席临时状态记录
        public bool UpdateAgentState2DB(int state, int agentAuxState, int callType, int agTime, DateTime startTime, string agentid)
        {
            //将当前时间转换成时间戳s
            DateTime dtstart = new DateTime(1970, 1, 1);
            DateTime dtnow = DateTime.Now;
            agTime = (int)(dtnow - dtstart).TotalSeconds;

            string sql = "";
            sql = "UPDATE CAgent SET State=" + state + ",agentAuxState=" + agentAuxState + ",CallType=" + callType + ",AGTime=" + agTime + ",StartTime='" + startTime + "'" +
                  "WHERE AgentID='" + SqlFilter(agentid) + "'";

            int ival = -2;
            ival = BitAuto.Utils.Data.SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, System.Data.CommandType.Text, sql);

            if (ival < 0)
                return false;

            return true;
        }
        #endregion

        #region 删除临时记录

        public bool DeleteAgentState2DB(string agentid)
        {
            string sql = "";
            sql = "DELETE FROM CAgent " +
                  "WHERE AgentID='" + SqlFilter(agentid) + "'";

            int ival = -2;
            ival = BitAuto.Utils.Data.SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, System.Data.CommandType.Text, sql);

            if (ival < 0)
                return false;

            return true;
        }

        #endregion

        #region 插入坐席状态明细记录
        public int InsertAgentStateDetail2DB(int state, int agentAuxState, int callType, DateTime startTime, DateTime endTime, string truename, string agentid, string extensionnum, int bgid)
        {
            if (startTime >= endTime)
            {
                //状态未结束，先设置一个默认的结束时间
                endTime = endTime.AddSeconds(1);
            }

            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@AgentName", SqlDbType.NVarChar,50),
					new SqlParameter("@AgentID", SqlDbType.NVarChar,50),
					new SqlParameter("@State", SqlDbType.Int,4),
					new SqlParameter("@AgentAuxState", SqlDbType.Int,4),
					new SqlParameter("@StartTime", SqlDbType.DateTime),
					new SqlParameter("@EndTime", SqlDbType.DateTime),
					new SqlParameter("@CallType", SqlDbType.Int,4),
                    new SqlParameter("@ExtensionNum", SqlDbType.NVarChar,10),
                    new SqlParameter("@BGID", SqlDbType.Int,4)};

            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = truename;
            parameters[2].Value = agentid;
            parameters[3].Value = state;
            parameters[4].Value = agentAuxState;
            parameters[5].Value = CommonFunction.GetDateTimeStr(startTime); //精确秒
            parameters[6].Value = CommonFunction.GetDateTimeStr(endTime);//精确秒
            parameters[7].Value = callType;
            parameters[8].Value = extensionnum;
            parameters[9].Value = bgid;

            BitAuto.Utils.Data.SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, System.Data.CommandType.StoredProcedure, "p_AgentStateDetail_Insert", parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region 更新状态明细记录
        /// 根据Oid标识更新明细状态结束时间
        /// <summary>
        /// 根据Oid标识更新明细状态结束时间
        /// </summary>
        /// <param name="oid"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public bool UpdateStateDetail2DB(int oid, DateTime endTime)
        {
            //不允许跨天
            string sql = string.Format(@"UPDATE AgentStateDetail 
                                SET EndTime= CASE WHEN StartTime < '{0}' THEN '{0}' ELSE StartTime END
                                WHERE Oid={1} And CAST(StartTime AS date)='{2}'", CommonFunction.GetDateTimeStr(endTime), oid, endTime.ToString("yyyy-MM-dd"));
            int ival = BitAuto.Utils.Data.SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, System.Data.CommandType.Text, sql);

            //清除无效的数据：更新结束时间之后，如果发现结束时间=开始时间，即为无效数据 强斐 2016-5-9
            string delsql = "DELETE FROM  dbo.AgentStateDetail WHERE StartTime=EndTime";
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, System.Data.CommandType.Text, delsql);

            if (ival <= 0)
                return false;
            else
                return true;
        }
        /// 坐席登录时更新退出时间
        /// <summary>
        /// 坐席登录时更新退出时间
        /// </summary>
        /// <param name="agentID"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public string UpdateLoginOffTime(string agentID, DateTime endTime)
        {
            string sql = "";
            string msg = "initmsg";
            sql = "SELECT TOP 1 CASE WHEN DATEDIFF(ss, StartTime, EndTime) IS NULL THEN 0 " + "ELSE DATEDIFF(ss, StartTime, EndTime) END AS logOnTime,Oid " +
                   "FROM    dbo.AgentStateDetail WHERE   State = 2 AND AgentID =" + StringHelper.SqlFilter(agentID) +
                   "ORDER BY Oid DESC";

            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, System.Data.CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count > 0)
            {
                int loginTime = -2;
                loginTime = Convert.ToInt32(ds.Tables[0].Rows[0]["logOnTime"]);

                int loginOid = Convert.ToInt32(ds.Tables[0].Rows[0]["Oid"]);
                if (loginTime == 0)
                {
                    sql = string.Format(@"UPDATE AgentStateDetail 
                                SET EndTime= CASE WHEN StartTime < '{0}' THEN '{0}' ELSE StartTime END
                                WHERE Oid={1}", CommonFunction.GetDateTimeStr(endTime), loginOid);
                    int ival = 0;
                    ival = BitAuto.Utils.Data.SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, System.Data.CommandType.Text, sql);

                    if (ival < 0)
                    {
                        msg = "failed";
                    }
                    else
                    {
                        msg = "success [LoginOnOid=" + loginOid + "]";
                    }
                }
            }
            else
            {
                msg = "norecord";
            }
            return msg;
        }
        #endregion


        /// <summary>
        /// 获取负责分组名字字符串
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserGroupNamesStr(int userId)
        {
            string str = string.Empty;
            SqlParameter parameter = new SqlParameter("@UserID", userId);
            object obj = BitAuto.Utils.Data.SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_UserGroupDataRigth_GetUserRightNameStr", parameter);
            if (obj != null)
            {
                str = CommonFunction.ObjectToString(obj).TrimEnd(',');
            }
            return str;
        }

        /// <summary>
        /// 根据用户ID获取所属分组及外呼出局号
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetBGNameAndOutNum(int userId)
        {
            string sql = "";

            sql = "SELECT bg.Name BGName,bg.BGID,cd.CallNum,cd.OutCallNum,ea.AgentNum FROM dbo.EmployeeAgent ea " +
                   "LEFT JOIN dbo.BusinessGroup bg ON bg.BGID=ea.BGID " +
                   "LEFT JOIN dbo.CallDisplay cd ON cd.CDID=bg.CDID " +
                   "WHERE ea.UserID=" + userId;

            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, System.Data.CommandType.Text, sql);

            string msg = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0][0] == null)
                    return msg;

                msg = ds.Tables[0].Rows[0]["BGID"].ToString() + "," + ds.Tables[0].Rows[0]["BGName"].ToString() + "," + ds.Tables[0].Rows[0]["OutCallNum"].ToString() + "," + ds.Tables[0].Rows[0]["AgentNum"].ToString();
            }

            return msg;
        }

        /// <summary>
        /// 获取在线坐席工号列表（置闲、置忙）
        /// </summary>
        /// <param name="iarray"></param>
        /// <param name="errormsg"></param>
        /// <returns></returns>
        public bool GetAgentNumsOnLine(out int[] iarray, out string errormsg)
        {
            iarray = null;
            errormsg = "";
            try
            {
                string sql = "";
                sql = "SELECT t2.AgentNum,t2.UserID FROM dbo.CAgent t1 LEFT JOIN dbo.EmployeeAgent t2 ON t1.AgentID=t2.UserID " +
                       "WHERE t1.State IN(3,4)";

                DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, System.Data.CommandType.Text, sql);


                if (ds.Tables[0].Rows.Count > 0)
                {
                    iarray = new int[ds.Tables[0].Rows.Count];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        iarray[i] = Convert.ToInt32(ds.Tables[0].Rows[i]["AgentNum"]);
                    }
                }
                else
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                errormsg = ex.StackTrace;
                return false;
            }

        }

        /// <summary>
        /// 获取在线坐席状态列表（置闲、置忙）
        /// </summary>
        /// <param name="iarray"></param>
        /// <param name="errormsg"></param>
        /// <returns></returns>
        public bool GetAgentStateOnLine(out DataTable dt, Vender vender, out string errormsg)
        {
            dt = null;
            errormsg = "";
            try
            {
                string sql = "";
                sql = @"SELECT t2.AgentNum,t2.UserID,t1.State,t1.AgentAuxState,t1.AgentName,t1.ExtensionNum
                        FROM dbo.CAgent t1 LEFT JOIN dbo.EmployeeAgent t2 ON t1.AgentID=t2.UserID 
                        WHERE t1.State IN(3,4) " + GetVenderWhere(vender);

                DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, System.Data.CommandType.Text, sql);


                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];
                }
                else
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                errormsg = ex.StackTrace;
                return false;
            }

        }
        /// <summary>
        /// 获取在线坐席状态列表（工作中）
        /// </summary>
        /// <param name="iarray"></param>
        /// <param name="errormsg"></param>
        /// <returns></returns>
        public bool GetAgentStateWorking(out DataTable dt, Vender vender, out string errormsg)
        {
            dt = null;
            errormsg = "";
            try
            {
                string sql = "";
                sql = @"SELECT t2.AgentNum,t2.UserID,t1.State,t1.AgentAuxState,t1.AgentName,t1.ExtensionNum
                             FROM dbo.CAgent t1 LEFT JOIN dbo.EmployeeAgent t2 ON t1.AgentID=t2.UserID
                             WHERE t1.State=9 " + GetVenderWhere(vender);

                DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, System.Data.CommandType.Text, sql);


                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];
                }
                else
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                errormsg = ex.StackTrace;
                return false;
            }

        }

        private string GetVenderWhere(Vender vender)
        {
            string num = "";
            switch (vender)
            {
                case Vender.Genesys:
                    num = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("GenesysExtensionNumStart");
                    break;
                case Vender.Holly:
                    num = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("HollyExtensionNumStart");
                    break;
                default:
                    break;
            }
            return " and t1.ExtensionNum like '" + StringHelper.SqlFilter(num) + "%'";
        }

        /// <summary>
        /// 根据userid获取所属业务组id串
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserGroupIDsStr(int userId)
        {
            string str = string.Empty;
            SqlParameter parameter = new SqlParameter("@UserID", userId);
            object obj = BitAuto.Utils.Data.SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_UserGroupDataRigth_GetUserRightIDStr", parameter);
            if (obj != null)
            {
                str = obj.ToString().Substring(0, obj.ToString().Length - 1);
            }

            return str;
        }

        /// 通过条件查询置闲或置忙的坐席
        /// <summary>
        /// 通过条件查询置闲或置忙的坐席
        /// </summary>
        /// <param name="vender"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetAgentStateOnLineByWhere(Vender vender, string where)
        {
            string sql = @"SELECT t2.AgentNum,t2.UserID,t1.State,t1.AgentAuxState,t1.AgentName,t1.ExtensionNum,
                                    t2.RegionID,
                                    CASE t2.RegionID WHEN 1 THEN '北京' WHEN 2 THEN '西安' ELSE '' END AS RegionName,
                                    t2.BGID,bg.Name AS BGName
                                    FROM dbo.CAgent t1 
                                    LEFT JOIN dbo.EmployeeAgent t2 ON t1.AgentID=t2.UserID 
                                    LEFT JOIN dbo.BusinessGroup bg ON t2.BGID=bg.BGID
                                    WHERE t1.State IN(3,4) "
                                    + GetVenderWhere(vender) + " "
                                    + where
                                    + @" ORDER BY bg.Name,t1.AgentName";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }
    }
}
