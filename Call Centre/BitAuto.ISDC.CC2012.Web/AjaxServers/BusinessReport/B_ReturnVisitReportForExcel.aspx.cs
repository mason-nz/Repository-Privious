using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;
using System.Data;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.BusinessReport;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.BusinessReport
{
    public partial class B_ReturnVisitReportForExcel : PageBase
    {
        #region
        public string AgentID  //即UserID
        {
            get { return BLL.Util.GetCurrentRequestStr("AgentID"); }
        }
        public string AgentNum
        {
            get { return BLL.Util.GetCurrentRequestStr("AgentNum"); }
        }
        public string Year
        {
            get { return BLL.Util.GetCurrentRequestStr("Year"); }
        }
        public string Month
        {
            get { return BLL.Util.GetCurrentRequestStr("Month"); }
        }
        public string BGID
        {
            get { return BLL.Util.GetCurrentRequestStr("BGID"); }
        }
        public string Browser
        {
            get { return BLL.Util.GetCurrentRequestStr("Browser"); }
        }
        #endregion
        private int userID = 0;
        private DateTime begintime;
        private DateTime endtime;
        protected void Page_Load(object sender, EventArgs e)
        {
            userID = BLL.Util.GetLoginUserID();

            if (BLL.Util.CheckRight(userID, "SYS024MOD8201"))
            {
                DateTime timestr = Convert.ToDateTime(Year.ToString() + "-" + Month.ToString() + "-10");
                begintime = FirstDayOfMonth(timestr);
                endtime = LastDayOfMonth(timestr);

                DataTable dt = BindData();
                string contentstr = GetContentstr(dt);
                bool isie = false;
                if (Browser == "IE")
                {
                    isie = true;
                }
                writetext("客户回访导出" + DateTime.Now.ToString("yyyy-MM-dd"), isie, contentstr);
            }
        }
        /// 取得某月的第一天
        /// </summary>
        /// <param name="datetime">要取得月份第一天的时间</param>
        /// <returns></returns>
        private DateTime FirstDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day);
        }
        /**/
        /// <summary>
        /// 取得某月的最后一天
        /// </summary>
        /// <param name="datetime">要取得月份最后一天的时间</param>
        /// <returns></returns>
        private DateTime LastDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(1).AddDays(-1);
        }
        /// <summary>
        /// 取excel导出table
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string GetContentstr(DataTable dt)
        {
            StringBuilder strContent = new StringBuilder();
            strContent.Append(Convert.ToDateTime(begintime).ToString("yyyy-MM-dd") + "至" + Convert.ToDateTime(endtime).ToString("yyyy-MM-dd"));
            strContent.Append("<table  style='BORDER-COLLAPSE: collapse' borderColor=#000000 height=40 cellPadding=1 align=center border=1>");
            strContent.Append("<tr style='font-weight:bold' >");
            strContent.Append("<td rowspan=2 width=100>");
            strContent.Append("客服");
            strContent.Append("</td>");
            strContent.Append("<td rowspan=2>");
            strContent.Append("工号");
            strContent.Append("</td>");
            strContent.Append("<td rowspan=2>");
            strContent.Append("当月负责会员数");
            strContent.Append("</td>");
            strContent.Append("<td rowspan=2 >");
            strContent.Append("本月已回访会员数");
            strContent.Append("</td>");
            strContent.Append("<td rowspan=2 >");
            strContent.Append("覆盖率");
            strContent.Append("</td>");



            //根据项目取接通后失败量
            Dictionary<int, string> dc = BLL.ProjectInfo.Instance.GetReturnVisitCategory();
            if (dc != null)
            {
                strContent.Append("<td colspan=" + dc.Keys.Count + " align=center>");
                strContent.Append("回访记录数");
                strContent.Append("</td>");
            }

            strContent.Append("<td colspan=4 align=center>");
            strContent.Append("未接通量");
            strContent.Append("</td>");
            strContent.Append("</tr>");
            strContent.Append("<tr style='font-weight:bold'>");
            if (dc != null)
            {
                foreach (int key in dc.Keys)
                {
                    strContent.Append("<td >");
                    strContent.Append(dc[key]);
                    strContent.Append("</td>");
                }
            }
            strContent.Append("<td >");
            strContent.Append("停机/空号/错号");
            strContent.Append("</td>");
            strContent.Append("<td >");
            strContent.Append("关机/占线/无法接通");
            strContent.Append("</td>");
            strContent.Append("<td >");
            strContent.Append("无人接听");
            strContent.Append("</td>");
            strContent.Append("<td >");
            strContent.Append("未接通挂断");
            strContent.Append("</td>");
            strContent.Append("</tr>");
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strContent.Append("<tr>");
                    DataRow dr = dt.Rows[i];
                    if (dr != null)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            strContent.Append("<td>");
                            strContent.Append(dr[j].ToString());
                            strContent.Append("</td>");
                        }
                        strContent.Append("</tr>");
                    }
                }
            }
            strContent.Append("</table>");
            return strContent.ToString();


        }
        private void writetext(string TrueName, bool IsIE, string strContent)
        {
            System.Web.HttpResponse Response = Page.Response;
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "UTF-8";
            if (IsIE)
            {
                Response.AddHeader("Content-Disposition", "inline;filename=\"" + System.Web.HttpUtility.UrlEncode(TrueName, System.Text.Encoding.UTF8) + ".xls\"");
            }
            else
            {
                Response.AddHeader("Content-Disposition", "inline;filename=\"" + TrueName + ".xls\"");
            }
            //Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "application/vnd.ms-excel";
            Page.EnableViewState = false;
            Response.Write(strContent);
            Response.Flush();
            Response.End();
        }
        private DataTable BindData()
        {
            QueryReturnVisitReport model = new QueryReturnVisitReport();
            if (!string.IsNullOrEmpty(AgentID))
            {
                int _userid = 0;
                if (int.TryParse(AgentID, out _userid))
                {
                    model.UserID = _userid;
                }
            }
            if (!string.IsNullOrEmpty(AgentNum))
            {
                model.AgentNum = AgentNum;
            }
            if (!string.IsNullOrEmpty(BGID))
            {
                int _bgid = 0;
                if (int.TryParse(BGID, out _bgid))
                {
                    model.BGID = _bgid;
                }
            }
            if (!string.IsNullOrEmpty(Year))
            {
                int _year = 0;
                if (int.TryParse(Year, out _year))
                {
                    model.Year = _year;
                }
            }
            if (!string.IsNullOrEmpty(Month))
            {
                int _month = 0;
                if (int.TryParse(Month, out _month))
                {
                    model.Month = _month;
                }
            }
            DataTable dt = null;
            int count = 0;
            dt = BLL.ProjectInfo.Instance.GetB_ReturnVisitReport_ForExcel(model);
            //DataTable dt1 = BLL.ProjectInfo.Instance.GetB_ReturnVisitReportSum_ForExcel(model);

            int sumdyfzmembercount = 0;
            int sumhfmembercount = 0;

            if (dt != null && dt.Rows.Count > 0)
            {
                int[] a = new int[dt.Columns.Count - 5];
                count = dt.Rows.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    row["dyfzmembercount"] = row["dyfzmembercount"] == DBNull.Value ? "0" : row["dyfzmembercount"];
                    row["hfmembercount"] = row["hfmembercount"] == DBNull.Value ? "0" : row["hfmembercount"];

                    sumdyfzmembercount += int.Parse(row["dyfzmembercount"].ToString());
                    sumhfmembercount += int.Parse(row["hfmembercount"].ToString());

                    row["fglv"] = BLL.Util.ProduceLv(row["hfmembercount"].ToString(), row["dyfzmembercount"].ToString());
                    for (int m = 5; m < dt.Columns.Count; m++)
                    {
                        row[m] = row[m] == DBNull.Value ? "0" : row[m];
                        a[m - 5] += int.Parse(row[m].ToString());
                    }
                }
                DataRow row1 = dt.NewRow();
                row1["TrueName"] = "合计（共" + count + "项）";
                row1["agentnum"] = "-";
                row1["dyfzmembercount"] = sumdyfzmembercount;
                row1["hfmembercount"] = sumhfmembercount;
                row1["fglv"] = BLL.Util.ProduceLv(sumhfmembercount.ToString(), sumdyfzmembercount.ToString());
                for (int m = 0; m < dt.Columns.Count - 5; m++)
                {
                    row1[m + 5] = a[m];
                }
                dt.Rows.Add(row1);
            }

            return dt;

        }

    }
}