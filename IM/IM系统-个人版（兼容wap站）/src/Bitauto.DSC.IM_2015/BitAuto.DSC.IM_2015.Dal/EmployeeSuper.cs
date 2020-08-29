using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;
using System.Data.SqlClient;
using BitAuto.Utils.Data;
using BitAuto.Utils;

namespace BitAuto.DSC.IM_2015.Dal
{
    public class EmployeeSuper : DataBase
    {
        public static readonly EmployeeSuper Instance = new EmployeeSuper();

        #region const
        private const string P_EmployeeSuper_SELECT = "p_ZuoxiList_Select";
        //private const string P_CUSTHISTORYINFO_INSERT = "p_CustHistoryInfo_Insert";
        //private const string P_CUSTHISTORYINFO_UPDATE = "p_CustHistoryInfo_Update";
        #endregion

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
            //默认在呼叫中心部门筛选
            //string where = " and DepartID in (select ID from SysRightsManager.dbo.f_Cid('DP00323'))";//ConfigurationUtil.GetAppSettingValue("ThisSysID")

            //Modify By Chybin At 2014-4-14 ，增加查询是否只是查询CC部门下的人员，默认是查询只有CC下面的人员
            string where = "";
            if (query.OnlyCCDepart)
            {
                where += " and UserInfo.Status=0 ";

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
                        where += " UserInfo.DepartID in (select ID from SysRightsManager.dbo.f_Cid('" + PartIDs.Split(',')[i] + "')) ";
                    }
                    where += " )";
                }

                //"and DepartID in (select ID from SysRightsManager.dbo.f_Cid('" + BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("PartID") + "'))";//2012.8.20 
            }

            if (query.TrueName != Constant.STRING_INVALID_VALUE)
            {
                where += " and TrueName like '%" + StringHelper.SqlFilter(query.TrueName) + "%'";
            }
            if (query.AgentNum != Constant.INT_INVALID_VALUE)
            {
                where += " and AgentNum=" + query.AgentNum.ToString();
            }
            if (query.RightType != Constant.INT_INVALID_VALUE)
            {
                where += " and RightType=" + query.RightType.ToString();
            }
            //if (query.Role != Constant.STRING_INVALID_VALUE)
            //{
            //    where += " and SysRightsManager.dbo.UserRole.RoleID in (" + StringHelper.SqlFilterByInCondition(query.Role) + ")";
            //}//and SysRightsManager.dbo.UserRole.RoleID in ('SYS022RL00056','SYS022RL00057')
            if (query.BGID != Constant.INT_INVALID_VALUE && query.BGID > 0)
            {
                where += " and CC2012.dbo.EmployeeAgent.BGID=" + query.BGID;
            }
            if (query.RegionID != Constant.INT_INVALID_VALUE && query.RegionID > 0)
            {
                where += " and CC2012.dbo.EmployeeAgent.RegionID=" + query.RegionID;
            }
            if (query.BGIDs != Constant.STRING_INVALID_VALUE)
            {
                where += " and CC2012.dbo.EmployeeAgent.BGID in (" + StringHelper.SqlFilter(query.BGIDs) + ")";
            }
            else
            {
                where += " and CC2012.dbo.EmployeeAgent.BGID in (-100000)";
            }
            if (query.UserID != Constant.INT_INVALID_VALUE)
            {
                where += "and SysRightsManager.dbo.UserInfo.UserID=" + query.UserID.ToString();
            }
            if (query.ADName != Constant.STRING_INVALID_VALUE && query.ADName != Constant.STRING_EMPTY_VALUE)
            {
                where += "and SysRightsManager.dbo.UserInfo.ADName='" + StringHelper.SqlFilter(query.ADName.Trim()) + "'";
            }
            if (!string.IsNullOrEmpty(query.SelectUserIdSql))
            {
                where += " and CC2012.dbo.EmployeeAgent.UserID in (SELECT UserID FROM #tmp) ";
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

            if (!string.IsNullOrEmpty(query.SelectUserIdSql))
            {
                return GetEmployeeSuper(query.SelectUserIdSql, parameters, out totalCount);
            }

            ds = SqlHelper.ExecuteDataset(ConnectionStrings_CC, CommandType.StoredProcedure, P_EmployeeSuper_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        /// 优化查询逻辑，采用临时表方式查询
        /// <summary>
        /// 优化查询逻辑，采用临时表方式查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        private DataTable GetEmployeeSuper(string sql, SqlParameter[] parameters, out int totalCount)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStrings_CC))
            {
                connection.Open();
                try
                {
                    SqlHelper.ExecuteNonQuery(connection, CommandType.Text, sql);
                    DataSet ds = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, P_EmployeeSuper_SELECT, parameters);
                    totalCount = (int)(parameters[4].Value);
                    return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
