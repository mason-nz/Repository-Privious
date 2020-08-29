using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.ISDC.CC2012.Dal;

namespace BitAuto.ISDC.CC2012.BLL
{
    /// <summary>
    /// 西门子报表模块访问Oracle库
    /// qiangfei
    /// 2014年7月9日
    /// </summary>
    public class ReportOracleBLL
    {
        private ReportOracleBLL()
        {
        }
        private static ReportOracleBLL instance = null;
        public static ReportOracleBLL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ReportOracleBLL();
                }
                return instance;
            }
        }

        #region 查询数据
        /// 查询简单sql
        /// <summary>
        /// 查询简单sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql)
        {
            return ReportOracleDAL.Instance.GetDataTable(sql);
        }
        /// 分页查询
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql, int pageIndex, int pageSize, out int total)
        {
            return ReportOracleDAL.Instance.GetDataTable(sql, pageIndex, pageSize, out total);
        }
        /// 查询总数
        /// <summary>
        /// 查询总数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int GetDataTableCount(string sql)
        {
            return ReportOracleDAL.Instance.GetDataTableCount(sql);
        }
        #endregion
    }
}
