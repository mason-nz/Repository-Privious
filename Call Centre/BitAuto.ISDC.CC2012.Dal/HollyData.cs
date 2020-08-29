using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using BitAuto.Utils.Data;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class HollyData : DataBase
    {
        public static HollyData Instance = new HollyData();

        ///  创建连接
        /// <summary>
        ///  创建连接
        /// </summary>
        /// <returns></returns>
        public SqlConnection CreateHollySqlConnection()
        {
            SqlConnection conn = new SqlConnection(ConnectionStrings_Holly_Report);
            conn.Open();
            return conn;
        }
        /// 关闭连接
        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <param name="conn"></param>
        public void CloseHollySqlConnection(SqlConnection conn)
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
        /// 创建临时表
        /// <summary>
        /// 创建临时表
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="tablename"></param>
        /// <param name="start"></param>
        public void CreateTempTable(SqlConnection conn, string tablename, long start, int maxrow, string RecordURl, string where)
        {
            string sql = @"SELECT top " + maxrow + @"
                                    RecID,
                                    ContactID AS SessionID ,--联络ID	
                                    DEVICEDN,	   --分机号码
                                    CASE WHEN (ORIANI LIKE '01%' AND ORIANI NOT LIKE '010%') THEN (SUBSTRING(ORIANI,2,LEN(ORIANI)-1)) ELSE  ORIANI END AS ORIANI,		   --主叫
                                    CASE WHEN (ORIDNIS LIKE '01%' AND ORIDNIS NOT LIKE '010%') THEN (SUBSTRING(ORIDNIS,2,LEN(ORIDNIS)-1)) ELSE  ORIDNIS END AS ORIDNIS,	   --被叫
                                    CAST(CALLDIRECTION AS INT) AS CALLDIRECTION,	   --呼叫类型（1：呼入，2:呼出）
                                    SKILLID,	   --技能组id
                                    CASE CAST(STARTTIME AS DATETIME) WHEN '1900-01-01' THEN NULL ELSE CAST(STARTTIME AS DATETIME) END AS InitiatedTime ,--初始化时间
                                    CASE CAST(OFFERINGTIME AS DATETIME) WHEN '1900-01-01' THEN NULL ELSE CAST(OFFERINGTIME AS DATETIME) END AS RingingTime ,--振铃时间
                                    CASE CAST(TALKINGTIME AS DATETIME) WHEN '1900-01-01' THEN NULL ELSE CAST(TALKINGTIME AS DATETIME) END AS EstablishedTime ,--接通时间
                                    CASE CAST(AGENTHANGUPTIME AS DATETIME) WHEN '1900-01-01' THEN NULL ELSE CAST(AGENTHANGUPTIME AS DATETIME) END AS AgentReleaseTime ,--坐席挂机时间
                                    CASE CAST(AGENTHANGUPTIME AS DATETIME) WHEN '1900-01-01' THEN NULL ELSE CAST(AGENTHANGUPTIME AS DATETIME) END AS CustomerReleaseTime ,--客户挂机时间
                                    CASE CAST(ENDTIME AS DATETIME) WHEN '1900-01-01' THEN NULL ELSE CAST(ENDTIME AS DATETIME) END AS ENDTIME, --结束时间
                                    ENDTIME AS ENDTIME_STR, --结束时间（精确毫秒）
                                    CASE CAST(ACWSTARTTIME AS DATETIME) WHEN '1900-01-01' THEN NULL ELSE CAST(ACWSTARTTIME AS DATETIME) END AS AfterWorkBeginTime ,--话后开始时间
                                    TALKINGTIMELEN AS TallTime ,
                                    CASE ISNULL(FileName,'') WHEN '' THEN '' ELSE '" + RecordURl + @"'+FileName END AS AudioURL, --录音URL地址
                                    (SELECT Top 1 VARAGENTIDZ FROM ivr_hang_up WHERE REASON='99' AND ContactID=t_contact_detail.ContactID) AS VARAGENTIDZ,--专属坐席工号
                                    istransfer,isconsult,isconference
                                    INTO #" + tablename + @"	
                                    From t_contact_detail
                                    WHERE   CAST(ENDTIME AS DATETIME)>'1900-01-01' 
                                    AND RecID > " + start + " " + where;

            SqlHelper.ExecuteNonQuery(conn, System.Data.CommandType.Text, sql);
            //创建索引
            SqlHelper.ExecuteNonQuery(conn, System.Data.CommandType.Text, "CREATE INDEX idx_RecID ON #" + tablename + " (RecID)");
        }
        /// 分页查询数据
        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="start"></param>
        /// <param name="RecordURl"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetHollyData(SqlConnection conn, string tablename, int currentPage, int pageSize, out int totalCount)
        {
            string sql = "SELECT * YanFaFrom #" + tablename;


            DataSet ds = null; ;
            SqlParameter[] parameters = {
					new SqlParameter("@SQL", SqlDbType.NVarChar, 4000),
					new SqlParameter("@Order", SqlDbType.NVarChar, 200),
					new SqlParameter("@CurPage", SqlDbType.Int, 4),
					new SqlParameter("@PageRows", SqlDbType.Int, 4),
					new SqlParameter("@TotalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = sql;
            parameters[1].Value = "RecID";
            parameters[2].Value = currentPage;
            parameters[3].Value = pageSize;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "P_Page", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        /// 获取总数
        /// <summary>
        /// 获取总数
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public int GetHollyDataCount(SqlConnection conn, string tablename)
        {
            string sql = @"SELECT  COUNT(*) FROM #" + tablename;
            return (int)SqlHelper.ExecuteScalar(conn, CommandType.Text, sql);
        }
        /// 删除临时表
        /// <summary>
        /// 删除临时表
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public void DropTempTable(SqlConnection conn, string tablename)
        {
            try
            {
                string sql = "drop table #" + tablename;
                SqlHelper.ExecuteNonQuery(conn, System.Data.CommandType.Text, sql);
            }
            catch
            {
            }
        }

        /// 根据主叫号码、被叫号码，查询合力DB，最近一次外呼接通的坐席工号
        /// <summary>
        /// 根据主叫号码、被叫号码，查询合力DB，最近一次外呼接通的坐席工号
        /// </summary>
        /// <param name="callNo">主叫</param>
        /// <param name="callOutPrefix">出局号</param>
        /// <param name="querytime">有效期</param>
        /// <param name="callOutStartTime"></param>
        /// <returns></returns>
        public string GetLastAgentIDByORIDNIS(string callNo, string callOutPrefix, DateTime querytime, out string callOutStartTime)
        {
            string sql = string.Format(@"
                                SELECT TOP 1 a.AGENTID,CAST(a.STARTTIME AS DATETIME) AS STARTTIME
                                FROM HOLLYREPORT.dbo.rt_agent_contact_save a
                                WHERE a.CALLDIRECTION = 2 
                                AND (a.ORIDNIS = '{0}0{1}' OR a.ORIDNIS = '{0}{1}')
                                AND a.TALKING_STARTTIME != ''
                                AND CAST(a.STARTTIME AS DATETIME)>'" + CommonFunction.GetDateTimeStr(querytime) + @"'
                                AND a.AGENTID IN (SELECT AgentNum FROM YCBusiness2015.dbo.EmployeeAgent WHERE IsExclusive=1)
                                ORDER BY CAST(a.STARTTIME AS DATETIME) DESC", callOutPrefix, callNo);

            DataTable dt = SqlHelper.ExecuteDataset(ConnectionStrings_Holly_Report, System.Data.CommandType.Text, sql).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                callOutStartTime = dt.Rows[0]["STARTTIME"].ToString().Trim();
                return dt.Rows[0]["AGENTID"].ToString().Trim();
            }
            else
            {
                callOutStartTime = string.Empty;
                return string.Empty;
            }
        }
    }
}
