using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Dal
{
    /// <summary>
    /// 西门子报表模块访问Oracle库
    /// qiangfei
    /// 2014年7月9日
    /// </summary>
    public class ReportOracleDAL
    {
        private ReportOracleDAL()
        {
        }
        private static ReportOracleDAL instance = null;
        public static ReportOracleDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ReportOracleDAL();
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
            using (OracleDataBase db = new OracleDataBase())
            {
                return db.ExecuteDataTable(sql);
            }
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
            using (OracleDataBase db = new OracleDataBase())
            {
                return db.ExecuteDataTable(sql, pageIndex, pageSize, out total);
            }
        }
        /// 查询总数
        /// <summary>
        /// 查询总数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int GetDataTableCount(string sql)
        {
            using (OracleDataBase db = new OracleDataBase())
            {
                return db.ExecuteDataTableCount(sql, null);
            }
        }
        #endregion
    }
}
