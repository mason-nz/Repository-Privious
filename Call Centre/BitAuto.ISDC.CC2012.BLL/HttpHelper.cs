using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Security;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class HttpHelper
    {
        /// 创建GET方式的HTTP请求  
        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        public static HttpWebResponse CreateGetHttpResponse(string url)
        {
            HttpWebRequest request = null;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //对服务端证书进行有效性校验（非第三方权威机构颁发的证书，如自己生成的，不进行验证，这里返回true）
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;    //http版本，默认是1.1,这里设置为1.0
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "GET";
            request.ContentType = "text/xml";
            return request.GetResponse() as HttpWebResponse;
        }
        /// 创建POST方式的HTTP请求  
        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>  
        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, CookieCollection cookies)
        {
            HttpWebRequest request = CreatePostRequest(url, cookies);
            string queryjson = CreateQueryJson(parameters);
            return SendPostRequest(queryjson, request);
        }
        /// 创建POST方式的HTTP请求  
        /// <summary>
        /// 创建POST方式的HTTP请求  
        /// </summary>
        /// <param name="url"></param>
        /// <param name="queryjson"></param>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public static HttpWebResponse CreatePostHttpResponse(string url, string queryjson, CookieCollection cookies)
        {
            HttpWebRequest request = CreatePostRequest(url, cookies);
            return SendPostRequest(queryjson, request);
        }

        /// 获取请求的数据
        /// <summary>
        /// 获取请求的数据
        /// </summary>
        public static string GetResponseString(HttpWebResponse webresponse)
        {
            using (Stream s = webresponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                return reader.ReadToEnd();
            }
        }

        /// 创建查询json字符串
        /// <summary>
        /// 创建查询json字符串
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static string CreateQueryJson(IDictionary<string, string> parameters)
        {
            StringBuilder buffer = new StringBuilder();
            if (!(parameters == null || parameters.Count == 0))
            {
                buffer.Append("{");
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i != 0)
                    {
                        buffer.Append(",");
                    }
                    if (parameters[key].StartsWith("["))
                    {
                        buffer.AppendFormat("\"{0}\":{1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("\"{0}\":\"{1}\"", key, parameters[key]);
                    }
                    i++;
                }
                buffer.Append("}");
            }
            return buffer.ToString();
        }
        /// 创建HttpWebRequest
        /// <summary>
        /// 创建HttpWebRequest
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookies"></param>
        /// <returns></returns>
        private static HttpWebRequest CreatePostRequest(string url, CookieCollection cookies)
        {
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                //request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/json";
            //设置代理UserAgent和超时
            //request.UserAgent = userAgent;
            //request.Timeout = timeout; 

            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            return request;
        }
        /// 发送POST数据  
        /// <summary>
        /// 发送POST数据  
        /// </summary>
        /// <param name="queryjson"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private static HttpWebResponse SendPostRequest(string queryjson, HttpWebRequest request)
        {
            //发送POST数据  
            if (!string.IsNullOrEmpty(queryjson))
            {
                byte[] data = Encoding.ASCII.GetBytes(queryjson.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            string[] values = request.Headers.GetValues("Content-Type");
            return request.GetResponse() as HttpWebResponse;
        }
        /// 验证证书
        /// <summary>
        /// 验证证书
        /// </summary>
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }
    }
}
