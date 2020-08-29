using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using System.Threading;
using System.IO;
using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class EmployeeSuper : DataBase
    {
        public static readonly EmployeeSuper Instance = new EmployeeSuper();

        #region const
        private const string P_ZuoxiList_Select = "p_ZuoxiList_Select";
        //private const string P_CUSTHISTORYINFO_INSERT = "p_CustHistoryInfo_Insert";
        //private const string P_CUSTHISTORYINFO_UPDATE = "p_CustHistoryInfo_Update";
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
        public DataTable GetEmployeeSuper(QueryEmployeeSuper query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = "";

            #region 部门条件
            if (query.OnlyCCDepart)
            {
                where += " AND UserInfo.Status=0 ";

                if (string.IsNullOrEmpty(query.PartIDType))
                {
                    query.PartIDType = "YichePartID";
                }
                string PartIDs = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue(query.PartIDType);
                int DepCount = PartIDs.Split(',').Length;
                if (DepCount > 0)
                {
                    where += " AND (";
                    for (int i = 0; i < DepCount; i++)
                    {
                        if (i != 0)
                        {
                            where += " OR ";
                        }
                        where += " UserInfo.DepartID IN (SELECT ID FROM SysRightsManager.dbo.f_Cid('" + PartIDs.Split(',')[i] + "')) ";
                    }
                    where += " )";
                }
            }
            #endregion

            if (query.UsercodeNotEmpty)
            {
                where += " AND SysRightsManager.dbo.UserInfo.UserCode >''";
            }

            if (query.TrueName != Constant.STRING_INVALID_VALUE)
            {
                where += " AND TrueName LIKE '%" + Utils.StringHelper.SqlFilter(query.TrueName) + "%'";
            }
            if (query.AgentNum != Constant.STRING_INVALID_VALUE)
            {
                where += " AND AgentNum='" + StringHelper.SqlFilter(query.AgentNum.ToString()) + "'";
            }
            if (query.RightType != Constant.INT_INVALID_VALUE)
            {
                where += " AND RightType=" + query.RightType.ToString();
            }
            if (query.Role != Constant.STRING_INVALID_VALUE)
            {
                where += " AND SysRightsManager.dbo.UserRole.RoleID IN (" + Dal.Util.SqlFilterByInCondition(query.Role) + ")";
            }
            if (query.BGID != Constant.INT_INVALID_VALUE && query.BGID > 0)
            {
                where += " AND CC2012.dbo.EmployeeAgent.BGID=" + query.BGID;
            }
            else
            {
                where += " AND CC2012.dbo.EmployeeAgent.BGID IS NOT NULL ";
            }
            if (query.RegionID != Constant.INT_INVALID_VALUE && query.RegionID > 0)
            {
                where += " AND CC2012.dbo.EmployeeAgent.RegionID=" + query.RegionID;
            }
            if (!string.IsNullOrEmpty(query.BGIDs))
            {
                //where += " and CC2012.dbo.EmployeeAgent.BGID in (" + Dal.Util.SqlFilterByInCondition(query.BGIDs) + ")";
                //包含本人
                where += " AND (CC2012.dbo.EmployeeAgent.BGID IN (" + Dal.Util.SqlFilterByInCondition(query.BGIDs) + ") "
                    + "OR SysRightsManager.dbo.UserInfo.UserID=" + query.ContainLoginUserID + ")";
            }
            if (query.UserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND SysRightsManager.dbo.UserInfo.UserID=" + query.UserID.ToString();
            }
            if (query.ADName != Constant.STRING_INVALID_VALUE && query.ADName != Constant.STRING_EMPTY_VALUE)
            {
                where += " AND SysRightsManager.dbo.UserInfo.ADName='" + StringHelper.SqlFilter(query.ADName.Trim()) + "'";
            }
            if (!string.IsNullOrEmpty(query.SelectUserIdSql))
            {
                where += " AND CC2012.dbo.EmployeeAgent.UserID IN (SELECT UserID FROM #tmp) ";
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

            using (SqlConnection connection = new SqlConnection(CONNECTIONSTRINGS))
            {
                connection.Open();
                if (!string.IsNullOrEmpty(query.SelectUserIdSql))
                {
                    SqlHelper.ExecuteNonQuery(connection, CommandType.Text, query.SelectUserIdSql);
                }
                ds = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, P_ZuoxiList_Select, parameters);
                totalCount = (int)(parameters[4].Value);
                return ds.Tables[0];
            }
        }
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
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return ds.Tables[0];
        }
        #endregion

        #region 更新数据权限
        /// <summary>
        /// 更新数据权限
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="dataType">数据权限码，1-个人，2-全部</param>
        /// <param name="creatUserID">修改者UserID</param>
        /// <returns></returns>
        public int UserDataRight_Update(string userID, string creatUserID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.BigInt,8),
					new SqlParameter("@CreateUserID", SqlDbType.BigInt,8)
                                        };
            parameters[0].Value = userID;
            parameters[1].Value = creatUserID;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_UserDataRigth_Update", parameters);
        }
        #endregion

        #region 更新数据权限
        /// <summary>
        /// 更新用户坐席号
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="dataType">坐席号</param>
        /// <param name="creatUserID">修改者UserID</param>
        /// <returns></returns>
        public int EmployeeAgent_Update(string userID, string agentNum, string creatUserID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.BigInt,8),
					new SqlParameter("@AgentNum", SqlDbType.Int,50),
					new SqlParameter("@CreateUserID", SqlDbType.BigInt,8)
                                        };
            parameters[0].Value = userID;
            parameters[1].Value = agentNum;
            parameters[2].Value = creatUserID;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_EmployeeAgent_Update", parameters);
        }
        #endregion

        #region 查询集中权限
        /// 查询集中权限人员信息
        /// <summary>
        /// 查询集中权限人员信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public SysRightUserInfo GetSysRightUserInfo(int userid)
        {
            List<SysRightUserInfo> list = GetSysRightUserInfo(userid.ToString());
            if (list != null && list.Count > 0)
            {
                return list[0];
            }
            else return null;
        }
        /// 查询集中权限人员信息
        /// <summary>
        /// 查询集中权限人员信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<SysRightUserInfo> GetSysRightUserInfo(string userids)
        {
            List<SysRightUserInfo> list = new List<SysRightUserInfo>();
            if (userids == "") return list;
            string sql = @"SELECT a.UserID,a.UserCode,a.TrueName,a.DepartID,a.DepartName,a.DepartPath,a.NamePath,a.BusinessLine,a.Email,a.ADName,
                                    b.BGID,b.AgentNum
                                    FROM dbo.v_userinfo a 
                                    LEFT JOIN dbo.EmployeeAgent b ON b.UserID = a.UserID
                                    WHERE a.Status=0 AND a.UserID in (" + Dal.Util.SqlFilterByInCondition(userids) + ")";
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new SysRightUserInfo(dr));
                }
            }
            return list;
        }
        /// 根据TrueName查询集中权限人员信息
        /// <summary>
        /// 根据TrueName查询集中权限人员信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetSysRightUserByName(string userName, string order, int pageindex, int pagesize, out int total)
        {
            string strSql = @"SELECT   
                                    ui.UserCode ,
                                    ui.TrueName TrueName ,
                                    NamePath ,
                                    ui.UserID,
                                    ui.ADName
                                    YanFaFROM dbo.v_userinfo AS ui
                                    WHERE ui.UserCode > ''
                                    AND ui.Status = 0";
            if (!string.IsNullOrEmpty(userName.Trim()))
            {
                strSql += " AND ui.TrueName LIKE '%" + SqlFilter(userName) + "%'";
            }


            SqlParameter[] parameters = {
					new SqlParameter("@SQL", SqlDbType.NVarChar, 4000),
					new SqlParameter("@Order", SqlDbType.NVarChar, 200),
					new SqlParameter("@CurPage", SqlDbType.Int, 4),
					new SqlParameter("@PageRows", SqlDbType.Int, 4),
					new SqlParameter("@TotalRecorder", SqlDbType.Int, 4)
					};
            parameters[0].Value = strSql;
            parameters[1].Value = order;
            parameters[2].Value = pageindex;
            parameters[3].Value = pagesize;
            parameters[4].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_Page", parameters);
            total = (int)(parameters[4].Value);
            //汇总数据查询
            return ds.Tables[0];
        }
        #endregion
    }
}
