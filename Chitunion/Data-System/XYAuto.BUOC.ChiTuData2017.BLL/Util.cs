using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using XYAuto.BUOC.ChiTuData2017.Entities;
using System.Reflection;
using XYAuto.Utils;
using System.IO;
using System.Threading;
using System.Web;
using System.Collections;
using System.Data;
using System.Configuration;
using System.Xml;
using System.Data.SqlClient;
using System.Security.Cryptography;
using XYAuto.Utils.Config;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using XYAuto.ITSC.Chitunion2017.Common;
using System.Drawing.Text;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing.Imaging;
using Xy.ImageFastDFS;

using XYAuto.BUOC.ChiTuData2017.Infrastruction;
using Loger = XYAuto.BUOC.ChiTuData2017.Infrastruction.Loger;

namespace XYAuto.BUOC.ChiTuData2017.BLL
{
    public class Util
    {
        public static int PageSize = 100;
        protected static string CleanImgURLPrefix = ConfigurationUtil.GetAppSettingValue("CleanImgURLPrefix");//文章中替换图片url后的域名
        private static Dictionary<String, ImageFormat> _imageFormats;

        public static Dictionary<String, ImageFormat> ImageFormats
        {
            get
            {
                return _imageFormats ?? (_imageFormats = GetImageFormats());
            }
        }

        public static object GetPropertyByName(object obj, string PropertyName)
        {
            string s = PropertyName;
            string sNew = ReadWord(ref s, ".");
            Type ot = obj.GetType();
            if (s.Length == 0)
            {
                return (ot.InvokeMember(sNew, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.GetField | BindingFlags.InvokeMethod | BindingFlags.Instance, null, obj, null));
            }
            else
            {
                object o = ot.InvokeMember(sNew, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.GetField | BindingFlags.InvokeMethod | BindingFlags.Instance, null, obj, null);
                return GetPropertyByName(o, s);
            }
        }

        public static string ReadWord(ref string Str, string ListSeparator)
        {
            int i;
            string rv;
            i = Str.IndexOf(ListSeparator);
            if (i != -1)
            {
                rv = Str.Substring(0, i);
                Str = Str.Substring(i + ListSeparator.Length, Str.Length - i - ListSeparator.Length);
                return (rv);
            }
            else
            {
                rv = Str;
                Str = "";
                return (rv);
            }
        }

        /// <summary>
        /// 获取枚举对应的文本描述
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum enumValue)
        {
            string str = enumValue.ToString();
            System.Reflection.FieldInfo field = enumValue.GetType().GetField(str);
            object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            if (objs == null || objs.Length == 0) return str;
            System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];
            return da.Description;
        }

        /// <summary>
        /// 根据枚举的值，得到枚举对应的文本
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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
                        return CommonFunction.ObjectToString(da.Text);
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
                    if (da.Text == null)
                    {
                        continue;
                    }
                    else
                    {
                        dr["name"] = da.Text;
                    }
                }
                dr["value"] = (int)Enum.Parse(type, field.Name);
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static DataTable GetEnumDataTable(Type type, string none_name)
        {
            DataTable dt = GetEnumDataTable(type);
            DataRow dr = dt.NewRow();
            dr["name"] = none_name;
            dr["value"] = -1;
            dt.Rows.InsertAt(dr, 0);
            return dt;
        }

        /// 给表插入一条默认的【请选择】数据
        /// <summary>
        /// 给表插入一条默认的【请选择】数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="namecol"></param>
        /// <param name="valuecol"></param>
        /// <param name="none_name"></param>
        /// <returns></returns>
        public static DataTable InsertEnumDataTable(DataTable dt, string valuecol, string namecol, string none_name)
        {
            DataRow dr = dt.NewRow();
            dr[namecol] = none_name;
            dr[valuecol] = -1;
            dt.Rows.InsertAt(dr, 0);
            return dt;
        }

        /// 根据特殊枚举id列表，获取枚举的按位或的值（枚举序号为1,2,4,8,16...）
        /// <summary>
        /// 根据特殊枚举id列表，获取枚举的按位或的值（枚举序号为1,2,4,8,16...）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="enumtype"></param>
        /// <returns></returns>
        public static int GetMutilEnumDataValue<T>(string ids)
        {
            int result = 0;
            foreach (string id in ids.Split(','))
            {
                int t = (int)Enum.Parse(typeof(T), id);
                result = result | t;
            }
            return result;
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

        #endregion HTML特殊字符转换

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

        private static Mutex m_mutex = new Mutex();

        /// 定时服务记录日志总方法入口
        /// <summary>
        /// 定时服务记录日志总方法入口
        /// </summary>
        /// <param name="pathName"></param>
        /// <param name="status"></param>
        /// <param name="msg"></param>
        public static void LogForService(string pathName, string status, string msg)
        {
            try
            {
                string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\" + pathName;
                m_mutex.WaitOne();
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string logfile = path + "\\" + System.DateTime.Today.ToString("yyyy-MM-dd") + ".log";
                System.IO.StreamWriter sw = File.AppendText(logfile);
                sw.WriteLine("{0:-20}\t{1}\t{2}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), status, msg);
                sw.Close();
                m_mutex.ReleaseMutex();
            }
            catch
            {
            }
        }

        /// cc网站记录日志总方法入口
        /// <summary>
        /// cc网站记录日志总方法入口
        /// 强斐
        /// 2016-9-1
        /// </summary>
        /// <param name="status"></param>
        /// <param name="msg"></param>
        /// <param name="func_name"></param>
        /// <param name="child"></param>
        public static void LogForWeb(string status, string msg, string func_name = null, string child = null)
        {
            try
            {
                string path = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LogWebAbsolutePath", false).TrimEnd('\\');
                if (path == "") return;
                if (string.IsNullOrEmpty(func_name))
                {
                    //默认：接口日志
                    func_name = "接口";
                }
                if (!string.IsNullOrEmpty(child))
                {
                    //按照参数手动分文件夹
                    path = path + "\\" + child;
                }
                else
                {
                    //默认：按照功能点分文件夹
                    path = path + "\\" + func_name;
                }
                m_mutex.WaitOne();
                //创建目录
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //追加文件
                string logfile = path + "\\" + func_name + "-" + System.DateTime.Today.ToString("yyyyMMdd") + ".log";
                System.IO.StreamWriter sw = File.AppendText(logfile);
                sw.WriteLine("{0:-20}\t{1}\t{2}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), status, msg);
                sw.Close();
                m_mutex.ReleaseMutex();
            }
            catch
            {
            }
        }

        /// 各功能点记录日志-分流日志-强斐-2016-8-29
        /// <summary>
        /// 各功能点记录日志-分流日志-强斐-2016-8-29
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="fengjihao"></param>
        public static void LogForWebForModule(string module, string func, string msg, int loginuserid = -1, string loginusername = "")
        {
            if (loginuserid < 0)
            {
                loginuserid = CommonFunction.ObjectToInteger(BLL.Util.GetLoginUserIDNotCheck(), -1);
            }
            if (string.IsNullOrEmpty(loginusername))
            {
                loginusername = BLL.Util.GetLoginRealName();
            }
            string ModuleLogOut = CommonFunction.ObjectToString(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ModuleLogOut", false));
            List<string> list = new List<string>(ModuleLogOut.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries));
            if (loginuserid > 0 && list.Contains(module))
            {
                BLL.Util.LogForWeb("[" + func + "]", msg, loginuserid.ToString() + "-" + loginusername, module + "\\" + System.DateTime.Today.ToString("yyyyMMdd"));
            }
        }

        public static string GetCurrentRequestFormStr(string r)
        {
            if (System.Web.HttpContext.Current.Request.Form[r] == null)
            {
                return string.Empty;
            }
            else
            {
                return System.Web.HttpUtility.UrlDecode(System.Web.HttpContext.Current.Request.Form[r].ToString().Trim().Replace("+", "%2B"));
            }
        }

        public static string GetCurrentRequestStr(string r)
        {
            if (System.Web.HttpContext.Current.Request[r] == null)
            {
                return string.Empty;
            }
            else
            {
                return System.Web.HttpUtility.UrlDecode(System.Web.HttpContext.Current.Request[r].ToString().Trim().Replace("+", "%2B"));
            }
        }

        public static int GetCurrentRequestInt(string r)
        {
            int val = -1;
            try
            {
                if (System.Web.HttpContext.Current.Request[r] != null)
                {
                    val = int.Parse(System.Web.HttpContext.Current.Request[r].ToString().Trim());
                }
            }
            catch (Exception e)
            {
                return val;
            }
            return val;
        }

        public static int GetCurrentRequestFormInt(string r)
        {
            int val = -1;
            try
            {
                if (System.Web.HttpContext.Current.Request.Form[r] != null)
                {
                    val = int.Parse(System.Web.HttpContext.Current.Request.Form[r].ToString().Trim());
                }
            }
            catch (Exception e)
            {
                return val;
            }
            return val;
        }

        public static string GetCurrentRequestQueryStr(string r)
        {
            if (System.Web.HttpContext.Current.Request.QueryString[r] == null)
            {
                return string.Empty;
            }
            else
            {
                return System.Web.HttpUtility.UrlDecode(System.Web.HttpContext.Current.Request.QueryString[r].ToString().Trim().Replace("+", "%2B"));
            }
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
                System.Web.HttpContext webHttp = System.Web.HttpContext.Current;
                if (webHttp != null && webHttp.Session != null && webHttp.Session["truename"] != null && webHttp.Session["truename"].ToString() != "")
                {
                    realName = webHttp.Session["truename"].ToString().Trim();
                }
            }
            catch (Exception)
            {
                realName = string.Empty;
            }
            return realName;
        }

        /// <summary>
        /// 根据Session名称，获取值
        /// </summary>
        /// <param name="sessionName">Session名称</param>
        /// <returns>返回Session的值</returns>
        public static string GetSessionValue(string sessionName)
        {
            string value = string.Empty;
            try
            {
                System.Web.HttpContext webHttp = System.Web.HttpContext.Current;
                if (webHttp != null && webHttp.Session != null && webHttp.Session[sessionName] != null && webHttp.Session[sessionName].ToString() != "")
                {
                    value = webHttp.Session[sessionName].ToString().Trim();
                }
                else
                {
                    string cookieValue = XYAuto.Utils.Security.DESEncryptor.Decrypt(webHttp.Request.Cookies["ncc-uinfo"].Value, key);
                    string[] strCookieArr = cookieValue.Split('|');
                    switch (sessionName)
                    {
                        case "Login_RoleName":
                            webHttp.Session[sessionName] = XYAuto.Utils.Security.DESEncryptor.Decrypt(webHttp.Server.UrlDecode(strCookieArr[1]), key);
                            break;

                        case "Login_BGID":
                            webHttp.Session[sessionName] = XYAuto.Utils.Security.DESEncryptor.Decrypt(strCookieArr[2], key);
                            break;

                        case "Login_BGName":
                            webHttp.Session[sessionName] = XYAuto.Utils.Security.DESEncryptor.Decrypt(webHttp.Server.UrlDecode(strCookieArr[3]), key);
                            break;

                        case "Login_OutCallNum":
                            webHttp.Session[sessionName] = XYAuto.Utils.Security.DESEncryptor.Decrypt(strCookieArr[4], key);
                            break;

                        default: break;
                    }
                }
            }
            catch (Exception)
            {
                value = string.Empty;
            }
            return value;
        }

        /// 获取登录人的userid
        /// <summary>
        /// 获取登录人的userid
        /// </summary>
        /// <returns></returns>
        public static string GetLoginUserIDNotCheck()
        {
            string userid = string.Empty;
            try
            {
                System.Web.HttpContext webHttp = System.Web.HttpContext.Current;
                if (webHttp != null && webHttp.Session != null && webHttp.Session["userid"] != null && webHttp.Session["userid"].ToString() != "")
                {
                    userid = webHttp.Session["userid"].ToString().Trim();
                }
            }
            catch (Exception)
            {
                userid = string.Empty;
            }
            return userid;
        }

        public static string GetLoginUserNumber()
        {
            return BLL.Util.GetSessionValue("EmployeeNumber");//从session取
        }

        /// <summary>
        /// 根据字符IDs，转换为'1','2','3'
        /// </summary>
        /// <param name="arrayStr">字符IDs</param>
        /// <returns></returns>
        public static string GetStringBySplitArray(string arrayStr)
        {
            string temp = string.Empty;
            if (!string.IsNullOrEmpty(arrayStr))
            {
                string[] array = arrayStr.Split(',');
                foreach (string item in array)
                {
                    temp += ("'" + item + "',");
                }
            }
            return temp.TrimEnd(',');
        }

        /// <summary>
        /// 根据字符串转化为字符串数组
        /// </summary>
        /// <param name="arrayStr">字符串</param>
        /// <returns></returns>
        public static string[] GetSplitStrArray(string arrayStr)
        {
            string[] r = new string[] { arrayStr };
            if (!string.IsNullOrEmpty(arrayStr))
            {
                ArrayList al = new ArrayList();
                al.Add(',');
                al.Add('，');
                al.Add('/');
                al.Add('$');
                al.Add('\\');
                al.Add('、');
                foreach (char c in al)
                {
                    if (arrayStr.Split(c).Length > 1)
                    {
                        r = arrayStr.Split(c);
                        break;
                    }
                }
            }
            return r;
        }

        /// <summary>
        /// 根据2个逗号分隔的字符串，判断是否相等
        /// </summary>
        /// <param name="ids1">逗号分隔的字符串1</param>
        /// <param name="ids2">逗号分隔的字符串2</param>
        /// <returns></returns>
        public static bool IsEqualsByStringArray(string ids1, string ids2)
        {
            bool flag = false;
            if (ids1 == ids2)
            {
                flag = true;
            }
            else
            {
                string[] str1 = ids1.Split(',');
                string[] str2 = ids2.Split(',');
                if (str1.Length == str2.Length)
                {
                    foreach (string str in str1)
                    {
                        bool f = false;
                        foreach (string strOther in str2)
                        {
                            if (str == strOther)
                            {
                                f = true;
                                break;
                            }
                        }
                        if (!f)
                        {
                            return false;
                        }
                    }
                    flag = true;
                }
            }
            return flag;
        }

        /// <summary>
        /// List转换为DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(List<T> entitys)
        {
            //检查实体集合不能为空
            if (entitys == null || entitys.Count < 1)
            {
                //throw new Exception("需转换的集合为空");
                return null;
            }
            //取出第一个实体的所有Propertie
            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();
            //生成DataTable的structure
            //生产代码中，应将生成的DataTable结构Cache起来，此处略
            DataTable dt = new DataTable();
            for (int i = 0; i < entityProperties.Length; i++)
            {
                //dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);
                dt.Columns.Add(entityProperties[i].Name);
            }
            //将所有entity添加到DataTable中
            foreach (object entity in entitys)
            {
                //检查所有的的实体都为同一类型
                if (entity.GetType() != entityType)
                {
                    throw new Exception("要转换的集合元素类型不一致");
                }
                object[] entityValues = new object[entityProperties.Length];
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);
                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }

        public static List<T> DataTableToList<T>(DataTable table) //where T : EntityBase, new()
        {
            List<T> list = new List<T>();
            if (table != null && table.Rows != null && table.Rows.Count > 0)
            {
                for (var i = 0; i < table.Rows.Count; i++)
                {
                    //创建泛型对象
                    T entity = Activator.CreateInstance<T>();
                    //属性和名称相同时则赋值
                    for (var j = 0; j < table.Columns.Count; j++)
                    {
                        var property = entity.GetType().GetProperty(table.Columns[j].ColumnName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
                        if (property != null && table.Rows[i][j] != DBNull.Value)
                        {
                            property.SetValue(entity, table.Rows[i][j], null);
                        }
                    }
                    list.Add(entity);
                }
            }
            return list;
        }

        public static T DataTableToEntity<T>(DataTable table) //where T : EntityBase, new()
        {
            var entity = Activator.CreateInstance<T>();
            if (table.Rows.Count == 0)
                return default(T);
            for (var i = 0; i < table.Columns.Count; i++)
            {
                //var property = entity.GetType().GetProperty(table.Columns[i].ColumnName);
                var property = entity.GetType().GetProperty(table.Columns[i].ColumnName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);

                if (property != null && table.Rows[0][i] != DBNull.Value)
                {
                    property.SetValue(entity, table.Rows[0][i], null);
                }
            }
            return entity;
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
        /// 验证电话号码（包括手机和座机）
        /// </summary>
        /// <param name="str_telephone"></param>
        /// <returns></returns>
        public static bool IsTelephone(string str_telephone)
        {
            return IsTelNumber(str_telephone) || IsHandset(str_telephone);
        }

        /// <summary>
        /// 验证电话号码（包括手机和座机、400电话）
        /// </summary>
        /// <param name="str_telephone"></param>
        /// <returns></returns>
        public static bool IsTelephoneAnd400Tel(string str_telephone)
        {
            return IsTelNumber(str_telephone) || IsHandset(str_telephone) || Is400Tel(str_telephone);
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
            return System.Text.RegularExpressions.Regex.IsMatch(str_handset, @"^[1]+[3,4,5,8,7,6]+\d{9}$");
        }

        /// <summary>
        /// 验证400电话
        /// </summary>
        /// <param name="str_handset"></param>
        /// <returns></returns>
        public static bool Is400Tel(string str_handset)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_handset, @"^400\d{7}$");
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
        /// 获取ip地址
        /// </summary>
        /// <returns></returns>
        private static string GetIP()
        {
            string ip = "";
            try
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["remote_addr"];
            }
            catch { }
            return ip;
        }

        /// <summary>
        /// 获取当前发出请求的客户端[IP]地址
        /// </summary>
        /// <returns></returns>
        public static string GetIPAddress()
        {
            string s_UserIp = "";
            try
            {
                HttpRequest f_Request = HttpContext.Current.Request;

                // 如果使用代理，获取真实[IP]
                if (f_Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != String.Empty)
                {
                    s_UserIp = f_Request.ServerVariables["REMOTE_ADDR"];
                }
                else
                {
                    s_UserIp = f_Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                }
                if (string.IsNullOrEmpty(s_UserIp))
                {
                    s_UserIp = f_Request.UserHostAddress;
                }
            }
            catch (Exception)
            {
                s_UserIp = string.Empty;
            }
            return s_UserIp;
        }

        #region 智能去除最后一个逗号

        /// <summary>
        /// 智能去除最后一个逗号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string removeLastComma(string str)
        {
            str = str.Trim();
            int strLength = str.Length;
            if (str == null || str == "")
            {
                return "";
            }
            else
            {
                if (str.Substring(strLength - 1, 1) == ",")
                {
                    return str.Substring(0, strLength - 1);
                }
                else
                {
                    return str;
                }
            }
        }

        #endregion 智能去除最后一个逗号

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

        public static string TryDecryptString(string inputStr)
        {
            string str = "";
            try
            {
                str = DecryptString(inputStr, key);
            }
            catch
            {
            }
            return str;
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

        #endregion 加密，解密

        /// <summary>
        /// 根据DataTable分页方法
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="PageSize">页大小</param>
        /// <param name="PageIndex">页索引</param>
        /// <returns>返回DataTable</returns>
        public static DataTable GetPagedTable(DataTable dt, int PageSize, int PageIndex)
        {
            if (PageIndex == 0)
                return dt;
            DataTable newdt = dt.Copy();
            newdt.Clear();

            int rowbegin = (PageIndex - 1) * PageSize;
            int rowend = PageIndex * PageSize;

            if (rowbegin >= dt.Rows.Count)
                return newdt;

            if (rowend > dt.Rows.Count)
                rowend = dt.Rows.Count;
            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow newdr = newdt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                newdt.Rows.Add(newdr);
            }

            return newdt;
        }

        /// <summary>
        /// 判断数字
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static bool IsNumber(string strNumber)
        {
            Regex regex = new Regex("[^0-9]");
            return !regex.IsMatch(strNumber);
        }

        #region DataTabel转换成JSON

        /// <summary>
        /// DataTabel转换成JSON
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        public static string DateTableToJson2(DataTable tb)
        {
            if (tb == null || tb.Rows.Count == 0)
            {
                return "";
            }

            StringBuilder sbJson = new StringBuilder();

            sbJson.Append("[");
            Hashtable htColumns = new Hashtable();
            for (int i = 0; i < tb.Columns.Count; i++)
            {
                htColumns.Add(i, tb.Columns[i].ColumnName.Trim());
            }

            for (int j = 0; j < tb.Rows.Count; j++)
            {
                if (j != 0)
                {
                    sbJson.Append(",");
                }
                sbJson.Append("{");
                for (int c = 0; c < tb.Columns.Count; c++)
                {
                    sbJson.Append(htColumns[c].ToString() + ":'" + tb.Rows[j][c].ToString() + "',");
                }
                sbJson.Append("index:" + j.ToString()); //序号
                sbJson.Append("}");
            }
            sbJson.Append("]");
            return sbJson.ToString();
        }

        /// <summary>
        /// DataTabel转换成JSON
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        public static string DateTableToJson2ForOtherDeal(DataTable tb)
        {
            if (tb == null || tb.Rows.Count == 0)
            {
                return "";
            }

            StringBuilder sbJson = new StringBuilder();

            sbJson.Append("[");
            Hashtable htColumns = new Hashtable();
            for (int i = 0; i < tb.Columns.Count; i++)
            {
                htColumns.Add(i, tb.Columns[i].ColumnName.Trim());
            }

            for (int j = 0; j < tb.Rows.Count; j++)
            {
                if (j != 0)
                {
                    sbJson.Append(",");
                }
                sbJson.Append("{");
                for (int c = 0; c < tb.Columns.Count; c++)
                {
                    sbJson.Append(htColumns[c].ToString() + ":'" + EscapeString(tb.Rows[j][c].ToString()) + "',");
                }
                sbJson.Append("index:" + j.ToString()); //序号
                sbJson.Append("}");
            }
            sbJson.Append("]");
            return sbJson.ToString();
        }

        #endregion DataTabel转换成JSON

        /// <summary>
        /// 根据表名获取该表的创建或操作人ID add lxw 13.3.23
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="field">输出的字段名</param>
        /// <param name="status">状态，如果不为空，则为指定状态的创建人</param>
        /// <returns></returns>
        public static DataTable GetAllUserIDByTable(string tableName, string field, string status)
        {
            string where = string.Empty;

            //if (status != string.Empty)
            //{
            //    where = " and Status in (" + status + ")";
            //}

            where = "select distinct " + field + " from " + tableName + " where " + field + "<>-2 ";

            DataSet ds = XYAuto.Utils.Data.SqlHelper.ExecuteDataset(ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC"), CommandType.Text, where, null);
            return ds.Tables[0];
        }

        /// <summary>
        /// 根据表名获取该表的创建或操作人ID add lxw 13.3.23
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="field">输出的字段名</param>
        /// <param name="status">状态，如果不为空，则为指定状态的创建人</param>
        /// <returns></returns>
        public static DataTable GetAllUserIDByTable(string tableName, string field, string strWhere, string status)
        {
            string where = string.Empty;
            where = "select distinct " + field + " from " + tableName + " where " + strWhere;
            DataSet ds = XYAuto.Utils.Data.SqlHelper.ExecuteDataset(ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC"), CommandType.Text, where, null);
            return ds.Tables[0];
        }

        /// <summary>
        /// 根据表名获取该表的信息
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="field">输出的字段名</param>
        /// <param name="status">状态，如果不为空，则为指定状态的创建人</param>
        /// <returns></returns>
        public static DataTable GetTableInfoByTableName(string tableName, string status)
        {
            string where = string.Empty;

            if (status != string.Empty)
            {
                where = " where Status in (" + status + ")";
            }

            where = "select * from " + tableName + where;

            DataSet ds;

            ds = XYAuto.Utils.Data.SqlHelper.ExecuteDataset(ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC"), CommandType.Text, where, null);
            return ds.Tables[0];
        }

        /// <summary>
        /// 根据sql获取该表的信息
        /// </summary>
        /// <param name="sql">执行的sql语句</param>
        /// <returns></returns>
        public static DataTable GetTableInfoBySql(string sql)
        {
            string where = string.Empty;

            if (sql == string.Empty)
            {
                return null;
            }

            DataSet ds;

            ds = XYAuto.Utils.Data.SqlHelper.ExecuteDataset(ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC"), CommandType.Text, sql, null);
            return ds.Tables[0];
        }

        /// <summary>
        /// 批量插入DB数据方法
        /// </summary>
        /// <param name="dt">Datatable</param>
        /// <param name="conn">链接字符串</param>
        /// <param name="tableName">目标表名</param>
        /// <param name="batchSize">批次大小</param>
        public static void BulkCopyToDB(DataTable dt, string conn, string tableName, int batchSize, out string msg)
        {
            msg = "";

            if (dt.Rows.Count > 0)
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.KeepIdentity,
                                   transaction))
                        {
                            bulkCopy.BulkCopyTimeout = 1800;

                            bulkCopy.BatchSize = batchSize;
                            bulkCopy.DestinationTableName = tableName;

                            try
                            {
                                bulkCopy.WriteToServer(dt);
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                msg = ex.Message.ToString();
                                //BLL.Loger.Log4Net.Error(ex.Message, ex);
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

        public static void BulkCopyToDB(DataTable dt, string conn, string tableName, out string msg)
        {
            msg = "";

            if (dt.Rows.Count > 0)
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        bulkCopy.DestinationTableName = tableName;
                        try
                        {
                            bulkCopy.WriteToServer(dt);
                        }
                        catch (Exception ex)
                        {
                            msg = ex.Message.ToString();
                            //BLL.Loger.Log4Net.Error(ex.Message, ex);
                        }
                        finally
                        {
                            bulkCopy.Close();
                        }
                    }
                }
            }
        }

        public static void BulkCopyToDB(DataTable dt, string conn, string tableName, SqlTransaction tran, out string msg)
        {
            msg = "";

            if (dt.Rows.Count > 0)
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(tran.Connection, SqlBulkCopyOptions.Default, tran))
                {
                    bulkCopy.DestinationTableName = tableName;
                    try
                    {
                        bulkCopy.WriteToServer(dt);
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message.ToString();
                        //BLL.Loger.Log4Net.Error(ex.Message, ex);
                    }
                    finally
                    {
                        bulkCopy.Close();
                    }
                }
            }
        }

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
                                //BLL.Loger.Log4Net.Error(ex.Message, ex);
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

        public static void BulkCopyToDB(DataTable dt, string conn, string tableName, int batchSize, IList<SqlBulkCopyColumnMapping> list, out string msg)
        {
            msg = "";
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
                                msg = ex.Message.ToString();
                                //BLL.Loger.Log4Net.Error(ex.Message, ex);
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
        /// 实现导出部分：导出内容的拼接
        /// </summary>
        /// <param name="dt">数据表</param>
        /// 增加一个参数，标识 是否对数字列 增加tab制表符，默认true-需要，false-不需要 add lxw 14.5.29
        public static StringWriter TableToCSV(DataTable dt, bool isTab)
        {
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

        public static void ExportToCSV(string filename, DataTable dt, bool isTab = true)
        {
            filename = filename.Replace(" ", "");
            var exportString = BLL.Util.TableToCSV(dt, isTab);
            HttpContext.Current.Response.HeaderEncoding = Encoding.Default;
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename + ".csv");
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
            HttpContext.Current.Response.Write(exportString);
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 获取ExcelSheetName
        /// </summary>
        /// <param name="filePath">文件物理全路径</param>
        /// <returns></returns>
        public static string GetExcelSheetName(string filePath)
        {
            string sheetName = "";

            System.IO.FileStream tmpStream = File.OpenRead(filePath);
            byte[] fileByte = new byte[tmpStream.Length];
            tmpStream.Read(fileByte, 0, fileByte.Length);
            tmpStream.Close();

            byte[] tmpByte = new byte[]{Convert.ToByte(11),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0),
        Convert.ToByte(11),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0),
        Convert.ToByte(30),Convert.ToByte(16),Convert.ToByte(0),Convert.ToByte(0)};

            int index = GetSheetIndex(fileByte, tmpByte);
            if (index > -1)
            {
                index += 16 + 12;
                System.Collections.ArrayList sheetNameList = new System.Collections.ArrayList();

                for (int i = index; i < fileByte.Length - 1; i++)
                {
                    byte temp = fileByte[i];
                    if (temp != Convert.ToByte(0))
                        sheetNameList.Add(temp);
                    else
                        break;
                }
                byte[] sheetNameByte = new byte[sheetNameList.Count];
                for (int i = 0; i < sheetNameList.Count; i++)
                    sheetNameByte[i] = Convert.ToByte(sheetNameList[i]);

                sheetName = System.Text.Encoding.Default.GetString(sheetNameByte);
            }
            return sheetName;
        }

        /// <summary>
        /// 只供方法GetSheetName()使用
        /// </summary>
        /// <returns></returns>
        private static int GetSheetIndex(byte[] FindTarget, byte[] FindItem)
        {
            int index = -1;

            int FindItemLength = FindItem.Length;
            if (FindItemLength < 1) return -1;
            int FindTargetLength = FindTarget.Length;
            if ((FindTargetLength - 1) < FindItemLength) return -1;

            for (int i = FindTargetLength - FindItemLength - 1; i > -1; i--)
            {
                System.Collections.ArrayList tmpList = new System.Collections.ArrayList();
                int find = 0;
                for (int j = 0; j < FindItemLength; j++)
                {
                    if (FindTarget[i + j] == FindItem[j]) find += 1;
                }
                if (find == FindItemLength)
                {
                    index = i;
                    break;
                }
            }
            return index;
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
                }
                catch
                {
                    //BLL.Loger.Log4Net.Info("转换model实体类（" + typeof(T).Name + "）属性（" + property.Name + "）传值（" + (value1 == null ? "null" : value1.ToLower()) + "），忽略");
                }
            }
            return model;
        }

        /// 将Dataset转成字节流
        /// <summary>
        /// 将Dataset转成字节流
        /// </summary>
        /// <param name="dsOriginal"></param>
        /// <returns></returns>
        public static byte[] GetBinaryFormatData(DataSet dsOriginal)
        {
            byte[] binaryDataResult = null;
            MemoryStream memStream = new MemoryStream();
            IFormatter brFormatter = new BinaryFormatter();
            dsOriginal.RemotingFormat = SerializationFormat.Binary;
            brFormatter.Serialize(memStream, dsOriginal);
            binaryDataResult = memStream.ToArray();
            memStream.Close();
            memStream.Dispose();
            return binaryDataResult;
        }

        /// 将二进制流转化成DataSet
        /// <summary>
        /// 将二进制流转化成DataSet
        /// </summary>
        /// <param name="binaryData"></param>
        /// <returns></returns>
        public static DataSet RetrieveDataSet(byte[] binaryData)
        {
            DataSet dataSetResult = null;
            MemoryStream memStream = new MemoryStream(binaryData);
            IFormatter brFormatter = new BinaryFormatter();
            object obj = brFormatter.Deserialize(memStream);
            dataSetResult = (DataSet)obj;
            return dataSetResult;
        }

        public static byte[] DataTableToBinary(DataTable dt)
        {
            DataSet ds = new DataSet();
            if (dt.DataSet != null)
                ds = dt.DataSet;
            else
                ds.Tables.Add(dt);
            return GetBinaryFormatData(ds);
        }

        public static DataTable BinaryToDataTable(byte[] binary)
        {
            DataSet ds = RetrieveDataSet(binary);
            return ds.Tables[0];
        }

        public static DateTime GetServerDateTime()
        {
            return DateTime.Now;
        }

        public static DateTime GetServerUtcDateTime()
        {
            return DateTime.UtcNow;
        }

        /// <summary>
        /// 将可序列化对象转成Byte数组
        /// </summary>
        /// <param name="obj">对象(对象不能为空)</param>
        /// <returns>返回相关数组</returns>
        protected static byte[] ObjectToByteArray<T>(T obj) where T : ISerializable
        {
            if (obj == null)
            {
                byte[] byteArr = new byte[] { };
                return byteArr;
            }
            else
            {
                MemoryStream ms = new MemoryStream();
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, obj);
                ms.Close();
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 将可序列化对象转成的byte数组还原为对象
        /// </summary>
        /// <param name="byteArry">byte数组</param>
        /// <returns>相关对象</returns>
        protected static T ByteArrayToObject<T>(byte[] byteArry) where T : ISerializable
        {
            if (byteArry != null && byteArry.Length > 0)
            {
                MemoryStream ms = new MemoryStream(byteArry);
                BinaryFormatter bf = new BinaryFormatter();
                return (T)bf.Deserialize(ms);
            }
            else
            {
                return default(T);
            }
        }

        public static string GetAppSettingValue(string key, string defaultValue = "")
        {
            try
            {
                return ConfigurationUtil.GetAppSettingValue(key);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// 号码处理（去掉前缀）
        /// <summary>
        /// 号码处理（去掉前缀）
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static string HaoMaProcess(string phone, string areacode)
        {
            phone = phone.Trim();
            //第一个0的位置
            int pos = phone.IndexOf('0');
            if (phone.Length >= 11 && phone[phone.Length - 11] == '1' && phone[phone.Length - 11 + 1] != '0')
            {
                //手机号
                phone = phone.Substring(phone.Length - 11, 11);
            }
            else if (pos >= 0 && phone.Length - pos >= 10)
            {
                //区号座机
                phone = phone.Substring(pos);
            }
            else if (phone.Length > 8)
            {
                //非区号座机
                phone = areacode + phone.Substring(phone.Length - 8, 8);
            }
            else
            {
                //无处理
            }
            //处理完成后，检查是否00开头
            if (phone.StartsWith("00"))
            {
                phone = phone.Substring(1);
            }
            return phone;
        }

        public static string HaoMaProcess(string phone)
        {
            return HaoMaProcess(phone, "029");
        }

        #region 2个DataTable根据字段进行Join关联

        public static DataTable JoinDataTable(DataTable First, DataTable Second, string FJC, string SJC, bool left, bool right)
        {
            return Join(First, Second, new DataColumn[] { First.Columns[FJC] }, new DataColumn[] { First.Columns[SJC] }, left, right);
        }

        private static DataTable Join(DataTable left, DataTable right,
            DataColumn[] leftCols, DataColumn[] rightCols,
            bool includeLeftJoin, bool includeRightJoin)
        {
            DataTable result = new DataTable("JoinResult");
            using (DataSet ds = new DataSet())
            {
                ds.Tables.AddRange(new DataTable[] { left.Copy(), right.Copy() });
                DataColumn[] leftRelationCols = new DataColumn[leftCols.Length];
                for (int i = 0; i < leftCols.Length; i++)
                    leftRelationCols[i] = ds.Tables[0].Columns[leftCols[i].ColumnName];

                DataColumn[] rightRelationCols = new DataColumn[rightCols.Length];
                for (int i = 0; i < rightCols.Length; i++)
                    rightRelationCols[i] = ds.Tables[1].Columns[rightCols[i].ColumnName];

                //create result columns
                for (int i = 0; i < left.Columns.Count; i++)
                    result.Columns.Add(left.Columns[i].ColumnName, left.Columns[i].DataType);
                for (int i = 0; i < right.Columns.Count; i++)
                {
                    string colName = right.Columns[i].ColumnName;
                    while (result.Columns.Contains(colName))
                        colName += "_2";
                    result.Columns.Add(colName, right.Columns[i].DataType);
                }

                //add left join relations
                DataRelation drLeftJoin = new DataRelation("rLeft", leftRelationCols, rightRelationCols, false);
                ds.Relations.Add(drLeftJoin);

                //join
                result.BeginLoadData();
                foreach (DataRow parentRow in ds.Tables[0].Rows)
                {
                    DataRow[] childrenRowList = parentRow.GetChildRows(drLeftJoin);
                    if (childrenRowList != null && childrenRowList.Length > 0)
                    {
                        object[] parentArray = parentRow.ItemArray;
                        foreach (DataRow childRow in childrenRowList)
                        {
                            object[] childArray = childRow.ItemArray;
                            object[] joinArray = new object[parentArray.Length + childArray.Length];
                            System.Array.Copy(parentArray, 0, joinArray, 0, parentArray.Length);
                            System.Array.Copy(childArray, 0, joinArray, parentArray.Length, childArray.Length);
                            result.LoadDataRow(joinArray, true);
                        }
                    }
                    else //left join
                    {
                        if (includeLeftJoin)
                        {
                            object[] parentArray = parentRow.ItemArray;
                            object[] joinArray = new object[parentArray.Length];
                            System.Array.Copy(parentArray, 0, joinArray, 0, parentArray.Length);
                            result.LoadDataRow(joinArray, true);
                        }
                    }
                }

                if (includeRightJoin)
                {
                    //add right join relations
                    DataRelation drRightJoin = new DataRelation("rRight", rightRelationCols, leftRelationCols, false);
                    ds.Relations.Add(drRightJoin);

                    foreach (DataRow parentRow in ds.Tables[1].Rows)
                    {
                        DataRow[] childrenRowList = parentRow.GetChildRows(drRightJoin);
                        if (childrenRowList == null || childrenRowList.Length == 0)
                        {
                            object[] parentArray = parentRow.ItemArray;
                            object[] joinArray = new object[result.Columns.Count];
                            System.Array.Copy(parentArray, 0, joinArray,
                                joinArray.Length - parentArray.Length, parentArray.Length);
                            result.LoadDataRow(joinArray, true);
                        }
                    }
                }
                result.EndLoadData();
            }
            return result;
        }

        #endregion 2个DataTable根据字段进行Join关联

        /// 计算month_ago月以前时的表名后缀
        /// <summary>
        /// 计算month_ago月以前时的表名后缀
        /// </summary>
        /// <param name="month_ago"></param>
        /// <param name="search_time"></param>
        /// <returns></returns>
        public static string CalcTableNameByMonth(int month_ago, DateTime search_time)
        {
            //当月1号往前推month_ago个月的1号
            DateTime spit = DateTime.Today.AddDays(-DateTime.Today.Day + 1).AddMonths(-month_ago + 1);
            if (search_time >= spit)
            {
                return "";
            }
            else if (search_time < new DateTime(2015, 1, 1))
            {
                //查询时间小于2015-1-1时，查询当前表
                return "";
            }
            else
            {
                return "_" + search_time.ToString("yyyyMM");
            }
        }

        /// 设置日期时间的显示格式
        /// <summary>
        /// 设置日期时间的显示格式
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ShowDateTimeFormat(string value)
        {
            if (value == "") return "";
            DateTime a = new DateTime();
            if (DateTime.TryParse(value, out a))
            {
                if (a == new DateTime() || a == new DateTime(1900, 1, 1))
                {
                    return "";
                }
                else
                {
                    return a.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            else return "";
        }

        public static string GetNotAccessMsgPage(string msg)
        {
            string gourl = ConfigurationUtil.GetAppSettingValue("NotAccessMsgPagePath").Replace("~", "");
            if (string.IsNullOrEmpty(gourl))
            {
                gourl = "/ErrorPage/NotAccessMsgPage.aspx";
            }
            return "<script type='text/javascript'>window.location.href='" + gourl + "?msg=" + HttpUtility.UrlEncode(msg) + "';</script>";
            ;
        }

        //取回访报表业务组
        public static string GetReturnVisitBG()
        {
            string rvBgInfo = ConfigurationUtil.GetAppSettingValue("rvBgInfo");
            return rvBgInfo;
        }

        public static string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        /// 读取文件
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static byte[] FileToBinary(string path)
        {
            byte[] buffer = null;
            FileStream stream = null;
            try
            {
                if (File.Exists(path))
                {
                    stream = new FileInfo(path).OpenRead();
                    buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                }
                return buffer;
            }
            catch (Exception ex)
            {
                //BLL.Loger.Log4Net.Error("读取文件 异常：", ex);
                return buffer;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// 写入文件
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="list"></param>
        public static void BinaryToFile(string path, byte[] buffer)
        {
            FileStream stream = null;
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                string pathroot = Path.GetDirectoryName(path);
                if (!Directory.Exists(pathroot))
                {
                    Directory.CreateDirectory(pathroot);
                }

                stream = new FileInfo(path).Create();
                stream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                //BLL.Loger.Log4Net.Error("写入文件 异常：", ex);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// 根据传递的被除数，除数字符串，算百分比，保留两位小数
        /// <summary>
        /// 根据传递的被除数，除数字符串，算百分比，保留两位小数
        /// </summary>
        /// <param name="aDividend">被除数</param>
        /// <param name="bDivisor">除数</param>
        /// <returns>百分比，保留两位小数</returns>
        public static string ProduceLv(string aDividend, string bDivisor)
        {
            string returnstr = string.Empty;
            decimal aTransit = 0;
            decimal bTransit = 0;
            if (!string.IsNullOrEmpty(aDividend))
            {
                aTransit = Convert.ToDecimal(aDividend);
            }
            else
            {
                return "0.00%";
            }
            if (!string.IsNullOrEmpty(bDivisor) && bDivisor != "0")
            {
                bTransit = Convert.ToDecimal(bDivisor);
            }
            else
            {
                return "-";
            }
            decimal cEnd = decimal.Round((decimal)aTransit / bTransit, 4);
            return (cEnd * 100).ToString("N2") + "%";
        }

        /// 转换实体类
        /// <summary>
        /// 转换实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static T ConvertJsonToEntity<T>(string msg) where T : new()
        {
            T jsondata = default(T);
            try
            {
                jsondata = (T)Newtonsoft.Json.JsonConvert.DeserializeObject(msg, typeof(T));
            }
            catch
            {
            }
            if (jsondata == null)
            {
                jsondata = new T();
            }
            return jsondata;
        }

        /// MD5方式加密字符串的方法
        /// <summary>
        /// MD5方式加密字符串的方法
        /// </summary>
        /// <param name="source">要进行加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5Encrypt(string source)
        {
            try
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] result = md5.ComputeHash(Encoding.UTF8.GetBytes(source));
                string returnResult = "";
                for (int i = 0; i < result.Length; i++)
                {
                    returnResult += result[i].ToString("x");
                }
                return returnResult;
            }
            catch (Exception ex)
            {
                throw new Exception("MD5方式加密字符串失败。错误信息：" + ex.Message);
            }
        }

        /// 集合减法
        /// <summary>
        /// 集合减法
        /// </summary>
        /// <param name="total_list">总集合</param>
        /// <param name="other_list">减去的集合</param>
        /// <param name="total_count">总数</param>
        /// <param name="remain_count">剩余数</param>
        /// <param name="delete_count">删除数</param>
        /// <returns>删除的集合</returns>
        public static List<string> SubtractionList(List<string> total_list, List<string> other_list, out int remain_count, out int delete_count)
        {
            List<string> del_list = new List<string>();
            foreach (string key in other_list)
            {
                if (total_list.Contains(key))
                {
                    total_list.Remove(key);
                    del_list.Add(key);
                }
            }
            //去重后的数据总数
            remain_count = total_list.Count;
            //重复数据
            delete_count = del_list.Count;
            //返回删除的list
            return del_list;
        }

        #region 上传下载文件路径管理

        /// 临时目录
        /// <summary>
        /// 临时目录
        /// </summary>
        /// <param name="split"></param>
        /// <returns></returns>
        public static string GetUploadTemp(string split = "\\")
        {
            string root = GetUploadWebRoot();
            return root + "Temp" + split + Guid.NewGuid().ToString() + split;
        }

        /// 项目目录
        /// <summary>
        /// 项目目录
        /// </summary>
        /// <param name="projectType"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static string GetUploadProject(ProjectTypePath projectType, string split = "\\")
        {
            return projectType + split;
        }

        /// 根目录
        /// <summary>
        /// 根目录
        /// </summary>
        /// <returns></returns>
        public static string GetUploadWebRoot()
        {
            string UploadWebAbsolutePath = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("UploadWebAbsolutePath", false);
            return UploadWebAbsolutePath.TrimEnd('\\') + '\\';
        }

        /// 功能点枚举
        /// <summary>
        /// 功能点枚举
        /// </summary>
        public enum ProjectTypePath
        {
            [EnumTextValue("其他")]
            Other = 0,

            [EnumTextValue("黑白名单")]
            BlackWhite = 1,

            [EnumTextValue("数据模版")]
            Template = 2,

            [EnumTextValue("知识库")]
            KnowledgeLib = 3,

            [EnumTextValue("项目管理")]
            Project = 4,

            [EnumTextValue("易派会员")]
            Member = 5,

            [EnumTextValue("工单")]
            WorkOrder = 6
        }

        #endregion 上传下载文件路径管理

        /// <summary>
        /// 阿拉伯数字转换成汉字，最大999999999，最小-999999999
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string ArabicNumbersConvertToChina(int d)
        {
            if (System.Math.Abs(d).ToString().Length > 9)
            {
                return "转换出错，超出转换范围";
            }
            if (d < 0)
            {
                return "负" + ArabicNumbersConvertToChina(-d);
            }

            string returnStr = string.Empty;
            //        String[] str = { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };
            String[] str = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
            //        String ss[] = new String[] { "元", "拾", "佰", "仟", "万", "拾", "佰", "仟", "亿" };
            String[] ss = new String[] { "", "十", "百", "千", "万", "十", "百", "千", "亿" };
            String s = d.ToString();
            char[] sChar = s.ToCharArray();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < sChar.Length; i++)
            {
                String index = sChar[i].ToString();
                sb.Append(str[int.Parse(index)]);
            }
            char[] sss = sb.ToString().ToCharArray();
            int n = 0;
            for (int j = sss.Length; j > 0; j--)
            {
                sb = sb.Insert(j, ss[n++]);
            }
            returnStr = sb.ToString();
            if (returnStr.StartsWith("一十") && returnStr.Length >= 2)
            {
                returnStr = returnStr.Substring(1);
            }
            if (returnStr.IndexOf("零") > 0)
            {
                returnStr = returnStr.Replace("零十", "零");
                returnStr = returnStr.Replace("零百", "零");
                returnStr = returnStr.Replace("零千", "零");
                returnStr = returnStr.Replace("零万", "零");
                returnStr = returnStr.Replace("零十万", "零");
                returnStr = returnStr.Replace("零百万", "零");
                returnStr = returnStr.Replace("零千万", "零");
            }
            //中间多个零合并成1个零
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex(@"[零]{2,}", options);
            returnStr = regex.Replace(returnStr, @"零");
            //末尾是零全部去掉
            while (returnStr.EndsWith("零") && returnStr.Length > 1)
            {
                returnStr = returnStr.Substring(0, returnStr.Length - 1);
            }
            return returnStr;
        }

        /// 提示并关闭当前页面
        /// <summary>
        /// 提示并关闭当前页面
        /// </summary>
        /// <param name="response"></param>
        /// <param name="info"></param>
        public static void CloseCurrentPageAfterAlert(HttpResponse response, string info)
        {
            response.Write(@"
                <script language='javascript'>
                javascript:" + @"
                try {
                    alert('" + info + @"');
                    window.external.MethodScript('/browsercontrol/closepage');
                } catch (e) {
                    window.opener = null; window.open('', '_self'); window.close();
                };
                </script>
          ");
        }

        /// <summary>
        /// 获取上传路径中的文件名称（包含扩展名）
        /// </summary>
        /// <param name="path">文件路径，如：/UpLoad/2017/3/9/15/QQ截图20170301204617$06881c95-bdc4-48fa-90ab-2f220083dea1.png</param>
        /// <returns>返回文件名称，如：QQ截图20170301204617.png</returns>
        public static string GetFileNameByUpload(string path)
        {
            try
            {
                string fileName = Path.GetFileName(path);
                string fielExt = Path.GetExtension(path);
                string[] array = fileName.Split('$');
                if (array.Length > 0)
                {
                    return array[0] + fielExt;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取文件扩展名及文件名称
        /// </summary>
        /// <param name="path">文件路径，如：/UpLoad/2017/3/9/15/QQ截图20170301204617$06881c95-bdc4-48fa-90ab-2f220083dea1.png</param>
        /// <returns>item1:文件名  item2:文件扩展名称</returns>
        public static Tuple<string, string> GetFileNameAndExtension(string path)
        {
            var tp = new Tuple<string, string>(string.Empty, string.Empty);
            try
            {
                var fileName = Path.GetFileName(path);
                var fielExt = Path.GetExtension(path);
                if (fileName != null)
                {
                    string[] array = fileName.Split('$');
                    if (array.Length > 0)
                    {
                        return new Tuple<string, string>(array[0], fielExt);
                    }
                }
            }
            catch (Exception)
            {
                return tp;
            }
            return tp;
        }

        /// <summary>
        /// 相对路径转换成服务器本地物理路径
        /// </summary>
        /// <param name="imagesurl1"></param>
        /// <returns></returns>
        public static string Urlconvertorlocal(string imagesurl1)
        {
            if (System.Web.HttpContext.Current == null)
                return string.Empty;
            if (System.Web.HttpContext.Current.Request.ApplicationPath != null)
            {
                try
                {
                    var solutePath = new Uri(imagesurl1).AbsolutePath;
                    string tmpRootDir = System.Web.HttpContext.Current.Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath);//获取程序根目录
                    return tmpRootDir + solutePath.Replace(@"/", @"\"); //转换成绝对路径
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 判断一个字符串是否为url
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsUrl(string str)
        {
            try
            {
                string Url = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
                return Regex.IsMatch(str, Url);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取文件的大小
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public static long GetDirectoryLength(string dirPath)
        {
            //判断给定的路径是否存在,如果不存在则退出
            if (!Directory.Exists(dirPath))
                return 0;
            long len = 0;
            //定义一个DirectoryInfo对象
            DirectoryInfo di = new DirectoryInfo(dirPath);
            //通过GetFiles方法,获取di目录中的所有文件的大小
            foreach (FileInfo fi in di.GetFiles())
            {
                len += fi.Length;
            }
            //获取di中所有的文件夹,并存到一个新的对象数组中,以进行递归
            DirectoryInfo[] dis = di.GetDirectories();
            if (dis.Length > 0)
            {
                for (int i = 0; i < dis.Length; i++)
                {
                    len += GetDirectoryLength(dis[i].FullName);
                }
            }
            return len;
        }

        /// <summary>
        /// 获取文件的大小
        /// </summary>
        /// <param name="sFileFullName"></param>
        /// <returns>KB</returns>
        public static long GetFileSize(string sFileFullName)
        {
            if (string.IsNullOrWhiteSpace(sFileFullName)) return 0;

            if (!File.Exists(sFileFullName))
                return 0;
            FileInfo fiInput = new FileInfo(sFileFullName);
            long len = fiInput.Length;

            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                len = len / 1024;
            }

            return len;
        }

        /// <summary>
        /// 拼接数据权限
        /// </summary>
        /// <param name="resourceType">资源类型</param>
        /// <param name="tablenameUserID">UserID所属的表名称，或表别名</param>
        /// <param name="UserIDFileName">UserID字段名称</param>
        /// <param name="UserID">用户ID</param>
        /// <param name="msg"></param>
        /// <returns>返回Sql字符串</returns>
        //public static string GetSqlRightStr(Entities.EnumResourceType resourceType, string tablenameUserID, string UserIDFileName, int UserID, out string msg, int pubID = 0)
        //{
        //    return BLL.SysRight.UserRole.Instance.GetSqlRightStr(resourceType, tablenameUserID, UserIDFileName, UserID, pubID, out msg);
        //}

        #region ls

        public static string CreateRightSql(CreateSqlDependOnEnum depend, UserRole ur, string userIDColumnName = "CreateUserID")
        {
            string sql = string.Empty;
            if (depend == CreateSqlDependOnEnum.系统)
            {
                sql = " and 1 = 1";
            }
            else
            {
                if (depend == CreateSqlDependOnEnum.角色)
                {
                    sql = string.Format(" and {0} in (select UserID from UserRole where RoleID = '{1}')", userIDColumnName, ur.Roles);
                }
                else
                {
                    sql = string.Format(" and {0} = {1}", userIDColumnName, ur.UserID);
                }
            }
            return sql;
        }

        public enum CreateSqlDependOnEnum
        {
            用户,
            角色,
            系统
        }

        #endregion ls

        #region Json

        /// 表转Json
        /// <summary>
        /// 表转Json
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="keycol"></param>
        /// <param name="namecol"></param>
        /// <param name="keyname"></param>
        /// <param name="namename"></param>
        /// <returns></returns>
        public static string DataTableToJson(DataTable dt, string keycol, string namecol, string keyname = "id", string namename = "name")
        {
            string str = "[";
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    str += "{" + keyname + ": " + Newtonsoft.Json.JsonConvert.ToString(dr[keycol].ToString()) + ", "
                        + namename + ": " + Newtonsoft.Json.JsonConvert.ToString(dr[namecol].ToString()) + "},";
                }
            }
            str = str.TrimEnd(',');
            str += "]";
            return str;
        }

        /// dic转Json
        /// <summary>
        /// dic转Json
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string DictionaryToJson(Dictionary<string, string> dic)
        {
            string str = "{";
            if (dic != null)
            {
                foreach (string key in dic.Keys)
                {
                    //字符串类型需要手动添加单双引号
                    //内容需要手动转义
                    str += key + ":" + dic[key] + ",";
                }
            }
            str = str.TrimEnd(',');
            str += "}";
            return str;
        }

        #endregion Json

        /// <summary>
        /// 根据文本内容，填写指定图片中的指定位置
        /// </summary>
        /// <param name="text">文本内容</param>
        /// <param name="fileOriginPath">图片模板位置物理路径</param>
        /// <param name="fileGenPath">图片生成后的物理路径</param>
        /// <returns></returns>
        public static void DrawStringByImage(string text, string fileOriginPath, string fileGenPath)
        {
            //string ModifyImagePath = string.Empty;
            Font font = new Font("宋体", 20);
            //路径
            string fontPath = System.AppDomain.CurrentDomain.BaseDirectory + "Fonts/SourceHanSansCN-Medium.otf";
            if (File.Exists(fontPath))
            {
                //读取字体文件
                PrivateFontCollection pfc = new PrivateFontCollection();
                pfc.AddFontFile(fontPath);
                //实例化字体
                font = new Font(pfc.Families[0], 20);
            }

            Image img = null;
            Graphics g = null;
            try
            {
                //string path = System.AppDomain.CurrentDomain.BaseDirectory + "Upload/test.png";
                img = Image.FromFile(fileOriginPath);
                Bitmap bitmap = new Bitmap(img, img.Width, img.Height);
                g = Graphics.FromImage(bitmap);
                //String str = "腾势热销中的促销优惠高达万元的\n腾势热销中 促销优惠高达11万元";
                //Font font = new Font("宋体", 14);
                SolidBrush sbrush = new SolidBrush(ColorTranslator.FromHtml("#5a5a5a"));
                //SolidBrush sbrush = new SolidBrush(Color.Black);
                //声明矩形域
                RectangleF textArea = new RectangleF(130, 85, 430, 110);
                g.DrawString(text, font, sbrush, textArea);
                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] imgData = ms.ToArray();
                ms.Dispose();
                g.Dispose();

                //ModifyImagePath = System.AppDomain.CurrentDomain.BaseDirectory + "Upload/test_m.png";
                FileStream fs = null;
                string genPath = Path.GetDirectoryName(fileGenPath);
                if (!Directory.Exists(genPath))
                {
                    Directory.CreateDirectory(genPath);
                }
                if (File.Exists(fileGenPath))
                {
                    File.Delete(fileGenPath);
                }

                fs = new FileStream(fileGenPath, FileMode.Create, FileAccess.Write);
                if (fs != null)
                {
                    fs.Write(imgData, 0, imgData.Length);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("在图片上绘制文本出错，DrawStringByImage方法", ex);
            }
            finally
            {
                try
                {
                    img.Dispose();
                    g.Dispose();
                }
                catch
                {
                }
            }
        }

        public static T HttpWebRequestCreate<T>(string httpUrl)
        {
            try
            {
                var request = WebRequest.Create(httpUrl) as HttpWebRequest;
                var response = request.GetResponse() as HttpWebResponse;
                string str = string.Empty;
                using (var stream = response.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                    str = sr.ReadToEnd();
                }
                response.Close();
                request.Abort();
                var obj = JsonConvert.DeserializeObject<T>(str);
                return obj;
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("抓取错误", ex);
                return Activator.CreateInstance<T>();
            }
        }

        /// <summary>
        /// 根据长URL，调用新浪接口，转换为短URL
        /// </summary>
        /// <param name="url">长URL</param>
        /// <returns>返回短URL，若出错，则返回字符串空</returns>
        public static string ConvertSINAShortUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return "";
            }
            try
            {
                //api地址
                var address = "http://api.t.sina.com.cn/short_url/shorten.json?source=2815391962";
                address += "&url_long=" + HttpUtility.UrlEncode(url);
                //http请求
                var json = HttpGet(address, string.Empty);

                //json转换
                JArray urls = JArray.Parse(json);
                //List<Result> urls = JsonConvert.DeserializeObject<List<Result>>(json);

                if (urls != null && urls.Count > 0)
                {
                    return ((JObject)urls[0]).GetValue("url_short").ToObject<String>();
                }
                return "";
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("根据长url转换短url方法报错，长url为：" + url, ex);
                return "";
            }
        }

        #region 图片URL本地化相关方法

        /// <summary>
        /// 根据URL，清洗图片，改成本地化url
        /// </summary>
        /// <param name="url">图片URL地址</param>
        /// <returns>返回本地化url</returns>
        public static string CleanImg(string url)
        {
            string imageUrl = string.Empty;
            if (!string.IsNullOrWhiteSpace(url))
            {
                Image image = GetImageByURL(url);
                if (image != null)
                {
                    imageUrl = CleanImg(image);
                    if (!string.IsNullOrWhiteSpace(imageUrl))
                    {
                        imageUrl = CleanImgURLPrefix + imageUrl;
                    }
                }
            }
            return imageUrl;
        }

        /// <summary>
        /// 根据图片URL，获取的Image对象
        /// </summary>
        /// <param name="imgUrl">图片URL</param>
        /// <returns>返回Image对象</returns>
        public static Image GetImageByURL(string imgUrl)
        {
            try
            {
                System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(imgUrl);
                System.Net.HttpWebResponse res = (System.Net.HttpWebResponse)req.GetResponse();
                Image img = Image.FromStream(res.GetResponseStream());
                res.Dispose();
                return img;
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("GetImageByURL-Error", ex);
                return null;
            }
        }

        /// <summary>
        /// 清洗图片（替换url，改为自己的连接）
        /// </summary>
        /// <param name="img">img对象</param>
        private static string CleanImg(Image img)
        {
            string imageUrl = string.Empty;
            string extName = string.Empty;
            ImageFormat imageFormat = GetImageExtension(img, out extName);
            if (!string.IsNullOrWhiteSpace(extName))
            {
                IFastDFSAdapter _iFastDFSAdapter = new StreamFastDFSAdapter(ImageToByteArray(img, imageFormat), extName.TrimStart('.'));
                imageUrl = _iFastDFSAdapter.Upload();
            }
            return imageUrl;
        }

        private static byte[] ImageToByteArray(Image image, ImageFormat imageFormat)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, imageFormat);
            return ms.ToArray();
        }

        private static ImageFormat GetImageExtension(Image image, out string extName)
        {
            extName = string.Empty;
            if (ImageFormats != null && ImageFormats.Count > 0)
            {
                foreach (var pair in ImageFormats)
                {
                    if (pair.Value.Guid == image.RawFormat.Guid)
                    {
                        extName = pair.Key.ToString().Trim();
                        return pair.Value;
                    }
                }
            }
            return null;
        }

        private static Dictionary<String, ImageFormat> GetImageFormats()
        {
            var dic = new Dictionary<String, ImageFormat>();
            var properties = typeof(ImageFormat).GetProperties(BindingFlags.Static | BindingFlags.Public);
            foreach (var property in properties)
            {
                var format = property.GetValue(null, null) as ImageFormat;
                if (format == null) continue;
                dic.Add(("." + (property.Name.ToLower() == "jpeg" ? "jpg" : property.Name.ToLower())).ToLower(), format);
            }
            return dic;
        }

        #endregion 图片URL本地化相关方法
    }
}