using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.Controls
{
    public partial class AjaxPager : System.Web.UI.UserControl
    {
        public AjaxPager()
        {
            this.generatedCode = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
            this.functionName = "ShowDataByPost_" + this.generatedCode;
            this.refreshFunctionName = "RefreshPager_" + this.generatedCode;
        }

        #region Properties

        private bool enabled = true;
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
        }


        private string contentElementId = string.Empty;
        /// <summary>
        /// 元素块（盛放列表内容）的ID
        /// </summary>
        public string ContentElementId
        {
            get
            {
                string str = (Request["ContentElementId"] + "").ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    return str;
                }
                if (string.IsNullOrEmpty(contentElementId))
                {
                    this.enabled = false;
                    //throw new Exception("元素块（盛放列表内容）的ID不能为空");
                    Debug.Assert(true, "元素块（盛放列表内容）的ID不能为空");
                    return "";

                }
                else
                {
                    return contentElementId;
                }
            }
            set { contentElementId = value; }
        }

        private string generatedCode = string.Empty;
        /// <summary>
        /// 生成的编码（按时间）
        /// </summary>
        public string GeneratedCode
        {
            get { return generatedCode; }
        }

        private string functionName = "";
        /// <summary>
        /// 函数名
        /// </summary>
        public string FunctionName
        {
            get { return functionName; }
        }

        private string refreshFunctionName;
        /// <summary>
        /// 刷新函数名称
        /// </summary>
        public string RefreshFunctionName
        {
            get { return refreshFunctionName; }
        }




        /// <summary>
        /// 请求页的URL
        /// </summary>
        public string Url
        {
            get
            {
                if (Request.Url.Query.Trim() == "")
                {
                    return Request.Url.ToString().Trim();
                }
                else
                {
                    return Request.Url.Scheme + "://" + Request.Url.Authority + ":" + Request.Url.Port +
                    Request.Url.AbsolutePath;
                }
            }
        }

        /// <summary>
        /// QueryString(除去pageSize/currentPage)
        /// </summary>
        public string QueryString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (string k in Request.QueryString.AllKeys)
                {
                    if (k != null && k.Trim() != "" && k != "page" && k != "pageSize")
                    {
                        sb.Append("&");
                        sb.Append(k);
                        sb.Append("=");
                        sb.Append(Request.QueryString[k]);
                    }
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Form方式的请求参数(除去pageSize/currentPage)
        /// </summary>
        public string RequestData
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                //foreach (string k in Request.QueryString.AllKeys)
                //{
                //    if (k != null && k.Trim() != "" && k != "page" && k != "pageSize")
                //    {
                //        sb.Append("&");
                //        sb.Append(k);
                //        sb.Append("=");
                //        sb.Append(Request.QueryString[k]);
                //    }
                //}
                foreach (string k in Request.Form)
                {
                    if (k != null && k.Trim() != "" && k != "page" && k != "pageSize")
                    {
                        //在QueryString中参数的优先级要高于Form中的参数
                        //即在QueryString出现的参数，就不再考虑Form中的参数
                        if (Request.QueryString[k] != null) { }
                        else
                        {
                            sb.Append("&");
                            sb.Append(k);
                            sb.Append("=");
                            sb.Append(HttpUtility.UrlEncode(Request.Form[k]));
                        }
                    }
                }
                return sb.ToString();
            }
        }


        private int totalCount = 0;
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount
        {
            get { return totalCount; }
            set { totalCount = value; }
        }

        private int pageSize = -1;
        /// <summary>
        /// 页面大小
        /// </summary>
        public int PageSize
        {
            get
            {
                int ps = 20;
                string psStr = (Request["pageSize"] + "").Trim();
                if (string.IsNullOrEmpty(psStr))//没有传入参数
                {
                    if (pageSize > 0) { ps = pageSize; }
                }
                else
                {
                    int.TryParse(psStr, out ps);
                }
                return ps;

            }
            set { this.pageSize = value; }
        }

        private int currentPage = -1;
        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage
        {
            get
            {
                int cp = 1;
                string cpStr = (Request["page"] + "").Trim();
                if (string.IsNullOrEmpty(cpStr))//没有传入参数
                {
                    if (currentPage > 0) { cp = currentPage; }
                    else { return 1; }
                }
                else
                {
                    int.TryParse(cpStr, out cp);
                }

                if (cp < 1) { cp = 1; }
                if (this.Pages > 0 && cp > this.Pages) { cp = this.Pages; }
                return cp;
            }
            set { this.currentPage = value; }
        }

        private int groupLength = -1;
        /// <summary>
        /// 一组显示多少页
        /// </summary>
        public int GroupLength
        {
            get
            {
                if (groupLength > 0) { return this.groupLength; }
                else { return PageCommon.Instance.GroupLength; }
            }
            set { this.groupLength = value; }
        }


        //--------------------
        /// <summary>
        /// 总页数
        /// </summary>
        private int Pages
        {
            get { return (TotalCount / PageSize) + ((TotalCount % PageSize > 0) ? 1 : 0); }
        }

        /// <summary>
        /// 当前是第几组
        /// </summary>
        private int CurrentGroup
        {
            get
            {
                if (CurrentPage <= GroupLength) { return 1; }
                else
                {
                    return (CurrentPage / GroupLength) + ((CurrentPage % GroupLength > 0) ? 1 : 0);
                }
            }
        }

        /// <summary>
        /// 一共有几组
        /// </summary>
        private int Groups
        {
            get
            {
                if (Pages <= GroupLength) { return 1; }
                else
                {
                    return (Pages / GroupLength) + ((Pages % GroupLength > 0) ? 1 : 0);
                }
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Enabled == false) { return; }

            if (TotalCount <= 0)
            {
                this.FirstPage.Text = "首页&nbsp;";
                this.PrePage.Text = "上一页&nbsp;";
                this.List.Text = "";
                this.NextPage.Text = "下一页&nbsp;";
                this.LastPage.Text = "尾页";
                return;
            }
            else
            {
                //首页/上一页
                if (CurrentPage == 1)
                {
                    this.FirstPage.Text = "首页&nbsp;";
                    this.PrePage.Text = "上一页&nbsp;";
                }
                else
                {
                    string suffix = "pageSize=" + this.PageSize + "&page=1";
                    this.FirstPage.Text = "<a href='javascript:void(0);'  onclick='" + this.FunctionName + "(\"" + suffix + "\");" + "' class='down' title='首页'>首页</a>&nbsp;";
                    suffix = "pageSize=" + this.PageSize + "&page=" + (this.CurrentPage - 1).ToString();
                    this.PrePage.Text = "<a href='javascript:void(0);'  onclick='" + this.FunctionName + "(\"" + suffix + "\");" + "' class='down' title='上一页'>上一页</a>&nbsp;";
                }

                //页码列表
                int beginPage = (CurrentGroup - 1) * GroupLength + 1;
                int endPage = CurrentGroup * GroupLength;
                if (endPage > Pages) { endPage = Pages; }
                StringBuilder sb = new StringBuilder();
                if (CurrentGroup > 1)
                {
                    string suffix = "pageSize=" + this.PageSize + "&page=" + (beginPage - this.GroupLength).ToString();
                    sb.Append("│<a href='javascript:void(0);'  onclick='" + this.FunctionName + "(\"" + suffix + "\");" + "' class='down'  title='前" + this.GroupLength.ToString() + "页'>...</a>");
                }
                for (int i = beginPage; i <= endPage; i++)
                {
                    if (i == this.CurrentPage)
                    {
                        sb.Append("│<b>" + i.ToString() + "</b>");
                    }
                    else
                    {
                        string suffix = "pageSize=" + this.PageSize + "&page=" + i.ToString();

                        sb.Append("│<a href='javascript:void(0);'  onclick='" + this.FunctionName + "(\"" + suffix + "\");" + "' class='down'  title='第" + i.ToString() + "页'>" + i.ToString() + "</a>");
                    }
                }
                if (CurrentGroup < Groups)
                {
                    string suffix = "pageSize=" + this.PageSize + "&page=" + (endPage + 1).ToString();
                    sb.Append("│<a href='javascript:void(0);'  onclick='" + this.FunctionName + "(\"" + suffix + "\");" + "' class='down'  title='后" + this.GroupLength.ToString() + "页'>...</a>│");
                }
                else
                {
                    sb.Append("|");
                }
                this.List.Text = sb.ToString();


                //下一页/尾页
                if (CurrentPage == Pages)
                {

                    this.NextPage.Text = "下一页&nbsp;";

                    this.LastPage.Text = "尾页";
                }
                else
                {
                    string suffix = "pageSize=" + this.PageSize + "&page=" + (this.CurrentPage + 1).ToString();
                    this.NextPage.Text = "<a href='javascript:void(0);'  onclick='" + this.FunctionName + "(\"" + suffix + "\");" + "' class='down' title='下一页'>下一页</a>&nbsp;";
                    suffix = "pageSize=" + this.PageSize + "&page=" + this.Pages;
                    this.LastPage.Text = "<a href='javascript:void(0);'  onclick='" + this.FunctionName + "(\"" + suffix + "\");" + "' class='down' title='尾页'>尾页</a>&nbsp;";
                }
            }
        }

        public void InitPager(int totalCount, string contentElementId)
        {
            this.TotalCount = totalCount;
            this.ContentElementId = contentElementId;
        }

        public void InitPager(int totalCount, string contentElementId, int pageSize)
        {
            InitPager(totalCount, contentElementId);
            this.PageSize = pageSize;
        }

        public void InitPager(int totalCount, string contentElementId, int pageSize, int currentPage)
        {
            InitPager(totalCount, contentElementId, pageSize);
            this.CurrentPage = currentPage;
        }

        public void InitPager(int totalCount)
        {
            this.TotalCount = totalCount;

        }

        public void InitPager(int totalCount, int pageSize)
        {
            InitPager(totalCount);
            this.PageSize = pageSize;
        }

        public void InitPager(int totalCount, int pageSize, int currentPage)
        {
            InitPager(totalCount, pageSize);
            this.CurrentPage = currentPage;
        }
    }


    namespace BitAuto.YanFa.Crm2009.Web.Common
    {
        /// <summary>
        /// 分页通用方法，因为BLL中的分页方法只从QueryString取数据，我只好在这里再写一个方法
        /// </summary>
        public static class PagerHelper
        {
            /// <summary>
            /// 获得请求的当前页面大小(默认为20)
            /// </summary>
            public static int GetPageSize()
            {
                return PagerHelper.GetPageSize(20);
            }

            /// <summary>
            /// 获得请求的当前页面大小
            /// </summary>
            public static int GetPageSize(int defaultSize)
            {
                int pageSize = 20;
                string psStr = (HttpContext.Current.Request["pageSize"] + "").Trim();
                if (!string.IsNullOrEmpty(psStr))//传入参数
                {
                    int ps = -1;
                    if (int.TryParse(psStr, out ps)) { pageSize = ps; }
                }
                if (pageSize < 0) { pageSize = defaultSize; }//校验是否合法
                return pageSize;
            }


            /// <summary>
            /// 获得请求的当前页码(默认为1)
            /// </summary>
            public static int GetCurrentPage()
            {
                return PagerHelper.GetCurrentPage(1);
            }

            /// <summary>
            /// 获得请求的当前页码
            /// </summary>
            public static int GetCurrentPage(int defaultPage)
            {
                int currentPage = 1;
                string cpStr = (HttpContext.Current.Request["page"] + "").Trim();
                if (!string.IsNullOrEmpty(cpStr))//传入参数
                {
                    int cp = -1;
                    if (int.TryParse(cpStr, out cp)) { currentPage = cp; }
                }
                //校验是否合法
                if (currentPage < 1) { currentPage = defaultPage; }
                return currentPage;
            }
        }
    }
}