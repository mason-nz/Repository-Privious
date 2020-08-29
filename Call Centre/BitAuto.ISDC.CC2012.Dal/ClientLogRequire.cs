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
    public class ClientLogRequire : DataBase
    {
        public static ClientLogRequire Instance = new ClientLogRequire();

        /// 获取所有坐席及在线状态
        /// <summary>
        /// 获取所有坐席及在线状态
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllEmployeeAgent(ClientLogRequireQuery query, int currentPage, int pageSize, out int totalCount)
        {
            string g_num = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("GenesysExtensionNumStart");
            string h_num = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("HollyExtensionNumStart");

            string otherwhere = GetWhere(query);
            string whereDepart = Dal.EmployeeAgent.Instance.GetDepertWhere();

            //人员查询sql
            string emp_sql = @"SELECT b.TrueName as AgentName,a.UserID,a.AgentNum,a.BGID,c.Name AS BGName,a.RegionID,
                                            CASE a.RegionID WHEN 1 THEN '北京' WHEN 2 THEN '西安' ELSE '' END AS RegionName
                                            FROM EmployeeAgent a
                                            INNER JOIN SysRightsManager.dbo.UserInfo b ON a.UserID = b.UserID
                                            LEFT JOIN dbo.BusinessGroup c ON a.BGID = c.BGID
                                            WHERE b.Status=0 AND ISNULL(a.AgentNum,'')<>'' " + whereDepart;
            //时间sql
            string date_sql = @"SELECT d as log_date FROM f_R_GetDay('" + CommonFunction.GetDateTimeStr(query.StartTime) + "','" + CommonFunction.GetDateTimeStr(query.EndTime) + @"')";

            //厂家sql
            string ven_sql = "";
            if (query.Vendor == "0")
            {
                //Genesys
                ven_sql = @"SELECT " + (int)Vender.Genesys + " AS VendorID,'" + Vender.Genesys.ToString() + @"' AS VendorName ";
            }
            else if (query.Vendor == "1")
            {
                //合力
                ven_sql = @"SELECT " + (int)Vender.Holly + " AS VendorID,'" + Vender.Holly.ToString() + @"' AS VendorName ";
            }
            else
            {
                ven_sql = @"SELECT " + (int)Vender.Genesys + " AS VendorID,'" + Vender.Genesys.ToString() + @"' AS VendorName 
                                            UNION ALL 
                                          SELECT " + (int)Vender.Holly + " AS VendorID,'" + Vender.Holly.ToString() + "' AS VendorName ";
            }

            //在线sql
            string onl_sql = @"SELECT AgentID,ExtensionNum,
                                            CASE SUBSTRING(ExtensionNum,1,1) WHEN " + h_num + @" THEN " + (int)Vender.Holly +
                                         " WHEN " + g_num + @" THEN " + (int)Vender.Genesys + @" ELSE -1 END AS VendorID
                                            FROM dbo.CAgent";

            //基础数据sql
            string base_sql = @"SELECT *  INTO #basedata FROM (    
                                    SELECT a.*, c.ExtensionNum,
                                    CASE WHEN c.ExtensionNum IS NULL THEN 1 ELSE 0 END AS On_lineID,
                                    CASE WHEN c.ExtensionNum IS NULL THEN '离线' ELSE '在线' END AS On_line                                   
                                    FROM (
                                    SELECT b.*,a.*,c.*
                                    FROM 
                                    (" + emp_sql + @") a,(" + date_sql + @") b,(" + ven_sql + @") c
                                    ) a LEFT JOIN (" + onl_sql + @") c ON c.AgentID =  a.UserID AND c.VendorID=a.VendorID) tmp 
                                    Where 1=1" + otherwhere;

            //总数据查询
            string sql = @"SELECT * YanfaFROM (
                                    SELECT a.log_date,a.UserID,a.AgentNum,a.AgentName,a.BGID,a.BGName,a.RegionID,a.RegionName,
                                    a.ExtensionNum,a.On_lineID,a.On_line,a.VendorName,a.VendorID,
                                    b.RecID,b.AgentID,b.LogDate,b.Vendor,b.RequireID,b.RequireDateTime,
                                    ISNULL(b.Status,0) AS Status,
                                    b.ResponseDateTime,b.ResponseRemark,
                                    ISNULL(b.ResponseSuccess,0) AS ResponseSuccess,
                                    b.FilePath,
                                    b.CreateTime,b.CreateUserID,b.LastUpdateTime,b.LastUpdateUserID,
                                    u.TrueName AS RequireName
                                    FROM #basedata a
                                    LEFT JOIN dbo.ClientLogRequire b ON a.log_date=b.LogDate AND a.UserID=b.AgentID AND a.VendorID=b.Vendor
                                    LEFT JOIN SysRightsManager.dbo.UserInfo u ON b.RequireID=u.UserID
                                    ) a";

            DataSet ds;
            SqlParameter[] parameters = {
					new SqlParameter("@SQL", SqlDbType.NVarChar, 4000),
					new SqlParameter("@Order", SqlDbType.NVarChar, 200),
					new SqlParameter("@CurPage", SqlDbType.Int, 4),
					new SqlParameter("@PageRows", SqlDbType.Int, 4),
					new SqlParameter("@TotalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = sql;
            parameters[1].Value = "log_date,VendorID,On_lineID,AgentNum desc";
            parameters[2].Value = currentPage;
            parameters[3].Value = pageSize;
            parameters[4].Direction = ParameterDirection.Output;

            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                //构建临时表
                SqlHelper.ExecuteNonQuery(conn, CommandType.Text, base_sql);
                DataTable testdt = SqlHelper.ExecuteDataset(conn, CommandType.Text, "SELECT * FROM #basedata").Tables[0];
                //查询数据
                ds = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "P_Page", parameters);
                totalCount = (int)(parameters[4].Value);
            }
            return ds.Tables[0];
        }
        /// 获取where条件
        /// <summary>
        /// 获取where条件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string GetWhere(ClientLogRequireQuery query)
        {
            string otherwhere = "";
            //姓名 分机号 工号
            if (!string.IsNullOrEmpty(query.Name))
            {
                string name = SqlFilter(query.Name);
                otherwhere += " and ( AgentName like '%" + query.Name + "%' or AgentNum like '%" + query.Name + "%' or ExtensionNum like '%" + query.Name + "%' ) ";
            }
            //厂家
            if (!string.IsNullOrEmpty(query.Vendor))
            {
                otherwhere += " and VendorID in (" + SqlFilter(query.Vendor) + ") ";
            }
            //在线 离线
            if (!string.IsNullOrEmpty(query.Online))
            {
                otherwhere += " and On_lineID in (" + SqlFilter(query.Online) + ") ";
            }
            return otherwhere;
        }
        /// 获取请求数据
        /// <summary>
        /// 获取请求数据
        /// </summary>
        /// <param name="logdate"></param>
        /// <param name="agentid"></param>
        /// <param name="vendorid"></param>
        /// <returns></returns>
        public ClientLogRequireInfo GetClientLogRequireInfo(DateTime logdate, int agentid, int vendorid)
        {
            string sql = "SELECT * FROM ClientLogRequire WHERE AgentID='" + agentid + "' AND Vendor='" + vendorid + "' AND LogDate='" + logdate.ToString("yyyy-MM-dd") + "'";
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return new ClientLogRequireInfo(dt.Rows[0]);
            }
            else return null;
        }
        /// 获取请求数据
        /// <summary>
        /// 获取请求数据
        /// </summary>
        /// <param name="agentid"></param>
        /// <param name="vendorid"></param>
        /// <returns></returns>
        public DataTable GetClientLogRequireInfo(int agentid, int vendorid)
        {
            string sql = "SELECT * FROM ClientLogRequire WHERE AgentID='" + agentid + "' AND Vendor='" + vendorid + "' AND Status=1";
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            return dt;
        }
    }
}
