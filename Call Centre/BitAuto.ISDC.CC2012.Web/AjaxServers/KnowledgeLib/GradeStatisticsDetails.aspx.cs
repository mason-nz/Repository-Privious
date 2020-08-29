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
using System.Data.SqlClient;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib
{
    public partial class GradeStatisticsDetails : PageBase
    {
        public string BusinessType
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("BusinessType");
            }
        }
        public string StartTime
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("StartTime");
            }
        }
        public string EndTime
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("EndTime");
            }
        }
        public string searchType
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("searchType");
            }
        }

        public int PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;
        private int userID = 0;


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
                if (!BLL.Util.CheckRight(userID, "SYS024MOD6305"))//"质检统计管理—部门成绩明细"功能验证逻辑
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                else
                {
                    if (searchType == "hotLine")
                    {
                        BindData();
                    }
                    else if (searchType == "onLine")
                    {
                        BindonLineData();
                    }

                }
            }
        }
        //绑定数据
        public void BindData()
        {  
            DateTime dateTimeS;
            DateTime dateTimeE;
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
            int RecordCount = 0;

            string searchTableName = "Report_CallRecord_Day";
            SqlConnection conn = null;
            DataTable dt = null;
            try
            {
                conn = CallRecordReport.Instance.CreateSqlConnection();
                string msg = "";
          
                if (DateTime.Now.Date == Convert.ToDateTime(EndTime).Date && Convert.ToDateTime(StartTime).Date == DateTime.Now.Date)
                {
                    Dictionary<ReportTempType, string> dic1 = CallRecordReport.Instance.CreateReportCallRecordStatForDayTmp(conn, Convert.ToDateTime(StartTime).Date, ReportTempType.Day, out msg);
                    searchTableName = dic1[ReportTempType.Day];
                }
                dt = BLL.QS_Result.Instance.GetQS_ResultGradeStatisticsHotLine(whereBGID, whereTime1, whereTime2, BLL.PageCommon.Instance.PageIndex, PageSize, searchTableName, out RecordCount,  conn); 
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                CallRecordReport.Instance.CloseSqlConnection(conn);
            }
          
          
            repeaterList.DataSource = dt;
            repeaterList.DataBind();

            AjaxPager.PageSize = 20;
            AjaxPager.InitPager(RecordCount);
        }

        public void BindonLineData()
        {
            string strWhereOut = "";
            DateTime dateTimeS;
            DateTime dateTimeE;
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

            int RecordCount = 0;
            string tableEndName = "";//查询质检话务冗余表
            DataTable dt = BLL.QS_Result.Instance.GetQS_ResultGradeStatisticsOnLine(strWhereOut, "BGID DESC", BLL.PageCommon.Instance.PageIndex, PageSize, tableEndName, out RecordCount);

            repeaterListHotLine.DataSource = dt;
            repeaterListHotLine.DataBind();

            AjaxPager.PageSize = 20;
            AjaxPager.InitPager(RecordCount);
        }
    }
}