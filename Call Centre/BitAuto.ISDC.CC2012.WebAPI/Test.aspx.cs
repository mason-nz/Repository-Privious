using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.IO;
using System.Text;

namespace BitAuto.ISDC.CC2012.WebAPI
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private int Get_business()
        {
            if (Radio1.Checked)
            {
                return 0;
            }
            else if (Radio2.Checked)
            {
                return 1;
            }
            else if (Radio3.Checked)
            {
                return 2;
            }
            else if (Radio4.Checked)
            {
                return 3;
            }
            else return -1;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            int b = Get_business();
            string url = "http://apincc.sys1.bitauto.com/hollycrm/ivr?method=checkCallNo&business=" + b + "&callNo=" + TextBox1.Text + "&calledNo=" + TextBox2.Text;

            this.link.HRef = url;
            this.link.InnerText = url;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            int b = Get_business();
            string url = "http://apincc.sys1.bitauto.com/hollycrm/ivr?method=addMediaInfo&business=" + b +
                "&callNo=" + TextBox1.Text + "&calledNo=" + TextBox2.Text +
                "&mediaInTime=" + TextBox3.Text + "&mediaOutTime=" + TextBox4.Text +
                "&filePath=" + TextBox5.Text + "&fileName=" + TextBox6.Text + "&contactID=" + TextBox11.Text;

            this.link.HRef = url;
            this.link.InnerText = url;
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            int b = Get_business();
            string url = "http://apincc.sys1.bitauto.com/hollycrm/ivr?method=addSatisfyIn&business=" + b +
                "&serviceId=" + TextBox7.Text + "&surveyResult=" + TextBox10.Text;

            this.link.HRef = url;
            this.link.InnerText = url;
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            int b = Get_business();
            string url = "http://apincc.sys1.bitauto.com/hollycrm/ivr?method=addIvrInfo&business=" + b +
                 "&callNo=" + TextBox1.Text + "&calledNo=" + TextBox2.Text +
                 "&startTime=" + TextBox3.Text + "&endTime=" + TextBox4.Text +
                 "&handle=" + TextBox8.Text + "&ivrKey=" + TextBox9.Text;

            this.link.HRef = url;
            this.link.InnerText = url;
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            string para = "";
            string name = "";
            if (RadioButton1.Checked)
            {
                name = "QueryAniDnis";
                para += "Oriani=" + TextBox12.Text + "&";
                para += "OriDnis=" + TextBox13.Text + "&";

            }
            else if (RadioButton2.Checked)
            {
                name = "QueryAni";
                para += "Oriani=" + TextBox12.Text + "&";
            }
            else if (RadioButton3.Checked)
            {
                name = "QueryDnis";
                para += "OriDnis=" + TextBox13.Text + "&";
            }
            else if (RadioButton4.Checked)
            {
                name = "QueryCallId";
                para += "CallID=" + TextBox14.Text + "&";
            }

            if (!string.IsNullOrEmpty(TextBox15.Text))
            {
                para += "Top=" + TextBox15.Text + "&";
            }

            para = para.Substring(0, para.Length - 1);

            string url = "http://apincc.sys1.bitauto.com/callrecord/" + name + "?" + para;

            this.link2.HRef = url;
            this.link2.InnerText = url;
        }

        protected void Button6_Click(object sender, EventArgs e)
        {
            link3.HRef = "http://apincc.sys1.bitauto.com/hollycrm/GetCurrentTaskID?beijiao=" + TextBox13.Text + "&key=" + HttpUtility.UrlEncode("yiche-ClineLog-!@#$#@!");
            link3.InnerText = link3.HRef;
        }

        protected void Button7_Click(object sender, EventArgs e)
        {
            string url = "http://apincc.sys1.bitauto.com/hollycrm/ivr?method=getagentidbycallno" +
                "&callNo=" + TextBox1.Text + "&calledNo=" + TextBox2.Text;
            this.link.HRef = url;
            this.link.InnerText = url;
        }
    }


    public class HttpHelper
    {
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
            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>  
        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, CookieCollection cookies)
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