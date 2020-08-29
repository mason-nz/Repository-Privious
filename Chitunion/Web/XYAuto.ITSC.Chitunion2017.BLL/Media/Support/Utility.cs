using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Support
{
    /// <summary>
    /// 工具类,主要是客户端使用
    /// </summary>
    public sealed class Utility
    {
        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
        /// </summary>
        /// <param name="dicArray">需要拼接的数组</param>
        /// <returns>拼接完成以后的字符串</returns>
        public static string CreateLinkString(IDictionary<string, string> dicArray)
        {
            return CreateLinkStringByDefault(dicArray);
        }

        /// <summary>
        ///  把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串，并对参数值做urlencode
        /// </summary>
        /// <param name="dicArray">需要拼接的数组</param>
        /// <param name="code">字符编码</param>
        /// <returns>拼接完成以后的字符串</returns>
        public static string CreateLinkStringUrlencode(IDictionary<string, string> dicArray, Encoding code)
        {
            return CreateLinkStringByDefault(dicArray, code);
        }

        public static string CreateLinkStringByDefault(IDictionary<string, string> dic, Encoding code = null)
        {
            List<string> preStr = dic.Select(
                x => string.Format("{0}={1}", x.Key, code == null ? x.Value : HttpUtility.UrlEncode(x.Value, code)))
                .ToList<string>();

            return string.Join("&", preStr);
        }
    }
}