
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BitAuto.Utils.Data;

namespace BitAuto.DSC.APPReport2016.Dal
{
    /// <summary>
    /// 数据访问类:MemberDaily
    /// </summary>
    public partial class MemberDaily
    {
        public static MemberDaily Instance = new MemberDaily();

        public MemberDaily()
        { }

        /// 查询天表-MemberDaily，获取当前时间的合作数和当年最大的合作数
        /// <summary>
        /// 查询天表-MemberDaily，获取当前时间的合作数和当年最大的合作数
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataSet GetMemberHZForDay(int itemId)
        {
            //查询昨天的合作数
            string sql1 = @"SELECT TOP 1 Date, Count, ItemId,
                (SELECT DictName FROM dbo.DictInfo WHERE DictId=MemberDaily.ItemId) AS NAME
                FROM dbo.MemberDaily
                WHERE Date<=GETDATE()-1
                AND ItemId=" + itemId + @"
                ORDER BY Date DESC";
            //查询今年最大的合作数
            string sql2 = @"SELECT Count
                FROM dbo.Member
                WHERE YearMonth=" + DateTime.Today.Year + @"00         
                AND ItemId=" + itemId;
            return SqlHelper.ExecuteDataset(DataBase.ConnectionStrings, CommandType.Text, sql1 + ";" + sql2);
        }
        /// 查询月表Member，某些会员和date时间之前的全部合作数据
        /// <summary>
        /// 查询月表Member，某些会员和date时间之前的全部合作数据
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="date"></param>
        public DataTable GetMemberHZForMonth(DateTime date, string itemids)
        {
            string sql = @"SELECT YearMonth,ItemId,Total,Count,Amount,
                (SELECT DictName FROM dbo.DictInfo WHERE DictId=Member.ItemId) AS NAME
                FROM dbo.Member
                WHERE ItemId IN (" + Utils.StringHelper.SqlFilter(itemids) + @")
                AND YearMonth%100!=0
                AND YearMonth<=" + date.ToString("yyyyMM")
                + " ORDER BY YearMonth";
            return SqlHelper.ExecuteDataset(DataBase.ConnectionStrings, CommandType.Text, sql).Tables[0];
        }
    }
}

