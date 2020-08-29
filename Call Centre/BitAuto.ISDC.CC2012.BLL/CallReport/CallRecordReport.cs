using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.CallReport;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class CallRecordReport
    {
        public static CallRecordReport Instance = new CallRecordReport();

        /// 清空话务统计
        /// <summary>
        /// 清空话务统计
        /// </summary>
        public int[] ClearCallRecordReportForDate(DateTime date)
        {
            return Dal.CallRecordReport.Instance.ClearCallRecordReportForDate(date);
        }
        /// 清空坐席状态统计
        /// <summary>
        /// 清空坐席状态统计
        /// </summary>
        public int[] ClearAgentStateReportForDate(DateTime date)
        {
            return Dal.CallRecordReport.Instance.ClearAgentStateReportForDate(date);
        }
        /// 创建话务统计临时表
        /// <summary>
        /// 创建话务统计临时表
        /// </summary>
        /// <param name="date"></param>
        /// <param name="tmp_hour"></param>
        /// <param name="tmp_day"></param>
        public Dictionary<ReportTempType, string> CreateReportCallRecordStatForDayTmp(SqlConnection conn, DateTime date, ReportTempType tmptype, out string msg)
        {
            string tableEndName = BLL.Util.CalcTableNameByMonth(3, date);
            return Dal.CallRecordReport.Instance.CreateReportCallRecordStatForDayTmp(conn, date, tmptype, tableEndName, out msg);
        }
        /// 创建坐席状态统计临时表
        /// <summary>
        /// 创建坐席状态统计临时表
        /// </summary>
        /// <param name="date"></param>
        /// <param name="tmp_hour"></param>
        /// <param name="tmp_day"></param>
        public Dictionary<ReportTempType, string> CreateReportAgentStateStatForDayTmp(SqlConnection conn, DateTime date, ReportTempType tmptype, out string msg)
        {
            string tablename = "";
            if (date.Date == DateTime.Today)
            {
                tablename = "AgentStateDetail";
            }
            else
            {
                tablename = "AgentStateDetailHistory";
            }
            return Dal.CallRecordReport.Instance.CreateReportAgentStateStatForDayTmp(conn, date, tmptype, tablename, out msg);
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
            return Dal.CallRecordReport.Instance.ImportReportCallRecordStatForDayTmp(conn, tmp_hour, tmp_day);
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
            return Dal.CallRecordReport.Instance.ImportReportAgentStateStatForDayTmp(conn, tmp_hour, tmp_day);
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
            return Dal.CallRecordReport.Instance.GetCallOutReportData(query, conn, searchTableName, searchAgentTableName, BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
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
            return Dal.CallRecordReport.Instance.GetCallOutDataReportTotal(query, conn, searchTableName, searchAgentTableName);
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
            return Dal.CallRecordReport.Instance.GetCallInReportData(query, conn, BLL.PageCommon.Instance.PageIndex, PageSize, searchTableName, searchAgentTableName, out RecordCount);
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
            return Dal.CallRecordReport.Instance.GetCallInReportDataTotal(query, conn, searchTableName, searchAgentTableName);
        }
        /// 创建连接
        /// <summary>
        /// 创建连接
        /// </summary>
        /// <returns></returns>
        public SqlConnection CreateSqlConnection()
        {
            SqlConnection conn = new SqlConnection(CommonBll.CC_conn);
            conn.Open();
            return conn;
        }
        /// 关闭连接
        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <param name="conn"></param>
        public void CloseSqlConnection(SqlConnection conn)
        {
            conn.Close();
            conn.Dispose();
        }
    }
}
