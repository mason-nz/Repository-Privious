using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.WorkReport
{
    public partial class WorkReportStat : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        private string txtName
        {
            get
            {
                return Request["txtName"] == null ? "" :
                HttpUtility.UrlDecode(Request["txtName"].ToString().Trim());
            }
        }

        public int PageSize = 20;
        public int GroupLength = 8;

        public string MonthFirst = string.Empty;
        public string MonthCenter = string.Empty;
        public string MonthLast = string.Empty;
        public string HtmlContent = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            int userid = BLL.Util.GetLoginUserID();
            //获取当前季度的开始时间和结束时间
            DateTime now = DateTime.Now; //当前时间
            DateTime startDate = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, 1);
            //本月第一天时间      
            DateTime monthFirst = now.AddDays(1 - (now.Day));
            //获得某年某月的天数    
            int dayCount = DateTime.DaysInMonth(now.Year, now.Month);
            //本月最后一天时间 
            DateTime endDate = monthFirst.AddDays(dayCount - 1);
            MonthFirst = startDate.Month.ToString();
            MonthCenter = startDate.AddMonths(1).Month.ToString();
            MonthLast = endDate.Month.ToString();

            //获取数据
            DataTable dt = YanFa.Crm2009.BLL.WorkReport.Instance.GetSubordinateWorkReport(userid, startDate, endDate, txtName);
            int totalCount = 0;
            if (dt != null)
            {
                //修改列名
                ChangeColumnName(startDate, endDate, dt);
                //总数
                totalCount = dt.Rows.Count;
                DataTable data = GetDataByPage(dt);
                repeaterTableList.DataSource = data;
                repeaterTableList.DataBind();
            }
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, totalCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }

        private void ChangeColumnName(DateTime startDate, DateTime endDate, DataTable dt)
        {
            //更换列名为固定列名
            foreach (DataColumn dc in dt.Columns)
            {
                if (dc.ColumnName == startDate.ToString("yyyy-MM") + "-1")
                    dc.ColumnName = "num1";
                if (dc.ColumnName == startDate.ToString("yyyy-MM") + "-2")
                    dc.ColumnName = "num2";
                if (dc.ColumnName == startDate.ToString("yyyy-MM") + "-3")
                    dc.ColumnName = "num3";

                if (dc.ColumnName == startDate.AddMonths(1).ToString("yyyy-MM") + "-1")
                    dc.ColumnName = "num4";
                if (dc.ColumnName == startDate.AddMonths(1).ToString("yyyy-MM") + "-2")
                    dc.ColumnName = "num5";
                if (dc.ColumnName == startDate.AddMonths(1).ToString("yyyy-MM") + "-3")
                    dc.ColumnName = "num6";

                if (dc.ColumnName == endDate.ToString("yyyy-MM") + "-1")
                    dc.ColumnName = "num7";
                if (dc.ColumnName == endDate.ToString("yyyy-MM") + "-2")
                    dc.ColumnName = "num8";
                if (dc.ColumnName == endDate.ToString("yyyy-MM") + "-3")
                    dc.ColumnName = "num9";
            }
        }
        private DataTable GetDataByPage(DataTable dt)
        {
            DataTable data = dt.Clone();
            int start = (BLL.PageCommon.Instance.PageIndex - 1) * PageSize;
            int end = start + PageSize - 1;
            if (end >= dt.Rows.Count)
            {
                end = dt.Rows.Count - 1;
            }
            for (int i = start; i <= end; i++)
            {
                data.ImportRow(dt.Rows[i]);
            }
            return data;
        }
    }
}