using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BitAuto.Utils.Data;
using System.Data.SqlClient;
using BitAuto.DSC.APPReport2016.Entities;

namespace BitAuto.DSC.APPReport2016.Dal
{
    public class MemberByDepart : DataBase
    {
        public static MemberByDepart Instance = new MemberByDepart();

        /// 获取最大日期
        /// <summary>
        /// 获取最大日期
        /// </summary>
        /// <param name="ItemIds"></param>
        /// <returns></returns>
        public int GetMaxDate(string ItemIds = "")
        {
            string sql = "SELECT MAX(YearMonth) from MemberByDepart where 1=1";
            if (!string.IsNullOrEmpty(ItemIds))
            {
                sql += " AND itemid in (" + Utils.StringHelper.SqlFilter(ItemIds )+ ")";
            }
            int result = CommonFunction.ObjectToInteger(SqlHelper.ExecuteScalar(DataBase.ConnectionStrings, CommandType.Text, sql));
            if (result > DateTime.Today.Year * 100 + DateTime.Today.Month)
            {
                //不能超过当前时间点
                result = DateTime.Today.Year * 100 + DateTime.Today.Month;
            }
            return result;
        }

        /// 获取新车会员按区域统计数据
        /// <summary>
        /// 获取新车会员按区域统计数据
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="yearMonth"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetData(string itemId, string yearMonth, string orderBy, int pageIndex, int pageSize, out int totalCount)
        {
            string sql = @"SELECT tmp.*,b.DepartName
                YanFaFROM (SELECT *,Count*1.0/Total AS fugailv 
                FROM dbo.MemberByDepart) tmp 
                inner JOIN dbo.v_Department b ON b.DepartId = tmp.DepartId 
                WHERE YearMonth='" + yearMonth + "'";
            if (!string.IsNullOrEmpty(itemId))
            {
                sql += " AND itemid in (" + BitAuto.Utils.StringHelper.SqlFilter(itemId) + ")";
            }
            if (string.IsNullOrWhiteSpace(orderBy))
            {
                //默认：覆盖率
                orderBy = "fugailv desc";
            }
            SqlParameter[] parameters = {				
                    new SqlParameter("@SQL", SqlDbType.NVarChar, 4000),
                    new SqlParameter("@Order", SqlDbType.NVarChar, 200),
					new SqlParameter("@CurPage", SqlDbType.Int, 4),
					new SqlParameter("@PageRows", SqlDbType.Int, 4),
                    new SqlParameter("@TotalRecorder", SqlDbType.Int, 4)
					};
            parameters[0].Value = sql;
            parameters[1].Value = orderBy;
            parameters[2].Value = pageIndex;
            parameters[3].Value = pageSize;
            parameters[4].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(DataBase.ConnectionStrings, CommandType.StoredProcedure, "P_Page", parameters);
            totalCount = (int)parameters[4].Value;
            return ds.Tables[0];
        }


        /// <summary>
        /// 获取某段时间内的会员数量
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="ItemIds">会员类型</param>
        /// <returns></returns>
        public DataTable GetDtFGL(int startDate, int endDate, string ItemIds)
        {
            string sql = "";
            sql = @"select m.*,d.DictName
 from memberbydepart m inner join  DictInfo d on m.ItemId=d.DictId AND m.DepartId='0'
 where YearMonth>=" + startDate + " and yearmonth<=" + endDate;

            if (!string.IsNullOrEmpty(ItemIds))
            {
                sql += " and itemid in (" + Utils.StringHelper.SqlFilter(ItemIds) + ")";
            }

            DataSet ds = SqlHelper.ExecuteDataset(DataBase.ConnectionStrings, CommandType.Text, sql);
            return ds.Tables[0];
        }
    }
}
