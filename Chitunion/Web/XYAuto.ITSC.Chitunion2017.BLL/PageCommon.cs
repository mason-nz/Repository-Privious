using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    public class PageCommon
    {
        #region Instance
        public static readonly PageCommon Instance = new PageCommon();
        public static Random random = new Random();
        #endregion

        #region Contructor
        protected PageCommon()
        {
        }
        #endregion
        private int m_pageIndex = 1;
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex
        {

            get
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["page"]))
                {
                    //try
                    //{
                    m_pageIndex = Convert.ToInt32(HttpContext.Current.Request.QueryString["page"]);
                    //}
                    //catch
                    //{ m_pageIndex = 1;
                    //}
                }
                else
                {
                    m_pageIndex = 1;
                }
                return m_pageIndex;
            }
            set
            {
                m_pageIndex = value;
            }
        }
        private int m_pageSize = 20;
        /// <summary>
        /// 每页显示行数
        /// </summary>
        public int PageSize
        {
            get
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["pageSize"]))
                {
                    try
                    {
                        m_pageSize = int.Parse(HttpContext.Current.Request.QueryString["pageSize"]);
                    }
                    catch
                    {
                        m_pageSize = 20;
                    }
                }
                return m_pageSize;
            }
            set
            {
                m_pageSize = value;
            }
        }

        private int m_groupLength = 10;
        /// <summary>
        /// 一组显示多少页
        /// </summary>
        public int GroupLength
        {
            get
            {
                return m_groupLength;
            }
            set
            {
                m_groupLength = value;
            }
        }
        /// <summary>
        /// 请求参数(除去pageSize/currentPage)
        /// </summary>
        public string RequestQueryData
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (string k in HttpContext.Current.Request.QueryString.AllKeys)
                {
                    if (k != null && k.Trim() != "" && k.ToLower() != "page" && k.ToLower() != "pagesize")
                    {
                        sb.Append("&");
                        sb.Append(k);
                        sb.Append("=");
                        sb.Append(Util.EscapeString(HttpContext.Current.Request.QueryString[k]));
                    }
                }
                foreach (string k in HttpContext.Current.Request.Form)
                {
                    if (k != null && k.Trim() != "" && k.ToLower() != "page" && k.ToLower() != "pagesize")
                    {
                        sb.Append("&");
                        sb.Append(k);
                        sb.Append("=");
                        sb.Append(Util.EscapeString(HttpContext.Current.Request.Form[k]));
                    }
                }
                //添加随机数
                Random random = new Random();
                sb.Append("&");
                sb.Append("random");
                sb.Append("=");
                sb.Append(random.Next().ToString());
                return sb.ToString().Trim('&');
            }
        }
        /// <summary>
        /// 获取页数，并对pageindex进行修正
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        private int GetPageCount(int totalCount, int pageSize, ref int pageIndex)
        {
            int pageCount = 20;
            if ((totalCount % pageSize) > 0)
            {
                pageCount = (totalCount / pageSize) + 1;
            }
            else
            {
                pageCount = (totalCount / pageSize);
            }
            if (pageIndex > pageCount)
            {
                pageIndex = pageCount;
            }
            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }
            return pageCount;
        }
        /// <summary>
        /// 普通分页
        /// </summary>
        /// <param name="p_FileName">传递参数</param>
        /// <param name="totalcount">总记录数</param>
        /// <returns></returns>
        public string LinkString(string parameters, int totalcount)
        {
            return LinkString(parameters, GroupLength, totalcount, PageSize, PageIndex);
        }
        /// <summary>
        /// 普通分页
        /// </summary>
        /// <param name="totalcount"></param>
        /// <returns></returns>
        public string LinkString(int totalcount)
        {
            return LinkString(HttpContext.Current.Request.Url.Query, totalcount);
        }
        /// <summary>
        /// 普通JS分页（JS方法名请注意，统一使用ShowDataByPost）
        /// </summary>
        /// <param name="parameters">传递参数</param>
        /// <param name="totalcount">总记录数</param>
        /// <param name="url">ajax请求地址</param>
        /// <param name="divID">容器ID</param>
        /// <returns></returns>
        public string LinkStringByPost(string parameters, int totalcount, int pageIndex, int type)
        {
            return LinkStringJs(parameters, GroupLength, totalcount, PageSize, pageIndex, type);
        }
        /// <summary>
        /// 所有参数自己设置的分页
        /// </summary>
        /// <param name="parameters">传递参数</param>
        /// <param name="groupLength">一组显示多少页</param>
        /// <param name="totalcount">总记录数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public string LinkString(string parameters, int groupLength, int totalcount, int pageSize)
        {
            return LinkString(parameters, groupLength, totalcount, pageSize, PageIndex);
        }
        /// <summary>
        /// 普通JS分页（JS方法名请注意，统一使用ShowDataByPost）
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="totalcount"></param>
        /// <param name="pageIndex"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string LinkStringByPost(int totalcount, int type)
        {
            return LinkStringJs(RequestQueryData, GroupLength, totalcount, PageSize, PageIndex, type);
        }
        /// <summary>
        /// 所有参数自己设置的JS分页(JS方法名请注意，统一使用ShowDataByPost)
        /// </summary>
        /// <param name="parameters">传递参数</param>
        /// <param name="groupLength">一组显示多少页</param>
        /// <param name="totalcount">总记录数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="url">ajax请求地址</param>
        /// <param name="divID">容器ID</param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public string LinkStringByPost(string parameters, int groupLength, int totalcount, int pageSize, int pageIndex, int type)
        {
            return LinkStringJs(parameters, groupLength, totalcount, pageSize, pageIndex, type);
        }
        /// <summary>
        /// 所有参数自己设置的分布
        /// </summary>
        /// <param name="parameters">传递参数</param>
        /// <param name="groupLength">一组显示多少页</param>
        /// <param name="totalcount">总记录数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示多少</param>
        /// <returns></returns>
        private string LinkString(string parameters, int groupLength, int totalcount, int pageSize, int pageIndex)
        {
            int pageCount = GetPageCount(totalcount, pageSize, ref pageIndex);
            int page = pageIndex;//当前页
            int gNum = groupLength;	//组元素数
            int recordCount = totalcount;//记录总数
            int gSum = (gNum > 0) ? (Convert.ToInt32(Math.Ceiling(pageCount * 1.0 / gNum))) : 0;//组数
            int pSum = pageCount;//总页数            
            string[] parametersList = parameters.ToLower().Split('&');
            parameters = "";
            for (int i = 0; i < parametersList.Length; i++)
            {
                if (!parametersList[i].Contains("page=") && !string.IsNullOrEmpty(parametersList[i]))
                {
                    parameters += parametersList[i] + "&";
                }
            }
            parameters = parameters.IndexOf("?") > -1 ? parameters : "?" + parameters;
            string str = "共 " + totalcount + " 项 ";

            if (recordCount == 0)
            {
                return str + "首页&nbsp;上一页&nbsp;下一页&nbsp;尾页";
            }

            if (page == 1)
            {
                str += "首页&nbsp;上一页&nbsp;";
            }
            else
            {
                str += "<a href='" + parameters + "page=1' class='down' title='首页'>首页</a>&nbsp;<a href='" + parameters + "page=" + (page - 1) + "' class='down' title='上一页'>上一页</a>&nbsp;";
            }
            str += "<span class='down'>";

            for (int j = 1; j <= gSum; j++)
            {
                if (page >= ((j - 1) * gNum + 1) && page <= (j * gNum))
                {
                    if (page > gNum)
                    {

                        str += "│<a href='" + parameters + "page=" + ((j - 2) * gNum + 1) + "' class='down'   title='前" + gNum.ToString() + "页'>...</a>";

                    }

                    for (int p = (j - 1) * gNum + 1; p <= (j * gNum); p++)
                    {
                        if (p == page)
                        {
                            str += "│<b>" + p.ToString() + "</b>";
                        }
                        else
                        {
                            if (p <= (j * gNum) && p <= pSum)
                            {

                                str += "│<a href='" + parameters + "page=" + p + "' class='down'  title='第" + p.ToString() + "页'>" + p.ToString() + "</a>";

                            }
                        }
                    }

                    if (page <= ((gSum - 1) * gNum))
                    {
                        str += "│<a href='" + parameters + "page=" + (j * gNum + 1) + "' class='down'  title='后" + gNum.ToString() + "页'>...</a>│";

                    }
                    else
                    {
                        str += "│";
                    }
                }
            }

            str += "</span>";


            if (page == pSum)
            {
                str += "&nbsp;下一页&nbsp;尾页";
            }
            else
            {
                str += "&nbsp;<a href='" + parameters + "page=" + (page + 1) + "' class='down'  title='下一页'>" + "下一页" + "</a>&nbsp;<a href='" + parameters + "page=" + pSum.ToString() + "' class='down' title='尾页'>尾页</a>";

            }
            return str;
        }
        /// <summary>
        /// 所有参数自己设置的分布
        /// </summary>
        /// <param name="parameters">传递参数</param>
        /// <param name="groupLength">一组显示多少页</param>
        /// <param name="totalcount">总记录数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示多少</param>
        /// <param name="url">ajax请求地址</param>
        /// <param name="divID">容器ID</param>
        /// <returns></returns>
        private string LinkStringJs(string parameters, int groupLength, int totalcount, int pageSize, int pageIndex, int type)
        {
            int pageCount = GetPageCount(totalcount, pageSize, ref pageIndex);
            int page = pageIndex;//当前页
            int gNum = groupLength;	//组元素数
            int recordCount = totalcount;//记录总数
            int gSum = (gNum > 0) ? (Convert.ToInt32(Math.Ceiling(pageCount * 1.0 / gNum))) : 0;//组数
            int pSum = pageCount;//总页数            
            string[] parametersList = parameters.ToLower().Split('&');
            parameters = "";
            for (int i = 0; i < parametersList.Length; i++)
            {
                if (!parametersList[i].Contains("page=") && !string.IsNullOrEmpty(parametersList[i]))
                {
                    parameters += parametersList[i] + "&";
                }
            }
            //parameters = (parameters.IndexOf("?") > -1) ? parameters : ("?" + parameters);
            parameters = parameters.TrimEnd('&') + "&";
            string str = "共 " + totalcount + " 项 ";
            if (recordCount == 0)
            {
                return str + "首页&nbsp;上一页&nbsp;下一页&nbsp;尾页";
            }

            if (page == 1)
            {
                str += "首页&nbsp;上一页&nbsp;";
            }
            else
            {

                str += "<a href='#'  onclick='ShowDataByPost" + type + "(\"" + parameters + "page=1\");' class='down' title='首页'>首页</a>&nbsp;<a href='#'  onclick='ShowDataByPost" + type + "(\"" + parameters + "page=" + (page - 1) + "\");' class='down' title='上一页'>上一页</a>&nbsp;";

            }

            str += "<span class='down'>";

            for (int j = 1; j <= gSum; j++)
            {
                if (page >= ((j - 1) * gNum + 1) && page <= (j * gNum))
                {
                    if (page > gNum)
                    {

                        str += "│<a href='#'  onclick='ShowDataByPost" + type + "(\"" + parameters + "page=" + ((j - 2) * gNum + 1) + "\");' class='down'   title='前" + gNum.ToString() + "页'>...</a>";

                    }

                    for (int p = (j - 1) * gNum + 1; p <= (j * gNum); p++)
                    {
                        if (p == page)
                        {
                            str += "│<b>" + p.ToString() + "</b>";
                        }
                        else
                        {
                            if (p <= (j * gNum) && p <= pSum)
                            {

                                str += "│<a href='#'  onclick='ShowDataByPost" + type + "(\"" + parameters + "page=" + p + "\");' class='down'  title='第" + p.ToString() + "页'>" + p.ToString() + "</a>";

                            }
                        }
                    }

                    if (page <= ((gSum - 1) * gNum))
                    {

                        str += "│<a href='#'  onclick='ShowDataByPost" + type + "(\"" + parameters + "page=" + (j * gNum + 1) + "\");' class='down'  title='后" + gNum.ToString() + "页'>...</a>│";

                    }
                    else
                    {
                        str += "│";
                    }
                }
            }

            str += "</span>";


            if (page == pSum)
            {
                str += "&nbsp;下一页&nbsp;尾页";
            }
            else
            {

                str += "&nbsp;<a href='#'  onclick='ShowDataByPost" + type + "(\"" + parameters + "page=" + (page + 1) + "\");' class='down'  title='下一页'>" + "下一页" + "</a>&nbsp;<a href='#'  onclick='ShowDataByPost" + type + "(\"" + parameters + "page=" + pSum.ToString() + "\");' class='down' title='尾页'>尾页</a>";

            }

            ////函数实现
            //string funstr = "<scr" + "ipt>function ShowDataByPost" + type + "(postBody){AjaxPost('" + url + "',postBody,null,ShowDataByPostCallBack" + type + ");function ShowDataByPostCallBack" + type + "(data){ document.getElementById('" + divID + "').innerHTML=data;}}</scri" + "pt>";
            //str += funstr;
            return str;
        }
        public DataView GetNewDataTable(DataTable dt, string condition)
        {
            DataView dv = new DataView(dt);
            dv.RowFilter = condition;
            return dv;//返回的查询结果
        }
        public DataView GetNewDataTable(DataTable dt, string condition, string orderby)
        {
            DataView dv = new DataView(dt);
            dv.RowFilter = condition;
            dv.Sort = orderby;
            return dv;//返回的查询结果
        }
        /// <summary>
        /// 获取当前页的URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetRedirectUrl(string url)
        {
            return url.IndexOf("?") > -1 ? (url + "&page=" + PageIndex + "&pagesize=" + PageSize) : (url + "?page=" + PageIndex + "&pagesize=" + PageSize);
        }
        /// <summary>
        /// 获取Request.Form
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetRequest(string name)
        {
            return HttpContext.Current.Request.Form[name] == null ? string.Empty : HttpContext.Current.Request.Form[name].ToString().Trim();
        }

    }
}
