using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.Utils;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.ISDC.CC2012.Web.AjaxServers.ZuoxiManage;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Entities;
using System.Data.SqlClient;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib
{
    public partial class GradeStatisticsExport : PageBase
    {
        public string BusinessType
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("ep_BusinessType");
            }
        }
        public string StartTime
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("ep_StartTime");
            }
        }
        public string EndTime
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("ep_EndTime");
            }
        }
        public string searchType
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("SearchType");
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (EndTime == "" || StartTime == "")
                {
                    Response.Write("<script type='text/javascript'> alert('日期不能为空!');</script>");
                    Response.End();
                }
                int userID = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userID, "SYS024MOD6315"))//"质检统计管理—部门成绩明细—导出"功能验证逻辑
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                string strWhereOut = "";
                DateTime dateTimeS;
                DateTime dateTimeE;
                DataTable dt=null;
               
              
                int RecordCount = 0;

                if (searchType == "hotLine")
                {
                    string whereBGID = "";
                    string whereTime1 = "";
                    string whereTime2 = "";
                    if (!string.IsNullOrEmpty(BusinessType) && BusinessType != "-1")
                    {
                        whereBGID += " AND cri.BGID in (" + BLL.Util.SqlFilterByInCondition(BusinessType) + ")";//页面BGID
                        int userid = BLL.Util.GetLoginUserID();
                        whereBGID += " And cri.BGID in (SELECT BGID FROM UserGroupDataRigth WHERE USERID = " + userid + ") ";//数据权限           

                    }
                    if (!string.IsNullOrEmpty(StartTime) && DateTime.TryParse(StartTime, out dateTimeS))
                    {
                        whereTime1 += " AND cri.CreateTime >='" + StartTime + " 00:00:00'";
                        whereTime2 += " AND cri.StatDate >='" + StartTime + " 00:00:00'";
                    }
                    if (!string.IsNullOrEmpty(EndTime) && DateTime.TryParse(EndTime, out dateTimeE))
                    {
                        whereTime1 += " AND cri.CreateTime <='" + EndTime + " 23:59:59'";
                        whereTime2 += " AND cri.StatDate <='" + EndTime + " 23:59:59'";
                    }

                    string searchTableName = "Report_CallRecord_Day";
                    SqlConnection conn = null;
                  
                    try
                    {
                        conn = CallRecordReport.Instance.CreateSqlConnection();
                        string msg = "";

                        if (DateTime.Now.Date == Convert.ToDateTime(EndTime).Date && Convert.ToDateTime(StartTime).Date == DateTime.Now.Date)
                        {
                            Dictionary<ReportTempType, string> dic1 = CallRecordReport.Instance.CreateReportCallRecordStatForDayTmp(conn, Convert.ToDateTime(StartTime).Date, ReportTempType.Day, out msg);
                            searchTableName = dic1[ReportTempType.Day];
                        }
                        dt = BLL.QS_Result.Instance.GetQS_ResultGradeStatisticsHotLine(whereBGID, whereTime1, whereTime2, 1, -1, searchTableName, out RecordCount,conn); 
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    finally
                    {
                        CallRecordReport.Instance.CloseSqlConnection(conn);
                    }
             
                }
                else if (searchType == "onLine")
                {

                    if (!string.IsNullOrEmpty(BusinessType) && BusinessType != "-1")
                    {
                        strWhereOut += " AND BGID in (" + BLL.Util.SqlFilterByInCondition(BusinessType) + ")";
                    }
                    if (!string.IsNullOrEmpty(StartTime) && DateTime.TryParse(StartTime, out dateTimeS))
                    {
                        strWhereOut += " AND CreateTime >='" + StartTime + " 00:00:00'";
                    }
                    if (!string.IsNullOrEmpty(EndTime) && DateTime.TryParse(EndTime, out dateTimeE))
                    {
                        strWhereOut += " AND CreateTime <='" + EndTime + " 23:59:59'";
                    }
                    string tableEndName = "";//查询质检话务冗余表                    
                     dt = BLL.QS_Result.Instance.GetQS_ResultGradeStatisticsOnLine(strWhereOut, "BGID DESC", 1, -1, tableEndName, out RecordCount);
                }
             
             

                if (dt != null)
                {
                    if (searchType=="onLine")
                    {                     
                        dt.Columns.Remove("BGID");
                        dt.Columns.Remove("Score");
                        dt.Columns.Remove("RowNumber");
                        dt.Columns.Remove("t_PingfenCount");
                        dt.Columns.Remove("t_heGeCount");

                        dt.Columns["Name"].SetOrdinal(0);
                        dt.Columns["t_AgentNum"].SetOrdinal(1);
                        dt.Columns["t_DailogCount"].SetOrdinal(2);
                        dt.Columns["t_AgentDailog"].SetOrdinal(3);
                        dt.Columns["t_NetFriendDailog"].SetOrdinal(4);
                        dt.Columns["t_QS"].SetOrdinal(5);
                        dt.Columns["p_Qs"].SetOrdinal(6);
                        dt.Columns["t_Qualified"].SetOrdinal(7);
                        dt.Columns["p_Qualified"].SetOrdinal(8);
                        dt.Columns["t_AVG"].SetOrdinal(9);
                     
                       

                        dt.Columns["Name"].ColumnName = "所属分组";
                        dt.Columns["t_AgentNum"].ColumnName = "客服人数";
                        dt.Columns["t_DailogCount"].ColumnName = "总接待量";
                        dt.Columns["t_AgentDailog"].ColumnName = "客服消息发送量";
                        dt.Columns["t_NetFriendDailog"].ColumnName = "访客消息发送量";
                        dt.Columns["t_QS"].ColumnName = "抽检量";
                        dt.Columns["p_Qs"].ColumnName = "抽检率";
                        dt.Columns["t_Qualified"].ColumnName = "合格量";
                        dt.Columns["p_Qualified"].ColumnName = "合格率";
                        dt.Columns["t_AVG"].ColumnName = "平均分";
                        BLL.Util.ExportToCSV("抽查频次在线统计" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"), dt);
                    }
                    else if (searchType == "hotLine")
                    {
                        dt.Columns.Remove("RowNumber");
                        dt.Columns.Remove("BGID");
                        dt.Columns.Remove("pingFenCount");
                        dt.Columns.Remove("heGeCount");
                        dt.Columns.Remove("t_Score");
                        dt.Columns.Remove("t_Talk");
                        //dt.Columns.Remove("N_ETotal");

                        dt.Columns["Name"].SetOrdinal(0);
                        dt.Columns["t_AgentCount"].SetOrdinal(1);
                        dt.Columns["N_ETotal"].SetOrdinal(2);
                        dt.Columns["t_TallTime"].SetOrdinal(3);
                        dt.Columns["t_QS"].SetOrdinal(4);
                        dt.Columns["p_Qs"].SetOrdinal(5);                        
                        dt.Columns["t_Qualified"].SetOrdinal(6);
                        dt.Columns["p_Qualified"].SetOrdinal(7);
                        dt.Columns["t_AVG"].SetOrdinal(8);

                        dt.Columns["Name"].ColumnName = "所属分组";
                        dt.Columns["t_AgentCount"].ColumnName = "客服人数";
                        dt.Columns["N_ETotal"].ColumnName = "总通话量";
                        dt.Columns["t_TallTime"].ColumnName = "总通话时长";
                        dt.Columns["t_QS"].ColumnName = "抽检量";
                        dt.Columns["p_Qs"].ColumnName = "抽检率";
                        dt.Columns["t_Qualified"].ColumnName = "合格量";
                        dt.Columns["p_Qualified"].ColumnName = "合格率";
                        dt.Columns["t_AVG"].ColumnName = "平均分";
                        BLL.Util.ExportToCSV("抽查频次热线统计" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"), dt);
                    }
                

              
                }
            }
        }
    }
}