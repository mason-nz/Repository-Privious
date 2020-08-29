using System.IO;
using System.Net;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.WebService.Common
{
    /// <summary>
    /// auth:lixiong
    /// desc:轻量级http封装
    /// </summary>
    public sealed class HttpClient
    {
        public static string PostByJson(string url, string postData)
        {
            return Post(url, postData, "application/json");
        }

        public static string PostByFrom(string url, string postData)
        {
            return Post(url, postData, "application/x-www-form-urlencoded");
        }

        public static string Get(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();

            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }

        /// <summary>
        ///  POST 数据到服务器
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="postData">post参数</param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        private static string Post(string url, string postData, string contentType)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var data = Encoding.UTF8.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = contentType;
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
    }
}