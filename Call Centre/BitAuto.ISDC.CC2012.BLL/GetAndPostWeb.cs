using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Sockets;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class GetAndPostWeb
    {

        /// <summary>
        /// 根据URL，Post数据
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr">要Post的数据</param>
        /// <returns></returns>
        public static string HttpPost(string Url, string postDataStr, string encodeName)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            // request.CookieContainer = cookie;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding(encodeName));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding(encodeName));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }


        #region 第二类POST方法


        const string sUserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        const string sContentType = "application/x-www-form-urlencoded";
        const string sRequestEncoding = "utf-8";
        const string sResponseEncoding = "utf-8";


        /// <summary>
        /// Post data到url
        /// </summary>
        /// <param name="data">要post的数据</param>
        /// <param name="url">目标url</param>
        /// <returns>服务器响应</returns>
        public static string PostDataToUrl(string data, string url)
        {
            Encoding encoding = Encoding.GetEncoding(sRequestEncoding);
            byte[] bytesToPost = encoding.GetBytes(data);
            return PostDataToUrl(bytesToPost, url);
        }

        /// <summary>
        /// Post data到url
        /// </summary>
        /// <param name="data">要post的数据</param>
        /// <param name="url">目标url</param>
        /// <returns>服务器响应</returns>
        public static string PostDataToUrl(byte[] data, string url)
        {
            #region 创建httpWebRequest对象
            WebRequest webRequest = WebRequest.Create(url);
            HttpWebRequest httpRequest = webRequest as HttpWebRequest;
            if (httpRequest == null)
            {
                throw new ApplicationException(
                    string.Format("Invalid url string: {0}", url)
                    );
            }
            #endregion

            #region 填充httpWebRequest的基本信息
            httpRequest.UserAgent = sUserAgent;
            httpRequest.ContentType = sContentType;
            httpRequest.Method = "POST";
            #endregion

            #region 填充要post的内容
            httpRequest.ContentLength = data.Length;
            Stream requestStream = httpRequest.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();
            #endregion

            #region 发送post请求到服务器并读取服务器返回信息
            Stream responseStream;
            try
            {
                responseStream = httpRequest.GetResponse().GetResponseStream();
            }

            catch (Exception e)
            {
                throw e;
            }
            #endregion

            #region 读取服务器返回信息
            string stringResponse = string.Empty;
            using (StreamReader responseReader =
                new StreamReader(responseStream, Encoding.GetEncoding(sResponseEncoding)))
            {
                stringResponse = responseReader.ReadToEnd();
            }
            responseStream.Close();
            #endregion
            return stringResponse;
        }
        #endregion

        #region Get方法
        /// <summary>
        /// Get data到url
        /// </summary>
        /// <param name="data">要get的数据</param>
        /// <param name="url">目标url</param>
        /// <returns>服务器响应</returns>
        public static string GetDataToUrl(byte[] data, string url)
        {
            #region 创建httpWebRequest对象
            WebRequest webRequest = WebRequest.Create(url);
            HttpWebRequest httpRequest = webRequest as HttpWebRequest;
            if (httpRequest == null)
            {
                throw new ApplicationException(
                    string.Format("Invalid url string: {0}", url)
                    );
            }
            #endregion

            #region 填充httpWebRequest的基本信息
            httpRequest.UserAgent = sUserAgent;
            httpRequest.ContentType = sContentType;
            httpRequest.Method = "GET";
            #endregion

            #region 填充要post的内容
            httpRequest.ContentLength = data.Length;
            Stream requestStream = httpRequest.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();
            #endregion

            #region 发送post请求到服务器并读取服务器返回信息
            Stream responseStream;
            try
            {
                responseStream = httpRequest.GetResponse().GetResponseStream();
            }

            catch (Exception e)
            {
                throw e;
            }
            #endregion

            #region 读取服务器返回信息
            string stringResponse = string.Empty;
            using (StreamReader responseReader =
                new StreamReader(responseStream, Encoding.GetEncoding(sResponseEncoding)))
            {
                stringResponse = responseReader.ReadToEnd();
            }
            responseStream.Close();
            #endregion
            return stringResponse;
        }


        public static string GetDataToUrl2(byte[] data, string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "get";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.Timeout = 6000;
            string stringResponse = string.Empty;
            try
            {
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default);
                stringResponse = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                return "Put data error";
            }
            return stringResponse;
        }

        private string GetHTMLTCP(string URL)
        {
            string strHTML = "";//用来保存获得的HTML代码
            TcpClient clientSocket = new TcpClient();
            Uri URI = new Uri(URL);
            clientSocket.Connect(URI.Host, URI.Port);
            StringBuilder RequestHeaders = new StringBuilder();//用来保存HTML协议头部信息
            RequestHeaders.AppendFormat("{0} {1} HTTP/1.1\r\n", "GET", URI.PathAndQuery);
            RequestHeaders.AppendFormat("Connection:close\r\n");
            RequestHeaders.AppendFormat("Host:{0}\r\n", URI.Host);
            RequestHeaders.AppendFormat("Accept:*/*\r\n");
            RequestHeaders.AppendFormat("Accept-Language:zh-cn\r\n");
            RequestHeaders.AppendFormat("User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)\r\n\r\n");
            Encoding encoding = Encoding.Default;
            byte[] request = encoding.GetBytes(RequestHeaders.ToString());
            clientSocket.Client.Send(request);
            //获取要保存的网络流
            Stream readStream = clientSocket.GetStream();
            StreamReader sr = new StreamReader(readStream, Encoding.Default);
            strHTML = sr.ReadToEnd();


            readStream.Close();
            clientSocket.Close();

            return strHTML;
        }

        /// <summary>
        /// Get data到url
        /// </summary>
        /// <param name="data">要get的数据</param>
        /// <param name="url">目标url</param>
        /// <returns>服务器响应</returns>
        public static string GetDataToUrl(string data, string url)
        {
            Encoding encoding = Encoding.GetEncoding(sRequestEncoding);
            byte[] bytesToGet = encoding.GetBytes(data);
            return GetDataToUrl(bytesToGet, url);
        }
        #endregion

    }
}
