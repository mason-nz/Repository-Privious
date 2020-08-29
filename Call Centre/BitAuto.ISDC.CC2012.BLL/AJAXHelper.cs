using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Data;
using System.IO;
using Newtonsoft.Json;

namespace BitAuto.ISDC.CC2012.BLL
{
    public static class AJAXHelper
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
            using (JsonWriter jsonWriter = new JsonTextWriter(swJson))
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

        /// <summary>
        /// 返回文本
        /// _ResponseDataType: 'json' or 'xml', 设置返回数据格式。
        /// </summary>
        public static void WrapResponse(bool success, string text, string message)
        {
            if (GetResponseDataType() == ResponseDataType.JSON)
            {
                AJAXHelper.WrapJsonResponse(success, text, message);
            }
            else if (GetResponseDataType() == ResponseDataType.XML)
            {
                AJAXHelper.WrapXMLResponse(success, text, message);
            }
            else
            {
            }
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

        ///// <summary>
        ///// 将DataTable以Json格式包装
        ///// </summary>
        public static void WrapJsonResponse4DataTable(bool success, DataTable dt, int totalCount, string message)
        {
            //(1)包装Datatable与TotalCount
            StringBuilder sbJson = new StringBuilder();
            StringWriter swJson = new StringWriter(sbJson);
            using (JsonWriter jsonWriter = new JsonTextWriter(swJson))
            {
                jsonWriter.Formatting = Newtonsoft.Json.Formatting.Indented;
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName(AJAXHelper.TotalCountKeyName);
                jsonWriter.WriteValue(totalCount);
                jsonWriter.WritePropertyName(AJAXHelper.DataKeyName);
                jsonWriter.WriteValue(Converter.DataTable2Json(dt));
                jsonWriter.WriteEndObject();
            }
            swJson.Flush();//清空缓存,并将缓冲的数据写出到基础设备
            //(2)
            AJAXHelper.WrapJsonResponse(success, sbJson.ToString(), message);
        }

        ///// <summary>
        ///// 将DataTable(某些列)以Json格式包装
        ///// </summary>
        public static void WrapJsonResponse4DataTable(bool success, DataTable dt, List<string> colIds, int totalCount, string message)
        {
            //(1)包装Datatable与TotalCount
            StringBuilder sbJson = new StringBuilder();
            StringWriter swJson = new StringWriter(sbJson);
            using (JsonWriter jsonWriter = new JsonTextWriter(swJson))
            {
                jsonWriter.Formatting = Newtonsoft.Json.Formatting.Indented;
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName(AJAXHelper.TotalCountKeyName);
                jsonWriter.WriteValue(totalCount);
                jsonWriter.WritePropertyName(AJAXHelper.DataKeyName);
                jsonWriter.WriteValue(Converter.DataTable2Json(dt, colIds));
                jsonWriter.WriteEndObject();
            }
            swJson.Flush();//清空缓存,并将缓冲的数据写出到基础设备
            //(2)
            AJAXHelper.WrapJsonResponse(success, sbJson.ToString(), message);
        }

        ///// <summary>
        ///// 将DataTable以XML格式包装
        ///// </summary>
        public static void WrapXMLResponse4DataTable(bool success, DataTable dt, int totalCount, string message)
        {
            //(1)包装Datatable与TotalCount
            XmlDocument doc = new XmlDocument();
            //doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", null));//"utf-8"
            XmlElement root = doc.CreateElement("root");

            XmlElement totalCountEle = doc.CreateElement(AJAXHelper.TotalCountKeyName);
            totalCountEle.InnerXml = totalCount.ToString();
            root.AppendChild(totalCountEle as XmlNode);

            XmlElement dataEle = doc.CreateElement(AJAXHelper.DataKeyName);
            dataEle.InnerXml = Converter.DataTable2XML(dt);
            root.AppendChild(dataEle as XmlNode);

            //(2)
            AJAXHelper.WrapXMLResponse(success, root.InnerXml, message);
        }


        ///// <summary>
        ///// 返回Datatable 及 TotalCount.
        ///// 兼容Json 和 XML
        ///// </summary>
        public static void WrapResponse(bool success, DataTable dt, int totalCount, string message)
        {
            if (GetResponseDataType() == ResponseDataType.JSON)
            {
                AJAXHelper.WrapJsonResponse4DataTable(success, dt, totalCount, message);
            }
            else if (GetResponseDataType() == ResponseDataType.XML)
            {
                AJAXHelper.WrapXMLResponse4DataTable(success, dt, totalCount, message);
            }
            else
            {
            }
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
        /// 得到响应的数据类型（JSON or XML）
        /// </summary>
        public static ResponseDataType GetResponseDataType()
        {
            return Converter.Str2ResponseDataType(HttpContext.Current.Request[AJAXHelper.ResponseDataTypeKeyName] + "");
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


        ///// <summary>
        ///// 判断是否登录。用在AJAX请求中。
        ///// </summary>
        //public static bool Authenticate()
        //{
        //    if (SysRightManager.Common.UserInfo.IsLogin() == false)
        //    {
        //        //HttpContext.Current.Server.Transfer("~/ErrorPages/AJAXNoAuthority.aspx");
        //        AJAXHelper.WrapResponse(false, "", "您还没有登录！");
        //        return false;
        //    }
        //    else return true;
        //}

        ///// <summary>
        ///// 判断是否登录。用在AJAX页面中。
        ///// </summary>
        //public static void PageAuthenticate()
        //{
        //    if (SysRightManager.Common.UserInfo.IsLogin() == false)
        //    {
        //        HttpContext.Current.Server.Transfer("~/ErrorPages/NoAuthority.aspx");
        //    }
        //}

    }

    public enum ResponseDataType
    {
        JSON,
        XML
    }


    public static class Converter
    {
        public static ResponseDataType Str2ResponseDataType(string value)
        {
            switch (value.Trim().ToLower())
            {
                case "xml":
                    return ResponseDataType.XML;
                case "json":
                    return ResponseDataType.JSON;
                default:
                    string type = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ResponseDataType", false).ToLower();
                    switch (type)
                    {
                        case "xml":
                            return ResponseDataType.XML;
                        case "json":
                            return ResponseDataType.JSON;
                        default:
                            return ResponseDataType.JSON;
                    }
            }
        }

        /// <summary>
        /// 将DataTable转换为Json
        /// </summary>
        public static string DataTable2Json(DataTable dt)
        {
            StringBuilder sbJson = new StringBuilder();
            StringWriter swJson = new StringWriter(sbJson);
            using (JsonWriter jsonWriter = new JsonTextWriter(swJson))
            {
                jsonWriter.Formatting = Newtonsoft.Json.Formatting.Indented;

                jsonWriter.WriteStartArray();
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataColumnCollection dcc = dt.Columns;

                    object cellValue = null;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        jsonWriter.WriteStartObject();
                        for (int k = 0; k < dcc.Count; k++)
                        {
                            jsonWriter.WritePropertyName(dcc[k].Caption);

                            cellValue = dt.Rows[i][k];
                            if (Convert.IsDBNull(cellValue))
                            {
                                jsonWriter.WriteValue("");
                                continue;
                            }
                            if (cellValue.ToString().Trim().Equals(string.Empty))
                            {
                                jsonWriter.WriteValue("");
                                continue;
                            }
                            switch (dcc[k].DataType.Name)
                            {
                                case "String":
                                    jsonWriter.WriteValue(Convert.ToString(cellValue.ToString()));
                                    break;
                                case "Boolean":
                                    jsonWriter.WriteValue(Convert.ToBoolean(cellValue.ToString()));
                                    break;
                                case "DateTime":
                                    jsonWriter.WriteValue(Convert.ToDateTime(cellValue.ToString()));
                                    break;
                                case "Byte":
                                    jsonWriter.WriteValue(Convert.ToByte(cellValue.ToString()));
                                    break;
                                case "Char":
                                    jsonWriter.WriteValue(Convert.ToChar(cellValue.ToString()));
                                    break;
                                case "Decimal":
                                    jsonWriter.WriteValue(Convert.ToDecimal(cellValue.ToString()));
                                    break;
                                case "Double":
                                    jsonWriter.WriteValue(Convert.ToDouble(cellValue.ToString()));
                                    break;
                                case "Int16":
                                    jsonWriter.WriteValue(Convert.ToInt16(cellValue.ToString()));
                                    break;
                                case "Int32":
                                    jsonWriter.WriteValue(Convert.ToInt32(cellValue.ToString()));
                                    break;
                                case "Int64":
                                    jsonWriter.WriteValue(Convert.ToInt64(cellValue.ToString()));
                                    break;
                                case "SByte":
                                    jsonWriter.WriteValue(Convert.ToSByte(cellValue.ToString()));
                                    break;
                                case "Single":
                                    jsonWriter.WriteValue(Convert.ToSingle(cellValue.ToString()));
                                    break;
                                case "TimeSpan":
                                    jsonWriter.WriteValue(Convert.ToDateTime(cellValue.ToString()));
                                    break;
                                case "UInt16":
                                    jsonWriter.WriteValue(Convert.ToUInt16(cellValue.ToString()));
                                    break;
                                case "UInt32":
                                    jsonWriter.WriteValue(Convert.ToUInt32(cellValue.ToString()));
                                    break;
                                default:
                                    break;
                            }
                        }
                        jsonWriter.WriteEndObject();
                    }
                }
                jsonWriter.WriteEndArray();
            }
            swJson.Flush();//清空缓存,并将缓冲的数据写出到基础设备
            return sbJson.ToString();
        }

        /// <summary>
        /// 将DataTable(某些列)转换为Json
        /// </summary>
        public static string DataTable2Json(DataTable dt, List<string> colNames)
        {
            StringBuilder sbJson = new StringBuilder();
            StringWriter swJson = new StringWriter(sbJson);
            using (JsonWriter jsonWriter = new JsonTextWriter(swJson))
            {
                jsonWriter.Formatting = Newtonsoft.Json.Formatting.Indented;

                jsonWriter.WriteStartArray();
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataColumnCollection dcc = dt.Columns;

                    object cellValue = null;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        jsonWriter.WriteStartObject();
                        //for (int k = 0; k < dcc.Count; k++)
                        foreach (string k in colNames)
                        {
                            jsonWriter.WritePropertyName(dcc[k].Caption);

                            cellValue = dt.Rows[i][k];
                            if (Convert.IsDBNull(cellValue))
                            {
                                jsonWriter.WriteValue("");
                                continue;
                            }
                            if (cellValue.ToString().Trim().Equals(string.Empty))
                            {
                                jsonWriter.WriteValue("");
                                continue;
                            }
                            switch (dcc[k].DataType.Name)
                            {
                                case "String":
                                    jsonWriter.WriteValue(Convert.ToString(cellValue.ToString()));
                                    break;
                                case "Boolean":
                                    jsonWriter.WriteValue(Convert.ToBoolean(cellValue.ToString()));
                                    break;
                                case "DateTime":
                                    jsonWriter.WriteValue(Convert.ToDateTime(cellValue.ToString()));
                                    break;
                                case "Byte":
                                    jsonWriter.WriteValue(Convert.ToByte(cellValue.ToString()));
                                    break;
                                case "Char":
                                    jsonWriter.WriteValue(Convert.ToChar(cellValue.ToString()));
                                    break;
                                case "Decimal":
                                    jsonWriter.WriteValue(Convert.ToDecimal(cellValue.ToString()));
                                    break;
                                case "Double":
                                    jsonWriter.WriteValue(Convert.ToDouble(cellValue.ToString()));
                                    break;
                                case "Int16":
                                    jsonWriter.WriteValue(Convert.ToInt16(cellValue.ToString()));
                                    break;
                                case "Int32":
                                    jsonWriter.WriteValue(Convert.ToInt32(cellValue.ToString()));
                                    break;
                                case "Int64":
                                    jsonWriter.WriteValue(Convert.ToInt64(cellValue.ToString()));
                                    break;
                                case "SByte":
                                    jsonWriter.WriteValue(Convert.ToSByte(cellValue.ToString()));
                                    break;
                                case "Single":
                                    jsonWriter.WriteValue(Convert.ToSingle(cellValue.ToString()));
                                    break;
                                case "TimeSpan":
                                    jsonWriter.WriteValue(Convert.ToDateTime(cellValue.ToString()));
                                    break;
                                case "UInt16":
                                    jsonWriter.WriteValue(Convert.ToUInt16(cellValue.ToString()));
                                    break;
                                case "UInt32":
                                    jsonWriter.WriteValue(Convert.ToUInt32(cellValue.ToString()));
                                    break;
                                default:
                                    break;
                            }
                        }
                        jsonWriter.WriteEndObject();
                    }
                }
                jsonWriter.WriteEndArray();
            }
            swJson.Flush();//清空缓存,并将缓冲的数据写出到基础设备
            return sbJson.ToString();
        }

        /// <summary>
        /// 将DataTable转换为XML
        /// </summary>
        public static string DataTable2XML(DataTable dt)
        {
            //将DataTable输出XML形式
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            XmlTextWriter xmlw = new XmlTextWriter(sw);
            xmlw.Formatting = System.Xml.Formatting.Indented;
            dt.WriteXml(xmlw);
            xmlw.Flush();
            xmlw.Close();
            sw.Flush();
            sw.Close();
            return sb.ToString();
        }

        /// <summary>
        /// 将XML转换成DataSet形式
        /// </summary>
        public static DataSet XML2DataSet(string xml)
        {
            DataSet ds = new DataSet();
            StringReader sr = new StringReader(xml);
            XmlTextReader xmlr = new XmlTextReader(sr);
            ds.ReadXml(xmlr);
            xmlr.Close();
            sr.Close();
            return ds;
        }

        /// <summary>
        /// 将XML转换成String
        /// </summary>
        public static string XML2String(XmlDocument xmlDoc)
        {
            MemoryStream stream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, null);
            writer.Formatting = System.Xml.Formatting.Indented;
            xmlDoc.Save(writer);
            StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
            stream.Position = 0;
            string xmlString = sr.ReadToEnd();
            sr.Close();
            stream.Close();
            return xmlString;
        }

        /// <summary>
        /// 将表的一列拼接成String
        /// </summary>
        public static string Column2String(DataTable dt, string columnName, string prefix, string separator, string suffix)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(prefix);
            int i = 0;
            for (; i < dt.Rows.Count - 1; i++)
            {
                sb.Append(dt.Rows[i][columnName].ToString());
                sb.Append(separator);
            }
            sb.Append(dt.Rows[i][columnName].ToString());
            sb.Append(suffix);
            return sb.ToString();
        }

        /// <summary>
        /// 将表的一列输出成List
        /// </summary>
        public static List<string> Column2List(DataTable dt, string columnName)
        {
            List<string> list = new List<string>();
            int i = 0;
            for (; i < dt.Rows.Count - 1; i++)
            {
                list.Add(dt.Rows[i][columnName].ToString());
            }
            return list;
        }

        /// <summary>
        /// 将String分隔成List
        /// </summary>
        public static List<string> String2List(string source, char[] separator, StringSplitOptions option)
        {
            List<string> list = new List<string>();
            string[] array = source.Split(separator, option);
            foreach (string str in array)
            {
                list.Add(str);
            }
            return list;
        }

        /// <summary>
        /// 将String分隔成Int类型的List
        /// </summary>
        public static List<int> String2IntList(string source, char[] separator, StringSplitOptions option)
        {
            List<int> list = new List<int>();
            string[] array = source.Split(separator, option);
            foreach (string str in array)
            {
                int i = String2Int(str, int.MinValue);
                if (i != int.MinValue)
                {
                    list.Add(i);
                }
            }
            return list;
        }

        public static int String2Int(string value, int defaultValue)
        {
            int i = defaultValue;
            if (int.TryParse(value, out i) == true)
            {
                return i;
            }
            else
            {
                return defaultValue;
            }
        }

        public static string List2String(List<string> list, string split, string prefix, string postfix)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(prefix);
            if (list != null && list.Count > 0)
            {
                int i = 0;
                for (; i < list.Count - 1; i++)
                {
                    sb.Append(list[i]);
                    sb.Append(split);
                }
                sb.Append(list[i]);
            }
            sb.Append(postfix);
            return sb.ToString();
        }


        public static string List2String(List<int> list, string split, string prefix, string postfix)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(prefix);
            if (list != null && list.Count > 0)
            {
                int i = 0;
                for (; i < list.Count - 1; i++)
                {
                    sb.Append(list[i].ToString());
                    sb.Append(split);
                }
                sb.Append(list[i].ToString());
            }
            sb.Append(postfix);
            return sb.ToString();
        }

        public static string DateTimeStr2Str(string dateStr, string format)
        {
            DateTime dt;
            if (DateTime.TryParse(dateStr, out dt))
            {
                return dt.ToString(format);
            }
            else { return ""; }
        }
    }
}
