using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;
namespace BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib
{
    public partial class FAQListFont : PageBase
    {
        #region 属性
        private string RequestKeywords
        {
            get
            {
                return HttpContext.Current.Request["Keywords"] == null
                ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Keywords"].ToString());
            }
        }
        /// <summary>
        /// 知识点分类ID
        /// </summary>
        private string RequestKCID
        {
            get
            {
                return HttpContext.Current.Request["KCID"] == null
                    ? "-1" : HttpUtility.UrlDecode(HttpContext.Current.Request["KCID"].ToString());
            }
        }

        private string Oreder
        {
            get
            {
                return HttpContext.Current.Request["od"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["od"].ToString());

            }
        }
        private string RequestKCPID
        {
            get { return HttpContext.Current.Request["KCPID"] == null ? "-1" : HttpUtility.UrlDecode(HttpContext.Current.Request["KCPID"].ToString()); }
        }
        #endregion
        public int RecordCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userID = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userID, "SYS024MOD3002"))//"知识&培训—FAQ"功能验证逻辑
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                else
                {
                    BindData();
                }
            }
        }

        public void BindData()
        {
            #region 添加搜索关键字
            if (RequestKeywords != "")
            {

                Entities.SearchLog searchLog = new Entities.SearchLog();
                searchLog.SearchKey = RequestKeywords;
                searchLog.Type = 2;
                searchLog.CreateTime = DateTime.Now;
                searchLog.CreateUserID = BLL.Util.GetLoginUserID();
                BLL.SearchLog.Instance.Insert(searchLog);

            }
            #endregion

            #region 整理Where

            StringBuilder sb = new StringBuilder();

            //sb.Append(BLL.UserGroupDataRigth.Instance.GetSqlRightstr("a", "BGID", "CreateUserID", BLL.Util.GetLoginUserID()));

            if (RequestKCPID != "-1")
            {
                sb.Append(" and c.ppid=");
                sb.Append(RequestKCPID.ToString());
            }
            if (RequestKCID != "-1")
            {
                sb.Append(" and c.kcid=");
                sb.Append(RequestKCID.ToString());
            }

            if (!string.IsNullOrEmpty(RequestKeywords))
            {
                var keyWorsArray = RequestKeywords.Split(' ').Where(s => !string.IsNullOrEmpty(s)).ToArray();
                if (keyWorsArray.Length > 0)
                {
                    sb.Append(" and (");
                    foreach (string s in keyWorsArray)
                    {
                        sb.Append(string.Format(" a.Ask like '%{0}%' OR a.Question like '%{0}%' or", BitAuto.Utils.StringHelper.SqlFilter(s)));
                    }

                    sb.Remove(sb.Length - 3, 3);

                    sb.Append(") ");

                }
            }
            var orderQuery = Oreder;
            if (string.IsNullOrEmpty(orderQuery))
            {
                orderQuery = "a.CreateTime desc";
            }
            #endregion

            DataSet ds = BLL.KLFAQ.Instance.GetKLFAQReport(BLL.Util.GetLoginUserID(), orderQuery, sb.ToString(), BLL.PageCommon.Instance.PageIndex, 10);


            sb = new StringBuilder();
            DataTable dt = ds.Tables[0];
            if (dt == null || dt.Rows.Count < 1) { return; }
            DataRow[] arrayDR = dt.Select("flag=1");
            DataRow[] drT;
            string idT = "";
            try
            {
                RecordCount = Convert.ToInt32(dt.Select("flag=2")[0]["num"]);
            }
            catch
            {
                RecordCount = 0;
            }

            int i = 0;
            foreach (DataRow dr in arrayDR) //flag=1
            {
                if (i == 0)
                {
                    sb.Append("<ul>");

                    sb.Append(" <li>全部（" + RecordCount.ToString() + "）</li>");
                }
                else if (i % 7 == 0)
                {
                    sb.Append("</ul>");
                    if (i == 7)
                    {
                        sb.Append("<span class='more dnowup' id='btnMore'><a href='#'>更多</a></span>");
                    }
                    sb.Append("<ul style='display: none;'><li></li>");
                }


                idT = dr["ppid"].ToString();
                sb.Append(string.Format("<li><a  class='aC' href='javascript:void(0)' lev='1' did='{0}'>{1}({2})</a>", idT, dr["ppname"].ToString(), dr["num"]));
                drT = dt.Select("ppid=" + idT + " and flag=0");
                if (drT.Length > 0)
                {
                    sb.Append("<div class='arrow' style='display: none;'></div><ul style='display: none;'>");

                    foreach (DataRow dr2 in drT)
                    {
                        sb.Append(string.Format("<li><a  class='aC' href='javascript:void(0)' lev='2' did='{0}'>{1}({2})</a></li>", dr2["KCID"], dr2["kcname"], dr2["num"]));
                    }
                    sb.Append("</ul>");
                }
                sb.Append("</li>");
                i++;

            }

            sb.Append("</ul>");
            lbKLC.Text = sb.ToString();

            Rt_FAQ.DataSource = ds.Tables[1];
            Rt_FAQ.DataBind();
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 8, RecordCount, 10, BLL.PageCommon.Instance.PageIndex, 1);

        }

        #region 数据列表绑定
        public void BindData_old()
        {
            Entities.QueryKLFAQ query = new QueryKLFAQ();
            if (RequestKeywords != "")
            {
                query.Keywords = RequestKeywords;
                #region 添加搜索关键字
                Entities.SearchLog searchLog = new Entities.SearchLog();
                searchLog.SearchKey = RequestKeywords;
                searchLog.Type = 2;
                searchLog.CreateTime = DateTime.Now;
                searchLog.CreateUserID = BLL.Util.GetLoginUserID();
                BLL.SearchLog.Instance.Insert(searchLog);
                #endregion
            }
            if (RequestKCID != "")
            {
                query.KCIDs = RequestKCID;
            }
            query.State = "2";
            int RecordCount = 0;
            DataTable dt = BLL.KLFAQ.Instance.GetKLFAQ(query, "KLFAQ.CreateTime DESC", BLL.PageCommon.Instance.PageIndex, 10, out RecordCount);
            Rt_FAQ.DataSource = dt;
            Rt_FAQ.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 10, RecordCount, 10, BLL.PageCommon.Instance.PageIndex, 1);
        }
        #endregion

        public string CheckGlobleHidden(string lbStatus, string klid, string title, string clickcount, string downloadcount)
        {
            if (lbStatus == "2")
            {

                return string.Format(
                        "<span>关联知识点：<a name='kl' href='/KnowledgeLib/KnowledgeViewForUsers.aspx?kid={0}' target='_blank'>{1} </a></span> <span style='margin-right:8px;'>点击次数：<em class='clkCount'>{2}</em>次 </span>  下载：{3}次</span>",
                        klid, title, clickcount, downloadcount);
            }
            else
            {
                return "";
            }
            /*
             <span>关联知识点：<a name="kl" href="/KnowledgeLib/KnowledgeViewForUsers.aspx?kid=<%#Eval("KLID") %>"
                                target="_blank"><%#Eval("Title") %></a></span> <span>点击次数：<%#Eval("ClickCount").ToString()%>次
                                    下载<%#Eval("DownLoadCount")%>次</span>
                            
             */
        }

        protected void rpt_FAQItemBound(object sender, RepeaterItemEventArgs e)
        {
            /*
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int klId = int.Parse(DataBinder.Eval(e.Item.DataItem, "KLID").ToString().Trim());

                Repeater rOpt = e.Item.FindControl("rptFiles") as Repeater;
                Entities.QueryKLUploadFile query = new QueryKLUploadFile();
                query.KLID = klId;
                int totalCount = 0;
                rOpt.DataSource = BLL.KLUploadFile.Instance.GetKLUploadFile(query, "", 1, 20, out totalCount);
                rOpt.DataBind();
                if (totalCount > 0)
                {
                    Literal ltrFile = e.Item.FindControl("ltrFile") as Literal;
                    ltrFile.Text = "附件：";
                }
            }
            */
        }
    }
}