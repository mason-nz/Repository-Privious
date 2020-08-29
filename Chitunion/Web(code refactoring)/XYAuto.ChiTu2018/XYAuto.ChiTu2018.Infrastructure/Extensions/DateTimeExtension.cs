using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Infrastructure.Extensions
{
    /// <summary>
    /// 注释：DateTimeExtension
    /// 作者：lix
    /// 日期：2018/5/15 15:37:09
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public static class DateTimeExtension
    {
        /// <summary>  
        /// 取得某月的第一天  
        /// </summary>  
        /// <param name="dateTime">要取得月份第一天的时间</param>  
        /// <returns></returns>  
        public static DateTime FirstDayOfMonth(this DateTime dateTime)
        {
            return dateTime.AddDays(1 - dateTime.Day);
        }

        /// <summary>  
        /// 取得某月的最后一天  
        /// </summary>  
        /// <param name="dateTime">要取得月份最后一天的时间</param>  
        /// <returns></returns>  
        public static DateTime LastDayOfMonth(this DateTime dateTime)
        {
            return dateTime.AddDays(1 - dateTime.Day).AddMonths(1).AddDays(-1);
        }

        /// <summary>  
        /// 取得上个月第一天  
        /// </summary>  
        /// <param name="datetime">要取得上个月第一天的当前时间</param>  
        /// <returns></returns>  
        public static DateTime FirstDayOfPreviousMonth(this DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(-1);
        }

        /// <summary>  
        /// 取得上个月的最后一天  
        /// </summary>  
        /// <param name="datetime">要取得上个月最后一天的当前时间</param>  
        /// <returns></returns>  
        public static DateTime LastDayOfPrdviousMonth(this DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddDays(-1);
        }
    }
}
