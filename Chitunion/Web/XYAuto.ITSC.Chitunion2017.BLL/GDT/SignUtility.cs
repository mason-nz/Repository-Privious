/********************************************************
*创建人：lixiong
*创建时间：2017/8/17 15:30:59
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.GDT
{
    public class SignUtility
    {
        /// <summary>
        /// 生成签名规则
        /// </summary>
        /// <param name="dicContent"></param>
        /// <returns></returns>
        public static string GetSign(Dictionary<string, object> dicContent)
        {
            var md5 = MD5.Create();
            var text = dicContent.Where(x => x.Key.ToUpper() != "SIGN")
                .OrderBy(x => (x.Key + x.Value))
                .Aggregate("", (current, next) => current + next.Key + next.Value.ToString());
            var sign = string.Join("", md5.ComputeHash(Encoding.UTF8.GetBytes(text)).Select(x => x.ToString("X2")));
            return sign;
        }

        public static string FilterPara(Dictionary<string, object> dicContent)
        {
            var text = dicContent.Where(x => x.Key.ToUpper() != "SIGN")
               .OrderBy(x => (x.Key + x.Value))
               .Aggregate("", (current, next) => current + next.Key + (next.Value == null ? string.Empty : next.Value.ToString()));
            return text;
        }

        /// <summary>
        /// 除去数组中的空值和签名参数并以字母a到z的顺序排序
        /// </summary>
        /// <param name="dicArrayPre">过滤前的参数组</param>
        /// <returns>过滤后的参数组</returns>
        public static Dictionary<string, string> FilterPara(SortedDictionary<string, string> dicArrayPre)
        {
            var removerItem = new string[] { "sign", "sign_type" };
            return dicArrayPre
                .Where(x => !removerItem.Contains(x.Key.ToLower()) && !string.IsNullOrEmpty(x.Value))
                .ToDictionary(t => t.Key.ToLower(), t => t.Value);
        }

        public static string GetSignature(string decryptString)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] str1 = Encoding.UTF8.GetBytes(decryptString);
            byte[] str2 = sha1.ComputeHash(str1);
            sha1.Clear();
            (sha1 as IDisposable).Dispose();

            return Convert.ToBase64String(str2);
        }

        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>double</returns>
        public static long ConvertDateTimeInt(DateTime time)
        {
            return (time.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        public static string Md5Hash(string strText, Encoding encoding)
        {
            MD5 md = new MD5CryptoServiceProvider();
            byte[] bytes = encoding.GetBytes(strText);
            byte[] buffer2 = md.ComputeHash(bytes);
            string str = null;
            for (int i = 0; i < buffer2.Length; i++)
            {
                string str2 = buffer2[i].ToString("x");
                if (str2.Length == 1)
                {
                    str2 = "0" + str2;
                }
                str = str + str2;
            }
            return str;
        }
    }
}