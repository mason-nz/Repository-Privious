using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib
{
    public partial class Knowledge : PageBase
    {
        #region 属性
        private string RequestKeywords
        {
            get { return HttpContext.Current.Request["Keywords"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Keywords"].ToString()); }
        }
        private string RequestKCID
        {
            get { return HttpContext.Current.Request["KCID"] == null ? "-1" : HttpUtility.UrlDecode(HttpContext.Current.Request["KCID"].ToString()); }
        }
        private string RequestKCPID
        {
            get { return HttpContext.Current.Request["KCPID"] == null ? "-1" : HttpUtility.UrlDecode(HttpContext.Current.Request["KCPID"].ToString()); }
        }
        private int RequestPageSize
        {
            get { return HttpContext.Current.Request["PageSize"] == null ? 8 : Convert.ToInt32(HttpUtility.UrlDecode(HttpContext.Current.Request["PageSize"].ToString())); }
        }
        private int RequestIndex
        {
            get { return HttpContext.Current.Request["page"] == null ? 1 : Convert.ToInt32(HttpUtility.UrlDecode(HttpContext.Current.Request["page"].ToString())); }
        }
        private string RequestUnRead
        {
            get { return HttpContext.Current.Request["UnRead"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["UnRead"].ToString()); }
        }

        private string Oreder
        {
            get
            {
                return HttpContext.Current.Request["od"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["od"].ToString());

            }
        }
        private string asds
        {
            get { return HttpContext.Current.Request["asds"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["asds"].ToString()); }
        }
        private bool isUnread
        {
            get
            {
                try
                {
                    return (HttpContext.Current.Request["isUnread"].ToString() == "1");

                }
                catch (Exception)
                {

                    return false;
                }
            }
        }
        #endregion

        public int PageSize = BLL.PageCommon.Instance.PageSize = 10;
        public int GroupLength = 8;
        public int RecordCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userID = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userID, "SYS024MOD3001"))//"知识&培训—知识点"功能验证逻辑
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                else
                {
                    BindData2(Convert.ToInt32(RequestKCPID), Convert.ToInt32(RequestKCID), RequestKeywords, isUnread);
                }
            }
        }

        public void BindData2(int kcpid, int kcid, string kw, bool isUnRead)
        {
      
            int unReadeCount = 0;
            DataSet ds = BLL.KnowledgeLib.Instance.GetKnowledgeReport(BLL.Util.GetLoginUserID(), RequestIndex, RequestPageSize, out unReadeCount, kcpid, kcid, kw, Oreder,asds, Convert.ToBoolean(isUnread));

            StringBuilder sb = new StringBuilder();
            //生成类别       
            DataTable dt = ds.Tables[0];
            if (dt == null || dt.Rows.Count < 1)
            {
                return;
            }
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

            ltUnreadCount.Text = unReadeCount.ToString();
            int i = 0;
            foreach (DataRow dr in arrayDR) //flag=1
            {
                if (i == 0)
                {
                    sb.Append("<ul>");
                    //drT = dt.Select("flag=2");
                    //sb.Append(" <li><a href='#'>全部</a><a href='#'>（" + RecordCount.ToString() + "）</a></li>");
                    sb.Append(" <li>全部（" + RecordCount.ToString() + "）</li>");
                }
                else if (i % 7 == 0)
                {
                    sb.Append("</ul>");
                    if (i == 7)
                    {
                        sb.Append("<span class='more dnowup' id='btnMore'><a href='javascript:void(0)'>更多</a></span>");
                    }
                    sb.Append("<ul style='display: none;'><li></li>");
                }


                idT = dr["ppid"].ToString();
                sb.Append(string.Format("<li><a class='aC' href='javascript:void(0)' lev='1' did='{0}'>{1}({2})</a>", idT, dr["ppname"].ToString(), dr["num"]));
                drT = dt.Select("ppid=" + idT + " and flag=0");
                if (drT.Length > 0)
                {
                    sb.Append("<div class='arrow' style='display: none;'></div><ul style='display: none;'>");

                    foreach (DataRow dr2 in drT)
                    {
                        sb.Append(string.Format("<li><a class='aC' href='javascript:void(0)' lev='2' did='{0}'>{1}({2})</a></li>", dr2["KCID"], dr2["kcname"], dr2["num"]));
                    }
                    sb.Append("</ul>");
                }
                sb.Append("</li>");
                i++;

            }

            sb.Append("</ul>");
            lbKLC.Text = sb.ToString();

            repeaterTableList.DataSource = ds.Tables[1];
            repeaterTableList.DataBind();
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }


        //old logic
        public void BindData()
        {
            Entities.QueryKnowledgeLib query = new Entities.QueryKnowledgeLib();
            if (RequestKeywords != "")
            {
                query.Keywords = RequestKeywords;
            }
            if (RequestKCID != "")
            {
                query.KCIDS = RequestKCID;
            }
            query.Content = "";     //这边为空，后台控制是 内容Content!=''
            query.Status = 2;   //知识点状态为审核通过

            //获取 全部 的总数
            int allCount;
            BLL.KnowledgeLib.Instance.GetKLIDAllCount(query, out allCount);
            hidRecordCount.Value = allCount.ToString();  //全部

            string str = string.Empty;
            if (RequestUnRead != "")
            {
                query.UnRead = "";
                query.UserID = BLL.Util.GetLoginUserID();
                str = "1";
            }

            DataTable dt = BLL.KnowledgeLib.Instance.GetKnowledgeLib(query, " CreateTime DESC", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

            int unReadCount;    //未读总数
            query.UnRead = "";
            query.UserID = BLL.Util.GetLoginUserID();
            DataTable dt_UnRead = BLL.KnowledgeLib.Instance.GetKnowledgeLib(query, " CreateTime DESC", BLL.PageCommon.Instance.PageIndex, PageSize, out unReadCount);
            //BLL.KnowledgeLib.Instance.GetKLIDAllCount(query, out unReadCount);
            // hidUnRead.Value = unReadCount.ToString();
            //获取全部table的ID 和 未读table的ID，如果全部table的ID不在未读table里面，则是已读，字体控制为正常，不需要加载未读小图标
            string[] allKLID = new string[dt.Rows.Count];
            string unReadKLID = string.Empty;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                allKLID[i] = dt.Rows[i]["KLID"].ToString();
            }
            for (int i = 0; i < dt_UnRead.Rows.Count; i++)
            {
                unReadKLID += dt_UnRead.Rows[i]["KLID"].ToString() + ",";
            }
            for (int i = 0; i < allKLID.Length; i++)
            {
                if (!unReadKLID.Contains(allKLID[i]))
                {
                    hidImgID.Value += allKLID[i] + ",";
                }
            }
            hidImgID.Value = hidImgID.Value.TrimEnd(',');

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);

        }

        //分类
        public string getCategory(string kcid)
        {
            string categoryStr = string.Empty;
            int _kcid;
            if (int.TryParse(kcid, out _kcid))
            {
                Entities.KnowledgeCategory model = BLL.KnowledgeCategory.Instance.GetKnowledgeCategory(_kcid);
                if (model != null)
                {
                    categoryStr = model.Name;
                }
            }
            return categoryStr;
        }

        public string BindImg(string nRead, string kid)
        {
            if (nRead == "1")
            {
                return "<img src='../Css/img/unread.png' id='ig" + kid + "' vl='" + kid + "' />";
            }
            return "";
        }

        //标题字数控制
        public string getTitle(string title)
        {
            string Title = string.Empty;

            if (title.Length > 40)
            {
                Title = title.Substring(0, 40) + "......";
            }
            else
            {
                Title = title;
            }

            return Title;
        }
        //内容字数控制
        public string getContent(string content)
        {
            string Content = string.Empty;

            if (content.Length > 200)
            {
                Content = content.Substring(0, 200) + "......";
            }
            else
            {
                Content = content;
            }

            return Content;
        }
    }
}