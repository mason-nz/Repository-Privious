using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BitAuto.Utils.Data;
namespace BitAuto.DSC.APPReport2016.Dal
{
    public class Empolyee
    {

        public static Empolyee Instance = new Empolyee();

        /// <summary>
        /// 获取最大日期
        /// </summary>
        /// <param name="ItemIds"></param>
        /// <returns></returns>
        public string GetMaxDate()
        {
            string sql = "select max(YearMonth) from Employee";
      
            object obj = SqlHelper.ExecuteScalar(DataBase.ConnectionStrings, CommandType.Text, sql);

            if (obj != null)
            {
                return obj.ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取一个时间的数据
        /// </summary>
        /// <param name="YearMonth"></param>
        /// <returns></returns>
        public DataTable GetData(int YearMonth)
        {
            string sql = "select top 1 * from Employee where YearMonth<=" + YearMonth + " order by YearMonth desc";

            return SqlHelper.ExecuteDataset(DataBase.ConnectionStrings, CommandType.Text, sql).Tables[0];
        }

        /// <summary>
        /// 获取时间段的数据
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public DataTable GetData(int StartDate, int EndDate)
        {
            string sql = "select * from Employee where 1=1";
            if (StartDate>=0)
            {
                sql += " and YearMonth>='" + StartDate + "'";
            }
            if (EndDate>=0)
            {
                sql += " and YearMonth<='" + EndDate + "'";
            }

            sql += " order by YearMonth";

            return SqlHelper.ExecuteDataset(DataBase.ConnectionStrings, CommandType.Text, sql).Tables[0];
        }


    }
}
