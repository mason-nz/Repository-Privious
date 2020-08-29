using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;

namespace BitAuto.ISDC.CC2012.BLL
{
    /// 黑白名单日志类
    /// <summary>
    /// 黑白名单日志类
    /// </summary>
    public class BlackWhiteListOperLog
    {
        public static BlackWhiteListOperLog Instance = new BlackWhiteListOperLog();

        /// <summary>
        /// 根据条件取黑白名单操作日志
        /// </summary>
        /// <returns></returns>
        public DataTable GetBlackWhiteListOperLog(QueryBlackWhiteListOperLog query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.BlackWhiteListOperLog.Instance.GetBlackWhiteListOperLog(query, order, currentPage, pageSize, out totalCount);
        }
    }
}
