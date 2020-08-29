using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using XYAuto.Utils.Config;
using System.Data;
using XYAuto.Utils.Data;
using System.Configuration;

namespace XYAuto.ITSC.Chitunion2017.Common.Dal
{
    public class UserInfo : DataBase
    {
        public static readonly UserInfo Instance = new UserInfo();

        #region Contructor

        protected UserInfo()
        {
        }

        #endregion Contructor

        /// <summary>
        /// 登陆逻辑
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="category">用户分类（29001—广告主；29002—媒体主）</param>
        /// <returns>返回用户ID</returns>
        internal int Login(string userName, string pwd, int category)
        {
            int returnValue = -2;	//bad username

            using (SqlConnection conn = new SqlConnection(SYSCONNECTIONSTRINGS))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Parameters.Add("@userName", userName);
                    command.Parameters.Add("@password", pwd);
                    command.Parameters.Add("@category", category);
                    command.Parameters.Add("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "P_UserInfo_Login";
                    command.Connection = conn;

                    conn.Open();
                    command.ExecuteNonQuery();
                    returnValue = Convert.ToInt32(command.Parameters["@RETURN_VALUE"].Value);
                    conn.Close();
                }
            }

            return returnValue;
        }

        /// <summary>
        /// 登陆后写入Cookies
        /// </summary>
        /// <param name="userid">登陆UserID</param>
        /// <returns>返回Cookies内容</returns>
        internal DataTable Passport(int userid, string sysID)
        {
            DataTable dt = GetLoginUserInfo(userid, sysID);
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt;
            }
            return null;
        }

        public DataTable GetLoginUserInfo(int userid, string sysid)
        {
            string sql = string.Format(@"SELECT ui.UserID,
(CASE WHEN ui.UserName='' OR ui.UserName IS NULL THEN ui.Mobile ELSE ui.UserName END) AS UserName,
ui.Mobile,ui.Source,
--ISNULL(ud.TrueName,'')AS TrueName ,
ui.[Type],ui.Category,
[RoleIDs]=ISNULL(STUFF((SELECT ',' + RTRIM(ur.RoleID)
                                 FROM dbo.UserRole ur
                                 WHERE ur.UserID=ui.UserID AND ur.Status=0 and
                                 ur.SysID  ='{0}'
                                  FOR XML PATH('')), 1, 1, ''), ''),
[BUTIDs]=ISNULL(STUFF((SELECT ',' + RTRIM(rm.ModuleID)
								 FROM dbo.UserRole ur
								 JOIN dbo.RoleModule AS rm ON ur.RoleID=rm.RoleID
								 WHERE ur.UserID=ui.UserID AND ur.Status=0 AND rm.Status=0 and
								 ur.SysID  ='{0}' AND rm.ModuleID LIKE '%BUT%'
								  FOR XML PATH('')), 1, 1, ''), '')
FROM dbo.UserInfo AS ui
--LEFT JOIN dbo.UserDetailInfo AS ud ON ui.UserID=ud.UserID
WHERE ui.UserID={1} AND ui.Status=0",
                sysid, userid);

            DataSet ds = SqlHelper.ExecuteDataset(SYSCONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return null;
        }

        /// <summary>
        /// 获取授权AE的UserID ‘,’拼接字符串
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetAuthAEUserIDList(int userId)
        {
            string strSql = string.Format(@" SELECT UserID FROM   dbo.UserInfo WHERE AuthAEUserID = {0} OR UserID = {0}", userId);
            DataTable dt = SqlHelper.ExecuteDataset(SYSCONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
            StringBuilder sb = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["UserID"] != DBNull.Value)
                    {
                        sb.Append(dt.Rows[i]["UserID"] + ",");
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 得到大菜单根据用户id
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="sysID"></param>
        /// <returns></returns>
        internal DataTable GetParentModuleInfoByUserID(string userID, string sysID)
        {
            string sqlstr =
               //"select [RecID],[ModuleID],[SysID],[ModuleName],[PID],[Level],[Intro],[Url],[Status],[CreateTime] from moduleinfo where moduleid in (select  distinct pid from moduleinfo where moduleid in(SELECT DISTINCT  moduleid FROM RoleModule WHERE sysid='" + XYAuto.Utils.StringHelper.SqlFilter(sysID) + "'  and  roleid IN(select roleid from userrole where userid =" + XYAuto.Utils.StringHelper.SqlFilter(userID) + " and sysid='" + XYAuto.Utils.StringHelper.SqlFilter(sysID) + "'  and status=0)) and status=0 and  sysid='" + XYAuto.Utils.StringHelper.SqlFilter(sysID) + "' and [level]=2) and status=0 and  sysid='" + XYAuto.Utils.StringHelper.SqlFilter(sysID) + "' order by OrderNum";
               string.Format(@"SELECT  m.[RecID] ,
        m.[ModuleID] ,
        m.[SysID] ,
        m.[ModuleName] ,
        m.[PID] ,
        m.[Level] ,
        m.[Intro] ,
        d.Domain+m.[Url] AS Url ,
        m.[Url] AS ModuleUrl ,
        m.[Status] ,
        m.[CreateTime]
FROM    ModuleInfo AS m
JOIN dbo.DomainInfo AS d ON m.DomainID=d.RecID
WHERE   m.ModuleID IN (
        SELECT  DISTINCT
                PID
        FROM    ModuleInfo
        WHERE   ModuleID IN ( SELECT DISTINCT
                                        ModuleID
                              FROM      RoleModule
                              WHERE     SysID = '{0}'
                                        AND RoleID IN ( SELECT
                                                              RoleID
                                                        FROM  UserRole
                                                        WHERE UserID = {1}
                                                              AND SysID = '{0}'
                                                              AND Status = 0 ) )
                AND Status = 0
                AND SysID = '{0}'
                AND [Level] = 2 )
        AND m.Status = 0
        AND m.SysID = '{0}'
ORDER BY m.OrderNum;",
               XYAuto.Utils.StringHelper.SqlFilter(sysID),
               XYAuto.Utils.StringHelper.SqlFilter(userID));
            DataSet ds = SqlHelper.ExecuteDataset(SYSCONNECTIONSTRINGS, CommandType.Text, sqlstr);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return null;
        }

        internal DataTable GetChildModuleByUserId(int userId, string sysID, string pid)
        {
            string sqlstr =
                string.Format(@"SELECT  m.ModuleID ,
        m.Level ,
        d.Domain+m.[Url] AS Url ,
        m.ModuleName ,
        m.PID
FROM    ModuleInfo AS m
JOIN dbo.DomainInfo AS d ON m.DomainID=d.RecID
WHERE   m.PID = '{1}'
        AND m.SysID = '{0}'
        AND m.Status = 0
        AND m.ModuleID IN ( SELECT DISTINCT
                                    ModuleID
                          FROM      RoleModule
                          WHERE     RoleID IN ( SELECT  RoleID
                                                FROM    UserRole
                                                WHERE   UserID = {2}
                                                        AND Status = 0 ) )
ORDER BY m.OrderNum",
                sysID, pid, userId);
            DataSet ds = SqlHelper.ExecuteDataset(SYSCONNECTIONSTRINGS, CommandType.Text, sqlstr);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return null;
        }

        /// <summary>
        /// 根据URL，获取ModuleID
        /// </summary>
        /// <param name="uri">URL</param>
        /// <returns>获取ModuleID</returns>
        internal string GetModuleIDByNoSysID(Uri uri)
        {
            string sysid = "";
            string moduleID = "";
            //string path = uri.AbsolutePath;
            string path = "";
            string[] strArr = uri.Segments;//输出 /   2013/      123.html
            foreach (string str in strArr)
            {
                if (!str.EndsWith("/"))
                {
                    break;
                }
                path += str;
            }
            path = path.Trim('/');
            //string sysid = ConfigurationManager.AppSettings["ThisSysID"];
            SqlParameter[] parameters = {
                    new SqlParameter("@Host", SqlDbType.VarChar,50),
                    new SqlParameter("@Path", SqlDbType.VarChar,200)
             };
            parameters[0].Value = uri.Host;
            parameters[1].Value = path;
            DataSet ds = SqlHelper.ExecuteDataset(SYSCONNECTIONSTRINGS, "p_SysInfo_GetSysIDByURL", parameters);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    sysid = ds.Tables[0].Rows[0]["SysID"].ToString();
                }
            }

            if (!string.IsNullOrEmpty(sysid))
            {
                string sqlstr = string.Format(@"select moduleID from  moduleinfo where moduleinfo.status=0 and moduleinfo.sysid='{0}' and CHARINDEX(',{1},',moduleinfo.links)>0", sysid, uri.AbsolutePath);
                DataSet ds2 = SqlHelper.ExecuteDataset(SYSCONNECTIONSTRINGS, CommandType.Text, sqlstr);
                if (ds != null)
                {
                    if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                    {
                        moduleID = ds2.Tables[0].Rows[0]["moduleID"].ToString();
                    }
                }
                return moduleID;
            }
            return moduleID;
        }

        /// <summary>
        /// 根据URI获取ModuleID
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        internal string GetModuleID(Uri uri, string sysid)
        {
            string moduleID = "";
            string path = uri.AbsolutePath;
            //string sysid = ConfigurationManager.AppSettings["ThisSysID"];
            string sqlstr = string.Format(@"select moduleID from  moduleinfo where moduleinfo.status=0 and moduleinfo.sysid='{0}' and CHARINDEX(',{1},',moduleinfo.links)>0", sysid, path);
            DataSet ds = SqlHelper.ExecuteDataset(SYSCONNECTIONSTRINGS, CommandType.Text, sqlstr);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    moduleID = ds.Tables[0].Rows[0]["moduleID"].ToString();
                }
            }
            return moduleID;
        }

        internal DataTable GetModuleByUserId(int userID)
        {
            string sqlstr =
               "select moduleid from moduleinfo where status=0 and moduleid in (SELECT DISTINCT  moduleid FROM RoleModule WHERE roleid IN(select roleid from userrole where userID=" + userID + " and  status=0)) order by level desc";
            DataSet ds = SqlHelper.ExecuteDataset(SYSCONNECTIONSTRINGS, CommandType.Text, sqlstr);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return null;
        }

        internal string GetPIDByModuleID(string moduleID)
        {
            string pid = "";
            string sqlstr = string.Format(@"select pid from moduleinfo where moduleid='{0}'", moduleID);
            DataSet ds = SqlHelper.ExecuteDataset(SYSCONNECTIONSTRINGS, CommandType.Text, sqlstr);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    pid = ds.Tables[0].Rows[0]["pid"].ToString();
                }
            }
            return pid;
        }

        internal static DataTable GetModuleByUserIdAndSysID(int userId, string sysID)
        {
            string sqlstr =
               "select moduleid from moduleinfo where status=0 and moduleid in (SELECT DISTINCT  moduleid FROM RoleModule WHERE roleid IN(select roleid from userrole where userID=" + userId + " and SysID='" + XYAuto.Utils.StringHelper.SqlFilter(sysID) + "' and  status=0)) order by level desc";
            DataSet ds = SqlHelper.ExecuteDataset(SYSCONNECTIONSTRINGS, CommandType.Text, sqlstr);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return null;
        }

        internal DataTable GetMenuModuleInfo(int userId, string sysID)
        {
            string sqlstr =
                //"select moduleid from moduleinfo where status=0 and moduleid in (SELECT DISTINCT  moduleid FROM RoleModule WHERE roleid IN(select roleid from userrole where userID=" + userId + " and SysID='" + XYAuto.Utils.StringHelper.SqlFilter(sysID) + "' and  status=0)) order by level desc";
                string.Format(@"SELECT  m.ModuleID ,
        m.Level ,
        (CASE WHEN ISNULL(m.[Url],'')='' THEN '' ELSE d.Domain+m.[Url] END) AS Url ,
        m.ModuleName ,
        m.PID,m.Links
FROM    ModuleInfo AS m
JOIN dbo.DomainInfo AS d ON m.DomainID=d.RecID
WHERE   m.Level<=2
        AND m.SysID = '{0}'
        AND m.Status = 0
        AND m.ModuleID NOT LIKE '%BUT%'
        AND m.ModuleID IN ( SELECT DISTINCT
                                    ModuleID
                          FROM      RoleModule
                          WHERE     RoleID IN ( SELECT  RoleID
                                                FROM    UserRole
                                                WHERE   UserID = {1}
                                                        AND Status = 0 ) )
ORDER BY m.Level,m.OrderNum", XYAuto.Utils.StringHelper.SqlFilter(sysID), userId);
            DataSet ds = SqlHelper.ExecuteDataset(SYSCONNECTIONSTRINGS, CommandType.Text, sqlstr);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return null;
        }
    }
}