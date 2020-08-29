using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.Utils.Data;
using System.Data;
using BitAuto.DSC.APPReport2016.Entities;

namespace BitAuto.DSC.APPReport2016.Dal
{
    public class Trades
    {
        public static Trades Instance = new Trades();

        /// <summary>
        /// 获取最新日期
        /// </summary>
        /// <returns></returns>
        public DateTime GetLatestDate()
        {
            string sql = @"SELECT TOP 1 [Date]  FROM Trades WHERE [Date]<CONVERT(varchar(100), GETDATE(), 23)  AND LineId=0  ORDER BY [Date] DESC";

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

        /// <summary>
        /// 根据日期获取数据
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataTable GetDataByDate(DateTime date)
        {
            string sql = @" SELECT a.LineId,a.Count,a.WeekBasis,a.DayBasis,b.DictName FROM Trades a 
                            INNER JOIN DictInfo b 
                            ON a.LineId=b.DictId
                            WHERE   b.Status=0  AND a.LineId IN(20003,20004)                            
                            AND a.[Date]>='" + date.Date.ToString() + "' And  a.[Date]<'" + date.AddDays(1).Date.ToString() + @"' 
                            ORDER BY b.OrderNum ";

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
        /// 获取交易量线图数据（该日期前所有数据）
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataTable GetDataTrendByDate(DateTime date)
        {
            string sql = @"SELECT LineId,[Date], Count FROM Trades WHERE LineId IN(20003,20004) AND [Date]<'" + date.AddDays(1).Date.ToString() + "'";

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
    }
}
