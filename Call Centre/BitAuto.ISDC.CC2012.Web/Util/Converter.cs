using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.IO;
using System.Xml;
using Newtonsoft.Json;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.WebUtil
{
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
            using (JsonWriter jsonWriter = new JsonWriter(swJson))
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
            using (JsonWriter jsonWriter = new JsonWriter(swJson))
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