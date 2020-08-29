
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BitAuto.Utils.Data;
using System.Data;

namespace BitAuto.DSC.APPReport2016.Dal
{
    /// <summary>
    /// 数据访问类:Member
    /// </summary>
    public partial class Member
    {

        public static Member Instance = new Member();

        public Member()
        { }


        /// 获取最大日期
        /// <summary>
        /// 获取最大日期
        /// </summary>
        /// <param name="ItemIds"></param>
        /// <returns></returns>
        public string GetMaxDate(string ItemIds = "")
        {
            string sql = "select MAX(YearMonth) from Member where 1=1";
            if (!string.IsNullOrEmpty(ItemIds))
            {
                sql += " and itemid in (" + Utils.StringHelper.SqlFilter(ItemIds) + ")";
            }

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
        /// 获取某段时间内的会员数量
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="ItemIds">会员类型</param>
        /// <returns></returns>
        public DataTable GetData(int startDate, int endDate, string ItemIds)
        {
            string sql = "";
            sql = @"select m.*,d.DictName
                from Member m inner join  DictInfo d on m.ItemId=d.DictId
                where YearMonth>=" + startDate + " and yearmonth<=" + endDate;

            if (!string.IsNullOrEmpty(ItemIds))
            {
                sql += " and itemid in (" + Utils.StringHelper.SqlFilter(ItemIds) + ")";
            }

            DataSet ds = SqlHelper.ExecuteDataset(DataBase.ConnectionStrings, CommandType.Text, sql);
            return ds.Tables[0];
        }

        /// <summary>
        /// 平均贡献值
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        public string GetAvgAmount(int startDate, int endDate, string itemIds)
        {

            string sql = "  select AVG(Amount) as Amount from member where 1=1";
            if (startDate > 0)
            {
                sql += " and yearmonth>='" + startDate + "'";
            }
            if (endDate > 0)
            {
                sql += " and yearmonth<='" + endDate + "'";
            }
            if (!string.IsNullOrEmpty(itemIds))
            {
                sql += " and itemid in (" + Utils.StringHelper.SqlFilter(itemIds) + ")";
            }

            var obj = SqlHelper.ExecuteScalar(DataBase.ConnectionStrings, CommandType.Text, sql);

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
        /// 年平均贡献值
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        public string GetAvgAmount(int year, string itemIds)
        {

            string sql = "  SELECT SUM(Amount) / MAX(TotalDays) * 30 from member where 1=1";
            if (year > 0)
            {
                sql += " and yearmonth='" + year + "'";
            }

            if (!string.IsNullOrEmpty(itemIds))
            {
                sql += " and itemid in (" + Utils.StringHelper.SqlFilter(itemIds) + ")";
            }

            var obj = SqlHelper.ExecuteScalar(DataBase.ConnectionStrings, CommandType.Text, sql);

            if (obj != null)
            {
                return obj.ToString();
            }
            else
            {
                return "";
            }
        }


    }
}

