
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BitAuto.Utils.Data;
using System.Data;

namespace BitAuto.DSC.APPReport2016.Dal
{
    public class MemberArpu
    {
        public static MemberArpu Instance = new MemberArpu();

        private MemberArpu()
        { }


        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        public DataTable GetData(int year, int quarter, string itemIds)
        {

            string sql = "  select * from memberarpu  where 1=1";
            if (year > 0)
            {
                sql += " and Year='" + year + "'";
            }
            if (quarter >= 0)
            {
                sql += " and quarter='" + quarter + "'";
            }

            if (!string.IsNullOrEmpty(itemIds))
            {
                sql += " and itemid in (" + Utils.StringHelper.SqlFilter(itemIds) + ")";
            }

            return SqlHelper.ExecuteDataset(DataBase.ConnectionStrings, CommandType.Text, sql).Tables[0];


        }

    }

}
