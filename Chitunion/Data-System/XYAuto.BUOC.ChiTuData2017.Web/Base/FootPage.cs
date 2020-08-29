using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;

namespace XYAuto.BUOC.ChiTuData2017.Web.Base
{
    public class FootPage : Page
    {
        public static string applicationPath = HttpContext.Current.Request.ApplicationPath;
        public int pageIndex = 1;
        public int pageSize = 20;
        public int pageCount = 20;
        public int groupLength = 10;

        protected static DataView GetNewDataTable(DataTable dt, string condition)
        {
            DataView dv = new DataView(dt);
            dv.RowFilter = condition;

            return dv;//返回的查询结果
        }
        protected static DataView GetNewDataTable(DataTable dt, string condition, string orderby)
        {
            DataView dv = new DataView(dt);
            dv.RowFilter = condition;
            dv.Sort = orderby;

            return dv;//返回的查询结果
        }
        public int PageIndex
        {
            get
            {
                if (Request["page"] != null && Request["page"] != "")
                {
                    pageIndex = int.Parse(Request["page"]);
                }
                return pageIndex;
            }
            set
            {
                pageIndex = value;
            }
        }

        public int PageSize
        {
            get
            {
                if (Request["pageSize"] != null && Request["pageSize"] != "")
                {
                    pageSize = int.Parse(Request["pageSize"]);
                }
                return pageSize;
            }
            set
            {
                pageSize = value;
            }
        }

        protected int GetPageCount(int totalCount)
        {

            if ((totalCount % PageSize) > 0)
            {
                pageCount = (totalCount / PageSize) + 1;
            }
            else
            {
                pageCount = (totalCount / PageSize);
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

        public int GroupLength
        {
            get
            {
                return groupLength;
            }
            set
            {
                groupLength = value;
            }
        }

        public static string LinkString(string p_FileName, int p_GroupLength, int p_RecordCount, int p_CurrPage, int p_PageCount)
        {
            int page = p_CurrPage;//当前页
            int gNum = p_GroupLength;	//组元素数
            int recordCount = p_RecordCount;//记录总数
            int gSum = (gNum > 0) ? (Convert.ToInt32(Math.Ceiling(p_PageCount * 1.0 / gNum))) : 0;//组数
            int pSum = p_PageCount;//总页数
            string fileName = (p_FileName.IndexOf("?") > 0) ? (p_FileName + "&") : (p_FileName + "?");

            string str = "共 " + p_RecordCount + " 项 ";

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
                str += "<a href='" + fileName + "page=1' class='down' title='首页'>首页</a>&nbsp;<a href='" + fileName + "page=" + (page - 1) + "' class='down' title='上一页'>上一页</a>&nbsp;";
            }

            str += "<span class='down'>";

            for (int j = 1; j <= gSum; j++)
            {
                if (page >= ((j - 1) * gNum + 1) && page <= (j * gNum))
                {
                    if (page > gNum)
                    {
                        str += "│<a href='" + fileName + "page=" + ((j - 2) * gNum + 1) + "' class='down'   title='前" + gNum.ToString() + "页'>...</a>";
                    }

                    for (int p = (j - 1) * gNum + 1; p <= (j * gNum); p++)
                    {
                        if (p == page)
                            str += "│<b>" + p.ToString() + "</b>";
                        else
                        {
                            if (p <= (j * gNum) && p <= pSum)
                                str += "│<a href='" + fileName + "page=" + p + "' class='down'  title='第" + p.ToString() + "页'>" + p.ToString() + "</a>";

                        }
                    }

                    if (page <= ((gSum - 1) * gNum))
                    {
                        str += "│<a href='" + fileName + "page=" + (j * gNum + 1) + "' class='down'  title='后" + gNum.ToString() + "页'>...</a>│";
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
                str += "&nbsp;<a href='" + fileName + "page=" + (page + 1) + "' class='down'  title='下一页'>" + "下一页" + "</a>&nbsp;<a href='" + fileName + "page=" + pSum.ToString() + "' class='down' title='尾页'>尾页</a>";
            }

            return str;
        }
        public static string LinkStringNew(string time, int pageSize, int p_GroupLength, int p_RecordCount, int p_CurrPage, int p_PageCount)
        {
            int page = p_CurrPage;//当前页
            int gNum = p_GroupLength;	//组元素数
            int recordCount = p_RecordCount;//记录总数
            int gSum = (gNum > 0) ? (Convert.ToInt32(Math.Ceiling(p_PageCount * 1.0 / gNum))) : 0;//组数
            int pSum = p_PageCount;//总页数

            string str = "共 " + p_RecordCount + " 项 ";

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
                str += "<a href='#'  onclick=ShowTableData(" + pageSize + ",1,'" + time + "'); class='down' title='首页'>首页</a>&nbsp;<a href='#'  onclick=ShowTableData(" + pageSize + "," + (page - 1) + ",'" + time + "'); class='down' title='上一页'>上一页</a>&nbsp;";
            }

            str += "<span class='down'>";

            for (int j = 1; j <= gSum; j++)
            {
                if (page >= ((j - 1) * gNum + 1) && page <= (j * gNum))
                {
                    if (page > gNum)
                    {
                        str += "│<a href='#'  onclick=ShowTableData(" + pageSize + "," + ((j - 2) * gNum + 1) + ",'" + time + "'); class='down'   title='前" + gNum.ToString() + "页'>...</a>";
                    }

                    for (int p = (j - 1) * gNum + 1; p <= (j * gNum); p++)
                    {
                        if (p == page)
                            str += "│<b>" + p.ToString() + "</b>";
                        else
                        {
                            if (p <= (j * gNum) && p <= pSum)
                                str += "│<a href='#'  onclick=ShowTableData(" + pageSize + "," + p + ",'" + time + "'); class='down'  title='第" + p.ToString() + "页'>" + p.ToString() + "</a>";

                        }
                    }

                    if (page <= ((gSum - 1) * gNum))
                    {
                        str += "│<a href='#'  onclick=ShowTableData(" + pageSize + "," + (j * gNum + 1) + ",'" + time + "'); class='down'  title='后" + gNum.ToString() + "页'>...</a>│";
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
                str += "&nbsp;<a href='#'  onclick=ShowTableData(" + pageSize + "," + (page + 1) + ",'" + time + "'); class='down'  title='下一页'>" + "下一页" + "</a>&nbsp;<a href='#'  onclick=ShowTableData(" + pageSize + "," + pSum.ToString() + ",'" + time + "'); class='down' title='尾页'>尾页</a>";
            }

            return str;
        }


    }
}