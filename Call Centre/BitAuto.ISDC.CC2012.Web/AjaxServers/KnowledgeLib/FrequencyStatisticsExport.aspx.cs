using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib
{
    public partial class FrequencyStatisticsExport : PageBase
    {
        public string ScoreCreater
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("ep_ScoreCreater").ToString();
            }
        }
        public string ScoreBeginTime
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("ep_ScoreBeginTime").ToString();
            }
        }
        public string ScoreEndTime
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("ep_ScoreEndTime").ToString();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userId = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userId, "SYS024MOD6314"))//"质检统计管理—抽查频次统计—导出"功能验证逻辑
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }

                Entities.QueryCallRecordInfo query = new Entities.QueryCallRecordInfo();

                query.LoginID = BLL.Util.GetLoginUserID(); ;
                if (!string.IsNullOrEmpty(ScoreBeginTime))
                {
                    query.ScoreBeginTime = ScoreBeginTime;
                }
                if (!string.IsNullOrEmpty(ScoreEndTime))
                {
                    query.ScoreEndTime = ScoreEndTime;
                }
                if (!string.IsNullOrEmpty(ScoreCreater))
                {
                    //query.ScoreCreater = int.Parse(ScoreCreater);
                    query.QSScoreCreaters = " and b.UserID IN (" + BLL.Util.SqlFilterByInCondition(ScoreCreater) + ")";
                }
                query.QSResultStatus = "20002,20003,20004,20005,20006";
                query.IsFilterNull = 1;
                int RecordCount = 0;

                string tableEndName = "_QS";//查询质检话务冗余表
                DataTable dt = BLL.QS_Result.Instance.GetQS_ResultFrequenyStatistics(query, "tCount desc", 1, -1, tableEndName, out RecordCount);

                if (dt != null)
                {
                    dt.Columns.Remove("RowNumber");
                    dt.Columns.Remove("CreateUserID");

                    dt.Columns["ScoreName"].ColumnName = "评分人";
                    dt.Columns["tCount"].ColumnName = "评分次数";
                    dt.Columns["col1"].ColumnName = "60秒";
                    dt.Columns["col2"].ColumnName = "61-90秒";
                    dt.Columns["col3"].ColumnName = "91-120秒";
                    dt.Columns["col4"].ColumnName = "121-150秒";
                    dt.Columns["col5"].ColumnName = "151-180秒";
                    dt.Columns["col6"].ColumnName = "181-210秒";
                    dt.Columns["col7"].ColumnName = "211-240秒";
                    dt.Columns["col8"].ColumnName = "241-270秒";
                    dt.Columns["col9"].ColumnName = "271-600秒";
                    dt.Columns["col10"].ColumnName = "600以上";

                    BLL.Util.ExportToCSV("抽查频次统计" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"), dt);
                }
            }
        }
    }
}