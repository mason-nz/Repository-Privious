using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BitAuto.Utils.Data;
using BitAuto.DSC.APPReport2016.Entities;

namespace BitAuto.DSC.APPReport2016.Dal
{
    public class Operation
    {
        public static Operation Instance = new Operation();


        public DataTable GetOperationDataByDate(DateTime date)
        {
            string sql = @"SELECT a.ItemId,a.Count,a.WeekBasis,a.DayBasis,b.DictName FROM Operation a INNER JOIN DictInfo b ON a.ItemId=b.DictId
                        WHERE   a.[Date]>='" + date.Date.ToString() + "' And  a.[Date]<'" + date.AddDays(1).Date.ToString() + @"' AND b.Status=0  AND a.ItemId!=0
                        ORDER BY b.OrderNum";

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
        /// 获取运营日报全量的数据
        /// <summary>
        /// 获取运营日报全量的数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataTable GetDataTrendByDate(DateTime date)
        {
            string sql = @"SELECT * FROM Operation WHERE ItemId!=0 AND [Date]<'" + date.AddDays(1).Date.ToString() + "' ORDER BY [Date],ItemId";

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
        /// 获取最新日期
        /// <summary>
        /// 获取最新日期
        /// </summary>
        /// <returns></returns>
        public DateTime GetLatestDate()
        {
            string sql = @"SELECT TOP 1 [Date]  FROM Operation WHERE [Date]<CONVERT(varchar(100), GETDATE(), 23) ORDER BY [Date] DESC";

            object obj = SqlHelper.ExecuteScalar(DataBase.ConnectionStrings, CommandType.Text, sql);
            return CommonFunction.ObjectToDateTime(obj);
        }

        /// <summary>
        /// 根据ID获取字典名称
        /// </summary>
        /// <param name="dictID"></param>
        /// <returns></returns>
        public string GetDictNameByDictID(int dictID)
        {
            string sql = @"SELECT DictName FROM DictInfo WHERE DictId=" + dictID;

            object obj = SqlHelper.ExecuteScalar(DataBase.ConnectionStrings, CommandType.Text, sql);

            return Convert.ToString(obj);
        }
    }
}
