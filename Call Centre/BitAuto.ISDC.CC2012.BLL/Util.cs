using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using BitAuto.ISDC.CC2012.Entities;
using System.Reflection;
using BitAuto.Utils;
using System.IO;
using System.Threading;
using System.Web;
using System.Collections;
using System.Data;
using System.Configuration;
using BitAuto.Services.Organization.Remoting;
using System.Xml;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Security.Cryptography;
using BitAuto.Utils.Config;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;


namespace BitAuto.ISDC.CC2012.BLL
{
    public class Util
    {
        /// <summary>
        /// 集中权限系统登录Cookie名称
        /// </summary>
        public const string SysLoginCookieName = "xxc-3244xx32323sdf";

        /// <summary>
        /// 根据枚举的值，得到枚举对应的文本
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumOptText(int value)
        {
            Type type;
            if (value >= 180000 && value < 190000)
            {
                type = typeof(EnumProjectTaskStatus);
                return GetEnumOptText(type, value);
            }
            return "";
        }

        //add by qizq 2012-8-2
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
                        //强斐 2016-7-26 
                        //新增一个null类型，如果是null类型，界面不呈现
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

        /// 获取特殊枚举的多选值代表的含义（枚举序号为1,2,4,8,16...）
        /// <summary>
        /// 获取特殊枚举的多选值代表的含义（枚举序号为1,2,4,8,16...）
        /// </summary>
        /// <param name="businesstype"></param>
        /// <param name="enumtype"></param>
        /// <returns></returns>
        public static string GetMutilEnumDataNames(int businesstype, Type enumtype)
        {
            string name = "";
            DataTable dt = GetEnumDataTable(enumtype);
            foreach (DataRow dr in dt.Rows)
            {
                int value = CommonFunction.ObjectToInteger(dr["value"]);
                //按位与
                if ((businesstype & value) == value)
                {
                    name += dr["name"].ToString() + ",";
                }
            }
            return name.TrimEnd(',');
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

        public static string GetCaseWhenByEnum(Type enumName)
        {
            string sql = "";
            foreach (int v in Enum.GetValues(enumName))
            {
                string name = Util.GetEnumOptText(enumName, v);
                sql += "when " + v + " then '" + name + "' ";
            }
            return sql;
        }

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
                string path = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LogWebAbsolutePath", false).TrimEnd('\\');
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
            string ModuleLogOut = CommonFunction.ObjectToString(BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ModuleLogOut", false));
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

        public static int GetCurrentRequestQueryInt(string r)
        {
            int val = -1;
            try
            {
                if (System.Web.HttpContext.Current.Request.QueryString[r] != null)
                {
                    val = CommonFunction.ObjectToInteger(System.Web.HttpContext.Current.Request.QueryString[r].ToString().Trim());
                }
            }
            catch (Exception e)
            {
                return val;
            }
            return val;
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
                    string cookieValue = BitAuto.Utils.Security.DESEncryptor.Decrypt(webHttp.Request.Cookies["ncc-uinfo"].Value, key);
                    string[] strCookieArr = cookieValue.Split('|');
                    switch (sessionName)
                    {
                        case "Login_RoleName": webHttp.Session[sessionName] = BitAuto.Utils.Security.DESEncryptor.Decrypt(webHttp.Server.UrlDecode(strCookieArr[1]), key);
                            break;
                        case "Login_BGID": webHttp.Session[sessionName] = BitAuto.Utils.Security.DESEncryptor.Decrypt(strCookieArr[2], key);
                            break;
                        case "Login_BGName": webHttp.Session[sessionName] = BitAuto.Utils.Security.DESEncryptor.Decrypt(webHttp.Server.UrlDecode(strCookieArr[3]), key);
                            break;
                        case "Login_OutCallNum": webHttp.Session[sessionName] = BitAuto.Utils.Security.DESEncryptor.Decrypt(strCookieArr[4], key);
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

        public static string GetLoginUserNumber()
        {
            return BLL.Util.GetSessionValue("EmployeeNumber");//从session取
        }

        /// <summary>
        /// 得到当前登录用户域账号
        /// </summary>
        /// <returns></returns>
        public static string GetLoginUserName()
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

            string userName = string.Empty;
            System.Web.HttpContext webHttp = System.Web.HttpContext.Current;
            if (webHttp != null && webHttp.Session["username"] != null && webHttp.Session["username"].ToString() != "")
            {
                userName = webHttp.Session["username"].ToString().Trim();
                bool success = string.IsNullOrEmpty(userName) ? false : true;
                if (success == false) { throw new Exception(string.Format("session中的['UserName']为空:{0}", userName)); }
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
                        return GetLoginUserName();
                    }
                }
            }
            return userName;
        }

        /// <summary>
        /// 获取DepartmentID
        /// </summary>
        /// <returns></returns>
        public static string GetDepartmentID()
        {
            try
            {
                bool isLogin = BitAuto.YanFa.SysRightManager.Common.UserInfo.IsLogin();
                if (isLogin == false) { throw new Exception("还没有登录"); }
            }
            catch (Exception)
            {
                throw new Exception("还没有登录");
            }

            HttpContext webHttp = HttpContext.Current;
            if (webHttp.Session["departid"] != null && webHttp.Session["departid"].ToString() != "")
            {
                string s = webHttp.Session["departid"].ToString().Trim();
                return s;
            }
            else
            {
                throw new Exception("无法读取相关session");
            }
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

        /// <summary>
        /// 从枚举中取得数据，以DataTable形式返回
        /// </summary>
        /// <param name="enumName"></param>
        /// <returns></returns>
        public static DataTable GetDataFromEnum(Type enumName)
        {
            DataTable dt = new DataTable();
            dt.TableName = "Table";
            dt.Columns.Add("ID");
            dt.Columns.Add("Name");
            foreach (int v in Enum.GetValues(enumName))
            {
                DataRow dr = dt.Rows.Add();
                dr[0] = v;
                dr[1] = BLL.Util.GetEnumOptText(enumName, v);
            }
            return dt;
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
        /// 插入日志记录
        /// </summary>
        /// <param name="loginInfo">日志描述</param>
        /// <param name="userType">用户类型（1.权限系统用户，2.HR系统用户）</param>
        /// <returns>返回主键</returns>
        public static int InsertUserLog(string loginInfo, int userID)
        {
            Entities.UserActionLog model = new Entities.UserActionLog();
            model.UserID = userID;
            model.TrueName = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(userID);
            model.Loginfo = loginInfo;
            model.IP = GetIP();
            model.CreateTime = DateTime.Now;

            return BLL.UserActionLog.Instance.Insert(model);
        }

        /// <summary>
        /// 插入日志记录
        /// </summary>
        /// <param name="loginInfo">日志描述</param>
        /// <returns>返回主键</returns>
        public static int InsertUserLog(string loginInfo)
        {
            Entities.UserActionLog model = new Entities.UserActionLog();
            model.UserEID = GetLoginUserID();
            model.TrueName = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(model.UserEID);
            model.Loginfo = loginInfo;
            model.IP = GetIP();
            model.CreateTime = DateTime.Now;

            return BLL.UserActionLog.Instance.Insert(model);
        }

        /// <summary>
        /// 插入日志记录
        /// </summary>
        /// <param name="loginInfo">日志描述</param>
        /// <returns>返回主键</returns>
        public static int InsertUserLogNoUser(string loginInfo)
        {
            Entities.UserActionLog model = new Entities.UserActionLog();
            model.UserEID = Entities.Constants.Constant.INT_INVALID_VALUE; //GetLoginUserID();
            model.TrueName = string.Empty;// BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(model.UserEID);
            model.Loginfo = loginInfo;
            model.IP = GetIP();
            model.CreateTime = DateTime.Now;

            return BLL.UserActionLog.Instance.Insert(model);
        }


        /// <summary>
        /// 插入日志记录(事务)
        /// </summary>
        /// <param name="sqltran">事务</param>
        /// <param name="loginInfo">日志描述</param>
        /// <returns></returns>
        public static int InsertUserLog(SqlTransaction sqltran, string loginInfo)
        {
            Entities.UserActionLog model = new Entities.UserActionLog();
            model.UserEID = GetLoginUserID();
            model.TrueName = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(model.UserEID);
            model.Loginfo = loginInfo;
            model.IP = GetIP();
            model.CreateTime = DateTime.Now;

            return BLL.UserActionLog.Instance.Insert(sqltran, model);
        }

        /// <summary>
        /// 插入日志记录(事务)
        /// </summary>
        /// <param name="sqltran">事务</param>
        /// <param name="loginInfo">日志描述</param>
        /// <returns></returns>
        public static int InsertUserLogNoUser(SqlTransaction sqltran, string loginInfo)
        {
            Entities.UserActionLog model = new Entities.UserActionLog();
            model.UserEID = Entities.Constants.Constant.INT_INVALID_VALUE;// GetLoginUserID();
            model.TrueName = string.Empty;//BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(model.UserEID);
            model.Loginfo = loginInfo;
            model.IP = GetIP();
            model.CreateTime = DateTime.Now;

            return BLL.UserActionLog.Instance.Insert(sqltran, model);
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

        /// <summary>
        /// 通过权限系统EID找到用户域名
        /// </summary>
        /// <param name="limitEID">权限系统EID</param>
        /// <returns>域名</returns>
        public static string GetDomainAccountByLimitEID(int limitEID)
        {
            //string DomainAccount = string.Empty;
            IOrganizationService service = (IOrganizationService)Activator.GetObject(typeof(IOrganizationService),
                                                         ConfigurationManager.AppSettings["OrganizationService"]);
            string adname = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerDomainAccountByUserID(limitEID);
            //DomainAccount = service.QueryEmployeeByDomainAccount(adname)[0].DomainAccount;
            return adname;
        }

        /// <summary>
        /// 通过权限系统EID找到用户在HR系统中的Name
        /// </summary>
        /// <param name="limitEID">权限系统EID</param>
        /// <returns>HR系统中的Name</returns>
        public static string GetNameInHRLimitEID(int limitEID)
        {
            return BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(limitEID);
        }//zxq 2012.09.07

        /// <summary>
        /// 验证用户是否具有某个功能点或者模块的权限
        /// </summary>
        /// <param name="userid">权限系统用户ID</param>
        /// <param name="moduleID">模块或功能点ID</param>
        /// <returns></returns>
        public static bool CheckRight(int userid, string moduleID)
        {
            return BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.CheckRight(moduleID, ConfigurationManager.AppSettings["ThisSysID"].ToString(), userid);
        }
        /// <summary>
        /// 验证用户是否具有某个功能点或者模块的权限
        /// </summary>
        /// <param name="userid">权限系统用户ID</param>
        /// <param name="moduleID">模块或功能点ID</param>
        /// <returns></returns>
        public static bool CheckRightForCRM(int userid, string moduleID)
        {
            return BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.CheckRight(moduleID, ConfigurationManager.AppSettings["CRMSysID"].ToString(), userid);
        }
        /// <summary>
        /// 验证用户是否具有某个功能点或者模块的权限
        /// </summary>
        /// <param name="userid">权限系统用户ID</param>
        /// <param name="moduleID">模块或功能点ID</param>
        /// <returns></returns>
        public static bool CheckRightForIM(int userid, string moduleID)
        {
            return BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.CheckRight(moduleID, ConfigurationManager.AppSettings["IMSysID"].ToString(), userid);
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
        /// 获取汉字的全拼,如汉字“学生”，转化为“xuesheng”
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertToPinYinAll(string str)
        {
            string PYstr = "";
            foreach (char item in str.ToCharArray())
            {
                if (Microsoft.International.Converters.PinYinConverter.ChineseChar.IsValidChar(item))
                {
                    Microsoft.International.Converters.PinYinConverter.ChineseChar cc = new Microsoft.International.Converters.PinYinConverter.ChineseChar(item);

                    //取最后一个
                    for (int i = cc.Pinyins.Count - 1; i >= 0; i--)
                    {
                        if (cc.Pinyins[i] != null)
                        {
                            PYstr += cc.Pinyins[i].Substring(0, cc.Pinyins[i].Length - 1).ToLower();
                        }
                    }
                }
                else
                {
                    PYstr += item.ToString();
                }
            }
            return PYstr;
        }

        /// <summary>
        /// 获取汉字的拼音缩写,如汉字“学生”，转化为“xues”
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertToPinY(string str)
        {
            string PYstr = "";
            char[] charArray = str.ToCharArray();
            for (int i = 0; i < charArray.Length; i++)
            {
                if (Microsoft.International.Converters.PinYinConverter.ChineseChar.IsValidChar(charArray[i]))
                {
                    Microsoft.International.Converters.PinYinConverter.ChineseChar cc = new Microsoft.International.Converters.PinYinConverter.ChineseChar(charArray[i]);
                    if (i != 0)
                    {
                        PYstr += cc.Pinyins[cc.Pinyins.Count - 1].Substring(0, 1).ToLower();
                    }
                    else
                    {
                        PYstr += cc.Pinyins[cc.Pinyins.Count - 1].Substring(0, cc.Pinyins[0].Length - 1).ToLower();
                    }
                }
                else
                {
                    PYstr += charArray[i].ToString();
                }
            }
            return PYstr;
        }


        /// <summary>
        /// 获取汉字的全拼,如汉字“学生”，转化为“xuesheng”
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static ArrayList ConvertToPinYinAllByPolyphone(string str)
        {
            ArrayList PYstr = new ArrayList();
            foreach (char item in str.ToCharArray())
            {
                if (Microsoft.International.Converters.PinYinConverter.ChineseChar.IsValidChar(item))
                {
                    Microsoft.International.Converters.PinYinConverter.ChineseChar cc = new Microsoft.International.Converters.PinYinConverter.ChineseChar(item);
                    if (cc.IsPolyphone)
                    {
                        ArrayList polyphone = new ArrayList();
                        Hashtable ht = new Hashtable();
                        for (int i = 0; i < cc.PinyinCount; i++)
                        {
                            string t = cc.Pinyins[i].Substring(0, cc.Pinyins[i].Length - 1).ToLower();
                            if (!ht.ContainsKey(t))
                            {
                                polyphone.Add(t);
                                ht.Add(t, t);
                            }
                        }
                        if (polyphone.Count == 1)
                        {
                            PYstr.Add(polyphone[0]);
                        }
                        else
                        {
                            PYstr.Add(polyphone);
                        }
                    }
                    else
                    {
                        PYstr.Add(cc.Pinyins[0].Substring(0, cc.Pinyins[0].Length - 1).ToLower());
                    }
                }
                else
                {
                    PYstr.Add(item.ToString());
                }
            }
            return PYstr;
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
        #endregion


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


        #endregion

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


        /// <summary>
        /// 根据客户经营范围ID，返回客户经营范围名称(项目管理->crm客户信息 add--lxw)
        /// </summary>
        /// <param name="carType"></param>
        /// <returns></returns>
        public static string GetCustCatTypeName(int carType)
        {
            string temp = string.Empty;
            //经营范围
            switch (carType)
            {
                case 3:
                    temp = "新车、二手车";
                    break;
                case 2:
                    temp = "二手车";
                    break;
                case 1:
                    temp = "新车";
                    break;
                default:
                    break;
            }
            return temp;
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

        #endregion


        /// <summary>
        /// 发送报错邮件
        /// </summary>
        /// <param name="errorMsg">报错信息</param>
        /// <param name="source">导致错误的应用程序或对象的名称</param>
        /// <param name="stackTrace">异常发生时调用堆栈上的帧的字符串表示形式</param>
        public static void SendErrorEmail(string errorMsg, string source, string stackTrace)
        {
            string trueName = BLL.Util.GetLoginRealName();
            string mailBody = string.Format("当前登陆者：{0} <br/>当前页面：{1}<br/>错位信息：{2}<br/>错误Source：{3}<br/>错误StackTrace：{4}<br/>内部错位信息：{5}<br/>内部错误Source：{6}<br/>内部错误StackTrace：{7}",
               trueName, BLL.Util.GetUrl(), errorMsg, source, stackTrace,
                                                 errorMsg, source, stackTrace);
            string subject = "新呼叫中心系统——报错通知";
            //string[] userEmail = ConfigurationManager.AppSettings["ReceiveErrorEmail"].Split(';');
            string[] userEmail = ConfigurationUtil.GetAppSettingValue("ReceiveErrorEmail").Split(';');
            if (userEmail != null && userEmail.Length > 0)
            {
                BLL.EmailHelper.Instance.SendErrorMail(mailBody, subject, userEmail);
            }
        }


        //发邮件影响呼入弹屏速度，改成异步发送
        //声明委托
        public delegate void AsyncEventHandler(string errorMsg, string source, string stackTrace);
        public static void SendEmailAsync(string errorMsg, string source, string stackTrace)
        {
            //实例委托
            AsyncEventHandler asy = new AsyncEventHandler(BLL.Util.SendErrorEmail);
            //异步调用开始
            IAsyncResult ia = asy.BeginInvoke(errorMsg, source, stackTrace, null, null);
            //异步结束
            asy.EndInvoke(ia);
        }

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


            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC"), CommandType.Text, where, null);
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
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC"), CommandType.Text, where, null);
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

            ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC"), CommandType.Text, where, null);
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

            ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC"), CommandType.Text, sql, null);
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
                            BLL.Loger.Log4Net.Error(ex.Message, ex);
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
                        BLL.Loger.Log4Net.Error(ex.Message, ex);
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
                    BLL.Loger.Log4Net.Info("转换model实体类（" + typeof(T).Name + "）属性（" + property.Name + "）传值（" + (value1 == null ? "null" : value1.ToLower()) + "），忽略");
                }
            }
            return model;
        }


        #region NPOI导出Excel
        private void InitializeWorkbook(HSSFWorkbook hssfworkbook, string headerText)
        {
            hssfworkbook = new HSSFWorkbook();

            //创建一个文档摘要信息实体。
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "Weilog Team"; //公司名称
            hssfworkbook.DocumentSummaryInformation = dsi;

            //创建一个摘要信息实体。
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "Weilog 系统生成";
            si.Author = "Weilog 系统";
            si.Title = headerText;
            si.Subject = headerText;
            si.CreateDateTime = DateTime.Now;
            hssfworkbook.SummaryInformation = si;

        }

        private static MemoryStream WriteToStream(HSSFWorkbook hssfworkbook)
        {
            //Write the stream data of workbook to the root directory
            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);
            return file;
        }
        //Export(DataTable table, string headerText, string sheetName, string[] columnName, string[] columnTitle)
        /// <summary>
        /// 向客户端输出文件。
        /// </summary>
        /// <param name="table">数据表。</param>
        /// <param name="headerText">头部文本。</param>
        /// <param name="sheetName"></param>
        /// <param name="columnName">数据列名称。</param>
        /// <param name="columnTitle">表标题。</param>
        /// <param name="fileName">文件名称。</param>
        public static void Write(DataTable table, string headerText, string sheetName, string[] columnName, string[] columnTitle, string fileName)
        {
            HttpContext context = HttpContext.Current;
            context.Response.ContentType = "application/vnd.ms-excel";
            context.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", HttpUtility.UrlEncode(fileName, Encoding.UTF8)));
            context.Response.Clear();
            HSSFWorkbook hssfworkbook = GenerateData(table, headerText, sheetName, columnName, columnTitle);
            context.Response.BinaryWrite(WriteToStream(hssfworkbook).GetBuffer());
            context.Response.End();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="headerText"></param>
        /// <param name="sheetName"></param>
        /// <param name="columnName"></param>
        /// <param name="columnTitle"></param>
        /// <returns></returns>
        public static HSSFWorkbook GenerateData(DataTable table, string headerText, string sheetName, string[] columnName, string[] columnTitle)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            ISheet sheet = hssfworkbook.CreateSheet(sheetName);

            #region 设置文件属性信息

            //创建一个文档摘要信息实体。
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "Weilog Team"; //公司名称
            hssfworkbook.DocumentSummaryInformation = dsi;

            //创建一个摘要信息实体。
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "本文档由 Weilog 系统生成";
            si.Author = " Weilog 系统";
            si.Title = headerText;
            si.Subject = headerText;
            si.CreateDateTime = DateTime.Now;
            hssfworkbook.SummaryInformation = si;

            #endregion

            ICellStyle dateStyle = hssfworkbook.CreateCellStyle();
            IDataFormat format = hssfworkbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            #region 取得列宽

            int[] colWidth = new int[columnName.Length];
            for (int i = 0; i < columnName.Length; i++)
            {
                colWidth[i] = Encoding.GetEncoding(936).GetBytes(columnTitle[i]).Length;
            }
            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int j = 0; j < columnName.Length; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(table.Rows[i][columnName[j]].ToString()).Length;
                    if (intTemp > colWidth[j])
                    {
                        colWidth[j] = intTemp;
                    }
                }
            }

            #endregion

            int rowIndex = 0;
            foreach (DataRow row in table.Rows)
            {
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = hssfworkbook.CreateSheet(sheetName + ((int)rowIndex / 65535).ToString());
                    }

                    #region 表头及样式
                    {
                        IRow headerRow = sheet.CreateRow(0);
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(headerText);

                        ICellStyle headStyle = hssfworkbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.CENTER;
                        IFont font = hssfworkbook.CreateFont();
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);

                        headerRow.GetCell(0).CellStyle = headStyle;
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, table.Columns.Count - 1));
                    }
                    #endregion

                    #region 列头及样式
                    {
                        IRow headerRow;
                        headerRow = sheet.CreateRow(1);
                        ICellStyle headStyle = hssfworkbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.CENTER;
                        IFont font = hssfworkbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);

                        for (int i = 0; i < columnName.Length; i++)
                        {
                            headerRow.CreateCell(i).SetCellValue(columnTitle[i]);
                            headerRow.GetCell(i).CellStyle = headStyle;
                            //设置列宽 
                            if ((colWidth[i] + 1) * 256 > 30000)
                            {
                                sheet.SetColumnWidth(i, 10000);
                            }
                            else
                            {
                                sheet.SetColumnWidth(i, (colWidth[i] + 1) * 256);
                            }
                        }
                        /* 
                        foreach (DataColumn column in dtSource.Columns) 
                        { 
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName); 
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle; 
   
                            //设置列宽    
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256); 
                        } 
                         * */
                    }
                    #endregion
                    //if (!string.IsNullOrEmpty(headerText))
                    //{
                    //    rowIndex = 1;
                    //}
                    //else
                    //{
                    rowIndex = 2;
                    //}

                }
                #endregion

                #region 填充数据

                IRow dataRow = sheet.CreateRow(rowIndex);
                for (int i = 0; i < columnName.Length; i++)
                {
                    ICell newCell = dataRow.CreateCell(i);

                    string drValue = row[columnName[i]].ToString();

                    switch (table.Columns[columnName[i]].DataType.ToString())
                    {
                        case "System.String"://字符串类型   
                            if (drValue.ToUpper() == "TRUE")
                                newCell.SetCellValue("是");
                            else if (drValue.ToUpper() == "FALSE")
                                newCell.SetCellValue("否");
                            newCell.SetCellValue(drValue);
                            break;
                        case "System.DateTime"://日期类型    
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle;//格式化显示    
                            break;
                        case "System.Boolean"://布尔型    
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            if (boolV)
                                newCell.SetCellValue("是");
                            else
                                newCell.SetCellValue("否");
                            break;
                        case "System.Int16"://整型    
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal"://浮点型    
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull"://空值处理    
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }

                }

                #endregion

                rowIndex++;
            }

            return hssfworkbook;
        }
        #endregion

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

        /// <summary>
        /// 根据sessionID取青牛录音地址
        /// </summary>
        /// <param name="sessionId"></param>
        public static string GetRecordURL(string sessionId)
        {
            string URL = "http://60.10.131.130/cca/record/RecordFileAction.do";
            string QNURL = ConfigurationUtil.GetAppSettingValue("QNURL");
            if (!string.IsNullOrEmpty(QNURL))
            {
                URL = QNURL;
            }
            string returnStr = string.Empty;
            try
            {

                URL = URL + "?reqCode=queryBySessionID&entID=YCW&sessionId=" + sessionId;
                System.Net.HttpWebRequest wr = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(URL);
                wr.Method = "get";
                using (System.Net.WebResponse ws = wr.GetResponse())
                {
                    StreamReader sr = new StreamReader(ws.GetResponseStream());
                    string html = sr.ReadToEnd();
                    string[] array = html.Split('"');
                    returnStr = array[3];
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("调用青牛取录音地址服务出现异常,SessionID为" + sessionId + "，异常信息：" + ex.Message);
            }
            return returnStr;
        }

        public static DateTime GetDBDateTime()
        {
            return BitAuto.ISDC.CC2012.Dal.Util.GetDBDateTime();
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
        /// 
        /// </summary>
        /// <param name="bgid"></param>
        /// <param name="scid"></param>
        /// <param name="url"></param>
        /// <param name="businessID"></param>
        /// <param name="ypFanXianHBuyCarURL"></param>
        /// <param name="epEmbedCCHBuyCar_APPID"></param>
        /// <returns></returns>
        public static string GenBusinessURLByBGIDAndSCID(string bgid, string scid, string url, string businessID, string ypFanXianHBuyCarURL, string epEmbedCCHBuyCar_APPID, bool isView = false)
        {
            if (string.IsNullOrEmpty(businessID))
            {
                return "";
            }
            string viewConent = isView ? "查看" : businessID;
            if (BLL.Util.IsMatchBGIDAndSCID(bgid, scid, "EPEmbedCC_HMCBGIDSCID"))//惠买车
            {
                return "<a href=\"javascript:void(0)\" urlstr='" + url + "' onclick=\"GoToEpURL(this,'" + ypFanXianHBuyCarURL + "','" + epEmbedCCHBuyCar_APPID + "')\" >" + viewConent + "</a>";
            }
            else if (BLL.Util.IsMatchBGIDAndSCID(bgid, scid, "EasySetOff_BGIDSCID"))//精准广告
            {
                return "<a href=\"javascript:void(0)\" urlstr='" + url + "' onclick=\"var obj = new Object();obj.businessType = 'jingzhunguanggao';obj.GoToEPURL='" + url + "';OtherBusinessLogin(obj);\" >" + viewConent + "</a>";
            }
            else if (BLL.Util.IsMatchBGIDAndSCID(bgid, scid, "CarFinancial_BGIDSCID"))//易车车贷
            {
                return "<a  href=\"javascript:void(0)\" onclick=\"var obj = new Object();obj.businessType = 'yichechedai';obj.callbackurl='" + url + "';OtherBusinessLogin(obj);\">" + viewConent + "</a>";
            }
            else if (BLL.Util.IsMatchBGIDAndSCID(bgid, scid, "YICHEHUI_BGIDSCID"))//易车惠
            {
                return "<a  href=\"javascript:void(0)\" onclick=\"var obj = new Object();obj.businessType = 'yichehui';obj.callbackurl='" + url + "';OtherBusinessLogin(obj);\">" + viewConent + "</a>";
            }
            else if (BLL.Util.IsMatchBGIDAndSCID(bgid, scid, "EasyPass_BGIDSCID"))//易湃业务
            {
                return "<a  href=\"javascript:void(0)\" onclick=\"var obj = new Object();obj.businessType = 'EasyPass';obj.businessID = '" + businessID + "';OtherBusinessLogin(obj);\">" + viewConent + "</a>";
            }
            //其他业务
            else
            {
                return "<a href='" + url + "' target=\"_blank\">" + viewConent + "</a>";
            }
        }

        /// <summary>
        /// 根据web.config中的配置，与参数分组、分类比较
        /// </summary>
        /// <param name="bgid">分组ID</param>
        /// <param name="scid">分类ID</param>
        /// <param name="configName">web.config中的配置节点</param>
        /// <returns>能匹配上，返回True，否则返回False</returns>
        public static bool IsMatchBGIDAndSCID(string bgid, string scid, string configName)
        {
            string bgidscids = ConfigurationUtil.GetAppSettingValue(configName);
            string[] array = bgidscids.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string item in array)
            {
                string[] bsarray = item.Split(',');

                if ((bsarray[0] == bgid && bsarray[1] == scid))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取所属业务分组、分类
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="bgid"></param>
        /// <param name="scid"></param>
        /// <param name="errormsg"></param>
        public static void GetBIGDSCID_UserID(string userid, string bgid, string scid, out string sbgid, out string sscid, out string errormsg)
        {
            BLL.Loger.Log4Net.Info("[GetBIGDSCID_UserID]begin...bgid：" + bgid + ",scid:" + scid + ",userid:" + userid);
            sbgid = "";
            sscid = "";
            errormsg = "";
            try
            {
                //if (isHuiMaiCHE(bgid, scid))
                if (IsMatchBGIDAndSCID(bgid, scid, "EPEmbedCC_HMCBGIDSCID"))
                {
                    BLL.Loger.Log4Net.Info("[GetBIGDSCID_UserID]isHuiMaiCHE：" + bgid + ",scid:" + scid + ",userid:" + userid);
                    Entities.EmployeeAgent model = BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(Convert.ToInt32(userid));

                    if (model == null)
                    {
                        errormsg = "未找到该用户信息!";
                        return;
                    }

                    if (model.BGID == -2)
                    {
                        errormsg = "业务组未分配!";
                        return;
                    }

                    string bgidscids = ConfigurationUtil.GetAppSettingValue("EPEmbedCC_HMCBGIDSCID");
                    string[] array = bgidscids.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string item in array)
                    {
                        string[] bsarray = item.Split(',');

                        if (model.BGID != null && model.BGID.Value.ToString() == bsarray[0])
                        {
                            sbgid = model.BGID.Value.ToString();
                            sscid = bsarray[1];
                            break;
                        }
                    }
                    BLL.Loger.Log4Net.Info("[GetBIGDSCID_UserID]isHuiMaiCHE...sbgid：" + sbgid + ",sscid:" + sscid + ",userid:" + userid);
                }
                else
                {
                    sbgid = bgid;
                    sscid = scid;
                }
                BLL.Loger.Log4Net.Info("[GetBIGDSCID_UserID]finish...sbgid：" + sbgid + ",sscid:" + sscid + ",userid:" + userid);
            }
            catch (Exception ex)
            {
                errormsg = "[HMC_GetBIGDSCID_UserID]出错:" + ex.Message;
            }
        }

        /// 根据省 城市 区县 查找大区数据
        /// <summary>
        /// 根据省 城市 区县 查找大区数据
        /// </summary>
        /// <param name="provinceid"></param>
        /// <param name="cityid"></param>
        /// <param name="countryid"></param>
        /// <returns></returns>
        public static BitAuto.YanFa.Crm2009.Entities.AreaInfo GetAreaInfoByPCC(string provinceid, string cityid, string countryid)
        {
            BitAuto.YanFa.Crm2009.Entities.AreaInfo info = null;
            if (!string.IsNullOrEmpty(countryid))
            {
                info = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaInfo(countryid);
            }
            if (info == null)
            {
                if (!string.IsNullOrEmpty(cityid))
                {
                    info = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaInfo(cityid);
                }
                if (info == null)
                {
                    if (!string.IsNullOrEmpty(provinceid))
                    {
                        info = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaInfo(provinceid);
                    }
                }
            }
            return info;
        }
        /// 获取行列的值
        /// <summary>
        /// 获取行列的值
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static string GetDataRowValue(DataRow dr, string col)
        {
            if (dr != null && dr.Table != null && dr.Table.Columns.Contains(col))
            {
                return CommonFunction.ObjectToString(dr[col]);
            }
            else return "";
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
        #endregion

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
        /// 校验month_ago月以前的分表数据是否存在，查询时间是否可用
        /// <summary>
        /// 校验month_ago月以前的分表数据是否存在，查询时间是否可用
        /// </summary>
        /// <param name="month_ago"></param>
        /// <param name="BeginTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="r"></param>
        /// <param name="info"></param>
        /// <param name="tablenames"></param>
        private static void CheckTableExists(int month_ago, string BeginTime, string EndTime, out bool r, out string info, params string[] tablenames)
        {
            DateTime st = CommonFunction.ObjectToDateTime(BeginTime);
            DateTime et = CommonFunction.ObjectToDateTime(EndTime);
            //当月1号往前推month_ago个月的1号
            DateTime spit = DateTime.Today.AddDays(-DateTime.Today.Day + 1).AddMonths(-month_ago + 1);

            r = false;
            info = "";
            if (st >= spit && et >= spit)
            {
                //month_ago个月以内，查【现在表】
                r = true;
            }
            else if (st < spit && et < spit)
            {
                //month_ago个月以外
                if (st.Month == et.Month && st.Year == et.Year)
                {
                    //同年同月，查【分月表】
                    string houzhui = CalcTableNameByMonth(month_ago, st);
                    for (int i = 0; i < tablenames.Length; i++)
                    {
                        tablenames[i] = tablenames[i] + houzhui;
                    }
                    if (CommonBll.Instance.CheckTableExists(tablenames))
                    {
                        r = true;
                    }
                    else
                    {
                        r = false;
                        info = "当前" + st.Year + "年" + st.Month + "月数据不存在！";
                    }
                }
                else
                {
                    r = false;
                    info = "只能查询最近" + month_ago + "个自然月数据，如需查询历史数据，请按单月查询！";
                }
            }
            else
            {
                r = false;
                info = "只能查询最近" + month_ago + "个自然月数据，如需查询历史数据，请按单月查询！";
            }
        }
        /// 校验话务分月表是否存在
        /// <summary>
        /// 校验话务分月表是否存在
        /// </summary>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool CheckForSelectCallRecordORIG(DateTime st, DateTime et, out string msg)
        {
            msg = "";
            try
            {
                bool r;
                string info;
                if (st < new DateTime(2015, 1, 1) || et < new DateTime(2015, 1, 1))
                {
                    msg = "2015年之前的数据已失效！";
                    return false;
                }
                else
                {
                    CheckTableExists(3, st.ToString(), et.ToString(), out r, out info, "CallRecordInfo", "CallRecord_ORIG", "CallRecord_ORIG_Business", "IVRSatisfaction");
                    msg = info;
                    return r;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
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

        /// <summary>
        /// sql过滤（仅用于In的情况）
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string SqlFilterByInCondition(string source)
        {
            return Dal.Util.SqlFilterByInCondition(source);
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

        public static string GetCallStatus(string CallStatus)
        {
            if (CallStatus == "1")
            {
                return "呼入";
            }
            else if (CallStatus == "2")
            {
                return "呼出";
            }
            else if (CallStatus == "3")
            {
                return "转接";
            }
            else if (CallStatus == "4")
            {
                return "接管";
            }
            else
                return "无";
        }
        /// 根据标识得到是否
        /// <summary>
        /// 根据标识得到是否
        /// </summary>
        /// <param name="isNotStatus"></param>
        /// <returns></returns>
        public static string GetIsNotStatus(string isNotStatus)
        {
            if (isNotStatus == "0")
            {
                return "否";
            }
            else if (isNotStatus == "1")
            {
                return "是";
            }
            else
                return "";
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
                BLL.Loger.Log4Net.Error("读取文件 异常：", ex);
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
                BLL.Loger.Log4Net.Error("写入文件 异常：", ex);
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

        /// <summary>
        /// 根据集中权限UserID，记录当前登录信息至Session
        /// </summary>
        /// <param name="userID">登录用户ID</param>
        /// <param name="sysID">当前系统ID</param>
        public static void LoginPassport(int userID, string sysID)
        {
            string roleName = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetRoleInfo(userID, sysID);
            System.Web.HttpContext.Current.Session["Login_RoleName"] = roleName;
            string BGIDStr = BLL.AgentTimeState.Instance.GetBGNameAndOutNum(userID);
            if (!string.IsNullOrEmpty(BGIDStr))
            {
                string[] strArray = BGIDStr.Split(',');
                System.Web.HttpContext.Current.Session["Login_BGID"] = strArray[0];
                System.Web.HttpContext.Current.Session["Login_BGName"] = strArray[1];
                System.Web.HttpContext.Current.Session["Login_OutCallNum"] = strArray[2];
                string cookieValue = BitAuto.Utils.Security.DESEncryptor.Encrypt(userID.ToString(), key);//userid
                cookieValue += "|" + System.Web.HttpContext.Current.Server.UrlEncode(BitAuto.Utils.Security.DESEncryptor.Encrypt(roleName, key));//Login_RoleName
                cookieValue += "|" + BitAuto.Utils.Security.DESEncryptor.Encrypt(strArray[0], key);//Login_BGID
                cookieValue += "|" + System.Web.HttpContext.Current.Server.UrlEncode(BitAuto.Utils.Security.DESEncryptor.Encrypt(strArray[1], key));//Login_BGName
                cookieValue += "|" + BitAuto.Utils.Security.DESEncryptor.Encrypt(strArray[2], key);//Login_OutCallNum
                System.Web.HttpContext.Current.Response.Cookies["ncc-uinfo"].Value = BitAuto.Utils.Security.DESEncryptor.Encrypt(cookieValue, key);
                //System.Web.HttpContext.Current.Response.Cookies["ncc-info"].Domain =".bitauto.com";
            }
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
            string UploadWebAbsolutePath = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("UploadWebAbsolutePath", false);
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
        #endregion

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

        /// <summary>
        /// category 1是大标题，2是二级标题，3是三级标题
        /// </summary>
        /// <param name="num"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public static string GetNum(int num, string category)
        {
            string str = string.Empty;
            if (category == "1")
            {
                str = ArabicNumbersConvertToChina(num) + "、";
            }
            else if (category == "2")
            {
                str = num + "、";
            }
            else
            {
                str = "(" + num + ")";
            }
            return str;
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
        #endregion

        /// <summary>
        /// 根据集中权限UserID，去集中权限表中，后去员工编号信息
        /// </summary>
        /// <param name="userid">集中权限UserID</param>
        /// <returns>返回员工编号信息</returns>
        public static string GetEmployeeNumberByUserID(int userid)
        {
            return Dal.Util.Instance.GetEmployeeNumberByUserID(userid);
        }
    }
}
