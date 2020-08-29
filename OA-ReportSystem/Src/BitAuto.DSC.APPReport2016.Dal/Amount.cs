using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.Utils.Data;
using BitAuto.DSC.APPReport2016.Entities;
using System.Data.SqlClient;

namespace BitAuto.DSC.APPReport2016.Dal
{
    public class Amount
    {
        public static Amount Instance = new Amount();


        /// <summary>
        /// 获取最新年
        /// </summary>
        /// <returns></returns>
        public int GetLatestYear()
        {
            string nowYearMonth = DateTime.Now.ToString("yyyyMM");
            string sql = @"SELECT TOP 1 YearMonth  FROM Amount WHERE YearMonth<=" + nowYearMonth + " AND ItemId=0 ORDER BY YearMonth DESC";

            object obj = SqlHelper.ExecuteScalar(DataBase.ConnectionStrings, CommandType.Text, sql);
            if (obj is DBNull)
            {
                return DateTime.Now.Year;
            }
            return Convert.ToInt32(obj.ToString().Substring(0, 4));
        }

        /// <summary>
        /// 根据年份获取数据（获取业务收入饼图）
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DataTable GetDataByYear(int year)
        {
            int yearMonth = year * 100;
            string sql = @"  SELECT t1.ItemId,t1.Amount,t2.DictName FROM Amount t1
                             INNER JOIN DictInfo t2
                             ON t1.ItemId=t2.DictId 
                             WHERE t1.ItemId!=0 AND t2.Status=0 AND
                             YearMonth=" + yearMonth;

            DataSet ds = SqlHelper.ExecuteDataset(DataBase.ConnectionStrings, CommandType.Text, sql);

            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 检查该年是否存在数据
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public bool CheckHasDataByYear(int year)
        {
            int startYearMonth = year * 100;
            int endYearMonth = year * 100 + 12;
            string sql = @"  SELECT  count(*) FROM Amount WHERE YearMonth BETWEEN " + startYearMonth + " AND " + endYearMonth + "";

            object obj = SqlHelper.ExecuteScalar(DataBase.ConnectionStrings, CommandType.Text, sql);

            int yearData= CommonFunction.ObjectToInteger(obj);
            if (yearData==0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取总统计数（获取业务收入饼图）
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public decimal GetTotalAmountByYear(int year)
        {
            int yearMonth = year * 100;
            string sql = @"SELECT Amount FROM Amount WHERE ItemId=0  AND YearMonth=" + yearMonth;

            object obj = SqlHelper.ExecuteScalar(DataBase.ConnectionStrings, CommandType.Text, sql);
            return CommonFunction.ObjectToDecimal(obj);

        }
        /// 获取业务收入柱状图数据
        /// <summary>
        /// 获取业务收入柱状图数据
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public DataTable GetAmountBarData(int year, int itemId)
        {
            SqlParameter[] parameters ={ new SqlParameter("@year", SqlDbType.Int, 4),
                                           new SqlParameter("@itemId", SqlDbType.Int, 4)
                                       };
            parameters[0].Value = year;
            parameters[1].Value = itemId;
            return SqlHelper.ExecuteDataset(DataBase.ConnectionStrings, CommandType.StoredProcedure, "p_GetAmountBarData", parameters).Tables[0];
        }

    }
}
