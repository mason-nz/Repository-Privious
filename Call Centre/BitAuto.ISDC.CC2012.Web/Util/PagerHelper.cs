using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.ISDC.CC2012.Web.Util
{/// <summary>
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