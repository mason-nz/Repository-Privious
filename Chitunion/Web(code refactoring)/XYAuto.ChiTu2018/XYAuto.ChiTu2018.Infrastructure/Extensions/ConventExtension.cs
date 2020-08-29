using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Infrastructure.Extensions
{
    /// <summary>
    /// 注释：ConventExtension
    /// 作者：lix
    /// 日期：2018/5/15 15:36:37
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public static class ConventExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strParam"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ToInt(this string strParam, int defaultValue = 0)
        {
            int outPutInt;
            if (!int.TryParse(strParam, out outPutInt))
                outPutInt = defaultValue;
            return outPutInt;
        }

        public static int ToInt(this int? intParam, int defaultValue = 0)
        {
            if (intParam.HasValue)
                return intParam.Value;
            else
            {
                return defaultValue;
            }
        }


        public static decimal ToDecimal(this string strParam, decimal defaultValue = 0)
        {
            decimal outPutDecimal;
            if (!decimal.TryParse(strParam, out outPutDecimal))
                outPutDecimal = defaultValue;
            return outPutDecimal;
        }

        public static decimal ToDecimal(this decimal? intParam, decimal defaultValue = 0)
        {
            if (intParam.HasValue)
                return intParam.Value;
            else
            {
                return defaultValue;
            }
        }

        public static float ToFloat(this string strParam, float defaultValue = 0)
        {
            float outPutInt;
            if (!float.TryParse(strParam, out outPutInt))
                outPutInt = defaultValue;
            return outPutInt;
        }

        public static bool ToBoolean(this string strParams, bool defaultValue = false)
        {
            bool outPutBool;
            if (!bool.TryParse(strParams, out outPutBool))
                outPutBool = defaultValue;
            return outPutBool;
        }

        public static bool ToBoolean(this int strParams, bool defaultValue = false)
        {
            try
            {
                return Convert.ToBoolean(strParams);
            }
            catch
            {
                return defaultValue;
            }
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
    }
}
