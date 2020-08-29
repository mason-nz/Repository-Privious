using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities.BusinessReport;
using BitAuto.ISDC.CC2012.Web.Base;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.BusinessReport
{
    public partial class B_ProjectReportForExcel : PageBase
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
        public string StartTime
        {
            get { return BLL.Util.GetCurrentRequestStr("StartTime"); }
        }
        public string EndTime
        {
            get { return BLL.Util.GetCurrentRequestStr("EndTime"); }
        }
        public string BusinessType
        {
            get { return BLL.Util.GetCurrentRequestStr("BusinessType"); }
        }
        public string ProjectID
        {
            get { return BLL.Util.GetCurrentRequestStr("ProjectID"); }
        }
        public string Browser
        {
            get { return BLL.Util.GetCurrentRequestStr("Browser"); }
        }
        public string ProjectTime
        {
            get { return BLL.Util.GetCurrentRequestStr("ProjectTime"); }
        }

        #endregion

        public int PageSize = BLL.PageCommon.Instance.PageSize = 20;
        public int RecordCount;
        public int userID = 0;
        public int GroupLength = 8;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                if (BLL.Util.CheckRight(userID, "SYS024MOD8101"))
                {
                    DataTable dt = BindData();
                    string contentstr = GetContentstr(dt);
                    bool isie = false;
                    if (Browser == "IE")
                    {
                        isie = true;
                    }
                    writetext("项目导出" + DateTime.Now.ToString("yyyy-MM-dd"), isie, contentstr);
                }
            }
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
            QueryProjectReport model = new QueryProjectReport();
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
            if (!string.IsNullOrEmpty(ProjectID))
            {
                int _projectid = 0;
                if (int.TryParse(ProjectID, out _projectid))
                {
                    model.ProjectID = _projectid;
                }
            }
            if (!string.IsNullOrEmpty(BusinessType))
            {
                int _businesstype = 0;
                if (int.TryParse(BusinessType, out _businesstype))
                {
                    model.BusinessType = _businesstype;
                }
            }
            else
            {//默认是其他任务
                model.BusinessType = 4;
            }
            if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
            {
                DateTime _begintime = DateTime.Now;
                DateTime.TryParse(StartTime, out _begintime);
                DateTime _endtime = DateTime.Now;
                DateTime.TryParse(EndTime, out _endtime);
                model.BeginTime = _begintime;
                model.EndTime = _endtime;

            }
            else
            {
                DateTime _begintime = DateTime.Now;
                DateTime.TryParse(ProjectTime, out _begintime);
                model.BeginTime = Convert.ToDateTime(_begintime.ToString("yyyy-MM-dd"));
                model.EndTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            }
            DataTable dt = null;
            int count = 0;
            dt = BLL.ProjectInfo.Instance.GetB_ProjectReport_Excel(model, userID);
            DataTable dt1 = BLL.ProjectInfo.Instance.GetB_ProjectReportSum_Excel(model, userID);

            if (dt != null)
            {
                dt.Columns.RemoveAt(0);
                dt.Columns.RemoveAt(2);
                dt.Columns.RemoveAt(4);
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                count = dt.Rows.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    row["assigncount"] = row["assigncount"] == DBNull.Value ? "0" : row["assigncount"];
                    row["tjcount"] = row["tjcount"] == DBNull.Value ? "0" : row["tjcount"];
                    row["jtcount"] = row["jtcount"] == DBNull.Value ? "0" : row["jtcount"];
                    row["successcount"] = row["successcount"] == DBNull.Value ? "0" : row["successcount"];
                    row["jtlv"] = BLL.Util.ProduceLv(row["jtcount"].ToString(), row["tjcount"].ToString());
                    row["cglv"] = BLL.Util.ProduceLv(row["successcount"].ToString(), row["jtcount"].ToString());
                    for (int m = 9; m < dt.Columns.Count; m++)
                    {
                        row[m] = row[m] == DBNull.Value ? "0" : row[m];
                    }



                }
            }
            if (dt1 != null && dt1.Rows.Count > 0)
            {
                dt1.Columns.RemoveAt(0);
                dt1.Columns.RemoveAt(2);
                if (dt != null)
                {
                    DataRow row = dt.NewRow();
                    row["TrueName"] = "合计（共" + count + "项）";
                    row["agentnum"] = "-";
                    row["ProjectName"] = dt1.Rows[0]["projectname"].ToString();
                    row["assigncount"] = dt1.Rows[0]["assigncount"] == DBNull.Value ? "0" : dt1.Rows[0]["assigncount"];
                    row["tjcount"] = dt1.Rows[0]["tjcount"] == DBNull.Value ? "0" : dt1.Rows[0]["tjcount"];
                    row["jtcount"] = dt1.Rows[0]["jtcount"] == DBNull.Value ? "0" : dt1.Rows[0]["jtcount"];
                    row["successcount"] = dt1.Rows[0]["successcount"] == DBNull.Value ? "0" : dt1.Rows[0]["successcount"];

                    row["jtlv"] = BLL.Util.ProduceLv(row["jtcount"].ToString(), row["tjcount"].ToString());
                    row["cglv"] = BLL.Util.ProduceLv(row["successcount"].ToString(), row["jtcount"].ToString());

                    for (int m = 7; m < dt1.Columns.Count; m++)
                    {
                        dt1.Rows[0][m] = dt1.Rows[0][m] == DBNull.Value ? "0" : dt1.Rows[0][m];
                        row[m + 2] = dt1.Rows[0][m];
                    }



                    dt.Rows.Add(row);
                }
            }
            return dt;
        }
        /// <summary>
        /// 取excel导出table
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string GetContentstr(DataTable dt)
        {
            StringBuilder strContent = new StringBuilder();
            if (BusinessType == "4")
            {
                strContent.Append("其他任务");
                strContent.Append("<br/>");
                if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
                {
                    strContent.Append(Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd") + "至" + Convert.ToDateTime(EndTime).ToString("yyyy-MM-dd"));

                }
                else
                {
                    DateTime _begintime = DateTime.Now;
                    DateTime.TryParse(ProjectTime, out _begintime);
                    strContent.Append(Convert.ToDateTime(_begintime).ToString("yyyy-MM-dd") + "至" + DateTime.Now.ToString("yyyy-MM-dd"));
                }

                strContent.Append("<table  style='BORDER-COLLAPSE: collapse' borderColor=#000000 height=40 cellPadding=1 align=center border=1>");
                strContent.Append("<tr style='font-weight:bold' >");
                strContent.Append("<td rowspan=2 width=50>");
                strContent.Append("客服");
                strContent.Append("</td>");
                strContent.Append("<td rowspan=2 width=140>");
                strContent.Append("工号");
                strContent.Append("</td>");
                strContent.Append("<td rowspan=2 width=150>");
                strContent.Append("项目");
                strContent.Append("</td>");
                strContent.Append("<td rowspan=2 >");
                strContent.Append("任务分配量");
                strContent.Append("</td>");
                strContent.Append("<td rowspan=2 >");
                strContent.Append("任务提交量");
                strContent.Append("</td>");
                strContent.Append("<td rowspan=2 >");
                strContent.Append("任务接通量");
                strContent.Append("</td>");
                strContent.Append("<td rowspan=2 >");
                strContent.Append("任务接通率");
                strContent.Append("</td>");
                strContent.Append("<td rowspan=2>");
                strContent.Append("成功量");
                strContent.Append("</td>");
                strContent.Append("<td rowspan=2>");
                strContent.Append("成功率");
                strContent.Append("</td>");


                //根据项目取接通后失败量
                int _projectid = 0;
                int.TryParse(ProjectID, out _projectid);
                Dictionary<int, string> dc = BLL.ProjectInfo.Instance.GetProjectFailReason(_projectid, "NotSuccessReason");
                if (dc != null)
                {
                    strContent.Append("<td colspan=" + dc.Keys.Count + " align=center>");
                    strContent.Append("接通后失败原因");
                    strContent.Append("</td>");
                }

                Dictionary<int, string> dc1 = BLL.ProjectInfo.Instance.GetProjectFailReason(_projectid, "NotEstablishReason");
                if (dc1 != null)
                {
                    strContent.Append("<td colspan=" + dc1.Keys.Count + "  align=center>");
                    strContent.Append("未接通原因");
                    strContent.Append("</td>");
                }
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
                if (dc1 != null)
                {
                    foreach (int key in dc1.Keys)
                    {
                        strContent.Append("<td >");
                        strContent.Append(dc1[key]);
                        strContent.Append("</td>");
                    }
                }
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
            }
            else
            {
                strContent.Append("线索邀约");
                strContent.Append("<br/>");
                if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
                {
                    strContent.Append(Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd") + "至" + Convert.ToDateTime(EndTime).ToString("yyyy-MM-dd"));

                }
                else
                {
                    DateTime _begintime = DateTime.Now;
                    DateTime.TryParse(ProjectTime, out _begintime);
                    strContent.Append(Convert.ToDateTime(_begintime).ToString("yyyy-MM-dd") + "至" + DateTime.Now.ToString("yyyy-MM-dd"));
                }
                strContent.Append("<table  style='BORDER-COLLAPSE: collapse' borderColor=#000000 height=40 cellPadding=1 align=center border=1>");
                strContent.Append("<tr style='font-weight:bold' >");
                strContent.Append("<td rowspan=2 width=50>");
                strContent.Append("客服");
                strContent.Append("</td>");
                strContent.Append("<td rowspan=2 width=140>");
                strContent.Append("工号");
                strContent.Append("</td>");
                strContent.Append("<td rowspan=2 width=150>");
                strContent.Append("项目");
                strContent.Append("</td>");
                strContent.Append("<td rowspan=2 >");
                strContent.Append("任务分配量");
                strContent.Append("</td>");
                strContent.Append("<td rowspan=2 >");
                strContent.Append("任务提交量");
                strContent.Append("</td>");
                strContent.Append("<td rowspan=2 >");
                strContent.Append("任务接通量");
                strContent.Append("</td>");
                strContent.Append("<td rowspan=2>");
                strContent.Append("任务接通率");
                strContent.Append("</td>");
                strContent.Append("<td rowspan=2 >");
                strContent.Append("成功量");
                strContent.Append("</td>");
                strContent.Append("<td rowspan=2 >");
                strContent.Append("成功率");
                strContent.Append("</td>");

                strContent.Append("<td colspan=17 align=center>");
                strContent.Append("接通后失败原因");
                strContent.Append("</td>");

                strContent.Append("<td colspan=5 align=center>");
                strContent.Append("未接通原因");
                strContent.Append("</td>");

                strContent.Append("</tr>");
                strContent.Append("<tr style='font-weight:bold'>");
                strContent.Append("<td>");
                strContent.Append("用户拒绝");
                strContent.Append("</td>");
                strContent.Append("<td>");
                strContent.Append("非本人");
                strContent.Append("</td>");
                strContent.Append("<td>");
                strContent.Append("接通后挂断");
                strContent.Append("</td>");
                strContent.Append("<td>");
                strContent.Append("一个月内外呼过");
                strContent.Append("</td>");
                strContent.Append("<td>");
                strContent.Append("自行联系经销商");
                strContent.Append("</td>");
                strContent.Append("<td>");
                strContent.Append("考虑其他品牌");
                strContent.Append("</td>");
                strContent.Append("<td>");
                strContent.Append("暂无购车计划");
                strContent.Append("</td>");
                strContent.Append("<td>");
                strContent.Append("时间原因/异地订单");
                strContent.Append("</td>");
                strContent.Append("<td>");
                strContent.Append("疑似经销商");
                strContent.Append("</td>");
                strContent.Append("<td >");
                strContent.Append("需要买二手车");
                strContent.Append("</td>");
                strContent.Append("<td >");
                strContent.Append("无声/单通");
                strContent.Append("</td>");
                strContent.Append("<td >");
                strContent.Append("已购车");
                strContent.Append("</td>");
                
                strContent.Append("<td >");
                strContent.Append("开场白拒绝");
                strContent.Append("</td>");
                strContent.Append("<td >");
                strContent.Append("产品介绍拒绝");
                strContent.Append("</td>");
                strContent.Append("<td >");
                strContent.Append("购车意向度低");
                strContent.Append("</td>");
                strContent.Append("<td >");
                strContent.Append("销商已联系并满意");
                strContent.Append("</td>");

                strContent.Append("<td >");
                strContent.Append("其他");
                strContent.Append("</td>");

                DataTable dtEstablishReason = BLL.Util.GetEnumDataTable(typeof(Entities.NotEstablishReason));
                foreach (DataRow dr in dtEstablishReason.Rows)
                {
                    strContent.Append("<td >");
                    strContent.Append(dr["name"].ToString());
                    strContent.Append("</td>");
                }
                //strContent.Append("<td >");
                //strContent.Append("停机/空号/错号");
                //strContent.Append("</td>");
                //strContent.Append("<td >");
                //strContent.Append("关机/占线/无法接通");
                //strContent.Append("</td>");
                //strContent.Append("<td >");
                //strContent.Append("无人接听");
                //strContent.Append("</td>");
                //strContent.Append("<td >");
                //strContent.Append("未接通挂断");
                //strContent.Append("</td>");
                //strContent.Append("<td >");
                //strContent.Append("免打扰屏蔽");
                //strContent.Append("</td>");


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
            }
            return strContent.ToString();


        }

    }
}