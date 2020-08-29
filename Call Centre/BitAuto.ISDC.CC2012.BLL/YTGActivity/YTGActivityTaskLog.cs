using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class YTGActivityTaskLog : CommonBll
    {
        private YTGActivityTaskLog() { }
        private static YTGActivityTaskLog _instance = null;
        public static new YTGActivityTaskLog Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new YTGActivityTaskLog();
                }
                return _instance;
            }
        }
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetYTGActivityTaskOperationLog(YTGActivityTaskLogInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.YTGActivityTaskLog.Instance.GetYTGActivityTaskOperationLog(query, order, currentPage, pageSize, out totalCount);
        }
    }
}
