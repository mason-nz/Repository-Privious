using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Common;

namespace XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend
{
    /// <summary>
    /// auth:lixiong
    /// desc:日常开发扩展方法
    /// </summary>
    public static class ExtendFunction
    {
        public static string ToSqlFilter(this string sql)
        {
            return string.IsNullOrWhiteSpace(sql) ? sql
                : XYAuto.Utils.StringHelper.SqlFilter(sql);
        }

        /// <summary>
        /// 将url转换为url绝对路径
        /// </summary>
        /// <param name="url">http://www.chitunion.com/UploadFiles/2017/3/20/17/wallhaven-13918$afbceb37-4294-4419-a252-5cb1d904b9e9.jpg</param>
        /// <returns>/UploadFiles/2017/3/20/17/wallhaven-13918$afbceb37-4294-4419-a252-5cb1d904b9e9.jpg</returns>
        public static string ToAbsolutePath(this string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return url;

            try
            {
                return new Uri(url).AbsolutePath;
            }
            catch
            {
                return url;
            }
        }

        public static string ToAbsolutePath(this string url, bool isCurrentDomain)
        {
            if (isCurrentDomain)
            {
                return ToAbsolutePath(url, UserInfo.WebDomain);
            }
            return ToAbsolutePath(url);
        }

        /// <summary>
        /// 过滤掉自己的域名下的url，如果是当前域名，则需要去掉域名存储，如果不是，则不用理会
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filterDomain">当前域名：chitunion.com，不要www</param>
        /// <returns></returns>
        public static string ToAbsolutePath(this string url, string filterDomain)
        {
            if (string.IsNullOrWhiteSpace(url)) return url;
            if (string.IsNullOrWhiteSpace(filterDomain)) return url;

            if (!filterDomain.Contains("www."))
            {
                filterDomain = $"www.{filterDomain}";
            }
            if (url.Contains(filterDomain))
            {
                return ToAbsolutePath(url);
            }
            return url;
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> list)
        {
            //创建属性的集合
            List<PropertyInfo> pList = new List<PropertyInfo>();
            //获得反射的入口

            Type type = typeof(T);
            DataTable dt = new DataTable();
            //把所有的public属性加入到集合 并添加DataTable的列
            Array.ForEach<PropertyInfo>(type.GetProperties(), p => { pList.Add(p); dt.Columns.Add(p.Name, p.PropertyType); });
            foreach (var item in list)
            {
                //创建一个DataRow实例
                DataRow row = dt.NewRow();
                //给row 赋值
                pList.ForEach(p => row[p.Name] = p.GetValue(item, null));
                //加入到DataTable
                dt.Rows.Add(row);
            }
            return dt;
        }

        public static void AddNotEmptyValue(this SortedDictionary<string, string> sorted, string key, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;
            sorted.Add(key, value);
        }

        public static string GetDictionaryValueByKey<T>(this IDictionary<string, string> dic, string key)
        {
            if (!dic.ContainsKey(key))
                return default(T).ToString();
            return dic[key];
        }

        /// <summary>
        /// 获取字典的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dirs"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetValue<T>(this IDictionary<string, T> dirs, string key)
        {
            T t;
            dirs.TryGetValue(key, out t);
            return t;
        }

        public static void RecordLog(this string content, Action<string> action)
        {
            if (action != null)
                action(content);
        }

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