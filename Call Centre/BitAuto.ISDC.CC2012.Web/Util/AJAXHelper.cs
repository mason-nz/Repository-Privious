using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Data;


namespace BitAuto.ISDC.CC2012.Web.Util
{
    public class AJAXHelper
    {
        #region 包装文本格式返回结果

        /// <summary>
        /// 包装Json格式的HttpResponse
        /// 如果要支持跨域请求，客户端要用jQuery.getJSON方法，并在URL中加入callback=?
        /// </summary>
        /// <param name="success">执行结果</param>
        /// <param name="result">执行成功时：返回值；执行失败时：错误编码</param>
        /// <param name="message">附加信息（一般为错误信息）</param>
        public static void WrapJsonResponse(bool success, string result, string message, string responseContentType)
        {
            HttpRequest request = HttpContext.Current.Request;
            HttpResponse response = HttpContext.Current.Response;

            //(1)
            StringBuilder sbJson = new StringBuilder();
            StringWriter swJson = new StringWriter(sbJson);
            using (JsonWriter jsonWriter = new JsonWriter(swJson))
            {
                jsonWriter.Formatting = Newtonsoft.Json.Formatting.Indented;
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("success"); jsonWriter.WriteValue(success);
                jsonWriter.WritePropertyName("result"); jsonWriter.WriteValue(result.ToString());
                jsonWriter.WritePropertyName("message"); jsonWriter.WriteValue(message.ToString());
                jsonWriter.WriteEndObject();
            }
            swJson.Flush();//清空缓存,并将缓冲的数据写出到基础设备

            //(2)
            string callbackName = (request["callback"] + "").Trim();
            string content = string.Empty;
            if (string.IsNullOrEmpty(callbackName) == false)
            {
                content = callbackName + "(" + sbJson.ToString() + ");";
            }
            else
            {
                content = sbJson.ToString();
            }
            response.ContentType = responseContentType; //"application/json";//"text/plain";
            response.Clear();
            response.Write(content);
        }

        /// <summary>
        /// 包装Json格式的HttpResponse
        /// 如果要支持跨域请求，要在请求的URL中加入callback=?，并且要有相应的处理，比如jQuery.getJSON方法。
        /// </summary>
        /// <param name="success">执行结果</param>
        /// <param name="result">执行成功时：返回值；执行失败时：错误编码</param>
        /// <param name="message">附加信息（一般为错误信息）</param>
        public static void WrapJsonResponse(bool success, string result, string message)
        {
            WrapJsonResponse(success, result, message, "text/plain");
        }


        /// <summary>
        /// 包装XML格式的HttpResponse
        /// </summary>
        /// <param name="success">执行结果</param>
        /// <param name="result">执行成功时：返回值；执行失败时：错误编码</param>
        /// <param name="message">附加信息（一般为错误信息）</param>
        /// <param name="encoding">编码</param>
        public static void WrapXMLResponse(bool success, string result, string message, string encoding)
        {
            HttpRequest request = HttpContext.Current.Request;
            HttpResponse response = HttpContext.Current.Response;

            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", encoding, null));//"utf-8"

            XmlElement root = doc.CreateElement("ResponseData");
            doc.AppendChild(root);

            XmlElement successEle = doc.CreateElement("Success");
            successEle.InnerXml = success == true ? "1" : "0";
            root.AppendChild(successEle as XmlNode);
            XmlElement resultEle = doc.CreateElement("Result");
            resultEle.InnerXml = result;
            root.AppendChild(resultEle as XmlNode);
            XmlElement messageEle = doc.CreateElement("Message");
            messageEle.InnerXml = message;
            root.AppendChild(messageEle as XmlNode);

            response.ContentType = "text/xml"; //??
            response.Clear();
            response.Write(doc.InnerXml);
        }

        /// <summary>
        /// 包装XML格式的HttpResponse
        /// </summary>
        /// <param name="success">执行结果</param>
        /// <param name="result">执行成功时：返回值；执行失败时：错误编码</param>
        /// <param name="message">附加信息（一般为错误信息）</param>
        public static void WrapXMLResponse(bool success, string result, string message)
        {
            WrapXMLResponse(success, result, message, "utf-8");
        }

        #endregion


        #region 包装DataTable格式返回结果(已注释)

        ///// <summary>
        ///// 返回DataTable时，表数据对应的键名称。
        ///// </summary>
        public static string DataKeyName
        {
            get { return "Data"; }
        }

        ///// <summary>
        ///// 返回DataTable时，总条目对应的键名称。
        ///// </summary>
        public static string TotalCountKeyName
        {
            get { return "TotalCount"; }
        }


        #endregion


        #region Others

        /// <summary>
        /// 在HttpRequest中的[响应数据类型]的键名。
        /// </summary>
        public static string ResponseDataTypeKeyName
        {
            get { return "_ResponseDataType"; }
        }

        /// <summary>
        /// 判断当前请求为ajax提交方式。
        /// 一般ajax请求都会在header中标记X-Requested-With为XMLHttpRequest。
        /// </summary>
        public static bool IsAjaxRequest
        {
            //X-Requested-With	XMLHttpRequest
            get
            {
                HttpRequest request = HttpContext.Current.Request;
                return (request.Headers["x-requested-with"] != null && request.Headers["x-requested-with"].Trim().ToLower().Equals("xmlhttprequest"));
            }
        }

        #endregion
    }
}