using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.WechatSign;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.WechatSign
{
    public class WechatSign : DataBase
    {
        public static readonly WechatSign Instance = new WechatSign();
        /// <summary>
        /// 插入签到表
        /// </summary>
        /// <param name="SignInfo">签到类</param>
        /// <returns></returns>
        public int InsetDaySign(WechatSignInfo SignInfo)
        {
            string strSql = $@"INSERT INTO dbo.LE_DaySign
                                (SignTime,
                                  SignUserID,
                                  SignPrice,
                                  SignNumber,
                                  IP
                                )
                        VALUES(GETDATE(), 
                                  {SignInfo.SignUserID},
                                  {SignInfo.SignPrice},
                                  {SignInfo.SignNumber},
                                  '{SignInfo.Ip}'
                                )";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        /// <summary>
        /// 根据年月查询签到日期
        /// </summary>
        /// <param name="SignUserID">签到用户ID</param>
        /// <param name="lastDate">签到开始日期</param>
        /// <param name="mextDate">签到结束日期</param>
        /// <returns></returns>
        public DataTable SelectDaySignListByMonth(int SignUserID, string lastDate, string mextDate)
        {
            string strSql = $@"SELECT CONVERT(CHAR(10), SignTime, 23) SignDate FROM  LE_DaySign WHERE SignUserID={SignUserID}  AND CONVERT(CHAR(7), SignTime, 23) BETWEEN '{lastDate}' AND '{mextDate}' ";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
        /// <summary>
        /// 查询指定用户签到总金额
        /// </summary>
        /// <param name="SignUserID">签到用户ID</param>
        /// <param name="CategoryID">收益类型（103003 签到红包统计）</param>
        /// <returns></returns>
        public decimal GetTotalPriceByUserID(int SignUserID, int CategoryID)
        {
            string strSql = $"SELECT IncomePrice FROM LE_IncomeStatisticsCategory WHERE  IncomeCategoryID={CategoryID} AND UserID={SignUserID}";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToDecimal(obj);
        }
        public DataTable GetSignNumber(int SignUserID, DateTime SignTime)
        {
            string strSql = $@"SELECT TOP 1 CONVERT(CHAR(10), SignTime, 23) SignDate,SignNumber FROM  LE_DaySign WHERE SignUserID={SignUserID}  AND CONVERT(CHAR(10), SignTime, 23) <='CONVERT(CHAR(10), {SignTime}, 23)' ORDER BY  SignTime DESC ";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
        /// <summary>
        /// 判断当天是否签到
        /// </summary>
        /// <param name="SignUserID"></param>
        /// <param name="SignTime"></param>
        /// <returns></returns>
        public bool IsDaySign(int SignUserID, DateTime SignTime)
        {
            string strSql = $"Select count(1) from LE_DaySign WHERE SignUserID={SignUserID}  AND CONVERT(CHAR(10), SignTime, 23)='{ SignTime.ToString("yyyy-MM-dd")}'";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? false : (Convert.ToInt32(obj) > 0 ? true : false);

        }
    }


}
