using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;

namespace IMPressureTest
{
    /// <summary>
    /// 有关HTTP请求的辅助类 
    /// </summary>
    public class HttpWebResponseUtility
    {

        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";//浏览器
        private static Encoding requestEncoding = System.Text.Encoding.UTF8;//字符集

        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebResponse CreateGetHttpResponse(string url, int? timeout, string userAgent, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = DefaultUserAgent;
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>
        /// 获取请求的数据
        /// </summary>
        public static string GetResponseString(HttpWebResponse webresponse)
        {
            if (webresponse == null)
            {
                return "HttpWebResponse 为空";
            }
            try
            {
                using (Stream s = webresponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(s, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }

        /// <summary>  
        /// 创建POST方式的HTTP请求
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>
        public static HttpWebResponse CreatePostHttpResponse(string url, string parameters, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            HttpWebRequest request = null;
            Stream stream = null;//用于传参数的流

            try
            {
                //如果是发送HTTPS请求  
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    request = WebRequest.Create(url) as HttpWebRequest;
                    //创建证书文件
                    System.Security.Cryptography.X509Certificates.X509Certificate objx509 = new System.Security.Cryptography.X509Certificates.X509Certificate(Application.StartupPath + @"\\licensefile\zjs.cer");
                    //添加到请求里
                    request.ClientCertificates.Add(objx509);
                    request.ProtocolVersion = HttpVersion.Version10;
                }
                else
                {
                    request = WebRequest.Create(url) as HttpWebRequest;
                }

                request.Method = "POST";//传输方式
                request.ContentType = "application/x-www-form-urlencoded";//协议                
                request.UserAgent = DefaultUserAgent;//请求的客户端浏览器信息,默认IE                
                request.Timeout = 6000;//超时时间，写死6秒

                //随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空
                if (cookies != null)
                {
                    request.CookieContainer = new CookieContainer();
                    request.CookieContainer.Add(cookies);
                }

                //如果需求POST传数据，转换成utf-8编码
                byte[] data = requestEncoding.GetBytes(parameters);
                request.ContentLength = data.Length;

                stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);

                stream.Close();
            }
            catch (Exception ee)
            {
                //写日志
                //LogHelper.
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            return request.GetResponse() as HttpWebResponse;
        }

        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, int? timeout, string userAgent, Encoding requestEncoding, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            if (requestEncoding == null)
            {
                throw new ArgumentNullException("requestEncoding");
            }
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            else
            {
                request.UserAgent = DefaultUserAgent;
            }

            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            //如果需要POST数据  
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
                byte[] data = requestEncoding.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            try
            {
                return request.GetResponse() as HttpWebResponse;
            }
            catch (Exception)
            {


            }
            return null;
        }

        public static void CreatePostHttpResponseAsync(string url, IDictionary<string, string> parameters, int? timeout, string userAgent, Encoding requestEncoding, CookieCollection cookies, AsyncCallback callback)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            if (requestEncoding == null)
            {
                throw new ArgumentNullException("requestEncoding");
            }
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Date = DateTime.Now;
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            else
            {
                request.UserAgent = DefaultUserAgent;
            }

            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            //如果需要POST数据  
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
                request.BeginGetRequestStream(new AsyncCallback((state) =>
                {
                    HttpWebRequest re = (HttpWebRequest)state.AsyncState;
                    Stream postStream = request.EndGetRequestStream(state);
                    byte[] data = requestEncoding.GetBytes(buffer.ToString());
                    postStream.Write(data, 0, data.Length);
                    postStream.Close();


                    request.BeginGetResponse(new AsyncCallback((obj) =>
                    {
                        try
                        {
                            if (callback != null)
                            {
                                callback(obj);
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.WriteErrorLog(ex.Message, ex);

                        }

                        //HttpWebRequest request = (HttpWebRequest) obj.AsyncState;
                        //string strResult = GetResponseString(request.EndGetResponse(obj) as HttpWebResponse);
                    }), request);

                    //if (callback != null)
                    //{
                    //    callback(state);
                    //}
                }), request);
                //byte[] data = requestEncoding.GetBytes(buffer.ToString());
                //using (Stream stream = request.GetRequestStream())
                //{
                //    stream.Write(data, 0, data.Length);
                //}
            }
            /*
            request.BeginGetResponse(new AsyncCallback((obj) =>
            {
                try
                {
                    if (callback != null)
                    {
                        callback(obj);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteErrorLog(ex.Message, ex);

                }

                //HttpWebRequest request = (HttpWebRequest) obj.AsyncState;
                //string strResult = GetResponseString(request.EndGetResponse(obj) as HttpWebResponse);
            }), request);
            */
            //return request.GetResponse() as HttpWebResponse;
        }


        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }




        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="HttpWebResponse">响应对象</param>
        /// <returns></returns>
        public static string OpenReadWithHttps(HttpWebResponse HttpWebResponse)
        {
            Stream responseStream = null;
            StreamReader sReader = null;
            String value = null;

            try
            {
                // 获取响应流
                responseStream = HttpWebResponse.GetResponseStream();
                // 对接响应流(以"utf-8"字符集)
                sReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                // 开始读取数据
                value = sReader.ReadToEnd();
            }
            catch (Exception)
            {
                //日志异常
            }
            finally
            {
                //强制关闭
                if (sReader != null)
                {
                    sReader.Close();
                }
                if (responseStream != null)
                {
                    responseStream.Close();
                }
                if (HttpWebResponse != null)
                {
                    HttpWebResponse.Close();
                }
            }

            return value;
        }

        /// <summary>
        /// 入口方法：获取传回来的XML文件
        /// </summary>
        /// <param name="url">请求的URL</param>  
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>
        public static string GetResultXML(string url, string parameters, CookieCollection cookies)
        {
            return OpenReadWithHttps(CreatePostHttpResponse(url, parameters, cookies));
        }

    }

}
