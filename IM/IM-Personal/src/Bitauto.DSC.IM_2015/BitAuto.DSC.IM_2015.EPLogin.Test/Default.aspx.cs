using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using BitAuto.DSC.IM_2015.Core;

namespace BitAuto.DSC.IM_2015.EPLogin.Test
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Unnamed1_Click(object sender, EventArgs e)
        {
            //WaitAgentAllocThread t=new WaitAgentAllocThread(null);
            //Parallel.For(0, 100, (i) =>
            //{
            //  //  Debug.WriteLine(string.Format("{0}\t",i));
            //    //<add key="SourceType" value="易车网,1|惠买车,2|易车商城,3" />
            //    t.EnQueueWaitAgent("易车网",Guid.NewGuid().ToString());
            //});
            //Parallel.For(0, 100, (i) =>
            //{
            //    //  Debug.WriteLine(string.Format("{0}\t",i));
            //    //<add key="SourceType" value="易车网,1|惠买车,2|易车商城,3" />
            //    t.EnQueueWaitAgent("惠买车"
            //       , Guid.NewGuid().ToString());
            //});
            //Parallel.For(0, 100, (i) =>
            //{
            //    //  Debug.WriteLine(string.Format("{0}\t",i));
            //    //<add key="SourceType" value="易车网,1|惠买车,2|易车商城,3" />
            //    t.EnQueueWaitAgent("易车商城", Guid.NewGuid().ToString());
            //});
        }

        protected void btnCreateObj_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "pressuretest");
            var res = HttpHelper.CreatePostHttpResponse("http://im.sys1.bitauto.com/AjaxServers/Handler.ashx", dic, null);
            var str = HttpHelper.GetResponseString(res);
            Console.WriteLine(str);
            Debug.WriteLine(str);
            return;

            HttpWebRequest request = WebRequest.Create("http://im.sys1.bitauto.com/AjaxServers/Handler.ashx") as HttpWebRequest;
            request.Method = "POST";

            string strParas = "{\"action\":\"pressuretest\"}";

            byte[] data = Encoding.ASCII.GetBytes(strParas.ToString());
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            string strMsg;

            using (Stream s = request.GetResponse().GetResponseStream())
            {
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                strMsg = reader.ReadToEnd();
            }
            Debug.WriteLine(strMsg);
        }

        protected void btnPressure_Click(object sender, EventArgs e)
        {

            Parallel.For(0, 600, (i) =>
            {
                HttpWebRequest request = WebRequest.Create("http://im.sys1.bitauto.com/DefaultChannel.ashx?privateToken=" + i.ToString() + "&lastMessageId=1") as HttpWebRequest;
                string strMsg;

                using (Stream s = request.GetResponse().GetResponseStream())
                {
                    StreamReader reader = new StreamReader(s, Encoding.UTF8);
                    strMsg = reader.ReadToEnd();
                }
                Debug.WriteLine(strMsg);
            });
        }
    }

    public class HttpHelper
    {
        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        public static HttpWebResponse CreateGetHttpResponse(string url, int timeout, string userAgent, CookieCollection cookies)
        {
            HttpWebRequest request = null;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //对服务端证书进行有效性校验（非第三方权威机构颁发的证书，如自己生成的，不进行验证，这里返回true）
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;    //http版本，默认是1.1,这里设置为1.0
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "GET";

            //设置代理UserAgent和超时
            //request.UserAgent = userAgent;
            //request.Timeout = timeout;
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>  
        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, CookieCollection cookies)
        {
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                //request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/json";

            //设置代理UserAgent和超时
            //request.UserAgent = userAgent;
            //request.Timeout = timeout; 

            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            //发送POST数据  
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
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
                byte[] data = Encoding.ASCII.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            string[] values = request.Headers.GetValues("Content-Type");
            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>
        /// 获取请求的数据
        /// </summary>
        public static string GetResponseString(HttpWebResponse webresponse)
        {
            using (Stream s = webresponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// 验证证书
        /// </summary>
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }
    }
}