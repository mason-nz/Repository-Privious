using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils;

namespace XYAuto.BUOC.ChiTuData2017.NunitTest.Common
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
        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, CookieCollection cookies, int requestContentType = (int)RequestContentType.Form)
        {
            HttpWebRequest request = CreatePostRequest(url, cookies, requestContentType);
            string queryjson = CreateQueryJson(parameters);
            Loger.Log4Net.Info($"请求地址：{url},请求参数：{queryjson},请求类型：{Util.GetEnumOptText(typeof(RequestContentType), requestContentType)},请求Method:{request.Method}");
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
        public static HttpWebResponse CreatePostHttpResponse(string url, string queryjson, CookieCollection cookies, int requestContentType = (int)RequestContentType.Form)
        {
            HttpWebRequest request = CreatePostRequest(url, cookies, requestContentType);
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

        public static string CreatePostHttpResponseByMultipart(string url, int timeOut, string fileKeyName,
                                    string filePath, IDictionary<string, string> stringDict)
        {
            string responseContent;
            var memStream = new MemoryStream();
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            // 边界符  
            var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
            // 边界符  
            var beginBoundary = Encoding.UTF8.GetBytes("--" + boundary + "\r\n");
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            // 最后的结束符  
            var endBoundary = Encoding.UTF8.GetBytes("--" + boundary + "--\r\n");

            // 设置属性  
            webRequest.Method = "POST";
            webRequest.Timeout = timeOut;
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;

            // 写入文件  
            const string filePartHeader =
                "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                 "Content-Type: application/octet-stream\r\n\r\n";
            var header = string.Format(filePartHeader, fileKeyName, filePath);
            var headerbytes = Encoding.UTF8.GetBytes(header);

            memStream.Write(beginBoundary, 0, beginBoundary.Length);
            memStream.Write(headerbytes, 0, headerbytes.Length);

            var buffer = new byte[1024];
            int bytesRead; // =0  

            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                memStream.Write(buffer, 0, bytesRead);
            }

            // 写入字符串的Key  
            var stringKeyHeader = "\r\n--" + boundary +
                                   "\r\nContent-Disposition: form-data; name=\"{0}\"" +
                                   "\r\n\r\n{1}";
            //foreach (byte[] formitembytes in from string key in stringDict.Keys
            //                                 select string.Format(stringKeyHeader, key, stringDict[key])
            //                                 into formitem
            //                                 select Encoding.UTF8.GetBytes(formitem))
            for (int i = 0; i < stringDict.Keys.Count; i++)
            {
                string temp = string.Format(stringKeyHeader, stringDict.Keys.ElementAt(i), (string)stringDict[stringDict.Keys.ElementAt(i)]);
                if (stringDict.Keys.Count - 1 == i)
                {
                    temp += "\r\n";
                }
                byte[] formitembytes = Encoding.UTF8.GetBytes(temp);
                memStream.Write(formitembytes, 0, formitembytes.Length);
            }

            // 写入最后的结束边界符  
            memStream.Write(endBoundary, 0, endBoundary.Length);

            webRequest.ContentLength = memStream.Length;

            var requestStream = webRequest.GetRequestStream();

            memStream.Position = 0;
            var tempBuffer = new byte[memStream.Length];
            memStream.Read(tempBuffer, 0, tempBuffer.Length);
            memStream.Close();

            requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            requestStream.Close();

            var httpWebResponse = (HttpWebResponse)webRequest.GetResponse();

            using (var httpStreamReader = new StreamReader(httpWebResponse.GetResponseStream(),
                                                            Encoding.GetEncoding("utf-8")))
            {
                responseContent = httpStreamReader.ReadToEnd();
            }

            fileStream.Close();
            httpWebResponse.Close();
            webRequest.Abort();

            return responseContent;
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
        private static HttpWebRequest CreatePostRequest(string url, CookieCollection cookies, int requestContentType)
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
            //request.ContentType = "application/json";
            //request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType = Util.GetEnumOptText(typeof(RequestContentType), requestContentType);
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
                byte[] data = Encoding.UTF8.GetBytes(queryjson.ToString());
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


        public static string OrderPara(IDictionary<string, string> para)
        {
            string paraStr = string.Empty;
            if (para != null)
            {
                para = para.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
                foreach (var item in para)
                {
                    paraStr += item.Key + item.Value;
                }
            }
            return paraStr;
        }
    }

    /// <summary>
    /// Post提交时，ContentType枚举类型
    /// </summary>
    [Serializable]
    public enum RequestContentType
    {

        [EnumTextValue("application/json")]
        JSON = 1,

        [EnumTextValue("application/x-www-form-urlencoded")]
        Form = 2,

        [EnumTextValue("multipart/form-data")]
        Multipart = 3
    }
}
