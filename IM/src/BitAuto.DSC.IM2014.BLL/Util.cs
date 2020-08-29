
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using System.Reflection;
using BitAuto.Utils;
using System.IO;
using System.Threading;
using System.Web;
using System.Collections;
using System.Data;
using System.Configuration;

using System.Xml;

using System.Data.SqlClient;
using System.Security.Cryptography;
using BitAuto.Utils.Config;


namespace BitAuto.DSC.IM2014.BLL
{
    public class Util
    {

        /// <summary>
        /// 将当前URL格式化，参数转换Escape编码
        /// </summary>
        /// <returns></returns>
        public static string GetUrl()
        {
            string new_Url = string.Empty;
            string url = "";
            if (System.Web.HttpContext.Current != null)
            {
                url = System.Web.HttpContext.Current.Request.Url.Query.ToLower().TrimStart('?');
                url = Regex.Replace(url, @"page=(\d+)", "");
                url = Regex.Replace(url, @"r=(0.\d+)", "");
                string[] array = url.Split('&');

                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].Trim() != string.Empty)
                    {
                        string[] para = array[i].Split('=');
                        if (para.Length == 2)
                        {
                            new_Url += para[0] + '=' +
                            EscapeString(System.Web.HttpUtility.UrlDecode(para[1].Trim())) + '&';
                        }
                    }
                }
                return new_Url.TrimEnd('&');
            }
            else
            {
                return "";
            }
        }

        public static string GetEnumOptText(Type type, int value)
        {
            FieldInfo[] fields = type.GetFields();
            for (int i = 1, count = fields.Length; i < count; i++)
            {
                FieldInfo field = fields[i];
                if (((int)Enum.Parse(type, field.Name)).ToString() == value.ToString())
                {
                    object[] objs = field.GetCustomAttributes(typeof(EnumTextValueAttribute), false);
                    if (objs == null || objs.Length == 0)
                    {
                        return field.Name;
                    }
                    else
                    {
                        EnumTextValueAttribute da = (EnumTextValueAttribute)objs[0];
                        return da.Text;
                    }
                }
            }
            return "";
        }

        public static DataTable GetEnumDataTable(Type type)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("name");
            dt.Columns.Add("value");
            FieldInfo[] fields = type.GetFields();
            for (int i = 1, count = fields.Length; i < count; i++)
            {
                FieldInfo field = fields[i];
                DataRow dr = dt.NewRow();
                object[] objs = field.GetCustomAttributes(typeof(EnumTextValueAttribute), false);
                if (objs == null || objs.Length == 0)
                {
                    dr["name"] = field.Name;
                }
                else
                {
                    EnumTextValueAttribute da = (EnumTextValueAttribute)objs[0];
                    dr["name"] = da.Text;
                }
                dr["value"] = (int)Enum.Parse(type, field.Name);
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// ASPNET实现javascript的escape 
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>返回加密后字符串</returns>
        public static string EscapeString(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                StringBuilder sb = new StringBuilder();
                byte[] ba = Encoding.Unicode.GetBytes(str);
                for (int i = 0; i < ba.Length; i += 2)
                {
                    sb.Append("%u");
                    sb.Append(ba[i + 1].ToString("X2"));
                    sb.Append(ba[i].ToString("X2"));
                }
                return sb.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// ASPNET实现javascript的unescape
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>返回解密后字符串</returns>
        public static string UnEscapeString(string s)
        {
            if ((!string.IsNullOrEmpty(s)) && s.Length > 2)
            {
                string str = s.Remove(0, 2);//删除最前面两个＂%u＂
                string[] strArr = str.Split(new string[] { "%u" }, StringSplitOptions.None);//以子字符串＂%u＂分隔
                byte[] byteArr = new byte[strArr.Length * 2];
                for (int i = 0, j = 0; i < strArr.Length; i++, j += 2)
                {
                    byteArr[j + 1] = Convert.ToByte(strArr[i].Substring(0, 2), 16); //把十六进制形式的字串符串转换为二进制字节
                    byteArr[j] = Convert.ToByte(strArr[i].Substring(2, 2), 16);
                }
                str = System.Text.Encoding.Unicode.GetString(byteArr);　//把字节转为unicode编码
                return str;
            }
            return string.Empty;
        }

        #region HTML特殊字符转换


        public static string HtmlEncode(string theString)
        {
            theString = theString.Replace(">", "&gt;");
            theString = theString.Replace("<", "&lt;");
            theString = theString.Replace(" ", " &nbsp;");
            theString = theString.Replace(" ", " &nbsp;");
            theString = theString.Replace("\"", "&quot;");
            theString = theString.Replace("\'", "&#39;");
            theString = theString.Replace("\n", "<br/> ");
            return theString;
        }

        public static string HtmlDiscode(string theString)
        {
            theString = theString.Replace("&gt;", ">");
            theString = theString.Replace("&lt;", "<");
            theString = theString.Replace("&nbsp;", " ");
            theString = theString.Replace(" &nbsp;", " ");
            theString = theString.Replace("&quot;", "\"");
            theString = theString.Replace("&#39;", "\'");
            theString = theString.Replace("<br/>", "\n");
            theString = theString.Replace("&copy;", "©");
            theString = theString.Replace("&middot;", "·");

            return theString;
        }

        //用正则表达式过滤html标记的




        #endregion

        /// <summary>
        /// 验证电话号码（包括手机和座机）
        /// </summary>
        /// <param name="str_telephone"></param>
        /// <returns></returns>
        public static bool IsTelephone(string str_telephone)
        {
            return IsTelNumber(str_telephone) || IsHandset(str_telephone);
        }
        /// <summary>
        /// 取字符串长度
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetLength(string str)
        {
            if (str.Length == 0) return 0;

            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            byte[] s = ascii.GetBytes(str);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }
            }

            return tempLen;
        }
        /// <summary>
        /// 验证座机
        /// </summary>
        /// <param name="str_handset"></param>
        /// <returns></returns>
        public static bool IsTelNumber(string str_handset)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_handset, @"^(([0\+]\d{2,3})?(0\d{2,3}))(\d{7,8})$");

        }
        /// <summary>
        /// 验证手机
        /// </summary>
        /// <param name="str_handset"></param>
        /// <returns></returns>
        public static bool IsHandset(string str_handset)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_handset, @"^[1]+[3,4,5,8]+\d{9}$");

        }
        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="str_Email"></param>
        /// <returns></returns>
        public static bool IsEmail(string str_Email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_Email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
        /// <summary>
        /// 获取请求超时时间秒数
        /// </summary>
        /// <returns></returns>
        public static int GetConnectionTimeoutSeconds()
        { 
            int result=0;
            if(int.TryParse(BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("TimeOut"),out result))
            {
                return result;
            }

            return 5;
        }
        /// <summary>
        /// 获取坐席空闲超时时间,也就是没有长连接多久坐席对象将被清除
        /// </summary>
        /// <returns></returns>
        public static int GetConnectionIdleSecondsAgent()
        {
            int result = 0;
            if (int.TryParse(BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("TimeidleAgent"), out result))
            {
                return result;
            }

            return 600;
        }

        /// <summary>
        /// 获取网友空闲超时时间,也就是没有长连接多久坐席对象将被清除
        /// </summary>
        /// <returns></returns>
        public static int GetConnectionIdleSecondsUser()
        {
            int result = 0;
            if (int.TryParse(BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("TimeidleUser"), out result))
            {
                return result;
            }

            return 600;
        }

        /// <summary>
        /// 网友无发送消息超时时长,也就是多久没有发消息网友对象将被清除 
        /// </summary>
        /// <returns></returns>
        public static int GetSendMessageIdleSeconds()
        {
            int result = 0;
            if (int.TryParse(BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("TimeidleSendMessage"), out result))
            {
                return result;
            }

            return 600;
        }

        /// <summary>
        /// 批量插入DB数据方法
        /// </summary>
        /// <param name="dt">Datatable</param>
        /// <param name="conn">链接字符串</param>
        /// <param name="tableName">目标表名</param>
        /// <param name="batchSize">批次大小</param>
        /// <param name="list">映射列表</param>
        public static void BulkCopyToDB(DataTable dt, string conn, string tableName, int batchSize, IList<SqlBulkCopyColumnMapping> list)
        {
            if (dt.Rows.Count > 0)
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default,
                                   transaction))
                        {
                            bulkCopy.BulkCopyTimeout = 1800;
                            if (list != null && list.Count > 0)
                            {
                                foreach (SqlBulkCopyColumnMapping sc in list)
                                {
                                    bulkCopy.ColumnMappings.Add(sc);
                                }
                            }
                            bulkCopy.BatchSize = batchSize;
                            bulkCopy.DestinationTableName = tableName;

                            try
                            {
                                bulkCopy.WriteToServer(dt);
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                BLL.Loger.Log4Net.Error(ex.Message, ex);
                                transaction.Rollback();
                            }
                            finally
                            {
                                bulkCopy.Close();
                            }
                        }
                    }
                }
            }
        }

    }

}
