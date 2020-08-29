using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using BitAuto.DSC.IM_2015.Entities.Constants;
using BitAuto.Utils;
using BitAuto.DSC.IM_2015.Entities;
using System.IO;
using System.Configuration;
using BitAuto.Services.Organization.Remoting;
using System.Security.Cryptography;
using System.Net;
using System.Runtime.Serialization.Json;
using BitAuto.Utils.Config;
using System.Collections;

namespace BitAuto.DSC.IM_2015.BLL
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
            int result = 0;
            if (int.TryParse(BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("TimeOut"), out result))
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
        /// 获取最大接待网友数，包括所有业务线
        /// </summary>
        /// <returns></returns>
        public static int GetMaxNetFrinedNumber()
        {
            int result = 0;
            if (int.TryParse(BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("MaxNetFriend"), out result))
            {
                return result;
            }

            return 5000;
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


        /// <summary>
        /// 得到当前登录用户的ID
        /// </summary>
        public static int GetLoginUserID()
        {
            try
            {
                BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
                bool isLogin = BitAuto.YanFa.SysRightManager.Common.UserInfo.IsLogin();
                if (isLogin == false) { throw new Exception("还没有登录"); }//？？
            }
            catch (Exception exp)
            {
                throw new Exception("还没有登录");
            }

            int userID = -1;
            System.Web.HttpContext webHttp = System.Web.HttpContext.Current;
            if (webHttp != null && webHttp.Session["userid"] != null && webHttp.Session["userid"].ToString() != "")
            {
                string s = webHttp.Session["userid"].ToString().Trim();
                bool success = int.TryParse(s, out userID);
                //if (success == false) { throw new Exception(string.Format("无法将session中的[{0}]转换为数字。", s)); }
            }
            else
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    string username = HttpContext.Current.User.Identity.Name.ToLower();
                    if (username.StartsWith("tech\\"))
                    {
                        username = username.Substring(5, username.Length - 5);
                    }
                    int ret = BitAuto.YanFa.SysRightManager.Common.UserInfo.Login(username);
                    if (ret > 0)
                    {
                        BitAuto.YanFa.SysRightManager.Common.UserInfo.passport(ret);
                        return GetLoginUserID();
                    }
                }
            }
            //else
            //{
            //    throw new Exception("无法读取相关session");
            //}
            return userID;
        }
        /// <summary>
        /// 验证用户功能点权限
        /// </summary>
        /// <param name="moduleId">权限系统功能点模块编号</param>
        /// <returns></returns>
        public static bool CheckButtonRight(string moduleId)
        {
            return BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.CheckRight(moduleId, ConfigurationManager.AppSettings["ThisSysID"].ToString(), BLL.Util.GetLoginUserID());
        }
        /// <summary>
        /// 获取登录者真实姓名
        /// </summary>
        /// <returns></returns>
        public static string GetLoginRealName()
        {
            string realName = string.Empty;
            try
            {
                BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
                bool isLogin = BitAuto.YanFa.SysRightManager.Common.UserInfo.IsLogin();
                if (isLogin)
                {
                    System.Web.HttpContext webHttp = System.Web.HttpContext.Current;
                    if (webHttp != null && webHttp.Session["truename"] != null && webHttp.Session["truename"].ToString() != "")
                    {
                        realName = webHttp.Session["truename"].ToString().Trim();
                    }
                }
            }
            catch (Exception)
            {
                realName = string.Empty;
            }
            return realName;
        }

        public static string GetCurrentRequestFormStr(string r)
        {
            return HttpContext.Current.Request.Form[r] == null ? string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request.Form[r].Trim());
        }

        public static int GetCurrentRequestFormInt(string r)
        {
            int val = Constant.INT_INVALID_VALUE;
            try
            {
                if (HttpContext.Current.Request.Form[r] != null)
                {
                    val = int.Parse(HttpContext.Current.Request.Form[r].Trim());
                }
            }
            catch (Exception)
            {
                return val;
            }
            return val;
        }

        public static string GetCurrentRequestQueryStr(string r)
        {
            if (HttpContext.Current.Request.QueryString[r] == null)
            {
                return string.Empty;
            }
            else
            {
                return HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString[r].Trim());
            }
        }

        public static int GetCurrentRequestQueryInt(string r)
        {
            int val = Constant.INT_INVALID_VALUE;
            try
            {
                if (HttpContext.Current.Request.QueryString[r] != null)
                {
                    val = int.Parse(HttpContext.Current.Request.QueryString[r].Trim());
                }
            }
            catch (Exception e)
            {
                return val;
            }
            return val;
        }

        public static string GetCurrentRequestStr(string r)
        {
            if (System.Web.HttpContext.Current.Request[r] == null)
            {
                return string.Empty;
            }
            else
            {
                return System.Web.HttpUtility.UrlDecode(System.Web.HttpContext.Current.Request[r].ToString().Trim());
            }
        }

        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static T BindQuery<T>(HttpContext context)
        {
            var request = context.Request;
            var entityType = typeof(T);

            var model = Activator.CreateInstance<T>();

            foreach (var property in entityType.GetProperties())
            {
                if ((request.Form[property.Name] == null || request.Form[property.Name] == "") && (request.QueryString[property.Name] == null || request.QueryString[property.Name] == ""))
                {
                    continue;
                }
                string value1 = "";
                if (request.Form[property.Name] != null && request.Form[property.Name] != "")
                {
                    value1 = request.Form[property.Name].ToString();
                }
                else
                {
                    value1 = request.QueryString[property.Name].ToString();
                }
                var pType = property.PropertyType;
                //字符串这就样走吧
                if (pType == typeof(string))
                {
                    value1 = HttpUtility.UrlDecode(value1).Trim();
                    property.SetValue(model, value1, null);
                    // BLL.Loger.Log4Net.Info("model实体，属性：" + property.Name + "，值为：" + value1);
                    continue;
                }
                //可空
                if (pType.IsGenericType && pType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    Type[] typeArray = pType.GetGenericArguments();
                    pType = typeArray[0];
                }
                try
                {
                    object value2 = Convert.ChangeType(value1, pType);
                    property.SetValue(model, value2, null);
                    // BLL.Loger.Log4Net.Info("model实体，属性：" + property.Name + "，值为：" + value2);
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Error("转换model实体失败：", ex);
                }

            }
            return model;
        }

        public static string DataTableToJson(DataTable dt)
        {
            string s = "";
            s += "[";
            foreach (DataRow dr in dt.Rows)
            {
                s += "{";
                foreach (DataColumn dc in dt.Columns)
                {
                    s += "'" + dc.ColumnName + "':'" + CommonFunc.ObjectToString(dr[dc]) + "',";
                }
                s = s.TrimEnd(',');
                s += "},";
            }
            s = s.TrimEnd(',');
            s += "]";
            return s;
        }

        /// <summary>
        /// DataTable 对象 转换为Json 字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableObjectToJson(DataTable dt)
        {
            ArrayList arrayList = new ArrayList();
            foreach (DataRow dataRow in dt.Rows)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>(); //实例化一个参数集合
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName].ToString());
                }
                arrayList.Add(dictionary); //ArrayList集合中添加键值
            }
            return Newtonsoft.Json.JavaScriptConvert.SerializeObject(arrayList);//返回一个json字符串
        }

        public static void ExportToSCV(string filename, DataTable dt, bool isTab = true)
        {
            filename = filename.Replace(" ", "");
            var exportString = BLL.Util.TableToSCV(dt, isTab);
            HttpContext.Current.Response.HeaderEncoding = Encoding.Default;
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename + ".csv");
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
            HttpContext.Current.Response.Write(exportString);
            HttpContext.Current.Response.End();



        }
        /// <summary>
        /// 实现导出部分：导出内容的拼接
        /// </summary>
        /// <param name="dt">数据表</param>
        /// 增加一个参数，标识 是否对数字列 增加tab制表符，默认true-需要，false-不需要 add lxw 14.5.29
        public static StringWriter TableToSCV(DataTable dt, bool isTab)
        {
            long longval = 0;
            var exportString = new StringWriter();
            string captionStr = "";
            //同时拼接成字符串，用于导出
            foreach (DataColumn column in dt.Columns)
            {
                var caption = GetCsvStr(column.ColumnName);  // "\"" + column.ColumnName + "\"";

                if (!string.IsNullOrEmpty(captionStr))
                {
                    captionStr += "," + caption;
                }
                else
                {
                    captionStr += caption;
                }
            }
            //写入表头信息
            exportString.WriteLine(captionStr);

            //写入表的内容
            foreach (DataRow dr in dt.Rows)
            {
                for (var i = 0; i < dt.Columns.Count; i++)
                {
                    var column = dt.Columns[i];
                    var columnName = column.ColumnName;
                    if (dt.Columns[columnName].DataType == typeof(string))
                    {
                        string newstr = GetCsvStr(dr[columnName].ToString());

                        if (isTab == true && Regex.IsMatch(newstr, @"^\d+$"))// 如果全是数字
                        {
                            exportString.Write(newstr + "\t");
                        }
                        else
                        {
                            exportString.Write(newstr);
                        }
                    }
                    else if (dt.Columns[columnName].DataType == typeof(DateTime))
                    {
                        DateTime dateTmp;
                        if (DateTime.TryParse(dr[columnName].ToString(), out dateTmp))
                        {
                            exportString.Write(dateTmp.ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        else
                        {
                            exportString.Write(dr[columnName]);
                        }
                    }
                    else
                    {
                        exportString.Write(dr[columnName]);
                    }
                    if (i < dt.Columns.Count - 1)
                    {
                        exportString.Write(",");
                    }
                }
                exportString.WriteLine();
            }
            exportString.Close();

            return exportString;

        }

        private static string GetCsvStr(string oldstr)
        {
            if (oldstr.IndexOf(',') != -1 || oldstr.IndexOf('"') != -1 || oldstr.IndexOf('\n') != -1)
            {
                oldstr = oldstr.Replace('"', '“');
                oldstr = "\"" + oldstr + "\"";
            }
            return oldstr;
        }
        /// <summary>
        /// 取业务线标识（如hmc，ycw，sc,jr）
        /// </summary>
        /// <param name="sourcetype"></param>
        /// <returns></returns>
        public static string GetSourceType(string sourcetype)
        {
            string sourcetypestr = System.Configuration.ConfigurationManager.AppSettings["SourceType"];
            string[] Arraytype = sourcetypestr.Split('|');
            string valuestr = string.Empty;
            foreach (var str in Arraytype)
            {
                if (str.Split(',')[0] == sourcetype)
                {
                    valuestr = str.Split(',')[1];
                    break;
                }
            }
            return valuestr;
        }
        /// <summary>
        /// 取业务线标识（如hmc，ycw，sc,jr）
        /// </summary>
        /// <param name="sourcetype"></param>
        /// <returns></returns>
        public static string GetSourceTypeName(string sourcetype)
        {
            string sourcetypestr = System.Configuration.ConfigurationManager.AppSettings["SourceType"];
            string[] Arraytype = sourcetypestr.Split('|');
            string valuestr = string.Empty;
            foreach (var str in Arraytype)
            {
                if (str.Split(',')[1] == sourcetype)
                {
                    valuestr = str.Split(',')[0];
                    break;
                }
            }
            return valuestr;
        }
        /// <summary>
        /// 获取所有的业务线数据——供下拉列表数据绑定用
        /// </summary>
        /// <param name="HasDefault">true:有“请选择”选项；false：没有“请选择”选项</param>
        /// <returns></returns>
        public static List<SourceType> GetAllSourceType(bool HasDefault)
        {
            List<SourceType> list = new List<SourceType>();
            if (HasDefault)
            {
                SourceType newItem = new SourceType();
                newItem.SourceTypeName = "请选择";
                newItem.SourceTypeValue = "-1";
                list.Add(newItem);
            }

            string sourcetypestr = System.Configuration.ConfigurationManager.AppSettings["SourceType"];
            string[] Arraytype = sourcetypestr.Split('|');
            string valuestr = string.Empty;
            foreach (var str in Arraytype)
            {
                string[] listItem = str.Split(',');
                if (listItem.Length == 2)
                {
                    SourceType stItem = new SourceType();
                    stItem.SourceTypeName = listItem[0];
                    stItem.SourceTypeValue = listItem[1];
                    list.Add(stItem);
                }
            }

            return list;
        }


        /// <summary>
        /// 获取所有的业务线数据——供下拉列表数据绑定用,把 易车网旗下各频道做了汇总
        /// </summary>
        /// <param name="HasDefault">true:有“请选择”选项；false：没有“请选择”选项</param>
        /// <returns></returns>
        public static List<SourceType> GetAllSourceHaveYiCheTotalType(bool HasDefault)
        {
            List<SourceType> list = new List<SourceType>();
            if (HasDefault)
            {
                SourceType newItem = new SourceType();
                newItem.SourceTypeName = "请选择";
                newItem.SourceTypeValue = "-1";
                list.Add(newItem);
            }

            string sourcetypestr = System.Configuration.ConfigurationManager.AppSettings["SourceType"];
            string[] Arraytype = sourcetypestr.Split('|');
            string valuestr = string.Empty;
            foreach (var str in Arraytype)
            {
                string[] listItem = str.Split(',');
                if (listItem.Length == 2)
                {
                    if (!IsInYicheLine(listItem[1]))
                    {
                        SourceType stItem = new SourceType();
                        stItem.SourceTypeName = listItem[0];
                        stItem.SourceTypeValue = listItem[1];
                        list.Add(stItem);
                    }

                }
            }
            SourceType stItemyiche = new SourceType();
            stItemyiche.SourceTypeName = "易车汇总";
            stItemyiche.SourceTypeValue = "100";
            list.Add(stItemyiche);
            return list;
        }
        /// <summary>
        /// 获取易车旗下各业务线数据——供下拉列表数据绑定用,sourcetype是一级业务线标识,取二级可以根据此方法是否返回数据来判断二级是否可用
        /// </summary>
        ///<param name="sourcetype">sourcetype</param>
        /// <param name="HasDefault">true:有“请选择”选项；false：没有“请选择”选项</param>
        /// <returns></returns>
        public static List<SourceType> GetYiCheSourceType(string sourcetype, bool HasDefault)
        {
            List<SourceType> list = new List<SourceType>();
            if (sourcetype == "100")
            {
                if (HasDefault)
                {
                    SourceType newItem = new SourceType();
                    newItem.SourceTypeName = "请选择";
                    newItem.SourceTypeValue = "-1";
                    list.Add(newItem);
                }

                string sourcetypestr = System.Configuration.ConfigurationManager.AppSettings["SourceType"];
                string[] Arraytype = sourcetypestr.Split('|');
                string valuestr = string.Empty;
                foreach (var str in Arraytype)
                {
                    string[] listItem = str.Split(',');
                    if (listItem.Length == 2)
                    {
                        if (IsInYicheLine(listItem[1]))
                        {
                            SourceType stItem = new SourceType();
                            stItem.SourceTypeName = listItem[0];
                            stItem.SourceTypeValue = listItem[1];
                            list.Add(stItem);
                        }

                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 根据业务线标识判断是否在易车网旗下
        /// </summary>
        /// <returns></returns>
        public static bool IsInYicheLine(string typestr)
        {
            bool flag = false;
            string sourceyichestr = System.Configuration.ConfigurationManager.AppSettings["YiCheLine"];
            if (!string.IsNullOrEmpty(sourceyichestr))
            {

                string[] listItem = sourceyichestr.Split(',');
                for (int i = 0; i < listItem.Length; i++)
                {
                    if (typestr == listItem[i])
                    {
                        flag = true;
                        break;
                    }
                }
            }
            return flag;
        }
        /// <summary>
        /// 通过权限系统EID找到用户在HR系统中的Name
        /// </summary>
        /// <param name="limitEID">权限系统EID</param>
        /// <returns>HR系统中的Name</returns>
        public static string GetNameInHRLimitEID(int limitEID)
        {
            string NameInHR = string.Empty;
            NameInHR = BLL.Util.GetEmployeeNameByEID(BLL.Util.GetHrEIDByLimitEID(limitEID));
            return NameInHR;
        }
        /// <summary>
        /// 根据eid取员工姓名
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        public static string GetEmployeeNameByEID(int eid)
        {
            string realName = string.Empty;

            IOrganizationService service = (IOrganizationService)Activator.GetObject(typeof(IOrganizationService),
                                                         ConfigurationManager.AppSettings["OrganizationService"]);
            Employee e = service.GetEmployeeByEmployeeID(eid);
            if (e != null)
            {
                realName = e.CnName;
            }
            return realName;
        }

        /// <summary>
        /// 通过权限系统EID找到HR系统EID
        /// </summary>
        /// <param name="limitEID">权限系统EID</param>
        /// <returns>HR系统EID</returns>
        public static int GetHrEIDByLimitEID(int limitEID)
        {
            int HrEID = 0;
            IOrganizationService service = (IOrganizationService)Activator.GetObject(typeof(IOrganizationService),
                                                         ConfigurationManager.AppSettings["OrganizationService"]);
            string adname = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerDomainAccountByUserID(limitEID);
            if (!string.IsNullOrEmpty(adname))
            {
                Employee EmployeeModel = service.GetEmployeeByDomainAccountWithDimission(adname);
                if (EmployeeModel != null)
                {
                    HrEID = EmployeeModel.EmployeeID;
                }
            }

            return HrEID;
        }

        #region 加密，解密

        private const string key = "bitauto"; //默认密钥

        public static string EncryptString(string inputStr)
        {
            return EncryptString(inputStr, key);
        }

        public static string DecryptString(string inputStr)
        {
            return DecryptString(inputStr, key);
        }

        /// <summary> 
        /// 加密字符串 
        /// </summary> 
        /// <param name="inputStr">输入字符串</param> 
        /// <param name="keyStr">密码，可以为“”</param> 
        /// <returns>输出加密后字符串</returns> 
        public static string EncryptString(string inputStr, string keyStr)
        {
            byte[] sKey;
            byte[] sIV;

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            if (keyStr == "")
            {
                keyStr = key;
            }

            byte[] inputByteArray = Encoding.Default.GetBytes(inputStr);
            byte[] keyByteArray = Encoding.Default.GetBytes(keyStr);

            SHA1 ha = new SHA1Managed();
            byte[] hb = ha.ComputeHash(keyByteArray);
            sKey = new byte[8];
            sIV = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                sKey[i] = hb[i];
            }
            for (int i = 8; i < 16; i++)
            {
                sIV[i - 8] = hb[i];
            }

            des.Key = sKey;
            des.IV = sIV;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();

            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }

            cs.Close();
            ms.Close();
            return ret.ToString();
        }


        /// <summary> 
        /// 解密字符串 
        /// </summary> 
        /// <param name="inputStr">要解密的字符串</param> 
        /// <param name="keyStr">密钥</param> 
        /// <returns>解密后的结果</returns> 
        public static string DecryptString(string inputStr, string keyStr)
        {
            byte[] sKey;
            byte[] sIV;

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            if (keyStr == "")
            {
                keyStr = key;
            }

            byte[] inputByteArray = new byte[inputStr.Length / 2];

            for (int x = 0; x < inputStr.Length / 2; x++)
            {
                int i = (Convert.ToInt32(inputStr.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            byte[] keyByteArray = Encoding.Default.GetBytes(keyStr);

            SHA1 ha = new SHA1Managed();
            byte[] hb = ha.ComputeHash(keyByteArray);

            sKey = new byte[8];
            sIV = new byte[8];

            for (int i = 0; i < 8; i++)
            {
                sKey[i] = hb[i];
            }
            for (int i = 8; i < 16; i++)
            {
                sIV[i - 8] = hb[i];
            }

            des.Key = sKey;
            des.IV = sIV;

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            return System.Text.Encoding.Default.GetString(ms.ToArray()).TrimEnd(' ');
        }


        #endregion

        ///// <summary>
        ///// 取本机ip
        ///// </summary>
        ///// <returns></returns>
        //public static string IpToLong()
        //{
        //    string hostname = Dns.GetHostName();//得到本机名   
        //    //IPHostEntry localhost = Dns.GetHostByName(hostname);//方法已过期，只得到IPv4的地址   
        //    IPHostEntry localhost = Dns.GetHostEntry(hostname);
        //    IPAddress localaddr = localhost.AddressList[0];
        //    return localaddr.ToString();
        //}


        public static long IpToLong()
        {
            string hostname = Dns.GetHostName();//得到本机名   
            IPHostEntry localhost = Dns.GetHostByName(hostname);//方法已过期，只得到IPv4的地址   
            //IPHostEntry localhost = Dns.GetHostEntry(hostname);
            IPAddress localaddr = localhost.AddressList[0];
            return (long)(uint)IPAddress.NetworkToHostOrder((int)IPAddress.Parse(localaddr.ToString()).Address);
        }

        public static string LongToIp(long address)
        {
            return IPAddress.Parse(address.ToString()).ToString();
        }

        public static string DataContractObject2Json(object obj, Type type)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(type);
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        public static T DataContractJson2Object<T>(string strJson) where T : class
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(strJson)))     //构造函数能够接受Stream参数，因此你可以用内存流，文件流等等创建
            {
                var result = serializer.ReadObject(ms) as T;
                return result;
            }

        }
        /// 验证域名
        /// <summary>
        /// 验证域名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool CheckDomainName(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                Loger.Log4Net.Info("域名取到为空，不合法");
                return false;
            }

            System.Uri urlstr = new Uri(url);

            string DomainName = ConfigurationUtil.GetAppSettingValue("DomainName");
            //Loger.Log4Net.Info("合法域名：" + DomainName);
            string[] array = DomainName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string key in array)
            {
                if (urlstr.Host.ToLower().EndsWith(key.ToLower()))
                {
                    return true;
                }
            }
            Loger.Log4Net.Info("不合法域名：" + urlstr.Host);
            return false;
        }

        /// <summary>
        /// 客服名称去除括号内的部门
        /// </summary>
        /// <param name="trueName"></param>
        /// <returns></returns>
        public static string SubTrueName(string trueName)
        {
            if (trueName.IndexOf('(', 0) > 0)
            {
                return trueName.Substring(0, trueName.IndexOf('(', 0));
            }
            return trueName;
        }

        public static string GetLv(string aNumber, string bNumber)
        {
            if (!string.IsNullOrEmpty(aNumber) && !string.IsNullOrEmpty(bNumber) && bNumber != "0")
            {
                return (float.Parse(aNumber) / float.Parse(bNumber) * 100).ToString("N2") + "%";
            }
            return "-";
        }

    }





}
