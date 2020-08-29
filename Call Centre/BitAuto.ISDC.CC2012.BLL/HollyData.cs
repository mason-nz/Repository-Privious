using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class HollyData
    {
        public static HollyData Instance = new HollyData();

        ///  创建连接
        /// <summary>
        ///  创建连接
        /// </summary>
        /// <returns></returns>
        public SqlConnection CreateHollySqlConnection()
        {
            return Dal.HollyData.Instance.CreateHollySqlConnection();
        }
        /// 关闭连接
        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <param name="conn"></param>
        public void CloseHollySqlConnection(SqlConnection conn)
        {
            Dal.HollyData.Instance.CloseHollySqlConnection(conn);
        }
        /// 创建临时表
        /// <summary>
        /// 创建临时表
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="tablename"></param>
        /// <param name="start"></param>
        public void CreateTempTable(SqlConnection conn, string tablename, long start, int maxrow, string RecordURl, string where)
        {
            Dal.HollyData.Instance.CreateTempTable(conn, tablename, start, maxrow, RecordURl, where);
        }
        /// 分页查询数据
        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="start"></param>
        /// <param name="RecordURl"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetHollyData(SqlConnection conn, string tablename, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.HollyData.Instance.GetHollyData(conn, tablename, currentPage, pageSize, out totalCount);
        }
        /// 获取总数
        /// <summary>
        /// 获取总数
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public int GetHollyDataCount(SqlConnection conn, string tablename)
        {
            return Dal.HollyData.Instance.GetHollyDataCount(conn, tablename);
        }
        /// 删除临时表
        /// <summary>
        /// 删除临时表
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public void DropTempTable(SqlConnection conn, string tablename)
        {
            Dal.HollyData.Instance.DropTempTable(conn, tablename);
        }
        /// 根据主叫号码、被叫号码，查询合力DB，最近一次外呼接通的坐席工号
        /// <summary>
        /// 根据主叫号码、被叫号码，查询合力DB，最近一次外呼接通的坐席工号
        /// </summary>
        /// <param name="callNo"></param>
        /// <param name="callOutPrefix"></param>
        /// <param name="callOutStartTime"></param>
        /// <returns></returns>
        public string GetLastAgentIDByORIDNIS(string callNo, string callOutPrefix, DateTime querytime, out string callOutStartTime)
        {
            return Dal.HollyData.Instance.GetLastAgentIDByORIDNIS(callNo, callOutPrefix, querytime, out callOutStartTime);
        }
    }
}
