using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;

namespace BitAuto.ISDC.CC2012.BLL.Common
{
  public  class JsonHelper
    {

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        public static string ToJsonString<T>(T jsonObject)
        {
            StringBuilder outStr = new StringBuilder();
            JavaScriptSerializer js = new JavaScriptSerializer();
            js.Serialize((object)jsonObject, outStr);

            #region 处理时间

            string sRet = outStr.ToString();

            //将时间由"\/Date(10000000000-0700)\/" 格式转换成 "yyyy-MM-dd HH:mm:ss" 格式的字符串
            string sPattern = @"\\/Date\(-?\d+\)\\/";
            MatchEvaluator myMatchEvaluator = new MatchEvaluator(GetDatetimeString);
            Regex reg = new Regex(sPattern);
            sRet = reg.Replace(sRet, myMatchEvaluator);

            #endregion

            return sRet;
        }

        /// <summary>
        /// 将时间由"\/Date(10000000000-0700)\/" 格式转换成 "yyyy-MM-dd HH:mm:ss" 格式的字符串
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private static string GetDatetimeString(Match m)
        {
            string sRet = "";
            try
            {
                System.DateTime dt = new DateTime(1970, 1, 1);
                string millsecond = m.Groups[0].Value;
                millsecond = millsecond.Substring(0, millsecond.LastIndexOf(')'));
                millsecond = millsecond.Substring(millsecond.IndexOf('(') + 1);

                dt = dt.AddMilliseconds(long.Parse(millsecond));
                dt = dt.ToLocalTime();
                sRet = dt.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch
            { }

            return sRet;
        }
    }
}
