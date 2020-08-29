using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using System.Data.SqlClient;
using System.Data;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.ISDC.CC2012.Entities.CallReport;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class CallRecordReport : DataBase
    {
        public static CallRecordReport Instance = new CallRecordReport();

        /// 清空话务统计
        /// <summary>
        /// 清空话务统计
        /// </summary>
        public int[] ClearCallRecordReportForDate(DateTime date)
        {
            string sql1 = "DELETE FROM Report_CallRecord_Hour WHERE StatDate='" + date.ToString("yyyy-MM-dd") + "'";
            string sql2 = "DELETE FROM Report_CallRecord_Day WHERE StatDate='" + date.ToString("yyyy-MM-dd") + "'";
            int a = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, System.Data.CommandType.Text, sql1);
            int b = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, System.Data.CommandType.Text, sql2);
            return new int[] { a, b };
        }
        /// 清空坐席状态统计
        /// <summary>
        /// 清空坐席状态统计
        /// </summary>
        public int[] ClearAgentStateReportForDate(DateTime date)
        {
            string sql1 = "DELETE FROM Report_AgentState_Hour WHERE StatDate='" + date.ToString("yyyy-MM-dd") + "'";
            string sql2 = "DELETE FROM Report_AgentState_Day WHERE StatDate='" + date.ToString("yyyy-MM-dd") + "'";
            int a = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, System.Data.CommandType.Text, sql1);
            int b = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, System.Data.CommandType.Text, sql2);
            return new int[] { a, b };
        }
        /// 创建话务统计临时表
        /// <summary>
        /// 创建话务统计临时表
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="date"></param>
        /// <param name="tableEndName"></param>
        /// <param name="tmp_hour"></param>
        /// <param name="tmp_day"></param>
        public Dictionary<ReportTempType, string> CreateReportCallRecordStatForDayTmp(SqlConnection conn, DateTime date, ReportTempType tmptype, string tableendname, out string msg)
        {
            #region 创建临时表
            string tmpsql = @"SELECT * ,
                                        CASE WHEN ISNULL(EstablishedTime,'1970-1-1')>'1970-1-1' THEN 1 ELSE 0 END AS ISEstablished,--接通
                                        CASE WHEN ISNULL(TransferInTime,'1970-1-1')>'1970-1-1' THEN 1 ELSE 0 END AS ISTransferIn,--转入
                                        CASE WHEN ISNULL(TransferOutTime,'1970-1-1')>'1970-1-1' THEN 1 ELSE 0 END AS ISTransferOut,--转出
                                        CASE WHEN ISNULL(EstablishedTime,'1970-1-1')>'1970-1-1' THEN (DATEDIFF(SECOND,RingingTime,EstablishedTime)) ELSE (DATEDIFF(SECOND,RingingTime,ReleaseTime)) END AS T_Ringing,--振铃时长
                                        CASE WHEN ISNULL(EstablishedTime,'1970-1-1')>'1970-1-1' THEN (DATEDIFF(SECOND,EstablishedTime,ReleaseTime)) ELSE (0) END AS T_Talk--通话时长
                                        INTO #tmp_call
                                        FROM
                                        (
                                        SELECT 
                                        --主要字段
                                        a.CreateTime,a.CreateUserID as AgentUserID,
                                        CAST(a.CreateTime AS DATE) AS StatDate,
                                        DATENAME(HOUR,a.CreateTime) AS [Hour],
                                        CASE WHEN ISNULL(b.BGID,0)>0 THEN ISNULL(b.BGID,0) ELSE ISNULL(c.BGID,0) END AS BGID,
                                        CASE a.CallStatus 
                                        WHEN 1 THEN ISNULL(d.CDID,0) 
                                        WHEN 2 THEN (CASE a.OutBoundType WHEN 1 THEN 1 WHEN 2 THEN 2 WHEN 4 THEN 3 ELSE 1 END) 
                                        WHEN 3 THEN ISNULL(d.CDID,0) 
                                        ELSE -1 END AS CallType,
                                        a.CallStatus AS CallDirection,
                                        --时间字段
                                        a.InitiatedTime,a.RingingTime,a.EstablishedTime,ISNULL(a.AgentReleaseTime,a.CustomerReleaseTime) AS ReleaseTime,
                                        a.AfterWorkBeginTime,a.AfterWorkTime,a.ConsultTime,a.ReconnectCall,a.TransferInTime,a.TransferOutTime
                                        FROM dbo.CallRecord_ORIG" + tableendname + @" a
                                        LEFT JOIN dbo.CallRecord_ORIG_Business" + tableendname + @" b ON a.CallID=b.CallID
                                        LEFT JOIN dbo.EmployeeAgent c ON a.CreateUserID=c.UserID
                                        LEFT JOIN dbo.CallDisplay d ON a.SwitchINNum=d.AreaCode+d.TelMainNum
                                        WHERE a.CreateTime>='" + date.ToString("yyyy-MM-dd") + @" 00:00:00' AND a.CreateTime<='" + date.ToString("yyyy-MM-dd") + @" 23:59:59'
                                        ) tmp
                                        WHERE tmp.BGID>0 AND tmp.CallType>0
                                        AND ISNULL(InitiatedTime,'1970-1-1')>'1970-1-1'
                                        AND ISNULL(RingingTime,'1970-1-1')>'1970-1-1'
                                        AND ISNULL(ReleaseTime,'1970-1-1')>'1970-1-1'";
            int a = SqlHelper.ExecuteNonQuery(conn, System.Data.CommandType.Text, tmpsql);
            msg = "查询一天的数据量=" + a + ";";
            #endregion

            Dictionary<ReportTempType, string> list = new Dictionary<ReportTempType, string>();
            if (tmptype == ReportTempType.Hour || tmptype == ReportTempType.All)
            {
                string tmp = "#tmp_callrecord_hour";
                string sql = CreateCallRecordStatSql(ReportTempType.Hour, tmp);
                int b = SqlHelper.ExecuteNonQuery(conn, System.Data.CommandType.Text, sql);
                list.Add(ReportTempType.Hour, tmp);
                msg += "统计小时数据=" + b + ";";
            }

            if (tmptype == ReportTempType.Day || tmptype == ReportTempType.All)
            {
                string tmp = "#tmp_callrecord_day";
                string sql = CreateCallRecordStatSql(ReportTempType.Day, tmp);
                int b = SqlHelper.ExecuteNonQuery(conn, System.Data.CommandType.Text, sql);
                list.Add(ReportTempType.Day, tmp);
                msg += "统计天数据=" + b + ";";
            }
            return list;
        }
        /// 创建坐席状态统计临时表
        /// <summary>
        /// 创建坐席状态统计临时表
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="date"></param>
        /// <param name="tablename"></param>
        /// <param name="tmp_hour"></param>
        /// <param name="tmp_day"></param>
        public Dictionary<ReportTempType, string> CreateReportAgentStateStatForDayTmp(SqlConnection conn, DateTime date, ReportTempType tmptype, string tablename, out string msg)
        {
            #region 创建临时表
            string tmpsql = @"
                        SELECT 
                        --统计时间，小时，坐席，状态1,2，时长
                        CAST(StatDate AS DATE) AS StatDate,[Hour],AgentUserID,State,AgentAuxState,T_time,
                        --指标
                        CASE a.STATE WHEN 1 THEN 1 ELSE 0 END AS is_qianchu,
                        CASE a.STATE WHEN 2 THEN 1 ELSE 0 END AS is_qianru,
                        CASE a.STATE WHEN 3 THEN 1 ELSE 0 END AS is_zhixian,
                        CASE a.STATE WHEN 4 THEN 1 ELSE 0 END AS is_zhimang,

                        CASE a.STATE WHEN 1 THEN T_time ELSE 0 END AS t_qianchu,
                        CASE a.STATE WHEN 2 THEN T_time ELSE 0 END AS t_qianru,
                        CASE a.STATE WHEN 3 THEN T_time ELSE 0 END AS t_zhixian,
                        CASE a.STATE WHEN 4 THEN T_time ELSE 0 END AS t_zhimang,

                        CASE WHEN a.STATE=4 AND a.AgentAuxState=0 THEN 1 ELSE 0 END AS is_busy0,
                        CASE WHEN a.STATE=4 AND a.AgentAuxState=1 THEN 1 ELSE 0 END AS is_busy1,
                        CASE WHEN a.STATE=4 AND a.AgentAuxState=2 THEN 1 ELSE 0 END AS is_busy2,
                        CASE WHEN a.STATE=4 AND a.AgentAuxState=3 THEN 1 ELSE 0 END AS is_busy3,
                        CASE WHEN a.STATE=4 AND a.AgentAuxState=4 THEN 1 ELSE 0 END AS is_busy4,
                        CASE WHEN a.STATE=4 AND a.AgentAuxState=5 THEN 1 ELSE 0 END AS is_busy5,
                        CASE WHEN a.STATE=4 AND a.AgentAuxState=6 THEN 1 ELSE 0 END AS is_busy6,

                        CASE WHEN a.STATE=4 AND a.AgentAuxState=0 THEN T_time ELSE 0 END AS t_busy0,
                        CASE WHEN a.STATE=4 AND a.AgentAuxState=1 THEN T_time ELSE 0 END AS t_busy1,
                        CASE WHEN a.STATE=4 AND a.AgentAuxState=2 THEN T_time ELSE 0 END AS t_busy2,
                        CASE WHEN a.STATE=4 AND a.AgentAuxState=3 THEN T_time ELSE 0 END AS t_busy3,
                        CASE WHEN a.STATE=4 AND a.AgentAuxState=4 THEN T_time ELSE 0 END AS t_busy4,
                        CASE WHEN a.STATE=4 AND a.AgentAuxState=5 THEN T_time ELSE 0 END AS t_busy5,
                        CASE WHEN a.STATE=4 AND a.AgentAuxState=6 THEN T_time ELSE 0 END AS t_busy6
                        INTO #tmp_state
                        FROM (
	                        --主表 计算每一个小时的间隔时长
	                        SELECT 
	                        Oid,StatDate,AgentUserID,AgentName,State,AgentAuxState,stat_hour AS [Hour],
	                        CASE WHEN stat_hour=start_hour THEN StartTime ELSE StatDate END AS calc_st,
	                        CASE WHEN stat_hour=end_hour THEN EndTime ELSE DATEADD(hour,1,StatDate) END AS calc_et,
	                        DATEDIFF(SECOND,(CASE WHEN stat_hour=start_hour THEN StartTime ELSE StatDate END),(CASE WHEN stat_hour=end_hour THEN EndTime ELSE DATEADD(hour,1,StatDate) END)) AS T_time
	                        FROM (
		                        SELECT
		                        --所有拆分小时后的明细数据
		                        tmp_main.StatDate,a.Oid,a.AgentID AS AgentUserID,a.AgentName,a.State,a.AgentAuxState,a.StartTime,a.EndTime,
		                        DATENAME(HOUR,a.StartTime) AS start_hour,
		                        DATENAME(HOUR,a.EndTime) AS end_hour,
		                        DATENAME(HOUR,tmp_main.StatDate) AS stat_hour
		                        FROM (
			                        SELECT mind AS StatDate,AgentID 
			                        --全量时间表
			                        FROM f_R_GetMWD('" + date.ToString("yyyy-MM-dd") + @" 00:00:00' ,'" + date.ToString("yyyy-MM-dd") + @" 23:59:59',4) AS tmp_date,
			                        --全量坐席表
			                        (SELECT DISTINCT AgentID FROM " + tablename + @" a 
                                        WHERE a.StartTime>='" + date.ToString("yyyy-MM-dd") + @" 00:00:00' 
                                        AND a.StartTime<='" + date.ToString("yyyy-MM-dd") + @" 23:59:59' 
                                        AND State IN (1,2,3,4)) AS tmp_agent
		                        ) AS tmp_main
		                        --关联具体数据
		                        INNER JOIN " + tablename + @" a 
		                        --8:30-12:30转成7:30-12:30转成8:00,9:00,10:00,11:00,12:00
		                        --8:00-12:00转成7:00-12:00转成9:00,10:00,11:00
		                        ON (tmp_main.AgentID=a.AgentID AND tmp_main.StatDate>DATEADD(HOUR,-1,a.StartTime) AND tmp_main.StatDate<a.EndTime)
                                WHERE a.State IN (1,2,3,4)
	                        ) tmp
                        ) a";
            int a = SqlHelper.ExecuteNonQuery(conn, System.Data.CommandType.Text, tmpsql);
            msg = "查询一天的数据量=" + a + ";";
            #endregion

            Dictionary<ReportTempType, string> list = new Dictionary<ReportTempType, string>();
            if (tmptype == ReportTempType.Hour || tmptype == ReportTempType.All)
            {
                string tmp = "#tmp_agentstate_hour";
                string sql = CreateAgentStatetatSql(ReportTempType.Hour, tmp);
                int b = SqlHelper.ExecuteNonQuery(conn, System.Data.CommandType.Text, sql);
                list.Add(ReportTempType.Hour, tmp);
                msg += "统计小时数据=" + b + ";";
            }

            if (tmptype == ReportTempType.Day || tmptype == ReportTempType.All)
            {
                string tmp = "#tmp_agentstate_day";
                string sql = CreateAgentStatetatSql(ReportTempType.Day, tmp);
                int b = SqlHelper.ExecuteNonQuery(conn, System.Data.CommandType.Text, sql);
                list.Add(ReportTempType.Day, tmp);
                msg += "统计天数据=" + b + ";";
            }
            return list;
        }
        /// 创建统计sql=话务
        /// <summary>
        /// 创建统计sql=话务
        /// </summary>
        /// <param name="type"></param>
        /// <param name="tmp"></param>
        /// <returns></returns>
        private string CreateCallRecordStatSql(ReportTempType type, string tmp)
        {
            string groupby = "";
            if (type == ReportTempType.Hour)
            {
                groupby = "StatDate,Hour,BGID,AgentUserID,CallDirection,CallType";
            }
            else if (type == ReportTempType.Day)
            {
                groupby = "StatDate,BGID,AgentUserID,CallDirection,CallType";
            }
            else
            {
                return "";
            }
            string sql = @"SELECT " + groupby + @",
                                    COUNT(*) AS N_Total,--电话总量
                                    SUM(ISEstablished) AS N_ETotal,--接通总量
                                    COUNT(*)-SUM(ISEstablished) AS N_NoETotal ,-- 未接通总量
                                    SUM(ISTransferIn) AS  N_TransferIn ,-- 转入总量
                                    SUM(ISTransferOut) AS N_TransferOut ,-- 转出总量
                                    SUM(CASE ISEstablished WHEN 1 THEN T_Ringing ELSE 0 END) AS T_ERinging ,-- 接通振铃时长
                                    SUM(CASE ISEstablished WHEN 0 THEN T_Ringing ELSE 0 END) AS T_NoERinging ,-- 未接通振铃时长
                                    SUM(T_Ringing) AS T_Ringing ,-- 总振铃时长
                                    SUM(T_Talk) AS T_Talk ,-- 总通话时长
                                    SUM(AfterWorkTime) AS T_AfterWork ,-- 总话后时长
                                    SUM(T_Ringing) + SUM(T_Talk) + SUM(AfterWorkTime) AS T_ALL ,-- 总时长=（总振铃+总通话+总话后）      
                                    GETDATE() AS CreateTime
                                    INTO " + tmp + @"
                                    FROM #tmp_call
                                    GROUP BY " + groupby + @"
                                    ORDER BY " + groupby;
            return sql;
        }
        /// 创建统计sql=坐席状态
        /// <summary>
        /// 创建统计sql=坐席状态
        /// </summary>
        /// <param name="type"></param>
        /// <param name="tmp"></param>
        /// <returns></returns>
        private string CreateAgentStatetatSql(ReportTempType type, string tmp)
        {
            string groupby = "";
            if (type == ReportTempType.Hour)
            {
                groupby = "StatDate,Hour,AgentUserID";
            }
            else if (type == ReportTempType.Day)
            {
                groupby = "StatDate,AgentUserID";
            }
            else
            {
                return "";
            }
            string sql = @"SELECT " + groupby + @",
                                    SUM(is_qianru) AS N_SignIn ,-- 签入次数
                                    SUM(is_qianchu) AS N_SignOut ,-- 签出次数
                                    SUM(is_zhixian) AS N_Free ,-- 置闲次数
                                    SUM(is_zhimang) AS N_Busy ,-- 置忙次数
                                    SUM(is_busy0) AS N_Busy_0 ,-- 置忙-自动次数
                                    SUM(is_busy1) AS N_Busy_1 ,-- 置忙-小休次数
                                    SUM(is_busy2) AS N_Busy_2 ,-- 置忙-任务回访次数
                                    SUM(is_busy3) AS N_Busy_3 ,-- 置忙-业务处理次数
                                    SUM(is_busy4) AS N_Busy_4 ,-- 置忙-会议次数
                                    SUM(is_busy5) AS N_Busy_5 ,-- 置忙-培训次数
                                    SUM(is_busy6) AS N_Busy_6 ,-- 置忙-离席次数
                                    SUM(t_qianru) AS T_SignIn ,-- 签入时长
                                    NULL AS T_SignOut ,-- 签出时长
                                    SUM(t_zhixian) AS T_Free ,-- 置闲时长
                                    SUM(t_zhimang) AS T_Busy ,-- 置忙时长
                                    SUM(t_busy0) AS T_Busy_0 ,-- 置忙-自动时长
                                    SUM(t_busy1) AS T_Busy_1 ,-- 置忙-小休时长
                                    SUM(t_busy2) AS T_Busy_2 ,-- 置忙-任务回访时长
                                    SUM(t_busy3) AS T_Busy_3 ,-- 置忙-业务处理时长
                                    SUM(t_busy4) AS T_Busy_4 ,-- 置忙-会议时长
                                    SUM(t_busy5) AS T_Busy_5 ,-- 置忙-培训时长
                                    SUM(t_busy6) AS T_Busy_6 ,-- 置忙-离席时长
                                    GETDATE() AS CreateTime
                                    INTO " + tmp + @"
                                    FROM #tmp_state
                                    GROUP BY " + groupby + @"
                                    ORDER BY " + groupby;
            return sql;
        }

        /// 从临时表导入数据到正式表-话务
        /// <summary>
        /// 从临时表导入数据到正式表-话务
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="tmp"></param>
        /// <returns></returns>
        public int[] ImportReportCallRecordStatForDayTmp(SqlConnection conn, string tmp_hour, string tmp_day)
        {
            string sql1 = @"INSERT Report_CallRecord_Hour
                                (StatDate,Hour,BGID,AgentUserID,CallDirection,CallType,N_Total,N_ETotal,N_NoETotal,N_TransferIn,N_TransferOut,T_ERinging,T_NoERinging,T_Ringing,T_Talk,T_AfterWork,T_ALL,CreateTime)
                                SELECT 
                                StatDate,Hour,BGID,AgentUserID,CallDirection,CallType,N_Total,N_ETotal,N_NoETotal,N_TransferIn,N_TransferOut,T_ERinging,T_NoERinging,T_Ringing,T_Talk,T_AfterWork,T_ALL,CreateTime 
                                FROM " + tmp_hour;
            int a = SqlHelper.ExecuteNonQuery(conn, System.Data.CommandType.Text, sql1);

            string sql2 = @"INSERT Report_CallRecord_Day
                                (StatDate,BGID,AgentUserID,CallDirection,CallType,N_Total,N_ETotal,N_NoETotal,N_TransferIn,N_TransferOut,T_ERinging,T_NoERinging,T_Ringing,T_Talk,T_AfterWork,T_ALL,CreateTime)
                                SELECT 
                                StatDate,BGID,AgentUserID,CallDirection,CallType,N_Total,N_ETotal,N_NoETotal,N_TransferIn,N_TransferOut,T_ERinging,T_NoERinging,T_Ringing,T_Talk,T_AfterWork,T_ALL,CreateTime 
                                FROM " + tmp_day;
            int b = SqlHelper.ExecuteNonQuery(conn, System.Data.CommandType.Text, sql2);

            return new int[] { a, b };
        }
        /// 从临时表导入数据到正式表-坐席状态
        /// <summary>
        /// 从临时表导入数据到正式表-坐席状态
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="tmp"></param>
        /// <returns></returns>
        public int[] ImportReportAgentStateStatForDayTmp(SqlConnection conn, string tmp_hour, string tmp_day)
        {
            string sql1 = @"INSERT Report_AgentState_Hour 
                                    (StatDate,HOUR,AgentUserID,N_SignIn,N_SignOut,N_Free,N_Busy,N_Busy_0,N_Busy_1,N_Busy_2,N_Busy_3,N_Busy_4,N_Busy_5,N_Busy_6,
                                    T_SignIn,T_SignOut,T_Free,T_Busy,T_Busy_0,T_Busy_1,T_Busy_2,T_Busy_3,T_Busy_4,T_Busy_5,T_Busy_6,CreateTime)
                                    SELECT 
                                    StatDate,HOUR,AgentUserID,N_SignIn,N_SignOut,N_Free,N_Busy,N_Busy_0,N_Busy_1,N_Busy_2,N_Busy_3,N_Busy_4,N_Busy_5,N_Busy_6,
                                    T_SignIn,T_SignOut,T_Free,T_Busy,T_Busy_0,T_Busy_1,T_Busy_2,T_Busy_3,T_Busy_4,T_Busy_5,T_Busy_6,CreateTime
                                    FROM " + tmp_hour;
            int a = SqlHelper.ExecuteNonQuery(conn, System.Data.CommandType.Text, sql1);

            string sql2 = @"INSERT Report_AgentState_Day 
                                    (StatDate,AgentUserID,N_SignIn,N_SignOut,N_Free,N_Busy,N_Busy_0,N_Busy_1,N_Busy_2,N_Busy_3,N_Busy_4,N_Busy_5,N_Busy_6,
                                    T_SignIn,T_SignOut,T_Free,T_Busy,T_Busy_0,T_Busy_1,T_Busy_2,T_Busy_3,T_Busy_4,T_Busy_5,T_Busy_6,CreateTime)
                                    SELECT 
                                    StatDate,AgentUserID,N_SignIn,N_SignOut,N_Free,N_Busy,N_Busy_0,N_Busy_1,N_Busy_2,N_Busy_3,N_Busy_4,N_Busy_5,N_Busy_6,
                                    T_SignIn,T_SignOut,T_Free,T_Busy,T_Busy_0,T_Busy_1,T_Busy_2,T_Busy_3,T_Busy_4,T_Busy_5,T_Busy_6,CreateTime
                                    FROM " + tmp_day;
            int b = SqlHelper.ExecuteNonQuery(conn, System.Data.CommandType.Text, sql2);

            return new int[] { a, b };
        }

        /// 呼入报表-明细
        /// <summary>
        /// 呼入报表-明细
        /// </summary>
        /// <param name="query"></param>
        /// <param name="conn"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="searchTableName"></param>
        /// <param name="searchAgentTableName"></param>
        /// <param name="RecordCount"></param>
        /// <returns></returns>
        public DataTable GetCallInReportData(QueryCallRecordInfo query, SqlConnection conn, int PageIndex, int PageSize, string searchTableName, string searchAgentTableName, out int RecordCount)
        {
            RecordCount = 0;
            //小时字段
            string addHour = GetHourFiled(query);
            //话务表过滤条件
            string where_call = GetReportWhereCall(query,"CallIN");
            //获取日期部分
            string sqldatename = GetMyOrderStatDate(query);
            //获取查询语句
            string sql = GetCallReportINSql(searchTableName, searchAgentTableName, addHour, where_call, sqldatename, "明细");
            //查询数据
            DataTable dt = GetPagedTable(conn, sql, "", "StartTime DESC,TrueName ASC ", PageIndex, PageSize, out RecordCount);
            return dt;
        }
        /// 呼入报表-汇总
        /// <summary>
        /// 呼入报表-汇总
        /// </summary>
        /// <param name="query"></param>
        /// <param name="conn"></param>
        /// <param name="searchTableName"></param>
        /// <param name="searchAgentTableName"></param>
        /// <returns></returns>
        public DataTable GetCallInReportDataTotal(QueryCallRecordInfo query, SqlConnection conn, string searchTableName, string searchAgentTableName)
        {
            //小时字段
            string addHour = GetHourFiled(query);
            //话务表过滤条件
            string where_call = GetReportWhereCall(query, "CallIN");
            //获取日期部分
            string sqldatename = GetMyOrderStatDate(query);
            //获取查询语句
            string sql = GetCallReportINSql(searchTableName, searchAgentTableName, addHour, where_call, sqldatename, "汇总");
            //查询数据
            return SqlHelper.ExecuteDataset(conn, System.Data.CommandType.Text, sql).Tables[0];
        }
        /// 获取呼入报表的统计sql
        /// <summary>
        /// 获取呼入报表的统计sql
        /// </summary>
        /// <param name="searchTableName"></param>
        /// <param name="searchAgentTableName"></param>
        /// <param name="addHour"></param>
        /// <param name="where_call"></param>
        /// <param name="sqldatename"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetCallReportINSql(string searchTableName, string searchAgentTableName, string addHour, string where_call, string sqldatename, string type)
        {
            string sql = @"
                                SELECT 
                                --格式转换
                                {0}
                                N_ETotal AS N_CallIsQuantity, 
                                N_Busy AS N_SetBuzy,
                                N_TransferOut,
                                N_TransferIn,
                                p_gongshilv AS P_WorkTimeUse,
                                t_pjring AS A_AverageRingTime ,
                                t_pjtime AS A_AverageTalkTime,
                                A_AverageSetBusy,
                                A_AfterworkTime,                                
                                --总时长
                                (" + GetHourFormatFiled("T_AfterWork") + @") AS T_AfterworkTime,
                                (" + GetHourFormatFiled("T_Ringing") + @") AS T_RingingTime,
                                (" + GetHourFormatFiled("T_Talk") + @") AS T_TalkTime,
                                (" + GetHourFormatFiled("T_SignIn") + @") AS T_SetLogin,
                                (" + GetHourFormatFiled("T_Busy") + @") AS T_SetBuzy
                                FROM(
                                    --计算率值指标
                                    SELECT  
                                    {0}
                                    N_Total, N_ETotal, N_Busy ,N_TransferIn,N_TransferOut,
                                    T_AfterWork,T_Talk,T_Ringing,T_SignIn,T_Busy,
                                    CASE WHEN N_Total=0  THEN '0.00%' ELSE CONVERT(NVARCHAR(100), CAST(N_ETotal*1.0/N_Total*100.0 AS decimal(18,2)))+ '%' END AS p_jietonglv,    
                                    CASE WHEN N_Total=0  THEN 0  ELSE CAST(T_Ringing*1.0/N_Total*1.0 AS decimal(18,2)) END  AS t_pjring,
                                    CASE WHEN N_Total=0  THEN 0  ELSE CAST(T_AfterWork*1.0/N_Total*1.0 AS decimal(18,2)) END  AS A_AfterworkTime,    
                                    CASE WHEN N_ETotal=0  THEN 0   ELSE CAST(T_Talk*1.0/N_ETotal*1.0 AS decimal(18,2))  END AS t_pjtime, 
                                    CASE WHEN N_Busy=0  THEN 0  ELSE CAST(T_Busy*1.0/N_Busy*1.0 AS decimal(18,2)) END  AS A_AverageSetBusy,    
                                    CASE WHEN T_SignIn=0  THEN '0.00%' ELSE CONVERT(NVARCHAR(100), CAST(T_Talk*1.0/T_SignIn*100.0 AS decimal(18,2))) + '%' END AS p_gongshilv 
                                    FROM (
                                        --按照【时间+坐席】分组统计
                                        SELECT 
                                        {0}
                                        SUM(N_Total) N_Total,
                                        SUM(N_ETotal) N_ETotal,
                                        SUM(T_Ringing) T_Ringing,
                                        SUM(T_Talk) T_Talk,
                                        SUM(T_AfterWork) T_AfterWork,
                                        SUM(N_TransferIn) N_TransferIn,
                                        SUM(N_TransferOut) N_TransferOut,
                                        SUM(T_SignIn) T_SignIn,
                                        SUM(T_Busy) T_Busy,
                                        SUM(N_Busy) N_Busy
                                        FROM (
                                            SELECT 
                                            " + sqldatename + @"
                                            b.UserID,b.TrueName,
                                            em.AgentNum,
                                            --7个指标
                                            a.*,
                                            --3个指标
                                            ISNULL(c.T_SignIn,0) T_SignIn,
                                            ISNULL(c.T_Busy,0) T_Busy,
                                            ISNULL(c.N_Busy,0) N_Busy
                                            FROM(
                                                --话务表有分组ID，根据数据权限过滤后，汇总到【时间+坐席】
                                                SELECT 
                                                StatDate,AgentUserID,hour, 
                                                --7个指标
                                                SUM(ISNULL(N_Total,0)) N_Total,
                                                SUM(ISNULL(N_ETotal,0)) N_ETotal,
                                                SUM(ISNULL(T_Ringing,0)) T_Ringing,
                                                SUM(ISNULL(T_Talk,0)) T_Talk,
                                                SUM(ISNULL(T_AfterWork,0)) T_AfterWork,
                                                SUM(ISNULL(N_TransferIn,0)) N_TransferIn,
                                                SUM(ISNULL(N_TransferOut,0)) N_TransferOut 
                                                FROM (SELECT * " + addHour + " FROM " + searchTableName + @" ) a
                                                INNER JOIN CallDisplay call ON a.CallType=call.CDID 
                                                LEFT JOIN EmployeeAgent em ON a.AgentUserID=em.UserID 
                                                WHERE a.CallDirection IN (1,3) 
                                                " + where_call + @"  
                                                GROUP BY StatDate,AgentUserID,hour
                                            ) a 
                                            INNER JOIN (
                                                --状态表数据
                                                SELECT *  " + addHour + @" 
                                                FROM  " + searchAgentTableName + @" a 
                                            ) c ON  a.StatDate=c.StatDate  AND a.AgentUserID=c.AgentUserID  
                                            INNER JOIN dbo.v_userinfo b ON a.AgentUserID=b.UserID 
                                            LEFT JOIN EmployeeAgent em ON b.UserID=em.UserID  
                                        )  tmp 
                                       {1}
                                    ) t1
                                ) t2";

            if (type == "明细")
            {
                sql = string.Format(sql, "StartTime,UserID,TrueName,AgentNum,", "GROUP BY StartTime,UserID,TrueName,AgentNum");
            }
            else if (type == "汇总")
            {
                sql = string.Format(sql, "", "");
            }
            return sql;
        }

        /// 呼出报表-明细
        /// <summary>
        /// 呼出报表-明细
        /// </summary>
        /// <param name="query"></param>
        /// <param name="conn"></param>
        /// <param name="searchTableName"></param>
        /// <param name="searchAgentTableName"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="RecordCount"></param>
        /// <returns></returns>
        public DataTable GetCallOutReportData(QueryCallRecordInfo query, SqlConnection conn, string searchTableName, string searchAgentTableName, int PageIndex, int PageSize, out int RecordCount)
        {
            RecordCount = 0;
            //小时字段
            string addHour = GetHourFiled(query);
            //话务表过滤条件
            string where_call = GetReportWhereCall(query, "CallOUT");
            //获取日期部分
            string sqldatename = GetMyOrderStatDate(query);
            //获取查询语句
            string sql = GetCallReportOutSql(searchTableName, searchAgentTableName, addHour, where_call, sqldatename, "明细");
            //查询数据
            return GetPagedTable(conn, sql, "", "StartTime DESC,TrueName ASC", PageIndex, PageSize, out RecordCount);
        }
        /// 呼出报表-汇总
        /// <summary>
        /// 呼出报表-汇总
        /// </summary>
        /// <param name="query"></param>
        /// <param name="conn"></param>
        /// <param name="searchTableName"></param>
        /// <param name="searchAgentTableName"></param>
        /// <returns></returns>
        public DataTable GetCallOutDataReportTotal(QueryCallRecordInfo query, SqlConnection conn, string searchTableName, string searchAgentTableName)
        {
            //小时字段
            string addHour = GetHourFiled(query);
            //话务表过滤条件
            string where_call = GetReportWhereCall(query, "CallOUT");
            //获取日期部分
            string sqldatename = GetMyOrderStatDate(query);
            //获取查询语句
            string sql = GetCallReportOutSql(searchTableName, searchAgentTableName, addHour, where_call, sqldatename, "汇总");
            //查询数据
            return SqlHelper.ExecuteDataset(conn, System.Data.CommandType.Text, sql).Tables[0];
        }
        /// 获取呼出报表的统计sql
        /// <summary>
        /// 获取呼出报表的统计sql
        /// </summary>
        /// <param name="searchTableName"></param>
        /// <param name="searchAgentTableName"></param>
        /// <param name="addHour"></param>
        /// <param name="where_call"></param>
        /// <param name="sqldatename"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetCallReportOutSql(string searchTableName, string searchAgentTableName, string addHour, string where_call, string sqldatename, string type)
        {
            string sql = @"
                                SELECT 
                                --格式转换
                                {0}
                                N_Total, 
                                N_ETotal,
                                p_jietonglv,
                                p_gongshilv,
                                t_pjtime,
                                t_pjring,
                                --总时长
                                (" + GetHourFormatFiled("T_Talk") + @") AS T_Talk,
                                (" + GetHourFormatFiled("T_Ringing") + @") AS T_Ringing,
                                (" + GetHourFormatFiled("T_SignIn") + @") AS T_SignIn
                                FROM( 
                                    --计算率值指标
                                    SELECT  
                                    {0}
                                    N_Total,
                                    N_ETotal,
                                    T_Talk,
                                    T_Ringing, 
                                    T_SignIn,  
                                    CASE WHEN N_Total=0  THEN '0.00%' ELSE CONVERT(NVARCHAR(100), CAST(N_ETotal*1.0/N_Total*100.0 AS decimal(18,2)))+ '%' END AS p_jietonglv,   
                                    CASE WHEN N_Total=0  THEN 0  ELSE CAST(T_Ringing*1.0/N_Total*1.0 AS decimal(18,2)) END AS t_pjring,
                                    CASE WHEN N_ETotal=0 THEN 0 ELSE CAST(T_Talk*1.0/N_ETotal*1.0 AS decimal(18,2)) END AS t_pjtime,     
                                    CASE WHEN T_SignIn=0 THEN '0.00%' ELSE CONVERT(NVARCHAR(100), CAST(T_Talk*1.0/T_SignIn*100.0 AS decimal(18,2)))+  '%' END AS p_gongshilv
                                    FROM ( 
                                        --按照【时间+坐席】分组统计
                                        SELECT 
                                        {0}
                                        SUM(N_Total) N_Total, 
                                        SUM(N_ETotal) N_ETotal ,
                                        SUM(T_Talk) T_Talk, 
                                        SUM(T_Ringing) T_Ringing,
                                        SUM(T_SignIn) T_SignIn 
                                        FROM (
                                            SELECT
                                            " + sqldatename + @"
                                            b.TrueName, b.UserID,
                                            em.AgentNum,
                                            a.*,
                                            --1个指标
                                            ISNULL(c.T_SignIn,0) as T_SignIn
                                            FROM (
                                                --话务表有分组ID，根据数据权限过滤后，汇总到【时间+坐席】
                                                SELECT 
                                                StatDate,AgentUserID,hour,
                                                --4个指标
                                                SUM(ISNULL(N_Total,0)) N_Total, 
                                                SUM(ISNULL(N_ETotal,0)) N_ETotal ,
                                                SUM(ISNULL(T_Talk,0)) T_Talk, 
                                                SUM(ISNULL(T_Ringing,0)) T_Ringing 
                                                FROM(
                                                SELECT a.* " + addHour + @" 
                                                FROM  " + searchTableName + @" a 
                                                LEFT JOIN EmployeeAgent em ON a.AgentUserID=em.UserID 
                                                WHERE  a.CallDirection=2 
                                                " + where_call + @"
                                            ) t 
                                           GROUP BY StatDate,AgentUserID,hour 
                                        ) a 
                                        INNER JOIN (
                                            --状态表数据
                                            SELECT *  " + addHour + @"  
                                            FROM " + searchAgentTableName + @"
                                        ) c ON a.StatDate=c.StatDate AND a.Hour=c.Hour AND a.AgentUserID=c.AgentUserID 
                                        INNER JOIN dbo.v_userinfo b ON a.AgentUserID=b.UserID 
                                        LEFT JOIN EmployeeAgent em ON b.UserID=em.UserID
                                        )  tmp
                                       {1}
                                    ) t1  
                                ) t2";
            if (type == "明细")
            {
                sql = string.Format(sql, "StartTime,UserID,TrueName,AgentNum,", "GROUP BY StartTime,UserID,TrueName,AgentNum");
            }
            else if (type == "汇总")
            {
                sql = string.Format(sql, "", "");
            }
            return sql;
        }

        /// 分页查询
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        private DataTable GetPagedTable(SqlConnection conn, string sql, string where, string order, int pageIndex, int pageSize, out int rowCount)
        {
            rowCount = 0;
            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }
            string pagesql = @"SELECT * FROM (
                                        SELECT *,row_number() OVER(ORDER BY " + order + @") AS rn 
                                        FROM (" + sql + @") AS mytab 
                                        WHERE 1=1 " + where + @") AS mytabtt ";
            if (pageSize > 0)
            {
                string sqlCount = "SELECT count(1) as count FROM ( " + sql + " ) as t4";
                rowCount = (int)SqlHelper.ExecuteScalar(conn, System.Data.CommandType.Text, sqlCount);

                pagesql += "WHERE mytabtt.rn BETWEEN " + ((pageIndex - 1) * pageSize + 1) + " AND " + pageIndex * pageSize;
                return SqlHelper.ExecuteDataset(conn, System.Data.CommandType.Text, pagesql).Tables[0];
            }
            else
            {
                DataTable dt = SqlHelper.ExecuteDataset(conn, System.Data.CommandType.Text, pagesql).Tables[0];
                rowCount = dt.Rows.Count;
                return dt;
            }
        }
        /// 报表条件
        /// <summary>
        /// 报表条件
        /// </summary>
        /// <param name="query"></param>
        /// <param name="where"></param>
        /// <param name="addHour"></param>
        private string GetReportWhereCall(QueryCallRecordInfo query,string type)
        {
            //是否需要数据权限
            bool isneedright = true;
            string where = "";
            //时间
            if (query.BeginTime != Constant.DATE_INVALID_VALUE)
            {
                where += " AND a.StatDate >='" + DateTime.Parse(query.BeginTime.ToString()).ToString("yyyy-MM-dd") + "'";
            }
            if (query.EndTime != Constant.DATE_INVALID_VALUE)
            {
                where += " AND a.StatDate <'" + DateTime.Parse(query.EndTime.ToString()).AddDays(1).ToString("yyyy-MM-dd") + "'";
            }
            //坐席
            if (!string.IsNullOrEmpty(query.Agent))
            {
                isneedright = false;
                where += " and a.AgentUserID=" + CommonFunction.ObjectToInteger(SqlFilter(query.Agent));
            }
            //呼出-分组
            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                isneedright = false;
                where += " AND em.BGID=" + query.BGID + "";
            }
            //工号
            if (!string.IsNullOrEmpty(query.AgentNum))
            {
                where += " and em.AgentNum='" + SqlFilter(query.AgentNum) + "'";
            }
            //呼入-业务线
            if (!string.IsNullOrEmpty(query.selBusinessType))
            {
                where += "  AND CONVERT(NVARCHAR(50),(call.AreaCode+''+call.TelMainNum))='" + query.selBusinessType + "'";
            }
            //数据权限
            if (isneedright)
            {
                if (query.LoginID != Constant.INT_INVALID_VALUE)
                {
                    string whereDataRight = "";
                    if (type == "CallIN")
                    {
                        whereDataRight = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("a", "a", "BGID", "AgentUserID", query.LoginID);
                    }
                    else if (type == "CallOUT")
                    {
                        whereDataRight = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("em", "a", "BGID", "AgentUserID", query.LoginID);
                    }
                   
                    where += whereDataRight;
                }
            }
            return where;
        }
        /// 计算小时字段名称
        /// <summary>
        /// 计算小时字段名称
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string GetHourFiled(QueryCallRecordInfo query)
        {
            string addHour = ",0 AS hour ";
            if (query.QueryType == 4)
            {
                addHour = " ";
            }
            return addHour;
        }
        /// 计算日期的分组名称
        /// <summary>
        /// 计算日期的分组名称
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string GetMyOrderStatDate(QueryCallRecordInfo query)
        {
            string sqldatename = "";
            if (query.QueryType == 1)
            {
                sqldatename = @"(CONVERT(NVARCHAR(50),(CONVERT(varchar(100),(
                SELECT TOP 1 mind FROM f_R_GetMWD(CONVERT(DATE,'" + query.BeginTime + "'),CONVERT(DATE,'" + query.EndTime + @"'),1) as tmptime 
                WHERE a.StatDate between tmptime.mind AND tmptime.maxd ),23))+'到'+ 
                CONVERT(NVARCHAR(50),(
                SELECT TOP 1 maxd FROM f_R_GetMWD(convert(DATE,'" + query.BeginTime + "'),CONVERT(DATE,'" + query.EndTime + @"'),1)  as tmptime 
                WHERE a.StatDate between tmptime.mind AND tmptime.maxd),23)))  AS StartTime,";
            }
            else if (query.QueryType == 2)
            {
                sqldatename = @"(CONVERT(NVARCHAR(50),(CONVERT(varchar(100),(
                SELECT TOP 1 mind FROM f_R_GetMWD(CONVERT(DATE,'" + query.BeginTime + "'),CONVERT(DATE,'" + query.EndTime + @"'),2)  as tmptime 
                WHERE a.StatDate between tmptime.mind AND tmptime.maxd),23))+'到'+ 
                CONVERT(NVARCHAR(50),(
                SELECT TOP 1 maxd FROM f_R_GetMWD(convert(DATE,'" + query.BeginTime + "'),CONVERT(DATE,'" + query.EndTime + @"'),2)  as tmptime 
                WHERE a.StatDate between tmptime.mind AND tmptime.maxd),23))) AS StartTime,";
            }
            else if (query.QueryType == 3)
            {
                sqldatename = "CONVERT(varchar(100), a.StatDate, 23)AS StartTime,";
            }
            else if (query.QueryType == 4)
            {
                sqldatename = "( '" + query.BeginTime.Value.ToString("yyyy-MM-dd") + @"'+' '+ right('0'+convert(VARCHAR(2),a.hour),2)+'时到'+right('0'+convert(VARCHAR(2),a.hour+1),2)+'时') AS StartTime,";
            }
            return sqldatename;
        }
        /// 获取小时的转换字段
        /// <summary>
        /// 获取小时的转换字段
        /// </summary>
        /// <param name="filed"></param>
        /// <returns></returns>
        private string GetHourFormatFiled(string filed)
        {
            return "(CASE WHEN LEN(ltrim(" + filed + "/3600))=1 THEN '0'+ltrim(" + filed + "/3600) ELSE ltrim(" + filed + "/3600) END)" +
                "+':'+right('0'+ltrim(" + filed + "%3600/60),2)+':'+right('0'+ltrim(" + filed + "%60),2)";
        }
    }
}
